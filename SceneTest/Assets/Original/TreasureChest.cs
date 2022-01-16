using DiligentEngine.RT.Sprites;
using Engine;
using Engine.Platform;
using System.Collections.Generic;

namespace SceneTest.Assets.Original
{
    class TreasureChest : ISpriteAsset
    {
        public Quaternion GetOrientation()
        {
            return new Quaternion(0, MathFloat.PI / 4f, 0);
        }

        public SpriteMaterialDescription CreateMaterial()
        {
            return new SpriteMaterialDescription
                (
                    colorMap: "original/treasure_chests_32x32.png",
                    materials: new HashSet<SpriteMaterialTextureItem>
                    {
                    }
                );
        }

        public Sprite CreateSprite()
        {
            return new Sprite(new Dictionary<string, SpriteAnimation>()
                {
                    { "default", new SpriteAnimation((int)(0.7f * Clock.SecondsToMicro),
                        new SpriteFrame(0, 0, 24f / 64f, 24f / 32f)
                        {
                            Attachments = new List<SpriteFrameAttachment>()
                            {
                                SpriteFrameAttachment.FromFramePosition(6, 25, 0, 32, 32), //Center of grip
                            }
                        } )
                    },
                    { "open", new SpriteAnimation((int)(0.7f * Clock.SecondsToMicro),
                        new SpriteFrame(25f / 64f, 0, 48f / 64f, 28f / 32f)
                        {
                            Attachments = new List<SpriteFrameAttachment>()
                            {
                                SpriteFrameAttachment.FromFramePosition(6, 25, 0, 32, 32), //Center of grip
                            }
                        } )
                    },
                })
            { BaseScale = new Vector3(0.55f, 0.55f, 0.55f) };
        }
    }
}
