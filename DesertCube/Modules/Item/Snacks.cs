using MCGalaxy.Blocks;
using MCGalaxy;
using MCGalaxy.Events.PlayerEvents;
using System.Collections.Generic;
using DesertCube.Modules.Player;

namespace DesertCube.Modules.Item
{
    public class Snacks
    {
        static Dictionary<ushort, byte> ParticleEffects = new Dictionary<ushort, byte>();
        static Dictionary<ushort, Sound.SoundDefinition> SoundEffects = new Dictionary<ushort, Sound.SoundDefinition>();
        public static void Load()
        {
            AddSnack(66, "Crossiant", 89, 18);
            AddSnack(67, "Cookie", 90, 10);
            AddSnack(68, "Bread", 91, 18);
            AddSnack(69, "Chips", 92, 16);
            AddSnack(70, "Pizza", 93, 10);
            AddSnack(71, "Chocolate", 94, 17);
            AddSnack(72, "Hamburger", 95, 10);
            AddSnack(73, "Orange juice", 109, 11, LiquidSound);
            AddSnack(74, "Beer", 110, 12, LiquidSound);
            AddSnack(75, "Water", 111, 13, LiquidSound);
            AddSnack(76, "Coin", 108, 14, CoinSound);
            AddSnack(77, "$10 Note", 107, 15);
            AddSnack(78, "Pasta", 106, 10); // for the italians :D

            Modules.Player.Effect.AddEffect(10, 200, 141, 77, 2); // Food
            Modules.Player.Effect.AddEffect(11, 249, 129, 0, 3); // OJ
            Modules.Player.Effect.AddEffect(12, 255, 180, 98, 3); // Beer
            Modules.Player.Effect.AddEffect(13, 98, 154, 255, 3); // Water
            Modules.Player.Effect.AddEffect(14, 255, 230, 0, 3); // Gold
            Modules.Player.Effect.AddEffect(15, 0, 125, 1, 1); // Cash
            Modules.Player.Effect.AddEffect(16, 255, 248, 117, 1); // Chips
            Modules.Player.Effect.AddEffect(17, 135, 71, 22, 1); // Chocolate
            Modules.Player.Effect.AddEffect(18, 125, 82, 10, 1); // Bread

            MCGalaxy.Events.PlayerEvents.OnPlayerClickEvent.Register(OnPlayerClick, Priority.Normal);
        }
        public static void Unload()
        {
            MCGalaxy.Events.PlayerEvents.OnPlayerClickEvent.Unregister(OnPlayerClick);
        }


        static void OnPlayerClick(MCGalaxy.Player p, MouseButton btn, MouseAction action, ushort yaw, ushort pitch, byte entityID, ushort x, ushort y, ushort z, TargetBlockFace face)
        {
            if (btn != MouseButton.Right) return;
            var block = Block.ToRaw(p.ClientHeldBlock);

            if (ParticleEffects.ContainsKey(block))
                Effect.EmitEffect(p, ParticleEffects[block]);

            if (SoundEffects.ContainsKey(block))
                Sound.EmitBlockSound(p, SoundEffects[block]);
        }

        static Sound.SoundDefinition DefaultSound = new Sound.SoundDefinition(0, SoundType.Sand, 70, 255);

        static Sound.SoundDefinition LiquidSound = new Sound.SoundDefinition(0, SoundType.Snow, 110, 255);

        static Sound.SoundDefinition CoinSound = new Sound.SoundDefinition(0, SoundType.Metal, 180, 255);

        public static void AddSnack(ushort Id, string Name, ushort Texture, byte Particle, Sound.SoundDefinition sound = null)
        {
            AddBlockItem(Id, Name, Texture);

            if (!ParticleEffects.ContainsKey(Id))
                ParticleEffects.Add(Id, Particle);
            ParticleEffects[Id] = Particle;

            if (sound == null) sound = DefaultSound;

            if (!SoundEffects.ContainsKey(Id))
                SoundEffects.Add(Id, sound);
            SoundEffects[Id] = sound;

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
    }
}
