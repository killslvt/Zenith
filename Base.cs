using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zenith.Injector;

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
        private void InjectBtnAsync(object sender, EventArgs e)
        {
            bool _Inject = false;
            foreach (Util.ProcInfo pinfo in Util.openProcessesByName("RobloxPlayerBeta.exe"))
            {
                if (WindowsPlayer.isInjected())
                {
                    continue;
                }

                InjectionStatus injectionStatus = WindowsPlayer.injectPlayer(pinfo);
                switch (injectionStatus)
                {
                    case InjectionStatus.SUCCESS:
                        _Inject = true;
                        MessageBox.Show("Celery injected");
                        Thread.Sleep(1000);
                        break;
                    case InjectionStatus.ALREADY_INJECTING:
                        Thread.Sleep(250);
                        break;
                    case InjectionStatus.FAILED:
                        MessageBox.Show("Injection failed! Unknown error.");
                        break;
                    case InjectionStatus.FAILED_ADMINISTRATOR_ACCESS:
                        MessageBox.Show("Please run CeleryLauncher.exe as an administrator");
                        break;
                }
            }

            if (!_Inject)
            {
                MessageBox.Show("Please use Roblox web client");
            }
        }

        private void ExecuteBtn(object sender, EventArgs e)
        {
            WindowsPlayer.sendScript(fastColoredTextBox1.Text);
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
