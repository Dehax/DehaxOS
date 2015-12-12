using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DehaxOS
{
    public class Group : IEquatable<Group>
    {
        public string groupName;
        public short groupId;
        public bool deleted;

        //private HashSet<User> _users;
        private List<User> _users;

        public int Count
        {
            get
            {
                return _users.Count;
            }
        }

        public Group()
        {
            //_users = new HashSet<User>();
            _users = new List<User>();
        }

        public Group(string groupName, short groupId, bool deleted = false)
            : this()
        {
            this.groupName = groupName;
            this.groupId = groupId;
            this.deleted = deleted;
            //_users = new HashSet<User>();
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public bool DeleteUser(User user)
        {
            return _users.Remove(user);
        }

        public User this[int index]
        {
            get
            {
                return _users.ElementAt(index);
            }
        }

        //public override int GetHashCode()
        //{
        //    return groupId.GetHashCode() ^ groupName.GetHashCode();
        //}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            sb.Append(groupId);
            sb.Append("] ");
            sb.Append(groupName);

            return sb.ToString();
        }

        bool IEquatable<Group>.Equals(Group other)
        {
            if (other is Group)
            {
                Group group = (Group)other;

                if (group.groupName == groupName || group.groupId == groupId)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class GroupsManager
    {
        //private HashSet<Group> _groups;
        private List<Group> _groups;

        public int Count
        {
            get
            {
                return _groups.Count;
            }
        }

        public GroupsManager()
        {
            //_groups = new HashSet<Group>();
            _groups = new List<Group>();
        }

        public bool AddGroup(Group group)
        {
            bool result = false;

            if (!_groups.Contains(group))
            {
                result = true;//
                _groups.Add(group);
            }

            return result;
        }

        //public bool AddGroup(string groupName, short groupId)
        //{
        //    Group group = new Group(groupName, groupId);

        //    return AddGroup(group);
        //}

        public void AddUserToGroup(short groupId, User user)
        {
            foreach (Group group in _groups)
            {
                if (group.groupId == groupId)
                {
                    group.AddUser(user);
                    return;
                }
            }
        }

        public bool DeleteGroup(Group group)
        {
            return _groups.Remove(group);
        }

        public bool DeleteUserFromGroup(short groupId, User user)
        {
            foreach (Group group in _groups)
            {
                if (group.groupId == groupId)
                {
                    return group.DeleteUser(user);
                }
            }

            return false;
        }

        public Group this[int index]
        {
            get
            {
                return _groups.ElementAt(index);
            }
        }
    }
}
