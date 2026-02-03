using MCGalaxy;
using MCGalaxy.Commands.Bots;
using MCGalaxy.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ["shrub_med"] = new RoadObjectModel("shrub|2.5", "shrub.png"),
            ["grass"] = new RoadObjectModel("shrub|1.5", "grass.png", posRange: 32),
            ["grass_med"] = new RoadObjectModel("shrub|2.5", "grass.png"),
        };

        public  static List<PlayerBot> SpawnedObjects;

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
            foreach(var b in DesertCubePlugin.Bus.Level.Bots.Items)
                if (b.name.StartsWith("r_obj_"))
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

            var m = Models[name];

            PlayerBot bot = new PlayerBot($"r_obj_{count}", lvl) {DisplayName="", SkinName=m.Skin, Model=m.Model };
            count++;
            if (count > 100)
                count = 0;

            var rnd_z = m.GetRandomPos(lvl);
            bot.SetInitialPos(new Position(32*(lvl.Width + 1024), (DesertCubePlugin.Bus.Level.Config.GetEnvProp(EnvProp.CloudsLevel)+2) * 32, rnd_z * 32));
            PlayerBot.Add(bot, false);

            SpawnedObjects.Add(bot);
        }

        public static void SpawnObject(PlayerBot bot, string name)
        {   
            if (!Models.ContainsKey(name)) return;

            var lvl = DesertCubePlugin.Bus.Level;
            if (lvl == null) return;

            var m = Models[name];
            bot.DisplayName = "";
            bot.SkinName = m.Skin;
            bot.GlobalSpawn();

            var rnd_z = m.GetRandomPos(lvl);
           // bot.autoBroadcastPosition = true;
            bot.Pos = new Position(32 * (lvl.Width + 1024), (DesertCubePlugin.Bus.Level.Config.GetEnvProp(EnvProp.CloudsLevel) + 2) * 32, rnd_z * 32);

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

        public override void Tick(float curTime)
        {
            if (DesertCubePlugin.Bus.BusSpeed == 0) return;
            var lvl = DesertCubePlugin.Bus.Level;

       
            foreach (PlayerBot bot in SpawnedObjects.ToList())
            {
                if (bot.Pos.Y == -2048)
                    continue;

                bot.Pos = new Position(
                    (int)(bot.Pos.X - (32 * curTime * DesertCubePlugin.Bus.BusSpeedGameWorld)),
                    bot.Pos.Y, 
                    bot.Pos.Z)
                 ;
                if (bot.Pos.X <= (lvl != null ? -32 *(lvl.Width + 1024) : -32*1024))
                {
                    //bot.autoBroadcastPosition = false;
                    bot.Pos = new Position(32 * (lvl.Width + 1024), -2048, 0);
                   /* if (lvl != null)
                    {
                        foreach (var p in lvl.players)
                        {
                            p.EntityList.GetID(bot, out byte id);
                            p.Send(Packet.Teleport(id, bot.Pos, bot.Rot, p.Supports("ExtEntityPositions")));
                        }
                    }*/
                    continue;
                }
            }

            if (DateTime.Now > nextRoadObject)
            {
                nextRoadObject = DateTime.Now.AddSeconds(rnd.Next(1, 3));
                if (SpawnedObjects.Count < 15)
                    CreateObject(getRandomModel());
                else
                {
                    var bots = SpawnedObjects.Where((x) => { return x.Pos.Y == -2048; });
                    if (bots.Count() == 0) return;
                    SpawnObject(bots.First(), getRandomModel());
                }
            }

        }
    }
}
