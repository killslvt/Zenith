using System;
using System.IO;
using System.Windows.Forms;
using _BF = CeleryAPI.ByfronPlayer;

namespace Zenith
{
    /*
     * This is just a base for the injector (Zenith is a name made by ilycross)
     * Make sure to use your own name for it
     * The injector uses Celery API, It work but not well
     * Zenith Discord: https://discord.gg/pMAsDK4Z9d
     * I wont update this much btw
    */
    public partial class Base : Form
    {
        public Base()
        {
            InitializeComponent();

            string userName = Environment.UserName;
            string folderPath = $@"C:\Users\{userName}\AppData\Local\Temp\celery";

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                else
                {
                    MessageBox.Show($"Error: Folder celery not found in C:\\Users\\{userName}\\AppData\\Local\\Temp\\celery");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating folder: {ex.Message}");
            }
        }

        #region Inject&Execute
        private void InjectBtn(object sender, EventArgs e)
        {
            _BF.Inject();
        }

        private void ExecuteBtn(object sender, EventArgs e)
        {
            _BF.execute(fastColoredTextBox1.Text);
        }

        #endregion

        private void ClearBtn(object sender, EventArgs e)
        {
            fastColoredTextBox1.Clear();
        }

        private void FolderBtn(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = Application.ExecutablePath,
                Title = "Open"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fastColoredTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);
            }
        }
    }
}
