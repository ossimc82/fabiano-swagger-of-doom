using System;
using System.Windows.Forms;

namespace Client_Updater
{
    public partial class Form1 : Form
    {
        private ClientUpdater updater;

        public Form1()
        {
            InitializeComponent();
        }

        private void localhost_btn_Click(object sender, EventArgs e)
        {
            updater = new ClientUpdater("127.0.0.1", label1);
            updater.UpdateClient();
        }

        private void c453_btn_Click(object sender, EventArgs e)
        {
            updater = new ClientUpdater("25.108.113.162", label1);
            updater.UpdateClient();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updater = new ClientUpdater("71.231.167.96", label1);
            updater.UpdateClient();
        }
    }
}
