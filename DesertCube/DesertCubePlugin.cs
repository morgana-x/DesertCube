using MCGalaxy;
using System.Collections.Generic;

namespace DesertCube
{
    public class DesertCubePlugin : Plugin
    {
        public override string name { get { return "DesertCube"; } }
        public override string MCGalaxy_Version { get { return "1.9.5.3"; } }
        public override int build { get { return 3; } }
        public override string welcome { get { return "DesertCube loaded!"; } }
        public override string creator { get { return "morgana"; } }
        public override bool LoadAtStartup { get { return true; } }

        public static string Version = "0.3";

        public static string SoftwareName = "&eDesert Bus";
        public static string SoftwareNameVersioned { get {  return $"{SoftwareName} &b{Version}&f"; } }

        public static DesertBus.DesertBus Bus;

        public static DesertConfig Config;

        public List<Command> Commands = new List<Command>()
        {
            new Commands.Points(),
            new Commands.PointsSet(),
            new Commands.Leaderboard(),
            new Commands.LeaderboardDistance(),

            new Commands.BusLevel(),
            new Commands.BusSpeed(),
            new Commands.BusDistance(),
            new Commands.BusTime(),
            new Commands.BusSetTime(),

            new Commands.BusStopSkip(),
            new Commands.BusStopLoad(),
            new Commands.BusStopSave(),
            new Commands.BusNextStop(),

            new Commands.BusEvent(),

            new Commands.ConfigReload()
        };
        public override void Load(bool startup)
        {
            Config = DesertConfig.Load();

            Bus = new DesertBus.DesertBus(Config.BusLevel);

            Modules.Chat.Cef.Load();

            Modules.Server.Hint.Load();
            Modules.Server.Journey.Load();
            Modules.Server.Name.Load();
            Modules.Server.Time.Load();

            Modules.Desert.DayNight.Load();
            Modules.Desert.Stop.Load();
            Modules.Desert.Weather.Load();

            Modules.Event.Event.Load();

            Modules.Item.Snacks.Load();

            Modules.Player.AntiGrief.Load();
            Modules.Player.Effect.Load();

            Modules.Player.Hold.Load();
            Modules.Player.Inventory.Load();
            Modules.Player.LeaveBehind.Load();
            Modules.Player.Sit.Load();
            Modules.Player.Sound.Load();
            Modules.Player.Stats.Load();
            Modules.Player.StatusHud.Load();

            Modules.Vegas.Gamble.Load();

            foreach (var cmd in this.Commands)
                Command2.Register(cmd);
        }
        public override void Unload(bool shutdown)
        {
            Bus.Unload();

            Modules.Chat.Cef.Unload();

            Modules.Server.Hint.Unload();
            Modules.Server.Journey.Unload();
            Modules.Server.Name.Unload();
            Modules.Server.Time.Unload();

            Modules.Desert.DayNight.Unload();
            Modules.Desert.Stop.Unload();
            Modules.Desert.Weather.Unload();

            Modules.Event.Event.Unload();

            Modules.Item.Snacks.Unload();

            Modules.Player.AntiGrief.Unload();
            Modules.Player.Effect.Unload();
            Modules.Player.Hold.Unload();
            Modules.Player.Inventory.Unload();
            Modules.Player.LeaveBehind.Unload();
            Modules.Player.Sit.Unload();
            Modules.Player.Sound.Unload();
            Modules.Player.Stats.Unload();
            Modules.Player.StatusHud.Unload();

            Modules.Vegas.Gamble.Unload();


            foreach (var cmd in this.Commands)
                MCGalaxy.Command2.Unregister(cmd);
        }
    }
}
