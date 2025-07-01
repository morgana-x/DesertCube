using MCGalaxy;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertCube.Commands
{
    public class LeaderboardCommand : Command2
    {
        public override string name => "leaderboard";

        public override string type => "fun";

        public override LevelPermission defaultRank => LevelPermission.Guest;
        public override void Help(Player p)
        {
            p.Message("/leaderboard - get the top point owners!");
        }

        public virtual string FormatRow(int index, string name, int value)
        {
            return $"%e{index}. %7{name} %d{value}";
        }

        public virtual string Column => "points";

        public override void Use(Player p, string message)
        {
            int page = 0;
            int pagesize = 5;
            int maxpages = Modules.Player.Stats.GetMaxPages(pagesize);

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

            var leaderboard = Modules.Player.Stats.GetLeaderboard(Column, page, pagesize);

            p.Message("%e=======================");
            p.Message("%eDesert Bus Leaderboard");
            p.Message("%e=======================");
            if (leaderboard.Key.Length == 0)
                p.Message("%eNoone has any data yet!");
            for (int i = 0; i < leaderboard.Key.Length; i++)
                p.Message(FormatRow((page * pagesize) + i + 1, leaderboard.Key.ElementAt(i), leaderboard.Value.ElementAt(i)));
                //p.Message($"%e{(page * pagesize) + i + 1}. %7{leaderboard.Key.ElementAt(i)} %d{FormatDistance(leaderboard.Value.ElementAt(i))}");
            p.Message("%e=======================");
            p.Message($"%eShowing page %d{page + 1}%e/%d{maxpages + 1}%e.");
            p.Message("%e=======================");
        }
    }
}
