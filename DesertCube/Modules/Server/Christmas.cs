using System;

namespace DesertCube.Modules.Server
{
    public class Christmas : DesertModule
    {
        public static bool IsChristmasMonth()
        {
            return DateTime.Now.Month == 12;
        }

        public override void Load()
        {
           
        }
        public override void Unload()
        {
            
        }
    }
}
