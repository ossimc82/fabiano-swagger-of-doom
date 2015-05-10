namespace Client_Updater
{
    partial class Form1
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
            this.localhost_btn = new System.Windows.Forms.Button();
            this.c453_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // localhost_btn
            // 
            this.localhost_btn.Location = new System.Drawing.Point(12, 12);
            this.localhost_btn.Name = "localhost_btn";
            this.localhost_btn.Size = new System.Drawing.Size(127, 23);
            this.localhost_btn.TabIndex = 0;
            this.localhost_btn.Text = "Localhost";
            this.localhost_btn.UseVisualStyleBackColor = true;
            this.localhost_btn.Click += new System.EventHandler(this.localhost_btn_Click);
            // 
            // c453_btn
            // 
            this.c453_btn.Location = new System.Drawing.Point(145, 12);
            this.c453_btn.Name = "c453_btn";
            this.c453_btn.Size = new System.Drawing.Size(127, 23);
            this.c453_btn.TabIndex = 1;
            this.c453_btn.Text = "c453.pw";
            this.c453_btn.UseVisualStyleBackColor = true;
            this.c453_btn.Click += new System.EventHandler(this.c453_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Status: Waiting...";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(260, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "KrazyShank";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 100);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.c453_btn);
            this.Controls.Add(this.localhost_btn);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client Updater";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button localhost_btn;
        private System.Windows.Forms.Button c453_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}

