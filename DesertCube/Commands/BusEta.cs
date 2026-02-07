using DesertCube.Modules.Server;
using MCGalaxy;
using MCGalaxy.Commands;
using System;

namespace DesertCube.Commands
{
    public class BusEta : Command2
    {
        public override string name => "eta";

        public override string type => "Bus";

        public override LevelPermission defaultRank => LevelPermission.Guest;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("estimatedtime") };
        public override void Help(Player p)
        {
            p.Message("/eta");
        }

        public override void Use(Player p, string message)
        {
            if (DesertCubePlugin.Bus == null || DesertCubePlugin.Config == null) return;
            p.Message($"%eThe bus will arrive at &6{Journey.DestinationName} &e in approx &d{Math.Round((DesertCubePlugin.Config.DestinationDistance/ DesertCubePlugin.Bus.BusSpeed)/60/60, 2).ToString("0")}%e hours at &d{Math.Round(DesertCubePlugin.Bus.BusSpeed,2)}m/s&e.");
        }
    }
}
