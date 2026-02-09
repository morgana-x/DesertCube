using DesertCube.Modules.Server;
using MCGalaxy.Tasks;

namespace DesertCube.Modules.Desert
{
    public class Weather : DesertModule // This whole module is pretty lousy, refactor later, was made for sandstorm event so far
    {

        public static int CurrentFog = int.MaxValue;
        public static bool Changed = false;
        public override void Load()
        {
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Register(OnSentMap, MCGalaxy.Priority.Normal);
        }

        public override void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Unregister(OnSentMap);
        }

        public override void Tick(float deltaTime)
        {
            if (!Changed) return;
            Changed = false;
            foreach (var p in DesertCubePlugin.Bus.GetPlayers())
                SendEnv(p);
        }

        public static void SetFog(int fog=int.MaxValue)
        {
            CurrentFog = fog;
            Changed = true;
        }

        static void SendEnv(MCGalaxy.Player p)
        {
            if (p.level != DesertCubePlugin.Bus.Level) return;
            if (!p.Session.Supports("EnvMapAspect", 2) && !p.Session.Supports("EnvMapAspect", 1)) return;
            p.Send(MCGalaxy.Network.Packet.EnvMapProperty(MCGalaxy.EnvProp.MaxFog, CurrentFog));
            if (Christmas.IsChristmasMonth())
                p.Send(MCGalaxy.Network.Packet.EnvMapProperty(MCGalaxy.EnvProp.Weather, 2));
        }

        static void OnSentMap(MCGalaxy.Player p, MCGalaxy.Level prev, MCGalaxy.Level current)
        {
            if (current == DesertCubePlugin.Bus.Level)
                SendEnv(p);
        }

    }
}
