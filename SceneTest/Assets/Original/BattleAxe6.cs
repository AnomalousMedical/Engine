using DiligentEngine.RT.Sprites;
using Engine;
using Engine.Platform;
using System.Collections.Generic;

namespace SceneTest.Assets.Original
{
    class BattleAxe6 : ISpriteAsset
    {
        public Quaternion GetOrientation()
        {
            return new Quaternion(0, MathFloat.PI / 4f, 0);
        }

        public SpriteMaterialDescription CreateMaterial()
        {
            return new SpriteMaterialDescription
                (
                    colorMap: "original/battle_axe_6.png",
                    materials: new HashSet<SpriteMaterialTextureItem>
                    {
                        new SpriteMaterialTextureItem(0xff865f41, "cc0Textures/Leather001_1K", "jpg"), //Hilt (brown)
                        new SpriteMaterialTextureItem(0xff949494, "cc0Textures/Metal032_1K", "jpg"), //Blade (grey)
                        new SpriteMaterialTextureItem(0xff545454, "cc0Textures/Metal032_1K", "jpg"), //Other Metal (grey)
                    },
                    shiny: true
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
                                SpriteFrameAttachment.FromFramePosition(6, 26, 0, 32, 32), //Center of grip
                            }
                        } )
                    },
                })
            { BaseScale = new Vector3(0.85f, 0.85f, 0.85f) };
        }
    }
}
