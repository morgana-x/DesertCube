using MCGalaxy.Events.ServerEvents;
using MCGalaxy;
using System;

namespace DesertCube.Modules.Chat
{
    public class EchoChamber : DesertModule
    {
        public override void Load()
        {
            OnChatEvent.Register(HandleChatEvent, Priority.Low);
        }

        public override void Unload()
        {
            OnChatEvent.Unregister(HandleChatEvent);
        }

        static string[] badwords = new string[]{
            "ojhhfs",
            "o2hhfs",
            "ojhmfu",
            "o2hmfu",
            "o2hh4s",
            "o2hm4u",
            "ifjmijumfs",
            "i4jmi2um4s",
            "ifjmi2um4s",
            "usbooz",
        };
        static string obfuscate(string w)
        {
            string l = w.ToLower();
            string res = string.Empty;
            for (int i = 0; i < w.Length; i++)
                res += (char)((byte)l[i] + 1);
            return res;
        }

        void HandleChatEvent(ChatScope scope, MCGalaxy.Player source, ref string msg, object arg, ref ChatMessageFilter filter, bool relay = false)
        {
            bool filterEcho(MCGalaxy.Player pl, object args) => pl == source;

            string obf = obfuscate(msg.Replace(" ", "").Trim());
            foreach (var w in badwords)
                if (obf.Contains(w))
                {
                    // Let them have the illusion of being a succesful edgelord / rage baiter (and then be confused as to why no one is mad)
                    filter = filterEcho;
                    relay = false;
                    source.Extras["SlurAmount"] = source.Extras.GetInt("SlurAmount") + 1;
                    break;
                }
        }

        DateTime nextAlert = DateTime.Now;
        public override void Tick(float curTime)
        {
            if (DateTime.Now < nextAlert) return;
            nextAlert = DateTime.Now.AddSeconds(10);

            foreach (var p in MCGalaxy.PlayerInfo.Online.Items)
            {
                if (!p.Extras.Contains("SlurAmount"))
                    continue;
                int numSlurs = p.Extras.GetInt("SlurAmount");
                if (numSlurs == 0)
                    continue;
                MCGalaxy.Chat.MessageOps($"&c{p.name} &etried saying &cslurs &ein &d{numSlurs}&e msg{(numSlurs > 1 ? "s" : "")}!");
                p.Extras["SlurAmount"] = 0;
            }
        }
    }
}
