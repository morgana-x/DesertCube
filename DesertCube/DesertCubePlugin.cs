using MCGalaxy;
using System;
using System.Collections.Generic;

namespace DesertCube
{
    public class DesertCubePlugin : Plugin
    {
        public override string name { get { return "DesertCube"; } }
        public override string MCGalaxy_Version { get { return "1.9.5.3"; } }
        public override int build { get { return 1; } }
        public override string welcome { get { return "DesertCube loaded!"; } }
        public override string creator { get { return "morgana"; } }
        public override bool LoadAtStartup { get { return true; } }

        public static DesertBus.DesertBus Bus;

        public static DesertConfig Config;

        public volatile static float TotalDistance = 0f;

        public static float RemainingDistance {get { return (Config.DestinationDistance - TotalDistance); }}
        public static int RemainingDistanceKilometers { get { return (int)Math.Ceiling(RemainingDistance / 1000f); } }

        public List<Command> Commands = new List<Command>()
        {
            new DesertCube.Commands.BusLevel(),
            new DesertCube.Commands.BusSpeed(),
            new DesertCube.Commands.Points(),
            new DesertCube.Commands.Leaderboard(),
            new DesertCube.Commands.BusDistance(),
        };
        public override void Load(bool startup)
        {
            Config = DesertConfig.Load();

            Bus = new DesertBus.DesertBus(Config.BusLevel);

            Modules.Player.AntiGrief.Load();
            Modules.Player.Hint.Load();
            Modules.Player.Inventory.Load();
            Modules.Player.LeaveBehind.Load();
            Modules.Player.Sit.Load();
            Modules.Player.Stats.Load();

            Modules.Server.Journey.Load();
            Modules.Server.Name.Load();

            foreach(var cmd in this.Commands)
                Command2.Register(cmd);
        }
        public override void Unload(bool shutdown)
        {
            Bus.Unload();

            Modules.Player.AntiGrief.Unload();
            Modules.Player.Hint.Unload();
            Modules.Player.Inventory.Unload();
            Modules.Player.LeaveBehind.Unload();
            Modules.Player.Sit.Unload();
            Modules.Player.Stats.Unload();

            Modules.Server.Journey.Unload();
            Modules.Server.Name.Unload();

            foreach (var cmd in this.Commands)
                MCGalaxy.Command2.Unregister(cmd);
        }
    }
}
