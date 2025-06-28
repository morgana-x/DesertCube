using MCGalaxy.Tasks;
using System;

namespace DesertCube.Modules.Desert
{
    public class Time
    {
        public static int CurrentTime = 0;

        public static int MaxTime = 12000;

        public static string FormattedTime => $"{TimeHour.ToString("D2")}:{TimeMinute.ToString("D2")}";
        public static int TimeSeconds { get { return (int)((CurrentTime / (float)MaxTime) * 86400f); } }
        public static int TimeMinute { get { int minutes = TimeSeconds / 60; if (minutes >= 60) { minutes = minutes % 60; } return minutes; } }
        public static int TimeHour { get { return TimeSeconds / 60 / 60; } }


        static SchedulerTask timeTask;
        public static void Load()
        {
            timeTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TimeTick, null, TimeSpan.FromSeconds(1));

            CurrentTime = DayNight.NightEnd + 100;
        }
        public static void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(timeTask);
        }
        static void TimeTick(SchedulerTask task)
        {
            CurrentTime++;
            if (CurrentTime > MaxTime)
                CurrentTime = 0;
        }
        public static int RealSecondsToBusSeconds(int seconds)
        {
            return (int)Math.Round(((float)seconds / 86400f) * MaxTime);
        }

        public static int RealTimeToBusSeconds(int hour, int minute)
        {
            int hourseconds = RealSecondsToBusSeconds(hour * 60 * 60);
            int minuteseconds = RealSecondsToBusSeconds(minute * 60);

            return minuteseconds + hourseconds;
        }
    }
}
