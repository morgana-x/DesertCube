using DesertCube.Modules.Server;
using MCGalaxy;
using MCGalaxy.Network;
using System;
using System.Collections.Generic;

namespace DesertCube.Modules.Desert
{
    internal class DayNight : DesertModule
    {
        public static bool IsNight => Time.CurrentTime > NightStart || Time.CurrentTime < NightEnd;
        public static DayTime lastDayState = DayTime.Day;

        public static int DuskStart = (int)(Time.MaxTime * 0.833333);
        public static int NightStart = (int)(Time.MaxTime * 0.872916666667f);

        public static int NightEnd = (int)(Time.MaxTime * 0.2084f);
        public static int DayStart = (int)(Time.MaxTime * 0.22916666666667f);
        public static DayTime CurrentTime { get {
                if (Time.CurrentTime >= NightEnd && Time.CurrentTime <= DayStart)
                    return DayTime.Dawn;

                if (Time.CurrentTime <= NightStart && Time.CurrentTime >= DuskStart)
                    return DayTime.Dusk;

                if (IsNight)
                    return DayTime.Night;

                return DayTime.Day;
        }}

        public static EnvConfigDayNight CurrentEnv => Envs[CurrentTime];



        public override void Load()
        {
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Register(OnSentMap, MCGalaxy.Priority.Low);

            var lvl = LevelInfo.FindExact(DesertCubePlugin.Config.BusLevel);
            if (lvl != null)
                SendEnv(lvl, CurrentEnv);
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
        public override void Tick(float curTime)
        {
            if (DateTime.Now < nextTick) return;
            nextTick = DateTime.Now.AddSeconds(1);

            if (CurrentTime == lastDayState) return;
            lastDayState = CurrentTime;

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

            p.Send(Packet.EnvColor((byte)EnvColour.FogColour    , env.FogColour[0]    , (env.FogColour[1])      , (env.FogColour[2])));
          
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

        public enum DayTime
        {
            Dawn,
            Day,
            Dusk,
            Night
        }
        public static Dictionary<DayTime, EnvConfigDayNight> Envs = new Dictionary<DayTime, EnvConfigDayNight>()
        {
            [DayTime.Dawn] = new EnvConfigDayNight()
            {
                SkyColour = new short[] { (32), (32), (96) },
                ShadowColour = new short[] { (22), (22), (69) },
                SunlightColour = new short[] { (152), (91), (67) },
                RoadColour = new short[] { (201), (164), (151) },
                FogColour = new short[] { (152), (91), (67) },
            },
            [DayTime.Day] = new EnvConfigDayNight(),
            [DayTime.Dusk] = new EnvConfigDayNight()
            {
                SkyColour = new short[] { (192), (128), (160) },
                ShadowColour = new short[] { (60), (53), (49) },
                SunlightColour = new short[] { (222), (168), (141) },
                RoadColour = new short[] { (220), (184), (172) },
                FogColour = new short[] { (224), (160), (104) },
            },
            [DayTime.Night] = new EnvConfigDayNight()
            {
                SkyColour = new short[] { 0x0, 0x0, 0x0 },
                RoadColour = new short[] { 20, 20, 20 },
                SunlightColour = new short[] { 55, 55, 55 },
                ShadowColour = new short[] { 45, 45, 45 },
                FogColour = new short[] { 0x0, 0x0, 0x0 }
            }
        };

        // This is dodgy I know... but it's 2am!
       // public static bool OverrideFog = false;
       // public static short[] OverrideFogColour = new short[] { (0xc9), (0xb8), (0x55) };
    }
}
