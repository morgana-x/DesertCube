using MCGalaxy.Events.ServerEvents;
using MCGalaxy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesertCube.Modules.Chat
{
    public class EchoChamber : DesertModule
    {
        static Dictionary<string,string[]> badwords = new Dictionary<string, string[]>()
        {
            ["slurs"] = new string[]{
                "ojhhfs",
                "ojh4s",
                "o2h4s",
                "o2hfs",
                "o2hhfs",
                "ojhmfu",
                "oj:mfu",
                "o2:mfu",
                "o2:m4u",
                "o2hmfu",
                "ojhm4u",
                "o2hh4s",
                "o2::4s",
                "oj::fs",
                "oj::4s",
                "o2hm4u",
                "oj::b",
                "oj::A",
                "o2::A",
                "o2::b",
                "ojhhb",
                "ojhhA",
                "o2hhb",
                "o2hhA",
                "ibufoj:fs",
                "ibufojhfs",
                "ibufo2h4s",
                "ibufojh4s",
                "ibufo2hfs",
                "ibufcmbdlqfpqmf",
                "ibufcmAdlqfpqmf",
                "ibufcmAdlq41qm4",
                "gvdlcmbdlqfpqmf",
                "gvdlcmAdlqfpqmf",
                "cmbdlcmbdlhvz",
                "cmbdlojokb",
                "cmbdlojh",
                "gvdljohojokb",
                "jibufojokbt",
                "tuvqjeojokb",
            },
            ["possiblyaccidentalracism"] = new string[]{
                "cmbdlojhiu",
            },
            ["nsfw"] = new string[]{
                "mpwfqpso",
                "xbouupxbudiqpso",
                "mpwfq1so",
                "m1wfq1so",
                "jmjlfqpso",
                "zpvmjlfqpso",
                "qpsophsbqiz",
                "q1sophsAqiz",
                "q1so1hsAqiz",
                "jmjlfifoubj",
                "jmpwfifoubj",
                "jmpwfujuzt",
                "xbudiifoubj",
                "nbtuvscbuf",
                "nAtuvscbuf",
                "nAbtuvscbu4",
                "nAbtuvscAu4",
                "nAb6uvscAuf",
                "nbtuvscbujoh",
                "nbtufscbujoh",
                "nbtuvscbu2oh",
                "nAtuvscAujoh",
                "nbtufscbuf",
                "nbtufscbu4",
                "nAtufscbu4",
                "qpsoivc",
                "q1soivc",
                "q1so",
                "ifoubjibwfo",
                "ibntufs/yyy",
                "esjqqjohxjuidvn",
                "esjqqjohdvn",
                "dvnnjoh",
                "kfsljohpgg",
                "mpwfkfsljohpgg",
                "mjlfkfsljohpgg",
                "hpoobkfslpgg",
                "k4slpgg",
                "k4sl1gg",
                "k4sljoh",
                "k4sl2oh",
                "k4sl2o:",
                "cjhdpdl",
                "cjhqfojt",
            },
            ["transphobic stuff"] = new string[]{
                "gbhhpu",
                "gAhh1u",
                "usboozgbh",
                "usbooz",
                "gAh",
            },
            ["nazi things"] = new string[]{
                "ijumfsxbtsjhiu",
                "i2um4s",
                "ifjmijumfs",
                "i4jmi2um4s",
                "ifjmi2um4s",
                "ifjmi2umfs",
                "ifjmijum4s",
                "hbtuifkfx",
                "hbtuifk4x",
            },
            ["antisemetic stuff"] = new string[]{
                "tuvqjekfx",
                "tuvqjek4x",
                "jibufkfx",
                "gvdlkfx",
                "gvdlk4x",
                "jibufk43",
            },
            ["islamophobic stuff"] = new string[]{
                "tuvqjebsbc",
                "gvdlhb{b",
            },
            ["requesting social accounts"] = new string[]{
                "nzjotubjt",
                "nzjotubhsbnjt",
                "epzpvibwfjotubhsbn",
                "vibwfjhpstpnfuijoh",
                "epftbozpofibwfjotubhsbn",
                "jotuAhsAn",
                "2otuAhsAn",
                "nztobqdibujt",
                "epzpvibwfxibutbqq",
                "xibutzpvsejtdpsebddpvou",
            },
        };

        public override void Load()
        {
            OnChatEvent.Register(HandleChatEvent, Priority.High);
            OnChatFromEvent.Register(HandleChatEvent, Priority.High);
        }

        public override void Unload()
        {
            OnChatEvent.Unregister(HandleChatEvent);
            OnChatFromEvent.Unregister(HandleChatEvent);
        }

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
            string obf = obfuscate(msg.Replace(" ", "").Trim());

            foreach (var c in badwords)
                foreach (var w in c.Value)
                    if (obf.Contains(w))
                    {
                        bool filterEcho(MCGalaxy.Player pl, object args) => (pl == source || (c.Key != "requesting social accounts" && pl.Extras.GetBoolean("IsRacist")));
                        // Let them have the illusion of being a succesful edgelord / rage baiter (and then be confused as to why no one is mad)
                        relay = false;

                        string recentCat = source.Extras.GetString("SlurCategory","");

                        source.Extras["SlurCategory"] = recentCat + (!recentCat.Contains(c.Key) ? c.Key + ";" : "");
                        source.Extras["SlurAmount"] = source.Extras.GetInt("SlurAmount") + 1;

                        if (c.Key != "requesting social accounts")
                            source.Extras["IsRacist"] = true;
                        
                        filter = filterEcho;
                        break;
                    }
        }

        DateTime nextAlert = DateTime.Now;
        public override void Tick(float deltaTime)
        {
            if (DateTime.Now < nextAlert) return;
            nextAlert = DateTime.Now.AddSeconds(10);

            foreach (var p in MCGalaxy.PlayerInfo.Online.Items)
            {
                int numSlurs = p.Extras.GetInt("SlurAmount");
                if (numSlurs == 0) continue;

                string[] slurcats = p.Extras.GetString("SlurCategory","slurs;").TrimEnd(';').Split(';');

                MCGalaxy.Chat.MessageOps($"&c{p.name} &etried saying &c{slurcats.First()} {(slurcats.Length == 2 ? $"+ {slurcats[1]} " : slurcats.Length > 2 ? "+ more! " : "")}&ein &d{numSlurs}&e msg{(numSlurs > 1 ? "s" : "")}!");
                
                p.Extras["SlurAmount"] = 0;
                p.Extras["SlurCategory"] = "";
            }
        }
    }
}
