using MCGalaxy;
using System;
using System.Collections.Generic;
using System.IO;

namespace DesertCube.Commands
{
    public class BusStopSave : Command2
    {
        public override string name => "busstopsave";

        public override string type => "building";

        public override LevelPermission defaultRank => LevelPermission.Owner;
        public override void Help(Player p)
        {
            p.Message("/busstopsave name");
        }

        public override void Use(Player p, string message)
        {
            string name = message;
            if (name.Trim() == "")
                p.Message("Can't have an empty name!");

            List<byte> data = new List<byte>();

            ushort width = p.level.Width;
            ushort height = (ushort)(p.level.Height-16);
            ushort length = 47;

            data.AddRange(BitConverter.GetBytes(width));
            data.AddRange(BitConverter.GetBytes(height));
            data.AddRange(BitConverter.GetBytes(length));

            ushort[] save = new ushort[width * height * length];
            int index = 0;

            for (int x = 0; x < p.level.Width; x++)
                for (int y = 16; y < p.level.Height; y++)
                    for (int z = 0; z < 47; z++)
                    {
                        save[index] = p.level.GetBlock((ushort)x, (ushort)y, (ushort)z);
                        index++;
                    }

            string folder = $"{DesertConfig.SaveFolder}/stops";
            string path = $"{folder}/{name}.stop";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);


            for (int i=0; i < save.Length; i++)
                data.AddRange(BitConverter.GetBytes(save[i]));

            File.WriteAllBytes(path, data.ToArray());

            p.Message($"Saved the bus stop to {name}.stop");
        }
    }
}
