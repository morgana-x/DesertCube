﻿using DesertCube.Modules.Server;
using MCGalaxy;
using MCGalaxy.Commands;
using System;

namespace DesertCube.Commands
{
    public class BusDistance : Command2
    {
        public override string name => "busdistance";

        public override string type => "map";

        public override CommandAlias[] Aliases => new CommandAlias[] { new CommandAlias("distance") };

        public override LevelPermission defaultRank => LevelPermission.Guest;
        public override void Help(Player p)
        {
            p.Message("/busdistance - Get the total distance travelled on this trip");
            p.Message("/distance - Get the total distance travelled on this trip");
        }

        public override void Use(Player p, string message)
        {
            string[] args = message.Split(' ');

            if (p.Rank >= LevelPermission.Owner && args.Length > 0 && args[0].Trim() != "") // Debug purposes
            {
                if (!int.TryParse(args[0], out var newdist))
                {
                    p.Message($"\"{args[0]}\" isn't a valid distance integer!");
                    return;
                }
                Journey.TotalDistance = newdist;
                p.Message($"%eSet the total distance to %d{newdist}%e!");
                return;
            }

            var diststring = Journey.TotalDistance < 1000 ? $"{(int)Journey.TotalDistance}%e meters" : $"{(int)Journey.TotalDistanceKilometers}%ekm";
            p.Message($"%eThe Bus has travelled %d{diststring}%e!");
            p.Message($"%eThere's %d{Journey.RemainingDistanceKilometers}%ekm until %6{Journey.DestinationName}%e!");
        }
    }
}
