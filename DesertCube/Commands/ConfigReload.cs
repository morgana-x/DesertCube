using MCGalaxy;
using MCGalaxy.Commands;

namespace DesertCube.Commands
{
    public class ConfigReload : Command2
    {
        public override string name => "configreload";

        public override string type => "fun";

        public override LevelPermission defaultRank => LevelPermission.Owner;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("reloadconfig") };
        public override void Help(Player p)
        {
            p.Message("/configreload");
        }

        public override void Use(Player p, string message)
        {
            DesertConfig.Load();
            p.Message("Reloaded config!");
        }
    }
}
