using MCGalaxy;

namespace DesertCube.Commands
{
    public class BusStopSave : Command2
    {
        public override string name => "busstopsave";

        public override string type => "BusAdmin";

        public override LevelPermission defaultRank => LevelPermission.Owner;
        public override void Help(Player p)
        {
            p.Message("/busstopsave name");
        }

        public override void Use(Player p, string message)
        {
            string name = message;
            if (name.Trim() == "")
                p.Message("Can't have an empty name!");

            Modules.Desert.Stop.SaveBusStop(p.level, name);

            p.Message($"Saved the bus stop to {name}.stop");
        }
    }
}
