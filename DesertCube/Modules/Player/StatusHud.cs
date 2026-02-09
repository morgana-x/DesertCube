using DesertCube.Modules.Server;
using MCGalaxy;
using MCGalaxy.Tasks;
using System;

namespace DesertCube.Modules.Player
{
    internal class StatusHud : DesertModule
    {
        public override void Load()
        {
        }
        public override void Unload()
        {
        }

        static string GetStatus1Message()
        {
            return $"%3{(DesertCubePlugin.Bus.BusSpeed * 3.6f).ToString("0")}%7km/h %3{Journey.RemainingDistanceKilometers}%7km";
        }
        static string GetStatus2Message()
        {
            return $"&3{Time.TimeHour.ToString("D2")}&7:&3{Time.TimeMinute.ToString("D2")}";
        }

        static bool ShouldSeeMessage(MCGalaxy.Player player)
        {
            if (player.Pos.BlockX != 71) return false;
            if (!DesertCubePlugin.Bus.InsideBus(player)) return false;
            return true;
        }
        static void EraseStatus(MCGalaxy.Player player)
        {
            UpdateStatus(player, CpeMessageType.Status1, "");
            UpdateStatus(player, CpeMessageType.Status2, "");
        }

        static void UpdateStatus(MCGalaxy.Player player, CpeMessageType type, string message)
        {
            var enumName = Enum.GetName(typeof(CpeMessageType), type);
            if (player.Extras.GetString(enumName) == message) return;
            player.SendCpeMessage(type, message);
            player.Extras[enumName] = message;
        }
        static void BroadcastStatus()
        {
            string message = GetStatus1Message();
            string message2 = GetStatus2Message();
            foreach (var player in DesertCubePlugin.Bus.GetPlayers())
            {
                if (!player.Session.hasCpe) continue;

                if (!ShouldSeeMessage(player))
                {
                    EraseStatus(player);
                    continue; 
                }
                UpdateStatus(player, CpeMessageType.Status1, message);
                UpdateStatus(player, CpeMessageType.Status2, message2);
            }
        }

        DateTime nextStatus = DateTime.Now;
        public override void Tick(float deltaTime)
        {
            if (DateTime.Now < nextStatus) return;
            nextStatus = DateTime.Now.AddSeconds(1);
            BroadcastStatus();
        }
    }
}
