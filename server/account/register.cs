#region

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using db;
using MySql.Data.MySqlClient;
using System.Net.Mail;

#endregion

namespace server.account
{
    internal class register : RequestHandler
    {
        protected override void HandleRequest()
        {
            if (Query["ignore"] == null || !String.IsNullOrWhiteSpace(Query["entrytag"]) || String.IsNullOrWhiteSpace(Query["isAgeVerified"]) || !Query["newGUID"].Contains("@"))
            {
                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                    wtr.Write("<Error>WebRegister.invalid_email_address</Error>");
                return;
            }

            if (Query.AllKeys.Length != 6)
            {
                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                    wtr.Write("<Error>WebRegister.invalid_email_address</Error>");
                return;
            }

            if(!IsValidEmail(Query["newGuid"]))
            {
                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                    wtr.Write("<Error>WebRegister.invalid_email_address</Error>");
                return;
            }

            using (Database db = new Database())
            {
                byte[] status;
                if (!IsValidEmail(Query["newGUID"]))
                    status = Encoding.UTF8.GetBytes("<Error>WebForgotPasswordDialog.emailError</Error>");
                if (db.HasUuid(Query["guid"]) &&
                    !db.Verify(Query["guid"], "", Program.GameData).IsGuestAccount)
                {
                    if (db.HasUuid(Query["newGUID"]))
                        status = Encoding.UTF8.GetBytes("<Error>Error.emailAlreadyUsed</Error>");
                    else
                    {
                        MySqlCommand cmd = db.CreateQuery();
                        cmd.CommandText =
                            "UPDATE accounts SET uuid=@newUuid, name=@newUuid, password=SHA1(@password), guest=FALSE WHERE uuid=@uuid, name=@name;";
                        cmd.Parameters.AddWithValue("@uuid", Query["guid"]);
                        cmd.Parameters.AddWithValue("@newUuid", Query["newGUID"]);
                        cmd.Parameters.AddWithValue("@password", Query["newPassword"]);

                        if (cmd.ExecuteNonQuery() > 0)
                            status = Encoding.UTF8.GetBytes("<Success />");
                        else
                            status = Encoding.UTF8.GetBytes("<Error>Error.emailAlreadyUsed</Error>");
                    }
                }
                else
                {
                    Account acc = db.Register(Query["newGUID"], Query["newPassword"], false, Program.GameData);
                    if (acc != null)
                    {
                        if (Program.Settings.GetValue<bool>("verifyEmail"))
                        {
                            MailMessage message = new MailMessage();
                            message.To.Add(Query["newGuid"]);
                            message.IsBodyHtml = true;
                            message.Subject = "Please verify your account.";
                            message.From = new MailAddress(Program.Settings.GetValue<string>("serverEmail", ""));
                            message.Body = "<center>Please verify your email via this <a href=\"" + Program.Settings.GetValue<string>("serverDomain", "localhost") + "/account/validateEmail?authToken=" + acc.AuthToken + "\" target=\"_blank\">link</a>.</center>";
                            Program.SendEmail(message, true);
                        }
                        status = Encoding.UTF8.GetBytes("<Success/>");
                    }
                    else
                        status = Encoding.UTF8.GetBytes("<Error>Error.emailAlreadyUsed</Error>");
                }
                Context.Response.OutputStream.Write(status, 0, status.Length);
            }
        }

        public bool IsValidEmail(string strIn)
        {
            var invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            MatchEvaluator DomainMapper = match =>
            {
                // IdnMapping class with default property values.
                IdnMapping idn = new IdnMapping();

                string domainName = match.Groups[2].Value;
                try
                {
                    domainName = idn.GetAscii(domainName);
                }
                catch (ArgumentException)
                {
                    invalid = true;
                }
                return match.Groups[1].Value + domainName;
            };

            // Use IdnMapping class to convert Unicode domain names. 
            strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper);
            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            if(Regex.IsMatch(strIn,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                      RegexOptions.IgnoreCase))
            {
                string s = strIn.Remove(0, strIn.IndexOf('@') + 1);
                string emailBody = s.Remove(s.IndexOf('.'));
                switch (emailBody)
                {
                    case "mail":
                    case "yandex":
                    case "yahoo":
                    case "aol":
                    case "eclipso":
                    case "maills":
                    case "gmail":
                    case "googlemail":
                    case "firemail":
                    case "maili":
                    case "hotmail":
                    case "emailn":
                    case "outlook":
                    case "rediffmail":
                    case "oyoony":
                    case "lycos":
                    case "directbox":
                    case "new-post":
                    case "gmx":
                    case "slucia":
                    case "5x2":
                    case "smart-mail":
                    case "spl":
                    case "t-online":
                    case "compu-freemail":
                    case "web":
                    case "x-mail":
                    case "k":
                    case "mc-free":
                    case "freenet":
                    case "k-bg":
                    case "overmail":
                    case "anpa":
                    case "freemailer":
                    case "vcmail":
                    case "mail4nature":
                    case "uims":
                    case "1vbb":
                    case "uni":
                    case "techmail":
                    case "hushmail":
                    case "freemail-24":
                    case "guru":
                    case "email":
                    case "1email":
                    case "canineworld":
                    case "zelx":
                    case "sify":
                    case "softhome":
                    case "kuekomail":
                    case "mailde":
                    case "mail-king":
                    case "noxamail":
                    case "h3c":
                    case "arcor":
                    case "logomail":
                    case "ueberschuss":
                    case "chattler":
                    case "modellraketen":
                        return true;
                }
            }
            return false;
        }
    }
}