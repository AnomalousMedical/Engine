using DiligentEngine.RT.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Assets.Original
{
    internal class FighterPlayerSprite : PlayerSprite
    {
        public FighterPlayerSprite()
        {
            SpriteMaterialDescription = new SpriteMaterialDescription
            (
                colorMap: "original/avt3_full.png",
                materials: new HashSet<SpriteMaterialTextureItem>
                {
                    new SpriteMaterialTextureItem(0xff0054a8, "cc0Textures/Fabric012_1K", "jpg"), //Helmet (blue)
                    new SpriteMaterialTextureItem(0xffffff00, "cc0Textures/Fabric027_1K", "jpg"), //Coat (yellow)
                    new SpriteMaterialTextureItem(0xffff8000, "cc0Textures/Metal032_1K", "jpg", reflective: true), //Armor (orange)
                }
            );
        }
    }
}
