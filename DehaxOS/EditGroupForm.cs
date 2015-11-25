using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DehaxOS
{
    public partial class EditGroupForm : Form
    {
        private string _groupName;
        /// <summary>
        /// Имя группы.
        /// </summary>
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException("Имя группы не может быть пустым или содержать только пробельные символы.");
                }

                if (Regex.IsMatch(value, @"[^a-zA-Z0-9]"))
                {
                    throw new ArgumentOutOfRangeException(nameof(GroupName), "Имя группы содержит недопустимые символы! Имя может состоять только из символов латиницы и цифр.");
                }

                if (value.Length < 4)
                {
                    throw new ArgumentOutOfRangeException(nameof(GroupName), "Имя группы должно состоять минимум из 4 символов.");
                }
                else if (value.Length > 20)
                {
                    throw new ArgumentOutOfRangeException(nameof(GroupName), "Имя группы должно состоять не более чем из 20 символов.");
                }

                _groupName = value;
            }
        }

        public EditGroupForm()
        {
            InitializeComponent();

            _groupName = string.Empty;
        }

        public EditGroupForm(string groupName)
            : this()
        {
            GroupName = groupName;
        }

        private void EditGroupForm_Load(object sender, EventArgs e)
        {
            groupNameTextBox.Text = GroupName;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                GroupName = groupNameTextBox.Text;
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
