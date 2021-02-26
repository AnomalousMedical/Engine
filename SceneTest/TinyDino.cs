using DiligentEngine;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using Engine.Platform;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class TinyDino : IDisposable
    {
        public class Desc : SceneObjectDesc
        {
            public String SkinMaterial { get; set; } = "cc0Textures/Leather008_1K"; //Skin (green)

            public String SpinesMaterial { get; set; } = "cc0Textures/SheetMetal004_1K"; //Spines (red)
        }

        private ISpriteMaterial spriteMaterial;
        private SceneObjectManager sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private SceneObject sceneObject;
        private Sprite sprite = new Sprite() { BaseScale = new Vector3(1.466666666666667f, 1, 1) };

        public TinyDino(
            SceneObjectManager sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ISpriteMaterialManager spriteMaterialManager,
            Desc tinyDinoDesc)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.sprites = sprites;
            this.destructionRequest = destructionRequest;
            this.spriteMaterialManager = spriteMaterialManager;
            IEnumerator<YieldAction> co()
            {
                yield return coroutine.Await(async () =>
                {
                    spriteMaterial = await this.spriteMaterialManager.Checkout(new SpriteMaterialDescription
                    (
                        colorMap: "original/TinyDino_Color.png",
                        materials: new HashSet<SpriteMaterialTextureItem>
                        {
                            new SpriteMaterialTextureItem(0xff168516, tinyDinoDesc.SkinMaterial, "jpg"), 
                            new SpriteMaterialTextureItem(0xffff0000, tinyDinoDesc.SpinesMaterial, "jpg"),
                        }
                    ));
                });

                sceneObject = new SceneObject()
                {
                    vertexBuffer = plane.VertexBuffer,
                    skinVertexBuffer = plane.SkinVertexBuffer,
                    indexBuffer = plane.IndexBuffer,
                    numIndices = plane.NumIndices,
                    pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                    position = tinyDinoDesc.Translation,
                    orientation = tinyDinoDesc.Orientation,
                    scale = sprite.BaseScale * tinyDinoDesc.Scale,
                    shaderResourceBinding = spriteMaterial.ShaderResourceBinding,
                    RenderShadow = true,
                    Sprite = sprite,
                };

                sprites.Add(sprite);
                sceneObjectManager.Add(sceneObject);
            }
            coroutine.Run(co());
        }

        public void Dispose()
        {
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.Return(spriteMaterial);
        }
    }
}
