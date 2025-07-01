using MCGalaxy;
using MCGalaxy.Commands;

namespace DesertCube.Commands
{
    public class LeaderboardDistance : LeaderboardCommand
    {
        public override string name => "leaderboarddistance";

        public override string type => "fun";

        public override LevelPermission defaultRank => LevelPermission.Guest;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("distboard"), new CommandAlias("distleaderboard"),  new CommandAlias("leaderboarddist") };
        public override void Help(Player p)
        {
            p.Message("/leaderboarddistance - get the top kilometers driven!");
        }

        static string FormatDistance(int dist)
        {
            return dist >= 1000 ? $"{dist / 1000}km" : $"{dist}m";
        }

        public override string FormatRow(int index, string name, int value)
        {
            return $"%e{index}. %7{name} %d{FormatDistance(value)}";
        }

        public override string Column => "distance";

    }
}
