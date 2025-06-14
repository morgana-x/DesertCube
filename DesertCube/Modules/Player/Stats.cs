﻿using MCGalaxy;
using MCGalaxy.SQL;
using System.Collections.Generic;


namespace DesertCube.Modules.Player
{
    public class Stats
    {
        private static ColumnDesc[] DesertBusPlayerTable = new ColumnDesc[] {
            new ColumnDesc("name", ColumnType.VarChar, 16),
            new ColumnDesc("points", ColumnType.UInt32)
        };
        private const string TableName = "desertbus_player";
        public static void Load()
        {
            Database.CreateTable(TableName, DesertBusPlayerTable);
        }
        public static void Unload()
        {

        }

        public static SortedDictionary<string, int> GetPointLeaderboard(int maxentries=10)
        {
            SortedDictionary<string, int> leaderboard = new SortedDictionary<string, int>();

            List<string[]> pRows = Database.GetRows(TableName, "name, points", $"LIMIT {maxentries}", "ORDER BY DESC");
            foreach(var row in pRows)
                leaderboard.Add(row[0], int.Parse(row[1]));
            return leaderboard;
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
                Database.AddRow(TableName, "name, points", player, points);
                return;
            }

            Database.UpdateRows(TableName, "points=@0", "WHERE name=@1", points, player);
        }

        public static void AddPoints(string player, int points)
        {
            SetPoints(player, GetPoints(player) + points);
        }
    }
}
