using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DFSformat
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void chooseButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                pathTextBox.Text = saveFileDialog.FileName;
            }
            else
            {
                pathTextBox.Clear();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            clusterSizeComboBox.SelectedIndex = 2;
        }

        private void writeButton_Click(object sender, EventArgs e)
        {
            DFSImage dfsImage = new DFSImage((long)partitionSizeNumericUpDown.Value * 1024 * 1024, Convert.ToByte(clusterSizeComboBox.SelectedIndex + 1));

            try
            {
                dfsImage.ExportToFile(pathTextBox.Text);
                MessageBox.Show(this, "Создание файла-образа файловой системы DehaxFS успешно завершено!", "Образ создан!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Проверьте правильность пути.\n" + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (pathTextBox.Text.Length > 0)
            {
                writeButton.Enabled = true;
            }
            else
            {
                writeButton.Enabled = false;
            }
        }
    }
}
