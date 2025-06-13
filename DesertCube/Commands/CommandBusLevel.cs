using MCGalaxy;

namespace DesertCube.Commands
{
    public class BusLevel : Command2
    {
        public override string name => "buslevel";

        public override string type => "map";

        public override LevelPermission defaultRank => LevelPermission.Owner;
        public override void Help(Player p)
        {
            p.Message("/buslevel level");
        }

        public override void Use(Player p, string message)
        {
            string[] arguments = message.Split(' ');
            if (arguments.Length < 1)
            {
                p.Message("Missing argument");
                return;
            }
            string level = arguments[0];

            if (!LevelInfo.MapExists(level))
            {
                p.Message($"Level {level} doesn't exist");
                return;
            }
            DesertCubePlugin.Config.BusLevel = level;
            DesertCubePlugin.Bus.SetLevel(level);
            DesertConfig.Save(DesertCubePlugin.Config);
        }
    }
}
