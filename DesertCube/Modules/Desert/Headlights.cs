using MCGalaxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertCube.Modules.Desert
{
    /*
    public class Headlights : DesertModule
    {
        PlayerBot headlightsModel;
        public bool HeadlightsOn = true;
        int YLevel;
        public override void Load()
        {
        }
        public override void PostLoad()
        {
            if (DesertCubePlugin.Bus == null || DesertCubePlugin.Bus.Level == null)
                return;
            foreach (var b in DesertCubePlugin.Bus.Level.Bots.Items.Where(x => x.name == "headlights"))
                PlayerBot.Remove(b, false);

            YLevel = ((DesertCubePlugin.Bus.Level.Config.GetEnvProp(EnvProp.CloudsLevel) + 1) * 32)+23;
            headlightsModel = new PlayerBot("headlights", DesertCubePlugin.Bus.Level);
            headlightsModel.DisplayName = "";
            headlightsModel.Model = "headlights|32";
            headlightsModel.SkinName = DesertCubePlugin.Config.AssetUrlPrefix + "insect.png";
            headlightsModel.SetInitialPos(new Position(95 * 32, -YLevel, 60 * 32));
            PlayerBot.Add(headlightsModel, false);
        }

        public override void Unload()
        {
            PlayerBot.Remove(headlightsModel, false);
        }

        public override void Tick(float deltaTime)
        {
            int correctYLevel = (HeadlightsOn && DayNight.IsNight) ? YLevel : -YLevel;
            if (headlightsModel.Pos.Y != correctYLevel)
                headlightsModel.Pos = new Position(headlightsModel.Pos.X, correctYLevel, headlightsModel.Pos.Z);
        }
    }*/
}
