using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace AdminPanel
{
    public partial class AdminPanel : MetroForm
    {
        private Account account;
        private string wServerPath;

        public AdminPanel()
        {
            InitializeComponent();

            var login = new Login();
            DialogResult res;
            while ((res = login.ShowDialog(out account)) != DialogResult.OK)
            {
                if (res != DialogResult.OK)
                {
                    Close();
                    return;
                }
            }

            Text = "Welcome " + account.Name;
            var vars = SendCommand("init").Split('\n');
            if (vars.Length < 3)
            {
                Close();
                return;
            }

            metroLabel1.Text = String.Format("Server Path: {0}", vars[1]);
            metroLabel2.Text = String.Format("WServer Path: {0}", wServerPath = vars[2]);

            toolTip1.SetToolTip(metroLabel1, vars[1]);
            toolTip1.SetToolTip(metroLabel2, vars[2]);
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            SendCommand("restartServer");
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            SendCommand("reloadServerCFG");
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            SendCommand("restartWServer");
        }

        private string SendCommand(string command)
        {
            try
            {
                var webRequest = WebRequest.CreateHttp(String.Format("http://127.0.0.1/admin/performCommand?guid={0}&password={1}&command={2}", account.Email, account.Password, command));
                using (StreamReader rdr = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    var xml = rdr.ReadToEnd().Trim();
                    if (xml == "<Error>WebChangePasswordDialog.passwordError</Error>")
                    {
                        MetroMessageBox.Show(this, "\n\nAccount credentials not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return xml;
                    }

                    if (xml.StartsWith("<Error>"))
                    {
                        MetroMessageBox.Show(this, "\n\n" + xml.Replace("<Error>", String.Empty).Replace("</Error>", String.Empty), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return xml;
                    }
                    if (!xml.StartsWith("NO_SHOW"))
                        MetroMessageBox.Show(this, "\n\n" + xml, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return xml.Replace("NO_SHOW", String.Empty);
                }
            }
            catch (WebException)
            {
                MetroMessageBox.Show(this, "\n\nUnable to contact server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            SendCommand("reloadWServerCFG");
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            SendCommand("stopServer");
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            SendCommand("stopWServer");
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            var edit = new EditForm("wServer.cfg", SendCommand("startEditWServerCFG&path=" + wServerPath.Remove(wServerPath.LastIndexOf('\\')) + "\\wServer.cfg"));
            if (edit.ShowDialog() == DialogResult.OK)
                SendCommand("endEditWServerCFG&path=" + wServerPath.Remove(wServerPath.LastIndexOf('\\')) + "\\wServer.cfg&content=" + HttpUtility.UrlEncode(edit.Content.Replace("\r\n", "\n")) + "");
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            SendCommand("startWServer&path=" + wServerPath);
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            MetroMessageBox.Show(this, "Comming soon");
            new DatabaseView(String.Format("Server={0};Database={1};uid={2};password={3};convert zero datetime=True;", "127.0.0.1", "rotmgprod", "root", "")).ShowDialog();
        }

        private void metroButton18_Click(object sender, EventArgs e)
        {
            var package = new CreatePackage();
            if (package.ShowDialog() == DialogResult.OK)
                SendCommand("createPackage&package=" + package.PackageResult);
        }
    }
}
