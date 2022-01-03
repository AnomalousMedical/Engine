using DiligentEngine.RT.Sprites;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Assets.Original
{
    class TinyDino : ISpriteAsset
    {
        public string SkinMaterial { get; set; } = "cc0Textures/Leather008_1K";
        public string SpineMaterial { get; set; } = "cc0Textures/Leather008_1K";

        public SpriteMaterialDescription CreateMaterial()
        {
            return new SpriteMaterialDescription
            (
                colorMap: "original/TinyDino_Color.png",
                materials: new HashSet<SpriteMaterialTextureItem>
                {
                    new SpriteMaterialTextureItem(0xff168516, SkinMaterial, "jpg"),//Skin (green)
                    new SpriteMaterialTextureItem(0xffff0000, SpineMaterial, "jpg"),//Spines (red)
                }
            );
        }

        public Sprite CreateSprite()
        {
            return new Sprite() { BaseScale = new Vector3(1.466666666666667f, 1, 1) };
        }
    }
}
