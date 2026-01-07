using MCGalaxy;
using MCGalaxy.DB;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Network;
using MCGalaxy.SQL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesertCube.Modules.Player
{
    public class Inventory
    {
        private static ColumnDesc[] DesertBusPlayerTable = new ColumnDesc[] {
            new ColumnDesc("name", ColumnType.VarChar, 16),
            new ColumnDesc("inventory", ColumnType.VarChar, 1024)
        };
        private const string TableName = "desertbus_inventory";

        public static Dictionary<string, Dictionary<ushort, ushort>> Cache = new Dictionary<string, Dictionary<ushort, ushort>>();
        public static void Load()
        {
            Database.CreateTable(TableName, DesertBusPlayerTable);
            OnPlayerSpawningEvent.Register(EventPlayerSpawn, Priority.High);
            OnSentMapEvent.Register(EventSentMap, Priority.Normal);

            foreach (var pl in MCGalaxy.PlayerInfo.Online.Items)
                SendBlockOrder(pl);
        }
        public static void Unload()
        {
            OnPlayerSpawningEvent.Unregister(EventPlayerSpawn);
            OnSentMapEvent.Unregister(EventSentMap);
        }

        public static Dictionary<ushort, ushort> GetInventory(string player)
        {
            return new Dictionary<ushort, ushort>()
            {
                [66] = 1,
                [67] = 1,
                [68] = 1,
                [69] = 1,
                [70] = 1,
                [71] = 1,
                [72] = 1,
                [73] = 1,
                [74] = 1,
                [75] = 1,
                [76] = 1,
                [77] = 1,
                [78] = 1,
                [79] = 1,
              //  [100] = 1
            };
            if (Cache.ContainsKey(player)) return Cache[player];

            Dictionary<ushort, ushort> inv = new Dictionary<ushort, ushort>();

            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE name=@0", player);

            if (pRows.Count == 0)
                return inv;


            var rawinventory = System.Text.Encoding.UTF8.GetBytes(pRows[0][1]);
            for (int i=0; i<rawinventory.Length/4; i+=4)
            {
                ushort id     = BitConverter.ToUInt16(rawinventory, i);
                ushort amount = BitConverter.ToUInt16(rawinventory, i+4);
                inv.Add(id, amount);
            }

            Cache.Add(player, inv);
            return inv;
        }

        public static void SaveInventory(string player, Dictionary<ushort, ushort> inv)
        {
            if (Cache.ContainsKey(player))
                Cache[player] = inv;
            else
                Cache.Add(player, inv);

            byte[] rawinventory = new byte[inv.Count * 4];
            for (int i=0; i < inv.Count; i++)
            {
                byte[] keybytes = BitConverter.GetBytes(inv.Keys.ElementAt(i));
                byte[] valbytes = BitConverter.GetBytes(inv.Values.ElementAt(i));

                int offset = i * 4;
                rawinventory[offset] = keybytes[0];
                rawinventory[offset+1] = keybytes[1];
                rawinventory[offset + 2] = valbytes[0];
                rawinventory[offset + 3] = valbytes[1];
            }

            string inventorystr = System.Text.Encoding.UTF8.GetString(rawinventory);

            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE name=@0", player);

            if (pRows.Count == 0)
            {
                Database.AddRow(TableName, "name, inventory", player, inventorystr);
                return;
            }

            Database.UpdateRows(TableName, "inventory=@0", "WHERE name=@1", inventorystr, player);
        }
        public static void SendBlockOrder(MCGalaxy.Player player)
        {
            if (player.Level.name != DesertCubePlugin.Config.BusLevel) return;
            if (!player.Session.hasCpe) return;
    
            var inventory = GetInventory(player.name);

            List<byte> bulk = new List<byte>();
            ushort x = 1;
            for (ushort i = 0; i < 767; i++)
                 bulk.AddRange(Packet.SetInventoryOrder(Block.Air, i, player.Session.hasExtBlocks));

            foreach (var pair in inventory)
            {
                bulk.AddRange(Packet.SetInventoryOrder(pair.Key, x, player.Session.hasExtBlocks));
                x++;
            }

            player.Send(bulk.ToArray());
        }
        public static void SendInventory(MCGalaxy.Player player)
        {
            if (player.Level.name != DesertCubePlugin.Config.BusLevel) return;
            if (!player.Session.hasCpe) return;

            List<byte> bulk = new List<byte>();

            // Send Hotbar
            for (int i = 0; i < 9; i++)
                bulk.AddRange(Packet.SetHotbar(0, (byte)i, player.Session.hasExtBlocks));
            player.Send(bulk.ToArray());



            // Send Inventory Order
            SendBlockOrder(player);


        }
        static void EventSentMap(MCGalaxy.Player plyaer, Level prev, Level cur)
        {
            SendInventory(plyaer);
        }
        private static void EventPlayerSpawn(MCGalaxy.Player player, ref Position pos, ref byte yaw, ref byte pitch, bool respawning)
        {
            SendInventory(player);
        }
    }
}
