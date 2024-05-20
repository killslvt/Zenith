namespace Zenith
{
    partial class Settings
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
        private void InitializeComponent()
        {
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.discord_rpc = new System.Windows.Forms.Label();
            this.consoletoggle = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Black;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.Red;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(327, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(29, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Black;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(292, 16);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(29, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "M";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Facon", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(15, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 33);
            this.label1.TabIndex = 8;
            this.label1.Text = "Zenith";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(132, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "v1.0.1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Location = new System.Drawing.Point(1, -5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(367, 50);
            this.panel1.TabIndex = 10;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // discord_rpc
            // 
            this.discord_rpc.AutoSize = true;
            this.discord_rpc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discord_rpc.ForeColor = System.Drawing.Color.Red;
            this.discord_rpc.Location = new System.Drawing.Point(56, 66);
            this.discord_rpc.Name = "discord_rpc";
            this.discord_rpc.Size = new System.Drawing.Size(59, 17);
            this.discord_rpc.TabIndex = 34;
            this.discord_rpc.Text = "Console";
            // 
            // consoletoggle
            // 
            this.consoletoggle.Animated = true;
            this.consoletoggle.BackColor = System.Drawing.Color.Transparent;
            this.consoletoggle.CheckedState.BorderColor = System.Drawing.Color.White;
            this.consoletoggle.CheckedState.BorderRadius = 2;
            this.consoletoggle.CheckedState.BorderThickness = 1;
            this.consoletoggle.CheckedState.FillColor = System.Drawing.Color.Black;
            this.consoletoggle.CheckMarkColor = System.Drawing.Color.Red;
            this.consoletoggle.ForeColor = System.Drawing.Color.White;
            this.consoletoggle.Location = new System.Drawing.Point(29, 64);
            this.consoletoggle.Name = "consoletoggle";
            this.consoletoggle.Size = new System.Drawing.Size(20, 20);
            this.consoletoggle.TabIndex = 33;
            this.consoletoggle.Text = "Console";
            this.consoletoggle.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.consoletoggle.UncheckedState.BorderRadius = 2;
            this.consoletoggle.UncheckedState.BorderThickness = 0;
            this.consoletoggle.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.consoletoggle.Click += new System.EventHandler(this.consoleToggle_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(26, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(265, 26);
            this.label3.TabIndex = 35;
            this.label3.Text = "This was added for debugging issues.\r\nYou can toggle it on but togging it off wil" +
    "l cause issues.";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(368, 400);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.discord_rpc);
            this.Controls.Add(this.consoletoggle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Settings";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label discord_rpc;
        private Guna.UI2.WinForms.Guna2CustomCheckBox consoletoggle;
        private System.Windows.Forms.Label label3;
    }
}

