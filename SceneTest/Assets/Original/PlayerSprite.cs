using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Assets.Original
{
    public class PlayerSprite : IPlayerSprite
    {
        const float SpriteStepX = 32f / 128f;
        const float SpriteStepY = 32f / 64f;

        const int spriteWalkFrameSpeed = (int)(0.2f * Clock.SecondsToMicro);

        public SpriteMaterialDescription SpriteMaterialDescription => spriteMaterialDescription;

        private SpriteMaterialDescription spriteMaterialDescription = new SpriteMaterialDescription
        (
            colorMap: "original/amg1_full4.png",
            materials: new HashSet<SpriteMaterialTextureItem>
            {
                new SpriteMaterialTextureItem(0xffa854ff, "cc0Textures/Fabric012_1K", "jpg"),
                new SpriteMaterialTextureItem(0xff909090, "cc0Textures/Fabric020_1K", "jpg"),
                new SpriteMaterialTextureItem(0xff8c4800, "cc0Textures/Leather026_1K", "jpg"),
                new SpriteMaterialTextureItem(0xffffe254, "cc0Textures/Metal038_1K", "jpg"),
            }
        );

        public Dictionary<String, SpriteAnimation> Animations => animations;

        private Dictionary<string, SpriteAnimation> animations = new Dictionary<string, SpriteAnimation>()
        {
            { "stand-down", new SpriteAnimation(spriteWalkFrameSpeed,
                new SpriteFrame(SpriteStepX * 2, SpriteStepY * 1, SpriteStepX * 3, SpriteStepY * 2)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(3, 23, -0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(29, 23, -0.01f, 32, 32), //Left Hand
                    }
                } )
            },
            { "stand-left", new SpriteAnimation(spriteWalkFrameSpeed,
                new SpriteFrame(SpriteStepX * 3, SpriteStepY * 0, SpriteStepX * 4, SpriteStepY * 1)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(16, 23, +0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(16, 23, -0.01f, 32, 32), //Left Hand
                    }
                })
            },
            { "stand-right", new SpriteAnimation(spriteWalkFrameSpeed,
                new SpriteFrame(SpriteStepX * 4, SpriteStepY * 0, SpriteStepX * 3, SpriteStepY * 1)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(16, 23, -0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(16, 23, +0.01f, 32, 32), //Left Hand
                    }
                } )
            },
            { "stand-up", new SpriteAnimation(spriteWalkFrameSpeed,
                new SpriteFrame(SpriteStepX * 3, SpriteStepY * 1, SpriteStepX * 4, SpriteStepY * 2)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(29, 23, +0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(3, 23, +0.01f, 32, 32), //Left Hand
                    }
                } )
            },
            { "down", new SpriteAnimation(spriteWalkFrameSpeed,
                new SpriteFrame(SpriteStepX * 1, SpriteStepY * 0, SpriteStepX * 2, SpriteStepY * 1)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(3, 23, -0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(26, 20, -0.01f, 32, 32), //Left Hand
                    }
                },
                new SpriteFrame(SpriteStepX * 1, SpriteStepY * 1, SpriteStepX * 2, SpriteStepY * 2)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(6, 20, -0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(29, 23, -0.01f, 32, 32), //Left Hand
                    }
                } )
            },
            { "up", new SpriteAnimation(spriteWalkFrameSpeed,
                new SpriteFrame(SpriteStepX * 0, SpriteStepY * 0, SpriteStepX * 1, SpriteStepY * 1)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(26, 20, +0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(3, 23, +0.01f, 32, 32), //Left Hand
                    }
                },
                new SpriteFrame(SpriteStepX * 0, SpriteStepY * 1, SpriteStepX * 1, SpriteStepY * 2)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(29, 23, +0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(6, 20, +0.01f, 32, 32), //Left Hand
                    }
                } )
            },
            { "right", new SpriteAnimation(spriteWalkFrameSpeed,
                new SpriteFrame(SpriteStepX * 4, SpriteStepY * 0, SpriteStepX * 3, SpriteStepY * 1)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(16, 23, -0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(16, 23, +0.01f, 32, 32), //Left Hand
                    }
                },
                new SpriteFrame(SpriteStepX * 3, SpriteStepY * 0, SpriteStepX * 2, SpriteStepY * 1)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(12, 24, -0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(23, 21, +0.01f, 32, 32), //Left Hand
                    }
                })
            },
            { "left", new SpriteAnimation(spriteWalkFrameSpeed,
                new SpriteFrame(SpriteStepX * 3, SpriteStepY * 0, SpriteStepX * 4, SpriteStepY * 1)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(16, 23, +0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(16, 23, -0.01f, 32, 32), //Left Hand
                    }
                },
                new SpriteFrame(SpriteStepX * 2, SpriteStepY * 0, SpriteStepX * 3, SpriteStepY * 1)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(9, 21, +0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(20, 24, -0.01f, 32, 32), //Left Hand
                    }
                })
            },
            { "cast-left", new SpriteAnimation(spriteWalkFrameSpeed,
                new SpriteFrame(SpriteStepX * 2, SpriteStepY * 0, SpriteStepX * 3, SpriteStepY * 1)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(9, 21, +0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(20, 24, -0.01f, 32, 32), //Left Hand
                    }
                })
            },
            { "cast-right", new SpriteAnimation(spriteWalkFrameSpeed,
                new SpriteFrame(SpriteStepX * 3, SpriteStepY * 0, SpriteStepX * 2, SpriteStepY * 1)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(12, 24, -0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(23, 21, +0.01f, 32, 32), //Left Hand
                    }
                })
            },
        };
    }
}
