﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DehaxOS.FileSystem
{
    class Directory : MetaFile
    {
        List<MetaFile> records;
        string _fullPathToDirectory;

        public string FullPath
        {
            get
            {
                return _fullPathToDirectory;
            }
            set
            {
                _fullPathToDirectory = value;
            }
        }

        public int Count
        {
            get
            {
                return records.Count;
            }
        }

        public Directory()
        {
            records = new List<MetaFile>();
            FullPath = "";
            // NOTE: Только логическая структура, без служебных записей?
            //Directory current = new Directory();
            //current.Name = ".";
            //Directory parent = new Directory();
            //parent.Name = "..";
            //records.Add()
        }

        public void AddRecord(MetaFile record)
        {
            records.Add(record);
        }

        public void ClearRecords()
        {
            records.Clear();
        }

        public MetaFile this[int index]
        {
            get
            {
                return records[index];
            }
        }

        public MetaFile Find(string fullName)
        {
            int index = records.FindIndex(
                delegate (MetaFile metaFile)
                {
                    return metaFile.FullName == fullName;
                }
            );

            if (index < 0)
            {
                return null;
            }

            return records[index];
        }
    }
}