using MCGalaxy;
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

        }
        public static void Unload()
        {
            OnPlayerSpawningEvent.Unregister(EventPlayerSpawn);
        }

        public static Dictionary<ushort, ushort> GetInventory(string player)
        {
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

        private static void EventPlayerSpawn(MCGalaxy.Player player, ref Position pos, ref byte yaw, ref byte pitch, bool respawning)
        {
            if (!player.Session.hasCpe) return;

            List<byte> bulk = new List<byte>();

            // Send Hotbar
            bulk.Clear();
            for (int i = 0; i < 9; i++)
                bulk.AddRange(Packet.SetHotbar(0, (byte)i, player.Session.hasExtBlocks));
            player.Send(bulk.ToArray());

            var inventory = GetInventory(player.name);


            if (player.Game.Referee) return;
            // Send Inventory Order
            bulk.Clear();
            for (int i = 0; i < 256; i++)
                bulk.AddRange(Packet.SetInventoryOrder((i < inventory.Count) ? inventory.Keys.ElementAt(i) : (ushort)0, (ushort)i, player.Session.hasExtBlocks));
            player.Send(bulk.ToArray());

        }
    }
}
