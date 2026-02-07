using MCGalaxy;
using System;

namespace DesertCube.Modules.Player
{
    internal class LeaveBehind : DesertModule
    {
        public override void Load()
        {
        }
        public override void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerMoveEvent.Unregister(OnPlayerMoveEvent);
        }
        public override void PostLoad()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerMoveEvent.Register(OnPlayerMoveEvent, Priority.Normal);
        }
        // DateTime nextCheck = DateTime.Now;
        private void OnPlayerMoveEvent(MCGalaxy.Player p, Position next, byte yaw, byte pitch, ref bool cancel)
        {
            if (DesertCubePlugin.Bus == null) return;
            if (DesertCubePlugin.Bus.BusSpeed == 0) return;
            if (CheckPlayer(p))
            {
                p.Pos = DesertCubePlugin.Bus.Level.SpawnPos;
                p.SendPosition(DesertCubePlugin.Bus.Level.SpawnPos, p.Rot );
                cancel = true;
            }

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
            if (player.Game.Referee) return false;
            if (player.Level != DesertCubePlugin.Bus.Level) return false;
            if (DesertCubePlugin.Bus.InsideBus(player)) return false;
            return true;
        }
    }
}
