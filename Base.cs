using CeleryAPI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using _BF = CeleryAPI.ByfronPlayer;

namespace Zenith
{
    /*
     * This is just a base for the injector (Zenith is a name made by ilycross)
     * Make sure to use your own name for it
     * The injector uses Celery API, It work but not well
     * I wont update this much btw
     * If There are any errors please join the discord and let me know
     * Zenith Discord: https://discord.gg/pMAsDK4Z9d
    */

    public partial class Base : Form
    {
        Point lastPoint;


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
           int nLeftRect,
           int nTopRect,
           int nRightRect,
           int nBottomRect,
           int nWidthEllipse,
           int nHeightEllipse
        );

        public Base()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        #region Inject&Execute
        private async void InjectBtnAsync(object sender, EventArgs e)
        {
            await PerformInjectionAsync();
        }

        private async Task PerformInjectionAsync()
        {
            _BF.StartFileSystemAPI();
            await Task.Delay(1000);
            _BF.Inject();
        }

        private void ExecuteBtn(object sender, EventArgs e)
        {
            _BF.execute(fastColoredTextBox1.Text);
        }

        #endregion

        #region Ect
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

        private void button2_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }
        #endregion
    }
}
