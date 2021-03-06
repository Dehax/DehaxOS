﻿using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DFSformat
{
    class DFSImage
    {
        /// <summary>
        /// Количество байт, занимаемых одним сектором жёсткого диска
        /// </summary>
        private const short DISK_BYTES_PER_SECTOR = 512;
        //private const int BITMAP_NUM_BITS = 102400;
        //private const int NUM_INODES = 2048000;
        //private const byte NUM_ROOT_DIRECTORY_RECORDS = 93;

        /// <summary>
        /// Длина поля имени файла в записи каталога
        /// </summary>
        private const byte FILE_NAME_LENGTH = 15;
        /// <summary>
        /// Длина поля расширения файла в записи каталога
        /// </summary>
        private const byte FILE_EXTENSION_LENGTH = 5;

        /// <summary>
        /// Значение свободного индексного дескриптора
        /// </summary>
        private const int FREE_INODE_ID = -1;

        private const short FIRST_INODE_TYPE_ADDRESS = 12814;

        private readonly long _diskPartitionSize;
        private readonly byte _diskClusterFactor;
        public readonly int _diskClusterSize;

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
            public int numClusters;
            /// <summary>
            /// Размер кластера (логического блока), множитель секторов:
            /// • 1 * 512 (размер сектора) = 512 байт;
            /// • 2 * 512 = 1024 байт.
            /// </summary>
            public byte clusterFactor;
            /// <summary>
            /// Размер массива индексных дескрипторов (inode).
            /// </summary>
            public int inodeArraySize;
            /// <summary>
            /// Размер битовой карты свободных/занятых кластеров.
            /// </summary>
            public int bitMapSize;
            /// <summary>
            /// Количество свободных кластеров.
            /// </summary>
            public int numFreeClusters;
            /// <summary>
            /// Количество свободных индексных дескрипторов (inode).
            /// </summary>
            public int numFreeInode;

            /// <summary>
            /// Адрес секции корневого каталога.
            /// </summary>
            public int rootAddress;
            /// <summary>
            /// Адрес секции битовой карты.
            /// </summary>
            public int bitMapAddress;
            /// <summary>
            /// Адрес секции блоков данных.
            /// </summary>
            public int dataAddress;
            /// <summary>
            /// Адрес секции массива индексных дескрипторов.
            /// </summary>
            public int inodeArrayAddress;

            /*public byte[] GetBytes()
            //{
            //    byte[] data = new byte[GetLength()];
            //    byte[] tmp;

            //    data[0] = filesystemType;

            //    tmp = BitConverter.GetBytes(numClusters);
            //    data[1] = tmp[0];
            //    data[2] = tmp[1];
            //    data[3] = tmp[2];
            //    data[4] = tmp[3];

            //    data[5] = clusterFactor;

            //    tmp = BitConverter.GetBytes(inodeArraySize);
            //    data[6] = tmp[0];
            //    data[7] = tmp[1];
            //    data[8] = tmp[2];
            //    data[9] = tmp[3];

            //    tmp = BitConverter.GetBytes(bitMapSize);
            //    data[10] = tmp[0];
            //    data[11] = tmp[1];
            //    data[12] = tmp[2];
            //    data[13] = tmp[3];

            //    tmp = BitConverter.GetBytes(numFreeClusters);
            //    data[14] = tmp[0];
            //    data[15] = tmp[1];
            //    data[16] = tmp[2];
            //    data[17] = tmp[3];

            //    tmp = BitConverter.GetBytes(numFreeInode);
            //    data[18] = tmp[0];
            //    data[19] = tmp[1];
            //    data[20] = tmp[2];
            //    data[21] = tmp[3];

            //    return data;
            //}

            //public static int GetLength()
            //{
            //    return 22;
            }*/
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

            public BitMap(int bitmapNumClusters)
            {
                _bits = new BitArray(bitmapNumClusters * 2);
            }

            public ClusterState this[int clusterIndex]
            {
                get
                {
                    int firstBitIndex = clusterIndex * 2;
                    bool firstBit = _bits[firstBitIndex];
                    bool secondBit = _bits[firstBitIndex + 1];

                    return (ClusterState)(Convert.ToByte(firstBit) * 2 + Convert.ToByte(secondBit));
                }
            }

            public byte SetClusterState(int clusterIndex, ClusterState state)
            {
                int firstBitIndex = clusterIndex * 2;

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
                //byte[] data = new byte[_bits.Length / 8];

                //_bits.CopyTo(data, 0);

                //return data;

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

            public int GetLength()
            {
                return _bits.Length / 8;
            }
        }

        /// <summary>
        /// Представляет секцию массива индексных дескрипторов.
        /// </summary>
        struct Inodes
        {
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
                public int inodeId;
                /// <summary>
                /// Код пользователя-владельца (создателя) файла.
                /// </summary>
                public short userId;
                /// <summary>
                /// Код группы-владельца (создателя) файла.
                /// </summary>
                public short groupId;
                /// <summary>
                /// Права доступа к файлу — набор бит [rwx|rwx|rwx].
                /// </summary>
                public short permissions;
                /// <summary>
                /// Атрибуты файла — набор битов.
                /// </summary>
                public byte attributes;
                /// <summary>
                /// Размер файла в байтах.
                /// </summary>
                public int fileSize;
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
                public int firstClusterIndex;

                //public byte[] GetBytes()
                //{
                //    byte[] data = new byte[GetLength()];
                //    byte[] tmp;

                //    data[0] = fileType;

                //    tmp = BitConverter.GetBytes(inodeId);
                //    data[1] = tmp[0];
                //    data[2] = tmp[1];
                //    data[3] = tmp[2];
                //    data[4] = tmp[3];

                //    tmp = BitConverter.GetBytes(userId);
                //    data[5] = tmp[0];
                //    data[6] = tmp[1];

                //    tmp = BitConverter.GetBytes(groupId);
                //    data[7] = tmp[0];
                //    data[8] = tmp[1];

                //    tmp = BitConverter.GetBytes(permissions);
                //    data[9] = tmp[0];
                //    data[10] = tmp[1];

                //    data[11] = attributes;

                //    tmp = BitConverter.GetBytes(fileSize);
                //    data[12] = tmp[0];
                //    data[13] = tmp[1];
                //    data[14] = tmp[2];
                //    data[15] = tmp[3];

                //    tmp = BitConverter.GetBytes(datetimeFileCreated);
                //    data[16] = tmp[0];
                //    data[17] = tmp[1];
                //    data[18] = tmp[2];
                //    data[19] = tmp[3];
                //    data[20] = tmp[4];
                //    data[21] = tmp[5];
                //    data[22] = tmp[6];
                //    data[23] = tmp[7];

                //    tmp = BitConverter.GetBytes(datetimeFileModified);
                //    data[24] = tmp[0];
                //    data[25] = tmp[1];
                //    data[26] = tmp[2];
                //    data[27] = tmp[3];
                //    data[28] = tmp[4];
                //    data[29] = tmp[5];
                //    data[30] = tmp[6];
                //    data[31] = tmp[7];

                //    tmp = BitConverter.GetBytes(datetimeInodeModified);
                //    data[32] = tmp[0];
                //    data[33] = tmp[1];
                //    data[34] = tmp[2];
                //    data[35] = tmp[3];
                //    data[36] = tmp[4];
                //    data[37] = tmp[5];
                //    data[38] = tmp[6];
                //    data[39] = tmp[7];

                //    tmp = BitConverter.GetBytes(firstClusterIndex);
                //    data[40] = tmp[0];
                //    data[41] = tmp[1];
                //    data[42] = tmp[2];
                //    data[43] = tmp[3];

                //    return data;
                //}

                //public static int GetLength()
                //{
                //    return 44;
                //}
            }

            Inode[] _inodes;

            public Inodes(int numInodes)
            {
                _inodes = new Inode[numInodes];

                Inode inode = new Inode();
                for (int i = 0; i < _inodes.Length; i++)
                {
                    inode.inodeId = i + 1;
                    _inodes[i] = inode;
                }
            }

            public Inode this[int inodeIndex]
            {
                get
                {
                    return _inodes[inodeIndex];
                }
                set
                {
                    _inodes[inodeIndex] = value;
                }
            }

            public byte[] GetBytes()
            {
                int inodeLength = Marshal.SizeOf<Inode>();
                byte[] data = new byte[_inodes.Length * inodeLength];
                byte[] tmp;
                GCHandle gcHandle;

                for (int i = 0; i < _inodes.Length; i++)
                {
                    tmp = new byte[Marshal.SizeOf(_inodes[i].GetType())];
                    gcHandle = GCHandle.Alloc(tmp, GCHandleType.Pinned);
                    Marshal.StructureToPtr(_inodes[i], gcHandle.AddrOfPinnedObject(), true);

                    for (int j = 0; j < inodeLength; j++)
                    {
                        data[i * inodeLength + j] = tmp[j];
                    }

                    gcHandle.Free();
                }

                return data;
            }

            public int GetLength()
            {
                return _inodes.Length * Marshal.SizeOf<Inode>();
            }

            public int GetCount()
            {
                return _inodes.Length;
            }
        }

        /// <summary>
        /// Представляет секцию корневого каталога.
        /// </summary>
        struct RootDirectory
        {
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
            public struct RootDirectoryRecord
            {
                public int fileInodeId;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = FILE_NAME_LENGTH)]
                public string fileName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = FILE_EXTENSION_LENGTH)]
                public string fileExtension;

                public RootDirectoryRecord(int freeInodeId/*byte fileNameLength, byte fileExtensionLength*/)
                {
                    fileInodeId = freeInodeId;
                    fileName = ""; // new char[fileNameLength];
                    fileExtension = ""; // new char[fileExtensionLength];
                }

                public void SetFullFileName(string fileName, string fileExtension)
                {
                    this.fileName = fileName;
                    this.fileExtension = fileExtension;
                }

                //public byte[] GetBytes()
                //{
                //    byte[] data = new byte[GetLength()];
                //    byte[] tmp;

                //    tmp = BitConverter.GetBytes(fileInodeId);
                //    data[0] = tmp[0];
                //    data[1] = tmp[1];
                //    data[2] = tmp[2];
                //    data[3] = tmp[3];

                //    for (int i = 0; i < fileName.Length; i++)
                //    {
                //        data[4 + i] = (byte)fileName[i];
                //    }

                //    for (int i = 0; i < fileExtension.Length; i++)
                //    {
                //        data[19 + i] = (byte)fileExtension[i];
                //    }

                //    return data;
                //}

                //public static int GetLength()
                //{
                //    return 24;
                //}
            }

            int _reserved;
            RootDirectoryRecord[] _rootDirectory;
            byte[] _emptySpace;

            public RootDirectory(int clusterSize)
            {
                _reserved = -1;
                int sizeOfRootDirectoryRecord = Marshal.SizeOf<RootDirectoryRecord>();
                int directoryRecordsCount = (clusterSize - sizeof(int)) / sizeOfRootDirectoryRecord;
                _rootDirectory = new RootDirectoryRecord[directoryRecordsCount];
                _emptySpace = new byte[clusterSize - directoryRecordsCount * sizeOfRootDirectoryRecord - sizeof(int)];

                _rootDirectory[0] = new RootDirectoryRecord()
                {
                    fileInodeId = 1,
                    fileName = ".",
                    fileExtension = ""
                };
                _rootDirectory[1] = new RootDirectoryRecord()
                {
                    fileInodeId = 1,
                    fileName = "..",
                    fileExtension = ""
                };

                for (int i = 2; i < _rootDirectory.Length; i++)
                {
                    _rootDirectory[i] = new RootDirectoryRecord(FREE_INODE_ID);
                }
            }

            public RootDirectoryRecord this[byte rootDirectoryRecordIndex]
            {
                get
                {
                    return _rootDirectory[rootDirectoryRecordIndex];
                }
            }

            public byte[] GetBytes()
            {
                int sizeOfRootDirectoryRecord = Marshal.SizeOf<RootDirectoryRecord>();
                int sizeOfInt = sizeof(int);
                byte[] data = new byte[sizeof(int) + _rootDirectory.Length * sizeOfRootDirectoryRecord + _emptySpace.Length];
                byte[] tmp;
                GCHandle gcHandle;

                tmp = BitConverter.GetBytes(_reserved);
                for (int i = 0; i < sizeOfInt; i++)
                {
                    data[i] = tmp[i];
                }

                for (int i = 0; i < _rootDirectory.Length; i++)
                {
                    tmp = new byte[sizeOfRootDirectoryRecord];
                    gcHandle = GCHandle.Alloc(tmp, GCHandleType.Pinned);
                    Marshal.StructureToPtr(_rootDirectory[i], gcHandle.AddrOfPinnedObject(), true);

                    for (int j = 0; j < sizeOfRootDirectoryRecord; j++)
                    {
                        data[sizeof(int) + i * sizeOfRootDirectoryRecord + j] = tmp[j];
                    }

                    gcHandle.Free();
                }

                for (int i = 0; i < _emptySpace.Length; i++)
                {
                    data[_rootDirectory.Length * sizeOfRootDirectoryRecord + i] = _emptySpace[i];
                }

                return data;
            }

            public int GetLength()
            {
                int sizeOfRootDirectoryRecord = Marshal.SizeOf<RootDirectoryRecord>();
                return sizeof(int) + _rootDirectory.Length * sizeOfRootDirectoryRecord + _emptySpace.Length;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct FileDataCluster
        {
            /// <summary>
            /// Номер следующего кластера данных файла. 0xFFFFFFFF для пометки текущего кластера последним.
            /// </summary>
            public int nextFileDataClusterIndex;
            /// <summary>
            /// Массив данных кластера.
            /// </summary>
            byte[] fileData;

            public FileDataCluster(short fileDataLength)
            {
                nextFileDataClusterIndex = -1;
                fileData = new byte[fileDataLength];
            }

            public byte[] GetBytes()
            {
                byte[] data = new byte[fileData.Length + 4];
                byte[] tmp;

                tmp = BitConverter.GetBytes(nextFileDataClusterIndex);
                data[0] = tmp[0];
                data[1] = tmp[1];
                data[2] = tmp[2];
                data[3] = tmp[3];

                for (int i = 0; i < fileData.Length; i++)
                {
                    data[4 + i] = fileData[i];
                }

                return data;
            }

            public int GetLength()
            {
                return fileData.Length + 4;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct DirectoryDataCluster
        {
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
            public struct DirectoryRecord
            {
                public int fileInodeId;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = FILE_NAME_LENGTH)]
                public string fileName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = FILE_EXTENSION_LENGTH)]
                public string fileExtension;

                public DirectoryRecord(int freeInodeId/*byte fileNameLength, byte fileExtensionLength*/)
                {
                    fileInodeId = freeInodeId;
                    fileName = "";
                    fileExtension = "";
                }

                //public byte[] GetBytes()
                //{
                //    byte[] data = new byte[GetLength()];
                //    byte[] tmp;

                //    tmp = BitConverter.GetBytes(fileInodeId);
                //    data[0] = tmp[0];
                //    data[1] = tmp[1];
                //    data[2] = tmp[2];
                //    data[3] = tmp[3];

                //    for (int i = 0; i < fileName.Length; i++)
                //    {
                //        data[4 + i] = (byte)fileName[i];
                //    }

                //    for (int i = 0; i < fileExtension.Length; i++)
                //    {
                //        data[19 + i] = (byte)fileExtension[i];
                //    }

                //    return data;
                //}

                //public static int GetLength()
                //{
                //    return 24;
                //}
            }

            /// <summary>
            /// Номер следующего кластера данных файла. 0xFFFF для пометки текущего кластера последним.
            /// </summary>
            public int nextFileDataClusterIndex;
            /// <summary>
            /// Записи каталога.
            /// </summary>
            DirectoryRecord[] _directoryRecords;

            public DirectoryDataCluster(byte numDirectoryRecords)
            {
                nextFileDataClusterIndex = -1;
                _directoryRecords = new DirectoryRecord[numDirectoryRecords];

                for (int i = 0; i < _directoryRecords.Length; i++)
                {
                    _directoryRecords[i] = new DirectoryRecord(FREE_INODE_ID);
                }
            }

            public DirectoryRecord this[byte directoryRecordIndex]
            {
                get
                {
                    return _directoryRecords[directoryRecordIndex];
                }
            }

            public byte[] GetBytes()
            {
                int sizeOfDirectoryRecord = Marshal.SizeOf<DirectoryRecord>();
                byte[] data = new byte[4 + _directoryRecords.Length * sizeOfDirectoryRecord];
                byte[] tmp;
                GCHandle gcHandle;

                tmp = BitConverter.GetBytes(nextFileDataClusterIndex);
                data[0] = tmp[0];
                data[1] = tmp[1];
                data[2] = tmp[2];
                data[3] = tmp[3];

                for (int i = 0; i < _directoryRecords.Length; i++)
                {
                    tmp = new byte[sizeOfDirectoryRecord];
                    gcHandle = GCHandle.Alloc(tmp, GCHandleType.Pinned);
                    Marshal.StructureToPtr(_directoryRecords[i], gcHandle.AddrOfPinnedObject(), true);

                    for (int j = 0; j < sizeOfDirectoryRecord; j++)
                    {
                        data[4 + i * sizeOfDirectoryRecord + j] = tmp[j];
                    }

                    gcHandle.Free();
                }

                return data;
            }

            public int GetLength()
            {
                int sizeOfDirectoryRecord = Marshal.SizeOf<DirectoryRecord>();
                return 4 + _directoryRecords.Length * sizeOfDirectoryRecord;
            }
        }

        #region Данные файла-образа
        private Superblock _superblock;
        private BitMap _bitMap;
        private Inodes _inodes;
        private RootDirectory _rootDirectory;
        private byte[] _emptyData;
        #endregion

        /// <summary>
        /// Создаёт файл-образ раздела жёсткого диска с файловой системой DehaxFS с параметрами:
        /// • размер раздела: 100 Мб;
        /// • размер кластера: 4 сектора (обычно 2048 байт).
        /// </summary>
        public DFSImage()
            : this(100 * 1024 * 1024, 4)
        {

        }

        /// <summary>
        /// Создаёт файл-образ раздела жёсткого диска с файловой системой DehaxFS с параметрами.
        /// </summary>
        /// <param name="diskPartitionSize">Размер раздела</param>
        /// <param name="diskClusterFactor">Размер кластера, множитель секторов диска</param>
        public DFSImage(long diskPartitionSize, byte diskClusterFactor)
        {
            diskClusterFactor = (byte)Math.Pow(2, diskClusterFactor - 1);

            _diskPartitionSize = diskPartitionSize;
            _diskClusterFactor = diskClusterFactor;

            _diskClusterSize = DISK_BYTES_PER_SECTOR * diskClusterFactor;


            _bitMap = new BitMap((int)(_diskPartitionSize / _diskClusterSize));
            _inodes = new Inodes((int)(_diskPartitionSize / _diskClusterSize));
            _rootDirectory = new RootDirectory(_diskClusterSize);

            _emptyData = new byte[_diskPartitionSize - Marshal.SizeOf<Superblock>() - _bitMap.GetLength() - _inodes.GetLength() - _rootDirectory.GetLength()];

            _superblock = new Superblock()
            {
                filesystemType = 0x28,
                numClusters = (int)(_diskPartitionSize / _diskClusterSize),
                clusterFactor = diskClusterFactor,
                inodeArraySize = _inodes.GetLength(),
                bitMapSize = _bitMap.GetLength(),
                numFreeClusters = _emptyData.Length / _diskClusterSize,
                numFreeInode = (int)(_diskPartitionSize / _diskClusterSize - 1)
            };

            InitializeFileSystem();
        }

        private void InitializeFileSystem()
        {
            Inodes.Inode inode = new Inodes.Inode()
            {
                fileType = 0x02, // Каталог
                inodeId = 1, // root-каталог
                userId = 1, // root пользователь
                groupId = 1, // root группа
                permissions = 0x01FD, // rwx|rwx|r-x
                attributes = 0x2, // Системный
                fileSize = _diskClusterSize,
                datetimeFileCreated = Utils.GetTimestamp(),
                datetimeFileModified = Utils.GetTimestamp(),
                datetimeInodeModified = Utils.GetTimestamp(),
                firstClusterIndex = 0x0 // Корневой каталог
            };

            _inodes[0] = inode;

            for (int i = 1; i < _inodes.GetCount(); i++)
            {
                inode = new Inodes.Inode()
                {
                    fileType = 0xFF,
                    inodeId = i + 1,
                    datetimeInodeModified = Utils.GetTimestamp()
                };

                _inodes[i] = inode;
            }

            _superblock.bitMapAddress = Marshal.SizeOf<Superblock>();
            _superblock.inodeArrayAddress = _superblock.bitMapAddress + _bitMap.GetLength();
            _superblock.rootAddress = _superblock.inodeArrayAddress + _inodes.GetLength();
            _superblock.dataAddress = _superblock.rootAddress + _rootDirectory.GetLength();
        }

        public void ExportToFile(string path)
        {
            FileInfo imageFile = new FileInfo(path);

            using (BinaryWriter bw = new BinaryWriter(imageFile.Create(), Encoding.ASCII))
            {
                //bw.Write(_superblock.GetBytes());
                Utils.WriteStruct(bw, _superblock);
                //Utils.WriteStruct(bw, _bitMap);
                bw.Write(_bitMap.GetBytes());
                //Utils.WriteStruct(bw, _inodes);
                bw.Write(_inodes.GetBytes());
                //Utils.WriteStruct(bw, _rootDirectory);
                bw.Write(_rootDirectory.GetBytes());


                bw.Write(_emptyData);
            }
        }
    }
}
