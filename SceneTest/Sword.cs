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

        private Vector3 position = new Vector3(0f, .65f, 0f);
        private Quaternion orientation = new Quaternion(0, MathFloat.PI / 4f, 0);
        private Vector3 scale = new Vector3(0.75f, 0.75f, 0.75f);

        //private Vector3 position = Vector3.Zero;
        //private Quaternion orientation = Quaternion.Identity;
        //private Vector3 scale = Vector3.ScaleIdentity * 0.5f;

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
                        //colorMap: "opengameart/Dungeon Crawl Stone Soup Full/misc/cursor_red.png",
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
                position = this.position,
                orientation = this.orientation,
                scale = sprite.BaseScale * this.scale,
                RenderShadow = true,
                Sprite = sprite,
            };
        }

        public void SetPosition(ref Vector3 position, ref Quaternion rotation, ref Vector3 scale)
        {
            var translate = scale * Quaternion.quatRotate(ref rotation, ref this.position);

            this.sceneObject.position = position + translate;
            this.sceneObject.orientation = rotation * this.orientation;
            this.sceneObject.scale = scale * sprite.BaseScale * this.scale;
        }

        public void Dispose()
        {
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.Return(spriteMaterial);
        }
    }
}
