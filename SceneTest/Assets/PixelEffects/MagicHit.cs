using DiligentEngine.RT.Sprites;
using Engine;
using Engine.Platform;
using System.Collections.Generic;

namespace SceneTest.Assets.PixelEffects
{
    class MagicHit : ISpriteAsset
    {
        public SpriteMaterialDescription CreateMaterial()
        {
            return new SpriteMaterialDescription
            (
                colorMap: "FreePixelEffectsPack/5_magickahit_spritesheet.png",
                materials: new HashSet<SpriteMaterialTextureItem>()
            );
        }

        public Sprite CreateSprite()
        {
            return new Sprite(new Dictionary<string, SpriteAnimation>()
            {
                 { "default", new SpriteAnimation((int)(0.1f * Clock.SecondsToMicro), SpriteBuilder.CreateAnimatedSprite(100, 100, 7, 41)) },
            })
            { BaseScale = new Vector3(1, 1, 1) };
        }
    }
}
