using MCGalaxy;
using System.Collections.Generic;

namespace DesertCube.Modules.Player
{
    public class Effect
    {
        public class DefinedEffect
        {
            public byte ID;
            public byte U1 = 0;
            public byte V1 = 0;
            public byte U2 = 10;
            public byte V2 = 10;
            public byte R = 255;
            public byte G = 255;
            public byte B = 255;
            public byte FrameCount = 1;
            public byte ParticleCount = 20;
            public byte Size = 7;
            public float SizeVariation = 1f;
            public float Spread = 0.6f;
            public float Speed = 0.5f;
            public float Gravity = 1f;
            public float BaseLifetime = 4f;
            public float LifetimeVariation = 1f;
            public bool ExpireOnTouchGround = false;
            public bool CollidesSolid = true;
            public bool CollidesLiquid = true;
            public bool CollidesLeaves = false;
            public bool FullBright = false;

            public DefinedEffect(byte id, byte[] colour)
            {
                ID = id;
                R = colour[0];
                G = colour[1];
                B = colour[2];
            }
            public DefinedEffect(byte id, byte r, byte g, byte b, int gravity)
            {
                ID = id;
                R = r;
                G = g;
                B = b;
                Gravity = gravity;
            }

            public DefinedEffect()
            {

            }
        }
        public static Dictionary<byte, DefinedEffect> ParticleDefs = new Dictionary<byte, DefinedEffect>();

        public static void Load()
        {
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Register(EventPlayerSentMap, Priority.Normal);
        }

        public static void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Unregister(EventPlayerSentMap);
        }
        static void EventPlayerSentMap(MCGalaxy.Player p, Level prevLevel, Level level)
        {
            SendDefineEffectAll(p);
        }

        public static void EmitEffect(MCGalaxy.Player p, byte id)
        {
            var packet = MCGalaxy.Network.Packet.SpawnEffect(id, p.Pos.X / 32f, p.Pos.Y / 32f, p.Pos.Z / 32f, p.Pos.X / 32f, p.Pos.Y / 32f, p.Pos.Z / 32f);
            foreach (var pl in PlayerInfo.Online.Items)
            {
                if (pl.level != p.level) continue;
                if (!pl.Session.hasCpe) continue;
                pl.Send(packet);
            }
        }
        public static void AddEffect(byte Id, byte r, byte g, byte b, int gravity)
        {
            AddEffect(Id, new DefinedEffect(Id, r, g, b, gravity));

        }
        public static void AddEffect(byte Id, DefinedEffect effect)
        {

            if (!ParticleDefs.ContainsKey(Id))
                ParticleDefs.Add(Id, effect);
            else
                ParticleDefs[Id] = effect;

            foreach(var pl in MCGalaxy.PlayerInfo.Online.Items)
                SendDefineEffect(pl, effect);
        }

        private static void SendDefineEffect(MCGalaxy.Player p, byte[] effect)
        {
            if (!p.Session.hasCpe) return;
            p.Send(effect);
        }
        public static void SendDefineEffect(MCGalaxy.Player p, DefinedEffect effect)
        {
            if (!p.Session.hasCpe) return;
            SendDefineEffect(p, EffectPacket(effect));
        }

        public static void SendDefineEffectAll(MCGalaxy.Player p)
        {
            if (!p.Session.hasCpe) return;
            foreach (var particle in ParticleDefs)
                SendDefineEffect(p, particle.Value);
        }
        public static byte[] EffectPacket(DefinedEffect effect)
        {
            return MCGalaxy.Network.Packet.DefineEffect(effect.ID, effect.U1, effect.V1, effect.U2, effect.V2,
                effect.R, effect.G, effect.B, effect.FrameCount, effect.ParticleCount, effect.Size, effect.SizeVariation,
                effect.Spread, effect.Speed, effect.Gravity, effect.BaseLifetime, effect.LifetimeVariation,
                effect.ExpireOnTouchGround, effect.CollidesSolid, effect.CollidesLiquid, effect.CollidesLeaves, effect.FullBright);
        }
    }
}
