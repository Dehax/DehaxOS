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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.dehaxOSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeUserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.quitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSystemMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDirectoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTextFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMetaFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteMetaFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.formatMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schedulerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.systemStateStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.fileSystemTreeView = new System.Windows.Forms.TreeView();
            this.fileSystemContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mainMenu.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dehaxOSMenuItem,
            this.fileSystemMenuItem,
            this.schedulerMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(312, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "Главное меню";
            // 
            // dehaxOSMenuItem
            // 
            this.dehaxOSMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dehaxOSMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeUserMenuItem,
            this.usersMenuItem,
            this.toolStripSeparator3,
            this.quitMenuItem});
            this.dehaxOSMenuItem.Image = global::DehaxOS.Properties.Resources.DehaxOSPicture;
            this.dehaxOSMenuItem.Name = "dehaxOSMenuItem";
            this.dehaxOSMenuItem.Size = new System.Drawing.Size(28, 20);
            this.dehaxOSMenuItem.Text = "DehaxOS";
            // 
            // changeUserMenuItem
            // 
            this.changeUserMenuItem.Name = "changeUserMenuItem";
            this.changeUserMenuItem.Size = new System.Drawing.Size(209, 22);
            this.changeUserMenuItem.Text = "Сменить пользователя...";
            this.changeUserMenuItem.Click += new System.EventHandler(this.changeUserMenuItem_Click);
            // 
            // usersMenuItem
            // 
            this.usersMenuItem.Image = global::DehaxOS.Properties.Resources.users;
            this.usersMenuItem.Name = "usersMenuItem";
            this.usersMenuItem.Size = new System.Drawing.Size(209, 22);
            this.usersMenuItem.Text = "Пользователи...";
            this.usersMenuItem.Click += new System.EventHandler(this.usersMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(206, 6);
            // 
            // quitMenuItem
            // 
            this.quitMenuItem.Image = global::DehaxOS.Properties.Resources.shutdown;
            this.quitMenuItem.Name = "quitMenuItem";
            this.quitMenuItem.Size = new System.Drawing.Size(209, 22);
            this.quitMenuItem.Text = "Выйти из системы";
            this.quitMenuItem.Click += new System.EventHandler(this.QuitMenuItem_Click);
            // 
            // fileSystemMenuItem
            // 
            this.fileSystemMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createMenuItem,
            this.openMetaFileMenuItem,
            this.toolStripSeparator1,
            this.deleteMetaFileMenuItem,
            this.renameMenuItem,
            this.toolStripSeparator2,
            this.propertiesMenuItem,
            this.toolStripSeparator4,
            this.formatMenuItem});
            this.fileSystemMenuItem.Name = "fileSystemMenuItem";
            this.fileSystemMenuItem.Size = new System.Drawing.Size(121, 20);
            this.fileSystemMenuItem.Text = "Файловая система";
            // 
            // createMenuItem
            // 
            this.createMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDirectoryMenuItem,
            this.newTextFileMenuItem});
            this.createMenuItem.Name = "createMenuItem";
            this.createMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createMenuItem.Text = "Создать";
            // 
            // newDirectoryMenuItem
            // 
            this.newDirectoryMenuItem.Image = global::DehaxOS.Properties.Resources.folder;
            this.newDirectoryMenuItem.Name = "newDirectoryMenuItem";
            this.newDirectoryMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newDirectoryMenuItem.Size = new System.Drawing.Size(240, 22);
            this.newDirectoryMenuItem.Text = "Папку";
            this.newDirectoryMenuItem.Click += new System.EventHandler(this.newDirectoryMenuItem_Click);
            // 
            // newTextFileMenuItem
            // 
            this.newTextFileMenuItem.Image = global::DehaxOS.Properties.Resources.TextFile;
            this.newTextFileMenuItem.Name = "newTextFileMenuItem";
            this.newTextFileMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.newTextFileMenuItem.Size = new System.Drawing.Size(240, 22);
            this.newTextFileMenuItem.Text = "Текстовый файл";
            this.newTextFileMenuItem.Click += new System.EventHandler(this.newTextFileMenuItem_Click);
            // 
            // openMetaFileMenuItem
            // 
            this.openMetaFileMenuItem.Name = "openMetaFileMenuItem";
            this.openMetaFileMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMetaFileMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openMetaFileMenuItem.Text = "Открыть";
            this.openMetaFileMenuItem.Click += new System.EventHandler(this.OpenMetaFileMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // deleteMetaFileMenuItem
            // 
            this.deleteMetaFileMenuItem.Name = "deleteMetaFileMenuItem";
            this.deleteMetaFileMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteMetaFileMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteMetaFileMenuItem.Text = "Удалить";
            this.deleteMetaFileMenuItem.Click += new System.EventHandler(this.deleteMetaFileMenuItem_Click);
            // 
            // renameMenuItem
            // 
            this.renameMenuItem.Name = "renameMenuItem";
            this.renameMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.renameMenuItem.Size = new System.Drawing.Size(180, 22);
            this.renameMenuItem.Text = "Переименовать";
            this.renameMenuItem.Click += new System.EventHandler(this.renameMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // propertiesMenuItem
            // 
            this.propertiesMenuItem.Name = "propertiesMenuItem";
            this.propertiesMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.propertiesMenuItem.Size = new System.Drawing.Size(180, 22);
            this.propertiesMenuItem.Text = "Свойства...";
            this.propertiesMenuItem.Click += new System.EventHandler(this.propertiesMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // formatMenuItem
            // 
            this.formatMenuItem.Name = "formatMenuItem";
            this.formatMenuItem.Size = new System.Drawing.Size(180, 22);
            this.formatMenuItem.Text = "Форматировать...";
            this.formatMenuItem.Click += new System.EventHandler(this.formatMenuItem_Click);
            // 
            // schedulerMenuItem
            // 
            this.schedulerMenuItem.Name = "schedulerMenuItem";
            this.schedulerMenuItem.Size = new System.Drawing.Size(99, 20);
            this.schedulerMenuItem.Text = "Планировщик";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemStateStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 311);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(312, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "Статусная строка";
            // 
            // systemStateStatusLabel
            // 
            this.systemStateStatusLabel.Name = "systemStateStatusLabel";
            this.systemStateStatusLabel.Size = new System.Drawing.Size(133, 17);
            this.systemStateStatusLabel.Text = "<Состояние системы>";
            this.systemStateStatusLabel.ToolTipText = "Показывает текущее состояние системы.";
            // 
            // fileSystemTreeView
            // 
            this.fileSystemTreeView.ContextMenuStrip = this.fileSystemContextMenuStrip;
            this.fileSystemTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileSystemTreeView.Location = new System.Drawing.Point(0, 24);
            this.fileSystemTreeView.Name = "fileSystemTreeView";
            this.fileSystemTreeView.PathSeparator = "/";
            this.fileSystemTreeView.Size = new System.Drawing.Size(312, 287);
            this.fileSystemTreeView.TabIndex = 2;
            this.fileSystemTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.FileSystemTreeView_BeforeExpand);
            // 
            // fileSystemContextMenuStrip
            // 
            this.fileSystemContextMenuStrip.Name = "fileSystemContextMenuStrip";
            this.fileSystemContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            this.fileSystemContextMenuStrip.Text = "Контекстное меню";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 333);
            this.Controls.Add(this.fileSystemTreeView);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Эмулятор ОС \"DehaxOS\"";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem dehaxOSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeUserMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel systemStateStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem fileSystemMenuItem;
        private System.Windows.Forms.ToolStripMenuItem schedulerMenuItem;
        private System.Windows.Forms.TreeView fileSystemTreeView;
        private System.Windows.Forms.ToolStripMenuItem createMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newDirectoryMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTextFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMetaFileMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteMetaFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem propertiesMenuItem;
        private System.Windows.Forms.ContextMenuStrip fileSystemContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem usersMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem formatMenuItem;
    }
}

