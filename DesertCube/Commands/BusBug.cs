using MCGalaxy;
using MCGalaxy.Commands;

namespace DesertCube.Commands
{
    public class BusBug : Command2
    {
        public override string name => "busbug";

        public override string type => "BusAdmin";

        public override LevelPermission defaultRank => LevelPermission.Owner;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("bug")};
        public override void Help(Player p)
        {
            p.Message("/bug");
        }
        public override void Use(Player p, string message)
        {
            // Requires custom models
            if (MCGalaxy.Plugin.FindCustom("CustomModels") == null)
            {
                p.Message("This feature requires the https://github.com/NotAwesome2/MCGalaxy-CustomModels plugin!");
                return;
            }

            Modules.Desert.Bug.SpawnBug();
        }
    }
}
