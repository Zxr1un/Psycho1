namespace PsychoAT
{
    partial class Start
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
            this.To_test_choise = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // To_test_choise
            // 
            this.To_test_choise.Animated = true;
            this.To_test_choise.BorderRadius = 20;
            this.To_test_choise.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.To_test_choise.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.To_test_choise.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.To_test_choise.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.To_test_choise.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(84)))), ((int)(((byte)(77)))));
            this.To_test_choise.Font = new System.Drawing.Font("Cascadia Mono SemiBold", 25.2F, System.Drawing.FontStyle.Bold);
            this.To_test_choise.ForeColor = System.Drawing.Color.White;
            this.To_test_choise.Location = new System.Drawing.Point(240, 156);
            this.To_test_choise.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.To_test_choise.Name = "To_test_choise";
            this.To_test_choise.Size = new System.Drawing.Size(256, 110);
            this.To_test_choise.TabIndex = 0;
            this.To_test_choise.Text = "Тесты";
            this.To_test_choise.Click += new System.EventHandler(this.To_test_choise_Click);
            // 
            // guna2Button2
            // 
            this.guna2Button2.Animated = true;
            this.guna2Button2.BorderRadius = 20;
            this.guna2Button2.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(84)))), ((int)(((byte)(77)))));
            this.guna2Button2.Font = new System.Drawing.Font("Cascadia Mono SemiBold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.Location = new System.Drawing.Point(240, 307);
            this.guna2Button2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.ShadowDecoration.BorderRadius = 90;
            this.guna2Button2.ShadowDecoration.Color = System.Drawing.Color.Silver;
            this.guna2Button2.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(10);
            this.guna2Button2.Size = new System.Drawing.Size(256, 110);
            this.guna2Button2.TabIndex = 1;
            this.guna2Button2.Text = "Статистика";
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
            // 
            // Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(199)))), ((int)(((byte)(167)))));
            this.ClientSize = new System.Drawing.Size(719, 562);
            this.Controls.Add(this.guna2Button2);
            this.Controls.Add(this.To_test_choise);
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "Start";
            this.Text = "Start";
            this.Load += new System.EventHandler(this.Start_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button To_test_choise;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
    }
}