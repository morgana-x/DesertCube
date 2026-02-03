using DesertCube.Modules.Server;
using MCGalaxy;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Maths;
using MCGalaxy.Network;
using MCGalaxy.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesertCube.Modules.Bus
{
    public class Bus : DesertModule
    {
        public volatile float BusSpeed = 0;

        private SchedulerTask tickTask;

        public Level Level = null;

        public Vec3U16 ButtonPosition = new Vec3U16(72, 21, 62);
        public Vec3U16 DoorPosition = new Vec3U16(69, 18, 56);

        public Vec3U16 Min = new Vec3U16(50, 18, 56);
        public Vec3U16 Max = new Vec3U16(72, 50, 64);

        DateTime nextDecel = DateTime.Now;

        public DateTime stopUntil = DateTime.Now;

        public void SetLevel(string level)
        {
            Level = LevelInfo.FindExact(level);
        }

        public override void Load()
        {
            SetLevel(DesertCubePlugin.Config.BusLevel);

            tickTask = MCGalaxy.Server.MainScheduler.QueueRepeat(Tick, null, TimeSpan.FromMilliseconds(500));

            OnPlayerClickEvent.Register(OnPlayerClick, Priority.Normal);
            OnBlockChangingEvent.Register(OnPlayerChangingBlock, Priority.Normal);
            OnSentMapEvent.Register(OnPlayerSentMap, Priority.Normal);
            SetSpeed(0);
        }
        public override void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(tickTask);
            OnPlayerClickEvent.Unregister(OnPlayerClick);
            OnBlockChangingEvent.Unregister(OnPlayerChangingBlock);
            OnSentMapEvent.Unregister(OnPlayerSentMap);
        }

        void OnPlayerInteract(MCGalaxy.Player p, ushort x, ushort y, ushort z)
        {
            if (p.Level != Level) return;
            if (x == ButtonPosition.X && y == ButtonPosition.Y && z == ButtonPosition.Z)
                Accelerate(DesertCubePlugin.Config.BusAcceleration);
        }
        void OnPlayerClick(MCGalaxy.Player p, MouseButton btn, MouseAction action, ushort yaw, ushort pitch, byte entityID, ushort x, ushort y, ushort z, TargetBlockFace face)
        {
            if (Level == p.level)
                OnPlayerInteract(p, x, y, z);
        }

        void OnPlayerChangingBlock(MCGalaxy.Player p, ushort x, ushort y, ushort z, ushort block, bool placing, ref bool cancel)
        {
            if (Level == p.level && !p.Session.hasCpe)
                OnPlayerInteract(p, x, y, z); // Support for Classic 0.30 Clients
        }
        void OnPlayerSentMap(MCGalaxy.Player p, Level prevLevl, Level level)
        {
            if (level == Level)
                SendBusSpeed(p);
        }
        public List<MCGalaxy.Player> GetPlayers()
        {
            return PlayerInfo.Online.Items.Where((x) => x.Level == Level).ToList();
        }

        public bool InsideBus(Vec3U16 pos)
        {
            return pos.X >= Min.X && pos.Y >= Min.Y && pos.Z >= Min.Z &&
                pos.X <= Max.X && pos.Y <= Max.Y && pos.Z <= Max.Z;
        }

        public bool InsideBus(MCGalaxy.Player p)
        {
            return InsideBus(new Vec3U16((ushort)p.Pos.BlockX, (ushort)p.Pos.BlockY, (ushort)p.Pos.BlockZ));
        }

        void Tick(SchedulerTask task)
        {
            tickTask = task;

            if (Level == null) return;
            if (BusSpeed == 0) return;

            Journey.AddDistance(BusSpeed / 2f);

            if (DateTime.Now > nextDecel)
            {
                nextDecel = DateTime.Now.AddSeconds(1);
                Deccelerate();
            }
        }

        public void Accelerate(float amount)
        {
            if (BusSpeed >= DesertCubePlugin.Config.BusMaxSpeed) return;
            if (DateTime.Now < stopUntil) return;
            if (BusSpeed == 0) { Desert.Stop.ClearBusStop(Level); }
            float speed = BusSpeed + amount;

            if (speed > DesertCubePlugin.Config.BusMaxSpeed)
                speed = DesertCubePlugin.Config.BusMaxSpeed;

            SetSpeed(speed);
            nextDecel = DateTime.Now.AddSeconds(20);
        }

        public void Deccelerate()
        {
            if (BusSpeed <= 0) return;
            float speed = BusSpeed - DesertCubePlugin.Config.BusDecceleration;
            if (speed < 0)
                speed = 0;
            SetSpeed(speed);
        }

        public void SetDoor(ushort block = 0)
        {
            if (Level == null) return;
            for (int i = 0; i < 3; i++)
            {
                if (Level.GetBlock(DoorPosition.X, (ushort)(DoorPosition.Y + i), DoorPosition.Z) == block) continue;
                Level.UpdateBlock(MCGalaxy.Player.Console, DoorPosition.X, (ushort)(DoorPosition.Y + i), DoorPosition.Z, block);
            }
        }

        public int MsToCloud(float speed)
        {
            return (int)(BusSpeed * 5000);
        }

        public void SendBusSpeed(MCGalaxy.Player player)
        {
            SendBusSpeed(player, MsToCloud(BusSpeed));
        }
        public void SendBusSpeed(MCGalaxy.Player player, int speed)
        {
            if (!player.Session.hasCpe) return;
            player.Send(Packet.EnvMapProperty(EnvProp.CloudsSpeed, speed));
        }
        public void SendBusSpeedAll()
        {
            foreach (var player in GetPlayers())
                SendBusSpeed(player);
        }

        public void SetSpeed(float speed)
        {
            if (speed == 0 && BusSpeed != 0) { Desert.Stop.ArriveBusStop(); }
            BusSpeed = speed;
            SendBusSpeedAll();

            SetDoor(speed == 0 ? (ushort)0 : (ushort)49);
        }

        public void Broadcast(string message)
        {
            foreach (var p in GetPlayers())
                p.Message(message);
        }
    }
}
