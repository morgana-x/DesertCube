using MCGalaxy;
using MCGalaxy.Tasks;
using System;

namespace DesertCube.Modules.Player
{
    internal class Hold
    {
        public static void Load()
        {
            sitTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TickPlayerSit, null, TimeSpan.FromSeconds(0.5f));
        }
        public static void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(sitTask);
        }

        static SchedulerTask sitTask;

        private static void TickPlayerSit(SchedulerTask task)
        {
            sitTask = task;

            foreach (var player in PlayerInfo.Online.Items)
            {
                int holding = player.Model == "sit" ? 0 : player.GetHeldBlock();
                if (player.Extras.GetInt("HeldBlock") == holding)  continue;
                player.Extras["HeldBlock"] = holding;

                bool has = true;// (!Inventory.GetInventory(player.name).ContainsKey((ushort)holding));

                if (holding >= 66) holding = (holding - 256);
                string model = (holding == 0 || !has)? "humanoid" : $"hold|1.{holding.ToString("D3")}";

                if (player.Model == model) continue;

                player.UpdateModel(model);
            }
        }
    }
}
