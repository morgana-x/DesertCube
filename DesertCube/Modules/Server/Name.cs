using MCGalaxy.Network;

namespace DesertCube.Modules.Server
{
    internal class Name
    {
        static string oldsoftwarename = "";
        public static void Load()
        {
            oldsoftwarename = MCGalaxy.Server.SoftwareNameVersioned;
            MCGalaxy.Server.SoftwareName = DesertCubePlugin.SoftwareNameVersioned;
            MCGalaxy.Events.ServerEvents.OnSendingHeartbeatEvent.Register(OnHeartbeat, MCGalaxy.Priority.High);
        }

        public static void Unload()
        {
            MCGalaxy.Server.SoftwareName = oldsoftwarename;
            MCGalaxy.Events.ServerEvents.OnSendingHeartbeatEvent.Unregister(OnHeartbeat);
        }

        static void OnHeartbeat(Heartbeat service, ref string name)
        {
            name += DesertCubePlugin.Config.ServerNameSuffix.Replace("%d", Journey.RemainingDistanceKilometers.ToString()).Replace("%p", Journey.DestinationName);
        }
    }
}
