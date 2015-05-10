namespace AdminPanel
{
    partial class DatabaseView
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.metroTabControl1 = new MetroFramework.Controls.MetroTabControl();
            this.accountsTabPage = new MetroFramework.Controls.MetroTabPage();
            this.accountsTableGrid = new MetroFramework.Controls.MetroGrid();
            this.charsTabPage = new MetroFramework.Controls.MetroTabPage();
            this.deathsTabPage = new MetroFramework.Controls.MetroTabPage();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.charactersTableGrid = new MetroFramework.Controls.MetroGrid();
            this.metroTabControl1.SuspendLayout();
            this.accountsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accountsTableGrid)).BeginInit();
            this.charsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.charactersTableGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.Controls.Add(this.accountsTabPage);
            this.metroTabControl1.Controls.Add(this.charsTabPage);
            this.metroTabControl1.Controls.Add(this.deathsTabPage);
            this.metroTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl1.Location = new System.Drawing.Point(20, 60);
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 0;
            this.metroTabControl1.Size = new System.Drawing.Size(1081, 526);
            this.metroTabControl1.TabIndex = 0;
            this.metroTabControl1.UseSelectable = true;
            this.metroTabControl1.UseStyleColors = true;
            // 
            // accountsTabPage
            // 
            this.accountsTabPage.Controls.Add(this.accountsTableGrid);
            this.accountsTabPage.HorizontalScrollbarBarColor = true;
            this.accountsTabPage.HorizontalScrollbarHighlightOnWheel = false;
            this.accountsTabPage.HorizontalScrollbarSize = 0;
            this.accountsTabPage.Location = new System.Drawing.Point(4, 38);
            this.accountsTabPage.Name = "accountsTabPage";
            this.accountsTabPage.Size = new System.Drawing.Size(1073, 484);
            this.accountsTabPage.TabIndex = 0;
            this.accountsTabPage.Text = "Accounts";
            this.accountsTabPage.VerticalScrollbarBarColor = true;
            this.accountsTabPage.VerticalScrollbarHighlightOnWheel = false;
            this.accountsTabPage.VerticalScrollbarSize = 0;
            // 
            // accountsTableGrid
            // 
            this.accountsTableGrid.AllowUserToAddRows = false;
            this.accountsTableGrid.AllowUserToDeleteRows = false;
            this.accountsTableGrid.AllowUserToResizeRows = false;
            this.accountsTableGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.accountsTableGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.accountsTableGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.accountsTableGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.accountsTableGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.accountsTableGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.accountsTableGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.accountsTableGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountsTableGrid.EnableHeadersVisualStyles = false;
            this.accountsTableGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.accountsTableGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.accountsTableGrid.Location = new System.Drawing.Point(0, 0);
            this.accountsTableGrid.Name = "accountsTableGrid";
            this.accountsTableGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.accountsTableGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.accountsTableGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.accountsTableGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.accountsTableGrid.Size = new System.Drawing.Size(1073, 484);
            this.accountsTableGrid.TabIndex = 2;
            // 
            // charsTabPage
            // 
            this.charsTabPage.Controls.Add(this.charactersTableGrid);
            this.charsTabPage.HorizontalScrollbarBarColor = true;
            this.charsTabPage.HorizontalScrollbarHighlightOnWheel = false;
            this.charsTabPage.HorizontalScrollbarSize = 0;
            this.charsTabPage.Location = new System.Drawing.Point(4, 38);
            this.charsTabPage.Name = "charsTabPage";
            this.charsTabPage.Size = new System.Drawing.Size(1073, 484);
            this.charsTabPage.TabIndex = 1;
            this.charsTabPage.Text = "Characters";
            this.charsTabPage.VerticalScrollbarBarColor = true;
            this.charsTabPage.VerticalScrollbarHighlightOnWheel = false;
            this.charsTabPage.VerticalScrollbarSize = 0;
            // 
            // deathsTabPage
            // 
            this.deathsTabPage.HorizontalScrollbarBarColor = true;
            this.deathsTabPage.HorizontalScrollbarHighlightOnWheel = false;
            this.deathsTabPage.HorizontalScrollbarSize = 0;
            this.deathsTabPage.Location = new System.Drawing.Point(4, 38);
            this.deathsTabPage.Name = "deathsTabPage";
            this.deathsTabPage.Size = new System.Drawing.Size(1073, 484);
            this.deathsTabPage.TabIndex = 2;
            this.deathsTabPage.Text = "Deaths";
            this.deathsTabPage.VerticalScrollbarBarColor = true;
            this.deathsTabPage.VerticalScrollbarHighlightOnWheel = false;
            this.deathsTabPage.VerticalScrollbarSize = 0;
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            // 
            // charactersTableGrid
            // 
            this.charactersTableGrid.AllowUserToResizeRows = false;
            this.charactersTableGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.charactersTableGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.charactersTableGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.charactersTableGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.charactersTableGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.charactersTableGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.charactersTableGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.charactersTableGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.charactersTableGrid.EnableHeadersVisualStyles = false;
            this.charactersTableGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.charactersTableGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.charactersTableGrid.Location = new System.Drawing.Point(0, 0);
            this.charactersTableGrid.Name = "charactersTableGrid";
            this.charactersTableGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.charactersTableGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.charactersTableGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.charactersTableGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.charactersTableGrid.Size = new System.Drawing.Size(1073, 484);
            this.charactersTableGrid.TabIndex = 2;
            // 
            // DatabaseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 606);
            this.Controls.Add(this.metroTabControl1);
            this.Name = "DatabaseView";
            this.Style = MetroFramework.MetroColorStyle.Default;
            this.Text = "DatabaseView";
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            this.metroTabControl1.ResumeLayout(false);
            this.accountsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accountsTableGrid)).EndInit();
            this.charsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.charactersTableGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTabControl metroTabControl1;
        private MetroFramework.Controls.MetroTabPage accountsTabPage;
        private MetroFramework.Controls.MetroTabPage charsTabPage;
        private MetroFramework.Controls.MetroTabPage deathsTabPage;
        private MetroFramework.Controls.MetroGrid accountsTableGrid;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Controls.MetroGrid charactersTableGrid;
    }
}