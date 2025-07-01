using MCGalaxy;
using MCGalaxy.Tasks;
using System;

namespace DesertCube.Modules.Player
{
    internal class Hold
    {
        public static void Load()
        {
            holdTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TickPlayerHold, null, TimeSpan.FromSeconds(0.45f));
        }
        public static void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(holdTask);
        }

        static SchedulerTask holdTask;

        static string GetModel(int holding)
        {
            if (holding == 0)
                return "humanoid";
            return $"hold|1.{holding.ToString("D3")}";
        }
        private static void TickPlayerHold(SchedulerTask task)
        {
            holdTask = task;

            foreach (var player in PlayerInfo.Online.Items)
            {
                if (player.Model == "sit")
                    continue;

                int holding = player.GetHeldBlock();

                if (player.Extras.GetInt("HeldBlock") == holding)
                    continue;
                player.Extras["HeldBlock"] = holding;

                if (holding >= 66) holding = (holding - 256);
                if (!player.Game.Referee &&
                    player.Level == DesertCubePlugin.Bus.Level &&
                    !Inventory.GetInventory(player.name).ContainsKey((ushort)holding))
                {
                    holding = 0;
                }

                string model = GetModel(holding);
                if (player.Model == model) continue;
                player.UpdateModel(model);
            }
        }
    }
}
