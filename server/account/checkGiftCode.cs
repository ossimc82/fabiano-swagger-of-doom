using db;
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
				string contents = String.Empty;
				string status = "Invalid code.";
				var cmd = db.CreateQuery();
				cmd.CommandText = "SELECT * FROM giftCodes WHERE code=@code";
				cmd.Parameters.AddWithValue("@code", Query["code"]);

				using (var rdr = cmd.ExecuteReader())
				{
					while (rdr.Read())
					{
						contents = rdr.GetString("content");
					}
				}

				var list = ParseContents(new StringReader(contents));
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
 @"<html>
	<head>
		<title>Check Giftcode</title>
	</head>
	<body style='background: #333333'>
		<h1 style='color: #EEEEEE; text-align: center'>
			" + status + @"
		</h1>
	</body>
</html>");
				}
				else
				{
					res = Encoding.UTF8.GetBytes(
 @"<html>
	<head>
		<title>Check Giftcode</title>
	</head>
	<body style='background: #333333'>
		<h1 style='color: #EEEEEE; text-align: center'>
			Your Giftcode contains the following Items:
		</h1>
		<h3 style='color: #EEEEEE; text-align: center'>
			" + status + @"
		</h3>
	</body>
</html>");
				}
				
				Context.Response.OutputStream.Write(res, 0, res.Length);
			}
		}

        //TODO: json
		private List<string> ParseContents(StringReader rdr)
		{
			List<string> ret = new List<string>();
			using (Database db = new Database())
			{
				List<string> tokens = new List<string>();

				while (true)
				{
					string s = rdr.ReadLine();
					if (s.IsNullOrWhiteSpace()) break;
					if (s.StartsWith("#")) continue;
					tokens.Add(s.Trim());
				}

				string[] headers = new string[tokens.Count];

				for (int i = 0; i < tokens.Count; i++)
				{
					if (tokens.Count > 0)
						headers[i] = tokens[i].Split(':')[0];
				}
				var cmd = db.CreateQuery();

				for (int i = 0; i < headers.Length; i++)
				{
					if (headers[i].StartsWith("items"))
					{
						Dictionary<string, int> itemDic = new Dictionary<string, int>();
						List<int> gifts = new List<int>();

						if (tokens[i].Split(':').Length == 3)
							for (int j = 0; j < tokens[i].Split(':')[1].Split(',').Length; j++)
								itemDic.Add(tokens[i].Split(':')[1].Split(',')[j],
									int.Parse(tokens[i].Split(':')[2].Split(',')[j]));
						else if (tokens[i].Split(':').Length == 2)
							gifts.AddRange(Utils.FromCommaSepString32(tokens[i].Split(':')[1]));
						else
							throw new Exception("Invalid giftCode data.");

						foreach (KeyValuePair<string, int> item in itemDic)
							for (int j = 0; j < item.Value; j++)
								gifts.Add(Utils.FromString(item.Key));

						foreach (var y in gifts)
							 ret.Add(Program.GameData.Items[(ushort)y].ObjectId);
					}

					if (headers[i].StartsWith("charSlot"))
					{
						ret.Add("Char Slot: +" + tokens[i].Split(':')[1]);
					}

					if (headers[i].StartsWith("vaultChest"))
						ret.Add("Vault Chest: +" + tokens[i].Split(':')[1]);

					if (headers[i].StartsWith("gold"))
						ret.Add("Gold: +" + tokens[i].Split(':')[1]);

					if (headers[i].StartsWith("fame"))
						ret.Add("Fame: +" + tokens[i].Split(':')[1]);
				}
			}
			return ret;
		}
	}
}