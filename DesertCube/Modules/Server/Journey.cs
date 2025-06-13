using MCGalaxy.SQL;
using System.Collections.Generic;

namespace DesertCube.Modules.Server
{
    public class Journey
    {
        private static ColumnDesc[] DesertBusJourneyTable = new ColumnDesc[] {
            new ColumnDesc("id", ColumnType.UInt32, priKey:true),
            new ColumnDesc("distance", ColumnType.UInt64),
        };

        private const string TableName = "desertbus_journey";
        public static void Load()
        {
            Database.CreateTable(TableName, DesertBusJourneyTable);

            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE id=0");
            if (pRows.Count > 0) 
                DesertCubePlugin.TotalDistance = (float)ulong.Parse(pRows[0][1]);
        }

        public static void Unload()
        {
            List<string[]> pRows = Database.GetRows(TableName, "*", "WHERE id=0");
            if (pRows.Count > 0)
                Database.UpdateRows(TableName, "distance=@0", "WHERE id=@1", (ulong)DesertCubePlugin.TotalDistance, 0);
            else 
                Database.AddRow(TableName, "id, distance", 0, (ulong)DesertCubePlugin.TotalDistance);

        }
    }
}
