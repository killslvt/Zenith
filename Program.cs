using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zenith
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CeleryFix(); //For the (cant find myfile) error
            Application.Run(new Base());
        }

        static void CeleryFix()
        {
            string userName = Environment.UserName;
            string folderPath = $@"C:\Users\{userName}\AppData\Local\Temp\celery";

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating folder: {ex.Message}");
            }
        }
    }
}
