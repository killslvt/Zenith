using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

        #region Console
        public bool isConsoleOpen = false;
        private ConsoleColor savedColor;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        public void OpenConsole()
        {
            AllocConsole();
            Console.Title = "Zenith v1.0.1";
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Console.SetError(new StreamWriter(Console.OpenStandardError()) { AutoFlush = true });
            Console.OutputEncoding = Encoding.UTF8;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                "══════════════════════════════════════════════════════════════════════════════════\n" +
                "░▒▓████████▓▒░ ░▒▓████████▓▒░ ░▒▓███████▓▒░  ░▒▓█▓▒░ ░▒▓████████▓▒░ ░▒▓█▓▒░░▒▓█▓▒░\n" +
                "       ░▒▓█▓▒░ ░▒▓█▓▒░        ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░    ░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░\n" +
                "     ░▒▓██▓▒░  ░▒▓█▓▒░        ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░    ░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░\n" +
                "    ▒▓██▓▒░    ░▒▓██████▓▒░   ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░    ░▒▓█▓▒░     ░▒▓████████▓▒░\n" +
                " ░▒▓██▓▒░      ░▒▓█▓▒░        ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░    ░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░\n" +
                "░▒▓█▓▒░        ░▒▓█▓▒░        ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░    ░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░\n" +
                "░▒▓████████▓▒░ ░▒▓████████▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░    ░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░\n" +
                "══════════════════════════════════════════════════════════════════════════════════\n" +
                "                                  Made By ilycross                                \n\n");
            Console.ResetColor();
        }

        public void CloseConsole()
        {
            Console.Clear();

            FreeConsole();

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Console.SetError(new StreamWriter(Console.OpenStandardError()) { AutoFlush = true });
            isConsoleOpen = false;
        }

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("user32.dll")]
        private static extern IntPtr GetConsoleWindow();
        #endregion

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
                        Thread.Sleep(1000);
                        if(isConsoleOpen == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"[ + ] ~ Successfully Injected");
                            Console.ResetColor();
                        }
                    }
                    else if (injectionStatus == InjectionStatus.ALREADY_INJECTING)
                    {
                        Thread.Sleep(250);
                    }
                    else if (injectionStatus == InjectionStatus.FAILED)
                    {
                        MessageBox.Show("Injection failed! Unknown error.");
                        if (isConsoleOpen == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[ - ] ~ Injection failed");
                            Console.ResetColor();
                        }
                    }
                    else if (injectionStatus == InjectionStatus.FAILED_ADMINISTRATOR_ACCESS)
                    {
                        MessageBox.Show("Please run CeleryInject.exe as an administrator");
                        if (isConsoleOpen == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[ - ] ~ Failed: Please Run CeleryInject.exe");
                            Console.ResetColor();
                        }
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ - ] ~ Failed: Please Run Roblox Web Client");
                Console.ResetColor();
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
            }
            else
            {
                WindowsPlayer.sendScript(fastColoredTextBox1.Text);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[ + ] ~ Succssfully Executed");
                Console.ResetColor();
            }
        }

        #endregion

        #region Ect
        private void ClearBtn(object sender, EventArgs e)
        {
            fastColoredTextBox1.Clear();
            Console.Clear();
        }

        private void FolderBtn(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[ + ] ~ Please Select A File");
            Console.ResetColor();
            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = Application.ExecutablePath,
                Title = "Open"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fastColoredTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[ Selected ] ~ {openFileDialog1.FileName}");
                Console.ResetColor();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("RobloxPlayerBeta");

            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[ + ] ~ Succssfully Killed Roblox");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ - ] ~ Failed To Kill Roblox {ex}");
                    Console.ResetColor();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            infSettings_Click(sender, e);
        }

        private void infSettings_Click(object sender, EventArgs e)
        {
            if (SettingsShown == false)
            {
                AddOwnedForm(ToggleSettings);
                ToggleSettings.Show();
                ToggleSettings.BringToFront();
                ToggleSettings.Location = Cursor.Position;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[ + ] ~ Opening Settings");
                Console.ResetColor();
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
    }
}
