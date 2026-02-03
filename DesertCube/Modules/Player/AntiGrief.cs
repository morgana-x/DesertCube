using MCGalaxy.Events.PlayerEvents;
using MCGalaxy;
using MCGalaxy.Network;
using System.Collections.Generic;
using MCGalaxy.Commands.Fun;

namespace DesertCube.Modules.Player
{
    internal class AntiGrief : DesertModule
    {
        public override void Load()
        {
            OnBlockChangingEvent.Register(EventPlayerBlockChange, Priority.High);
            OnPlayerSpawningEvent.Register(EventPlayerSpawn, Priority.High);
            OnPlayerActionEvent.Register(EventPlayerAction, Priority.High);
            OnSentMapEvent.Register(EventSentMap, Priority.High);   
        }
        public override void Unload()
        {
            OnBlockChangingEvent.Unregister(EventPlayerBlockChange);
            OnPlayerSpawningEvent.Unregister(EventPlayerSpawn);
            OnPlayerActionEvent.Unregister(EventPlayerAction);
            OnSentMapEvent.Unregister(EventSentMap);
        }

        private static bool CanBuild(MCGalaxy.Player p)
        {
            return (p.Game.Referee) || p.Level != DesertCubePlugin.Bus.Level;
        }
        private static void EventPlayerBlockChange(MCGalaxy.Player p, ushort x, ushort y, ushort z, ushort block, bool placing, ref bool cancel)
        {
            if (CanBuild(p)) return;

            cancel = true;
            p.RevertBlock(x, y, z);
        }

       
        private static void SendBlockPerms(MCGalaxy.Player player)
        {
            if (!player.Session.hasCpe) return;
            if (player.Level.name != DesertCubePlugin.Config.BusLevel) return; 
            if (CanBuild(player)) { player.SendCurrentBlockPermissions(); return; }
    

            List<byte> bulk = new List<byte>();
            // Send Can Break/Place Order
            bulk.Clear();
            for (int i = 0; i < 256; i++)
                bulk.AddRange(Packet.BlockPermission((ushort)i, false, false, player.Session.hasExtBlocks));
            player.Send(bulk.ToArray());
        }
        private static void EventPlayerAction(MCGalaxy.Player player, PlayerAction action, string message, bool stealth)
        {
            if (player.Level.name != DesertCubePlugin.Config.BusLevel) return;
            if (action != PlayerAction.Referee && action != PlayerAction.UnReferee) return;

            player.Game.Referee = (action == PlayerAction.Referee);
            SendBlockPerms(player);
            Inventory.SendInventory(player);
        }
        private static void EventSentMap(MCGalaxy.Player player, Level lvlprev, Level lvl)
        {
            if (lvl != DesertCubePlugin.Bus.Level) return;
            SendBlockPerms(player);
            Inventory.SendInventory(player);
        }
        private static void EventPlayerSpawn(MCGalaxy.Player player, ref Position pos, ref byte yaw, ref byte pitch, bool respawning)
        {
            if (player.Level.name != DesertCubePlugin.Config.BusLevel) return;
            SendBlockPerms(player);
            Inventory.SendInventory(player);
        }
    }
}
