using DiligentEngine.RT.Sprites;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Assets.Original
{
    public abstract class PlayerSprite : IPlayerSprite
    {
        const float SpriteStepX = 32f / 128f;
        const float SpriteStepY = 32f / 64f;

        const int spriteWalkFrameSpeed = (int)(0.2f * Clock.SecondsToMicro);

        public SpriteMaterialDescription SpriteMaterialDescription { get; protected set; }

        public Dictionary<String, SpriteAnimation> Animations => animations;

        /*********************************************
         * 
         * To make a sprite for this animation add the textures in the following order to the image atlas packer
         * 
         * Back right dominant
         * Back left dominant
         * Front left dominant
         * Front right dominant
         * Side wide
         * Side narrow
         * Front stand
         * Back stand
         * 
         * 128 x 64
         * 
         ********************************************/

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
