using System;
using System.Collections.Generic;

namespace DesertCube.Modules.Server
{
    public class Hint : DesertModule
    {
        public static List<string> Hints = new List<string>()
        {
            "%eDo %d/leaderboard %eto get the hottest stats!",
            "%eUse %d/distance %eto check the total distance",
            "%eUse %d/distboard %eto see the distance leaderboard!",
            "%eCheck your points with %d/points",
            "%eDo %d/nextstop%e to check how far the next stop is!",
            "%eWant to hear snack sounds? Use%f https://github.com/morgana-x/ClassiCube/releases/tag/AudioCPE-v1.0.0 %e!",
            "%eSource code can be found at %f https://github.com/morgana-x/DesertCube",
            "%eDo %d/speed%e to get the bus speed!",
            "%eUse %d/clock%e to get the time!",
        };

        public static int index = 0;
        public override void Load()
        {
        }
        public override void Unload()
        {
        }

        DateTime nextHint = DateTime.Now;
        public override void Tick()
        {
            if (DateTime.Now < nextHint)
                return;
            nextHint = DateTime.Now.AddMinutes(60);

            foreach (var p in DesertCubePlugin.Bus.GetPlayers())
                p.Message(Hints[index]);

            index++;
            if (index >= Hints.Count) index = 0;
        }
    }
}
