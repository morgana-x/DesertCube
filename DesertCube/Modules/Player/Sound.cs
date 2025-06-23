using MCGalaxy;
using MCGalaxy.Blocks;

namespace DesertCube.Modules.Player
{
    public class Sound
    {
        public static void Load()
        {
            
        }

        public static void Unload()
        {

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
            return player.Supports("PlaySound") || player.Session.ClientName().Contains("playsound");
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
        public static void EmitSound(MCGalaxy.Level level, byte channel, ushort id, ushort x, ushort y, ushort z, byte volume=255, byte rate=100)
        {
            byte[] packet = ExtPlaySound3D(channel, id, x, y, z, volume, rate);

            foreach(var player in level.getPlayers())
            {
                if (!SupportsSoundCPE(player)) continue;
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
