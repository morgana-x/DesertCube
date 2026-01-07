using MCGalaxy;
using MCGalaxy.Commands;

namespace DesertCube.Commands
{
    public class BusSpeed : Command2
    {
        public override string name => "busspeed";

        public override string type => "Bus";

        public override LevelPermission defaultRank => LevelPermission.Guest;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("speed") };
        public override void Help(Player p)
        {
            p.Message("/busspeed");
        }

        public override void Use(Player p, string message)
        {

            string[] arguments = message.Split(' ');
            if (p.Rank >= LevelPermission.Admin && arguments.Length > 0 && arguments[0] != "")
            {

                if (!float.TryParse(arguments[0], out var speed))
                {
                    p.Message("Invalid float");
                    return;
                }
                DesertCubePlugin.Bus.SetSpeed(speed);
            }

            p.Message($"%eThe bus is travelling at %d{(DesertCubePlugin.Bus.BusSpeed * 3.6f).ToString("0")}%ekm/h.");
        }
    }
}
