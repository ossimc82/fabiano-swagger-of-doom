namespace AdminPanel
{
    partial class CreatePackage
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.itemsList = new System.Windows.Forms.ListBox();
            this.charSlotsBox = new MetroFramework.Controls.MetroTextBox();
            this.vaultChestsBox = new MetroFramework.Controls.MetroTextBox();
            this.addItem = new MetroFramework.Controls.MetroButton();
            this.removeItem = new MetroFramework.Controls.MetroButton();
            this.saveBox = new MetroFramework.Controls.MetroButton();
            this.packageName = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.maxPurchase = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.weight = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.bgUrl = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.price = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.quantity = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel9 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel10 = new MetroFramework.Controls.MetroLabel();
            this.endDate = new MetroFramework.Controls.MetroDateTime();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(23, 66);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(71, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "Char Slots:";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(23, 95);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(82, 19);
            this.metroLabel2.TabIndex = 1;
            this.metroLabel2.Text = "Vault Chests:";
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(23, 127);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(43, 19);
            this.metroLabel3.TabIndex = 2;
            this.metroLabel3.Text = "Items:";
            // 
            // itemsList
            // 
            this.itemsList.FormattingEnabled = true;
            this.itemsList.Location = new System.Drawing.Point(111, 127);
            this.itemsList.Name = "itemsList";
            this.itemsList.Size = new System.Drawing.Size(256, 238);
            this.itemsList.TabIndex = 3;
            // 
            // charSlotsBox
            // 
            this.charSlotsBox.Lines = new string[] {
        "0"};
            this.charSlotsBox.Location = new System.Drawing.Point(111, 64);
            this.charSlotsBox.MaxLength = 11;
            this.charSlotsBox.Name = "charSlotsBox";
            this.charSlotsBox.PasswordChar = '\0';
            this.charSlotsBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.charSlotsBox.SelectedText = "";
            this.charSlotsBox.Size = new System.Drawing.Size(256, 23);
            this.charSlotsBox.TabIndex = 4;
            this.charSlotsBox.Text = "0";
            this.charSlotsBox.UseSelectable = true;
            // 
            // vaultChestsBox
            // 
            this.vaultChestsBox.Lines = new string[] {
        "0"};
            this.vaultChestsBox.Location = new System.Drawing.Point(111, 93);
            this.vaultChestsBox.MaxLength = 11;
            this.vaultChestsBox.Name = "vaultChestsBox";
            this.vaultChestsBox.PasswordChar = '\0';
            this.vaultChestsBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.vaultChestsBox.SelectedText = "";
            this.vaultChestsBox.Size = new System.Drawing.Size(256, 23);
            this.vaultChestsBox.TabIndex = 5;
            this.vaultChestsBox.Text = "0";
            this.vaultChestsBox.UseSelectable = true;
            // 
            // addItem
            // 
            this.addItem.Location = new System.Drawing.Point(111, 371);
            this.addItem.Name = "addItem";
            this.addItem.Size = new System.Drawing.Size(125, 31);
            this.addItem.TabIndex = 6;
            this.addItem.Text = "Add";
            this.addItem.UseSelectable = true;
            this.addItem.Click += new System.EventHandler(this.addItem_Click);
            // 
            // removeItem
            // 
            this.removeItem.Location = new System.Drawing.Point(242, 371);
            this.removeItem.Name = "removeItem";
            this.removeItem.Size = new System.Drawing.Size(125, 31);
            this.removeItem.TabIndex = 7;
            this.removeItem.Text = "Remove";
            this.removeItem.UseSelectable = true;
            this.removeItem.Click += new System.EventHandler(this.removeItem_Click);
            // 
            // saveBox
            // 
            this.saveBox.Location = new System.Drawing.Point(386, 278);
            this.saveBox.Name = "saveBox";
            this.saveBox.Size = new System.Drawing.Size(344, 124);
            this.saveBox.TabIndex = 8;
            this.saveBox.Text = "Save box";
            this.saveBox.UseSelectable = true;
            this.saveBox.Click += new System.EventHandler(this.saveBox_Click);
            // 
            // packageName
            // 
            this.packageName.Lines = new string[0];
            this.packageName.Location = new System.Drawing.Point(482, 64);
            this.packageName.MaxLength = 128;
            this.packageName.Name = "packageName";
            this.packageName.PasswordChar = '\0';
            this.packageName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.packageName.SelectedText = "";
            this.packageName.Size = new System.Drawing.Size(248, 23);
            this.packageName.TabIndex = 10;
            this.packageName.UseSelectable = true;
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(386, 66);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(48, 19);
            this.metroLabel4.TabIndex = 9;
            this.metroLabel4.Text = "Name:";
            // 
            // maxPurchase
            // 
            this.maxPurchase.Lines = new string[] {
        "-1"};
            this.maxPurchase.Location = new System.Drawing.Point(482, 95);
            this.maxPurchase.MaxLength = 11;
            this.maxPurchase.Name = "maxPurchase";
            this.maxPurchase.PasswordChar = '\0';
            this.maxPurchase.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.maxPurchase.SelectedText = "";
            this.maxPurchase.Size = new System.Drawing.Size(248, 23);
            this.maxPurchase.TabIndex = 12;
            this.maxPurchase.Text = "-1";
            this.maxPurchase.UseSelectable = true;
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(386, 97);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(93, 19);
            this.metroLabel5.TabIndex = 11;
            this.metroLabel5.Text = "Max Purchase:";
            // 
            // weight
            // 
            this.weight.Lines = new string[] {
        "-1"};
            this.weight.Location = new System.Drawing.Point(482, 127);
            this.weight.MaxLength = 11;
            this.weight.Name = "weight";
            this.weight.PasswordChar = '\0';
            this.weight.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.weight.SelectedText = "";
            this.weight.Size = new System.Drawing.Size(248, 23);
            this.weight.TabIndex = 14;
            this.weight.Text = "-1";
            this.weight.UseSelectable = true;
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(386, 129);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(54, 19);
            this.metroLabel6.TabIndex = 13;
            this.metroLabel6.Text = "Weight:";
            // 
            // bgUrl
            // 
            this.bgUrl.Lines = new string[] {
        "http://i.imgur.com/3NFqdr1.png"};
            this.bgUrl.Location = new System.Drawing.Point(482, 156);
            this.bgUrl.MaxLength = 512;
            this.bgUrl.Name = "bgUrl";
            this.bgUrl.PasswordChar = '\0';
            this.bgUrl.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.bgUrl.SelectedText = "";
            this.bgUrl.Size = new System.Drawing.Size(248, 23);
            this.bgUrl.TabIndex = 16;
            this.bgUrl.Text = "http://i.imgur.com/3NFqdr1.png";
            this.bgUrl.UseSelectable = true;
            // 
            // metroLabel7
            // 
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.Location = new System.Drawing.Point(386, 158);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(56, 19);
            this.metroLabel7.TabIndex = 15;
            this.metroLabel7.Text = "BG URL:";
            // 
            // price
            // 
            this.price.Lines = new string[] {
        "0"};
            this.price.Location = new System.Drawing.Point(482, 185);
            this.price.MaxLength = 11;
            this.price.Name = "price";
            this.price.PasswordChar = '\0';
            this.price.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.price.SelectedText = "";
            this.price.Size = new System.Drawing.Size(248, 23);
            this.price.TabIndex = 18;
            this.price.Text = "0";
            this.price.UseSelectable = true;
            // 
            // metroLabel8
            // 
            this.metroLabel8.AutoSize = true;
            this.metroLabel8.Location = new System.Drawing.Point(386, 187);
            this.metroLabel8.Name = "metroLabel8";
            this.metroLabel8.Size = new System.Drawing.Size(41, 19);
            this.metroLabel8.TabIndex = 17;
            this.metroLabel8.Text = "Price:";
            // 
            // quantity
            // 
            this.quantity.Lines = new string[] {
        "-1"};
            this.quantity.Location = new System.Drawing.Point(482, 214);
            this.quantity.MaxLength = 11;
            this.quantity.Name = "quantity";
            this.quantity.PasswordChar = '\0';
            this.quantity.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.quantity.SelectedText = "";
            this.quantity.Size = new System.Drawing.Size(248, 23);
            this.quantity.TabIndex = 20;
            this.quantity.Text = "-1";
            this.quantity.UseSelectable = true;
            // 
            // metroLabel9
            // 
            this.metroLabel9.AutoSize = true;
            this.metroLabel9.Location = new System.Drawing.Point(386, 216);
            this.metroLabel9.Name = "metroLabel9";
            this.metroLabel9.Size = new System.Drawing.Size(61, 19);
            this.metroLabel9.TabIndex = 19;
            this.metroLabel9.Text = "Quantity:";
            // 
            // metroLabel10
            // 
            this.metroLabel10.AutoSize = true;
            this.metroLabel10.Location = new System.Drawing.Point(386, 245);
            this.metroLabel10.Name = "metroLabel10";
            this.metroLabel10.Size = new System.Drawing.Size(65, 19);
            this.metroLabel10.TabIndex = 21;
            this.metroLabel10.Text = "End Date:";
            // 
            // endDate
            // 
            this.endDate.Location = new System.Drawing.Point(482, 243);
            this.endDate.MinimumSize = new System.Drawing.Size(0, 29);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(248, 29);
            this.endDate.TabIndex = 23;
            this.endDate.Value = new System.DateTime(2015, 5, 9, 0, 0, 0, 0);
            // 
            // CreatePackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 425);
            this.Controls.Add(this.endDate);
            this.Controls.Add(this.metroLabel10);
            this.Controls.Add(this.quantity);
            this.Controls.Add(this.metroLabel9);
            this.Controls.Add(this.price);
            this.Controls.Add(this.metroLabel8);
            this.Controls.Add(this.bgUrl);
            this.Controls.Add(this.metroLabel7);
            this.Controls.Add(this.weight);
            this.Controls.Add(this.metroLabel6);
            this.Controls.Add(this.maxPurchase);
            this.Controls.Add(this.metroLabel5);
            this.Controls.Add(this.packageName);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.saveBox);
            this.Controls.Add(this.removeItem);
            this.Controls.Add(this.addItem);
            this.Controls.Add(this.vaultChestsBox);
            this.Controls.Add(this.charSlotsBox);
            this.Controls.Add(this.itemsList);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Name = "CreatePackage";
            this.Text = "Create Package";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private System.Windows.Forms.ListBox itemsList;
        private MetroFramework.Controls.MetroTextBox charSlotsBox;
        private MetroFramework.Controls.MetroTextBox vaultChestsBox;
        private MetroFramework.Controls.MetroButton addItem;
        private MetroFramework.Controls.MetroButton removeItem;
        private MetroFramework.Controls.MetroButton saveBox;
        private MetroFramework.Controls.MetroTextBox packageName;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroTextBox maxPurchase;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroTextBox weight;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroTextBox bgUrl;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private MetroFramework.Controls.MetroTextBox price;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        private MetroFramework.Controls.MetroTextBox quantity;
        private MetroFramework.Controls.MetroLabel metroLabel9;
        private MetroFramework.Controls.MetroLabel metroLabel10;
        private MetroFramework.Controls.MetroDateTime endDate;
    }
}