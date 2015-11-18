using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DehaxOS
{
    struct Group
    {
        public string groupName;
        public short groupId;
        public bool deleted;

        private HashSet<User> _users;

        public Group(string groupName, short groupId, bool deleted = false)
        {
            this.groupName = groupName;
            this.groupId = groupId;
            this.deleted = deleted;
            _users = new HashSet<User>();
        }

        public bool AddUser(User user)
        {
            return _users.Add(user);
        }

        public override bool Equals(object obj)
        {
            if (obj is Group)
            {
                Group group = (Group)obj;

                if (group.groupName == groupName || group.groupId == groupId)
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return groupId;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            sb.Append(groupId);
            sb.Append("] ");
            sb.Append(groupName);

            return sb.ToString();
        }
    }

    class GroupsManager
    {
        private HashSet<Group> _groups;

        public int Count
        {
            get
            {
                return _groups.Count;
            }
        }

        public GroupsManager()
        {
            _groups = new HashSet<Group>();
        }

        public bool AddGroup(Group group)
        {
            return _groups.Add(group);
        }

        public bool AddGroup(string groupName, short groupId)
        {
            Group group = new Group(groupName, groupId);

            return AddGroup(group);
        }

        public bool AddUserToGroup(short groupId, User user)
        {
            foreach (Group group in _groups)
            {
                if (group.groupId == groupId)
                {
                    return group.AddUser(user);
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
