namespace DFSformat
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pathLabel = new System.Windows.Forms.Label();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.chooseButton = new System.Windows.Forms.Button();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.clusterSizeComboBox = new System.Windows.Forms.ComboBox();
            this.clusterSizeLabel = new System.Windows.Forms.Label();
            this.partitionSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.diskPartitionSizeLabel = new System.Windows.Forms.Label();
            this.writeButton = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.settingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.partitionSizeNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(13, 13);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(160, 13);
            this.pathLabel.TabIndex = 0;
            this.pathLabel.Text = "Путь к файлу-образу раздела:";
            // 
            // pathTextBox
            // 
            this.pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBox.Location = new System.Drawing.Point(16, 30);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(475, 20);
            this.pathTextBox.TabIndex = 1;
            this.pathTextBox.TextChanged += new System.EventHandler(this.pathTextBox_TextChanged);
            // 
            // chooseButton
            // 
            this.chooseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chooseButton.Location = new System.Drawing.Point(497, 28);
            this.chooseButton.Name = "chooseButton";
            this.chooseButton.Size = new System.Drawing.Size(75, 23);
            this.chooseButton.TabIndex = 2;
            this.chooseButton.Text = "Выбрать...";
            this.chooseButton.UseVisualStyleBackColor = true;
            this.chooseButton.Click += new System.EventHandler(this.chooseButton_Click);
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsGroupBox.Controls.Add(this.clusterSizeComboBox);
            this.settingsGroupBox.Controls.Add(this.clusterSizeLabel);
            this.settingsGroupBox.Controls.Add(this.partitionSizeNumericUpDown);
            this.settingsGroupBox.Controls.Add(this.diskPartitionSizeLabel);
            this.settingsGroupBox.Location = new System.Drawing.Point(16, 57);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(556, 99);
            this.settingsGroupBox.TabIndex = 3;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Настройки раздела жёсткого диска";
            // 
            // clusterSizeComboBox
            // 
            this.clusterSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clusterSizeComboBox.FormattingEnabled = true;
            this.clusterSizeComboBox.Items.AddRange(new object[] {
            "512",
            "1024",
            "2048",
            "4096",
            "8192",
            "16384",
            "32768",
            "65536"});
            this.clusterSizeComboBox.Location = new System.Drawing.Point(6, 71);
            this.clusterSizeComboBox.Name = "clusterSizeComboBox";
            this.clusterSizeComboBox.Size = new System.Drawing.Size(121, 21);
            this.clusterSizeComboBox.TabIndex = 3;
            // 
            // clusterSizeLabel
            // 
            this.clusterSizeLabel.AutoSize = true;
            this.clusterSizeLabel.Location = new System.Drawing.Point(6, 55);
            this.clusterSizeLabel.Name = "clusterSizeLabel";
            this.clusterSizeLabel.Size = new System.Drawing.Size(99, 13);
            this.clusterSizeLabel.TabIndex = 2;
            this.clusterSizeLabel.Text = "Размер кластера:";
            // 
            // partitionSizeNumericUpDown
            // 
            this.partitionSizeNumericUpDown.Location = new System.Drawing.Point(6, 32);
            this.partitionSizeNumericUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.partitionSizeNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.partitionSizeNumericUpDown.Name = "partitionSizeNumericUpDown";
            this.partitionSizeNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.partitionSizeNumericUpDown.TabIndex = 1;
            this.partitionSizeNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // diskPartitionSizeLabel
            // 
            this.diskPartitionSizeLabel.AutoSize = true;
            this.diskPartitionSizeLabel.Location = new System.Drawing.Point(6, 16);
            this.diskPartitionSizeLabel.Name = "diskPartitionSizeLabel";
            this.diskPartitionSizeLabel.Size = new System.Drawing.Size(94, 13);
            this.diskPartitionSizeLabel.TabIndex = 0;
            this.diskPartitionSizeLabel.Text = "Размер раздела:";
            // 
            // writeButton
            // 
            this.writeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.writeButton.Enabled = false;
            this.writeButton.Location = new System.Drawing.Point(497, 162);
            this.writeButton.Name = "writeButton";
            this.writeButton.Size = new System.Drawing.Size(75, 23);
            this.writeButton.TabIndex = 4;
            this.writeButton.Text = "Записать";
            this.writeButton.UseVisualStyleBackColor = true;
            this.writeButton.Click += new System.EventHandler(this.writeButton_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "dfs";
            this.saveFileDialog.Filter = "Файл-образ DehaxFS|*.dfs|Бинарный файл|*.bin|Все файлы|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 197);
            this.Controls.Add(this.writeButton);
            this.Controls.Add(this.settingsGroupBox);
            this.Controls.Add(this.chooseButton);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.pathLabel);
            this.MinimumSize = new System.Drawing.Size(300, 235);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Форматирование файловой системы DehaxFS";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.settingsGroupBox.ResumeLayout(false);
            this.settingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.partitionSizeNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Button chooseButton;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.ComboBox clusterSizeComboBox;
        private System.Windows.Forms.Label clusterSizeLabel;
        private System.Windows.Forms.NumericUpDown partitionSizeNumericUpDown;
        private System.Windows.Forms.Label diskPartitionSizeLabel;
        private System.Windows.Forms.Button writeButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

