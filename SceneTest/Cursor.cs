using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Cursor : IDisposable
    {
        private readonly SceneObjectManager<IBattleManager> sceneObjectManager;
        private readonly SpriteManager sprites;
        private readonly IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private ISpriteMaterial spriteMaterial;
        private SceneObject sceneObject;
        private Sprite sprite;
        private bool disposed;

        public Cursor(SceneObjectManager<IBattleManager> sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IScopedCoroutine coroutine,
            IDestructionRequest destructionRequest,
            ISpriteMaterialManager spriteMaterialManager)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.sprites = sprites;
            this.destructionRequest = destructionRequest;
            this.spriteMaterialManager = spriteMaterialManager;

            this.sprite = new Sprite() { BaseScale = new Vector3(0.5f, 0.5f, 1f) };

            sceneObject = new SceneObject()
            {
                vertexBuffer = plane.VertexBuffer,
                skinVertexBuffer = plane.SkinVertexBuffer,
                indexBuffer = plane.IndexBuffer,
                numIndices = plane.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                position = Vector3.Zero,
                orientation = Quaternion.Identity,
                scale = sprite.BaseScale * Vector3.ScaleIdentity,
                RenderShadow = true,
                Sprite = sprite,
            };

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until coroutine is finished and this is disposed.

                var matDesc = new SpriteMaterialDescription(
                    colorMap: "original/pointingfinger.png",
                    materials: new HashSet<SpriteMaterialTextureItem>());

                spriteMaterial = await this.spriteMaterialManager.Checkout(matDesc);

                if (disposed)
                {
                    spriteMaterialManager.Return(spriteMaterial);
                }
                else
                {
                    sceneObject.shaderResourceBinding = spriteMaterial.ShaderResourceBinding;
                }

                if (!destructionRequest.DestructionRequested)
                {
                    sprites.Add(sprite);
                    sceneObjectManager.Add(sceneObject);
                }
            });
        }

        private bool visible = true;
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                if(visible != value)
                {
                    visible = value;
                    if (visible)
                    {
                        sceneObjectManager.Add(sceneObject);
                    }
                    else
                    {
                        sceneObjectManager.Remove(sceneObject);
                    }
                }
            }
        }

        public void Dispose()
        {
            disposed = true;
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.TryReturn(spriteMaterial);
        }
    }
}
