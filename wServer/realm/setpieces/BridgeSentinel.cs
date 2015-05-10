namespace wServer.realm.setpieces
{
    internal class BridgeSentinel : ISetPiece
    {
        public int Size
        {
            get { return 5; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            Entity boss = Entity.Resolve(world.Manager, "shtrs Bridge Sentinel");
            boss.Move(pos.X, pos.Y);

            Entity chestSpawner = Entity.Resolve(world.Manager, "shtrs encounterchestspawner");
            chestSpawner.Move(pos.X, pos.Y + 5f);


            Entity blobombSpawner1 = Entity.Resolve(world.Manager, "shtrs blobomb maker");
            blobombSpawner1.Move(pos.X, pos.Y + 5f);

            Entity blobombSpawner2 = Entity.Resolve(world.Manager, "shtrs blobomb maker");
            blobombSpawner2.Move(pos.X + 5f, pos.Y + 5f);

            Entity blobombSpawner3 = Entity.Resolve(world.Manager, "shtrs blobomb maker");
            blobombSpawner3.Move(pos.X - 5f, pos.Y + 5f);

            world.EnterWorld(boss);

            world.EnterWorld(chestSpawner);

            world.EnterWorld(blobombSpawner1);
            world.EnterWorld(blobombSpawner2);
            world.EnterWorld(blobombSpawner3);
        }
    }
}