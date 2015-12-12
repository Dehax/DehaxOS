using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DehaxOS
{
    static class Program
    {
#if DEBUG
        private const string FORMAT_DISK_TOOL_PATH = @"..\..\..\DFSformat\bin\Debug\DFSformat.exe";
#else
        private const string FORMAT_DISK_TOOL_PATH = @"DFSformat.exe";
#endif

        public static bool FormatDisk = false;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            if (System.IO.File.Exists(DehaxOS.FS_IMAGE_PATH))
            {
                Application.Run(new MainForm());
            }
            else
            {
                FormatDisk = true;
            }

            while (FormatDisk)
            {
                Process.Start(FORMAT_DISK_TOOL_PATH, Path.GetFullPath(DehaxOS.FS_IMAGE_PATH)).WaitForExit();
                //MessageBox.Show("Форматирование завершено!", "Диск отформатирован", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FormatDisk = false;
                Application.Run(new MainForm());
            }

            //string imageFilePath = @"C:\Users\Dehax\OneDrive\Documents\DonNTU\OS\Project\DehaxOS\image.dfs";
            //int bufferSize = 512;
            //DehaxFileSystem dfs = new DehaxFileSystem(new FileStream(imageFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, bufferSize, FileOptions.RandomAccess), 1, 1);
        }
    }
}
