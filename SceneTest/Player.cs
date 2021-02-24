using DiligentEngine;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using Engine.Platform;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Player : IDisposable
    {
        private AutoPtr<IShaderResourceBinding> pboMatBindingSprite;
        private SceneObjectManager sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private SceneObject sceneObject;
        private Sprite sprite = new Sprite(new Dictionary<string, SpriteAnimation>()
        {
        { "default", new SpriteAnimation((int)(0.3f * Clock.SecondsToMicro), SpriteFrame.MakeFramesFromHorizontal(24f / 192f, 32f / 128f, 192f, 8, 0).ToArray()) }
        });

        public Player(
            SceneObjectManager sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            IMaterialSpriteBuilder materialSpriteBuilder)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.sprites = sprites;
            this.destructionRequest = destructionRequest;

            IEnumerator<YieldAction> co()
            {
                yield return coroutine.Await(async () =>
                {
                    pboMatBindingSprite = await materialSpriteBuilder.CreateSpriteAsync(new MaterialSpriteBindingDescription()
                    {
                        ColorMap = "spritewalk/rpg_sprite_walk_Color.png"
                    });
                });

                sceneObject = new SceneObject()
                {
                    vertexBuffer = plane.VertexBuffer,
                    skinVertexBuffer = plane.SkinVertexBuffer,
                    indexBuffer = plane.IndexBuffer,
                    numIndices = plane.NumIndices,
                    pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                    position = new Vector3(0, 0.291666666666667f, 0),
                    orientation = Quaternion.Identity,
                    scale = new Vector3(1, 1.291666666666667f, 1),
                    shaderResourceBinding = pboMatBindingSprite.Obj,
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
            pboMatBindingSprite.Dispose();
        }
    }
}
