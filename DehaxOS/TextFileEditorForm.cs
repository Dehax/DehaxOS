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
    public partial class TextFileEditorForm : Form
    {
        public string FileContent { get; set; }
        private bool ContentChanged { get; set; }

        public TextFileEditorForm()
        {
            InitializeComponent();

            ContentChanged = false;
        }

        private void TextFileEditorForm_Load(object sender, EventArgs e)
        {
            contentTextBox.Text = FileContent;
            contentTextBox.DeselectAll();
            contentTextBox.TextChanged += new EventHandler(contentTextBox_TextChanged);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            FileContent = contentTextBox.Text;
        }

        private void contentTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!ContentChanged)
            {
                ContentChanged = true;

                okButton.Enabled = true;
            }
        }

        private void contentTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                contentTextBox.SelectAll();
            }
        }
    }
}
