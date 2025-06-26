using MCGalaxy;

namespace DesertCube.Commands
{
    public class BusStopSkip : Command2
    {
        public override string name => "busstopskip";

        public override string type => "fun";

        public override LevelPermission defaultRank => LevelPermission.Owner;
        public override void Help(Player p)
        {
            p.Message("/busstopskip");
        }

        public override void Use(Player p, string message)
        {
            if (DesertCubePlugin.Bus.Level == null) return;
            if (!Modules.Desert.Stop.AtStop)
            {
                p.Message("Can't skip a non existant stop!");
                return;
            }
         //   Modules.Desert.Stop.ClearBusStop(DesertCubePlugin.Bus.Level);
            DesertCubePlugin.Bus.stopUntil = System.DateTime.Now;
           // Modules.Desert.Stop.AtStop = false;
            p.Message($"Skipped the stop!");
            DesertCubePlugin.Bus.Broadcast("%cThe stop has been skipped! The road's waiting!");
        }
    }
}
