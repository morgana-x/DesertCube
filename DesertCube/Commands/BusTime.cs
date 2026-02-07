using DesertCube.Modules.Server;
using MCGalaxy;
using MCGalaxy.Commands;

namespace DesertCube.Commands
{
    public class BusTime : Command2
    {
        public override string name => "clock";

        public override string type => "Bus";

        public override LevelPermission defaultRank => LevelPermission.Guest;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("clock"), new CommandAlias("gettime") };
        public override void Help(Player p)
        {
            p.Message("/clock");
        }

        public override void Use(Player p, string message)
        {
            p.Message($"%eThe Bus's clock reads: %d{Time.FormattedTime}");
        }
    }
}
