using DesertCube.Modules.Server;
using MCGalaxy;
using MCGalaxy.Tasks;
using System;

namespace DesertCube.Modules.Player
{
    internal class StatusHud
    {
        public static void Load()
        {
            sitTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TickPlayerSit, null, TimeSpan.FromSeconds(1));
            BroadcastStatus();
        }
        public static void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(sitTask);
        }

        static SchedulerTask sitTask;

        static string GetStatus1Message()
        {
            return $"%3{(DesertCubePlugin.Bus.BusSpeed * 3.6f).ToString("0")}%7km/h %3{Journey.RemainingDistanceKilometers}%7km";
        }

        static bool ShouldSeeMessage(MCGalaxy.Player player)
        {
            if (player.Pos.BlockX != 71) return false;
            if (!DesertCubePlugin.Bus.InsideBus(player)) return false;
            return true;
        }
        static void EraseStatus(MCGalaxy.Player player)
        {
            if (player.Extras.GetString("Status1") == "") return;
            player.SendCpeMessage(CpeMessageType.Status1, "");
            player.Extras["Status1"] = "";
        }
        static void BroadcastStatus()
        {
            string message = GetStatus1Message();
            foreach (var player in DesertCubePlugin.Bus.GetPlayers())
            {
                if (!player.Session.hasCpe) continue;

                if (!ShouldSeeMessage(player))
                {
                    EraseStatus(player);
                    continue; 
                }

                if (player.Extras.GetString("Status1") == message) continue;
                player.SendCpeMessage(CpeMessageType.Status1, message);
                player.Extras["Status1"] = message;
            }
        }
        private static void TickPlayerSit(SchedulerTask task)
        {
            sitTask = task;
            BroadcastStatus();
        }
    }
}
