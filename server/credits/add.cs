#region
using System.Net;
using System.Text;

#endregion

namespace server.credits
{
    internal class add : RequestHandler
    {
        protected override void HandleRequest()
        {
            string status = "Nope, fuck you faggot :D";
            //using (Database db = new Database())
            //{
            //    NameValueCollection query = HttpUtility.ParseQueryString(context.Request.Url.Query);

            //    MySqlCommand cmd = db.CreateQuery();
            //    cmd.CommandText = "SELECT id FROM accounts WHERE uuid=@uuid";
            //    cmd.Parameters.AddWithValue("@uuid", query["guid"]);
            //    object id = cmd.ExecuteScalar();

            //    if (id != null)
            //    {
            //        int amount = int.Parse(query["jwt"]);
            //        cmd = db.CreateQuery();
            //        cmd.CommandText = "UPDATE stats SET credits = credits + @amount WHERE accId=@accId";
            //        cmd.Parameters.AddWithValue("@accId", (int) id);
            //        cmd.Parameters.AddWithValue("@amount", amount);
            //        int result = cmd.ExecuteNonQuery();
            //        if (result > 0)
            //            status = "Ya done...";
            //        else
            //            status = "Internal error :(";
            //    }
            //    else
            //        status = "Account not exists :(";
            //}

            byte[] res = Encoding.UTF8.GetBytes(
                @"<html>
    <head>
        <title>Nope...</title>
    </head>
    <body style='background: #333333'>
        <h1 style='color: #EEEEEE; text-align: center'>
            " + status + @"
        </h1>
    </body>
</html>");
            Context.Response.OutputStream.Write(res, 0, res.Length);
        }
    }
}