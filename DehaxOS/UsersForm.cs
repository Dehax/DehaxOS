using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DehaxOS
{
    public partial class UsersForm : Form
    {
        public UsersManager UsersManager { get; set; }
        public GroupsManager GroupsManager { get; set; }

        public bool UsersChanged { get; set; }
        public bool GroupsChanged { get; set; }

        public UsersForm(UsersManager usersManager, GroupsManager groupsManager)
        {
            InitializeComponent();

            UsersManager = usersManager;
            GroupsManager = groupsManager;

            UsersChanged = false;
            GroupsChanged = false;
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {
            usersListBox.Items.Clear();

            for (int i = 0; i < UsersManager.Count; i++)
            {
                User user = UsersManager[i];
                usersListBox.Items.Add((user.deleted ? "- " : string.Empty) + user.userName);
            }

            groupsListBox.Items.Clear();

            for (int i = 0; i < GroupsManager.Count; i++)
            {
                Group group = GroupsManager[i];
                groupsListBox.Items.Add((group.deleted ? "- " : string.Empty) + group.groupName);
            }

            usersListBox.SelectedIndex = 0;
            groupsListBox.SelectedIndex = 0;
        }

        private void AddUser()
        {
            EditUserForm editUserForm = new EditUserForm();
            editUserForm.Groups = new Group[GroupsManager.Count];

            for (int i = 0; i < GroupsManager.Count; i++)
            {
                editUserForm.Groups[i] = new Group(GroupsManager[i].groupName, GroupsManager[i].groupId);
            }

            if (editUserForm.ShowDialog(this) == DialogResult.OK)
            {
                User user = new User();

                short newId = 0;
                for (int i = 0; i < UsersManager.Count; i++)
                {
                    if (UsersManager[i].userId > newId)
                    {
                        newId = UsersManager[i].userId;
                    }
                }

                user.userId = ++newId;
                user.userName = editUserForm.UserName;
                user.passwordHash = Utils.GetPasswordHash(editUserForm.Password);
                user.groupId = editUserForm.GroupId;

                if (UsersManager.AddUser(user))
                {
                    GroupsManager.AddUserToGroup(user.groupId, user);
                    usersListBox.Items.Add(user.userName);
                }
                else
                {
                    MessageBox.Show(this, "Пользователь с таким именем уже существует!", "Пользователь существует", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void AddGroup()
        {
            EditGroupForm editGroupForm = new EditGroupForm();

            if (editGroupForm.ShowDialog(this) == DialogResult.OK)
            {
                Group group = new Group();

                short newId = 0;
                for (int i = 0; i < GroupsManager.Count; i++)
                {
                    if (GroupsManager[i].groupId > newId)
                    {
                        newId = GroupsManager[i].groupId;
                    }
                }

                group.groupId = ++newId;
                group.groupName = editGroupForm.GroupName;
                
                if (GroupsManager.AddGroup(group))
                {
                    groupsListBox.Items.Add(group.groupName);
                }
                else
                {
                    MessageBox.Show(this, "Группа с таким именем уже существует!", "Группа существует", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void EditUser()
        {
            User user = UsersManager[usersListBox.SelectedIndex];

            if (user.userId == 1)
            {
                MessageBox.Show(this, "Пользователь root не может быть переименован!", "Невозможно переименовать", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (user.deleted)
            {
                MessageBox.Show(this, "Данный пользователь удалён! Редактирование недоступно.", "Пользователь удалён", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EditUserForm editUserForm = new EditUserForm();
            editUserForm.UserName = user.userName;
            editUserForm.GroupId = user.groupId;
            editUserForm.Groups = new Group[GroupsManager.Count];

            for (int i = 0; i < GroupsManager.Count; i++)
            {
                editUserForm.Groups[i] = new Group(GroupsManager[i].groupName, GroupsManager[i].groupId);
            }

            if (editUserForm.ShowDialog(this) == DialogResult.OK)
            {
                //user = new User();

                GroupsManager.DeleteUserFromGroup(user.groupId, user);
                user.userName = editUserForm.UserName;
                user.passwordHash = Utils.GetPasswordHash(editUserForm.Password);
                user.groupId = editUserForm.GroupId;
                GroupsManager.AddUserToGroup(user.groupId, user);

                usersListBox.Items[usersListBox.SelectedIndex] = user.userName;
            }
        }

        private void EditGroup()
        {
            Group group = GroupsManager[groupsListBox.SelectedIndex];

            if (group.groupId == 1)
            {
                MessageBox.Show(this, "Группа root не может быть переименована!", "Невозможно переименовать", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (group.deleted)
            {
                MessageBox.Show(this, "Данная группа удалёна! Редактирование недоступно.", "Группа удалёна", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EditGroupForm editGroupForm = new EditGroupForm();
            editGroupForm.GroupName = group.groupName;

            if (editGroupForm.ShowDialog(this) == DialogResult.OK)
            {
                short newId = 0;
                for (int i = 0; i < GroupsManager.Count; i++)
                {
                    if (GroupsManager[i].groupId > newId)
                    {
                        newId = GroupsManager[i].groupId;
                    }
                }

                group.groupName = editGroupForm.GroupName;

                groupsListBox.Items[groupsListBox.SelectedIndex] = group.groupName;
            }
        }

        private void DeleteUser()
        {
            User user = UsersManager[usersListBox.SelectedIndex];

            if (user.userId == 1)
            {
                MessageBox.Show(this, "Пользователь root не может быть удалён!", "Невозможно удалить", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (user.deleted)
            {
                MessageBox.Show(this, "Пользователь уже был удалён!", "Пользователь удалён", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            user.deleted = true;

            //UsersManager.DeleteUser(user);

            usersListBox.Items[usersListBox.SelectedIndex] = "- " + user.userName;
        }

        private void DeleteGroup()
        {
            Group group = GroupsManager[groupsListBox.SelectedIndex];

            if (group.groupId == 1)
            {
                MessageBox.Show(this, "Группа root не может быть удалёна!", "Невозможно удалить", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool canDelete = !group.deleted;
            for (int i = 0; i < group.Count; i++)
            {
                if (!group[i].deleted)
                {
                    canDelete &= false;
                    break;
                }
            }

            if (!canDelete)
            {
                MessageBox.Show(this, "Данная группа содержит пользователей! Редактирование недоступно.", "Группа удалёна", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            group.deleted = true;
            //GroupsManager.DeleteGroup(group);

            groupsListBox.Items[groupsListBox.SelectedIndex] = "- " + group.groupName;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == usersTabPage)
            {
                AddUser();
            }
            else if (tabControl.SelectedTab == groupsTabPage)
            {
                AddGroup();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == usersTabPage)
            {
                EditUser();
            }
            else if (tabControl.SelectedTab == groupsTabPage)
            {
                EditGroup();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == usersTabPage)
            {
                DeleteUser();
            }
            else if (tabControl.SelectedTab == groupsTabPage)
            {
                DeleteGroup();
            }
        }
    }
}
