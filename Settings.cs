using System;
using System.Collections.Generic;
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
    public partial class Settings : Form
    {
        Point lastPoint;

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

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

        public Settings()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        #region Settings
       

        #endregion

        #region Ect

        private void button2_Click(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ - ] ~ Closing Settings");
            Console.ResetColor();
            Hide();
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
                        temp.AppendText(DateTime.Now.ToString($"[HH:mm:ss] " + "[Zenith] - "));
                    temp.AppendText(text);
                    rtb.Select(rtb.Rtf.Length, 0);
                    rtb.SelectedRtf = temp.Rtf;
                }
            }));
        }
        #endregion

        private void fastColoredTextBox1_Load(object sender, EventArgs e)
        {
            
        }


        private void consoleToggle_Click(object sender, EventArgs e)
        {
            Base consoleInstance = new Base();
            if (consoletoggle.Checked)
            {
                consoleInstance.isConsoleOpen = true;
                consoleInstance.OpenConsole();
            }
            else
            {
                consoleInstance.isConsoleOpen = false;
                consoleInstance.CloseConsole();
            }
        }
    }
}
