using MCGalaxy;
using MCGalaxy.Commands;

namespace DesertCube.Commands
{
    public class BusTime : Command2
    {
        public override string name => "bustime";

        public override string type => "fun";

        public override LevelPermission defaultRank => LevelPermission.Guest;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("clock"), new CommandAlias("gettime") };
        public override void Help(Player p)
        {
            p.Message("/bustime");
        }

        public override void Use(Player p, string message)
        {
            p.Message($"%eThe Bus's clock reads: %d{Modules.Desert.Time.FormattedTime}");
        }
    }
}
