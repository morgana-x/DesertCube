using MCGalaxy;
using MCGalaxy.Tasks;
using System;

namespace DesertCube.Modules.Player
{
    internal class LeaveBehind
    {
        public static void Load()
        {
            sitTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TickPlayerSit, null, TimeSpan.FromSeconds(0.25f));
        }
        public static void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(sitTask);
        }

        static SchedulerTask sitTask;
        private static void TickPlayerSit(SchedulerTask task)
        {
            sitTask = task;
            if (DesertCubePlugin.Bus.Level == null) return;
            if (DesertCubePlugin.Bus.BusSpeed == 0) return;
            foreach (var player in DesertCubePlugin.Bus.GetPlayers())
            {
                if (DesertCubePlugin.Bus.InsideBus(player)) continue;
                player.SendPosition(DesertCubePlugin.Bus.Level.SpawnPos, player.Rot);
            }
        }
    }
}
