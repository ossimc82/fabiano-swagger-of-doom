#region

using System.Collections.Generic;
using System.Linq;
using db;
using MySql.Data.MySqlClient;
using wServer.networking;
using wServer.realm.entities;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.worlds
{
    public class PetYard : World
    {
        private readonly Player player;

        public PetYard(Player player)
        {
            this.player = player;
            Name = "Pet Yard";
            ClientWorldName = "{nexus.Pet_Yard_" + player.Client.Account.PetYardType + "}";
            Background = 0;
            Difficulty = -1;
            ShowDisplays = true;
            AllowTeleport = false;
        }

        protected override void Init()
        {
            string petYard = "wServer.realm.worlds.maps.PetYard_Common.wmap";
            switch (player.Client.Account.PetYardType)
            {
                case 1: petYard = "wServer.realm.worlds.maps.PetYard_Common.wmap"; break;
                case 2: petYard = "wServer.realm.worlds.maps.PetYard_Uncommon.wmap"; break;
                case 3: petYard = "wServer.realm.worlds.maps.PetYard_Rare.wmap"; break;
                case 4: petYard = "wServer.realm.worlds.maps.PetYard_Legendary.wmap"; break;
                case 5: petYard = "wServer.realm.worlds.maps.PetYard_Divine.wmap"; break;
            }

            LoadMap(petYard, MapType.Wmap);
            LoadPetYardData(player);
        }

        private void LoadPetYardData(Player player)
        {
            Manager.Database.DoActionAsync(db =>
            {
                MySqlCommand cmd = db.CreateQuery();
                cmd.CommandText = "SELECT petId, objType FROM pets WHERE accId=@accId AND NOT petId=@petId;";
                cmd.Parameters.AddWithValue("@accId", player.AccountId);
                cmd.Parameters.AddWithValue("@petId", player.Pet is Pet ? player.Pet.PetId : -1);

                List<PetItem> petData = new List<PetItem>();

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int petId = rdr.GetInt32("petId");
                        if (player.Pet != null && player.Pet.PetId == petId) continue;
                        petData.Add(db.GetPet(petId, player.Client.Account));
                    }
                }

                foreach (PetItem i in petData)
                {
                    Pet obj = new Pet(player.Manager, i, null);
                    int x, y;
                    do
                    {
                        x = player.Random.Next(0, this.Map.Width);
                        y = player.Random.Next(0, this.Map.Height);
                    } while (this.Map[x, y].Region != TileRegion.PetRegion || this.Map[x, y].ObjType != 0);
                    obj.Move(x + 0.5f, y + 0.5f);
                    EnterWorld(obj);
                }
            });
        }

        public Pet FindPetById(int petId)
        {
            Pet ret = null;
            for (int i = 0; i < this.Pets.Values.Count; i++)
            {
                ret = this.Pets.Values.ToArray()[i];
                if (ret != null)
                {
                    if (ret.PlayerOwner != null)
                        ret.PlayerOwner = null;

                    if (ret.PetId == petId)
                        return ret;
                }
            }
            return ret;
        }

        public override World GetInstance(Client psr)
        {
            return Manager.AddWorld(new PetYard(psr.Player));
        }
    }
}