using MCGalaxy.Events.PlayerEvents;
using MCGalaxy;
using MCGalaxy.Network;
using System.Collections.Generic;

namespace DesertCube.Modules.Player
{
    internal class AntiGrief
    {
        public static void Load()
        {
            OnBlockChangingEvent.Register(EventPlayerBlockChange, Priority.High);
            OnPlayerSpawningEvent.Register(EventPlayerSpawn, Priority.High);
        }
        public static void Unload()
        {
            OnBlockChangingEvent.Unregister(EventPlayerBlockChange);
            OnPlayerSpawningEvent.Unregister(EventPlayerSpawn);
        }

        private static bool CanBuild(MCGalaxy.Player p)
        {
            return p.Rank >= LevelPermission.Operator || p.Level != DesertCubePlugin.Bus.Level;
        }
        private static void EventPlayerBlockChange(MCGalaxy.Player p, ushort x, ushort y, ushort z, ushort block, bool placing, ref bool cancel)
        {
            if (CanBuild(p)) return;

            cancel = true;

            if (p.Session.hasCpe) // Hacked client most likely as we sent block permission packets
            {
                p.Kick("Having fun grrr");
                return;
            }

            p.RevertBlock(x, y, z);
        }

        private static void EventPlayerSpawn(MCGalaxy.Player player, ref Position pos, ref byte yaw, ref byte pitch, bool respawning)
        {
            if (!player.Session.hasCpe) return;
            if (CanBuild(player)) return;

            List<byte> bulk = new List<byte>();
            // Send Can Break/Place Order
            bulk.Clear();
            for (int i = 0; i < 256; i++)
                bulk.AddRange(Packet.BlockPermission((ushort)i, false, false, player.Session.hasExtBlocks));
            player.Send(bulk.ToArray());
        }
    }
}
