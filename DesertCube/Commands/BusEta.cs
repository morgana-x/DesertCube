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
            if (DesertCubePlugin.Bus.BusSpeed == 0)
            {
                p.Message("&WUnable to give eta when bus is stationary!");
                return;
            }
            var seconds = Journey.RemainingDistance / DesertCubePlugin.Bus.BusSpeed;
            string format = (seconds > 3600) ? ((float)seconds / 60 / 60).ToString("0.#") + " hours" : (((int)(seconds / 60)).ToString()) + " minutes";
            p.Message($"%eThe bus will arrive at &6{Journey.DestinationName}&e in approx &d{format}%e at &d{Math.Round(DesertCubePlugin.Bus.BusSpeed,2)}m/s&e.");
        }
    }
}
