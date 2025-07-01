using DesertCube.Modules.Desert;
using System;
using System.Threading;

namespace DesertCube.Modules.Event.Events
{
    internal class Sandstorm : EventBase
    {
        public override string Name => "sandstorm";
        public override void Start()
        {
            DesertCubePlugin.Bus.Broadcast("%eThere's a %csandstorm%e!");
            DesertCubePlugin.Bus.Broadcast("%cSlow down or we'll crash!!!");
            Weather.SetFog(16);
            DayNight.OverrideFog = true;
            // Update fog colour
            DayNight.SendEnv(DesertCubePlugin.Bus.Level, DayNight.CurrentEnv); 
        }

        public override void End()
        {
            DesertCubePlugin.Bus.Broadcast("%eThe sandstorm is over!");
            Weather.SetFog();
            DayNight.OverrideFog = false;
            // Update fog colour
            DayNight.SendEnv(DesertCubePlugin.Bus.Level, DayNight.CurrentEnv);
        }

        public override void Run()
        {
            var sandstormover = DateTime.Now.AddSeconds(45);
            var busslowdowndeadline = DateTime.Now.AddSeconds(25);
            bool busstalled = false;
            while (DateTime.Now < sandstormover)
            {
                if (!busstalled && DateTime.Now > busslowdowndeadline && 
                    DesertCubePlugin.Bus.BusSpeed > DesertCubePlugin.Config.BusMaxSpeed * 0.8f)
                {
                    busstalled = true;
                    DesertCubePlugin.Bus.Broadcast("%cWe didn't slow in time!");
                    DesertCubePlugin.Bus.Broadcast("%cThe driver is getting a pay reduction grrr");

                    DesertCubePlugin.Bus.SetSpeed(0);
                    Stop.ArriveBusStop(Stop.RandomStop(), 300, true);
                }
                Thread.Sleep(1000);
            }

        }
    }
}
