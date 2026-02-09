using MCGalaxy;
using MCGalaxy.Commands;

namespace DesertCube.Commands
{
    public class PointsAdd : Command2
    {
        public override string name => "pointsadd";

        public override string type => "BusAdmin";

        public override LevelPermission defaultRank => LevelPermission.Owner;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("addpoints") };
        public override void Help(Player p)
        {
            p.Message("/addpoints player points");
        }

        public override void Use(Player p, string message)
        {
            var args = message.SplitSpaces(2);

            var target = MCGalaxy.PlayerInfo.FindMatches(p, args[0]);
            if (target == null)
                return;

            if (!int.TryParse(args[1], out var amount))
            {
                target.Message("Invalid score int!");
                return;
            }

            Modules.Player.Stats.AddPoints(target.truename, amount);
            p.Message($"Added {amount} points to {target.name}! Now has {Modules.Player.Stats.GetPoints(target.truename)} points!");
        }
    }
}
