using MCGalaxy;
using System.Linq;

namespace DesertCube.Commands
{
    public class Leaderboard : Command2
    {
        public override string name => "leaderboard";

        public override string type => "fun";

        public override LevelPermission defaultRank => LevelPermission.Guest;
        public override void Help(Player p)
        {
            p.Message("/leaderboard - get the top point owners!");
        }

        public override void Use(Player p, string message)
        {
            int page = 0;
            int maxpages = Modules.Player.Stats.GetMaxPages();

            if (message.Length > 0)
            {
                if (!int.TryParse(message, out page))
                {
                    p.Message("%cIncorrect int for page!");
                    return;
                }
                page = page - 1;

                if (page < 0)
                {
                    p.Message("%cCan't have a page below 1!");
                    return;
                }
                if (page > maxpages)
                {
                    p.Message("%cThere's not that many pages!");
                    return;
                }
            }

            var leaderboard = Modules.Player.Stats.GetPointLeaderboard(page);

            p.Message("%e=======================");
            p.Message("%eDesert Bus Leaderboard:");
            p.Message("%e=======================");
            if (leaderboard.Key.Length == 0)
                p.Message("%eNoone has any points yet!");
            for (int i = 0; i < leaderboard.Key.Length; i++)
                p.Message($"%e{i+1}. %7{leaderboard.Key.ElementAt(i)} %d{leaderboard.Value.ElementAt(i)}");
            p.Message("%e=======================");
            p.Message($"%eShowing page %d{page+1}%e/%d{maxpages+1}.");
        }
    }
}
