using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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


        Settings ToggleSettings = new Settings();
        public static bool SettingsShown = false;


        public Base()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int borderRadius = 20;
            float borderThickness = 3f;
            Color borderColor = Color.Purple;

            using (GraphicsPath path = new GraphicsPath())
            {
                float halfBorderThickness = borderThickness / 2;

                path.AddArc(new RectangleF(halfBorderThickness, halfBorderThickness, borderRadius, borderRadius), 180, 90);
                path.AddArc(new RectangleF(Width - borderRadius - 1 - halfBorderThickness, halfBorderThickness, borderRadius, borderRadius), 270, 90);
                path.AddArc(new RectangleF(Width - borderRadius - 1 - halfBorderThickness, Height - borderRadius - 1 - halfBorderThickness, borderRadius, borderRadius), 0, 90);
                path.AddArc(new RectangleF(halfBorderThickness, Height - borderRadius - 1 - halfBorderThickness, borderRadius, borderRadius), 90, 90);
                path.CloseFigure();

                Region = new Region(path);

                using (Pen pen = new Pen(borderColor, borderThickness))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
            {
                m.Result = (IntPtr)HTCAPTION;
            }
        }

        #region Inject&Execute
        private void InjectBtnAsync(object sender, EventArgs e)
        {
            bool _Inject = false;
            foreach (Util.ProcInfo pinfo in Util.openProcessesByName("RobloxPlayerBeta.exe"))
            {
                if (!WindowsPlayer.isInjected())
                {
                    InjectionStatus injectionStatus = WindowsPlayer.injectPlayer(pinfo);
                    if (injectionStatus == InjectionStatus.SUCCESS)
                    {
                        _Inject = true;
                        MessageBox.Show("Zenith injected");
                        AppendMsg(richTextBox1, Color.Green, $"Succssfully Injected", true);
                        Thread.Sleep(1000);

                    }
                    else if (injectionStatus == InjectionStatus.ALREADY_INJECTING)
                    {
                        Thread.Sleep(250);
                        AppendMsg(richTextBox1, Color.Orange, $"Already Injected", true);
                    }
                    else if (injectionStatus == InjectionStatus.FAILED)
                    {
                        MessageBox.Show("Injection failed! Unknown error.");
                        AppendMsg(richTextBox1, Color.Red, $"Injection failed! Unknown error", true);

                    }
                    else if (injectionStatus == InjectionStatus.FAILED_ADMINISTRATOR_ACCESS)
                    {
                        MessageBox.Show("Please run CeleryInject.exe as an administrator");
                        AppendMsg(richTextBox1, Color.Red, $"Please run CeleryInject.exe as an administrator", true);

                    }
                }
                else
                {
                    WindowsPlayer.injectPlayer(pinfo);
                }
            }

            if (!_Inject)
            {
                MessageBox.Show("Please use Roblox web client");
                AppendMsg(richTextBox1, Color.Red, $"Please use Roblox web client", true);
            }
        }

        private void ExecuteBtn(object sender, EventArgs e)
        {
            Dictionary<string, string> scriptDictionary = new Dictionary<string, string>()
            {
                { "Dex()", "https://raw.githubusercontent.com/TheSeaweedMonster/Luau/main/scripts/dexv2.lua" },
                { "InfYield()", "https://raw.githubusercontent.com/killslvt/Zenith/master/Scripts/infyield.lua" },
                { "Esp()", "https://raw.githubusercontent.com/TheSeaweedMonster/Luau/main/scripts/unnamedesp.lua" },
                { "BallSpin()", "https://raw.githubusercontent.com/killslvt/Zenith/master/Scripts/ballspin.lua" },
                { "InfJump()", "https://raw.githubusercontent.com/killslvt/Zenith/master/Scripts/infjump.lua" }
            };

            if (scriptDictionary.TryGetValue(fastColoredTextBox1.Text, out string scriptUrl))
            {
                WindowsPlayer.sendScript($"loadstring(game:HttpGet('{scriptUrl}'))()");
                AppendMsg(richTextBox1, Color.Green, $"Succssfully Executed {scriptUrl}", true);
            }
            else
            {
                WindowsPlayer.sendScript(fastColoredTextBox1.Text);
                AppendMsg(richTextBox1, Color.Green, $"Succssfully Executed", true);
            }
        }

        #endregion

        #region Ect
        public static void AppendMsg(RichTextBox rtb, Color color, string text, bool autoTime)
        {
            rtb.BeginInvoke(new ThreadStart(() =>
            {
                lock (rtb)
                {
                    rtb.Focus();
                    if (rtb.TextLength > 100000) rtb.Clear();

                    var temp = new RichTextBox();
                    temp.SelectionColor = color;
                    if (autoTime)
                        temp.AppendText(DateTime.Now.ToString($"[HH:mm:ss] " + "[Z] - "));
                    temp.AppendText(text);
                    rtb.Select(rtb.Rtf.Length, 0);
                    rtb.SelectedRtf = temp.Rtf;
                }
            }));
        }

        private void ClearBtn(object sender, EventArgs e)
        {
            fastColoredTextBox1.Clear();
            richTextBox1.Clear();
            AppendMsg(richTextBox1, Color.White, $"Cleared Script & Console Log", true);
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
                AppendMsg(richTextBox1, Color.White, $"Opening {openFileDialog1.FileName}", true);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("RobloxPlayerBeta");

            foreach (Process process in processes)
            {
                process.Kill();
            }

            AppendMsg(richTextBox1, Color.Red, $"Killing Roblox", true);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            infSettings_Click(sender, e);
            AppendMsg(richTextBox1, Color.White, $"Opening Settings", true);
        }

        private void infSettings_Click(object sender, EventArgs e)
        {
            if (SettingsShown == false)
            {
                AddOwnedForm(ToggleSettings);
                ToggleSettings.Show();
                ToggleSettings.BringToFront();
            }
            else
            {
                ToggleSettings.Hide();
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

        private void fastColoredTextBox1_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/pMAsDK4Z9d");
        }

        private void topmosttoggle_Click(object sender, EventArgs e)
        {
            if (topmosttoggle.Checked)
            {
                TopMost = true;
            }
            else
            {
                TopMost = false;
            }
        }
    }
}