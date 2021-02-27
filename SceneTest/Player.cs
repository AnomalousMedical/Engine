using BepuPlugin;
using BepuPlugin.Characters;
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
        private readonly IBepuScene bepuScene;
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

        private Attachment sword;
        private Attachment shield;

        //Make these configurable
        private int primaryHand = RightHand;
        private int secondaryHand = LeftHand;

        private Vector3 position = new Vector3(-1, 0, 0);
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 scale = Vector3.ScaleIdentity;

        private CharacterInput characterInput;

        public Player(
            SceneObjectManager sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ISpriteMaterialManager spriteMaterialManager,
            IObjectResolverFactory objectResolverFactory,
            IBepuScene bepuScene)
        {
            objectResolver = objectResolverFactory.Create();

            sword = objectResolver.Resolve<Attachment, Attachment.Description>(o =>
            {
                o.Orientation = new Quaternion(0, MathFloat.PI / 4f, 0);
                o.Sprite = new Sprite(new Dictionary<string, SpriteAnimation>()
                {
                    { "default", new SpriteAnimation((int)(0.7f * Clock.SecondsToMicro),
                        new SpriteFrame(0, 0, 1, 1)
                        {
                            Attachments = new List<SpriteFrameAttachment>()
                            {
                                SpriteFrameAttachment.FromFramePosition(6, 25, 0, 32, 32), //Center of grip
                            }
                        } )
                    },
                })
                { BaseScale = new Vector3(0.75f, 0.75f, 0.75f) };
                o.SpriteMaterial = new SpriteMaterialDescription
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
                );
            });

            shield = objectResolver.Resolve<Attachment, Attachment.Description>(o =>
            {
                o.Sprite = new Sprite() { BaseScale = new Vector3(0.75f, 0.75f, 0.75f) };
                o.SpriteMaterial = new SpriteMaterialDescription
                (
                    colorMap: "original/shield_of_reflection.png",
                    //colorMap: "opengameart/Dungeon Crawl Stone Soup Full/misc/cursor_red.png",
                    materials: new HashSet<SpriteMaterialTextureItem>
                    {
                        new SpriteMaterialTextureItem(0xffa0a0a0, "cc0Textures/Pipe002_1K", "jpg"), //Blade (grey)
                    }
                );
            });

            sprite.FrameChanged += Sprite_FrameChanged;

            this.sceneObjectManager = sceneObjectManager;
            this.sprites = sprites;
            this.destructionRequest = destructionRequest;
            this.spriteMaterialManager = spriteMaterialManager;
            this.bepuScene = bepuScene;
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

            characterInput = bepuScene.CreateCharacter(new System.Numerics.Vector3(this.position.x, this.position.y, this.position.z));
        }

        public void Dispose()
        {
            bepuScene.DestroyCharacter(characterInput);
            sprite.FrameChanged -= Sprite_FrameChanged;
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.Return(spriteMaterial);
            objectResolver.Dispose();
        }

        private void Sprite_FrameChanged(FrameEventSprite obj)
        {
            var frame = obj.GetCurrentFrame();

            var scale = sprite.BaseScale * this.scale;
            {
                var primaryAttach = frame.Attachments[this.primaryHand];
                var offset = scale * primaryAttach.translate;
                offset = Quaternion.quatRotate(ref this.rotation, ref offset) + this.position;
                sword.SetPosition(ref offset, ref this.rotation, ref scale);
            }

            {
                var secondaryAttach = frame.Attachments[this.secondaryHand];
                var offset = scale * secondaryAttach.translate;
                offset = Quaternion.quatRotate(ref this.rotation, ref offset) + this.position;
                shield.SetPosition(ref offset, ref this.rotation, ref scale);
            }
        }

        public void SyncPhysics(Clock clock)
        {
            var body = bepuScene.Simulation.Bodies.GetBodyReference(characterInput.BodyHandle);
            var pose = body.Pose;
            var bodPos = pose.Position;
            this.sceneObject.position = this.position = new Vector3(bodPos.X, bodPos.Y, bodPos.Z);
            var bodOrientation = pose.Orientation;
            this.sceneObject.orientation = this.rotation = new Quaternion(bodOrientation.X, bodOrientation.Y, bodOrientation.Z, bodOrientation.W);
            Sprite_FrameChanged(sprite);
            Console.WriteLine(this.sceneObject.position);
        }
    }
}
