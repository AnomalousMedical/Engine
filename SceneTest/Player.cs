using BepuPhysics;
using BepuPhysics.Collidables;
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
        public class Description : SceneObjectDesc
        {
            public int PrimaryHand = RightHand;
            public int SecondaryHand = LeftHand;
            public EventLayers EventLayer = EventLayers.Default;
            public GamepadId Gamepad = GamepadId.Pad1;
        }

        public const int RightHand = 0;
        public const int LeftHand = 1;

        private ISpriteMaterial spriteMaterial;
        private SceneObjectManager sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private readonly IBepuScene bepuScene;
        private readonly EventManager eventManager;
        private readonly EventLayer eventLayer;
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

        private CharacterMover characterMover;
        private TypedIndex shapeIndex;

        private int primaryHand;
        private int secondaryHand;
        private GamepadId gamepadId;
        private bool allowJoystickInput = true;

        ButtonEvent moveForward = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_W });
        ButtonEvent moveBackward = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_S });
        ButtonEvent moveRight = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_D });
        ButtonEvent moveLeft = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_A });
        ButtonEvent sprint = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_LSHIFT });
        ButtonEvent jump = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_SPACE });

        private bool disposed;

        public Player(
            SceneObjectManager sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ISpriteMaterialManager spriteMaterialManager,
            IObjectResolverFactory objectResolverFactory,
            IBepuScene bepuScene,
            EventManager eventManager,
            Description description)
        {
            this.primaryHand = description.PrimaryHand;
            this.secondaryHand = description.SecondaryHand;
            this.gamepadId = description.Gamepad;

            //Events
            eventManager.addEvent(moveForward);
            eventManager.addEvent(moveBackward);
            eventManager.addEvent(moveLeft);
            eventManager.addEvent(moveRight);
            eventManager.addEvent(sprint);
            eventManager.addEvent(jump);

            eventLayer = eventManager[description.EventLayer];
            eventLayer.OnUpdate += EventLayer_OnUpdate;

            //These events are owned by this class, so don't have to unsubscribe
            moveForward.FirstFrameDownEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    characterMover.movementDirection.Y = 1;
                    l.alertEventsHandled();
                    allowJoystickInput = false;
                }
            };
            moveForward.FirstFrameUpEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    if (characterMover.movementDirection.Y > 0.5f) { characterMover.movementDirection.Y = 0; }
                    l.alertEventsHandled();
                    allowJoystickInput = moveForward.Up && moveBackward.Up && moveLeft.Up && moveRight.Up;
                }
            };
            moveBackward.FirstFrameDownEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    characterMover.movementDirection.Y = -1;
                    l.alertEventsHandled();
                    allowJoystickInput = false;
                }
            };
            moveBackward.FirstFrameUpEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    if (characterMover.movementDirection.Y < -0.5f) { characterMover.movementDirection.Y = 0; }
                    l.alertEventsHandled();
                    allowJoystickInput = moveForward.Up && moveBackward.Up && moveLeft.Up && moveRight.Up;
                }
            };
            moveLeft.FirstFrameDownEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    characterMover.movementDirection.X = -1;
                    l.alertEventsHandled();
                    allowJoystickInput = false;
                }
            };
            moveLeft.FirstFrameUpEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    if (characterMover.movementDirection.X < 0.5f) { characterMover.movementDirection.X = 0; }
                    l.alertEventsHandled();
                    allowJoystickInput = moveForward.Up && moveBackward.Up && moveLeft.Up && moveRight.Up;
                }
            };
            moveRight.FirstFrameDownEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    characterMover.movementDirection.X = 1;
                    l.alertEventsHandled();
                    allowJoystickInput = false;
                }
            };
            moveRight.FirstFrameUpEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    if (characterMover.movementDirection.X > 0.5f) { characterMover.movementDirection.X = 0; }
                    l.alertEventsHandled();
                    allowJoystickInput = moveForward.Up && moveBackward.Up && moveLeft.Up && moveRight.Up;
                }
            };
            jump.FirstFrameDownEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    characterMover.tryJump = true;
                    l.alertEventsHandled();
                }
            };
            jump.FirstFrameUpEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    characterMover.tryJump = false;
                    l.alertEventsHandled();
                }
            };
            sprint.FirstFrameDownEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    characterMover.sprint = true;
                    l.alertEventsHandled();
                }
            };
            sprint.FirstFrameUpEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    characterMover.sprint = false;
                    l.alertEventsHandled();
                }
            };

            //Sub objects
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
            this.bepuScene.OnUpdated += BepuScene_OnUpdated;
            this.eventManager = eventManager;

            sceneObject = new SceneObject()
            {
                vertexBuffer = plane.VertexBuffer,
                skinVertexBuffer = plane.SkinVertexBuffer,
                indexBuffer = plane.IndexBuffer,
                numIndices = plane.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                position = description.Translation,
                orientation = description.Orientation,
                scale = description.Scale,
                RenderShadow = true,
                Sprite = sprite,
            };

            Sprite_FrameChanged(sprite);

            //Character Mover
            var shape = new Box(1, 1, 1); //Each character creates a shape, try to load from resources somehow
            shapeIndex = bepuScene.Simulation.Shapes.Add(shape);

            var moverDesc = new CharacterMoverDescription()
            {
                MinimumSupportDepth = shape.HalfHeight * -0.01f
            };

            //Because characters are dynamic, they require a defined BodyInertia. For the purposes of the demos, we don't want them to rotate or fall over, so the inverse inertia tensor is left at its default value of all zeroes.
            //This is effectively equivalent to giving it an infinite inertia tensor- in other words, no torque will cause it to rotate.
            var mass = 1f;
            var bodyDesc = 
                BodyDescription.CreateDynamic(description.Translation.ToSystemNumerics(), new BodyInertia { InverseMass = 1f / mass },
                new CollidableDescription(shapeIndex, moverDesc.SpeculativeMargin),
                new BodyActivityDescription(shape.HalfHeight * 0.02f));

            characterMover = bepuScene.CreateCharacterMover(bodyDesc, moverDesc);

            IEnumerator<YieldAction> co()
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until coroutine is finished and this is disposed.

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

                    if (disposed)
                    {
                        spriteMaterialManager.Return(spriteMaterial);
                    }
                    else
                    {
                        sceneObject.shaderResourceBinding = spriteMaterial.ShaderResourceBinding;
                    }
                });

                if (!destructionRequest.DestructionRequested)
                {
                    sprites.Add(sprite);
                    sceneObjectManager.Add(sceneObject);
                }
            }
            coroutine.Run(co());
        }

        internal void RequestDestruction()
        {
            destructionRequest.RequestDestruction();
        }

        private void BepuScene_OnUpdated(IBepuScene obj)
        {
            var body = bepuScene.Simulation.Bodies.GetBodyReference(characterMover.BodyHandle);
            var pose = body.Pose;
            var bodPos = pose.Position;
            this.sceneObject.position = new Vector3(bodPos.X, bodPos.Y, bodPos.Z);
            var bodOrientation = pose.Orientation;
            this.sceneObject.orientation = new Quaternion(bodOrientation.X, bodOrientation.Y, bodOrientation.Z, bodOrientation.W);
            Sprite_FrameChanged(sprite);
        }

        private void EventLayer_OnUpdate(EventLayer eventLayer)
        {
            if (eventLayer.EventProcessingAllowed && allowJoystickInput)
            {
                var pad = eventLayer.getGamepad(gamepadId);
                characterMover.movementDirection = pad.LStick.ToSystemNumerics();
            }
        }

        public void Dispose()
        {
            disposed = true;
            eventManager.removeEvent(moveForward);
            eventManager.removeEvent(moveBackward);
            eventManager.removeEvent(moveLeft);
            eventManager.removeEvent(moveRight);
            eventManager.removeEvent(sprint);
            eventManager.removeEvent(jump);

            eventLayer.OnUpdate -= EventLayer_OnUpdate; //Do have to remove this since its on the layer itself

            this.bepuScene.OnUpdated -= BepuScene_OnUpdated;
            bepuScene.DestroyCharacterMover(characterMover);
            bepuScene.Simulation.Shapes.Remove(shapeIndex);
            sprite.FrameChanged -= Sprite_FrameChanged;
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.TryReturn(spriteMaterial);
            objectResolver.Dispose();
        }

        private void Sprite_FrameChanged(FrameEventSprite obj)
        {
            var frame = obj.GetCurrentFrame();

            var scale = sprite.BaseScale * this.sceneObject.scale;
            {
                var primaryAttach = frame.Attachments[this.primaryHand];
                var offset = scale * primaryAttach.translate;
                offset = Quaternion.quatRotate(ref this.sceneObject.orientation, ref offset) + this.sceneObject.position;
                sword.SetPosition(ref offset, ref this.sceneObject.orientation, ref scale);
            }

            {
                var secondaryAttach = frame.Attachments[this.secondaryHand];
                var offset = scale * secondaryAttach.translate;
                offset = Quaternion.quatRotate(ref this.sceneObject.orientation, ref offset) + this.sceneObject.position;
                shield.SetPosition(ref offset, ref this.sceneObject.orientation, ref scale);
            }
        }
    }
}
