﻿using MCGalaxy.Tasks;

namespace DesertCube.Modules.Desert
{
    public class Weather // This whole module is pretty lousy, refactor later, was made for sandstorm event so far
    {
        static SchedulerTask weatherTask;

        public static int CurrentFog = int.MaxValue;
        public static bool Changed = false;
        public static void Load()
        {
            weatherTask = MCGalaxy.Server.MainScheduler.QueueRepeat(WeatherTick, null, System.TimeSpan.FromSeconds(2));
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Register(OnSentMap, MCGalaxy.Priority.Normal);
        }

        public static void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Unregister(OnSentMap);
            MCGalaxy.Server.MainScheduler.Cancel(weatherTask);
        }

        static void WeatherTick(SchedulerTask task)
        {
            if (!Changed) return;
            Changed = false;
            foreach (var p in DesertCubePlugin.Bus.GetPlayers())
                SendEnv(p);
        }

        public static void SetFog(int fog=int.MaxValue)
        {
            CurrentFog = fog;
            Changed = true;
        }

        static void SendEnv(MCGalaxy.Player p)
        {
            if (p.level != DesertCubePlugin.Bus.Level) return;
            if (!p.Session.Supports("EnvMapAspect", 2) && !p.Session.Supports("EnvMapAspect", 1)) return;
            p.Send(MCGalaxy.Network.Packet.EnvMapProperty(MCGalaxy.EnvProp.MaxFog, CurrentFog));
        }

        static void OnSentMap(MCGalaxy.Player p, MCGalaxy.Level prev, MCGalaxy.Level current)
        {
            if (current == DesertCubePlugin.Bus.Level)
                SendEnv(p);
        }

    }
}
