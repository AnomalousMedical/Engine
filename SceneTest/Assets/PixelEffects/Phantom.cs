using Engine;
using Engine.Platform;
using SceneTest.Resources;
using System.Collections.Generic;

namespace SceneTest.Assets.PixelEffects
{
    class Phantom : ISpriteAsset
    {
        public SpriteMaterialDescription CreateMaterial()
        {
            return new SpriteMaterialDescription
            (
                colorMap: "FreePixelEffectsPack/14_phantom_spritesheet.png",
                materials: new HashSet<SpriteMaterialTextureItem>()
            );
        }

        public Sprite CreateSprite()
        {
            return new Sprite(new Dictionary<string, SpriteAnimation>()
            {
                 { "default", new SpriteAnimation((int)(0.1f * Clock.SecondsToMicro), SpriteBuilder.CreateAnimatedSprite(100, 100, 8, 61)) },
            })
            { BaseScale = new Vector3(1, 1, 1) };
        }
    }
}
