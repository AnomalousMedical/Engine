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
        private AutoPtr<IShaderResourceBinding> pboMatBindingSprite;
        private SceneObjectManager sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private SceneObject sceneObject;
        private Sprite sprite = new Sprite();

        public TinyDino(
            SceneObjectManager sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            IMaterialSpriteBuilder materialSpriteBuilder)
        {
            IEnumerator<YieldAction> co()
            {
                this.sceneObjectManager = sceneObjectManager;
                this.sprites = sprites;
                this.destructionRequest = destructionRequest;

                yield return coroutine.Await(async () =>
                {
                    pboMatBindingSprite = await materialSpriteBuilder.CreateSpriteAsync(new MaterialSpriteBindingDescription()
                    {
                        ColorMap = "original/TinyDino_Color.png",
                        Materials = new Dictionary<uint, (String, String)>()
                        {
                            { 0xff168516, ( "cc0Textures/Leather008_1K", "jpg" ) }, //Skin (green)
                            { 0xffff0000, ( "cc0Textures/SheetMetal004_1K", "jpg" ) }, //Spines (red)
                        }
                    });
                });

                sceneObject = new SceneObject()
                {
                    vertexBuffer = plane.VertexBuffer,
                    skinVertexBuffer = plane.SkinVertexBuffer,
                    indexBuffer = plane.IndexBuffer,
                    numIndices = plane.NumIndices,
                    pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                    position = new Vector3(-4, 0, -3),
                    orientation = Quaternion.Identity,
                    scale = new Vector3(1.466666666666667f, 1, 1),
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
