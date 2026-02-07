using DesertCube.Modules.Player;
using MCGalaxy.DB;
using MCGalaxy;

namespace DesertCube.Modules.Shop.Items
{
    public class LogInMessageItem : ShopItem
    {
        public override string Name => "LoginMessage";
        public LogInMessageItem()
        {
            Price = 5;
        }
        public override void Purchase(MCGalaxy.Player p, string message = "")
        {
            if (message.Trim() == "")
            {
                PlayerDB.SetLoginMessage(p.name, "");
                p.Message("&WReset your login message for free!");
                return;
            }
            if (!CanPurchase(p))
                return;
            if (message == PlayerDB.GetLoginMessage(p.name))
            {
                p.Message("&WAlready have that login message!");
                return;
            }
            if (message.Length > 64)
            {
                p.Message("&WMessage can not be over 64 characters!");
                return;
            }
            if (PlayerOperations.SetLoginMessage(p, p.name, message))
                Stats.AddPoints(p.name, -Price);

        }
    }
    public class LogOutMessageItem : ShopItem
    {
        public override string Name => "LogoutMessage";
        public LogOutMessageItem()
        {
            Price = 5;
        }
        public override void Purchase(MCGalaxy.Player p, string message = "")
        {
            if (message.Trim() == "")
            {
                PlayerDB.SetLogoutMessage(p.name, "");
                p.Message("&WReset your logout message for free!");
                return;
            }
            if (!CanPurchase(p))
                return;
            if (message == PlayerDB.GetLogoutMessage(p.name))
            {
                p.Message("&WAlready have that logout message!");
                return;
            }
            if (message.Length > 64)
            {
                p.Message("&WMessage can not be over 64 characters!");
                return;
            }
            if (PlayerOperations.SetLogoutMessage(p, p.name, message))
                Stats.AddPoints(p.name, -Price);

        }
    }

    /*
    public class ColourItem : ShopItem
    {
        public override string Name => "Colour";
        public ColourItem ()
        {
            Price = 20;
        }
        public override void Purchase(MCGalaxy.Player p, string args = "")
        {
            if (args.Length == 0)
            {
                Help(p);
                return;
            }

            string text = Matcher.FindColor(p, args);
            if (text == null)
                return;

            string text2 = Colors.Name(text);
            if (!CanPurchase(p))
                return;
            
            if (text == p.color)
            {
                p.Message("&WYour color is already " + text + text2);
            }
            else if (PlayerOperations.SetColor(p, p.name, text2))
            {
                Stats.AddPoints(p.name, -Price);
            }
            
        }
    }*/

}
