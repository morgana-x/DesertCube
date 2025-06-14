using MCGalaxy;
using MCGalaxy.Tasks;
using System;

namespace DesertCube.Modules.Player
{
    internal class Sit
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

        static bool IsSeated(MCGalaxy.Player player)
        {
            var feetCords = player.Pos.FeetBlockCoords;

            if (!player.Level.IsValidPos(feetCords.X, feetCords.Y, feetCords.Z)) return false;
            if (player.Level.GetBlock((ushort)feetCords.X, (ushort)feetCords.Y, (ushort)feetCords.Z) != 50) return false;

            return true;
        }
        private static void TickPlayerSit(SchedulerTask task)
        {
            sitTask = task;

            foreach (var player in PlayerInfo.Online.Items)
            {
                string model = IsSeated(player) ? "sit" : "humanoid";

                if (player.Model == model) continue;
                if (model == "humanoid" && player.Model.StartsWith("hold")) continue;
                player.UpdateModel(model);
            }
        }
    }
}
