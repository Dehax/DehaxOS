using DehaxOS.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DehaxOS
{
    static class Utils
    {
        /// <summary>
        /// Проверяет путь на правильность формата (UNIX-style-path).
        /// </summary>
        /// <param name="path">Путь для проверки.</param>
        /// <param name="fullPath">Должен ли путь быть абсолютным.</param>
        public static void CheckPath(string path, bool fullPath = false)
        {
            if (fullPath && path[0] != '/')
            {
                throw new ArgumentException("Путь не является полным (абсолютным)!", nameof(path));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), "Неверно задан путь!");
            }

            if (path == "/")
            {
                return;
            }

            if (path[0] == '/')
            {
                path = path.Substring(1);
            }

            string[] pathList = path.Split('/');
            string pathPart;

            for (int i = 0; i < pathList.Length; i++)
            {
                pathPart = pathList[i];

                if (pathPart.Length == 0 ||
                    pathPart.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    throw new FormatException("Путь содержит недопустимые символы!");
                }
            }
        }

        /// <summary>
        /// Возвращает имя файла.
        /// </summary>
        /// <param name="path">Полный путь к файлу</param>
        /// <returns>Имя файла.</returns>
        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Возвращает имя файла без расширения.
        /// </summary>
        /// <param name="path">Полный путь к файлу</param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Возвращает расширение файла.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExtension(string path)
        {
            return Path.GetExtension(path).Substring(1);
        }

        /// <summary>
        /// Возвращает имя каталога, содержащего файл.
        /// </summary>
        /// <param name="path">Полный путь к файлу, который находится в каталоге, имя которого будет возвращено.</param>
        /// <returns>Имя каталога, содержащего файл.</returns>
        public static string GetDirectoryName(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            directoryName = directoryName.Replace("\\", "/");
            return directoryName/* == "\\" ? "/" : directoryName*/;
        }

        /// <summary>
        /// Возвращает массив имён каталогов, предшествующих заданному в пути файлу или каталогу либо включая его.
        /// </summary>
        /// <param name="fullPath">Полный путь к файлу или каталогу</param>
        /// <param name="includeLastName">Включать ли в массив имён каталогов последнее имя в пути.</param>
        /// <returns>массив имён каталогов в заданном пути.</returns>
        public static string[] GetDirectoriesNames(string fullPath, bool includeLastName = false)
        {
            CheckPath(fullPath, true);

            fullPath = fullPath.Substring(1);

            string[] directoriesNames = fullPath.Split('/');

            string[] result = new string[directoriesNames.Length - (includeLastName ? 0 : 1)];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = directoriesNames[i];
            }

            return result;
        }

        /// <summary>
        /// Преобразовывает относительный путь к полному (абсолютному) пути. Если заданный путь является полным, возвращает его.
        /// </summary>
        /// <param name="path">Относительный или полный путь.</param>
        /// <param name="currentPath">Текущий полный путь, относительно которого будет сформирован итоговый полный путь.</param>
        /// <returns></returns>
        public static string GetFullPath(string path, string currentPath)
        {
            CheckPath(path);
            //CheckPath(currentPath, true);

            if (path[0] == '/')
            {
                return path;
            }

            CheckPath(currentPath, true);

            StringBuilder sb = new StringBuilder(currentPath, 0, currentPath.Length, currentPath.Length);
            if (currentPath != "/")
            {
                sb.Append('/');
            }
            sb.Append(path);
            sb.Replace("/./", "/");

            string cleaned = sb.ToString();

            int startIndex, endIndex;
            string sliced;
            while ((endIndex = cleaned.IndexOf("/../")) >= 0)
            {
                sliced = cleaned.Substring(0, endIndex);
                startIndex = sliced.LastIndexOf('/');
                startIndex++;

                sb.Remove(startIndex, endIndex - startIndex + 4);
                cleaned = sb.ToString();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Возвращает текущее время время UTC в формате UNIX (FILE).
        /// </summary>
        /// <returns>текущее время время UTC в формате UNIX (FILE)</returns>
        public static long GetTimestamp()
        {
            return DateTime.UtcNow.ToFileTimeUtc();
        }

        /// <summary>
        /// Проверяет права доступа к объекту файловой системы.
        /// </summary>
        /// <param name="userId">ID текущего пользователя в системе.</param>
        /// <param name="groupId">ID группы текущего пользователя в системе.</param>
        /// <param name="accessRights">Права доступа к объекту.</param>
        /// <returns>набор разрешений на чтение/запись/исполнение.</returns>
        public static AccessRights.RightsGroup CanAccess(short userId, short groupId, AccessRights accessRights)
        {
            AccessRights.RightsGroup result = new AccessRights.RightsGroup();

            // TODO: Дописать проверку прав доступа.

            return result;
        }
    }
}
