using DiligentEngine.RT.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Assets.Original
{
    internal class ThiefPlayerSprite : PlayerSprite
    {
        public ThiefPlayerSprite()
        {
            SpriteMaterialDescription = new SpriteMaterialDescription
            (
                colorMap: "original/bmg3_full.png",
                materials: new HashSet<SpriteMaterialTextureItem>
                {
                    new SpriteMaterialTextureItem(0xff1c8cff, "cc0Textures/Leather001_1K", "jpg"), //Blue (armor)
                    new SpriteMaterialTextureItem(0xffb470ff, "cc0Textures/Fabric012_1K", "jpg"), //Purple (cape)
                    new SpriteMaterialTextureItem(0xffa85400, "cc0Textures/Carpet008_1K", "jpg"), //Brown (hair)
                }
            );
        }
    }
}
