using MCGalaxy;

namespace DesertCube.Commands
{
    public class Leaderboard : LeaderboardCommand
    {
        public override string name => "leaderboard";

        public override string type => "fun";

        public override LevelPermission defaultRank => LevelPermission.Guest;
        public override void Help(Player p)
        {
            p.Message("/leaderboard - get the top point owners!");
        }
    }
}
