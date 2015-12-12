using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DehaxOS
{
    public partial class LoginForm : Form
    {
        private string _userName;
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_userName))
                {
                    throw new NullReferenceException("Имя пользователя не заполнено! Проверьте, было ли запущено окно логина перед получением имени пользователя.");
                }

                return _userName;
            }
            private set
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
                if (string.IsNullOrWhiteSpace(_password))
                {
                    throw new NullReferenceException("Пароль не заполнен! Проверьте, было ли запущено окно логина перед получением пароля пользователя.");
                }

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

        public LoginForm()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                UserName = loginTextBox.Text;
                Password = passwordTextBox.Text;
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is ArgumentOutOfRangeException)
            {
                MessageBox.Show(this, ex.Message, "Ошибка ввода!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }

            ClearFields();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            loginTextBox.Clear();
            passwordTextBox.Clear();
            loginTextBox.Focus();
        }
    }
}
