using DehaxOS.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            DehaxFileSystem dfs = new DehaxFileSystem(new FileStream(@"C:\Users\Dehax\OneDrive\Documents\DonNTU\OS\Project\DehaxOS\image.dfs", FileMode.Open, FileAccess.ReadWrite, FileShare.Read, 512, FileOptions.RandomAccess));

        }
    }
}
