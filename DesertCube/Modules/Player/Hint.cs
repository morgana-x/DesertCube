using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Tasks;
using System;
using System.Collections.Generic;

namespace DesertCube.Modules.Player
{
    public class Hint
    {
        public static List<string> Hints = new List<string>()
        { 
            "%eDo %d/leaderboard %eto get the hottest stats!",
            "%eDo %d/distance %eto check the total distance",
        };

        public static int index = 0;
        public static void Load()
        {
            sitTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TickPlayerSit, null, TimeSpan.FromMinutes(120));
        }
        public static void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(sitTask);
        }

        static SchedulerTask sitTask;
        private static void TickPlayerSit(SchedulerTask task)
        {
            sitTask = task;
            foreach(var p in DesertCubePlugin.Bus.GetPlayers())
            {
                p.Message(Hints[index]);
            }
            index++;
            if (index >= Hints.Count) index = 0;
        }
    }
}
