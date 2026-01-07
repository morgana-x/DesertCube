using MCGalaxy;
using MCGalaxy.Commands;

namespace DesertCube.Commands
{
    public class PointsSet : Command2
    {
        public override string name => "pointsset";

        public override string type => "BusAdmin";

        public override LevelPermission defaultRank => LevelPermission.Owner;

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("setpoints") };
        public override void Help(Player p)
        {
            p.Message("/setpoints player points");
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

            Modules.Player.Stats.SetPoints(target.truename, amount);

            p.Message($"Set {target.name}'s points to {amount}!");
        }
    }
}
