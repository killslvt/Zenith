using System.Windows.Forms;

namespace Zenith
{
    partial class StartUp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 
        private Label WelcomeLabel;
        private Label EvolveLabel;
        private PictureBox EvolveLogo;
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartUp));
            this.WelcomeLabel = new System.Windows.Forms.Label();
            this.EvolveLabel = new System.Windows.Forms.Label();
            this.EvolveLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.EvolveLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // WelcomeLabel
            // 
            this.WelcomeLabel.AutoSize = true;
            this.WelcomeLabel.BackColor = System.Drawing.Color.Transparent;
            this.WelcomeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.WelcomeLabel.Location = new System.Drawing.Point(250, 80);
            this.WelcomeLabel.Name = "WelcomeLabel";
            this.WelcomeLabel.Size = new System.Drawing.Size(64, 13);
            this.WelcomeLabel.TabIndex = 0;
            this.WelcomeLabel.Text = "Welcome to";
            // 
            // EvolveLabel
            // 
            this.EvolveLabel.AutoSize = true;
            this.EvolveLabel.BackColor = System.Drawing.Color.Transparent;
            this.EvolveLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold);
            this.EvolveLabel.ForeColor = System.Drawing.Color.Purple;
            this.EvolveLabel.Location = new System.Drawing.Point(250, 120);
            this.EvolveLabel.Name = "EvolveLabel";
            this.EvolveLabel.Size = new System.Drawing.Size(126, 39);
            this.EvolveLabel.TabIndex = 1;
            this.EvolveLabel.Text = "Evolve";
            // 
            // EvolveLogo
            // 
            this.EvolveLogo.BackColor = System.Drawing.Color.Transparent;
            this.EvolveLogo.ForeColor = System.Drawing.Color.Transparent;
            this.EvolveLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("EvolveLogo.BackgroundImage")));
            this.EvolveLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.EvolveLogo.Location = new System.Drawing.Point(77, 64);
            this.EvolveLogo.Name = "EvolveLogo";
            this.EvolveLogo.Size = new System.Drawing.Size(150, 150);
            this.EvolveLogo.TabIndex = 2;
            this.EvolveLogo.TabStop = false;
            // 
            // StartUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(500, 280);
            this.Controls.Add(this.EvolveLabel);
            this.Controls.Add(this.WelcomeLabel);
            this.Controls.Add(this.EvolveLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StartUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Startup";
            this.Load += new System.EventHandler(this.StartUpForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.EvolveLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}