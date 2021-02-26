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
    class Sword : IDisposable
    {
        private ISpriteMaterial spriteMaterial;
        private SceneObjectManager sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private SceneObject sceneObject;
        private Sprite sprite = new Sprite() { BaseScale = new Vector3(1, 1, 1) };

        public Sword(
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
                        colorMap: "original/greatsword_01.png",
                        materials: new HashSet<SpriteMaterialTextureItem>
                        {
                            new SpriteMaterialTextureItem(0xff802000, "cc0Textures/Leather001_1K", "jpg"), //Hilt (brown)
                            new SpriteMaterialTextureItem(0xffadadad, "cc0Textures/Metal032_1K", "jpg"), //Blade (grey)
                            new SpriteMaterialTextureItem(0xff5e5e5f, "cc0Textures/Metal032_1K", "jpg"), //Blade (grey)
                            new SpriteMaterialTextureItem(0xffe4ac26, "cc0Textures/Metal038_1K", "jpg"), //Blade (grey)
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
                orientation = new Quaternion(0, 3.14f / 4f, 0),
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
