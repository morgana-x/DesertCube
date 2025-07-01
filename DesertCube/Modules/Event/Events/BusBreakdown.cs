using MCGalaxy.Tasks;

namespace DesertCube.Modules.Event.Events
{
    internal class BusBreakdown : EventBase
    {
        public override void Start()
        {
            DesertCubePlugin.Bus.Broadcast("%cThe bus has broken down!");
            DesertCubePlugin.Bus.Broadcast("%cSomeone better %efix%c it!");
        }

        public override void End()
        {
            DesertCubePlugin.Bus.Broadcast("%aThe bus has been fixed!");
        }

        public override void Run()
        {
            
        }
    }
}
