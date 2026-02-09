using DesertCube.Commands;
using DesertCube.Modules.Player;
using MCGalaxy;
using MCGalaxy.SQL;
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

        private const string TableName = "desertbus_player_shop";
        private static ColumnDesc[] ShopSavedataTable = new ColumnDesc[] {
            new ColumnDesc("name", ColumnType.VarChar, 16),
            new ColumnDesc("data", ColumnType.VarChar, 128),
        };

        static Dictionary<string, byte[]> cached = new Dictionary<string, byte[]>();
        public override void Load()
        {
            var classes = Assembly.GetExecutingAssembly()
                       .GetTypes().Where(t => t.IsClass && t.Namespace != null && t.Namespace.StartsWith("DesertCube.Modules.Shop.Items"))
                       .ToList();
            Items.Clear();
            foreach (var type in classes.Where((x) => { return x.IsSubclassOf(typeof(ShopItem)); }))
                Items.Add((ShopItem)Activator.CreateInstance(type));

            try
            {
                Database.CreateTable(TableName, ShopSavedataTable);
            }
            catch (Exception e)
            {
                Logger.Log(LogType.ConsoleMessage, "desertbus_player already defined");
            }

            MCGalaxy.Events.PlayerEvents.OnPlayerDisconnectEvent.Register(OnDisconnect, MCGalaxy.Priority.Normal);
        }

        public override void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerDisconnectEvent.Unregister(OnDisconnect);
            Items.Clear();
            cached.Clear();
        }

        public static void SetData(string player, byte[] data)
        {
            lock (cached)
            {
                if (cached.ContainsKey(player))
                    cached[player] = data;
                else
                    cached.Add(player, data);
            }
            string data_pack = "";
            for (int i = 0; i < data.Length; i++)
                data_pack += (char)data[i];

            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE name=@0", player);
            if (pRows.Count == 0)
            {
                Database.AddRow(TableName, "name, data", player, data_pack);
                return;
            }

            Database.UpdateRows(TableName, "data=@0", "WHERE name=@1", data_pack, player);
        }
        public static byte[] GetData(string player)
        {
            if (cached.ContainsKey(player))
                return cached[player];

            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE name=@0", player);
            byte[] data = new byte[128];
            if (pRows.Count == 0) return data;

            for (int i = 0; i < pRows[0][1].Length; i++)
                data[i] = (byte)pRows[0][1][i];

            return data;
        }

        public static void ModifyData(string player, byte[] data, int offset=0)
        {
            byte[] pdata = GetData(player);
            for (int i=0; i < data.Length && i+offset < pdata.Length; i++)
                pdata[offset+i] = data[i];
            SetData(player, pdata);
        }

        public static void ModifyData(string player, byte b, int offset=0)
            => ModifyData(player, new byte[] { b }, offset);

        public static void ModifyData(string player, int i, int offset=0)
            => ModifyData(player, BitConverter.GetBytes(i), offset);

        void OnDisconnect(MCGalaxy.Player p, string reason)
        {
            if (cached.ContainsKey(p.name))
            {
                lock (cached)
                {
                    cached.Remove(p.name);
                }
            }
        }
    }
}
