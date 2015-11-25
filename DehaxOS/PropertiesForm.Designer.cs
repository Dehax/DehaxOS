namespace DehaxOS
{
    partial class PropertiesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.commonTabPage = new System.Windows.Forms.TabPage();
            this.systemCheckBox = new System.Windows.Forms.CheckBox();
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.readOnlyCheckBox = new System.Windows.Forms.CheckBox();
            this.attributesLabel = new System.Windows.Forms.Label();
            this.accessTabPage = new System.Windows.Forms.TabPage();
            this.othersGroupBox = new System.Windows.Forms.GroupBox();
            this.othersExecuteCheckBox = new System.Windows.Forms.CheckBox();
            this.othersWriteCheckBox = new System.Windows.Forms.CheckBox();
            this.othersReadCheckBox = new System.Windows.Forms.CheckBox();
            this.groupGroupBox = new System.Windows.Forms.GroupBox();
            this.groupExecuteCheckBox = new System.Windows.Forms.CheckBox();
            this.groupWriteCheckBox = new System.Windows.Forms.CheckBox();
            this.groupReadCheckBox = new System.Windows.Forms.CheckBox();
            this.userGroupBox = new System.Windows.Forms.GroupBox();
            this.userExecuteCheckBox = new System.Windows.Forms.CheckBox();
            this.userWriteCheckBox = new System.Windows.Forms.CheckBox();
            this.userReadCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.commonTabPage.SuspendLayout();
            this.accessTabPage.SuspendLayout();
            this.othersGroupBox.SuspendLayout();
            this.groupGroupBox.SuspendLayout();
            this.userGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.commonTabPage);
            this.tabControl.Controls.Add(this.accessTabPage);
            this.tabControl.Location = new System.Drawing.Point(13, 13);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(336, 413);
            this.tabControl.TabIndex = 0;
            // 
            // commonTabPage
            // 
            this.commonTabPage.Controls.Add(this.systemCheckBox);
            this.commonTabPage.Controls.Add(this.hiddenCheckBox);
            this.commonTabPage.Controls.Add(this.readOnlyCheckBox);
            this.commonTabPage.Controls.Add(this.attributesLabel);
            this.commonTabPage.Location = new System.Drawing.Point(4, 22);
            this.commonTabPage.Name = "commonTabPage";
            this.commonTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.commonTabPage.Size = new System.Drawing.Size(328, 387);
            this.commonTabPage.TabIndex = 0;
            this.commonTabPage.Text = "Общие";
            this.commonTabPage.UseVisualStyleBackColor = true;
            // 
            // systemCheckBox
            // 
            this.systemCheckBox.AutoSize = true;
            this.systemCheckBox.Location = new System.Drawing.Point(71, 53);
            this.systemCheckBox.Name = "systemCheckBox";
            this.systemCheckBox.Size = new System.Drawing.Size(84, 17);
            this.systemCheckBox.TabIndex = 3;
            this.systemCheckBox.Text = "Системный";
            this.systemCheckBox.UseVisualStyleBackColor = true;
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.AutoSize = true;
            this.hiddenCheckBox.Location = new System.Drawing.Point(71, 29);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(72, 17);
            this.hiddenCheckBox.TabIndex = 2;
            this.hiddenCheckBox.Text = "Скрытый";
            this.hiddenCheckBox.UseVisualStyleBackColor = true;
            // 
            // readOnlyCheckBox
            // 
            this.readOnlyCheckBox.AutoSize = true;
            this.readOnlyCheckBox.Location = new System.Drawing.Point(71, 6);
            this.readOnlyCheckBox.Name = "readOnlyCheckBox";
            this.readOnlyCheckBox.Size = new System.Drawing.Size(121, 17);
            this.readOnlyCheckBox.TabIndex = 1;
            this.readOnlyCheckBox.Text = "Только для чтения";
            this.readOnlyCheckBox.UseVisualStyleBackColor = true;
            // 
            // attributesLabel
            // 
            this.attributesLabel.AutoSize = true;
            this.attributesLabel.Location = new System.Drawing.Point(7, 7);
            this.attributesLabel.Name = "attributesLabel";
            this.attributesLabel.Size = new System.Drawing.Size(58, 13);
            this.attributesLabel.TabIndex = 0;
            this.attributesLabel.Text = "Атрибуты:";
            // 
            // accessTabPage
            // 
            this.accessTabPage.Controls.Add(this.othersGroupBox);
            this.accessTabPage.Controls.Add(this.groupGroupBox);
            this.accessTabPage.Controls.Add(this.userGroupBox);
            this.accessTabPage.Location = new System.Drawing.Point(4, 22);
            this.accessTabPage.Name = "accessTabPage";
            this.accessTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.accessTabPage.Size = new System.Drawing.Size(328, 387);
            this.accessTabPage.TabIndex = 1;
            this.accessTabPage.Text = "Доступ";
            this.accessTabPage.UseVisualStyleBackColor = true;
            // 
            // othersGroupBox
            // 
            this.othersGroupBox.Controls.Add(this.othersExecuteCheckBox);
            this.othersGroupBox.Controls.Add(this.othersWriteCheckBox);
            this.othersGroupBox.Controls.Add(this.othersReadCheckBox);
            this.othersGroupBox.Location = new System.Drawing.Point(6, 197);
            this.othersGroupBox.Name = "othersGroupBox";
            this.othersGroupBox.Size = new System.Drawing.Size(315, 89);
            this.othersGroupBox.TabIndex = 3;
            this.othersGroupBox.TabStop = false;
            this.othersGroupBox.Text = "Другие";
            // 
            // othersExecuteCheckBox
            // 
            this.othersExecuteCheckBox.AutoSize = true;
            this.othersExecuteCheckBox.Location = new System.Drawing.Point(6, 66);
            this.othersExecuteCheckBox.Name = "othersExecuteCheckBox";
            this.othersExecuteCheckBox.Size = new System.Drawing.Size(86, 17);
            this.othersExecuteCheckBox.TabIndex = 2;
            this.othersExecuteCheckBox.Text = "исполнение";
            this.othersExecuteCheckBox.UseVisualStyleBackColor = true;
            // 
            // othersWriteCheckBox
            // 
            this.othersWriteCheckBox.AutoSize = true;
            this.othersWriteCheckBox.Location = new System.Drawing.Point(7, 43);
            this.othersWriteCheckBox.Name = "othersWriteCheckBox";
            this.othersWriteCheckBox.Size = new System.Drawing.Size(62, 17);
            this.othersWriteCheckBox.TabIndex = 1;
            this.othersWriteCheckBox.Text = "запись";
            this.othersWriteCheckBox.UseVisualStyleBackColor = true;
            // 
            // othersReadCheckBox
            // 
            this.othersReadCheckBox.AutoSize = true;
            this.othersReadCheckBox.Location = new System.Drawing.Point(6, 19);
            this.othersReadCheckBox.Name = "othersReadCheckBox";
            this.othersReadCheckBox.Size = new System.Drawing.Size(60, 17);
            this.othersReadCheckBox.TabIndex = 0;
            this.othersReadCheckBox.Text = "чтение";
            this.othersReadCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupGroupBox
            // 
            this.groupGroupBox.Controls.Add(this.groupExecuteCheckBox);
            this.groupGroupBox.Controls.Add(this.groupWriteCheckBox);
            this.groupGroupBox.Controls.Add(this.groupReadCheckBox);
            this.groupGroupBox.Location = new System.Drawing.Point(7, 102);
            this.groupGroupBox.Name = "groupGroupBox";
            this.groupGroupBox.Size = new System.Drawing.Size(315, 89);
            this.groupGroupBox.TabIndex = 1;
            this.groupGroupBox.TabStop = false;
            this.groupGroupBox.Text = "Группа";
            // 
            // groupExecuteCheckBox
            // 
            this.groupExecuteCheckBox.AutoSize = true;
            this.groupExecuteCheckBox.Location = new System.Drawing.Point(6, 66);
            this.groupExecuteCheckBox.Name = "groupExecuteCheckBox";
            this.groupExecuteCheckBox.Size = new System.Drawing.Size(86, 17);
            this.groupExecuteCheckBox.TabIndex = 2;
            this.groupExecuteCheckBox.Text = "исполнение";
            this.groupExecuteCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupWriteCheckBox
            // 
            this.groupWriteCheckBox.AutoSize = true;
            this.groupWriteCheckBox.Location = new System.Drawing.Point(7, 43);
            this.groupWriteCheckBox.Name = "groupWriteCheckBox";
            this.groupWriteCheckBox.Size = new System.Drawing.Size(62, 17);
            this.groupWriteCheckBox.TabIndex = 1;
            this.groupWriteCheckBox.Text = "запись";
            this.groupWriteCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupReadCheckBox
            // 
            this.groupReadCheckBox.AutoSize = true;
            this.groupReadCheckBox.Location = new System.Drawing.Point(6, 19);
            this.groupReadCheckBox.Name = "groupReadCheckBox";
            this.groupReadCheckBox.Size = new System.Drawing.Size(60, 17);
            this.groupReadCheckBox.TabIndex = 0;
            this.groupReadCheckBox.Text = "чтение";
            this.groupReadCheckBox.UseVisualStyleBackColor = true;
            // 
            // userGroupBox
            // 
            this.userGroupBox.Controls.Add(this.userExecuteCheckBox);
            this.userGroupBox.Controls.Add(this.userWriteCheckBox);
            this.userGroupBox.Controls.Add(this.userReadCheckBox);
            this.userGroupBox.Location = new System.Drawing.Point(7, 7);
            this.userGroupBox.Name = "userGroupBox";
            this.userGroupBox.Size = new System.Drawing.Size(315, 89);
            this.userGroupBox.TabIndex = 0;
            this.userGroupBox.TabStop = false;
            this.userGroupBox.Text = "Владелец";
            // 
            // userExecuteCheckBox
            // 
            this.userExecuteCheckBox.AutoSize = true;
            this.userExecuteCheckBox.Location = new System.Drawing.Point(6, 66);
            this.userExecuteCheckBox.Name = "userExecuteCheckBox";
            this.userExecuteCheckBox.Size = new System.Drawing.Size(86, 17);
            this.userExecuteCheckBox.TabIndex = 2;
            this.userExecuteCheckBox.Text = "исполнение";
            this.userExecuteCheckBox.UseVisualStyleBackColor = true;
            // 
            // userWriteCheckBox
            // 
            this.userWriteCheckBox.AutoSize = true;
            this.userWriteCheckBox.Location = new System.Drawing.Point(7, 43);
            this.userWriteCheckBox.Name = "userWriteCheckBox";
            this.userWriteCheckBox.Size = new System.Drawing.Size(62, 17);
            this.userWriteCheckBox.TabIndex = 1;
            this.userWriteCheckBox.Text = "запись";
            this.userWriteCheckBox.UseVisualStyleBackColor = true;
            // 
            // userReadCheckBox
            // 
            this.userReadCheckBox.AutoSize = true;
            this.userReadCheckBox.Location = new System.Drawing.Point(6, 19);
            this.userReadCheckBox.Name = "userReadCheckBox";
            this.userReadCheckBox.Size = new System.Drawing.Size(60, 17);
            this.userReadCheckBox.TabIndex = 0;
            this.userReadCheckBox.Text = "чтение";
            this.userReadCheckBox.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(274, 432);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(193, 432);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "ОК";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // PropertiesForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(361, 467);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertiesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Свойства: ";
            this.Load += new System.EventHandler(this.PropertiesForm_Load);
            this.tabControl.ResumeLayout(false);
            this.commonTabPage.ResumeLayout(false);
            this.commonTabPage.PerformLayout();
            this.accessTabPage.ResumeLayout(false);
            this.othersGroupBox.ResumeLayout(false);
            this.othersGroupBox.PerformLayout();
            this.groupGroupBox.ResumeLayout(false);
            this.groupGroupBox.PerformLayout();
            this.userGroupBox.ResumeLayout(false);
            this.userGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage commonTabPage;
        private System.Windows.Forms.TabPage accessTabPage;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckBox systemCheckBox;
        private System.Windows.Forms.CheckBox hiddenCheckBox;
        private System.Windows.Forms.CheckBox readOnlyCheckBox;
        private System.Windows.Forms.Label attributesLabel;
        private System.Windows.Forms.GroupBox othersGroupBox;
        private System.Windows.Forms.CheckBox othersExecuteCheckBox;
        private System.Windows.Forms.CheckBox othersWriteCheckBox;
        private System.Windows.Forms.CheckBox othersReadCheckBox;
        private System.Windows.Forms.GroupBox groupGroupBox;
        private System.Windows.Forms.CheckBox groupExecuteCheckBox;
        private System.Windows.Forms.CheckBox groupWriteCheckBox;
        private System.Windows.Forms.CheckBox groupReadCheckBox;
        private System.Windows.Forms.GroupBox userGroupBox;
        private System.Windows.Forms.CheckBox userExecuteCheckBox;
        private System.Windows.Forms.CheckBox userWriteCheckBox;
        private System.Windows.Forms.CheckBox userReadCheckBox;
    }
}