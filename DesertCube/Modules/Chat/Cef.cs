using MCGalaxy.Events.ServerEvents;
using MCGalaxy;

namespace DesertCube.Modules.Chat
{
    internal class Cef
    {
        public static void Load()
        {
            OnChatEvent.Register(HandleChatEvent, Priority.Low);
        }

        public static void Unload()
        {
            OnChatEvent.Unregister(HandleChatEvent);
        }
        static bool filterCef(MCGalaxy.Player pl, object arg)
        {
            if (!pl.Session.ClientName().CaselessContains("cef"))
                return false;
            return true;
        }
        // Make sure non cef players dont see cef players' spammed commands in chat
        static void HandleChatEvent(ChatScope scope, MCGalaxy.Player source, ref string msg, object arg, ref ChatMessageFilter filter, bool relay = false)
        {
            if (msg.CaselessContains("cef ") && !msg.StartsWith(" ") && source.Session.ClientName().CaselessContains("cef"))
                filter = filterCef;
        }
    }
}
