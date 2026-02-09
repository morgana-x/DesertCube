using MCGalaxy;
using MCGalaxy.Tasks;
using System;

namespace DesertCube.Modules.Player
{
    internal class Hold : DesertModule
    {
        public override void Load() { }
        public override void Unload() { }


        static string GetModel(int holding)
        {
            if (holding == 0)
                return "humanoid";
            return $"hold|1.{holding.ToString("D3")}";
        }

        DateTime nextHold = DateTime.Now;
        public override void Tick(float deltaTime)
        {
            if (DateTime.Now < nextHold) return;
            nextHold = DateTime.Now.AddSeconds(0.45f);

            foreach (var player in PlayerInfo.Online.Items)
            {
                if (!player.Level.Config.MOTD.Contains("+hold")) continue;
                if (player.Model == "sit" || player.Model.Contains("sit"))
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
