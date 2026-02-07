using MCGalaxy;
using System;

namespace DesertCube.Modules.Player
{
    internal class LeaveBehind : DesertModule
    {
        public override void Load()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerMoveEvent.Register(OnPlayerMoveEvent, Priority.Normal);
        }
        public override void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerMoveEvent.Unregister(OnPlayerMoveEvent);
        }

       // DateTime nextCheck = DateTime.Now;
        private void OnPlayerMoveEvent(MCGalaxy.Player p, Position next, byte yaw, byte pitch, ref bool cancel)
        {
            if (DesertCubePlugin.Bus.Level == null) return;
            if (DesertCubePlugin.Bus.BusSpeed == 0) return;
            if (CheckPlayer(p))
                next = DesertCubePlugin.Bus.Level.SpawnPos;
        }



        /*public override void Tick(float curTime)
        {
            if (DesertCubePlugin.Bus.Level == null) return;
            if (DesertCubePlugin.Bus.BusSpeed == 0) return;

            if (DateTime.Now < nextCheck) return;
            nextCheck = DateTime.Now.AddSeconds(0.25f);

            foreach (var player in DesertCubePlugin.Bus.GetPlayers())
                CheckPlayer(player);
        }*/


        private bool CheckPlayer(MCGalaxy.Player player)
        {
            if (player.Level.name != DesertCubePlugin.Config.BusLevel) return false;
            if (player.Game.Referee) return false;
            if (DesertCubePlugin.Bus.InsideBus(player)) return false;
            player.SendPosition(DesertCubePlugin.Bus.Level.SpawnPos, player.Rot);
            return true;
        }
    }
}
