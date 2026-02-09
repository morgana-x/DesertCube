using DesertCube.Modules.Player;

namespace DesertCube.Modules.Shop.Items
{
    public class SitCute : ShopItem
    {
        public override string Name => "SitCute";

        public SitCute() { Price = 3; }
        public override void Purchase(MCGalaxy.Player p, string message = "")
        {
            var data = Shop.GetData(p.name);
            if (data[0]==1)
            {
                Shop.ModifyData(p.name, 2, 0);
                p.Message("Disabled sit cute, you still own this, purchase it again to toggle it for free!");
                return;
            }
            if (data[0] == 2)
            {
                Shop.ModifyData(p.name, 1, 0);
                p.Message("Enabled sit cute, purchase it again to toggle it for free!");
                return;
            }
            if (!CanPurchase(p))
                return;

            Shop.ModifyData(p.name, 1, 0);
            Stats.AddPoints(p.name, -Price);
            p.Message("Purchased sit cute! Purchase this again to toggle it for free whenever!");
        }
    }
}
