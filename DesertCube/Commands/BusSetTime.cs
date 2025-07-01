using DesertCube.Modules.Server;
using MCGalaxy;
using MCGalaxy.Commands;
using System;

namespace DesertCube.Commands
{
    public class BusSetTime : Command2
    {
        public override string name => "bussettime";

        public override string type => "fun";

        public override LevelPermission defaultRank => LevelPermission.Owner;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("settime") };
        public override void Help(Player p)
        {
            p.Message("/bussettime time");
        }

        public override void Use(Player p, string message)
        {
            string[] split = message.Split(':');
            if (split.Length < 2)
            {
                p.Message("Incorrectly formatted time!");
                return;

            }

            if (!int.TryParse(split[0], out var timehours))
            {
                p.Message("Invalid int for minutes of time");
                return;
            }
            if (!int.TryParse(split[1], out var timeminutes))
            {
                p.Message("Invalid int for minutes of time");
                return;
            }
            if (timeminutes >= 60)
            {
                p.Message("Invalid minutes! over 60!");
                return;
            }
            Time.CurrentTime = Time.RealTimeToBusSeconds(timehours, timeminutes);
            p.Message($"Set the time to {Time.FormattedTime}");
        }
    }
}
