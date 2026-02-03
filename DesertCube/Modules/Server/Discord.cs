using MCGalaxy.Modules.Relay.Discord;
using MCGalaxy.Tasks;
using System;

namespace DesertCube.Modules.Server
{
    public class Discord : DesertModule
    {
        static string oldConfigStatusMessage = "";
        static string ConfigStatusMessage = "Driving to {DEST} with {PLAYERS} players! {DIST} remaining!";
        public override void Load()
        {
            oldConfigStatusMessage = DiscordPlugin.Config.StatusMessage;
            hintTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TickPlayerSit, null, TimeSpan.FromMinutes(1));
        }
        public override void Unload()
        {
            MCGalaxy.Server.MainScheduler.Cancel(hintTask);
            DiscordPlugin.Config.StatusMessage = oldConfigStatusMessage;
        }

        static SchedulerTask hintTask;
        private static void TickPlayerSit(SchedulerTask task)
        {
            DiscordPlugin.Config.StatusMessage = ConfigStatusMessage.Replace("{DIST}", Modules.Server.Journey.RemainingDistanceKilometers.ToString() + "km").Replace("{DEST}", Modules.Server.Journey.DestinationName);
        }
    }
}
