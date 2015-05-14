using db;
using db.JsonObjects;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace server.account
{
	internal class checkGiftCode : RequestHandler
	{
		protected override void HandleRequest()
		{
			using (Database db = new Database())
			{
				string jsonCode = String.Empty;
				string status = "Invalid code.";
				var cmd = db.CreateQuery();
				cmd.CommandText = "SELECT * FROM giftCodes WHERE code=@code";
				cmd.Parameters.AddWithValue("@code", Query["code"]);

				using (var rdr = cmd.ExecuteReader())
					while (rdr.Read())
						jsonCode = rdr.GetString("content");

                var list = ParseContents(jsonCode);
				if(list.Count > 0)
				{
					status = String.Empty;
					foreach (var i in list)
						 status += (i + "</br>");
				}

				byte[] res = new byte[0];
				if (status.IsNullOrWhiteSpace() || status == "Invalid code.")
				{
					res = Encoding.UTF8.GetBytes(
 $@"<html>
	<head>
		<title>Check Giftcode</title>
	</head>
	<body style='background: #333333'>
		<h1 style='color: #EEEEEE; text-align: center'>
			{status}
		</h1>
	</body>
</html>");
				}
				else
				{
					res = Encoding.UTF8.GetBytes(
 $@"<html>
	<head>
		<title>Check Giftcode</title>
	</head>
	<body style='background: #333333'>
		<h1 style='color: #EEEEEE; text-align: center'>
			Your Giftcode contains the following Items:
		</h1>
		<h3 style='color: #EEEEEE; text-align: center'>
			{status}
		</h3>
	</body>
</html>");
				}
				
				Context.Response.OutputStream.Write(res, 0, res.Length);
			}
		}

		private List<string> ParseContents(string json)
		{
            var code = GiftCode.FromJson(json);
			List<string> ret = new List<string>();
            if (code == null) return ret;
            var added = new List<int>();

            if(code.Fame != 0)
                ret.Add($"Fame: {code.Fame}");
            if(code.Gold != 0)
                ret.Add($"Gold: {code.Gold}");
            if (code.VaultChests != 0)
                ret.Add($"Vault Chest{(code.VaultChests > 1 ? "s" : String.Empty)}: {code.VaultChests}");
            if (code.CharSlots != 0)
                ret.Add($"Char Slot{(code.CharSlots > 1 ? "s" : String.Empty)}: {code.CharSlots}");

            foreach(var item in code.Gifts)
                if (!added.Contains(item))
                {
                    ret.Add($"{code.Gifts.Count(_ => _ == item)} {Program.GameData.Items[(ushort)item].ObjectId}");
                    added.Add(item);
                }
            return ret;
		}
	}
}