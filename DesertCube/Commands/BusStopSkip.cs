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
            if (!Modules.Desert.Stop.AtStop)
            {
                p.Message("Can't skip a non existant stop!");
                return;
            }
            DesertCubePlugin.Bus.stopUntil = System.DateTime.Now;
            Modules.Desert.Stop.AtStop = false;
            p.Message($"Skipped the stop!");
        }
    }
}
