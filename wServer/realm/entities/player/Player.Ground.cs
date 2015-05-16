#region

using System;
using System.Linq;

#endregion

namespace wServer.realm.entities.player
{
    public partial class Player
    {
        private long b;

        public void HandleGround(RealmTime time)
        {
            if (time.tickTimes - b <= 100) return;
            try
            {
                if (Owner.Name == "Ocean Trench")
                {
                    if (!(Owner.StaticObjects.Where(i => i.Value.ObjectType == 0x0731).Count(i => (X - i.Value.X) * (X - i.Value.X) + (Y - i.Value.Y) * (Y - i.Value.Y) < 1) > 0))
                    {
                        if (OxygenBar == 0)
                            HP -= 2;
                        else
                            OxygenBar -= 1;

                        UpdateCount++;

                        if (HP <= 0)
                            Death("server.damage_suffocation");
                    }
                    else
                    {
                        if (OxygenBar < 100)
                            OxygenBar += 8;
                        if (OxygenBar > 100)
                            OxygenBar = 100;

                        UpdateCount++;
                    }
                }

                b = time.tickTimes;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}