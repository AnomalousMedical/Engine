using Engine;
using Engine.Platform;
using SceneTest.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Assets.PixelEffects
{
    class Vortex : ISpriteAsset
    {
        public SpriteMaterialDescription CreateMaterial()
        {
            return new SpriteMaterialDescription
            (
                colorMap: "FreePixelEffectsPack/13_vortex_spritesheet.png",
                materials: new HashSet<SpriteMaterialTextureItem>()
            );
        }

        public Sprite CreateSprite()
        {
            return new Sprite(new Dictionary<string, SpriteAnimation>()
            {
                 { "default", new SpriteAnimation((int)(0.1f * Clock.SecondsToMicro), SpriteBuilder.CreateAnimatedSprite(100, 100, 8, 60)) },
            })
            { BaseScale = new Vector3(2.75f, 2.75f, 2.75f) };
        }
    }
}
