using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DFSformat
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            //args = Environment.GetCommandLineArgs();

            //foreach (string s in args)
            //{
            //    MessageBox.Show(s);
            //}

            if (args.Length > 1)
            {
                Console.Error.WriteLine("Неверно заданы параметры!");
                Console.Error.WriteLine("Синтаксис:");
                Console.Error.WriteLine("DFSformat.exe [ПУТЬ_К_ОБРАЗУ_ДИСКА]");
                return 1;
            }

            string imagePath = args[0];

            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine("Указанный файл не существует!");
                return 2;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(imagePath));

            return 0;
        }
    }
}
