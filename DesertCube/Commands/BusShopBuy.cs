using DesertCube.Modules.Shop;
using MCGalaxy;
using MCGalaxy.Commands;
using System;
using System.Linq;

namespace DesertCube.Commands
{
    public class BusShopBuy : Command2
    {
        public override string name => "purchase";

        public override string type => "Bus";

        public override LevelPermission defaultRank => LevelPermission.Guest;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("busbuy"), new CommandAlias("buy"), new CommandAlias("buspurchase") };
        public override void Help(Player p)
        {
            p.Message("/buy &Ditem &Emessage");
        }

        public override void Use(Player p, string message)
        {
            string item;
            string msg = "";
            if (message.Contains(" "))
            {
                int split = message.IndexOf(' ');
                item = message.Substring(0, split);
                msg = message.Substring(split+1);
            }
            else
                item = message;

            var shopitems = Shop.Items.Where((x)=> x.Name.CaselessEq(item));
            if (shopitems.Count() == 0)
            {
                p.Message($"&cNo such shop item &e{item.ToLower()}&c.");
                return;
            }
            try
            {
                shopitems.First().Purchase(p, msg);
            }
            catch(Exception e)
            {
                p.Message($"Error occured {e.ToString()}");
            }
        }
    }
}
