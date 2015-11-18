using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DehaxOS
{
    /// <summary>
    /// Представляет сведения о пользователе ОС.
    /// </summary>
    struct User
    {
        /// <summary>
        /// Символьное имя пользователя, вводит сам пользователь при регистрации.
        /// Допустимы только латинские буквы и цифры.
        /// </summary>
        public string userName;
        /// <summary>
        /// Хеш пароля пользователя, вычисленный по алгоритму MD5 (16 байт).
        /// </summary>
        public byte[] passwordHash;
        /// <summary>
        /// Уникальный код пользователя. Root-пользователь имеет код 1.
        /// </summary>
        public short userId;
        /// <summary>
        /// Код группы пользователей, в которой состоит данный пользователь. Код группы root-пользователей имеет значение 1.
        /// </summary>
        public short groupId;

        public bool deleted;

        ///// <summary>
        ///// Размер данных о пользователе в байтах.
        ///// </summary>
        //public int SizeInBytes
        //{
        //    get
        //    {
        //        return userName.Length + passwordHash.Length + sizeof(short) + sizeof(short);
        //    }
        //}

        public User(string userName, byte[] passwordHash, short userId, short groupId, bool deleted = false)
        {
            this.userName = userName;
            this.passwordHash = passwordHash;
            this.userId = userId;
            this.groupId = groupId;
            this.deleted = deleted;
        }

        public override bool Equals(object obj)
        {
            if (obj is User)
            {
                User user = (User)obj;

                if (user.userName == userName || user.userId == userId)
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return userId;
        }
    }

    class UsersManager
    {
        private HashSet<User> _users;

        public int Count
        {
            get
            {
                return _users.Count;
            }
        }

        public UsersManager()
        {
            _users = new HashSet<User>();
        }

        /// <summary>
        /// Добавляет нового пользователя в список пользователей ОС.
        /// </summary>
        /// <param name="user">Новый пользователь ОС.</param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            return _users.Add(user);
        }

        /// <summary>
        /// Добавляет нового пользователя в список пользователей ОС.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="passwordHash">Хеш пароля пользователя.</param>
        /// <param name="userId">Уникальный код пользователя.</param>
        /// <param name="groupId">Код группы пользователя.</param>
        /// <returns></returns>
        public bool AddUser(string userName, byte[] passwordHash, short userId, short groupId)
        {
            User user = new User()
            {
                userName = userName,
                passwordHash = passwordHash,
                userId = userId,
                groupId = groupId
            };

            return AddUser(user);
        }

        public User this[int index]
        {
            get
            {
                return _users.ElementAt(index);
            }
        }
    }
}
