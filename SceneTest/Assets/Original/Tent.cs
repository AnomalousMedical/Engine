using DiligentEngine.RT.Sprites;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Assets.Original
{
    class Tent : ISpriteAsset
    {
        //Threax drew this one

        public string TentMaterial { get; set; } = "cc0Textures/Fabric045_1K";

        public SpriteMaterialDescription CreateMaterial()
        {
            return new SpriteMaterialDescription
            (
                colorMap: "original/Tent.png",
                materials: new HashSet<SpriteMaterialTextureItem>
                {
                    new SpriteMaterialTextureItem(0xff2476cf, TentMaterial, "jpg"),//blue
                }
            );
        }

        public Sprite CreateSprite()
        {
            return new Sprite() { BaseScale = new Vector3(2, 1, 1) };
        }
    }
}
