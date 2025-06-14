using MCGalaxy;
using MCGalaxy.Tasks;
using System;
using System.ComponentModel;
using System.IO;

namespace DesertCube.Modules.Desert
{
    internal class Stop
    {

        public static int nextStopMeters = 0;
        public static bool loaded = false;
        public static string nextStop = "";
        public static void Load()
        {
            nextStopMeters = (int)DesertCubePlugin.TotalDistance + rnd.Next(10000, 50000);
            nextStop = RandomStop();
        }

        public static void Unload()
        {
            if (DesertCubePlugin.Bus.Level != null)
                ClearBusStop(DesertCubePlugin.Bus.Level);
        }

        public static void ArriveBusStop()
        {
            if (DesertCubePlugin.TotalDistance < nextStopMeters) return;

            if (nextStop != "")
                LoadBusStop(DesertCubePlugin.Bus.Level, nextStop);


            nextStopMeters = (int)DesertCubePlugin.TotalDistance + rnd.Next(10000, 50000);
            nextStop = RandomStop();

            if (!loaded) return;

            int minutes = rnd.Next(30, 120);
            DesertCubePlugin.Bus.stopUntil = DateTime.Now.AddSeconds(minutes);
            DesertCubePlugin.Bus.Broadcast($"%eWe've arrived at a %dstop%e! We'll be here for %d{minutes}%e seconds!");
            MCGalaxy.Server.MainScheduler.QueueOnce((SchedulerTask task) => {
                DesertCubePlugin.Bus.Broadcast($"%eThat's enough of this %cboring %eplace! %cBack on the road!"); }, null, TimeSpan.FromSeconds(minutes));
        }

        public static void LoadBusStop(MCGalaxy.Level level, string name)
        {
         
            if (loaded) ClearBusStop(level);

            string folder = $"{DesertConfig.SaveFolder}/stops";
            string path = $"{folder}/{name}.stop";
            MCGalaxy.Player.Console.Message(path);
            if (!Directory.Exists(folder)) return;
            if (!File.Exists(path)) return;
            MCGalaxy.Player.Console.Message($"Loading stop {path}");
            byte[] bytes = File.ReadAllBytes(path);
            MCGalaxy.Player.Console.Message($"Read {bytes.Length} bytes");
            MemoryStream stream = new MemoryStream(bytes);
         
            BinaryReader br = new BinaryReader(stream);
            MCGalaxy.Player.Console.Message($"Reading data...");
            br.BaseStream.Position = 0;
            ushort width = br.ReadUInt16();
            ushort height = br.ReadUInt16();
            ushort length = br.ReadUInt16();
            MCGalaxy.Player.Console.Message($"Bus stop size {width} {height} {length}");

            for (ushort x = 0; x < width; x++)
                for (ushort y = 0; y < height; y++)
                    for (ushort z = 0; z < length; z++)
                    {
                        var block = br.ReadUInt16();
                        if (block == 0) continue;
                     //   MCGalaxy.Player.Console.Message($"Placing block {block} at {x} {y+16} {z}");
                        level.UpdateBlock(MCGalaxy.Player.Console, x, (ushort)(y+16), z, block);
                    }

            loaded = true;
        }

        static System.Random rnd = new System.Random();

        public static string RandomStop()
        {
            string folder = $"{DesertConfig.SaveFolder}/stops";
            if (!Directory.Exists(folder)) return "";
            var files = Directory.GetFiles(folder);
            return Path.GetFileNameWithoutExtension(files[rnd.Next(files.Length)]);
        }
        public static void LoadBusStop(Level level)
        {
            var stop = RandomStop();
            if (stop == "") return;
            LoadBusStop(level, stop);
        }

        public static void ClearBusStop(Level level)
        {
            if (!loaded) return;
            for (ushort x = 0; x < level.Width; x++)
                for (ushort y = 16; y < level.Height; y++)
                    for (ushort z = 0; z < 47; z++)
                    {
                        level.UpdateBlock(MCGalaxy.Player.Console, x, y, z, 0);
                    }
        }
    }
}
