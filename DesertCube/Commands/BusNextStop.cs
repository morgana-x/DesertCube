using DesertCube.Modules.Server;
using MCGalaxy;
using MCGalaxy.Commands;
using System;

namespace DesertCube.Commands
{
    public class BusNextStop : Command2
    {
        public override string name => "busnextstop";

        public override string type => "Bus";

        public override LevelPermission defaultRank => LevelPermission.Guest;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("nextstop"), new CommandAlias("stop") };
        public override void Help(Player p)
        {
            p.Message("/busnextstop");
        }

        public override void Use(Player p, string message)
        {
            string[] args = message.Split(' ');

            if (p.Rank >= LevelPermission.Owner && message.Length > 0 && args.Length > 0)
            {
                if (!float.TryParse(args[0], out var newdist))
                {
                    p.Message("Invalid distance");
                    return;
                }
                Modules.Desert.Stop.nextStopMeters = (int)(Journey.TotalDistance + (newdist * 1000));
                p.Message("Set distance to " + args[0] + "km");
                if (args.Length > 1)
                {
                    Modules.Desert.Stop.nextStop = args[1];
                    p.Message("Set next stop to " + args[1]);
                }
                return;
            }

            p.Message($"%eThe next stop is %d{Math.Ceiling(((Modules.Desert.Stop.nextStopMeters - Journey.TotalDistance) / 1000)).ToString("0")}%ekm away!");
        }
    }
}
