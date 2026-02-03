using MCGalaxy;
using MCGalaxy.Commands;

namespace DesertCube.Commands
{
    public class BusObject : Command2
    {
        public override string name => "busobject";

        public override string type => "BusAdmin";

        public override LevelPermission defaultRank => LevelPermission.Owner;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("roadobject"), new CommandAlias("busobj"), new CommandAlias("robj") };
        public override void Help(Player p)
        {
            p.Message("/busobject name");
        }
        public override void Use(Player p, string message)
        {
            // Requires custom models
            if (MCGalaxy.Plugin.FindCustom("CustomModels") == null)
            {
                p.Message("This feature requires the https://github.com/NotAwesome2/MCGalaxy-CustomModels plugin!");
                return;
            }

            if (!Modules.Desert.RoadObjects.Models.ContainsKey(message))
            {
                p.Message($"No such model {message}");
                return;
            }
            Modules.Desert.RoadObjects.CreateObject(message);
            p.Message($"Spawned model {message}");
        }
    }
}
