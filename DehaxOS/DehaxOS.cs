﻿using DehaxOS.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DehaxOS
{
    class DehaxOS
    {
        #region Константы

        #region Файловая система
        private const int FS_IMAGE_BUFFER = 512;
#if DEBUG
        private const string FS_IMAGE_PATH = @"../../../image.dfs";
#else
        private const string FS_IMAGE_PATH = @"image.dfs";
#endif
        private const string SYSTEM_DIRECTORY_PATH = @"/dehaxos";
        private const string USERS_FILE_PATH = @"/dehaxos/users";
        private const string GROUPS_FILE_PATH = @"/dehaxos/groups";
        #endregion

        #region Root-пользователь (суперадминистратор)
        private const string ROOT_USER_NAME = "root";
        private const short ROOT_USER_ID = 1;
        private const string ROOT_GROUP_NAME = "root";
        private const short ROOT_GROUP_ID = 1;
        /// <summary>
        /// MD5-хеш пароля root-пользователя: "toor".
        /// </summary>
        private readonly byte[] ROOT_USER_PASSWORD_HASH = new byte[] { 0x7b, 0x24, 0xaf, 0xc8, 0xbc, 0x80, 0xe5, 0x48, 0xd6, 0x6c, 0x4e, 0x7f, 0xf7, 0x21, 0x71, 0xc5 };
        #endregion

        #endregion

        private DehaxFileSystem FileSystem { get; set; }
        private FileStream _fileSystemImage;

        private UsersManager _usersManager;
        private GroupsManager _groupsManager;

        private bool IsLoggedIn { get; set; }
        private User _user;
        private User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;

                if (FileSystem != null)
                {
                    FileSystem.UserId = value.userId;
                    FileSystem.GroupId = value.groupId;
                }
            }
        }

        /// <summary>
        /// Проверяет, впервые ли запускается система. Пытается найти системный каталог в корне файловой системы.
        /// </summary>
        private bool IsFirstRun
        {
            get
            {
                try
                {
                    FileSystem.OpenDirectory(SYSTEM_DIRECTORY_PATH);
                }
                catch (DirectoryNotFoundException)
                {
                    return true;
                }

                return false;
            }
        }

        public DehaxOS()
        {
            _fileSystemImage = new FileStream(FS_IMAGE_PATH, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, FS_IMAGE_BUFFER, FileOptions.RandomAccess);
            FileSystem = new DehaxFileSystem(_fileSystemImage, ROOT_USER_ID, ROOT_GROUP_ID);

            _usersManager = new UsersManager();
            _groupsManager = new GroupsManager();

            IsLoggedIn = false;

            if (IsFirstRun)
            {
                User rootUser = new User(ROOT_USER_NAME, ROOT_USER_PASSWORD_HASH, ROOT_USER_ID, ROOT_GROUP_ID);
                _usersManager.AddUser(rootUser);
                Group rootGroup = new Group(ROOT_GROUP_NAME, ROOT_GROUP_ID);
                rootGroup.AddUser(rootUser);
                _groupsManager.AddGroup(rootGroup);

                User = rootUser;
                IsLoggedIn = true;

                Install();
            }
            else
            {
                Initialize();
            }
        }

        ~DehaxOS()
        {
            _fileSystemImage.Close();
        }

        /// <summary>
        /// Устанавливает ОС, подготавливает систему для первого запуска.
        /// Копирует необходимые системные файлы на жёсткий диск.
        /// </summary>
        private void Install()
        {
            FileSystem.CreateDirectory(SYSTEM_DIRECTORY_PATH);
            FileSystem.SetAttributes(SYSTEM_DIRECTORY_PATH, new Attributes()
            {
                system = true
            });
            FileSystem.CreateFile(USERS_FILE_PATH);
            FileSystem.SetAttributes(USERS_FILE_PATH, new Attributes()
            {
                system = true
            });
            // Заполнить файл пользователей одним стандартным root-пользователем:
            // файл пользователей содержит набор записей, разделяемых символом '\n'
            // каждая запись состоит из полей [uid - уникальный код пользователя], [имя пользователя - уникально], [хэш пароля], [gid - код группы - FK], разделённых пробелом
            // если число uid начинается со знака '-', то данный пользователь был удалён
            // файл должен также завершаться символом '\n'
            byte[] usersFileData;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _usersManager.Count; i++)
            {
                User user = _usersManager[i];

                sb.Append(user.userId);
                sb.Append(' ');
                sb.Append(user.userName);
                sb.Append(' ');
                sb.Append(Utils.ByteArrayToHexString(user.passwordHash));
                sb.Append(' ');
                sb.Append(user.groupId);
                sb.Append('\n');
            }

            usersFileData = Encoding.ASCII.GetBytes(sb.ToString());

            FileSystem.WriteFile(USERS_FILE_PATH, usersFileData);

            // Создать файл групп и заполнить группу root-пользователей
            FileSystem.CreateFile(GROUPS_FILE_PATH);
            FileSystem.SetAttributes(GROUPS_FILE_PATH, new Attributes()
            {
                system = true
            });
            // файл групп состоит из записей, разделённых символом '\n'
            // каждая запись состоит из полей [gid - уникальный код группы], [имя группы - уникально]
            // если число gid начинается со знака '-', то данная группа была удалена
            // файл должен также завершаться символом '\n'
            byte[] groupsFileData;
            sb = new StringBuilder();
            for (int i = 0; i < _groupsManager.Count; i++)
            {
                Group group = _groupsManager[i];

                sb.Append(group.groupId);
                sb.Append(' ');
                sb.Append(group.groupName);
                sb.Append('\n');
            }

            groupsFileData = Encoding.ASCII.GetBytes(sb.ToString());

            FileSystem.WriteFile(GROUPS_FILE_PATH, groupsFileData);
        }

        /// <summary>
        /// Инициализирует ОС при запуске, загружает в ОЗУ системные структуры.
        /// </summary>
        private void Initialize()
        {
            byte[] usersFileData = FileSystem.ReadFile(USERS_FILE_PATH);
            byte[] groupsFileData = FileSystem.ReadFile(GROUPS_FILE_PATH);

            string usersFileString = Encoding.ASCII.GetString(usersFileData);
            string groupsFileString = Encoding.ASCII.GetString(groupsFileData);

            // Считываем группы.
            string[] groupsRecords = groupsFileString.Split('\n');
            for (int i = 0; i < groupsRecords.Length - 1; i++)
            {
                string[] groupFields = groupsRecords[i].Split(' ');
                short groupId = short.Parse(groupFields[0]);
                string groupName = groupFields[1];

                Group group = new Group(groupName, groupId);
                if (groupId < 0)
                {
                    groupId *= -1;
                    group.deleted = true;
                }

                _groupsManager.AddGroup(group);
            }

            // Считываем пользователей.
            string[] usersRecords = usersFileString.Split('\n');
            for (int i = 0; i < usersRecords.Length - 1; i++)
            {
                string[] userFields = usersRecords[i].Split(' ');
                short userId = short.Parse(userFields[0]);
                string userName = userFields[1];
                byte[] passwordHash = Utils.HexStringToByteArray(userFields[2]);
                short groupId = short.Parse(userFields[3]);

                User user = new User(userName, passwordHash, userId, groupId);
                if (userId < 0)
                {
                    userId *= -1;
                    user.deleted = true;
                }

                _usersManager.AddUser(user);
                _groupsManager.AddUserToGroup(user.groupId, user);
            }
        }

        /// <summary>
        /// Выполняет вход в систему.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>true - если вход выполнен успешно; false - в противном случае.</returns>
        public bool Login(string username, string password)
        {
            for (int i = 0; i < _usersManager.Count; i++)
            {
                User user = _usersManager[i];
                if (!user.deleted && user.userName == username)
                {
                    byte[] firstHash = user.passwordHash;
                    byte[] secondHash = Utils.GetPasswordHash(password);

                    User = user;
                    IsLoggedIn = true;

                    return firstHash.SequenceEqual(secondHash);
                }
            }

            return false;
        }
    }
}