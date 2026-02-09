using DesertCube.Modules.Server;
using ICSharpCode.SharpZipLib.GZip;
using MCGalaxy;
using MCGalaxy.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DesertCube.Modules.Desert
{
    public class Stop : DesertModule
    {

        public static int nextStopMeters = 0;
        public static bool loaded = false;
        public static string nextStop = "";
        public static bool AtStop = false;

        public override void Load()
        {
            nextStopMeters = (int)Journey.TotalDistance + rnd.Next(10000, 50000);
            nextStop = RandomStop();

        }

        public override void Unload()
        {
            if (DesertCubePlugin.Bus.Level != null)
                ClearBusStop(DesertCubePlugin.Bus.Level);


        }

        public override void Tick(float deltaTime)
        {
            if (!AtStop) return;
            if (DateTime.Now < DesertCubePlugin.Bus.stopUntil) return;

            AtStop = false;
            DesertCubePlugin.Bus.Broadcast($"%eThat's enough of this %cboring %eplace! %cBack on the road!");
        }

        public static void ChooseNextStop()
        {
            nextStopMeters = (int)Journey.TotalDistance + rnd.Next(10000, 50000);
            nextStop = RandomStop();
        }

        public static void ArriveBusStop(string stop="", int minutes = -1, bool force=false)
        {
            if (Journey.TotalDistance < nextStopMeters && !force) return;

            stop = (stop == "") ? nextStop : stop;
            if (stop != "")
                LoadBusStop(DesertCubePlugin.Bus.Level, stop);


            ChooseNextStop();

            if (!loaded) return;

            minutes = minutes == -1 ? rnd.Next(30, 120) : minutes;
            DesertCubePlugin.Bus.stopUntil = DateTime.Now.AddSeconds(minutes);
            DesertCubePlugin.Bus.Broadcast($"%eWe've arrived at a %dstop%e! We'll be here for %d{minutes}%e seconds!");
            AtStop = true;
        }

        static byte[] GZIPHeader = { 0x1F, 0x8B };


        public static bool BusStopExists(string name)
        {
            name = name.EndsWith(".stop") ? name : name + ".stop";
            return File.Exists($"{DesertConfig.SaveFolder}/stops/{name}");
        }


        public static void LoadBusStop(MCGalaxy.Level level, string name)
        {
         
            ClearBusStop(level);

            string folder = $"{DesertConfig.SaveFolder}/stops";
            string path = $"{folder}/{name}.stop";
            MCGalaxy.Player.Console.Message(path);
            if (!Directory.Exists(folder)) return;
            if (!File.Exists(path)) return;
            MCGalaxy.Player.Console.Message($"Loading stop {path}");
            byte[] bytes = File.ReadAllBytes(path);
            MCGalaxy.Player.Console.Message($"Read {bytes.Length} bytes");
            MemoryStream stream = new MemoryStream(bytes);
            MemoryStream outStream = stream;

            // If this is a compressed .stop
            byte[] buffer = new byte[2];
            stream.Read(buffer, 0, 2);
            if (buffer.SequenceEqual(GZIPHeader))
            {
                outStream = new MemoryStream();
                stream.Position = 0;
                GZip.Decompress(stream, outStream, false);
            }

            BinaryReader br = new BinaryReader(outStream);
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
                        if (z >= 48 && z <= 71) continue;
                        //MCGalaxy.Player.Console.Message($"Placing block {block} at {x} {y+16} {z}");
                        level.UpdateBlock(MCGalaxy.Player.Console, x, (ushort)(y+16), z, block);
                    }

            loaded = true;
        }
        public static void SaveBusStop(Level level, string name)
        {
            List<byte> data = new List<byte>();

            ushort width = level.Width;
            ushort height = (ushort)(level.Height - 16);
            ushort length = level.Length;

            data.AddRange(BitConverter.GetBytes(width));
            data.AddRange(BitConverter.GetBytes(height));
            data.AddRange(BitConverter.GetBytes(length));

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int z = 0; z < length; z++)
                    {
                        if (z >= 48 && z <= 71)
                        {
                            data.AddRange(BitConverter.GetBytes((ushort)0));
                            continue;
                        }
                        var block = level.GetBlock((ushort)x, (ushort)(y+16), (ushort)z);
                        data.AddRange(BitConverter.GetBytes(block));
                    }

      
            var instream = new MemoryStream(data.ToArray());
            var outstream = new MemoryStream();
            instream.Position = 0;
            GZip.Compress(instream, outstream, false);


            string folder = $"{DesertConfig.SaveFolder}/stops";
            string path = $"{folder}/{name}.stop";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            File.WriteAllBytes(path, outstream.ToArray());
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
            if (level == null) return;
            for (ushort x = 0; x < level.Width; x++)
                for (ushort y = 16; y < level.Height; y++)
                    for (ushort z = 0; z < level.Length; z++)
                    {
                        if (z >= 48 && z <= 71) continue;
                        if (level.GetBlock(x, y, z) == 0) continue;
                        level.UpdateBlock(MCGalaxy.Player.Console, x, y, z, 0);
                    }
            loaded = false;
        }
    }
}
