#region

using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using db;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

#endregion

namespace server.account
{
    internal class setName : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);
                byte[] status = new byte[0];
                if (CheckAccount(acc, db))
                {
                    if (!acc.NameChosen)
                    {
                        if (Regex.IsMatch(Query["name"], @"^[a-zA-Z]+$"))
                        {
                            MySqlCommand cmd = db.CreateQuery();
                            object exescala;
                            cmd.CommandText = "SELECT COUNT(name) FROM accounts WHERE name=@name;";
                            cmd.Parameters.AddWithValue("@name", Query["name"]);
                            exescala = cmd.ExecuteScalar();
                            if (int.Parse(exescala.ToString()) > 0)
                                status = Encoding.UTF8.GetBytes("<Error>Duplicated name</Error>");
                            else
                            {
                                cmd = db.CreateQuery();
                                cmd.CommandText = "UPDATE accounts SET name=@name, namechosen=TRUE WHERE id=@accId;";
                                cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                                cmd.Parameters.AddWithValue("@name", Query["name"]);
                                if (cmd.ExecuteNonQuery() != 0)
                                    status = Encoding.UTF8.GetBytes("<Success />");
                                else
                                    status = Encoding.UTF8.GetBytes("<Error>Internal error</Error>");
                            }
                        }
                        else
                            status = Encoding.UTF8.GetBytes("<Error>Invalid name</Error>");
                    }
                    else
                        status = Encoding.UTF8.GetBytes("<Error>You have already a name</Error>");
                }
                Context.Response.OutputStream.Write(status, 0, status.Length);
            }
        }
    }
}