using MCGalaxy.Tasks;
using System;
using System.Collections.Generic;

namespace DesertCube.Modules.Server
{
    public class Hint
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
        public static void Load()
        {
            hintTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TickPlayerSit, null, TimeSpan.FromMinutes(60));
        }
        public static void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(hintTask);
        }

        static SchedulerTask hintTask;
        private static void TickPlayerSit(SchedulerTask task)
        {
            hintTask = task;

            foreach (var p in DesertCubePlugin.Bus.GetPlayers())
                p.Message(Hints[index]);

            index++;
            if (index >= Hints.Count) index = 0;
        }
    }
}
