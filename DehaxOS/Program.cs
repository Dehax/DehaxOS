using DehaxOS.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DehaxOS
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            byte[] data = Utils.GetPasswordHash("toor");
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            //string imageFilePath = @"C:\Users\Dehax\OneDrive\Documents\DonNTU\OS\Project\DehaxOS\image.dfs";
            //int bufferSize = 512;
            //DehaxFileSystem dfs = new DehaxFileSystem(new FileStream(imageFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, bufferSize, FileOptions.RandomAccess), 1, 1);
        }
    }
}
