using MCGalaxy;
using MCGalaxy.Tasks;
using System;

namespace DesertCube.Modules.Player
{
    internal class Sit : DesertModule
    {
        public override void Load()
        {
        }
        public override void Unload()
        {
        }

        static bool IsSeated(MCGalaxy.Player player)
        {
            var feetCords = player.Pos.FeetBlockCoords;

            if (!player.Level.IsValidPos(feetCords.X, feetCords.Y, feetCords.Z)) return false;
            if (player.Level.GetBlock((ushort)feetCords.X, (ushort)feetCords.Y, (ushort)feetCords.Z) != 50) return false;

            return true;
        }

        DateTime nextSit = DateTime.Now;
        public override void Tick(float curTime)
        {
            if (DateTime.Now < nextSit) return;
            nextSit = DateTime.Now.AddSeconds(0.25f);

            foreach (var player in PlayerInfo.Online.Items)
            {
                bool seated = IsSeated(player);

                bool cute = Shop.Shop.GetData(player.name)[0] == 1;
                string model = seated ? (cute ? "sitcute" :"sit") : "humanoid";

                if (player.Model == model) continue;
                if (model == "humanoid" && player.Model.StartsWith("hold")) continue;


                player.UpdateModel(model);

                if (seated)
                    player.Extras["HeldBlock"] = 0;
            }
        }
    }
}
