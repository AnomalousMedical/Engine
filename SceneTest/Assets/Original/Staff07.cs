using Engine;
using Engine.Platform;
using SceneTest.Resources;
using System.Collections.Generic;

namespace SceneTest.Assets.Original
{
    class Staff07 : ISpriteAsset
    {
        public Quaternion GetOrientation()
        {
            return new Quaternion(0, MathFloat.PI / 4f, 0);
        }

        public SpriteMaterialDescription CreateMaterial()
        {
            return new SpriteMaterialDescription
                (
                    colorMap: "original/staff_7.png",
                    materials: new HashSet<SpriteMaterialTextureItem>
                    {
                        new SpriteMaterialTextureItem(0xff9f7f66, "cc0Textures/AcousticFoam003_1K", "jpg"),
                        new SpriteMaterialTextureItem(0xff6a5db6, "cc0Textures/MetalPlates008_1K", "jpg"),
                    }
                );
        }

        public Sprite CreateSprite()
        {
            return new Sprite(new Dictionary<string, SpriteAnimation>()
                {
                    { "default", new SpriteAnimation((int)(0.7f * Clock.SecondsToMicro),
                        new SpriteFrame(0, 0, 1, 1)
                        {
                            Attachments = new List<SpriteFrameAttachment>()
                            {
                                SpriteFrameAttachment.FromFramePosition(6, 25, 0, 32, 32), //Center of grip
                            }
                        } )
                    },
                })
            { BaseScale = new Vector3(0.75f, 0.75f, 0.75f) };
        }
    }
}
