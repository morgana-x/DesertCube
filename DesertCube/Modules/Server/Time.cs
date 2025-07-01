using DesertCube.Modules.Desert;
using MCGalaxy.SQL;
using MCGalaxy.Tasks;
using System;
using System.Collections.Generic;

namespace DesertCube.Modules.Server
{
    public class Time
    {
        public static int CurrentTime = 5000;

        public static int MaxTime = 12000;

        public static string FormattedTime => $"{TimeHour.ToString("D2")}:{TimeMinute.ToString("D2")}";
        public static int TimeSeconds { get { return (int)(CurrentTime / (float)MaxTime * 86400f); } }
        public static int TimeMinute { get { int minutes = TimeSeconds / 60; if (minutes >= 60) { minutes = minutes % 60; } return minutes; } }
        public static int TimeHour { get { return TimeSeconds / 60 / 60; } }


        static SchedulerTask timeTask;

        private static ColumnDesc[] TimeTable = new ColumnDesc[] {
            new ColumnDesc("id", ColumnType.UInt32, priKey:true),
            new ColumnDesc("time", ColumnType.UInt64),
        };

        private const string TableName = "desertbus_time";

        public static void Load()
        {
            CurrentTime = DayNight.NightEnd + 100;

            Database.CreateTable(TableName, TimeTable);
            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE id=0");
            if (pRows.Count > 0)
                CurrentTime = (int)ulong.Parse(pRows[0][1]);


            timeTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TimeTick, null, TimeSpan.FromSeconds(1));
        }
        public static void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(timeTask);
            Save();
        }
        public static void Save()
        {
            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE id=0");
            if (pRows.Count > 0)
                Database.UpdateRows(TableName, "time=@0", "WHERE id=@1", (ulong)CurrentTime, 0);
            else
                Database.AddRow(TableName, "id, time", 0, (ulong)CurrentTime);
        }

        static void TimeTick(SchedulerTask task)
        {
            CurrentTime++;
            if (CurrentTime > MaxTime)
                CurrentTime = 0;
        }
        public static int RealSecondsToBusSeconds(int seconds)
        {
            return (int)Math.Round(seconds / 86400f * MaxTime);
        }

        public static int RealTimeToBusSeconds(int hour, int minute)
        {
            int hourseconds = RealSecondsToBusSeconds(hour * 60 * 60);
            int minuteseconds = RealSecondsToBusSeconds(minute * 60);

            return minuteseconds + hourseconds;
        }
    }
}
