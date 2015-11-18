namespace DehaxOS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.DehaxOSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeUserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.QuitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DehaxOSMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(284, 24);
            this.MainMenuStrip.TabIndex = 0;
            this.MainMenuStrip.Text = "Главное меню";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 240);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(284, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // DehaxOSMenuItem
            // 
            this.DehaxOSMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DehaxOSMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChangeUserMenuItem,
            this.QuitMenuItem});
            this.DehaxOSMenuItem.Image = global::DehaxOS.Properties.Resources.DehaxOSPicture;
            this.DehaxOSMenuItem.Name = "DehaxOSMenuItem";
            this.DehaxOSMenuItem.Size = new System.Drawing.Size(28, 20);
            this.DehaxOSMenuItem.Text = "DehaxOS";
            // 
            // ChangeUserMenuItem
            // 
            this.ChangeUserMenuItem.Name = "ChangeUserMenuItem";
            this.ChangeUserMenuItem.Size = new System.Drawing.Size(209, 22);
            this.ChangeUserMenuItem.Text = "Сменить пользователя...";
            // 
            // QuitMenuItem
            // 
            this.QuitMenuItem.Name = "QuitMenuItem";
            this.QuitMenuItem.Size = new System.Drawing.Size(209, 22);
            this.QuitMenuItem.Text = "Выйти из системы";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Эмулятор ОС \"DehaxOS\"";
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem DehaxOSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChangeUserMenuItem;
        private System.Windows.Forms.ToolStripMenuItem QuitMenuItem;
    }
}

