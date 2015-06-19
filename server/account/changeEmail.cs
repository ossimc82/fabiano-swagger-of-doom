using db;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace server.account
{
    internal class changeEmail : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);

                if (CheckAccount(acc, db))
                {
                    if (acc.VerifiedEmail || !Program.Settings.GetValue<bool>("verifyEmail")) return;
                    string authKey = Database.GenerateRandomString(128);
                    var cmd = db.CreateQuery();
                    cmd.CommandText = "UPDATE accounts SET uuid=@newGuid, authToken=@newAuthToken WHERE uuid=@oldGuid;";
                    cmd.Parameters.AddWithValue("@newGuid", Query["newGuid"]);
                    cmd.Parameters.AddWithValue("@newAuthToken", authKey);
                    cmd.Parameters.AddWithValue("@oldGuid", Query["guid"]);
                    cmd.Parameters.AddWithValue("@password", Query["password"]);
                    cmd.ExecuteNonQuery();

                    MailMessage message = new MailMessage();
                    message.To.Add(Query["newGuid"]);
                    message.IsBodyHtml = true;
                    message.Subject = "Please verify your account.";
                    message.From = new MailAddress(Program.Settings.GetValue<string>("serverEmail", ""));
                    message.Body = "<center>Please verify your email via this <a href=\"" + Program.Settings.GetValue<string>("serverDomain", "localhost") + "/account/validateEmail?authToken=" + authKey + "\" target=\"_blank\">link</a>.</center>";
                    Program.SendEmail(message, true);
                }
            }
        }
    }
}
