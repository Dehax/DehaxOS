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
    public partial class EditNameForm : Form
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException("Имя не может быть пустым или содержать пробельные символы!");
                }

                _name = value;
            }
        }

        public EditNameForm()
        {
            InitializeComponent();

            _name = string.Empty;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                Name = nameTextBox.Text;
            }
            catch (Exception ex) when (ex is NullReferenceException)
            {
                MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.OK;
                return;
            }
        }

        private void EditNameForm_Load(object sender, EventArgs e)
        {
            nameTextBox.Text = Name;
        }
    }
}
