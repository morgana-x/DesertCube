using DesertCube.Modules.Server;
using MCGalaxy;
using MCGalaxy.Commands;

namespace DesertCube.Commands
{
    public class BusEvent : Command2
    {
        public override string name => "busevent";

        public override string type => "fun";

        public override LevelPermission defaultRank => LevelPermission.Owner;

      //  public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("clock"), new CommandAlias("gettime") };
        public override void Help(Player p)
        {
            p.Message("/busevent eventname");
        }

        public override void Use(Player p, string message)
        {
            Modules.Event.Event.StartEvent(message);
            p.Message($"Started event {message}");
        }
    }
}
