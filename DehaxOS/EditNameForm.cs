using System;
using System.IO;
using System.Windows.Forms;

namespace DehaxOS
{
    public partial class EditNameForm : Form
    {
        private string _newName;
        public string NewName
        {
            get
            {
                return _newName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException("Имя не может быть пустым или содержать пробельные символы!");
                }

                if (value.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(NewName), "Имя содержит недопустимые символы!");
                }

                _newName = value;
            }
        }

        public EditNameForm()
        {
            InitializeComponent();

            _newName = string.Empty;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                NewName = nameTextBox.Text;
            }
            catch (Exception ex) when (ex is NullReferenceException)
            {
                MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }
        }

        private void EditNameForm_Load(object sender, EventArgs e)
        {
            nameTextBox.Text = NewName;
        }
    }
}
