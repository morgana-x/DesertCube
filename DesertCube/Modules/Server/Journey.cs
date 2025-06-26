using DesertCube.Modules.Desert;
using MCGalaxy.SQL;
using MCGalaxy.Tasks;
using System;
using System.Collections.Generic;

namespace DesertCube.Modules.Server
{
    public class Journey
    {

        public enum DestinationType
        {
            Forward,
            Backward
        }

        private static ColumnDesc[] DesertBusJourneyTable = new ColumnDesc[] {
            new ColumnDesc("id", ColumnType.UInt32, priKey:true),
            new ColumnDesc("distance", ColumnType.UInt64),
            new ColumnDesc("destination", ColumnType.UInt8),
        };

        public static DestinationType Destination = DestinationType.Forward;
        public static string DestinationName { get { return (Destination == DestinationType.Forward) ? DesertCubePlugin.Config.DestinationName : DesertCubePlugin.Config.OriginName;  } }

        public volatile static float TotalDistance = 0f;
        public static float TotalDistanceKilometers { get { return (float)Math.Ceiling(TotalDistance / 1000f); } } 
        public static float RemainingDistance { get { return (DesertCubePlugin.Config.DestinationDistance - TotalDistance); } }
        public static int RemainingDistanceKilometers { get { return (int)Math.Ceiling(RemainingDistance / 1000f); } }


        private const string TableName = "desertbus_journey";

        static SchedulerTask autosavetask;
        public static void Load()
        {
            Database.CreateTable(TableName, DesertBusJourneyTable);

            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE id=0");
            if (pRows.Count > 0)
            {
                TotalDistance = (float)ulong.Parse(pRows[0][1]);
                Destination = (DestinationType)(byte.Parse(pRows[0][2]));
            }

            autosavetask = MCGalaxy.Server.MainScheduler.QueueRepeat(AutoSave, null, TimeSpan.FromMinutes(5));
        }

        public static void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(autosavetask);
            Save();
        }

        public static void Save()
        {
            byte destination = (byte)Destination;
            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE id=0");
            if (pRows.Count > 0)
                Database.UpdateRows(TableName, "distance=@0, destination=@1", "WHERE id=@2", (ulong)TotalDistance, destination, 0);
            else
                Database.AddRow(TableName, "id, distance, destination", 0, (ulong)TotalDistance, destination);
        }

        static void AutoSave(SchedulerTask task)
        {
            autosavetask = task;
            Save();
        }

        public static void AddDistance(float meters)
        {

            TotalDistance += meters; 

            if (TotalDistance >= Modules.Desert.Stop.nextStopMeters)
            {
                DesertCubePlugin.Bus.SetSpeed(0);
                Modules.Desert.Stop.ArriveBusStop();
                return;
            }

            if (RemainingDistance > 0) return;

            TotalDistance = 0;
            Modules.Desert.Stop.ChooseNextStop();

            foreach (var player in DesertCubePlugin.Bus.GetPlayers())
                DesertCube.Modules.Player.Stats.AddPoints(player.name, 1); // wow!!

            DesertCubePlugin.Bus.Broadcast($"%eWow you %cno lifers %edid it! %d{DestinationName}%e!");
            DesertCubePlugin.Bus.Broadcast("%eFor your troubles you get %a1%e whole point!");

            Destination = (Destination == DestinationType.Forward) ? DestinationType.Backward : DestinationType.Forward;

            MCGalaxy.Server.MainScheduler.QueueOnce((SchedulerTask task) =>
            {
                DesertCubePlugin.Bus.Broadcast($"%eLet's head back towards %d{DestinationName}%e!");
            }, null, TimeSpan.FromSeconds(1));
           
        }
    }
}
