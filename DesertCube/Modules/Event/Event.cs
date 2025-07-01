using MCGalaxy.Tasks;
using System;
using System.Collections.Generic;

namespace DesertCube.Modules.Event
{
    public class Event
    {
        public List<EventBase> Events = new List<EventBase>()
        { 
            new Events.BusBreakdown(),
            new Events.Sandstorm(),
        };

        public static SchedulerTask CurrentEventTask = null;
        public static EventBase CurrentEvent = null;
        public static void Load()
        {

        }

        public static void Unload()
        {
            StopEvent();
        }

        public static void StartEvent(EventBase evnt)
        {
            StopEvent();
            CurrentEvent = evnt;
            CurrentEventTask = MCGalaxy.Server.MainScheduler.QueueOnce(RunEvent, null, TimeSpan.Zero);
        }
        public static void StopEvent()
        {
            if (CurrentEventTask != null)
                MCGalaxy.Server.MainScheduler.Cancel(CurrentEventTask);
            CurrentEvent = null;
        }
        static void RunEvent(SchedulerTask task)
        {
            CurrentEventTask = task;
            if (CurrentEvent == null) return;
            CurrentEvent.Start();
            CurrentEvent.Run();
            CurrentEvent.End();
        }
    }
}
