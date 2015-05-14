//#region

//using System;
//using System.Collections.Generic;
//using wServer.logic.behaviors;
//using db;

//#endregion

//namespace wServer.logic
//{
//    partial class BehaviorDb
//    {
//        private _ Nexus = () => Behav()
//            .Init("Nexus Crier",
//                new State("Active",
//                    new BackAndForth(.2, 3),
//                    new Taunt(new string[1]
//                    {
//                        "Active"
//                    },
//                    text: "FREE Giftcode!  :  " + GenerateCode(), cooldown: 60000)
//                    )
//            );

//        public static string GenerateCode()
//        {
//            string contents = String.Empty;

//            for (int i = 0; i < 5; i++)
//            {
//                int y = Random.Next(0, stuff.Count);
//                contents += stuff[y].Key + stuff[y].Value + Environment.NewLine;
//            }

//            using (var db = new Database())
//            {
//                return db.GenerateGiftcode(contents);
//            }
//        }
//        private readonly static List<KeyValuePair<string, string>> stuff = new List<KeyValuePair<string, string>>
//        {
//            new KeyValuePair<string, string>("items:", "0xccc:5"),
//            new KeyValuePair<string, string>("items:", "0xccc:2"),
//            new KeyValuePair<string, string>("items:", "0xc08:1"),
//            new KeyValuePair<string, string>("items:", "0xb41:2"),
//            new KeyValuePair<string, string>("items:", "0x227a:1"),
//            new KeyValuePair<string, string>("items:", "0x226c:1"),
//            new KeyValuePair<string, string>("items:", "0xa1f,0xa34,0xa35,0xa4c,0xae9,0xaea,0xa21:10,10,10,10,10,10,10"),
//            new KeyValuePair<string, string>("items:", "0x710:10"),
//            new KeyValuePair<string, string>("items:", "0xc98:1"),
//            new KeyValuePair<string, string>("items:", "0xc98:2"),
//            new KeyValuePair<string, string>("gold:", "1000"),
//            new KeyValuePair<string, string>("gold:", "2000"),
//            new KeyValuePair<string, string>("fame:", "1000"),
//            new KeyValuePair<string, string>("fame:", "2000"),
//            new KeyValuePair<string, string>("charSlot:", "1"),
//            new KeyValuePair<string, string>("charSlot:", "2"),
//            new KeyValuePair<string, string>("vaultChest:", "1"),
//            new KeyValuePair<string, string>("vaultChest:", "2"),
//        };
//    }
//}