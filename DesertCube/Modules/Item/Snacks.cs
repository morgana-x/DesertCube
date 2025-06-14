using MCGalaxy.Blocks;
using MCGalaxy;
using MCGalaxy.Events.PlayerEvents;
using System.Collections.Generic;

namespace DesertCube.Modules.Item
{
    public class Snacks
    {
        static Dictionary<ushort, byte> ParticleEffects = new Dictionary<ushort, byte>();
        static Dictionary<byte, FoodEatEffect> ParticleDefs = new Dictionary<byte, FoodEatEffect>();
        public static void Load()
        {
            AddSnack(66, "Crossiant", 89, 10);
            AddSnack(67, "Cookie", 90, 10);
            AddSnack(68, "Bread", 91, 10);
            AddSnack(69, "Chips", 92, 10);
            AddSnack(70, "Pizza", 93, 10);
            AddSnack(71, "Chocolate", 94, 10);
            AddSnack(72, "Hamburger", 95, 10);
            AddSnack(73, "Orange juice", 109, 11);
            AddSnack(74, "Beer", 110, 12);
            AddSnack(75, "Water", 111, 13);
            AddSnack(76, "Coin", 108, 14);
            AddSnack(77, "$10 Note", 107, 15);

            AddEffect(10, 200, 141, 77, 2); // Food
            AddEffect(11, 249, 129, 0, 3); // OJ
            AddEffect(12, 255, 180, 98, 3); // Beer
            AddEffect(13, 98, 154, 255, 3); // Water
            AddEffect(14, 255, 230, 0, 3); // Gold
            AddEffect(15, 0, 125, 1, 1); // Cash

            MCGalaxy.Events.PlayerEvents.OnPlayerClickEvent.Register(OnPlayerClick, Priority.Normal);
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Register(EventPlayerSentMap, Priority.Normal);
            foreach (var p in PlayerInfo.Online.Items)
                DefineEffects(p);
        }
        public static void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerClickEvent.Unregister(OnPlayerClick);
            MCGalaxy.Events.PlayerEvents.OnSentMapEvent.Unregister(EventPlayerSentMap);
        }

        static void EventPlayerSentMap(MCGalaxy.Player p, Level prevLevel, Level level)
        {
            DefineEffects(p);
        }

        static void OnPlayerClick(MCGalaxy.Player p, MouseButton btn, MouseAction action, ushort yaw, ushort pitch, byte entityID, ushort x, ushort y, ushort z, TargetBlockFace face)
        {
            if (btn != MouseButton.Right) return;
            var block = Block.ToRaw(p.ClientHeldBlock);
            if (!ParticleEffects.ContainsKey(block)) return;

            foreach (var pl in PlayerInfo.Online.Items)
            {
                if (pl.level != p.level) continue;
                if (!pl.Session.hasCpe) continue;
                pl.Send(MCGalaxy.Network.Packet.SpawnEffect(ParticleEffects[block], p.Pos.BlockX, p.Pos.BlockY, p.Pos.BlockZ, p.Pos.BlockX, p.Pos.BlockY, p.Pos.BlockZ));
            }
            
        }

        static void DefineEffects(MCGalaxy.Player p)
        {
            if (!p.Session.hasCpe) return;
            foreach(var value in ParticleDefs.Values)
                GoodlyEffects.DefineEffect(p, value);
        }
        public static void AddSnack(ushort Id, string Name, ushort Texture, byte Particle)
        {
            AddBlockItem(Id, Name, Texture);

            if (!ParticleEffects.ContainsKey(Id))
                ParticleEffects.Add(Id, Particle);
            ParticleEffects[Id] = Particle;
        }
        public static void AddEffect(byte Id, byte r, byte g, byte b, int gravity)
        {
            var effect = new FoodEatEffect(Id, r, g, b, gravity);

            if (!ParticleDefs.ContainsKey(Id))
                ParticleDefs.Add(Id, effect);
            else
                ParticleDefs[Id] = effect;

        }
        public static void AddBlockItem(ushort Id, string Name, ushort Texture, bool admin = false)
        {
            BlockDefinition def = new BlockDefinition();
            def.RawID = Id; def.Name = Name;
            def.Speed = 1; def.CollideType = 0;
            def.TopTex = Texture; def.BottomTex = Texture;

            def.BlocksLight = false; def.WalkSound = 1;
            def.FullBright = false; def.Shape = 0;
            def.BlockDraw = 2; def.FallBack = 5;

            def.FogDensity = 0;
            def.FogR = 0; def.FogG = 0; def.FogB = 0;
            def.MinX = 0; def.MinY = 0; def.MinZ = 0;
            def.MaxX = 0; def.MaxY = 0; def.MaxZ = 0;

            def.LeftTex = Texture; def.RightTex = Texture;
            def.FrontTex = Texture; def.BackTex = Texture;
            def.InventoryOrder = -1;
            ushort block = Id;
            if (admin)
            {
                BlockPerms perms = BlockPerms.GetPlace((ushort)(block + 256));
                perms.MinRank = LevelPermission.Guest; // LevelPermission.Nobody
            }
            BlockPerms.Save();
            BlockPerms.ApplyChanges();

            if (!Block.IsPhysicsType(block))
            {
                BlockPerms.ResendAllBlockPermissions();
            }
            BlockDefinition.Add(def, BlockDefinition.GlobalDefs, null);
        }

        
        public class FoodEatEffect : GoodlyEffects.EffectConfig
        {
            public FoodEatEffect(byte id = 10, byte r = 255, byte g = 255, byte b = 255, int grav = 2, byte fCount = 1)
            {
                ID = id;
                pixelU1 = 0;
                pixelV1 = 0;
                pixelU2 = 10;
                pixelV2 = 10;
                //tint uses RGB color values to determine what color to tint the particle. Here we've set it to be tinted pink, since the original texture is white.
                tintRed = r;
                tintGreen = g;
                tintBlue = b;
                //#frameCount determines how many frames of animation will be played over the particle's lifespan (faster life, faster animation).
                //#Frames are always the same size as each other and are stored left-to-right in particles.png.
                frameCount = fCount;
                //#particleCount is how many hearts are spawned per-effect.
                particleCount = 14;
                //#pixelSize is how large in "pixel" units the particle is. 8 is the size of a player's head. You are allowed to be as precise as half a pixel, therefore the smallest possible size is 0.5.
                pixelSize = 7;
                //#sizeVariation is how much the particle can randomly vary in size. 1 means 100% variation, 0 means 0% variation.
                sizeVariation = 0.5f;
                //#spread allows the particles to spawn randomly around the point they were told to spawn at. A spread of "0.5" is equal to the width of a full block (because the spread goes both ways).
                spread = 1f;
                //#speed is how fast this particles moves away from the origin.
                speed = 0.25f;
                //#gravity adds to the up/down speed of the particle over time. -1 here means the heart will float up
                gravity = grav;
                //#baseLifetime is the time (in seconds) this particle is allowed to live at most (colliding with blocks may kill it sooner).
                baseLifetime = 2f;
                //#lifetimeVariation is how much the particle's lifespan can randomly vary. 1 means 100% variation, 0 means 0% variation.
                lifetimeVariation = 0.5f;
                //#expireUponTouchingGround means particle dies if it hits solid floor
                expireUponTouchingGround = false;
                //#collides determine what blocks count as "solid".
                collidesSolid = true;
                collidesLiquid = false;
                collidesLeaves = false;
                //#fullBright means it will always have its original brightness, even in dark environments.
                fullBright = false;
            }
        }

    }
}
