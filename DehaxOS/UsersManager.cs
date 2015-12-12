using System;
using System.Collections.Generic;
using System.Linq;

namespace DehaxOS
{
    /// <summary>
    /// Представляет сведения о пользователе ОС.
    /// </summary>
    public class User : IEquatable<User>
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
        /// <summary>
        /// Был ли пользователь удалён.
        /// </summary>
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

        public User()
        {

        }

        public User(string userName, byte[] passwordHash, short userId, short groupId, bool deleted = false)
        {
            this.userName = userName;
            this.passwordHash = passwordHash;
            this.userId = userId;
            this.groupId = groupId;
            this.deleted = deleted;
        }

        //public override int GetHashCode()
        //{
        //    return userId.GetHashCode() ^ userName.GetHashCode();
        //}

        bool IEquatable<User>.Equals(User other)
        {
            if (other is User)
            {
                User user = (User)other;

                if (user.userName == userName || user.userId == userId)
                    return true;
            }

            return false;
        }
    }

    public class UsersManager
    {
        /// <summary>
        /// Все пользователи ОС, включая удалённых.
        /// </summary>
        //private HashSet<User> _users;
        private List<User> _users;

        /// <summary>
        /// Количество пользователей ОС, включая root-пользователя и удалённых пользователей.
        /// </summary>
        public int Count
        {
            get
            {
                return _users.Count;
            }
        }

        public UsersManager()
        {
            //_users = new HashSet<User>();
            _users = new List<User>();
        }

        /// <summary>
        /// Добавляет нового пользователя в список пользователей ОС.
        /// </summary>
        /// <param name="user">Новый пользователь ОС.</param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            bool result = false;

            if (!_users.Contains(user))
            {
                result = true;// _users.Add(user);
                _users.Add(user);
            }

            return result;
        }

        ///// <summary>
        ///// Добавляет нового пользователя в список пользователей ОС.
        ///// </summary>
        ///// <param name="userName">Имя пользователя.</param>
        ///// <param name="passwordHash">Хеш пароля пользователя.</param>
        ///// <param name="userId">Уникальный код пользователя.</param>
        ///// <param name="groupId">Код группы пользователя.</param>
        ///// <returns></returns>
        //public bool AddUser(string userName, byte[] passwordHash, short userId, short groupId)
        //{
        //    User user = new User()
        //    {
        //        userName = userName,
        //        passwordHash = passwordHash,
        //        userId = userId,
        //        groupId = groupId
        //    };

        //    return AddUser(user);
        //}

        //public bool DeleteUser(User user)
        //{
        //    for (int i = 0; i < _users.Count; i++)
        //    {
        //        if (_users.ElementAt(i).deleted)
        //    }

        //    //return _users.Remove(user);
        //}

        public User this[int index]
        {
            get
            {
                return _users.ElementAt(index);
            }
        }
    }
}
