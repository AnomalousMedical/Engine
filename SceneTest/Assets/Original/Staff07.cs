using DiligentEngine.RT.Sprites;
using Engine;
using Engine.Platform;
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
                        new SpriteMaterialTextureItem(0xff9f7f66, "cc0Textures/Leather001_1K", "jpg"),
                        new SpriteMaterialTextureItem(0xff6a5db6, "cc0Textures/Metal032_1K", "jpg"),
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
            { BaseScale = new Vector3(1f, 1f, 1f) };
        }
    }
}
