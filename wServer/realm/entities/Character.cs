namespace wServer.realm.entities
{
    public abstract class Character : Entity
    {
        public Character(RealmManager manager, ushort objType, wRandom rand)
            : base(manager, objType)
        {
            Random = rand;

            if (ObjectDesc == null) return;
            Name = ObjectDesc.DisplayId ?? "";
            if (ObjectDesc.SizeStep != 0)
            {
                int step = Random.Next(0, (ObjectDesc.MaxSize - ObjectDesc.MinSize)/ObjectDesc.SizeStep + 1)*
                           ObjectDesc.SizeStep;
                Size = ObjectDesc.MinSize + step;
            }
            else
                Size = ObjectDesc.MinSize;

            HP = (int) ObjectDesc.MaxHP;
        }

        public wRandom Random { get; private set; }

        public int HP { get; set; }
    }
}