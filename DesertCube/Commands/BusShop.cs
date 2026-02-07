using DesertCube.Modules.Player;
using DesertCube.Modules.Shop;
using MCGalaxy;
using MCGalaxy.Commands;

namespace DesertCube.Commands
{
    public class BusShop : Command2
    {
        public override string name => "shop";

        public override string type => "Bus";

        public override LevelPermission defaultRank => LevelPermission.Guest;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("busstore"), new CommandAlias("store"), new CommandAlias("shop") };
        public override void Help(Player p)
        {
            p.Message("/shop");
        }

        public override void Use(Player p, string message)
        {
            p.Message($"&eShop Items: (&a{Stats.GetPoints(p.name)} points&e)");
            foreach( var i in Shop.Items)
                p.Message($" - &d{i.Name} &a{i.Price} points");
            p.Message("Do &3/buy name message");
        }
    }
}
