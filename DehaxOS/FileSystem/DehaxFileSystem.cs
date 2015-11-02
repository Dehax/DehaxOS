using System;
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
        struct RightsGroup
        {
            bool canRead;
            bool canWrite;
            bool canExecute;
        }

        RightsGroup user;
        RightsGroup group;
        RightsGroup others;
    }

    struct Attributes
    {
        bool hidden;
        bool system;
        bool readOnly;
    }

    class MetaFile
    {
        uint inodeId;
        uint userId;
        uint groupId;
        AccessRights accessRights;
        Attributes attributes;
        uint size;
        long creationTime;
        long modificationTime;
        long inodeModificationTime;
    }

    class File : MetaFile
    {
        byte[] data;
    }

    class Directory : MetaFile
    {
        List<MetaFile> records;
    }

    class DehaxFileSystem
    {
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
        #endregion

        private FileStream FileStream { get; set; }
        private Superblock _superblock;

        public DehaxFileSystem(FileStream fileStream)
        {
            FileStream = fileStream;

            Initialize();
        }

        private void Initialize()
        {
            _superblock = (Superblock)ReadStruct(FileStream, typeof(Superblock));
        }

        /// <summary>
        /// Создаёт новый пустой каталог.
        /// </summary>
        /// <returns>созданную директорию</returns>
        public Directory CreateDirectory(string path)
        {
            Directory directory = new Directory();
            
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
    }
}
