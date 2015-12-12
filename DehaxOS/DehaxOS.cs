using DehaxOS.FileSystem;
using DehaxOS.Scheduler;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace DehaxOS
{
    /// <summary>
    /// Представляет ядро операционной системы DehaxOS.
    /// </summary>
    public class DehaxOS : IDisposable
    {
        #region Константы

        #region Файловая система
        private const int FS_IMAGE_BUFFER = 512;
#if DEBUG
        public const string FS_IMAGE_PATH = @"..\..\..\image.dfs";
#else
        /// <summary>
        /// Путь к образу диска с файловой системой DehaxFS.
        /// </summary>
        public const string FS_IMAGE_PATH = @"image.dfs";
#endif
        /// <summary>
        /// Путь в файловой системе к системному каталогу.
        /// </summary>
        public const string SYSTEM_DIRECTORY_PATH = @"/dehaxos";
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

        readonly string NEW_LINE = Environment.NewLine;

        #endregion

        private DehaxFileSystem FileSystem { get; set; }
        private FileStream _fileSystemImage;

        private DehaxScheduler Scheduler { get; set; }

        private UsersManager _usersManager;
        private GroupsManager _groupsManager;

        /// <summary>
        /// Менеджер пользователей.
        /// </summary>
        public UsersManager UsersManager
        {
            get
            {
                return _usersManager;
            }
        }
        /// <summary>
        /// Менеджер групп пользователей.
        /// </summary>
        public GroupsManager GroupsManager
        {
            get
            {
                return _groupsManager;
            }
        }

        private User ROOT_USER;
        /// <summary>
        /// Выполнен ли вход в систему.
        /// </summary>
        public bool IsLoggedIn { get; private set; }
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
        /// <summary>
        /// Имя текущего пользователя.
        /// </summary>
        public string UserName
        {
            get
            {
                return User.userName;
            }
        }
        /// <summary>
        /// ID текущего пользователя.
        /// </summary>
        public short UserId
        {
            get
            {
                return User.userId;
            }
        }
        /// <summary>
        /// ID группы текущего пользователя.
        /// </summary>
        public short GroupId
        {
            get
            {
                return User.groupId;
            }
        }

        private int _lastPID = 0;
        /// <summary>
        /// Следующий свободный PID.
        /// </summary>
        public int NextPID
        {
            get
            {
                _lastPID++;

                return _lastPID;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(NextPID), "Значение PID не может быть отрицательным!");
                }

                _lastPID = value - 1;
            }
        }

        /// <summary>
        /// Выполняет инициализацию ядра ОС.
        /// </summary>
        public DehaxOS()
        {
            _fileSystemImage = new FileStream(FS_IMAGE_PATH, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, FS_IMAGE_BUFFER, FileOptions.RandomAccess);
            FileSystem = new DehaxFileSystem(_fileSystemImage, ROOT_USER_ID, ROOT_GROUP_ID);

            Scheduler = new DehaxScheduler();

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

                ROOT_USER = rootUser;
                User = rootUser;
                IsLoggedIn = true;

                Install();

                IsLoggedIn = false;
            }
            else
            {
                Initialize();
            }
        }

        /// <summary>
        /// Выполняет служебные действия при завершении работы ядра ОС.
        /// </summary>
        ~DehaxOS()
        {
            _fileSystemImage.Close();
        }

        /// <summary>
        /// Вызывается при освобождении ресурсов.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)_fileSystemImage).Dispose();
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
            // файл пользователей содержит набор записей, разделяемых символом NEW_LINE
            // каждая запись состоит из полей [uid - уникальный код пользователя], [имя пользователя - уникально], [хэш пароля], [gid - код группы - FK], разделённых пробелом
            // если число uid начинается со знака '-', то данный пользователь был удалён
            // файл должен также завершаться символом NEW_LINE
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
                sb.Append(NEW_LINE);
            }

            usersFileData = Encoding.ASCII.GetBytes(sb.ToString());

            FileSystem.WriteFile(USERS_FILE_PATH, usersFileData);

            // Создать файл групп и заполнить группу root-пользователей
            FileSystem.CreateFile(GROUPS_FILE_PATH);
            FileSystem.SetAttributes(GROUPS_FILE_PATH, new Attributes()
            {
                system = true
            });
            // файл групп состоит из записей, разделённых символом NEW_LINE
            // каждая запись состоит из полей [gid - уникальный код группы], [имя группы - уникально]
            // если число gid начинается со знака '-', то данная группа была удалена
            // файл должен также завершаться символом NEW_LINE
            byte[] groupsFileData;
            sb = new StringBuilder();
            for (int i = 0; i < _groupsManager.Count; i++)
            {
                Group group = _groupsManager[i];

                sb.Append(group.groupId);
                sb.Append(' ');
                sb.Append(group.groupName);
                sb.Append(NEW_LINE);
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
            string[] groupsRecords = groupsFileString.Split(new string[] { NEW_LINE }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < groupsRecords.Length; i++)
            {
                string[] groupFields = groupsRecords[i].Split(' ');
                short groupId = short.Parse(groupFields[0]);
                string groupName = groupFields[1];

                Group group = new Group(groupName, groupId);
                if (groupId < 0)
                {
                    group.groupId *= -1;
                    group.deleted = true;
                }

                _groupsManager.AddGroup(group);
            }

            // Считываем пользователей.
            string[] usersRecords = usersFileString.Split(new string[] { NEW_LINE }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < usersRecords.Length; i++)
            {
                string[] userFields = usersRecords[i].Split(' ');
                short userId = short.Parse(userFields[0]);
                string userName = userFields[1];
                byte[] passwordHash = Utils.HexStringToByteArray(userFields[2]);
                short groupId = short.Parse(userFields[3]);

                User user = new User(userName, passwordHash, userId, groupId);
                if (userId < 0)
                {
                    user.userId *= -1;
                    user.deleted = true;
                }

                if (userId == ROOT_USER_ID && groupId == ROOT_GROUP_ID)
                {
                    ROOT_USER = user;
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

                    if (firstHash.SequenceEqual(secondHash))
                    {
                        User = user;
                        IsLoggedIn = true;

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Выполняет выход из системы.
        /// </summary>
        /// <returns>true в случае успешного выхода из системы и false, иначе</returns>
        public bool Logout()
        {
            // TODO: Написать процедуру выхода из системы (завершение работы, выключение компьютера).
            // Записать файл пользователей
            Flush();
            User = ROOT_USER;
            IsLoggedIn = false;

            return true;
        }

        /// <summary>
        /// Вызывает процедуру записи структур ОС на диск.
        /// </summary>
        public void Flush()
        {
            byte[] usersFileData;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _usersManager.Count; i++)
            {
                User user = _usersManager[i];

                sb.Append(user.deleted ? -user.userId : user.userId);
                sb.Append(' ');
                sb.Append(user.userName);
                sb.Append(' ');
                sb.Append(Utils.ByteArrayToHexString(user.passwordHash));
                sb.Append(' ');
                sb.Append(user.groupId);
                sb.Append(NEW_LINE);
            }

            usersFileData = Encoding.ASCII.GetBytes(sb.ToString());

            FileSystem.WriteFile(USERS_FILE_PATH, usersFileData);

            // Записать файл групп
            byte[] groupsFileData;
            sb = new StringBuilder();
            for (int i = 0; i < _groupsManager.Count; i++)
            {
                Group group = _groupsManager[i];

                sb.Append(group.deleted ? -group.groupId : group.groupId);
                sb.Append(' ');
                sb.Append(group.groupName);
                sb.Append(NEW_LINE);
            }

            groupsFileData = Encoding.ASCII.GetBytes(sb.ToString());

            FileSystem.WriteFile(GROUPS_FILE_PATH, groupsFileData);
        }

        #region Интерфейс файловой системы
        /// <summary>
        /// Создаёт новый пустой каталог.
        /// </summary>
        /// <param name="path">Путь к создаваемому каталогу.</param>
        public void CreateDirectory(string path)
        {
            FileSystem.CreateDirectory(path);
        }

        /// <summary>
        /// Создаёт новый пустой файл.
        /// </summary>
        /// <param name="path">Путь к создаваемому файлу.</param>
        public void CreateFile(string path)
        {
            FileSystem.CreateFile(path);
        }

        /// <summary>
        /// Открывает каталог для работы. Загружает в структуры ФС информацию о каталоге.
        /// </summary>
        /// <param name="path">Путь к каталогу для открытия.</param>
        public FileSystem.Directory OpenDirectory(string path)
        {
            return FileSystem.OpenDirectory(path);
        }

        /// <summary>
        /// Возвращает атрибуты файла или каталога.
        /// </summary>
        /// <param name="path">Путь к файлу или каталогу.</param>
        /// <returns>атрибуты файла или каталога</returns>
        public Attributes GetAttributes(string path)
        {
            return FileSystem.GetAttributes(path);
        }

        /// <summary>
        /// Задаёт указанные атрибуты файлу или каталогу.
        /// Не изменяет текущий рабочий каталог.
        /// </summary>
        /// <param name="path">Путь к файлу или каталогу.</param>
        /// <param name="attributes">Атрибуты, которые будут заданы.</param>
        public void SetAttributes(string path, Attributes attributes)
        {
            FileSystem.SetAttributes(path, attributes);
        }

        /// <summary>
        /// Возвращает права доступа к файлу или каталогу.
        /// </summary>
        /// <param name="path">Путь к файлу или каталогу.</param>
        /// <returns>права доступа к файлу или каталогу</returns>
        public AccessRights GetAccessRights(string path)
        {
            return FileSystem.GetAccessRights(path);
        }

        /// <summary>
        /// Назначает указанные права доступа файлу или каталогу.
        /// </summary>
        /// <param name="path">Путь к файлу или каталогу.</param>
        /// <param name="accessRights">Права доступа, которые будут назначены.</param>
        public void SetAccessRights(string path, AccessRights accessRights)
        {
            FileSystem.SetAccessRights(path, accessRights);
        }

        /// <summary>
        /// Удаляет файл в файловой системе.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        public void DeleteFile(string path)
        {
            FileSystem.DeleteFile(path);
        }

        /// <summary>
        /// Удаляет каталог в файловой системе.
        /// </summary>
        /// <param name="path">Путь к каталогу.</param>
        public void DeleteDirectory(string path)
        {
            FileSystem.DeleteDirectory(path);
        }

        /// <summary>
        /// Считывает указанное количество байтов содержимого файла, начиная с определённой позиции.
        /// </summary>
        /// <param name="path">Путь к файлу, содержимое которого необходимо прочитать.</param>
        /// <returns>Возвращает массив байтов в соответствии с заданными параметрами.</returns>
        public byte[] ReadFile(string path, int offset = 0, int count = -1)
        {
            return FileSystem.ReadFile(path, offset, count);
        }

        /// <summary>
        /// Записывает массив байтов в файл, переписывая его содержимое.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="data">Массив байтов.</param>
        /// <returns>Количество записанных байтов.</returns>
        public void WriteFile(string path, byte[] data)
        {
            FileSystem.WriteFile(path, data);
        }

        /// <summary>
        /// Выполняет дозапись массива байтов в конец файла.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="data">Массив байтов.</param>
        /// <returns>Количество записанных байтов.</returns>
        public void AppendFile(string path, byte[] data)
        {
            FileSystem.AppendFile(path, data);
        }

        /// <summary>
        /// Переименовывает файл.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="newFileName">Новое имя файла.</param>
        public void RenameFile(string path, string newFileName)
        {
            FileSystem.RenameFile(path, newFileName);
        }

        /// <summary>
        /// Переименовывает каталог.
        /// </summary>
        /// <param name="path">Путь к каталогу.</param>
        /// <param name="newDirectoryName">Новое имя каталога.</param>
        public void RenameDirectory(string path, string newDirectoryName)
        {
            FileSystem.RenameDirectory(path, newDirectoryName);
        }

        /// <summary>
        /// Выполняет копирование каталога в указанный каталог.
        /// При копировании каталога также копируется его содержимое.
        /// </summary>
        /// <param name="pathFrom">Путь к копируемому каталогу.</param>
        /// <param name="pathTo">Путь к каталогу, который будет содержать скопированный каталог.</param>
        public void CopyDirectory(string pathFrom, string pathTo)
        {
            FileSystem.Directory directory = FileSystem.OpenDirectory(pathFrom);
            FileSystem.Directory resultDirectory = FileSystem.OpenDirectory(pathTo);

            if (resultDirectory.Find(directory.FullName) == null)
            {
                FileSystem.CreateDirectory(pathTo + "/" + directory.FullName);
            }

            for (int i = 2; i < directory.Count; i++)
            {
                MetaFile metaFile = directory[i];

                if (metaFile is FileSystem.Directory)
                {
                    CopyDirectory(pathFrom + "/" + metaFile.FullName, pathTo + "/" + directory.FullName);
                }
                else
                {
                    CopyFile(pathFrom + "/" + metaFile.FullName, pathTo + "/" + directory.FullName);
                }
            }
        }

        /// <summary>
        /// Выполняет копирование файла в указанный каталог.
        /// </summary>
        /// <param name="pathFrom">Путь к копируемому файлу.</param>
        /// <param name="pathTo">Путь к каталогу, который будет содержать скопированный файл.</param>
        public void CopyFile(string pathFrom, string pathTo)
        {
            byte[] data = FileSystem.ReadFile(pathFrom);

            FileSystem.Directory resultDirectory = FileSystem.OpenDirectory(pathTo);

            string fileFullName = Utils.GetFileName(pathFrom);
            string path = pathTo + "/" + fileFullName;

            if (resultDirectory.Find(fileFullName) == null)
            {
                FileSystem.CreateFile(path);
                FileSystem.WriteFile(path, data);
            }
        }
        #endregion

        #region Интерфейс планировщика
        /// <summary>
        /// Запускает работу планировщика и обработку процессов.
        /// </summary>
        public void StartScheduling(BackgroundWorker worker)
        {
            Scheduler.Start(worker);
            ClearScheduler();
        }

        /// <summary>
        /// Посылает планировщику запрос на создание нового процесса.
        /// </summary>
        /// <param name="pid">ID процесса.</param>
        /// <param name="cpuBurstTime">Время CPU, необходимое процессу на выполнение.</param>
        /// <param name="ioBurstTime">Время I/O, необходимое процессу на выполнение.</param>
        /// <param name="priority">Приоритет процесса.</param>
        /// <param name="parent">Родительский процесс.</param>
        /// <returns>true, если процесс успешно добавлен, и false, иначе.</returns>
        public bool AddProcess(int pid, int cpuBurstTime, int ioBurstTime, ProcessPriority priority = ProcessPriority.Normal, Process parent = null)
        {
            return Scheduler.AddProcess(pid, cpuBurstTime, ioBurstTime, priority, parent);
        }

        /// <summary>
        /// Изменяет приоритет процесса в системе.
        /// </summary>
        /// <param name="pid">ID процесса, приоритет которого нужно изменить.</param>
        /// <param name="priority">Значение нового приоритета.</param>
        /// <returns>true, если приоритет процесса успешно изменён, и false, иначе.</returns>
        public bool ChangeProcessPriority(int pid, ProcessPriority priority)
        {
            return Scheduler.ChangeProcessPriority(pid, priority);
        }

        /// <summary>
        /// Посылает запрос на уничтожение процесса.
        /// </summary>
        /// <param name="pid">ID процесса, который нужно уничтожить.</param>
        /// <returns>true, если процесс назначен на уничтожение, и false, иначе.</returns>
        public bool KillProcess(int pid)
        {
            return Scheduler.KillProcess(pid);
        }

        /// <summary>
        /// Останавливает работу планировщика и очищает списки процессов.
        /// </summary>
        public void ClearScheduler()
        {
            Scheduler.Clear();

            NextPID = 1;
        }
        #endregion
    }
}
