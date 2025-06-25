using System.IO;

namespace DesertCube
{
    public class DesertConfig
    {
        public string BusLevel { get; set; } = "desertbus";
        public float BusMaxSpeed { get; set; } = 20f; // meters per second
        public float BusAcceleration { get; set; } = 3f;  // meters per second
        public float BusDecceleration { get; set; } = 1f; // meters per second

        public int DestinationDistance { get; set; } = 580000; // Meters

        public string ServerNameSuffix { get; set; } = " | %dkm left until %p!!!";

        public string DestinationName { get; set; } = "Vegas";

        public string OriginName { get; set; } = "Tucson, Arizona";

        public static string SaveFolder { get { return Directory.GetCurrentDirectory()  + "/plugins/DesertBus"; } }
        static string SaveFile { get { return $"{SaveFolder}/config.txt"; } }
        
        public static DesertConfig Load()
        {
            DesertConfig config = new DesertConfig();
            if (!File.Exists(SaveFile))
                CreateConfig();
            string[] configs = File.ReadAllLines(SaveFile);
            
            config.BusLevel = configs[0].Split('=')[1].Trim();
            config.BusMaxSpeed = float.Parse(configs[1].Split('=')[1].Trim());
            config.BusAcceleration = float.Parse(configs[2].Split('=')[1].Trim());
            config.BusDecceleration = float.Parse(configs[3].Split('=')[1].Trim());
            config.DestinationDistance = int.Parse(configs[4].Split('=')[1].Trim());
            config.ServerNameSuffix = configs[5].Split('=')[1].TrimEnd();
            config.DestinationName = configs[6].Split('=')[1].TrimEnd();
            config.OriginName = configs[7].Split('=')[1].TrimEnd();
            return config;
        }

        public static void Save(DesertConfig config)
        {
            File.WriteAllText(SaveFile,
            $"BusLevel={config.BusLevel}\nBusMaxSpeed={config.BusMaxSpeed}\nBusAcceleration={config.BusAcceleration}\nBusDecceleration={config.BusDecceleration}\nDestinationDistance={config.DestinationDistance}\nServerNameSuffix={config.ServerNameSuffix}\nDestinationName={config.DestinationName}\nOriginName={config.OriginName}");
        }
        public static void CreateConfig()
        {
            if (!Directory.Exists(SaveFolder))
                Directory.CreateDirectory(SaveFolder);

            Save(new DesertConfig());

        }

    }
}
