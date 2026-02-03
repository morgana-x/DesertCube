using System;

namespace DesertCube.Modules.Player
{
    internal class LeaveBehind : DesertModule
    {
        public override void Load()
        {
        }
        public override void Unload()
        {
        }

        DateTime nextCheck = DateTime.Now;
        public override void Tick(float curTime)
        {
            if (DesertCubePlugin.Bus.Level == null) return;
            if (DesertCubePlugin.Bus.BusSpeed == 0) return;
            if (DateTime.Now < nextCheck) return;
            nextCheck = DateTime.Now.AddSeconds(0.25f);
            foreach (var player in DesertCubePlugin.Bus.GetPlayers())
            {
                if (player.Level.name != DesertCubePlugin.Config.BusLevel) continue;
                if (player.Game.Referee) continue;
                if (DesertCubePlugin.Bus.InsideBus(player)) continue;
                player.SendPosition(DesertCubePlugin.Bus.Level.SpawnPos, player.Rot);
            }
        }
    }
}
