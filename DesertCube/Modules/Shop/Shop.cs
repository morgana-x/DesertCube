using DesertCube.Modules.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DesertCube.Modules.Shop
{
    public abstract class ShopItem
    {
        public abstract string Name { get; }
        public string Description;
        public int Price;

        public virtual bool CanPurchase(MCGalaxy.Player p, string message = "")
        {
            int points = Stats.GetPoints(p.name);
            if (points < Price)
            {
                p.Message($"&cInsufficient points! (&e{points} < {Price}&c)");
                return false;
            }
            return true;
        }
        public virtual void Purchase(MCGalaxy.Player p, string message="")
        {
          
        }

        public virtual void Help(MCGalaxy.Player p)
        {
            p.Message($"/buy {Name} Message");
        }
    }

   
    public class Shop : DesertModule
    {
        public static List<ShopItem> Items = new List<ShopItem>();
        public override void Load()
        {
            var classes = Assembly.GetExecutingAssembly()
                       .GetTypes().Where(t => t.IsClass && t.Namespace != null && t.Namespace.StartsWith("DesertCube.Modules.Shop.Items"))
                       .ToList();
            Items.Clear();
            foreach (var type in classes.Where((x) => { return x.IsSubclassOf(typeof(ShopItem)); }))
                Items.Add((ShopItem)Activator.CreateInstance(type));
        }

        public override void Unload()
        {
            Items.Clear();
        }
    }
}
