using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DehaxOS.FileSystem
{
    abstract class MetaFile
    {
        public const char EXTENSION_PREFIX = '.';

        /// <summary>
        /// Адрес первого блока данных на диске.
        /// </summary>
        public long StreamAddress { get; set; }
        public long DiskRecordAddress { get; set; }
        public uint InodeId { get; set; }
        public uint UserId { get; set; }
        public uint GroupId { get; set; }
        public AccessRights AccessRights { get; set; }
        public Attributes Attributes { get; set; }
        public uint Size { get; set; }
        public long CreationTime { get; set; }
        public long ModificationTime { get; set; }
        public long InodeModificationTime { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }

        public string FullName
        {
            get
            {
                return Name + EXTENSION_PREFIX + Extension;
            }
            set
            {
                int index = value.LastIndexOf(EXTENSION_PREFIX);
                if (index < 0)
                {
                    Name = value;
                    Extension = string.Empty;
                }
                else
                {
                    Name = value.Substring(0, index);
                    index++;
                    Extension = value.Substring(index, value.Length - index);
                }
            }
        }
    }
}
