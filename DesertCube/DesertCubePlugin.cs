using DesertCube.Modules.Bus;
using MCGalaxy;

namespace DesertCube
{
    public class DesertCubePlugin : Plugin
    {
        public override string name { get { return "DesertCube"; } }
        public override string MCGalaxy_Version { get { return "1.9.5.3"; } }
        public override int build { get { return 3; } }
        public override string welcome { get { return "DesertCube loaded!"; } }
        public override string creator { get { return "morgana"; } }
        public override bool LoadAtStartup { get { return true; } }

        public static string Version = "0.3";

        public static string SoftwareName = "&eDesert Bus";
        public static string SoftwareNameVersioned { get {  return $"{SoftwareName} &b{Version}&f"; } }

        public static Bus Bus;

        public static DesertConfig Config;

    
        public override void Load(bool startup)
        {
            Config = DesertConfig.Load();


            DesertModule.LoadModules();
            DesertCommands.LoadCommands();

            Bus = (Bus)DesertModule.GetInstance(typeof(Bus));
        }
        public override void Unload(bool shutdown)
        {
            DesertModule.UnloadModules();
            DesertCommands.UnloadCommands();

            Bus.Unload();
        }
    }
}
