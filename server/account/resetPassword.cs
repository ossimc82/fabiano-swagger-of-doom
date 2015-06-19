using db;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

namespace server.account
{
    internal class resetPassword : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                string password = Database.GenerateRandomString(10);
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE accounts SET password=SHA1(@password) WHERE authToken=@authToken;";
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@authToken", Query["authToken"]);
                bool success = cmd.ExecuteNonQuery() > 0;
                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                {
                    if (success)
                        wtr.Write(resetPasswordSuccess.Replace("{PASSWORD}", password).Replace("{SERVERDOMAIN}", Program.Settings.GetValue<string>("serverDomain", "localhost")));
                    else
                        wtr.Write(resetPasswordFailure);
                }
            }
        }

        private const string resetPasswordSuccess =
@"<html>
<body bgcolor=""#000000"">
    <div align=""center"">
        <font color=""#FFFFFF"">Your new password is {PASSWORD}, please note that passwords are CaSeSensItivE. Play the game <a href=""{SERVERDOMAIN}"">here</a>.</font>
    </div>
</body>
</html>";

        private const string resetPasswordFailure =
@"<html>
<body bgcolor=""#000000"">
    <div align=""center"">
        <font color=""#FFFFFF"">Ohhh something went wrong, please request a new password.</font>
    </div>
</body>
</html>";
    }
}
