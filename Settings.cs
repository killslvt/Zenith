using DiscordRpcDemo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zenith.Injector;
using Zenith.RPC;

namespace Zenith
{
    public partial class Settings : Form
    {
        Point lastPoint;

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        public Settings()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
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

                // Adjusted to ensure the border is not clipped at the bottom
                path.AddArc(new RectangleF(halfBorderThickness, halfBorderThickness, borderRadius, borderRadius), 180, 90);
                path.AddArc(new RectangleF(this.Width - borderRadius - 1 - halfBorderThickness, halfBorderThickness, borderRadius, borderRadius), 270, 90);
                path.AddArc(new RectangleF(this.Width - borderRadius - 1 - halfBorderThickness, this.Height - borderRadius - 1 - halfBorderThickness, borderRadius, borderRadius), 0, 90);
                path.AddArc(new RectangleF(halfBorderThickness, this.Height - borderRadius - 1 - halfBorderThickness, borderRadius, borderRadius), 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);

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
            this.Invalidate();
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

        private void rpcToggle_Click(object sender, EventArgs e)
        {
            if (rpctoggle.Checked)
            {
                rpcstart.isEnabled = true;
                var RPC = new rpcstart();
                RPC.DiscordRPC();
            }
            else
            {
                rpcstart.isEnabled = false;
                DiscordRpc.Shutdown();
            }
        }
    }
}
