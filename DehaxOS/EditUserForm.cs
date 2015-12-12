using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DehaxOS
{
    public partial class EditUserForm : Form
    {
        private string _userName;
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException("Имя пользователя не может быть пустым или содержать только пробельные символы.");
                }

                if (Regex.IsMatch(value, @"[^a-zA-Z0-9]"))
                {
                    throw new ArgumentOutOfRangeException(nameof(UserName), "Имя пользователя содержит недопустимые символы! Имя может состоять только из символов латиницы и цифр.");
                }

                if (value.Length < 4)
                {
                    throw new ArgumentOutOfRangeException(nameof(UserName), "Имя пользователя должно состоять минимум из 4 символов.");
                }
                else if (value.Length > 20)
                {
                    throw new ArgumentOutOfRangeException(nameof(UserName), "Имя пользователя должно состоять не более чем из 20 символов.");
                }

                _userName = value;
            }
        }
        private string _password;
        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException("Пароль пользователя не может быть пустым или содержать только пробельные символы.");
                }

                if (Regex.IsMatch(value, @"[^a-zA-Z0-9]"))
                {
                    throw new ArgumentOutOfRangeException(nameof(Password), "Пароль пользователя содержит недопустимые символы! Пароль может состоять только из символов латиницы и цифр.");
                }

                if (value.Length < 4)
                {
                    throw new ArgumentOutOfRangeException(nameof(Password), "Пароль пользователя должен состоять минимум из 4 символов.");
                }
                else if (value.Length > 20)
                {
                    throw new ArgumentOutOfRangeException(nameof(Password), "Пароль пользователя должен состоять не более чем из 20 символов.");
                }

                _password = value;
            }
        }
        public short GroupId { get; set; }

        public Group[] Groups { get; set; }

        public EditUserForm()
        {
            InitializeComponent();

            _userName = string.Empty;
            _password = string.Empty;
            GroupId = -1;
            Groups = new Group[0];
        }

        private void EditUserForm_Load(object sender, EventArgs e)
        {
            userNameTextBox.Text = UserName;
            passwordTextBox.Text = Password;
            repeatPasswordTextBox.Text = Password;
            groupComboBox.Items.Clear();

            for (int i = 0; i < Groups.Length; i++)
            {
                groupComboBox.Items.Add(Groups[i].groupName);

                if (Groups[i].groupId == GroupId)
                {
                    groupComboBox.SelectedIndex = i;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            string userName = userNameTextBox.Text;
            string password = passwordTextBox.Text;
            string repeatPassword = repeatPasswordTextBox.Text;

            if (password != repeatPassword)
            {
                MessageBox.Show(this, "Пароли не совпадают!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            try
            {
                UserName = userName;
                Password = password;

                for (int i = 0; i < Groups.Length; i++)
                {
                    if (Groups[i].groupName == (string)groupComboBox.SelectedItem)
                    {
                        GroupId = Groups[i].groupId;
                    }
                }

                if (GroupId < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(GroupId), "Не выбрана группа пользователей!");
                }
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is ArgumentOutOfRangeException)
            {
                MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }
        }
    }
}
