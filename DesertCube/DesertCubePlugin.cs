using MCGalaxy;
using System;
using System.Collections.Generic;

namespace DesertCube
{
    public class DesertCubePlugin : Plugin
    {
        public override string name { get { return "DesertCube"; } }
        public override string MCGalaxy_Version { get { return "1.9.5.3"; } }
        public override int build { get { return 2; } }
        public override string welcome { get { return "DesertCube loaded!"; } }
        public override string creator { get { return "morgana"; } }
        public override bool LoadAtStartup { get { return true; } }

        public static string Version = "0.2";

        public static string SoftwareName = "&eDesert Bus";
        public static string SoftwareNameVersioned { get {  return $"{SoftwareName} &b{Version}&f"; } }

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
            new DesertCube.Commands.BusStopSave(),
            new DesertCube.Commands.BusNextStop(),
        };
        public override void Load(bool startup)
        {
            Config = DesertConfig.Load();

            Bus = new DesertBus.DesertBus(Config.BusLevel);

            Modules.Server.Journey.Load();
            Modules.Server.Name.Load();

            Modules.Desert.Stop.Load();

            Modules.Item.Snacks.Load();

            Modules.Player.AntiGrief.Load();
            Modules.Player.Hint.Load();
            Modules.Player.Hold.Load();
            Modules.Player.Inventory.Load();
            Modules.Player.LeaveBehind.Load();
            Modules.Player.Sit.Load();
            Modules.Player.Stats.Load();
            Modules.Player.StatusHud.Load();


            foreach(var cmd in this.Commands)
                Command2.Register(cmd);
        }
        public override void Unload(bool shutdown)
        {
            Bus.Unload();

            Modules.Server.Journey.Unload();
            Modules.Server.Name.Unload();

            Modules.Desert.Stop.Unload();

            Modules.Item.Snacks.Unload();

            Modules.Player.AntiGrief.Unload();
            Modules.Player.Hint.Unload();
            Modules.Player.Hold.Unload();
            Modules.Player.Inventory.Unload();
            Modules.Player.LeaveBehind.Unload();
            Modules.Player.Sit.Unload();
            Modules.Player.Stats.Unload();
            Modules.Player.StatusHud.Unload();


            foreach (var cmd in this.Commands)
                MCGalaxy.Command2.Unregister(cmd);
        }
    }
}
