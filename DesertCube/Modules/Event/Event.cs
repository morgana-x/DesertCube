using MCGalaxy.Tasks;
using System;
using System.Collections.Generic;

namespace DesertCube.Modules.Event
{
    public class Event : DesertModule
    {
        public static List<EventBase> Events = new List<EventBase>()
        { 
         //   new Events.BusBreakdown(),
            new Events.Sandstorm(),
        };

        public static SchedulerTask ScheduleEventTask;

        public static SchedulerTask CurrentEventTask = null;
        public static EventBase CurrentEvent = null;

        public static DateTime nextEvent = DateTime.Now;


        static Scheduler eventScheduler = new Scheduler("desertbusevent");
        public override void Load()
        {
            ScheduleEventTask = MCGalaxy.Server.MainScheduler.QueueRepeat(ChooseEventTask, null, TimeSpan.FromSeconds(60));
            nextEvent = DateTime.Now.AddHours(rnd.Next(5, 6)); // Ensure event is scheduled sometime later
        }

        public override void Unload()
        {
            StopEvent();
        }

        public static void StartEvent(EventBase evnt)
        {
            StopEvent();
            nextEvent = DateTime.Now.AddHours(rnd.Next(7, 12));
            CurrentEvent = evnt;
            CurrentEventTask = eventScheduler.QueueOnce(RunEvent, null, TimeSpan.Zero);
        }

        public static void StartEvent(string eventname)
        {
            foreach(var evnt in Events)
                if (evnt.Name == eventname)
                {
                    StartEvent(evnt);
                    break;
                }
        }
        public static void StopEvent()
        {
            if (CurrentEventTask != null)
                eventScheduler.Cancel(CurrentEventTask);
            CurrentEvent = null;
        }

        static System.Random rnd  = new System.Random();
        static void ChooseEventTask(SchedulerTask task)
        {
            if (DateTime.Now < nextEvent) return;
            if (CurrentEvent != null) return;
            StartEvent(Events[rnd.Next(0, Events.Count)]);
        }
        static void RunEvent(SchedulerTask task)
        {
            CurrentEventTask = task;
            if (CurrentEvent == null) return;

            try
            {
                CurrentEvent.Start();
                try
                {
                    CurrentEvent.Run();
                }
                catch (Exception ex)
                {
                    MCGalaxy.Player.Console.Message("Error running desert bus event!");
                    MCGalaxy.Player.Console.Message(ex.ToString());
                }
                CurrentEvent.End();
            }
            catch(Exception ex)
            {
                MCGalaxy.Player.Console.Message("Error running desert bus event!");
                MCGalaxy.Player.Console.Message(ex.ToString());
            }

            StopEvent();
        }
    }
}
