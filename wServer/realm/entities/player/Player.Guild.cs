namespace wServer.realm.entities.player
{
    public partial class Player
    {
        public string ResolveGuildChatName()
        {
            return Name;
        }
        public string ResolveRankName(int rank)
        {
            string name;
            switch (rank)
            {
                case 0:
                    name = "Initiate"; break;
                case 10:
                    name = "Member"; break;
                case 20:
                    name = "Officer"; break;
                case 30:
                    name = "Leader"; break;
                case 40:
                    name = "Founder"; break;
                default:
                    name = ""; break;
            }
            return name;
        }
    }
}
