#region

using db;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

#endregion

namespace server.account
{
    internal class forgotPassword : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                string authKey = Database.GenerateRandomString(128);
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE accounts SET authToken=@authToken WHERE uuid=@email;";
                cmd.Parameters.AddWithValue("@authToken", authKey);
                cmd.Parameters.AddWithValue("@email", Query["guid"]);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MailMessage message = new MailMessage();
                    message.To.Add(Query["guid"]);
                    message.Subject = "Forgot Password";
                    message.From = new MailAddress(Program.Settings.GetValue<string>("serverEmail", ""), "Forgot Passowrd");
                    message.Body = emailBody.
                        Replace("{RPLINK}", String.Format("{0}/{1}{2}", Program.Settings.GetValue<string>("serverDomain", "localhost"), "account/resetPassword?authToken=", authKey)).
                        Replace("{SUPPORTLINK}", String.Format("{0}", Program.Settings.GetValue<string>("supportLink", "localhost"))).
                        Replace("{SERVERDOMAIN}", Program.Settings.GetValue<string>("serverDomain", "localhost"));

                    Program.SendEmail(message, true);
                }
                else
                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                        wtr.Write("<Error>Error.accountNotFound</Error>");
            }
        }

        const string emailBody = @"Hello,

If your wish to reset your password in Fabiano Swagger of Doom, please use the 
link below:

{RPLINK}

If you do NOT wish to reset your password, do nothing.

Do not reply to this email, it will not be read. If you need support, go 
here:

{SUPPORTLINK}

- Fabiano Swagger of Doom Team
{SERVERDOMAIN}";
    }
}