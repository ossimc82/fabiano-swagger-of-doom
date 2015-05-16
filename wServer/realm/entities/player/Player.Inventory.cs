#region

using System;

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
                SetTypeSkin setType = null;
                var item = Inventory[0];
                if (item != null && !Manager.GameData.SetTypeSkins.TryGetValue((ushort)item.SetType, out setType)) return;

                setTypeSkin = setType;
                if (setTypeBoosts != null || setTypeSkin == null) return;
                setTypeBoosts = new int[8];

                foreach (var i in setTypeSkin.StatsBoost)
                {
                    var idx = -1;

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
            var soulbound = false;
            if (i.Soulbound)
            {
                bagId = 0x0503;
                soulbound = true;
            }

            var container = new Container(Manager, bagId, 1000*60, true);
            if (soulbound)
                container.BagOwners = new [] { AccountId };
            container.Inventory[0] = i;
            container.Move(X + (float) ((invRand.NextDouble()*2 - 1)*0.5),
                Y + (float) ((invRand.NextDouble()*2 - 1)*0.5));
            container.Size = 75;
            Owner.EnterWorld(container);
        }
    }
}