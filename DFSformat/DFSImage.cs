using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DFSformat
{
    class DFSImage
    {
        /// <summary>
        /// Количество байт, занимаемых одним сектором жёсткого диска
        /// </summary>
        private const ushort DISK_BYTES_PER_SECTOR = 512;
        //private const uint BITMAP_NUM_BITS = 102400;
        //private const uint NUM_INODES = 2048000;
        //private const byte NUM_ROOT_DIRECTORY_RECORDS = 93;

        /// <summary>
        /// Длина поля имени файла в записи каталога
        /// </summary>
        private const byte FILE_NAME_LENGTH = 15;
        /// <summary>
        /// Длина поля расширения файла в записи каталога
        /// </summary>
        private const byte FILE_EXTENSION_LENGTH = 5;

        private const ushort FIRST_INODE_TYPE_ADDRESS = 12814;

        private readonly ulong _diskPartitionSize;
        private readonly byte _diskClusterFactor;
        public readonly uint _diskClusterSize;

        struct Superblock
        {
            public byte filesystemType;
            public uint numClusters;
            public byte clusterFactor;
            public uint inodeArraySize;
            public uint bitMapSize;
            uint numFreeClusters;
            public uint numFreeInode;

            public void SetNumFreeClusters(ushort numFreeClusters)
            {
                this.numFreeClusters = numFreeClusters;
            }

            public byte[] GetBytes()
            {
                byte[] data = new byte[GetLength()];
                byte[] tmp;

                data[0] = filesystemType;

                tmp = BitConverter.GetBytes(numClusters);
                data[1] = tmp[0];
                data[2] = tmp[1];
                data[3] = tmp[2];
                data[4] = tmp[3];

                data[5] = clusterFactor;

                tmp = BitConverter.GetBytes(inodeArraySize);
                data[6] = tmp[0];
                data[7] = tmp[1];
                data[8] = tmp[2];
                data[9] = tmp[3];

                tmp = BitConverter.GetBytes(bitMapSize);
                data[10] = tmp[0];
                data[11] = tmp[1];
                data[12] = tmp[2];
                data[13] = tmp[3];

                tmp = BitConverter.GetBytes(numFreeClusters);
                data[14] = tmp[0];
                data[15] = tmp[1];
                data[16] = tmp[2];
                data[17] = tmp[3];

                tmp = BitConverter.GetBytes(numFreeInode);
                data[18] = tmp[0];
                data[19] = tmp[1];
                data[20] = tmp[2];
                data[21] = tmp[3];

                return data;
            }

            public static uint GetLength()
            {
                return 22;
            }
        }

        struct BitMap
        {
            private BitArray _bits;

            public BitMap(uint bitmapNumClusters)
            {
                _bits = new BitArray((int)bitmapNumClusters * 2);
            }

            public byte this[uint clusterIndex]
            {
                get
                {
                    int firstBitIndex = (int)clusterIndex * 2;
                    bool firstBit = _bits[firstBitIndex];
                    bool secondBit = _bits[firstBitIndex + 1];

                    return (byte)(Convert.ToByte(firstBit) * 2 + Convert.ToByte(secondBit));
                }
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

            public uint GetLength()
            {
                return (uint)_bits.Length / 8;
            }
        }

        struct Inodes
        {
            public struct Inode
            {
                byte fileType;
                uint inodeId;
                ushort userId;
                ushort groupId;
                ushort permissions;
                byte attributes;
                uint fileSize;
                ulong datetimeFileCreated;
                ulong datetimeFileModified;
                ulong datetimeInodeModified;
                uint firstClusterIndex;

                public byte[] GetBytes()
                {
                    byte[] data = new byte[GetLength()];
                    byte[] tmp;

                    data[0] = fileType;

                    tmp = BitConverter.GetBytes(inodeId);
                    data[1] = tmp[0];
                    data[2] = tmp[1];
                    data[3] = tmp[2];
                    data[4] = tmp[3];

                    tmp = BitConverter.GetBytes(userId);
                    data[5] = tmp[0];
                    data[6] = tmp[1];

                    tmp = BitConverter.GetBytes(groupId);
                    data[7] = tmp[0];
                    data[8] = tmp[1];

                    tmp = BitConverter.GetBytes(permissions);
                    data[9] = tmp[0];
                    data[10] = tmp[1];

                    data[11] = attributes;

                    tmp = BitConverter.GetBytes(fileSize);
                    data[12] = tmp[0];
                    data[13] = tmp[1];
                    data[14] = tmp[2];
                    data[15] = tmp[3];

                    tmp = BitConverter.GetBytes(datetimeFileCreated);
                    data[16] = tmp[0];
                    data[17] = tmp[1];
                    data[18] = tmp[2];
                    data[19] = tmp[3];
                    data[20] = tmp[4];
                    data[21] = tmp[5];
                    data[22] = tmp[6];
                    data[23] = tmp[7];

                    tmp = BitConverter.GetBytes(datetimeFileModified);
                    data[24] = tmp[0];
                    data[25] = tmp[1];
                    data[26] = tmp[2];
                    data[27] = tmp[3];
                    data[28] = tmp[4];
                    data[29] = tmp[5];
                    data[30] = tmp[6];
                    data[31] = tmp[7];

                    tmp = BitConverter.GetBytes(datetimeInodeModified);
                    data[32] = tmp[0];
                    data[33] = tmp[1];
                    data[34] = tmp[2];
                    data[35] = tmp[3];
                    data[36] = tmp[4];
                    data[37] = tmp[5];
                    data[38] = tmp[6];
                    data[39] = tmp[7];

                    tmp = BitConverter.GetBytes(firstClusterIndex);
                    data[40] = tmp[0];
                    data[41] = tmp[1];
                    data[42] = tmp[2];
                    data[43] = tmp[3];

                    return data;
                }

                public static uint GetLength()
                {
                    return 44;
                }
            }

            private Inode[] _inodes;

            public Inodes(uint numInodes)
            {
                _inodes = new Inode[numInodes];
            }

            public Inode this[uint inodeIndex]
            {
                get
                {
                    return _inodes[inodeIndex];
                }
            }

            public byte[] GetBytes()
            {
                uint inodeLength = Inode.GetLength();
                byte[] data = new byte[_inodes.Length * inodeLength];
                byte[] tmp;
                
                for (int i = 0; i < _inodes.Length; i++)
                {
                    tmp = _inodes[i].GetBytes();

                    for (int j = 0; j < inodeLength; j++)
                    {
                        data[i * inodeLength + j] = tmp[j];
                    }
                }

                return data;
            }

            public uint GetLength()
            {
                return (uint)_inodes.Length * Inode.GetLength();
            }
        }

        struct RootDirectory
        {
            public struct RootDirectoryRecord
            {
                uint fileInodeId;
                char[] fileName;
                char[] fileExtension;

                public RootDirectoryRecord(byte fileNameLength, byte fileExtensionLength)
                {
                    fileInodeId = 0xFFFF;
                    fileName = new char[fileNameLength];
                    fileExtension = new char[fileExtensionLength];
                }

                public void SetFullFileName(char[] fileName, char[] fileExtension)
                {
                    this.fileName = fileName;
                    this.fileExtension = fileExtension;
                }

                public byte[] GetBytes()
                {
                    byte[] data = new byte[GetLength()];
                    byte[] tmp;

                    tmp = BitConverter.GetBytes(fileInodeId);
                    data[0] = tmp[0];
                    data[1] = tmp[1];
                    data[2] = tmp[2];
                    data[3] = tmp[3];

                    for (int i = 0; i < fileName.Length; i++)
                    {
                        data[4 + i] = (byte)fileName[i];
                    }

                    for (int i = 0; i < fileExtension.Length; i++)
                    {
                        data[19 + i] = (byte)fileExtension[i];
                    }

                    return data;
                }

                public static uint GetLength()
                {
                    return 24;
                }
            }

            RootDirectoryRecord[] _rootDirectory;
            byte[] _emptySpace;

            public RootDirectory(uint clusterSize)
            {
                _rootDirectory = new RootDirectoryRecord[clusterSize / RootDirectoryRecord.GetLength()];
                _emptySpace = new byte[clusterSize - (clusterSize / RootDirectoryRecord.GetLength()) * RootDirectoryRecord.GetLength()];

                for (int i = 0; i < _rootDirectory.Length; i++)
                {
                    _rootDirectory[i] = new RootDirectoryRecord(FILE_NAME_LENGTH, FILE_EXTENSION_LENGTH);
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
                byte[] data = new byte[_rootDirectory.Length * RootDirectoryRecord.GetLength() + _emptySpace.Length];
                byte[] tmp;

                for (int i = 0; i < _rootDirectory.Length; i++)
                {
                    tmp = _rootDirectory[i].GetBytes();

                    for (int j = 0; j < RootDirectoryRecord.GetLength(); j++)
                    {
                        data[i * RootDirectoryRecord.GetLength() + j] = tmp[j];
                    }
                }

                for (int i = 0; i < _emptySpace.Length; i++)
                {
                    data[_rootDirectory.Length * RootDirectoryRecord.GetLength()] = _emptySpace[i];
                }

                return data;
            }

            public uint GetLength()
            {
                return (uint)(_rootDirectory.Length * RootDirectoryRecord.GetLength() + _emptySpace.Length);
            }
        }

        struct FileDataCluster
        {
            // Номер следующего кластера данных файла. 0xFFFFFFFF для пометки текущего кластера последним.
            uint nextFileDataClusterIndex;
            // Массив данных кластера.
            char[] fileData;

            public FileDataCluster(ushort fileDataLength)
            {
                nextFileDataClusterIndex = 0xFFFFFFFF;
                fileData = new char[fileDataLength];
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
                    data[4 + i] = (byte)fileData[i];
                }

                return data;
            }

            public uint GetLength()
            {
                return (uint)(fileData.Length + 4);
            }
        }

        struct DirectoryDataCluster
        {
            public struct DirectoryRecord
            {
                uint fileInodeId;
                char[] fileName;
                char[] fileExtension;

                public DirectoryRecord(byte fileNameLength, byte fileExtensionLength)
                {
                    fileInodeId = 0xFFFFFFFF;
                    fileName = new char[fileNameLength];
                    fileExtension = new char[fileExtensionLength];
                }

                public byte[] GetBytes()
                {
                    byte[] data = new byte[GetLength()];
                    byte[] tmp;

                    tmp = BitConverter.GetBytes(fileInodeId);
                    data[0] = tmp[0];
                    data[1] = tmp[1];
                    data[2] = tmp[2];
                    data[3] = tmp[3];

                    for (int i = 0; i < fileName.Length; i++)
                    {
                        data[4 + i] = (byte)fileName[i];
                    }

                    for (int i = 0; i < fileExtension.Length; i++)
                    {
                        data[19 + i] = (byte)fileExtension[i];
                    }

                    return data;
                }

                public static uint GetLength()
                {
                    return 24;
                }
            }

            // Номер следующего кластера данных файла. 0xFFFF для пометки текущего кластера последним.
            uint nextFileDataClusterIndex;
            // Записи каталога
            DirectoryRecord[] _directoryRecords;

            public DirectoryDataCluster(byte numDirectoryRecords)
            {
                nextFileDataClusterIndex = 0xFFFFFFFF;
                _directoryRecords = new DirectoryRecord[numDirectoryRecords];

                for (int i = 0; i < _directoryRecords.Length; i++)
                {
                    _directoryRecords[i] = new DirectoryRecord(FILE_NAME_LENGTH, FILE_EXTENSION_LENGTH);
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
                byte[] data = new byte[4 + _directoryRecords.Length * DirectoryRecord.GetLength()];
                byte[] tmp;

                tmp = BitConverter.GetBytes(nextFileDataClusterIndex);
                data[0] = tmp[0];
                data[1] = tmp[1];
                data[2] = tmp[2];
                data[3] = tmp[3];

                for (int i = 0; i < _directoryRecords.Length; i++)
                {
                    tmp = _directoryRecords[i].GetBytes();

                    for (int j = 0; j < DirectoryRecord.GetLength(); j++)
                    {
                        data[4 + i * DirectoryRecord.GetLength() + j] = tmp[j];
                    }
                }

                return data;
            }

            public uint GetLength()
            {
                return (uint)(4 + _directoryRecords.Length * DirectoryRecord.GetLength());
            }
        }

        #region Данные файла-образа
        private Superblock _superblock;
        private BitMap _bitMap;
        private Inodes _inodes;
        private RootDirectory _rootDirectory;
        private byte[] emptyData;
        #endregion

        /// <summary>
        /// Создаёт файл-образ раздела жёсткого диска с файловой системой DehaxFS с параметрами:
        /// — размер раздела: 100 Мб;
        /// — размер кластера: 4 сектора (обычно 2048 байт).
        /// </summary>
        public DFSImage()
            : this(100 * 1024 * 1024, 4)
        {

        }

        public DFSImage(ulong diskPartitionSize, byte diskClusterFactor)
        {
            diskClusterFactor = (byte)Math.Pow(2, diskClusterFactor - 1);

            _diskPartitionSize = diskPartitionSize;
            _diskClusterFactor = diskClusterFactor;

            _diskClusterSize = (uint)DISK_BYTES_PER_SECTOR * diskClusterFactor;

            
            _bitMap = new BitMap((uint)(_diskPartitionSize / _diskClusterSize));
            _inodes = new Inodes((uint)(_diskPartitionSize / _diskClusterSize));
            _rootDirectory = new RootDirectory(_diskClusterSize);

            _superblock = new Superblock()
            {
                filesystemType = 0x28,
                numClusters = (ushort)(_diskPartitionSize / _diskClusterSize),
                clusterFactor = diskClusterFactor,
                inodeArraySize = _inodes.GetLength(),
                bitMapSize = (ushort)_bitMap.GetLength(),
                numFreeInode = (ushort)(_diskPartitionSize / _diskClusterSize)
            };

            emptyData = new byte[_diskPartitionSize - Superblock.GetLength() - _bitMap.GetLength() - _inodes.GetLength() - _rootDirectory.GetLength()];

            _superblock.SetNumFreeClusters((ushort)(emptyData.Length / _diskClusterSize));
        }

        private void InitializeRootDirectory(BinaryWriter bw)
        {
            byte free = 0xFF;

            bw.Seek(FIRST_INODE_TYPE_ADDRESS, SeekOrigin.Begin);

            bw.Write(free);

            int offset = (int)Inodes.Inode.GetLength();
            int inodesLength = (int)_inodes.GetLength();

            for (int i = 1; i < inodesLength; i++)
            {
                bw.Seek(offset, SeekOrigin.Current);
                bw.Write(free);
            }
        }

        public void ExportToFile(string path)
        {
            FileInfo imageFile = new FileInfo(path);

            using (BinaryWriter bw = new BinaryWriter(imageFile.Create(), Encoding.ASCII))
            {
                bw.Write(_superblock.GetBytes());
                bw.Write(_bitMap.GetBytes());
                bw.Write(_inodes.GetBytes());
                bw.Write(_rootDirectory.GetBytes());
                bw.Write(emptyData);

                InitializeRootDirectory(bw);
            }
        }
    }
}
