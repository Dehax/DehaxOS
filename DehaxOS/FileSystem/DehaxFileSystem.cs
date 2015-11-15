using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DehaxOS.FileSystem
{
    struct AccessRights
    {
        public struct RightsGroup
        {
            public bool canRead;
            public bool canWrite;
            public bool canExecute;
        }

        public RightsGroup user;
        public RightsGroup group;
        public RightsGroup others;

        public AccessRights(bool ur, bool uw, bool ux, bool gr, bool gw, bool gx, bool or, bool ow, bool ox)
        {
            user.canRead = ur;
            user.canWrite = uw;
            user.canExecute = ux;

            group.canRead = gr;
            group.canWrite = gw;
            group.canExecute = gx;

            others.canRead = or;
            others.canWrite = ow;
            others.canExecute = ox;
        }

        public AccessRights(ushort permissions)
            : this((permissions & (1 << 8)) > 0,
                   (permissions & (1 << 7)) > 0,
                   (permissions & (1 << 6)) > 0,
                   (permissions & (1 << 5)) > 0,
                   (permissions & (1 << 4)) > 0,
                   (permissions & (1 << 3)) > 0,
                   (permissions & (1 << 2)) > 0,
                   (permissions & (1 << 1)) > 0,
                   (permissions & (1 << 0)) > 0)
        {
        }

        public void SetFullAccess(bool user, bool group, bool others)
        {
            if (user)
            {
                this.user.canRead = true;
                this.user.canWrite = true;
                this.user.canExecute = true;
            }

            if (group)
            {
                this.group.canRead = true;
                this.group.canWrite = true;
                this.group.canExecute = true;
            }

            if (others)
            {
                this.others.canRead = true;
                this.others.canWrite = true;
                this.others.canExecute = true;
            }
        }

        public ushort ToUInt16()
        {
            return (ushort)((user.canRead ? 1 : 0 << 8) |
                            (user.canWrite ? 1 : 0 << 7) |
                            (user.canExecute ? 1 : 0 << 6) |
                            (group.canRead ? 1 : 0 << 5) |
                            (group.canWrite ? 1 : 0 << 4) |
                            (group.canExecute ? 1 : 0 << 3) |
                            (others.canRead ? 1 : 0 << 2) |
                            (others.canWrite ? 1 : 0 << 1) |
                            (others.canExecute ? 1 : 0 << 0));
        }
    }

    struct Attributes
    {
        public bool hidden;
        public bool system;
        public bool readOnly;

        public Attributes(byte attributes)
        {
            hidden = (attributes & (1 << 2)) > 0;
            system = (attributes & (1 << 1)) > 0;
            readOnly = (attributes & (1 << 0)) > 0;
        }

        public byte ToByte()
        {
            return (byte)((hidden ? 1 : 0 << 2) |
                          (system ? 1 : 0 << 1) |
                          (readOnly ? 1 : 0 << 0));
        }
    }

    class DehaxFileSystem
    {
        #region Константы
        /// <summary>
        /// Количество байт, занимаемых одним сектором жёсткого диска.
        /// </summary>
        private const ushort DISK_BYTES_PER_SECTOR = 512;
        /// <summary>
        /// Код типа файловой системы.
        /// </summary>
        public const byte DFS_ID = 0x28;
        /// <summary>
        /// Длина поля имени файла в записи каталога.
        /// </summary>
        private const byte FILE_NAME_LENGTH = 15;
        /// <summary>
        /// Длина поля расширения файла в записи каталога.
        /// </summary>
        private const byte FILE_EXTENSION_LENGTH = 5;
        /// <summary>
        /// Значение свободной записи каталога.
        /// </summary>
        private const uint FREE_DIRECTORY_RECORD = 0xFFFFFFFF;
        /// <summary>
        /// Значение свободного индексного дескриптора.
        /// </summary>
        private const byte FREE_INODE_TYPE = 0xFF;
        /// <summary>
        /// Значение типа файла индекного дескриптора.
        /// </summary>
        private const byte FILE_INODE_TYPE = 0x01;
        /// <summary>
        /// Значение типа директории индексного дескриптора.
        /// </summary>
        private const byte DIRECTORY_INODE_TYPE = 0x02;
        /// <summary>
        /// Значение, указывающее на последний кластер данных.
        /// </summary>
        private const uint LAST_CLUSTER_ID = 0xFFFFFFFF;
        /// <summary>
        /// Значение типа файла в поле "Тип файла" индексного дескриптора.
        /// </summary>
        private const byte INODE_FILE_TYPE = 0x01;
        /// <summary>
        /// Значение типа каталога в поле "Тип файла" индексного дескриптора.
        /// </summary>
        private const byte INODE_DIRECTORY_TYPE = 0x02;
        #endregion

        #region Объявление структур
        /// <summary>
        /// Представляет секцию суперблока в начале раздела жёсткого диска.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Superblock
        {
            /// <summary>
            /// Тип файловой системы, для DehaxFS = 0x28.
            /// </summary>
            public byte filesystemType;
            /// <summary>
            /// Количество кластеров (блоков данных).
            /// </summary>
            public uint numClusters;
            /// <summary>
            /// Размер кластера (логического блока), множитель секторов:
            /// • 1 * 512 (размер сектора) = 512 байт;
            /// • 2 * 512 = 1024 байт.
            /// </summary>
            public byte clusterFactor;
            /// <summary>
            /// Размер массива индексных дескрипторов (inode).
            /// </summary>
            public uint inodeArraySize;
            /// <summary>
            /// Размер битовой карты свободных/занятых кластеров.
            /// </summary>
            public uint bitMapSize;
            /// <summary>
            /// Количество свободных кластеров.
            /// </summary>
            public uint numFreeClusters;
            /// <summary>
            /// Количество свободных индексных дескрипторов (inode).
            /// </summary>
            public uint numFreeInode;

            /// <summary>
            /// Адрес секции корневого каталога.
            /// </summary>
            public uint rootAddress;
            /// <summary>
            /// Адрес секции битовой карты.
            /// </summary>
            public uint bitMapAddress;
            /// <summary>
            /// Адрес секции блоков данных.
            /// </summary>
            public uint dataAddress;
            /// <summary>
            /// Адрес секции массива индексных дескрипторов.
            /// </summary>
            public uint inodeArrayAddress;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct DirectoryRecord
        {
            public uint fileInodeId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = FILE_NAME_LENGTH)]
            public string fileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = FILE_EXTENSION_LENGTH)]
            public string fileExtension;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct Inode
        {
            /// <summary>
            /// Тип файла.
            /// </summary>
            public byte fileType;
            /// <summary>
            /// Код индексного дескриптора.
            /// </summary>
            public uint inodeId;
            /// <summary>
            /// Код пользователя-владельца (создателя) файла.
            /// </summary>
            public ushort userId;
            /// <summary>
            /// Код группы-владельца (создателя) файла.
            /// </summary>
            public ushort groupId;
            /// <summary>
            /// Права доступа к файлу — набор бит [rwx|rwx|rwx].
            /// </summary>
            public ushort permissions;
            /// <summary>
            /// Атрибуты файла — набор битов.
            /// </summary>
            public byte attributes;
            /// <summary>
            /// Размер файла в байтах.
            /// </summary>
            public uint fileSize;
            /// <summary>
            /// Время и дата создания файла.
            /// </summary>
            public long datetimeFileCreated;
            /// <summary>
            /// Время и дата последнего изменения файла.
            /// </summary>
            public long datetimeFileModified;
            /// <summary>
            /// Время и дата последнего изменения индексного дескриптора файла.
            /// </summary>
            public long datetimeInodeModified;
            /// <summary>
            /// Номер первого блока данных файла.
            /// </summary>
            public uint firstClusterIndex;
        }

        /// <summary>
        /// Представляет секцию битовой карты свободных/занятых кластеров данных.
        /// </summary>
        struct BitMap
        {
            /// <summary>
            /// Возможные значения элементов битовой карты для кластера.
            /// </summary>
            public enum ClusterState : byte
            {
                /// <summary>
                /// Свободен
                /// </summary>
                Free = 0x0,
                /// <summary>
                /// BAD-сектор
                /// </summary>
                Bad1 = 0x1,
                /// <summary>
                /// BAD-сектор
                /// </summary>
                Bad2 = 0x2,
                /// <summary>
                /// Занят
                /// </summary>
                Used = 0x3
            }

            BitArray _bits;

            public BitMap(uint bitmapNumClusters)
            {
                _bits = new BitArray((int)bitmapNumClusters * 2);
            }

            public BitMap(byte[] data)
            {
                _bits = new BitArray(data);
            }

            /// <summary>
            /// Ищет первый свободный блок данных.
            /// </summary>
            /// <param name="reserveCluster">Резервировать ли блок данных (помечать занятым)?</param>
            /// <returns>номер свободного блока данных, нумерация с 1</returns>
            public int FindFirstFreeCluster(bool reserveCluster = false)
            {
                for (int i = 0; i < _bits.Length / 2; i++)
                {
                    if (this[(uint)i] == ClusterState.Free)
                    {
                        if (reserveCluster)
                        {
                            SetClusterState((uint)i, ClusterState.Used);
                        }

                        return i;
                    }
                }

                return -1;
            }

            public ClusterState this[uint clusterIndex]
            {
                get
                {
                    int firstBitIndex = (int)(clusterIndex * 2);
                    bool firstBit = _bits[firstBitIndex];
                    bool secondBit = _bits[firstBitIndex + 1];

                    return (ClusterState)(Convert.ToByte(firstBit) * 2 + Convert.ToByte(secondBit));
                }
            }

            public byte SetClusterState(uint clusterIndex, ClusterState state)
            {
                int firstBitIndex = (int)(clusterIndex * 2);

                bool firstBit;
                bool secondBit;

                switch (state)
                {
                    case ClusterState.Free:
                        firstBit = false;
                        secondBit = false;
                        break;
                    case ClusterState.Bad1:
                        firstBit = false;
                        secondBit = true;
                        break;
                    case ClusterState.Bad2:
                        firstBit = true;
                        secondBit = false;
                        break;
                    case ClusterState.Used:
                        firstBit = true;
                        secondBit = true;
                        break;
                    default:
                        firstBit = false;
                        secondBit = false;
                        break;
                }

                _bits[firstBitIndex] = firstBit;
                _bits[firstBitIndex + 1] = secondBit;

                return (byte)state;
            }

            public byte[] GetBytes()
            {
                int numBytes = _bits.Count / 8;
                if (_bits.Count % 8 != 0)
                    numBytes++;

                byte[] bytes = new byte[numBytes];
                int byteIndex = 0, bitIndex = 0;

                for (int i = 0; i < _bits.Count; i++)
                {
                    if (_bits[i])
                        bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                    bitIndex++;
                    if (bitIndex == 8)
                    {
                        bitIndex = 0;
                        byteIndex++;
                    }
                }

                return bytes;
            }

            public uint GetLength()
            {
                return (uint)_bits.Length / 8;
            }
        }

        #endregion

        private FileStream FileStream { get; set; }
        private Superblock _superblock;
        private BitMap _bitMap;

        public ushort UserId { get; set; }
        public ushort GroupId { get; set; }

        public int CLUSTER_SIZE;

        //private bool _isFileOrDirectoryOpened = false;
        private string _openedPath;
        private Directory _currentDirectory;
        private Directory _rootDirectory;

        public bool CanCreateInode
        {
            get
            {
                return _superblock.numFreeInode >= 1;
            }
        }

        public bool CanCreateFileOrDirectory
        {
            get
            {
                return CanCreateInode && _superblock.numFreeClusters >= 1;
            }
        }

        private Directory CurrentDirectory
        {
            get
            {
                return _currentDirectory;
            }

            set
            {
                _currentDirectory = value;
            }
        }

        /// <summary>
        /// Создаёт экземпляр класса файловой системы, выполняя инициализацию структур в памяти.
        /// </summary>
        /// <param name="fileStream">Поток файла-образа диска с файловой системой.</param>
        /// <param name="userId">ID пользователя в системе, который будет обращаться в файловой системе.</param>
        /// <param name="groupId">ID группы пользователей, в которой состоит текущий пользователь.</param>
        public DehaxFileSystem(FileStream fileStream, ushort userId, ushort groupId)
        {
            FileStream = fileStream;
            UserId = userId;
            GroupId = groupId;

            Initialize();
        }

        /// <summary>
        /// Выполняет инициализацию структур файловой системы.
        /// </summary>
        private void Initialize()
        {
            _superblock = (Superblock)ReadStruct(FileStream, typeof(Superblock));
            CLUSTER_SIZE = _superblock.clusterFactor * DISK_BYTES_PER_SECTOR;
            FileStream.Seek(_superblock.bitMapAddress, SeekOrigin.Begin);
            byte[] bitMapData = new byte[_superblock.bitMapSize];
            FileStream.Read(bitMapData, 0, bitMapData.Length);
            _bitMap = new BitMap(bitMapData);

            if (_superblock.filesystemType != DFS_ID)
            {
                throw new InvalidDataException("Неизвестный тип файлово системы! Возможно, диск был повреждён.");
            }

            _rootDirectory = ReadDirectoryClusters(0, true);
            CurrentDirectory = _rootDirectory;
            _openedPath = "/";
        }

        private void FlushNewFile(string fileName, string fileExtension, int freeDirectoryRecordAddress, int freeInodeAddress, uint freeInodeId, int freeDataClusterIndex)
        {
            DirectoryRecord newDirectoryRecord = new DirectoryRecord();
            newDirectoryRecord.fileName = fileName;
            newDirectoryRecord.fileExtension = fileExtension;
            newDirectoryRecord.fileInodeId = freeInodeId;

            Inode newFileInode = new Inode();
            newFileInode.fileType = FILE_INODE_TYPE;
            newFileInode.inodeId = freeInodeId;
            newFileInode.userId = UserId;
            newFileInode.groupId = GroupId;
            newFileInode.permissions = new AccessRights(true, true, true, true, false, true, true, false, true).ToUInt16();
            Attributes attributes = new Attributes();
            attributes.hidden = false;
            attributes.readOnly = false;
            attributes.system = false;
            newFileInode.attributes = attributes.ToByte();
            newFileInode.fileSize = (uint)(_superblock.clusterFactor * DISK_BYTES_PER_SECTOR);
            newFileInode.datetimeFileCreated = DateTime.UtcNow.Ticks;
            newFileInode.datetimeFileModified = DateTime.UtcNow.Ticks;
            newFileInode.datetimeInodeModified = DateTime.UtcNow.Ticks;
            newFileInode.firstClusterIndex = (uint)freeDataClusterIndex;

            FileStream.Seek(freeDirectoryRecordAddress, SeekOrigin.Begin);
            WriteStruct(FileStream, newDirectoryRecord);
            FileStream.Seek(freeInodeAddress, SeekOrigin.Begin);
            WriteStruct(FileStream, newFileInode);
            FileStream.Seek(_superblock.dataAddress + (freeDataClusterIndex - 1) * CLUSTER_SIZE, SeekOrigin.Begin);

            // Записать один кластер файла
            FileStream.Write(BitConverter.GetBytes(LAST_CLUSTER_ID), 0, sizeof(uint));
        }

        private void FlushNewDirectory(string direcoryName, int freeDirectoryRecordAddress, int freeInodeAddress, uint freeInodeId, uint parentInodeId, int freeDataClusterIndex)
        {
            DirectoryRecord newDirectoryRecord = new DirectoryRecord();
            newDirectoryRecord.fileName = direcoryName;
            newDirectoryRecord.fileExtension = string.Empty;
            newDirectoryRecord.fileInodeId = freeInodeId;

            Inode newDirectoryInode = new Inode();
            newDirectoryInode.fileType = DIRECTORY_INODE_TYPE;
            newDirectoryInode.inodeId = freeInodeId;
            newDirectoryInode.userId = UserId;
            newDirectoryInode.groupId = GroupId;
            newDirectoryInode.permissions = new AccessRights(true, true, true, true, false, true, true, false, true).ToUInt16();
            Attributes attributes = new Attributes();
            attributes.hidden = false;
            attributes.readOnly = false;
            attributes.system = false;
            newDirectoryInode.attributes = attributes.ToByte();
            newDirectoryInode.fileSize = (uint)(_superblock.clusterFactor * DISK_BYTES_PER_SECTOR);
            newDirectoryInode.datetimeFileCreated = DateTime.UtcNow.Ticks;
            newDirectoryInode.datetimeFileModified = DateTime.UtcNow.Ticks;
            newDirectoryInode.datetimeInodeModified = DateTime.UtcNow.Ticks;
            newDirectoryInode.firstClusterIndex = (uint)freeDataClusterIndex;

            FileStream.Seek(freeDirectoryRecordAddress, SeekOrigin.Begin);
            WriteStruct(FileStream, newDirectoryRecord);
            FileStream.Seek(freeInodeAddress, SeekOrigin.Begin);
            WriteStruct(FileStream, newDirectoryInode);
            FileStream.Seek(_superblock.dataAddress + (freeDataClusterIndex - 1) * CLUSTER_SIZE, SeekOrigin.Begin);

            // Записать один кластер каталога
            FileStream.Write(BitConverter.GetBytes(LAST_CLUSTER_ID), 0, sizeof(uint));
            DirectoryRecord current = new DirectoryRecord();
            current.fileName = ".";
            current.fileExtension = "";
            current.fileInodeId = freeInodeId;
            DirectoryRecord parent = new DirectoryRecord();
            parent.fileName = "..";
            parent.fileExtension = "";
            parent.fileInodeId = parentInodeId;
            WriteStruct(FileStream, current);
            WriteStruct(FileStream, parent);
        }

        /// <summary>
        /// Создаёт новый пустой каталог.
        /// </summary>
        /// <param name="path">Путь к создаваемому каталогу.</param>
        public void CreateDirectory(string path)
        {
            if (!CanCreateFileOrDirectory)
            {
                throw new OutOfMemoryException("Невозможно создать каталог, превышен лимит!");
            }

            Utils.CheckPath(path);
            string fullPath = Utils.GetFullPath(path, _openedPath);
            string parentDirectoryPath = Utils.GetDirectoryName(fullPath);
            string newDirectoryName = Utils.GetFileName(fullPath);

            CurrentDirectory = _rootDirectory;

            CurrentDirectory = OpenDirectory(parentDirectoryPath);

            // Найти свободный блок данных
            int freeDataClusterIndex = _bitMap.FindFirstFreeCluster();

            if (freeDataClusterIndex < 0)
            {
                throw new OutOfMemoryException("Невозможно создать каталог, недостаточно места на жёстком диске!");
            }

            // Найти свободный инод
            int addressFreeInode = -1;
            uint addressInodes = _superblock.inodeArrayAddress;
            uint inodesCount = _superblock.numClusters;
            uint sizeOfInode = (uint)Marshal.SizeOf(typeof(Inode));
            FileStream.Seek(addressInodes, SeekOrigin.Begin);
            Inode freeInode = new Inode();

            for (int i = 0; i < inodesCount; i++)
            {
                FileStream.Seek(sizeOfInode, SeekOrigin.Current);
                freeInode = (Inode)ReadStruct(FileStream, typeof(Inode));

                if (freeInode.fileType == FREE_INODE_TYPE)
                {
                    addressFreeInode = (int)FileStream.Position;
                    break;
                }
            }

            if (addressFreeInode < 0)
            {
                throw new OutOfMemoryException("Невозможно создать каталог, не найден адрес свободного индексного дескриптора!");
            }

            // Найти свободное место в каталоге. Разместить запись в каталоге
            int freeDirectoryRecordAddress = -1;
            FileStream.Seek(CurrentDirectory.StreamAddress, SeekOrigin.Begin);

            long address = CurrentDirectory.StreamAddress;

            uint nextClusterIndex = LAST_CLUSTER_ID;
            uint sizeOfDirectoryRecord = (uint)Marshal.SizeOf(typeof(DirectoryRecord));

            do
            {
                FileStream.Seek(address, SeekOrigin.Begin);

                nextClusterIndex = (uint)ReadStruct(FileStream, typeof(uint));

                DirectoryRecord directoryRecord;
                uint upperBound = (uint)(address + _superblock.clusterFactor * DISK_BYTES_PER_SECTOR);
                for (long i = address + sizeof(uint); i < upperBound; i += sizeOfDirectoryRecord)
                {
                    directoryRecord = (DirectoryRecord)ReadStruct(FileStream, typeof(DirectoryRecord));

                    if (directoryRecord.fileInodeId == FREE_DIRECTORY_RECORD)
                    {
                        freeDirectoryRecordAddress = (int)FileStream.Position;
                        break;
                    }
                    else
                    {
                        FileStream.Seek(sizeOfDirectoryRecord, SeekOrigin.Current);
                    }
                }

                address = _superblock.dataAddress + (nextClusterIndex - 1) * CLUSTER_SIZE;

            } while (nextClusterIndex != LAST_CLUSTER_ID);

            if (freeDirectoryRecordAddress < 0)
            {
                throw new OutOfMemoryException("Невозможно создать каталог, превышен лимит записей в каталоге!");
            }

            // Поток уже находится на свободной записи родительского каталога.
            FileStream.Seek(freeDirectoryRecordAddress, SeekOrigin.Begin);

            // Записать новый каталог на диск.
            FlushNewDirectory(newDirectoryName, freeDirectoryRecordAddress, addressFreeInode, freeInode.inodeId, CurrentDirectory.InodeId, freeDataClusterIndex);
            _bitMap.SetClusterState((uint)freeDataClusterIndex, BitMap.ClusterState.Used);
        }

        /// <summary>
        /// Создаёт новый пустой файл.
        /// </summary>
        /// <param name="path">Путь к создаваемому файлу.</param>
        public void CreateFile(string path)
        {
            if (!CanCreateFileOrDirectory)
            {
                throw new OutOfMemoryException("Невозможно создать файл, недостаточно свободного дискового пространства!");
            }

            Utils.CheckPath(path);
            string fullPath = Utils.GetFullPath(path, _openedPath);
            string parentDirectoryPath = Utils.GetDirectoryName(fullPath);
            string newFileName = Utils.GetFileName(fullPath);
            string newFileNameWithoutExtension = Utils.GetFileNameWithoutExtension(fullPath);
            string newFileExtension = Utils.GetExtension(fullPath);

            CurrentDirectory = _rootDirectory;

            CurrentDirectory = OpenDirectory(parentDirectoryPath);

            // Найти свободный блок данных
            int freeDataClusterIndex = _bitMap.FindFirstFreeCluster();

            if (freeDataClusterIndex < 0)
            {
                throw new OutOfMemoryException("Невозможно создать каталог, недостаточно места на жёстком диске!");
            }

            // Найти свободный инод
            int addressFreeInode = -1;
            uint addressInodes = _superblock.inodeArrayAddress;
            uint inodesCount = _superblock.numClusters;
            uint sizeOfInode = (uint)Marshal.SizeOf(typeof(Inode));
            FileStream.Seek(addressInodes, SeekOrigin.Begin);
            Inode freeInode = new Inode();

            for (int i = 0; i < inodesCount; i++)
            {
                FileStream.Seek(sizeOfInode, SeekOrigin.Current);
                freeInode = (Inode)ReadStruct(FileStream, typeof(Inode));

                if (freeInode.fileType == FREE_INODE_TYPE)
                {
                    addressFreeInode = (int)FileStream.Position;
                    break;
                }
            }

            if (addressFreeInode < 0)
            {
                throw new OutOfMemoryException("Невозможно создать каталог, не найден адрес свободного индексного дескриптора!");
            }

            // Найти свободное место в каталоге. Разместить запись в каталоге
            int freeDirectoryRecordAddress = -1;
            FileStream.Seek(CurrentDirectory.StreamAddress, SeekOrigin.Begin);

            long address = CurrentDirectory.StreamAddress;

            uint nextClusterIndex = LAST_CLUSTER_ID;
            uint sizeOfDirectoryRecord = (uint)Marshal.SizeOf(typeof(DirectoryRecord));

            do
            {
                FileStream.Seek(address, SeekOrigin.Begin);

                nextClusterIndex = (uint)ReadStruct(FileStream, typeof(uint));

                DirectoryRecord directoryRecord;
                uint upperBound = (uint)(address + _superblock.clusterFactor * DISK_BYTES_PER_SECTOR);
                for (long i = address + sizeof(uint); i < upperBound; i += sizeOfDirectoryRecord)
                {
                    directoryRecord = (DirectoryRecord)ReadStruct(FileStream, typeof(DirectoryRecord));

                    if (directoryRecord.fileInodeId == FREE_DIRECTORY_RECORD)
                    {
                        freeDirectoryRecordAddress = (int)FileStream.Position;
                        break;
                    }
                    else
                    {
                        FileStream.Seek(sizeOfDirectoryRecord, SeekOrigin.Current);
                    }
                }

                address = _superblock.dataAddress + (nextClusterIndex - 1) * CLUSTER_SIZE;

            } while (nextClusterIndex != LAST_CLUSTER_ID);

            if (freeDirectoryRecordAddress < 0)
            {
                throw new OutOfMemoryException("Невозможно создать каталог, превышен лимит записей в каталоге!");
            }

            // Поток уже находится на свободной записи родительского каталога.
            FileStream.Seek(freeDirectoryRecordAddress, SeekOrigin.Begin);

            FlushNewFile(newFileNameWithoutExtension, newFileExtension, freeDirectoryRecordAddress, addressFreeInode, freeInode.inodeId, freeDataClusterIndex);
            _bitMap.SetClusterState((uint)freeDataClusterIndex, BitMap.ClusterState.Used);
            _superblock.numFreeClusters--;
            _superblock.numFreeInode--;
        }

        /// <summary>
        /// Считывает каталог из кластеров данных, сохраняя информацию о списке содержимого и сведения о каждом элементе.
        /// </summary>
        /// <param name="clusterIndex">Номер первого кластера каталога.</param>
        /// <param name="rootDirectory">Является ли считываемый каталог корневым.</param>
        /// <returns></returns>
        private Directory ReadDirectoryClusters(uint clusterIndex, bool rootDirectory = false)
        {
            Directory result = new Directory();

            long address = rootDirectory ? _superblock.rootAddress : _superblock.dataAddress + (clusterIndex - 1) * CLUSTER_SIZE;

            uint nextClusterIndex = LAST_CLUSTER_ID;

            do
            {
                FileStream.Seek(address, SeekOrigin.Begin);

                if (!rootDirectory)
                {
                    nextClusterIndex = (uint)ReadStruct(FileStream, typeof(uint));
                }

                uint sizeOfDirectoryRecord = (uint)Marshal.SizeOf(typeof(DirectoryRecord));
                DirectoryRecord[] directoryRecords = new DirectoryRecord[(CLUSTER_SIZE - (!rootDirectory ? sizeof(uint) : 0)) / sizeOfDirectoryRecord];
                uint upperBound = (uint)(address + CLUSTER_SIZE);
                for (uint j = 0; j < directoryRecords.Length; j++)
                {
                    directoryRecords[j] = (DirectoryRecord)ReadStruct(FileStream, typeof(DirectoryRecord));
                    //FileStream.Seek(sizeOfDirectoryRecord, SeekOrigin.Current);
                }

                uint fileInodeId;
                address = _superblock.inodeArrayAddress;
                FileStream.Seek(address, SeekOrigin.Begin);
                uint sizeOfInode = (uint)Marshal.SizeOf(typeof(Inode));
                Inode inode;
                MetaFile metaFile;
                for (int i = 0; i < directoryRecords.Length; i++)
                {
                    fileInodeId = directoryRecords[i].fileInodeId;

                    if (fileInodeId != FREE_DIRECTORY_RECORD)
                    {
                        FileStream.Seek(i * sizeOfInode, SeekOrigin.Current);
                        inode = (Inode)ReadStruct(FileStream, typeof(Inode));

                        switch (inode.fileType)
                        {
                            case INODE_FILE_TYPE:
                                metaFile = new File();
                                break;
                            case INODE_DIRECTORY_TYPE:
                                metaFile = new Directory();
                                break;
                            default:
                                throw new InvalidDataException("Данные индексного дескриптора повреждены! Неизвестный тип записи в каталоге.");
                        }

                        metaFile.CreationTime = inode.datetimeFileCreated;
                        metaFile.Extension = directoryRecords[i].fileExtension;
                        metaFile.Name = directoryRecords[i].fileName;
                        metaFile.GroupId = inode.groupId;
                        metaFile.InodeId = inode.inodeId;
                        metaFile.InodeModificationTime = inode.datetimeInodeModified;
                        metaFile.ModificationTime = inode.datetimeFileModified;
                        metaFile.Size = inode.fileSize;
                        metaFile.StreamAddress = _superblock.dataAddress + (inode.firstClusterIndex - 1) * CLUSTER_SIZE;
                        //metaFile.DiskRecordAddress = _superblock.dataAddress + ()
                        metaFile.UserId = inode.userId;

                        metaFile.AccessRights = new AccessRights(inode.permissions);
                        metaFile.Attributes = new Attributes(inode.attributes);

                        result.AddRecord(metaFile);
                    }
                }

                address = _superblock.dataAddress + (nextClusterIndex - 1) * CLUSTER_SIZE;
            } while (nextClusterIndex != LAST_CLUSTER_ID);

            return result;
        }

        /// <summary>
        /// Находит в текущем каталоге указанный каталог и возвращает его.
        /// </summary>
        /// <param name="directoryName">Имя искомого каталога.</param>
        /// <returns>Объект найденного каталога или null.</returns>
        private Directory ReadDirectory(string directoryName)
        {
            MetaFile metaFile = CurrentDirectory.Find(directoryName);
            Directory directory = metaFile as Directory;

            if (directory != null)
            {
                return directory;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Открывает каталог для работы. Загружает в структуры ФС информацию о каталоге.
        /// </summary>
        /// <param name="path">Путь к каталогу для открытия.</param>
        public Directory OpenDirectory(string path)
        {
            Utils.CheckPath(path);
            string fullPath = Utils.GetFullPath(path, _openedPath);
            string[] directoriesNames = Utils.GetDirectoriesNames(fullPath);

            // Корневой каталог
            if (directoriesNames.Length == 0)
            {
                return _rootDirectory;
            }

            Directory directory = _rootDirectory;
            for (int i = 0; i < directoriesNames.Length; i++)
            {
                directory = ReadDirectory(directoriesNames[i]);

                if (directory != null)
                {
                    CurrentDirectory = directory;
                }
                else
                {
                    throw new DirectoryNotFoundException("Директория не существует! Проверьте правильность пути.");
                }
            }

            return directory;
        }

        /// <summary>
        /// Считывает указанное количество байт содержимого файла, начиная с определённой позиции.
        /// </summary>
        /// <param name="fileName">Имя файла в текущем каталоге, содержимое которого необходимо прочитать.</param>
        /// <returns>Возвращает массив байтов в соответствии с заданными параметрами.</returns>
        public byte[] ReadFile(string fileName, int offset = 0, int count = -1)
        {
            File file = new File();

            MetaFile metaFile = CurrentDirectory.Find(fileName);
            file = metaFile as File;

            if (file == null)
            {
                return null;
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Значение смещения не может быть отрицательным.");
            }

            if (count > file.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Значение количества байтов превышает размер файла. Для чтения всего файла используйте значение -1.");
            }

            long address = file.StreamAddress;
            FileStream.Seek(address, SeekOrigin.Begin);

            int currentOffset = 0;
            count = (count == -1) ? (int)file.Size : count;
            byte[] data = new byte[count];

            while (currentOffset + CLUSTER_SIZE - sizeof(uint) <= offset)
            {
                currentOffset += CLUSTER_SIZE;
            }

            uint nextCluster = (uint)ReadStruct(FileStream, typeof(uint));
            while (currentOffset - offset < count)
            {
                if (currentOffset - offset >= CLUSTER_SIZE - sizeof(uint))
                {
                    currentOffset += FileStream.Read(data, currentOffset - offset, CLUSTER_SIZE - sizeof(uint));
                }
                else
                {
                    currentOffset += FileStream.Read(data, currentOffset - offset, count - currentOffset - offset);
                }

                if (nextCluster != LAST_CLUSTER_ID)
                {
                    address = _superblock.dataAddress + (nextCluster - 1) * CLUSTER_SIZE;
                    FileStream.Seek(address, SeekOrigin.Begin);
                }
            }

            return data;
        }

        public int WriteFile(string fileName, byte[] data)
        {
            File file = new File();

            MetaFile metaFile = CurrentDirectory.Find(fileName);
            file = metaFile as File;

            if (file == null)
            {
                throw new FileNotFoundException("Файл не найден, невозможно записать данные в файл.", fileName);
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException(nameof(data), "Данные не могут быть пустым массивом!");
            }

            int sizeOfInode = Marshal.SizeOf(typeof(Inode));
            FileStream.Seek(_superblock.inodeArrayAddress + (file.InodeId - 1) * sizeOfInode, SeekOrigin.Begin);
            Inode inode = (Inode)ReadStruct(FileStream, typeof(Inode));

            // Найти свободные блоки данных
            int numberNeedDataClusters = data.Length / (CLUSTER_SIZE - sizeof(uint)) + ((data.Length % (CLUSTER_SIZE - sizeof(uint)) > 0) ? 1 : 0);
            int numberFreeDataClusters = (int)_superblock.numFreeClusters;

            if (numberFreeDataClusters < numberNeedDataClusters)
            {
                throw new OutOfMemoryException("Невозможно записать данные в файл, недостаточно свободного места на жёстком диске!");
            }

            uint[] freeDataClustersIndexes = new uint[numberNeedDataClusters];

            for (int i = 0; i < numberNeedDataClusters; i++)
            {
                int freeDataClusterIndex = _bitMap.FindFirstFreeCluster(true);

                if (freeDataClusterIndex < 0)
                {
                    throw new OutOfMemoryException("Невозможно записать данные в файл, недостаточно места на жёстком диске!");
                }

                freeDataClustersIndexes[i] = (uint)freeDataClusterIndex;
                _superblock.numFreeClusters--;
            }


            int numberWrittenBytes = 0;
            uint nextClusterValue = LAST_CLUSTER_ID;
            //while (numberWrittenBytes < data.Length)
            //{

            //}
            long address = _superblock.dataAddress + (inode.firstClusterIndex - 1) * CLUSTER_SIZE;
            FileStream.Seek(address, SeekOrigin.Begin);

            for (int clusterNumber = 0; clusterNumber < numberNeedDataClusters - 1; clusterNumber++)
            {
                nextClusterValue = freeDataClustersIndexes[clusterNumber];
                WriteStruct(FileStream, nextClusterValue);
                FileStream.Write(data, numberWrittenBytes, CLUSTER_SIZE - sizeof(uint));
                numberWrittenBytes += CLUSTER_SIZE - sizeof(uint);
                address = _superblock.dataAddress + (nextClusterValue - 1) * CLUSTER_SIZE;
                FileStream.Seek(address, SeekOrigin.Begin);
            }
            nextClusterValue = LAST_CLUSTER_ID;
            WriteStruct(FileStream, nextClusterValue);
            FileStream.Write(data, numberWrittenBytes, data.Length - numberWrittenBytes);
            numberWrittenBytes += data.Length - numberWrittenBytes;

            // Переместить к индексному дескриптору, записать размер файла.

            inode.fileSize = (uint)numberWrittenBytes;
            FileStream.Seek(-sizeOfInode, SeekOrigin.Current);
            WriteStruct(FileStream, inode);

            return numberWrittenBytes;
        }

        public int AppendFile(string fileName, byte[] data)
        {
            File file = new File();

            MetaFile metaFile = CurrentDirectory.Find(fileName);
            file = metaFile as File;

            if (file == null)
            {
                throw new FileNotFoundException("Файл не найден, невозможно записать данные в файл.", fileName);
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException(nameof(data), "Данные не могут быть пустым массивом!");
            }

            int sizeOfInode = Marshal.SizeOf(typeof(Inode));
            FileStream.Seek(_superblock.inodeArrayAddress + (file.InodeId - 1) * sizeOfInode, SeekOrigin.Begin);
            Inode inode = (Inode)ReadStruct(FileStream, typeof(Inode));

            // Найти свободные блоки данных
            int numberNeedDataClusters = (data.Length - (int)(CLUSTER_SIZE - (inode.fileSize % (CLUSTER_SIZE - sizeof(uint))))) / (CLUSTER_SIZE - sizeof(uint)) + ((data.Length % (CLUSTER_SIZE - sizeof(uint)) > 0) ? 1 : 0);
            int numberFreeDataClusters = (int)_superblock.numFreeClusters;

            if (numberFreeDataClusters < numberNeedDataClusters)
            {
                throw new OutOfMemoryException("Невозможно записать данные в файл, недостаточно свободного места на жёстком диске!");
            }

            uint[] freeDataClustersIndexes = new uint[numberNeedDataClusters];

            for (int i = 0; i < numberNeedDataClusters; i++)
            {
                int freeDataClusterIndex = _bitMap.FindFirstFreeCluster(true);

                if (freeDataClusterIndex < 0)
                {
                    throw new OutOfMemoryException("Невозможно записать данные в файл, недостаточно места на жёстком диске!");
                }

                freeDataClustersIndexes[i] = (uint)freeDataClusterIndex;
                _superblock.numFreeClusters--;
            }

            uint lastClusterIndex = inode.firstClusterIndex;

            long address = _superblock.dataAddress + (inode.firstClusterIndex - 1) * CLUSTER_SIZE;
            FileStream.Seek(address, SeekOrigin.Begin);

            uint nextClusterValue = LAST_CLUSTER_ID;
            while ((nextClusterValue = (uint)ReadStruct(FileStream, typeof(uint))) != LAST_CLUSTER_ID)
            {
                FileStream.Seek(CLUSTER_SIZE - sizeof(uint), SeekOrigin.Current);
                lastClusterIndex = nextClusterValue;
            }

            int numberWrittenBytes = 0;
            nextClusterValue = LAST_CLUSTER_ID;

            address = _superblock.dataAddress + (lastClusterIndex - 1) * CLUSTER_SIZE;
            FileStream.Seek(address, SeekOrigin.Begin);

            int lastClusterUsedCount = (int)(inode.fileSize % (CLUSTER_SIZE - sizeof(uint)));
            
            if (numberNeedDataClusters > 0)
            {
                nextClusterValue = freeDataClustersIndexes[0];
                WriteStruct(FileStream, nextClusterValue);
                FileStream.Write(data, numberWrittenBytes, CLUSTER_SIZE - sizeof(uint) - lastClusterUsedCount);
                numberWrittenBytes += CLUSTER_SIZE - sizeof(uint);
                address = _superblock.dataAddress + (nextClusterValue - 1) * CLUSTER_SIZE;
                FileStream.Seek(address, SeekOrigin.Begin);
            }

            for (int clusterNumber = 1; clusterNumber < numberNeedDataClusters - 1; clusterNumber++)
            {
                nextClusterValue = freeDataClustersIndexes[clusterNumber];
                WriteStruct(FileStream, nextClusterValue);
                FileStream.Write(data, numberWrittenBytes, CLUSTER_SIZE - sizeof(uint));
                numberWrittenBytes += CLUSTER_SIZE - sizeof(uint);
                address = _superblock.dataAddress + (nextClusterValue - 1) * CLUSTER_SIZE;
                FileStream.Seek(address, SeekOrigin.Begin);
            }
            nextClusterValue = LAST_CLUSTER_ID;
            WriteStruct(FileStream, nextClusterValue);
            FileStream.Write(data, numberWrittenBytes, data.Length - numberWrittenBytes);
            numberWrittenBytes += data.Length - numberWrittenBytes;

            // Переместить к индексному дескриптору, записать размер файла.

            inode.fileSize = (uint)numberWrittenBytes;
            FileStream.Seek(-sizeOfInode, SeekOrigin.Current);
            WriteStruct(FileStream, inode);

            return numberWrittenBytes;
        }

        /// <summary>
        /// Задаёт указанные атрибуты файлу или каталогу.
        /// Не изменяет текущий рабочий каталог.
        /// </summary>
        /// <param name="path">Путь к файлу или каталогу.</param>
        /// <param name="attributes">Атрибуты, которые будут заданы.</param>
        public void SetAttributes(string path, Attributes attributes)
        {
            Directory current = CurrentDirectory;

            Directory directory = OpenDirectory(path);

            string fileName = Utils.GetFileName(path);
            MetaFile metaFile = directory.Find(path);

            Inode inode = new Inode();
            int sizeOfInode = Marshal.SizeOf(typeof(Inode));
            FileStream.Seek(_superblock.inodeArrayAddress + (metaFile.InodeId - 1) * sizeOfInode, SeekOrigin.Begin);
            inode = (Inode)ReadStruct(FileStream, typeof(Inode));
            inode.attributes = attributes.ToByte();
            WriteStruct(FileStream, inode);

            CurrentDirectory = current;
        }

        /// <summary>
        /// Возвращает атрибуты файла или каталога.
        /// </summary>
        /// <param name="path">Путь к файлу или каталогу.</param>
        /// <returns>атрибуты файла или каталога</returns>
        public Attributes GetAttributes(string path)
        {
            Directory current = CurrentDirectory;

            Directory directory = OpenDirectory(path);

            string fileName = Utils.GetFileName(path);
            MetaFile metaFile = directory.Find(path);

            Inode inode = new Inode();
            int sizeOfInode = Marshal.SizeOf(typeof(Inode));
            FileStream.Seek(_superblock.inodeArrayAddress + (metaFile.InodeId - 1) * sizeOfInode, SeekOrigin.Begin);
            inode = (Inode)ReadStruct(FileStream, typeof(Inode));
            Attributes attributes = new Attributes(inode.attributes);

            CurrentDirectory = current;

            return attributes;
        }

        /// <summary>
        /// Удаляет файл в файловой системе.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        public void DeleteFile(string path)
        {
            DeleteFileOrDirectory(path);
        }

        /// <summary>
        /// Удаляет каталог в файловой системе.
        /// </summary>
        /// <param name="path">Путь к каталогу.</param>
        public void DeleteDirectory(string path)
        {
            DeleteFileOrDirectory(path);
            // BUG: Не удаляется содержимое каталога.
        }

        /// <summary>
        /// Удаляет запись из родительского каталога объекта и освобождает индексный дескриптор.
        /// </summary>
        /// <param name="path">Путь к каталогу или файлу, который необходимо удалить.</param>
        private void DeleteFileOrDirectory(string path)
        {
            string fileName = Utils.GetFileNameWithoutExtension(path);
            string fileExtension = Utils.GetExtension(path);
            string fullFileName = Utils.GetFileName(path);
            string directoryPath = Utils.GetDirectoryName(path);
            Directory directory = OpenDirectory(directoryPath);
            MetaFile metaFile = directory.Find(fullFileName);

            FileStream.Seek(metaFile.StreamAddress, SeekOrigin.Begin);
            DirectoryRecord directoryRecord = new DirectoryRecord();
            directoryRecord.fileInodeId = FREE_DIRECTORY_RECORD;
            WriteStruct(FileStream, directoryRecord);
            _superblock.numFreeClusters++;

            // Перейти к заданному индексному дескриптору
            int sizeOfInode = Marshal.SizeOf(typeof(Inode));
            FileStream.Seek(_superblock.inodeArrayAddress + (metaFile.InodeId - 1) * sizeOfInode, SeekOrigin.Begin);
            Inode inode = new Inode();
            inode.fileType = FREE_INODE_TYPE;
            WriteStruct(FileStream, inode);
            _superblock.numFreeInode++;
        }

        private static object ReadStruct(Stream stream, Type t)
        {
            byte[] buffer = new byte[Marshal.SizeOf(t)];
            for (int read = 0; read < buffer.Length; read += stream.Read(buffer, read, buffer.Length)) ;
            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            object o = Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), t);
            gcHandle.Free();
            return o;
        }

        private static void WriteStruct(Stream writer, object o)
        {
            byte[] buffer = new byte[Marshal.SizeOf(o.GetType())];
            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(o, gcHandle.AddrOfPinnedObject(), true);
            writer.Write(buffer, 0, buffer.Length);
            gcHandle.Free();
        }
    }
}
