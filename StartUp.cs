using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zenith
{
    public partial class StartUp : Form
    {
        public StartUp()
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

        private async void StartUpForm_Load(object sender, EventArgs e)
        {
            WelcomeLabel.Location = new Point(600, 80); 
            EvolveLabel.Location = new Point(600, 120);
            EvolveLogo.Location = new Point(-250, 65);

            await Task.Delay(500);

            // Simplified animation example
            AnimateControl(WelcomeLabel, new Point(270, 80), 700);
            await Task.Delay(100);
            AnimateControl(EvolveLabel, new Point(270, 120), 700);
            await Task.Delay(100);
            AnimateControl(EvolveLogo, new Point(60, 65), 700);

            await Task.Delay(1500);

            // Closing
            AnimateControl(WelcomeLabel, new Point(600, 80), 700);
            await Task.Delay(100);
            AnimateControl(EvolveLabel, new Point(600, 120), 700);
            await Task.Delay(100);
            AnimateControl(EvolveLogo, new Point(-250, 65), 700);

            await Task.Delay(500);
            this.Hide();
            Base @base = new Base();
            @base.Show();
        }

        private void AnimateControl(Control control, Point target, int duration)
        {
            var timer = new Timer { Interval = 10 };
            var start = control.Location;
            var startTime = DateTime.Now;
            timer.Tick += (s, e) =>
            {
                var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                if (elapsed >= duration)
                {
                    control.Location = target;
                    timer.Stop();
                }
                else
                {
                    var t = (float)elapsed / duration;
                    control.Location = new Point(
                        (int)(start.X + (target.X - start.X) * t),
                        (int)(start.Y + (target.Y - start.Y) * t));
                }
            };
            timer.Start();
        }
    }
}
