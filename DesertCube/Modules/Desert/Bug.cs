using MCGalaxy;
using System;
using System.Linq;

namespace DesertCube.Modules.Desert
{
    public class Bug : DesertModule
    {
        static string bug_skin = "insect.png";

        static PlayerBot bug;
        static PlayerBot bug_splat;

        public override void Load()
        {
        }

        static Position startPos;
        static Position bugSplatStartPos;
        public override void PostLoad()
        {
            if (DesertCubePlugin.Bus.Level == null) return;

            foreach (var b in DesertCubePlugin.Bus.Level.Bots.Items.Where(x => x.name.StartsWith("insect_")))
                PlayerBot.Remove(b, false);

            int middle = 32*(DesertCubePlugin.Bus.Level.Width / 2);
            startPos = new Position( RoadObjects.spawnDistance * 32, RoadObjects.despawnedYPos * 32, middle);
            bugSplatStartPos = new Position((32*74)+1, RoadObjects.despawnedYPos * 32, middle);

            bug = new PlayerBot("insect_", DesertCubePlugin.Bus.Level) { SkinName = DesertCubePlugin.Config.AssetUrlPrefix+ bug_skin, Model = "insect|1.5", DisplayName="", Pos = startPos };
            bug_splat = new PlayerBot("insect_splat", DesertCubePlugin.Bus.Level) { SkinName = DesertCubePlugin.Config.AssetUrlPrefix + bug_skin, Model = "insect_splat|1.5", DisplayName = "", Pos = startPos };
            PlayerBot.Add(bug, false);
            PlayerBot.Add(bug_splat, false);

        }
        public override void Unload()
        {
            PlayerBot.Remove(bug, false);
            PlayerBot.Remove(bug_splat, false);
        }


        static System.Random rnd = new System.Random();
        static DateTime nextBug = DateTime.Now.AddHours(rnd.Next(4, 5));
        static DateTime cleanBug = DateTime.Now;
        public override void Tick(float deltaTime)
        {
            if (DateTime.Now > nextBug && bug_splat.Pos.BlockY <= RoadObjects.despawnedYPos && bug.Pos.BlockY <= RoadObjects.despawnedYPos)
            {
                SpawnBug();
                return;
            }

            if (bug.Pos.BlockY > RoadObjects.despawnedYPos)
            {
                var velx = DesertCubePlugin.Bus.BusSpeedInGame > 0 ? DesertCubePlugin.Bus.BusSpeedInGame :
                    Math.Sin(DateTime.Now.Ticks) * 0.05f;
                var velz = (int)(32 * deltaTime * Math.Cos(DateTime.Now.Ticks) * 0.5f);
                var vely = (int)(32 * deltaTime * Math.Sin(DateTime.Now.Ticks) * 0.5f);
                var next = (int)(32 * deltaTime * velx);

                if (bug.Pos.X - next < 74 * 32)
                {
                    bug_splat.Pos = new Position(bugSplatStartPos.X, bug.Pos.Y + vely, bug.Pos.Z + velz);
                    bug.Pos = startPos;
                    cleanBug = DateTime.Now.AddMinutes(10);
                }
                else
                {
                    bug.Pos = new Position(bug.Pos.X - next, bug.Pos.Y + vely, bug.Pos.Z + velz);
                }
            }

            if (bug_splat.Pos.BlockY > RoadObjects.despawnedYPos && DateTime.Now > cleanBug)
                CleanSplat();
            
        }

        public static void SpawnBug()
        {
            if (Plugin.FindCustom("CustomModels") == null)
            {
                return;
            }
            nextBug = DateTime.Now.AddHours(rnd.Next(5, 6));
            if (DesertCubePlugin.Bus.Level == null) return;

            int y =  (rnd.Next(22, 23)+1) * 32;
            int middle = ( 32 * rnd.Next(57, 64)) + 16;
            bug.Pos = new Position(startPos.X, y, middle);
            bug_splat.Pos = new Position(74, -2048, middle);
        }

        public static void CleanSplat()
        {
            cleanBug = DateTime.Now;
            bug_splat.Pos = bugSplatStartPos;
        }
    }
}
