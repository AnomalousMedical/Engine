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
        public const int RightHand = 0;
        public const int LeftHand = 1;

        private ISpriteMaterial spriteMaterial;
        private SceneObjectManager sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private SceneObject sceneObject;
        private IObjectResolver objectResolver;

        const float SpriteStepX = 32f / 128f;
        const float SpriteStepY = 32f / 64f;

        private FrameEventSprite sprite = new FrameEventSprite(new Dictionary<string, SpriteAnimation>()
        {
            { "down", new SpriteAnimation((int)(0.3f * Clock.SecondsToMicro),
                new SpriteFrame(SpriteStepX * 1, SpriteStepY * 0, SpriteStepX * 2, SpriteStepY * 1)
                {
                    Attachments = new List<Vector3>()
                    {
                        new Vector3(-0.8f, +0.2f, -0.01f), //Right Hand
                        new Vector3(0.65f, -0.2f, -0.01f), //Left Hand
                    }
                },
                new SpriteFrame(SpriteStepX * 1, SpriteStepY * 1, SpriteStepX * 2, SpriteStepY * 2)
                {
                    Attachments = new List<Vector3>()
                    {
                        new Vector3(-0.65f, +0.4f, -0.01f), //Right Hand
                        new Vector3(0.8f, -0.4f, -0.01f), //Left Hand
                    }
                } )
            },
            { "left", new SpriteAnimation((int)(0.3f * Clock.SecondsToMicro), 
                new SpriteFrame(SpriteStepX * 2, SpriteStepY * 0, SpriteStepX * 3, SpriteStepY * 1), 
                new SpriteFrame(SpriteStepX * 3, SpriteStepY * 0, SpriteStepX * 4, SpriteStepY * 1) ) 
            },
            { "up", new SpriteAnimation((int)(0.3f * Clock.SecondsToMicro), 
                new SpriteFrame(SpriteStepX * 0, SpriteStepY * 0, SpriteStepX * 1, SpriteStepY * 1), 
                new SpriteFrame(SpriteStepX * 0, SpriteStepY * 1, SpriteStepX * 1, SpriteStepY * 2) ) 
            },
            { "right", new SpriteAnimation((int)(0.3f * Clock.SecondsToMicro), 
                new SpriteFrame(SpriteStepX * 2, SpriteStepY * 1, SpriteStepX * 3, SpriteStepY * 2), 
                new SpriteFrame(SpriteStepX * 3, SpriteStepY * 1, SpriteStepX * 4, SpriteStepY * 2) ) 
            },
        });

        private Sword sword;
        private Shield shield;

        public Player(
            SceneObjectManager sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ISpriteMaterialManager spriteMaterialManager,
            IObjectResolverFactory objectResolverFactory)
        {
            objectResolver = objectResolverFactory.Create();

            sword = objectResolver.Resolve<Sword>();
            shield = objectResolver.Resolve<Shield>();

            sprite.FrameChanged += Sprite_FrameChanged;

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
                        colorMap: "original/amg1_full4.png",
                        materials: new HashSet<SpriteMaterialTextureItem>
                        {
                            new SpriteMaterialTextureItem(0xffa854ff, "cc0Textures/Fabric012_1K", "jpg"),
                            new SpriteMaterialTextureItem(0xff909090, "cc0Textures/Fabric020_1K", "jpg"),
                            new SpriteMaterialTextureItem(0xff8c4800, "cc0Textures/Leather026_1K", "jpg"),
                            new SpriteMaterialTextureItem(0xffffe254, "cc0Textures/Metal038_1K", "jpg"),
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
                orientation = Quaternion.Identity,
                scale = new Vector3(1, 1, 1),
                RenderShadow = true,
                Sprite = sprite,
            };

            Sprite_FrameChanged(sprite);
        }

        private void Sprite_FrameChanged(FrameEventSprite obj)
        {
            var frame = obj.GetCurrentFrame();
            var offset = frame.Attachments[RightHand] + sceneObject.position;
            sword.SetPosition(ref offset);

            offset = frame.Attachments[LeftHand] + sceneObject.position;
            shield.SetPosition(ref offset);
        }

        public void Dispose()
        {
            sprite.FrameChanged -= Sprite_FrameChanged;
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.Return(spriteMaterial);
            objectResolver.Dispose();
        }
    }
}
