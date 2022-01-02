using DiligentEngine.RT.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Assets.Original
{
    internal class ClericPlayerSprite : PlayerSprite
    {
        public ClericPlayerSprite()
        {
            SpriteMaterialDescription = new SpriteMaterialDescription
            (
                colorMap: "original/jli1_full.png",
                materials: new HashSet<SpriteMaterialTextureItem>
                {
                    new SpriteMaterialTextureItem(0xff0080ff, "cc0Textures/Fabric012_1K", "jpg"), //Grey (robe)
                    new SpriteMaterialTextureItem(0xffb4b4b4, "cc0Textures/Fabric020_1K", "jpg"), //Blue (arms)
                    new SpriteMaterialTextureItem(0xffde3707, "cc0Textures/Carpet008_1K", "jpg"), //Red (hair)
                }
            );
        }
    }
}
