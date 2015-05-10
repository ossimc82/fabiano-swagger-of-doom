namespace AdminPanel
{
    partial class AdminPanel
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.metroButton3 = new MetroFramework.Controls.MetroButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.metroButton9 = new MetroFramework.Controls.MetroButton();
            this.metroButton8 = new MetroFramework.Controls.MetroButton();
            this.metroButton7 = new MetroFramework.Controls.MetroButton();
            this.metroButton6 = new MetroFramework.Controls.MetroButton();
            this.metroButton4 = new MetroFramework.Controls.MetroButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.metroButton10 = new MetroFramework.Controls.MetroButton();
            this.metroButton11 = new MetroFramework.Controls.MetroButton();
            this.metroButton12 = new MetroFramework.Controls.MetroButton();
            this.metroButton5 = new MetroFramework.Controls.MetroButton();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.metroButton13 = new MetroFramework.Controls.MetroButton();
            this.metroButton14 = new MetroFramework.Controls.MetroButton();
            this.metroButton15 = new MetroFramework.Controls.MetroButton();
            this.metroButton16 = new MetroFramework.Controls.MetroButton();
            this.metroButton17 = new MetroFramework.Controls.MetroButton();
            this.metroButton18 = new MetroFramework.Controls.MetroButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroButton1
            // 
            this.metroButton1.Highlight = true;
            this.metroButton1.Location = new System.Drawing.Point(6, 125);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(136, 47);
            this.metroButton1.TabIndex = 0;
            this.metroButton1.TabStop = false;
            this.metroButton1.Text = "Reload \"server.cfg\"";
            this.metroButton1.UseMnemonic = false;
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroButton2
            // 
            this.metroButton2.Highlight = true;
            this.metroButton2.Location = new System.Drawing.Point(6, 19);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(136, 47);
            this.metroButton2.TabIndex = 1;
            this.metroButton2.TabStop = false;
            this.metroButton2.Text = "Restart Server";
            this.metroButton2.UseMnemonic = false;
            this.metroButton2.UseSelectable = true;
            this.metroButton2.Click += new System.EventHandler(this.metroButton2_Click);
            // 
            // metroButton3
            // 
            this.metroButton3.Highlight = true;
            this.metroButton3.Location = new System.Drawing.Point(6, 19);
            this.metroButton3.Name = "metroButton3";
            this.metroButton3.Size = new System.Drawing.Size(136, 47);
            this.metroButton3.TabIndex = 2;
            this.metroButton3.TabStop = false;
            this.metroButton3.Text = "Restart WServer";
            this.metroButton3.UseMnemonic = false;
            this.metroButton3.UseSelectable = true;
            this.metroButton3.Click += new System.EventHandler(this.metroButton3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.metroButton9);
            this.groupBox1.Controls.Add(this.metroButton8);
            this.groupBox1.Controls.Add(this.metroButton7);
            this.groupBox1.Controls.Add(this.metroButton6);
            this.groupBox1.Controls.Add(this.metroButton4);
            this.groupBox1.Controls.Add(this.metroButton3);
            this.groupBox1.Location = new System.Drawing.Point(23, 130);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 181);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WServer";
            // 
            // metroButton9
            // 
            this.metroButton9.Highlight = true;
            this.metroButton9.Location = new System.Drawing.Point(156, 125);
            this.metroButton9.Name = "metroButton9";
            this.metroButton9.Size = new System.Drawing.Size(136, 47);
            this.metroButton9.TabIndex = 7;
            this.metroButton9.TabStop = false;
            this.metroButton9.Text = "Edit \"wServer.cfg\"";
            this.metroButton9.UseMnemonic = false;
            this.metroButton9.UseSelectable = true;
            this.metroButton9.Click += new System.EventHandler(this.metroButton9_Click);
            // 
            // metroButton8
            // 
            this.metroButton8.Highlight = true;
            this.metroButton8.Location = new System.Drawing.Point(156, 72);
            this.metroButton8.Name = "metroButton8";
            this.metroButton8.Size = new System.Drawing.Size(136, 47);
            this.metroButton8.TabIndex = 6;
            this.metroButton8.TabStop = false;
            this.metroButton8.Text = "Start WServer";
            this.metroButton8.UseMnemonic = false;
            this.metroButton8.UseSelectable = true;
            this.metroButton8.Click += new System.EventHandler(this.metroButton8_Click);
            // 
            // metroButton7
            // 
            this.metroButton7.Highlight = true;
            this.metroButton7.Location = new System.Drawing.Point(156, 19);
            this.metroButton7.Name = "metroButton7";
            this.metroButton7.Size = new System.Drawing.Size(136, 47);
            this.metroButton7.TabIndex = 5;
            this.metroButton7.TabStop = false;
            this.metroButton7.Text = "Edit Database";
            this.metroButton7.UseMnemonic = false;
            this.metroButton7.UseSelectable = true;
            this.metroButton7.Click += new System.EventHandler(this.metroButton7_Click);
            // 
            // metroButton6
            // 
            this.metroButton6.Highlight = true;
            this.metroButton6.Location = new System.Drawing.Point(6, 72);
            this.metroButton6.Name = "metroButton6";
            this.metroButton6.Size = new System.Drawing.Size(136, 47);
            this.metroButton6.TabIndex = 4;
            this.metroButton6.TabStop = false;
            this.metroButton6.Text = "Stop WServer";
            this.metroButton6.UseMnemonic = false;
            this.metroButton6.UseSelectable = true;
            this.metroButton6.Click += new System.EventHandler(this.metroButton6_Click);
            // 
            // metroButton4
            // 
            this.metroButton4.Highlight = true;
            this.metroButton4.Location = new System.Drawing.Point(6, 125);
            this.metroButton4.Name = "metroButton4";
            this.metroButton4.Size = new System.Drawing.Size(136, 47);
            this.metroButton4.TabIndex = 3;
            this.metroButton4.TabStop = false;
            this.metroButton4.Text = "Reload \"wServer.cfg\"";
            this.metroButton4.UseMnemonic = false;
            this.metroButton4.UseSelectable = true;
            this.metroButton4.Click += new System.EventHandler(this.metroButton4_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.metroButton10);
            this.groupBox2.Controls.Add(this.metroButton11);
            this.groupBox2.Controls.Add(this.metroButton12);
            this.groupBox2.Controls.Add(this.metroButton5);
            this.groupBox2.Controls.Add(this.metroButton1);
            this.groupBox2.Controls.Add(this.metroButton2);
            this.groupBox2.Location = new System.Drawing.Point(327, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 181);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server";
            // 
            // metroButton10
            // 
            this.metroButton10.Highlight = true;
            this.metroButton10.Location = new System.Drawing.Point(156, 72);
            this.metroButton10.Name = "metroButton10";
            this.metroButton10.Size = new System.Drawing.Size(136, 47);
            this.metroButton10.TabIndex = 5;
            this.metroButton10.TabStop = false;
            this.metroButton10.Text = "null";
            this.metroButton10.UseMnemonic = false;
            this.metroButton10.UseSelectable = true;
            // 
            // metroButton11
            // 
            this.metroButton11.Highlight = true;
            this.metroButton11.Location = new System.Drawing.Point(156, 125);
            this.metroButton11.Name = "metroButton11";
            this.metroButton11.Size = new System.Drawing.Size(136, 47);
            this.metroButton11.TabIndex = 3;
            this.metroButton11.TabStop = false;
            this.metroButton11.Text = "null";
            this.metroButton11.UseMnemonic = false;
            this.metroButton11.UseSelectable = true;
            // 
            // metroButton12
            // 
            this.metroButton12.Highlight = true;
            this.metroButton12.Location = new System.Drawing.Point(156, 19);
            this.metroButton12.Name = "metroButton12";
            this.metroButton12.Size = new System.Drawing.Size(136, 47);
            this.metroButton12.TabIndex = 4;
            this.metroButton12.TabStop = false;
            this.metroButton12.Text = "null";
            this.metroButton12.UseMnemonic = false;
            this.metroButton12.UseSelectable = true;
            // 
            // metroButton5
            // 
            this.metroButton5.Highlight = true;
            this.metroButton5.Location = new System.Drawing.Point(6, 72);
            this.metroButton5.Name = "metroButton5";
            this.metroButton5.Size = new System.Drawing.Size(136, 47);
            this.metroButton5.TabIndex = 2;
            this.metroButton5.TabStop = false;
            this.metroButton5.Text = "Stop Server";
            this.metroButton5.UseMnemonic = false;
            this.metroButton5.UseSelectable = true;
            this.metroButton5.Click += new System.EventHandler(this.metroButton5_Click);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(24, 64);
            this.metroLabel1.MaximumSize = new System.Drawing.Size(600, 0);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(98, 19);
            this.metroLabel1.TabIndex = 5;
            this.metroLabel1.Text = "Server Path: {0}";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(24, 83);
            this.metroLabel2.MaximumSize = new System.Drawing.Size(600, 0);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(111, 19);
            this.metroLabel2.TabIndex = 6;
            this.metroLabel2.Text = "WServer Path: {0}";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.metroButton13);
            this.groupBox3.Controls.Add(this.metroButton14);
            this.groupBox3.Controls.Add(this.metroButton15);
            this.groupBox3.Controls.Add(this.metroButton16);
            this.groupBox3.Controls.Add(this.metroButton17);
            this.groupBox3.Controls.Add(this.metroButton18);
            this.groupBox3.Location = new System.Drawing.Point(631, 130);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(298, 181);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Misc";
            // 
            // metroButton13
            // 
            this.metroButton13.Highlight = true;
            this.metroButton13.Location = new System.Drawing.Point(156, 72);
            this.metroButton13.Name = "metroButton13";
            this.metroButton13.Size = new System.Drawing.Size(136, 47);
            this.metroButton13.TabIndex = 5;
            this.metroButton13.TabStop = false;
            this.metroButton13.Text = "null";
            this.metroButton13.UseMnemonic = false;
            this.metroButton13.UseSelectable = true;
            // 
            // metroButton14
            // 
            this.metroButton14.Highlight = true;
            this.metroButton14.Location = new System.Drawing.Point(156, 125);
            this.metroButton14.Name = "metroButton14";
            this.metroButton14.Size = new System.Drawing.Size(136, 47);
            this.metroButton14.TabIndex = 3;
            this.metroButton14.TabStop = false;
            this.metroButton14.Text = "null";
            this.metroButton14.UseMnemonic = false;
            this.metroButton14.UseSelectable = true;
            // 
            // metroButton15
            // 
            this.metroButton15.Highlight = true;
            this.metroButton15.Location = new System.Drawing.Point(156, 19);
            this.metroButton15.Name = "metroButton15";
            this.metroButton15.Size = new System.Drawing.Size(136, 47);
            this.metroButton15.TabIndex = 4;
            this.metroButton15.TabStop = false;
            this.metroButton15.Text = "null";
            this.metroButton15.UseMnemonic = false;
            this.metroButton15.UseSelectable = true;
            // 
            // metroButton16
            // 
            this.metroButton16.Highlight = true;
            this.metroButton16.Location = new System.Drawing.Point(6, 72);
            this.metroButton16.Name = "metroButton16";
            this.metroButton16.Size = new System.Drawing.Size(136, 47);
            this.metroButton16.TabIndex = 2;
            this.metroButton16.TabStop = false;
            this.metroButton16.Text = "null";
            this.metroButton16.UseMnemonic = false;
            this.metroButton16.UseSelectable = true;
            // 
            // metroButton17
            // 
            this.metroButton17.Highlight = true;
            this.metroButton17.Location = new System.Drawing.Point(6, 125);
            this.metroButton17.Name = "metroButton17";
            this.metroButton17.Size = new System.Drawing.Size(136, 47);
            this.metroButton17.TabIndex = 0;
            this.metroButton17.TabStop = false;
            this.metroButton17.Text = "null";
            this.metroButton17.UseMnemonic = false;
            this.metroButton17.UseSelectable = true;
            // 
            // metroButton18
            // 
            this.metroButton18.Highlight = true;
            this.metroButton18.Location = new System.Drawing.Point(6, 19);
            this.metroButton18.Name = "metroButton18";
            this.metroButton18.Size = new System.Drawing.Size(136, 47);
            this.metroButton18.TabIndex = 1;
            this.metroButton18.TabStop = false;
            this.metroButton18.Text = "Create Package";
            this.metroButton18.UseMnemonic = false;
            this.metroButton18.UseSelectable = true;
            this.metroButton18.Click += new System.EventHandler(this.metroButton18_Click);
            // 
            // AdminPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 330);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "AdminPanel";
            this.Style = MetroFramework.MetroColorStyle.Default;
            this.Text = "Admin Panel";
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroButton metroButton2;
        private MetroFramework.Controls.MetroButton metroButton3;
        private System.Windows.Forms.GroupBox groupBox1;
        private MetroFramework.Controls.MetroButton metroButton4;
        private System.Windows.Forms.GroupBox groupBox2;
        private MetroFramework.Controls.MetroButton metroButton6;
        private MetroFramework.Controls.MetroButton metroButton5;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroButton metroButton9;
        private MetroFramework.Controls.MetroButton metroButton8;
        private MetroFramework.Controls.MetroButton metroButton7;
        private MetroFramework.Controls.MetroButton metroButton10;
        private MetroFramework.Controls.MetroButton metroButton11;
        private MetroFramework.Controls.MetroButton metroButton12;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.GroupBox groupBox3;
        private MetroFramework.Controls.MetroButton metroButton13;
        private MetroFramework.Controls.MetroButton metroButton14;
        private MetroFramework.Controls.MetroButton metroButton15;
        private MetroFramework.Controls.MetroButton metroButton16;
        private MetroFramework.Controls.MetroButton metroButton17;
        private MetroFramework.Controls.MetroButton metroButton18;
    }
}

