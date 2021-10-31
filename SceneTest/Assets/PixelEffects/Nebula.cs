using Engine;
using Engine.Platform;
using SceneTest.Resources;
using System.Collections.Generic;

namespace SceneTest.Assets.PixelEffects
{
    class Nebula : ISpriteAsset
    {
        public SpriteMaterialDescription CreateMaterial()
        {
            return new SpriteMaterialDescription
            (
                colorMap: "FreePixelEffectsPack/12_nebula_spritesheet.png",
                materials: new HashSet<SpriteMaterialTextureItem>()
            );
        }

        public Sprite CreateSprite()
        {
            return new Sprite(new Dictionary<string, SpriteAnimation>()
            {
                 { "default", new SpriteAnimation((int)(0.1f * Clock.SecondsToMicro), SpriteBuilder.CreateAnimatedSprite(100, 100, 8, 60)) },
            })
            { BaseScale = new Vector3(0.4f, 0.4f, 0.4f) };
        }
    }
}
