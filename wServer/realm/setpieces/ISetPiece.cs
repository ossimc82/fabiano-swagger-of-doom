namespace wServer.realm.setpieces
{
    internal interface ISetPiece
    {
        int Size { get; }
        void RenderSetPiece(World world, IntPoint pos);
    }
}