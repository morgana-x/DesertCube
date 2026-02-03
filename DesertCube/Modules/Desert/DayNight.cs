using DesertCube.Modules.Server;
using MCGalaxy.Network;
using MCGalaxy.Tasks;
using System;
namespace DesertCube.Modules.Desert
{
    internal class DayNight : DesertModule
    {
        public static bool IsNight => Time.CurrentTime > NightStart || Time.CurrentTime < NightEnd;
        public static bool lastNight = true;

        public static int NightEnd = (int)(Time.MaxTime * 0.26f);
        public static int NightStart = (int)(Time.MaxTime * 0.78f);

        public static EnvConfigDayNight CurrentEnv => IsNight ? NightConfig : DayConfig;


        public override void Load()
        {
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Register(OnSentMap, MCGalaxy.Priority.Low);

            SendEnv(DesertCubePlugin.Bus.Level, CurrentEnv);
        }

        public override void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Unregister(OnSentMap);
        }

        static void OnSentMap(MCGalaxy.Player p, MCGalaxy.Level prev, MCGalaxy.Level current)
        {
            if (current == DesertCubePlugin.Bus.Level)
                SendEnv(p, CurrentEnv);
        }

        static DateTime nextTick = DateTime.Now;
        public override void Tick()
        {
            if (DateTime.Now < nextTick) return;
            nextTick = DateTime.Now.AddSeconds(2);

            if (IsNight == lastNight) return;
            lastNight = IsNight;

            SendEnv(DesertCubePlugin.Bus.Level, CurrentEnv);
        }


        public static void SendEnv(MCGalaxy.Level level, EnvConfigDayNight env)
        {
            if (level == null) return;
            foreach (var player in level.getPlayers())
                SendEnv(player, env);
        }

        public static void SendEnv(MCGalaxy.Player p, EnvConfigDayNight env)
        {
            if (!p.Session.hasCpe) return;
            if (!p.Session.Supports("EnvMapAspect",2) && !p.Session.Supports("EnvMapAspect", 1)) return;
           
            p.Send(Packet.EnvColor((byte)EnvColour.SkyColour    , env.SkyColour[0] , (env.SkyColour[1])      , (env.SkyColour[2])));
            p.Send(Packet.EnvColor((byte)EnvColour.RoadColour   , env.RoadColour[0]     , (env.RoadColour[1])     , (env.RoadColour[2])));

            if (!OverrideFog) // Dodgy logic cause im tired
                p.Send(Packet.EnvColor((byte)EnvColour.FogColour    , env.FogColour[0]    , (env.FogColour[1])      , (env.FogColour[2])));
            else
                p.Send(Packet.EnvColor((byte)EnvColour.FogColour, OverrideFogColour[0], (OverrideFogColour[1]), (OverrideFogColour[2])));

            p.Send(Packet.EnvColor((byte)EnvColour.ShadowColour , env.ShadowColour[0]  , (env.ShadowColour[1])   , (env.ShadowColour[2])));
            p.Send(Packet.EnvColor((byte)EnvColour.SkyboxColour , env.SkyboxColour[0]   , (env.SkyboxColour[1])   , (env.SkyboxColour[2])));
            p.Send(Packet.EnvColor((byte)EnvColour.LightColour, env.SunlightColour[0], (env.SunlightColour[1]), (env.SunlightColour[2])));
            if (!p.Session.Supports("EnvMapAspect", 2)) return;
            p.Send(Packet.SetLightingMode(Packet.LightingMode.Fancy, true));
        }


        static short ToI16Col(byte col) => (short)(((float)col / 255f) * (float)short.MaxValue);

        enum EnvColour
        {
            SkyColour,
            RoadColour,
            FogColour,
            ShadowColour,
            LightColour,
            SkyboxColour,
            LavaLightColour,
            LampLightColour
        }
        public class EnvConfigDayNight
        {
            public short[] SkyColour = new short[] { ToI16Col(0x99), ToI16Col(0xcc), ToI16Col(0xff) };
            public short[] RoadColour = new short[] { short.MaxValue, short.MaxValue, short.MaxValue };
            public short[] FogColour = new short[] { short.MaxValue, short.MaxValue, short.MaxValue };
            public short[] SunlightColour = new short[] { short.MaxValue, short.MaxValue, short.MaxValue };
            public short[] ShadowColour = new short[] { ToI16Col(0x9b), ToI16Col(0x9b), ToI16Col(0x9b) };
            public short[] SkyboxColour = new short[] { short.MaxValue, short.MaxValue, short.MaxValue };
        }
        public static EnvConfigDayNight DayConfig = new EnvConfigDayNight();
        public static EnvConfigDayNight NightConfig = new EnvConfigDayNight()
        {
            SkyColour = new short[] { 0x0, 0x0, 0x0 },
            RoadColour = new short[] { 20, 20, 20 },
            SunlightColour = new short[] { 55, 55, 55 },
            ShadowColour = new short[] { 45, 45, 45 },
            FogColour = new short[] { 0x0, 0x0, 0x0 }
        };

        // This is dodgy I know... but it's 2am!
        public static bool OverrideFog = false;
        public static short[] OverrideFogColour = new short[] { ToI16Col(0xc9), ToI16Col(0xb8), ToI16Col(0x55) };
    }
}