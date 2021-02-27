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
    class Shield : IDisposable
    {
        private ISpriteMaterial spriteMaterial;
        private SceneObjectManager sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private SceneObject sceneObject;
        private Sprite sprite = new Sprite() { BaseScale = new Vector3(1, 1, 1) };

        public Shield(
            SceneObjectManager sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ISpriteMaterialManager spriteMaterialManager)
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
                        //colorMap: "original/shield_of_reflection.png",
                        colorMap: "opengameart/Dungeon Crawl Stone Soup Full/misc/cursor_red.png",
                        materials: new HashSet<SpriteMaterialTextureItem>
                        {
                            new SpriteMaterialTextureItem(0xffa0a0a0, "cc0Textures/Pipe002_1K", "jpg"), //Blade (grey)
                        }
                    ));

                    sceneObject.shaderResourceBinding = spriteMaterial.ShaderResourceBinding;
                });
                sprites.Add(sprite);
                sceneObjectManager.Add(sceneObject);
            }
            coroutine.Run(co());

            sceneObject = new SceneObject()
            {
                vertexBuffer = plane.VertexBuffer,
                skinVertexBuffer = plane.SkinVertexBuffer,
                indexBuffer = plane.IndexBuffer,
                numIndices = plane.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                position = new Vector3(-1, 0, 0),
                orientation = new Quaternion(0, 0, 0),
                scale = sprite.BaseScale * 0.75f,
                RenderShadow = true,
                Sprite = sprite,
            };
        }

        public void SetPosition(ref Vector3 position)
        {
            this.sceneObject.position = position;
        }

        public void Dispose()
        {
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.Return(spriteMaterial);
        }
    }
}
