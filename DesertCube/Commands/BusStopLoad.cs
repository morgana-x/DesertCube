using MCGalaxy;

namespace DesertCube.Commands
{
    public class BusStopLoad : Command2
    {
        public override string name => "busstopload";

        public override string type => "BusAdmin";

        public override LevelPermission defaultRank => LevelPermission.Owner;
        public override void Help(Player p)
        {
            p.Message("/busstopload name");
        }

        public override void Use(Player p, string message)
        {
            string name = message;
            if (name.Trim() == "")
                p.Message("Can't have an empty name!");

            if (!Modules.Desert.Stop.BusStopExists(name))
            {
                p.Message($"Bus stop \"{name}\" doesn't exist!");
                return;
            }

            Modules.Desert.Stop.LoadBusStop(p.level, name);

            p.Message($"Loaded the bus stop {name}.stop");
        }
    }
}
