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
    class Enemy : IDisposable
    {
        public class Desc : SceneObjectDesc
        {
            public Sprite Sprite { get; set; }

            public SpriteMaterialDescription SpriteMaterial { get; set; }

            public static void MakeTinyDino(Desc desc, String skinMaterial = "cc0Textures/Leather008_1K", String spineMaterial = "cc0Textures/SheetMetal004_1K")
            {
                desc.Sprite = new Sprite() { BaseScale = new Vector3(1.466666666666667f, 1, 1) };
                desc.SpriteMaterial = new SpriteMaterialDescription
                (
                    colorMap: "original/TinyDino_Color.png",
                    materials: new HashSet<SpriteMaterialTextureItem>
                    {
                        new SpriteMaterialTextureItem(0xff168516, skinMaterial, "jpg"),//Skin (green)
                        new SpriteMaterialTextureItem(0xffff0000, spineMaterial, "jpg"),//Spines (red)
                    }
                );
            }

            public static void MakeSkeleton(Desc desc)
            {
                desc.Sprite = new Sprite() { BaseScale = new Vector3(1, 1, 1) };
                desc.SpriteMaterial = new SpriteMaterialDescription
                (
                    colorMap: "original/skeletal_warrior_new.png",
                    materials: new HashSet<SpriteMaterialTextureItem>
                    {
                        new SpriteMaterialTextureItem(0xffd0873a, "cc0Textures/Metal040_1K", "jpg"),//Armor Highlight (copper)
                        new SpriteMaterialTextureItem(0xff453c31, "cc0Textures/Leather001_1K", "jpg"),//Armor (brown)
                        new SpriteMaterialTextureItem(0xffefefef, "cc0Textures/Rock022_1K", "jpg"),//Bone (almost white)
                    }
                );
            }
        }

        private ISpriteMaterial spriteMaterial;
        private SceneObjectManager<BattleManager> sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private SceneObject sceneObject;
        private Sprite sprite;
        private bool disposed;

        public Enemy(
            SceneObjectManager<BattleManager> sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ISpriteMaterialManager spriteMaterialManager,
            Desc description)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.sprites = sprites;
            this.destructionRequest = destructionRequest;
            this.spriteMaterialManager = spriteMaterialManager;

            this.sprite = description.Sprite;

            sceneObject = new SceneObject()
            {
                vertexBuffer = plane.VertexBuffer,
                skinVertexBuffer = plane.SkinVertexBuffer,
                indexBuffer = plane.IndexBuffer,
                numIndices = plane.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                position = description.Translation,
                orientation = description.Orientation,
                scale = sprite.BaseScale * description.Scale,
                RenderShadow = true,
                Sprite = sprite,
            };

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until coroutine is finished and this is disposed.

                spriteMaterial = await this.spriteMaterialManager.Checkout(description.SpriteMaterial);

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

        public void RequestDestruction()
        {
            this.destructionRequest.RequestDestruction();
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
