using MCGalaxy;
using MCGalaxy.Blocks;
using System;

namespace DesertCube.Modules.Player
{
    public class Sound : DesertModule
    {
        const string PlaySoundCPE = "PlaySound";
        public override void Load()
        {
            AddCPE(PlaySoundCPE, "Enables server to send sounds", 1);
        }

        public override void Unload()
        {

        }

        static bool ContainsCPE(string name)
        {
            var cpeextensions = CpeExtension.All;
            foreach (var c in cpeextensions)
                if (c.Name == name) return true;
            return false;
        }

        static void AddCPE(string name, string desc, byte version)
        {
            if (ContainsCPE(name)) return;
            var all = CpeExtension.All;
            var newextensions = new CpeExtension[all.Length + 1];
            for (int i=0; i<all.Length; i++)
                newextensions[i] = all[i];
            newextensions[newextensions.Length - 1] = new CpeExtension(name, desc, version) { Enabled = true};

            CpeExtension.All = newextensions;

            MCGalaxy.Player.Console.Message($"Added CPE {name} \"{desc}\" v{version}");
        }

        public class SoundDefinition
        {
            public byte channel;
            public SoundType type;
            public byte rate;
            public byte volume;

            public SoundDefinition(byte channel, SoundType type, byte rate, byte volume)
            {
                this.channel = channel;
                this.type = type;
                this.rate = rate;
                this.volume = volume;
            }
        }


        public static bool SupportsSoundCPE(MCGalaxy.Player player)
        {
            return player.Supports(PlaySoundCPE);
        }

        public static void EmitBlockSound(MCGalaxy.Player player, SoundDefinition sound)
        {
            EmitBlockSound(player, sound.channel, sound.type, sound.volume, sound.rate);
        }
        public static void EmitBlockSound(MCGalaxy.Player player, byte channel, SoundType id, byte volume=255, byte rate = 100)
        {
            EmitBlockSound(player.level, channel, id, (ushort)player.Pos.BlockX, (ushort)player.Pos.BlockY, (ushort)player.Pos.BlockZ, volume, rate);
        }
        public static void EmitBlockSound(MCGalaxy.Level level, byte channel, SoundType id, ushort x, ushort y, ushort z, byte volume=255, byte rate=100)
        {
            EmitSound(level, channel, (ushort)id, x, y, z, volume, rate);
        }

        static float distance(Position pos, ushort x, ushort y, ushort z)
        {
            float dx = ((pos.X / 32f) - x);
            float dy = ((pos.Y / 32f) - y);
            float dz = ((pos.Z / 32f) - z);
            return (float)Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
        }
        public static void EmitSound(MCGalaxy.Level level, byte channel, ushort id, ushort x, ushort y, ushort z, byte volume=255, byte rate=100)
        {
            byte[] packet = ExtPlaySound3D(channel, id, x, y, z, volume, rate);

            foreach(var player in level.getPlayers())
            {
                if (!SupportsSoundCPE(player)) continue;
                if (distance(player.Pos, x,y,z) > (volume/2f))
                        continue;
                player.Send(packet);
            }
        }

        public static byte[] ExtPlaySound3D(byte channel, ushort id, ushort x, ushort y, ushort z, byte volume = 255, byte rate = 100)
        {
            byte[] buffer = new byte[12];
            buffer[0] = 61;
            buffer[1] = channel;
            buffer[2] = volume;
            buffer[3] = rate;
            NetUtils.WriteI16((short)id, buffer, 4);
            NetUtils.WriteI16((short)x, buffer, 6);
            NetUtils.WriteI16((short)y, buffer, 8);
            NetUtils.WriteI16((short)z, buffer, 10);
            return buffer;
        }
    }
}
