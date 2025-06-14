using MCGalaxy;
using MCGalaxy.Commands;
using System;

namespace DesertCube.Commands
{
    public class ItemGive : Command2
    {
        public override string name => "itemgive";

        public override string type => "fun";

        public override LevelPermission defaultRank => LevelPermission.Owner;
        public override void Help(Player p)
        {
            p.Message("/itemgive player block amount");
        }

        public override void Use(Player p, string message)
        {
            string[] args = message.Split(' ');

            int matches;
            Player who = PlayerInfo.FindMatches(p, args[0], out matches);
            if (who == null)
            {
                p.Message("Couldn't find player " + args[0]);
                return;
            }
            ushort blockId = 0;
            try
            {
                blockId = ushort.Parse(args[1]);
            }
            catch (Exception e)
            {
                if (!CommandParser.GetBlock(p, args[1], out blockId))
                {
                    p.Message("%cBlock %5\"" + args[1] + "\"%c doesn't exist!");
                    return;
                }
                if (blockId > 256)
                    blockId = (ushort)(blockId - 256);
            }
            ushort amount = 1;
            if (args.Length > 2)
            {
                try
                {
                    amount = ushort.Parse(args[2]);
                }
                catch (Exception e)
                {
                    Help(p);
                    return;
                }
            }
          //  Modules.Player.Inventory.GetI
            p.Message("Gave " + who.name + " %5" + Block.GetName(p, blockId > 65 ? (ushort)(blockId + 256) : blockId) + "%a x" + (args.Length > 2 ? args[2].ToString() : "1"));
        }
    }
}
