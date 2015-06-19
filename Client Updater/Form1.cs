using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace Client_Updater
{
    public partial class Form1 : MetroForm
    {
        private ClientUpdater updater;

        public Form1()
        {
            InitializeComponent();
        }

        private void localhost_btn_Click(object sender, EventArgs e) => runUpdater("127.0.0.1");
        private void c453_btn_Click(object sender, EventArgs e) => runUpdater("25.108.113.162");
        private void button1_Click(object sender, EventArgs e) => runUpdater("71.231.167.96");

        private void runUpdater(string ip)
        {
            if (metroCheckBox1.Checked)
            {
                label1.Text = "Status: Downloading latest client...";
                label1.Update();
                var webCli = new WebClient();
                var clientVersion = Encoding.UTF8.GetString(webCli.DownloadData("https://realmofthemadgodhrd.appspot.com/version.txt"));
                webCli.DownloadFile($"https://realmofthemadgodhrd.appspot.com/AssembleeGameClient{clientVersion}.swf", "client.swf");
            }

            updater = new ClientUpdater(ip, label1);
            updater.UpdateClient();
        }
    }
}
