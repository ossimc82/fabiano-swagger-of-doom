#region

using System;
using wServer.realm.worlds;

#endregion

namespace wServer.realm.entities.player
{
    public partial class Player
    {
        private readonly Random invRand = new Random();

        private int[] setTypeBoosts;

        private void CheckSetTypeSkin()
        {
            if (Inventory[0]?.SetType == Inventory[1]?.SetType &&
               Inventory[1]?.SetType == Inventory[2]?.SetType &&
               Inventory[2]?.SetType == Inventory[3]?.SetType &&
               Inventory[3]?.SetType == Inventory[0]?.SetType)
            {
                SetTypeSkin setType;
                if (!Manager.GameData.SetTypeSkins.TryGetValue((ushort)Inventory[0].SetType, out setType)) return;

                setTypeSkin = setType;
                if (setTypeBoosts == null && setTypeSkin != null)
                {
                    setTypeBoosts = new int[8];

                    foreach (var i in setTypeSkin.StatsBoost)
                    {
                        int idx = -1;

                        if (i.Key == StatsType.MaximumHP) idx = 0;
                        else if (i.Key == StatsType.MaximumMP) idx = 1;
                        else if (i.Key == StatsType.Attack) idx = 2;
                        else if (i.Key == StatsType.Defense) idx = 3;
                        else if (i.Key == StatsType.Speed) idx = 4;
                        else if (i.Key == StatsType.Vitality) idx = 5;
                        else if (i.Key == StatsType.Wisdom) idx = 6;
                        else if (i.Key == StatsType.Dexterity) idx = 7;
                        if (idx == -1) continue;
                        setTypeBoosts[idx] = i.Value;
                    }
                }
                return;
            }
            if (setTypeSkin == null) return;
            setTypeSkin = null;
            setTypeBoosts = null;
        }

        public bool HasSlot(int slot) => Inventory[slot] != null;

        public void DropBag(Item i)
        {
            ushort bagId = 0x0500;
            bool soulbound = false;
            if (i.Soulbound)
            {
                bagId = 0x0503;
                soulbound = true;
            }

            Container container = new Container(Manager, bagId, 1000*60, true);
            if (soulbound)
                container.BagOwners = new string[1] { AccountId };
            container.Inventory[0] = i;
            container.Move(X + (float) ((invRand.NextDouble()*2 - 1)*0.5),
                Y + (float) ((invRand.NextDouble()*2 - 1)*0.5));
            container.Size = 75;
            Owner.EnterWorld(container);

            if (Owner is Vault)
                if ((Owner as Vault).PlayerOwnerName == Client.Account.Name)
                    return;
        }
    }
}