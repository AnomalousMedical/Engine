﻿using DiligentEngine;
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
            { "down", new SpriteAnimation((int)(0.7f * Clock.SecondsToMicro),
                new SpriteFrame(SpriteStepX * 1, SpriteStepY * 0, SpriteStepX * 2, SpriteStepY * 1)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(3, 23, -0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(26, 20, -0.01f, 32, 32), //Left Hand
                    }
                },
                new SpriteFrame(SpriteStepX * 1, SpriteStepY * 1, SpriteStepX * 2, SpriteStepY * 2)
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(6, 20, -0.01f, 32, 32), //Right Hand
                        SpriteFrameAttachment.FromFramePosition(29, 23, -0.01f, 32, 32), //Left Hand
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

        //Make these configurable
        private int primaryHand = RightHand;
        private int secondaryHand = LeftHand;

        private Vector3 position = new Vector3(-1, 0, 0);
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 scale = Vector3.ScaleIdentity;

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
                position = this.position,
                orientation = this.rotation,
                scale = this.scale,
                RenderShadow = true,
                Sprite = sprite,
            };

            Sprite_FrameChanged(sprite);
        }

        private void Sprite_FrameChanged(FrameEventSprite obj)
        {
            var frame = obj.GetCurrentFrame();
            var primaryAttach = frame.Attachments[this.primaryHand];

            {
                var scale = sprite.BaseScale * this.scale;
                var offset = scale * primaryAttach.translate;
                offset = Quaternion.quatRotate(ref this.rotation, ref offset) + this.position;
                sword.SetPosition(ref offset, ref this.rotation, ref scale);
            }

            {
                var secondaryAttach = frame.Attachments[this.secondaryHand];
                var offset = secondaryAttach.translate + sceneObject.position;
                shield.SetPosition(ref offset);
            }
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
