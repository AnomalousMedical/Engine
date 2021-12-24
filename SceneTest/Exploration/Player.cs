using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin;
using BepuPlugin.Characters;
using DiligentEngine;
using DiligentEngine.RT;
using DiligentEngine.RT.Sprites;
using Engine;
using Engine.Platform;
using SceneTest.Assets;
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
            public EventLayers EventLayer = EventLayers.Exploration;
            public GamepadId Gamepad = GamepadId.Pad1;
            public IPlayerSprite PlayerSpriteInfo { get; set; }
            public ISpriteAsset PrimaryHandItem { get; set; }
            public ISpriteAsset SecondaryHandItem { get; set; }
        }

        public const int RightHand = 0;
        public const int LeftHand = 1;

        private readonly RTInstances<ILevelManager> rtInstances;
        private readonly TLASBuildInstanceData tlasData;
        private readonly IDestructionRequest destructionRequest;
        private readonly SpriteInstanceFactory spriteInstanceFactory;
        private readonly IBepuScene bepuScene;
        private readonly EventManager eventManager;
        private readonly CameraMover cameraMover;
        private readonly ICollidableTypeIdentifier collidableIdentifier;
        private readonly EventLayer eventLayer;
        private readonly IObjectResolver objectResolver;

        private FrameEventSprite sprite;
        private SpriteInstance spriteInstance;

        private Attachment<ILevelManager> sword;
        private Attachment<ILevelManager> shield;

        private CharacterMover characterMover;
        private TypedIndex shapeIndex;

        private int primaryHand;
        private int secondaryHand;
        private GamepadId gamepadId;
        private bool allowJoystickInput = true;

        ButtonEvent moveForward;
        ButtonEvent moveBackward;
        ButtonEvent moveRight;
        ButtonEvent moveLeft;
        ButtonEvent sprint;
        ButtonEvent jump;

        private bool disposed;
        private Vector3 cameraOffset = new Vector3(0, 3, -12);
        private Quaternion cameraAngle = new Quaternion(Vector3.Left, -MathF.PI / 14f);

        private Vector3 currentPosition;
        private Quaternion currentOrientation;
        private Vector3 currentScale;

        private System.Numerics.Vector2 movementDir;
        private const float MovingBoundary = 0.001f;
        public bool IsMoving => !(movementDir.X < MovingBoundary && movementDir.X > -MovingBoundary
                             && movementDir.Y < MovingBoundary && movementDir.Y > -MovingBoundary);

        public Player
        (
            RTInstances<ILevelManager> rtInstances,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            SpriteInstanceFactory spriteInstanceFactory,
            IObjectResolverFactory objectResolverFactory,
            IBepuScene bepuScene,
            EventManager eventManager,
            Description description,
            CameraMover cameraMover,
            ICollidableTypeIdentifier collidableIdentifier
        )
        {
            var playerSpriteInfo = description.PlayerSpriteInfo ?? throw new InvalidOperationException($"You must include the {nameof(description.PlayerSpriteInfo)} property in your description.");

            this.moveForward = new ButtonEvent(description.EventLayer, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_W });
            this.moveBackward = new ButtonEvent(description.EventLayer, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_S });
            this.moveRight = new ButtonEvent(description.EventLayer, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_D });
            this.moveLeft = new ButtonEvent(description.EventLayer, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_A });
            this.sprint = new ButtonEvent(description.EventLayer, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_LSHIFT });
            this.jump = new ButtonEvent(description.EventLayer, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_SPACE });

            this.primaryHand = description.PrimaryHand;
            this.secondaryHand = description.SecondaryHand;
            this.gamepadId = description.Gamepad;

            sprite = new FrameEventSprite(playerSpriteInfo.Animations);

            //Events
            eventManager.addEvent(moveForward);
            eventManager.addEvent(moveBackward);
            eventManager.addEvent(moveLeft);
            eventManager.addEvent(moveRight);
            eventManager.addEvent(sprint);
            eventManager.addEvent(jump);

            eventLayer = eventManager[description.EventLayer];
            eventLayer.OnUpdate += EventLayer_OnUpdate;

            SetupInput();

            //Sub objects
            objectResolver = objectResolverFactory.Create();

            if (description.PrimaryHandItem != null)
            {
                sword = objectResolver.Resolve<Attachment<ILevelManager>, Attachment<ILevelManager>.Description>(o =>
                {
                    var asset = description.PrimaryHandItem;
                    o.Orientation = asset.GetOrientation();
                    o.Sprite = asset.CreateSprite();
                    o.SpriteMaterial = asset.CreateMaterial();
                });
            }

            if (description.SecondaryHandItem != null)
            {
                shield = objectResolver.Resolve<Attachment<ILevelManager>, Attachment<ILevelManager>.Description>(o =>
                {
                    var asset = description.SecondaryHandItem;
                    o.Orientation = asset.GetOrientation();
                    o.Sprite = asset.CreateSprite();
                    o.SpriteMaterial = asset.CreateMaterial();
                });
            }

            sprite.FrameChanged += Sprite_FrameChanged;

            this.rtInstances = rtInstances;
            this.destructionRequest = destructionRequest;
            this.spriteInstanceFactory = spriteInstanceFactory;
            this.bepuScene = bepuScene;
            this.bepuScene.OnUpdated += BepuScene_OnUpdated;
            this.eventManager = eventManager;
            this.cameraMover = cameraMover;
            this.collidableIdentifier = collidableIdentifier;
            var scale = description.Scale * sprite.BaseScale;
            var halfScale = scale.y / 2f;
            var startPos = description.Translation;
            startPos.y += halfScale;

            this.currentPosition = startPos;
            this.currentOrientation = description.Orientation;
            this.currentScale = scale;

            this.tlasData = new TLASBuildInstanceData()
            {
                InstanceName = Guid.NewGuid().ToString("N"),
                Mask = RtStructures.OPAQUE_GEOM_MASK,
                Transform = new InstanceMatrix(currentPosition, currentOrientation, currentScale)
            };

            Sprite_FrameChanged(sprite);

            //Character Mover
            var shape = new Sphere(halfScale); //Each character creates a shape, try to load from resources somehow
            shapeIndex = bepuScene.Simulation.Shapes.Add(shape);

            var moverDesc = new CharacterMoverDescription()
            {
                MinimumSupportDepth = shape.Radius * -0.01f,

            };

            //Because characters are dynamic, they require a defined BodyInertia. For the purposes of the demos, we don't want them to rotate or fall over, so the inverse inertia tensor is left at its default value of all zeroes.
            //This is effectively equivalent to giving it an infinite inertia tensor- in other words, no torque will cause it to rotate.
            var mass = 1f;
            var bodyDesc =
                BodyDescription.CreateDynamic(startPos.ToSystemNumerics(), new BodyInertia { InverseMass = 1f / mass },
                new CollidableDescription(shapeIndex, moverDesc.SpeculativeMargin),
                new BodyActivityDescription(shape.Radius * 0.02f));

            characterMover = bepuScene.CreateCharacterMover(bodyDesc, moverDesc);
            characterMover.sprint = true;
            bepuScene.AddToInterpolation(characterMover.BodyHandle);
            collidableIdentifier.AddIdentifier(new CollidableReference(CollidableMobility.Dynamic, characterMover.BodyHandle), this);

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until coroutine is finished and this is disposed.

                this.spriteInstance = await spriteInstanceFactory.Checkout(playerSpriteInfo.SpriteMaterialDescription);

                if (this.disposed)
                {
                    this.spriteInstanceFactory.TryReturn(spriteInstance);
                    return; //Stop loading
                }

                if (!destructionRequest.DestructionRequested)
                {
                    this.tlasData.pBLAS = spriteInstance.Instance.BLAS.Obj;
                    rtInstances.AddTlasBuild(tlasData);
                    rtInstances.AddShaderTableBinder(Bind);
                    rtInstances.AddSprite(sprite);
                }
            });
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
            collidableIdentifier.RemoveIdentifier(new CollidableReference(CollidableMobility.Dynamic, characterMover.BodyHandle));
            bepuScene.RemoveFromInterpolation(characterMover.BodyHandle);
            bepuScene.DestroyCharacterMover(characterMover);
            bepuScene.Simulation.Shapes.Remove(shapeIndex);
            sprite.FrameChanged -= Sprite_FrameChanged;
            this.spriteInstanceFactory.TryReturn(spriteInstance);
            rtInstances.RemoveSprite(sprite);
            rtInstances.RemoveShaderTableBinder(Bind);
            rtInstances.RemoveTlasBuild(tlasData);
            objectResolver.Dispose();
        }

        public void StopMovement()
        {
            characterMover.movementDirection.X = 0;
            characterMover.movementDirection.Y = 0;
        }

        private void SetupInput()
        {
            //These events are owned by this class, so don't have to unsubscribe
            moveForward.FirstFrameDownEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    characterMover.movementDirection.Y = 1;
                    l.alertEventsHandled();
                    allowJoystickInput = false;
                    this.sprite.SetAnimation("up");
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
                    this.sprite.SetAnimation("down");
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
                    this.sprite.SetAnimation("left");
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
                    this.sprite.SetAnimation("right");
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
                    characterMover.sprint = false;
                    l.alertEventsHandled();
                }
            };
            sprint.FirstFrameUpEvent += l =>
            {
                if (l.EventProcessingAllowed)
                {
                    characterMover.sprint = true;
                    l.alertEventsHandled();
                }
            };
        }

        public void SetLocation(in Vector3 location)
        {
            var finalLoc = location + new Vector3(0f, sprite.BaseScale.y / 2f, 0f);

            bepuScene.RemoveFromInterpolation(characterMover.BodyHandle);
            this.characterMover.SetLocation(finalLoc.ToSystemNumerics());
            bepuScene.AddToInterpolation(characterMover.BodyHandle);
        }

        public Vector3 GetLocation()
        {
            return this.currentPosition - new Vector3(0f, sprite.BaseScale.y / 2f, 0f);
        }

        public void RequestDestruction()
        {
            destructionRequest.RequestDestruction();
        }

        private void BepuScene_OnUpdated(IBepuScene obj)
        {
            bepuScene.GetInterpolatedPosition(characterMover.BodyHandle, ref this.currentPosition, ref this.currentOrientation);
            this.tlasData.Transform = new InstanceMatrix(this.currentPosition, this.currentOrientation, this.currentScale);
            Sprite_FrameChanged(sprite);
            cameraMover.Position = this.currentPosition + cameraOffset;
            cameraMover.Orientation = cameraAngle;
            cameraMover.SceneCenter = this.currentPosition;

            var movementDir = characterMover.movementDirection;
            if (movementDir.Y > 0.3f)
            {
                sprite.SetAnimation("up");
            }
            else if (movementDir.Y < -0.3f)
            {
                sprite.SetAnimation("down");
            }
            else if (movementDir.X > 0)
            {
                sprite.SetAnimation("right");
            }
            else if (movementDir.X < 0)
            {
                sprite.SetAnimation("left");
            }
            this.movementDir = movementDir;
        }

        private void EventLayer_OnUpdate(EventLayer eventLayer)
        {
            if (eventLayer.EventProcessingAllowed && allowJoystickInput)
            {
                var pad = eventLayer.getGamepad(gamepadId);
                var movementDir = pad.LStick;
                characterMover.movementDirection = movementDir.ToSystemNumerics();
            }

            if(characterMover.movementDirection.X == 0 && characterMover.movementDirection.Y == 0)
            {
                switch (sprite.CurrentAnimationName)
                {
                    case "up":
                    case "down":
                    case "left":
                    case "right":
                        sprite.SetAnimation($"stand-{sprite.CurrentAnimationName}");
                        break;
                }
            }
        }

        private void Sprite_FrameChanged(FrameEventSprite obj)
        {
            var frame = obj.GetCurrentFrame();

            var scale = sprite.BaseScale * this.currentScale;

            if(sword != null)
            {
                var primaryAttach = frame.Attachments[this.primaryHand];
                var offset = scale * primaryAttach.translate;
                offset = Quaternion.quatRotate(this.currentOrientation, offset) + this.currentPosition;
                sword.SetPosition(offset, this.currentOrientation, scale);
            }

            if(shield != null)
            {
                var secondaryAttach = frame.Attachments[this.secondaryHand];
                var offset = scale * secondaryAttach.translate;
                offset = Quaternion.quatRotate(this.currentOrientation, offset) + this.currentPosition;
                shield.SetPosition(offset, this.currentOrientation, scale);
            }
        }

        private void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            spriteInstance.Bind(this.tlasData.InstanceName, sbt, tlas);
        }
    }
}
