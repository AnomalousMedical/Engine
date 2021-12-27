using DiligentEngine.RT.Sprites;
using Engine;
using Engine.Platform;
using System.Collections.Generic;

namespace SceneTest.Assets.Original
{
    class ShieldOfReflection : ISpriteAsset
    {
        public SpriteMaterialDescription CreateMaterial()
        {
            return new SpriteMaterialDescription
                (
                    colorMap: "original/shield_of_reflection.png",
                    //colorMap: "opengameart/Dungeon Crawl Stone Soup Full/misc/cursor_red.png",
                    materials: new HashSet<SpriteMaterialTextureItem>
                    {
                        new SpriteMaterialTextureItem(0xffa0a0a0, "cc0Textures/Pipe002_1K", "jpg"), //Blade (grey)
                    },
                    shiny: true
                );
        }

        public Sprite CreateSprite()
        {
            return new Sprite() { BaseScale = new Vector3(0.75f, 0.75f, 0.75f) };
        }
    }
}
