using MCGalaxy.Network;

namespace DesertCube.Modules.Server
{
    internal class Name
    {
        static string oldsoftwarename = "";
        public static void Load()
        {
            string oldsoftwarename = MCGalaxy.Server.SoftwareNameVersioned;
            MCGalaxy.Server.SoftwareNameVersioned = "%eDesert Bus %b0.1";
            MCGalaxy.Events.ServerEvents.OnSendingHeartbeatEvent.Register(OnHeartbeat, MCGalaxy.Priority.High);
        }

        public static void Unload()
        {
            MCGalaxy.Server.SoftwareNameVersioned = oldsoftwarename;
            MCGalaxy.Events.ServerEvents.OnSendingHeartbeatEvent.Unregister(OnHeartbeat);
        }

        static void OnHeartbeat(Heartbeat service, ref string name)
        {
            name += DesertCubePlugin.Config.ServerNameSuffix.Replace("%d",  ((int)(DesertCubePlugin.RemainingDistance/1000)).ToString());
            //MCGalaxy.Player.Console.Message(name);
        }
    }
}
