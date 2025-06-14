using MCGalaxy;
using MCGalaxy.Commands;
using System;

namespace DesertCube.Commands
{
    public class BusNextStop : Command2
    {
        public override string name => "busnextstop";

        public override string type => "map";

        public override LevelPermission defaultRank => LevelPermission.Guest;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("nextstop"), new CommandAlias("stop") };
        public override void Help(Player p)
        {
            p.Message("/busnextstop");
        }

        public override void Use(Player p, string message)
        {
            p.Message($"%eThe next stop is %d{Math.Ceiling(((Modules.Desert.Stop.nextStopMeters - DesertCubePlugin.TotalDistance) / 1000)).ToString("0")}%ekm away!");
        }
    }
}
