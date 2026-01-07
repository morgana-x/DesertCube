using MCGalaxy;

namespace DesertCube.Commands
{
    public class Points : Command2
    {
        public override string name => "points";

        public override string type => "Bus";

        public override LevelPermission defaultRank => LevelPermission.Guest;
        public override void Help(Player p)
        {
            p.Message("/points");
        }

        public override void Use(Player p, string message)
        {
            p.Message($"%eYou have %c{DesertCube.Modules.Player.Stats.GetPoints(p.name)} %epoints!");
        }
    }
}
