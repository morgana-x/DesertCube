using MCGalaxy;
using MCGalaxy.SQL;
using System;
using System.Collections.Generic;

namespace DesertCube.Modules.Player
{
    public class Stats
    {
        private static ColumnDesc[] DesertBusPlayerTable = new ColumnDesc[] {
            new ColumnDesc("name", ColumnType.VarChar, 16),
            new ColumnDesc("points", ColumnType.UInt32),
            new ColumnDesc("distance", ColumnType.UInt64)
        };
        private const string TableName = "desertbus_player";
        public static void Load()
        {
            try
            {
                Database.CreateTable(TableName, DesertBusPlayerTable);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.ConsoleMessage, "desertbus_player already defined");
            }
        }
        public static void Unload()
        {

        }

        public static int GetPoints(string player)
        {
            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE name=@0", player);

            return pRows.Count == 0 ? 0 : int.Parse(pRows[0][1]);
        }

        public static void SetPoints(string player, int points)
        {
            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE name=@0", player);
            if (pRows.Count == 0)
            {
                Database.AddRow(TableName, "name, points, distance", player, points, 0);
                return;
            }

            Database.UpdateRows(TableName, "points=@0", "WHERE name=@1", points, player);
        }

        public static void AddPoints(string player, int points)
        {
            SetPoints(player, GetPoints(player) + points);
        }


        public static ulong GetDistance(string player)
        {
            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE name=@0", player);

            return pRows.Count == 0 ? 0 : pRows[0][2] != string.Empty ? ulong.Parse(pRows[0][2]) : 0;
        }

        public static void SetDistance(string player, ulong dist)
        {
            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE name=@0", player);
            if (pRows.Count == 0)
            {
                Database.AddRow(TableName, "name, points, distance", player, 0, dist);
                return;
            }

            Database.UpdateRows(TableName, "distance=@0", "WHERE name=@1", dist, player);
        }

        public static void AddDistance(string player, int dist)
        {
            SetDistance(player, GetDistance(player) + (ulong)dist);
        }


        public static KeyValuePair<string[], int[]> GetDistanceLeaderboard(int page = 0, int pagesize = 10)
        {
            return GetLeaderboard("distance", page, pagesize);
        }
        public static KeyValuePair<string[], int[]> GetPointLeaderboard(int page = 0, int pagesize = 10)
        {
            return GetLeaderboard("points", page, pagesize);
        }

        public static int GetMaxPages(int pagesize = 10)
        {
            return (Database.CountRows(TableName) / pagesize);
        }

        public static KeyValuePair<string[], int[]> GetLeaderboard(string column, int page = 0, int pagesize = 10)
        {
            List<string> PlayerNames = new List<string>();
            List<int> PlayerScores = new List<int>();

            List<string[]> pRows = Database.GetRows(TableName, $"name, {column}", $"ORDER BY {column} DESC LIMIT {pagesize} OFFSET {page * pagesize}");
            foreach (var row in pRows)
            {
                PlayerScores.Add(row[1].Trim() != "" ? int.Parse(row[1]) : 0);
                PlayerNames.Add(row[0]);
            }
            return new KeyValuePair<string[], int[]>(PlayerNames.ToArray(), PlayerScores.ToArray());
        }
    }
}
