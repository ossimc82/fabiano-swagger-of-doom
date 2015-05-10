using MetroFramework;
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
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace AdminPanel
{
    public partial class Login : MetroForm
    {
        private Account account;

        public Login()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(out Account user)
        {
            var ret = ShowDialog();
            user = this.account;
            return ret;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var webRequest = WebRequest.CreateHttp(String.Format("http://127.0.0.1/account/verify?guid={0}&password={1}", emailTextBox.Text, passwordTextBox.Text));
                using (StreamReader rdr = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    var xml = rdr.ReadToEnd();
                    if (xml == "<Error>WebChangePasswordDialog.passwordError</Error>")
                    {
                        DialogResult = DialogResult.None;
                        MetroMessageBox.Show(this, "\n\nAccount credentials not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var serializer = new XmlSerializer(typeof(Account));
                    account = (Account)serializer.Deserialize(new StringReader(xml));

                    if (!account.Admin)
                    {
                        DialogResult = DialogResult.None;
                        MetroMessageBox.Show(this, "\n\nYou are not an admin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    account.Password = passwordTextBox.Text;
                    account.Email = emailTextBox.Text;
                    DialogResult = DialogResult.OK;
                }
            }
            catch (WebException)
            {
                MetroMessageBox.Show(this, "\n\nUnable to contact server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
