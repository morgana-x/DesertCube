using MCGalaxy;

namespace DesertCube.Commands
{
    public class BusEvent : Command2
    {
        public override string name => "busevent";

        public override string type => "BusAdmin";

        public override LevelPermission defaultRank => LevelPermission.Owner;

      //  public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("clock"), new CommandAlias("gettime") };
        public override void Help(Player p)
        {
            p.Message("/busevent eventname");
        }

        // pure laziness here, I am tired!!! mostly for testing anyway
        public override void Use(Player p, string message)
        {
            if (int.TryParse(message, out int seconds))
            {
                Modules.Event.Event.nextEvent = System.DateTime.Now.AddSeconds(seconds);
                p.Message($"Making event start in {message} seconds");
                return;
            }

            Modules.Event.Event.StartEvent(message);
            p.Message($"Started event {message}");
        }
    }
}
