using MCGalaxy.Tasks;
using System;

namespace DesertCube.Modules.Player
{
    internal class LeaveBehind : DesertModule
    {
        public override void Load()
        {
            leaveBehindTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TickPlayerLeavebehind, null, TimeSpan.FromSeconds(0.25f));
        }
        public override void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(leaveBehindTask);
        }

        static SchedulerTask leaveBehindTask;
        private static void TickPlayerLeavebehind(SchedulerTask task)
        {
            leaveBehindTask = task;
            if (DesertCubePlugin.Bus.Level == null) return;
            if (DesertCubePlugin.Bus.BusSpeed == 0) return;
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
