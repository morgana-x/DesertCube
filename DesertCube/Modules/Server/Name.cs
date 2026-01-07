using MCGalaxy.Network;

namespace DesertCube.Modules.Server
{
    internal class Name
    {
        static string oldsoftwarename = "";
        public static void Load()
        {
            oldsoftwarename = MCGalaxy.Server.SoftwareNameVersioned;

            if (Christmas.IsChristmasMonth())
                MCGalaxy.Server.SoftwareName = DesertCubePlugin.SoftwareNameVersioned.Replace("&b", "&9").Replace(DesertCubePlugin.SoftwareName, "&bSnow Bus");
            else
                MCGalaxy.Server.SoftwareName = DesertCubePlugin.SoftwareNameVersioned;

            MCGalaxy.Events.ServerEvents.OnSendingHeartbeatEvent.Register(OnHeartbeat, MCGalaxy.Priority.High);
        }

        public static void Unload()
        {
            MCGalaxy.Events.ServerEvents.OnSendingHeartbeatEvent.Unregister(OnHeartbeat);
            
            MCGalaxy.Server.SoftwareName = oldsoftwarename;
        }

        static void OnHeartbeat(Heartbeat service, ref string name)
        {
            name += DesertCubePlugin.Config.ServerNameSuffix.Replace("%d", Journey.RemainingDistanceKilometers.ToString()).Replace("%p", Journey.DestinationName);
            if (Christmas.IsChristmasMonth())
                name = name.Replace("Desert Bus", "Snow Bus");
        }
    }
}