using MCGalaxy;
using MCGalaxy.Bots;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DesertCube.Modules.Desert
{
    public class RoadObjects : DesertModule
    {
        public class RoadObjectModel
        {
            public float Chance = 1;
            public string Model;

            public string Skin { get
                {
                    if (DesertCubePlugin.Config != null)
                        return DesertCubePlugin.Config.AssetUrlPrefix + _skin;
                    return _skin;
                } }


            public string _skin;

            public int PosRange = 256;

            public RoadObjectModel(string model, string skin= null, int posRange = 64)
            {
                Model = model;
                _skin = skin != null ? skin : model + ".png";
                PosRange = posRange;
            }
            public virtual int GetRandomPos(Level lvl)
            {
                return rnd.Next(2) == 1 ? rnd.Next(-PosRange, 47) : rnd.Next(76, lvl.Length + PosRange);
            }
        }

        public class RoadSignModel : RoadObjectModel
        {
            public RoadSignModel(string model, string skin=null) : base(model, skin, 0) { }

            public override int GetRandomPos(Level lvl)
            {
                return rnd.Next(2) == 1 ? rnd.Next(40, 47) : rnd.Next(76, 83);
            }
        }


        public static Dictionary<string,RoadObjectModel> Models = new Dictionary<string, RoadObjectModel>()
        {
            ["road_sign"] = new RoadSignModel("road_sign|4.5", "road_sign.png") { Chance =  0.05f},
            ["shrub"] = new RoadObjectModel("shrub|1.5", "shrub.png", posRange:32),
            ["shrub_med"] = new RoadObjectModel("shrub|2.5", "shrub.png")
        };

        public  static List<PlayerBot> SpawnedObjects;

        internal const int spawnDistance = 1024;
        internal const int despawnedYPos = -100;
        public override void Load()
        {
            SpawnedObjects = new List<PlayerBot>();
        }

        public override void Unload()
        {
            foreach(PlayerBot bot in SpawnedObjects)
                PlayerBot.Remove(bot, false);

            SpawnedObjects.Clear();
        }

        public override void PostLoad()
        {
            if (DesertCubePlugin.Bus.Level == null) return;
            foreach(var b in DesertCubePlugin.Bus.Level.Bots.Items.Where(x=>x.name.StartsWith("r_obj_")))
                    PlayerBot.Remove(b, false);
        }

        public static System.Random rnd = new System.Random();
        static int count = 0;

        public static void CreateObject(string name)
        {
            // Requires custom models
            if (MCGalaxy.Plugin.FindCustom("CustomModels") == null)
                return;
            if (!Models.ContainsKey(name)) return;

            var lvl = DesertCubePlugin.Bus.Level;
            if (lvl == null) return;


            PlayerBot bot = new PlayerBot($"r_obj_{count}", lvl) {DisplayName=""};
            count++;
            if (count > 100)
                count = 0;

            bot.SetInitialPos(new Position(32*(lvl.Width + spawnDistance), 32*RoadObjects.despawnedYPos, 0));
            
            PlayerBot.Add(bot, false);
            SpawnedObjects.Add(bot);
            SpawnObject(bot, getRandomModel());
        }

        public static void SpawnObject(PlayerBot bot, string name)
        {   
            if (!Models.ContainsKey(name)) return;

            var lvl = DesertCubePlugin.Bus.Level;
            if (lvl == null) return;

            var m = Models[name];

            if (bot.SkinName != m.Skin)
            {
                bot.SkinName = m.Skin;
                bot.SetModel(m.Model);
                bot.DisplayName = "";
                bot.GlobalSpawn();
            }
            else if (m.Model != bot.Model)
                bot.UpdateModel(bot.Model);

            var rnd_z = m.GetRandomPos(lvl);
            bot.Pos = new Position(32 * (lvl.Width + spawnDistance), (DesertCubePlugin.Bus.Level.Config.GetEnvProp(EnvProp.CloudsLevel) + 2) * 32, rnd_z * 32);

        }

        DateTime nextRoadObject = DateTime.Now;

        static string getRandomModel()
        {
            string k = Models.Keys.ToArray()[rnd.Next(0, Models.Keys.Count)];
            RoadObjectModel m;
            for (int i =0; i < Models.Keys.Count; i++)
            {
                k = Models.Keys.ToArray()[rnd.Next(0, Models.Keys.Count)];
                m = Models[k];
                if (m.Chance == 1 || rnd.Next() < m.Chance)
                    return k;
            }
            return k;
        }
        public override void Tick(float deltaTime)
        {
            if (DesertCubePlugin.Bus == null) return;
            if (DesertCubePlugin.Bus.Level == null) return;
            if (DesertCubePlugin.Bus.BusSpeed == 0) return;
            var lvl = DesertCubePlugin.Bus.Level;

            

            foreach (PlayerBot bot in SpawnedObjects.ToList())
            {
                if (bot.Pos.BlockY == RoadObjects.despawnedYPos)
                    continue;

                bot.Pos = new Position(
                    (int)(bot.Pos.X - (32 * deltaTime * DesertCubePlugin.Bus.BusSpeedInGame)),
                    bot.Pos.Y, 
                    bot.Pos.Z);

                if (bot.Pos.X <= (lvl != null ? -32 *(lvl.Width + spawnDistance) : -32* spawnDistance))
                {
                    bot.Pos = new Position(32 * (lvl.Width + spawnDistance), 32*RoadObjects.despawnedYPos, 0);
                    continue;
                }
            }

            if (DateTime.Now > nextRoadObject)
            {
                nextRoadObject = DateTime.Now.AddSeconds(4+(rnd.NextDouble()*2));
                if (SpawnedObjects.Count < DesertCubePlugin.Config.MaxObjects)
                    CreateObject(getRandomModel());
                else
                {
                    var bots = SpawnedObjects.Where((x) => { return x.Pos.BlockY == RoadObjects.despawnedYPos; });
                    if (bots.Count() == 0) return;
                    SpawnObject(bots.First(), getRandomModel());
                }
            }

        }
    }
}
