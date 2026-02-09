using MCGalaxy;
using MCGalaxy.Config;
using System;
using System.IO;

namespace DesertCube
{

    public sealed class DesertConfig
    {
        [ConfigString("bus-level", "Bus", "desertbus", false)]
        public string BusLevel  = "desertbus";

        [ConfigFloat("max-speed", "Bus", 20f)]
        public float BusMaxSpeed  = 20f; // meters per second

        [ConfigFloat("acceleration-rate", "Bus", 3f)]
        public float BusAcceleration  = 3f;  // meters per second

        [ConfigFloat("decceleration-rate", "Bus", 1f)]
        public float BusDecceleration  = 1f; // meters per second

        [ConfigInt("max-objects", "Desert", 10)]
        public int MaxObjects = 10;

        [ConfigInt("distance", "Journey", 580000, min:1)]
        public int DestinationDistance  = 580000; // Meters

        [ConfigString("destination-name", "Journey", "Vegas", false)]
        public string DestinationName  = "Vegas";

        [ConfigString("origin-name", "Journey", "Vegas", false)]
        public string OriginName  = "Tucson, Arizona";

        [ConfigString("server-name-suffix", "Server", " | %dkm left until %p!!!")]
        public string ServerNameSuffix  = " | %dkm left until %p!!!";

        [ConfigString("asset-url-prefix", "Server", "https://garbage.loan/f/morgana/")]
        public string AssetUrlPrefix  = "https://garbage.loan/f/morgana/";

        public static string SaveFolder { get { return DesertConfigManager.SaveFolder; } }
    }

    public class DesertConfigManager
    {
        public static string SaveFolder { get { return Directory.GetCurrentDirectory() + "/plugins/DesertBus"; } }
        static string SaveFile { get { return $"{SaveFolder}/config.txt"; } }


        internal static ConfigElement[] desertConfig;

        public static void Load()
        {
            desertConfig = ConfigElement.GetAll(typeof(DesertConfig));
    
            if (!File.Exists(SaveFile) || !ConfigElement.ParseFile(desertConfig, SaveFile, DesertCubePlugin.Config))
            {
                CreateConfig();
                return;
            }
        }
        static readonly object saveLock = new object();
        public static void Save(DesertConfig config)
        {
            try
            {
                lock (saveLock)
                {
                    using (StreamWriter w = FileIO.CreateGuarded(SaveFile))
                        ConfigElement.Serialise(desertConfig, w, config);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error saving " + SaveFile, ex);
            }
        }

        public static void CreateConfig()
        {
            if (!Directory.Exists(SaveFolder))
                Directory.CreateDirectory(SaveFolder);

            DesertCubePlugin.Config = new DesertConfig();
            Save(DesertCubePlugin.Config);
        }
    }
}
