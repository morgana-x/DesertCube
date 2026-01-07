using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertCube.Modules.Server
{
    public class Christmas
    {
        public static bool IsChristmasMonth()
        {
            return DateTime.Now.Month == 12;
        }

        public static void Load()
        {
           
        }
        public static void Unload()
        {
            
        }
    }
}
