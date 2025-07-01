using MCGalaxy.Tasks;

namespace DesertCube.Modules.Event.Events
{
    internal class Sandstorm : EventBase
    {
        public override void Start()
        {
            DesertCubePlugin.Bus.Broadcast("%eThere's a %csandstorm%e!");
            DesertCubePlugin.Bus.Broadcast("%cSlow down or we'll crash!!!");
        }

        public override void End()
        {
            DesertCubePlugin.Bus.Broadcast("%eThe sandstorm is over!");
        }

        public override void Run()
        {

        }
    }
}
