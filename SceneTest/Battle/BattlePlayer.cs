using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using Engine.Platform;
using RpgMath;
using SceneTest.Sprites;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class BattlePlayer : IDisposable, IBattleTarget
    {
        private readonly PlayerSprite playerSpriteInfo;
        private readonly SceneObjectManager<IBattleManager> sceneObjectManager;
        private readonly SpriteManager sprites;
        private readonly IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private readonly IScopedCoroutine coroutine;
        private readonly IScaleHelper scaleHelper;
        private readonly IScreenPositioner screenPositioner;
        private readonly BattleManager battleManager;
        private readonly SceneObject sceneObject;
        private readonly IObjectResolver objectResolver;
        private ISpriteMaterial spriteMaterial;
        private CharacterSheet characterSheet;

        private bool disposed = false;
        private int primaryHand;
        private int secondaryHand;
        private GamepadId gamepadId;
        private FrameEventSprite sprite;

        private Attachment<IBattleManager> sword;
        private Attachment<IBattleManager> shield;

        private SharpButton attackButton = new SharpButton() { Text = "Attack" };
        private SharpButton magicButton = new SharpButton() { Text = "Magic" };
        private SharpButton itemButton = new SharpButton() { Text = "Item" };
        private SharpButton defendButton = new SharpButton() { Text = "Defend" };

        public IBattleStats Stats => this.characterSheet;

        public Vector3 DamageDisplayLocation => this.sceneObject.position;

        public Vector3 CursorDisplayLocation => this.sceneObject.position;

        public Vector3 MeleeAttackLocation => this.sceneObject.position - new Vector3(sprite.BaseScale.x, 0, 0);

        public BattleTargetType BattleTargetType => BattleTargetType.Player;

        public bool IsDead => false;

        private Vector3 startPosition;

        public class Description : SceneObjectDesc
        {
            public int PrimaryHand = Player.RightHand;
            public int SecondaryHand = Player.LeftHand;
            public EventLayers EventLayer = EventLayers.Battle;
            public GamepadId Gamepad = GamepadId.Pad1;
            public BattleManager BattleManager;
            public CharacterSheet CharacterSheet;
        }

        public BattlePlayer(PlayerSprite playerSpriteInfo,
            SceneObjectManager<IBattleManager> sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            Description description,
            ISpriteMaterialManager spriteMaterialManager,
            IObjectResolverFactory objectResolverFactory,
            IScopedCoroutine coroutine,
            IScaleHelper scaleHelper,
            IScreenPositioner screenPositioner)
        {
            this.characterSheet = description.CharacterSheet ?? throw new InvalidOperationException("You must include a character sheet in the description");
            this.playerSpriteInfo = playerSpriteInfo;
            this.sceneObjectManager = sceneObjectManager;
            this.sprites = sprites;
            this.destructionRequest = destructionRequest;
            this.spriteMaterialManager = spriteMaterialManager;
            this.coroutine = coroutine;
            this.scaleHelper = scaleHelper;
            this.screenPositioner = screenPositioner;
            this.battleManager = description.BattleManager ?? throw new InvalidOperationException("You must include a battle manager in the description");
            this.primaryHand = description.PrimaryHand;
            this.secondaryHand = description.SecondaryHand;
            this.gamepadId = description.Gamepad;
            this.objectResolver = objectResolverFactory.Create();

            sprite = new FrameEventSprite(playerSpriteInfo.Animations);
            sprite.FrameChanged += Sprite_FrameChanged;
            sprite.SetAnimation("stand-left");

            var scale = description.Scale * sprite.BaseScale;
            var halfScale = scale.y / 2f;
            var startPos = description.Translation;
            startPos.y += halfScale;

            sword = objectResolver.Resolve<Attachment<IBattleManager>, Attachment<IBattleManager>.Description>(o =>
            {
                o.Orientation = new Quaternion(0, MathF.PI / 4f, 0);
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

            shield = objectResolver.Resolve<Attachment<IBattleManager>, Attachment<IBattleManager>.Description>(o =>
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

            this.startPosition = startPos;
            sceneObject = new SceneObject()
            {
                vertexBuffer = plane.VertexBuffer,
                skinVertexBuffer = plane.SkinVertexBuffer,
                indexBuffer = plane.IndexBuffer,
                numIndices = plane.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                position = startPos,
                orientation = description.Orientation,
                scale = scale,
                RenderShadow = true,
                Sprite = sprite,
            };

            Sprite_FrameChanged(sprite);

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until coroutine is finished and this is disposed.

                spriteMaterial = await this.spriteMaterialManager.Checkout(playerSpriteInfo.SpriteMaterialDescription);

                if (disposed)
                {
                    spriteMaterialManager.Return(spriteMaterial);
                    return; //Stop loading
                }

                if (!destructionRequest.DestructionRequested)
                {
                    sceneObject.shaderResourceBinding = spriteMaterial.ShaderResourceBinding;
                    sprites.Add(sprite);
                    sceneObjectManager.Add(sceneObject);
                }
            });
        }

        private bool guiActive = false;
        internal void SetGuiActive(bool active)
        {
            if (guiActive != active)
            {
                guiActive = active;
                if (guiActive)
                {
                    this.sceneObject.position = this.startPosition + new Vector3(-1f, 0f, 0f);
                }
                else
                {
                    this.sceneObject.position = this.startPosition;
                }
                Sprite_FrameChanged(sprite);
            }
        }

        public void Dispose()
        {
            sprite.FrameChanged -= Sprite_FrameChanged;
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.TryReturn(spriteMaterial);
            objectResolver.Dispose();
        }

        public bool UpdateGui(ISharpGui sharpGui)
        {
            bool didSomething = false;
            var layout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                new ColumnLayout(attackButton, magicButton, itemButton, defendButton) { Margin = new IntPad(10) }
                ));
            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(screenPositioner.GetBottomRightRect(desiredSize));

            if (sharpGui.Button(attackButton))
            {
                coroutine.RunTask(async() =>
                {
                    var target = await battleManager.GetTarget();
                    if (target != null)
                    {
                        Attack(target);
                    }
                });
                didSomething = true;
            }

            if (sharpGui.Button(magicButton))
            {
                didSomething = true;
            }

            if (sharpGui.Button(itemButton))
            {
                didSomething = true;
            }

            if (sharpGui.Button(defendButton))
            {
                didSomething = true;
            }

            return didSomething;
        }

        private void Attack(IBattleTarget target)
        {
            var swingEnd = Quaternion.Identity;
            var swingStart = new Quaternion(0f, MathF.PI / 2.1f, 0f);

            long remainingTime = (long)(1.8f * Clock.SecondsToMicro);
            long standTime = (long)(0.2f * Clock.SecondsToMicro);
            long standStartTime = remainingTime / 2;
            long swingTime = standStartTime - standTime / 3;
            long standEndTime = standStartTime - standTime;
            bool needsAttack = true;
            battleManager.QueueTurn(c =>
            {
                var done = false;
                remainingTime -= c.DeltaTimeMicro;
                Vector3 start;
                Vector3 end;
                float interpolate;

                if (remainingTime > standStartTime)
                {
                    sprite.SetAnimation("left");
                    target = battleManager.ValidateTarget(this, target);
                    start = this.startPosition;
                    end = target.MeleeAttackLocation;
                    interpolate = (remainingTime - standStartTime) / (float)standStartTime;
                }
                else if(remainingTime > standEndTime)
                {
                    var slerpAmount = (remainingTime - standEndTime) / (float)standEndTime;
                    sword.SetAdditionalRotation(swingStart.slerp(swingEnd, slerpAmount));
                    sprite.SetAnimation("stand-left");
                    interpolate = 0.0f;
                    start = target.MeleeAttackLocation;
                    end = target.MeleeAttackLocation;

                    if(needsAttack && remainingTime < swingTime)
                    {
                        needsAttack = false;
                        battleManager.Attack(this, target);
                    }
                }
                else
                {
                    sprite.SetAnimation("right");

                    sword.SetAdditionalRotation(Quaternion.Identity);

                    start = target.MeleeAttackLocation;
                    end = this.startPosition;
                    interpolate = remainingTime / (float)standEndTime;
                }
                
                this.sceneObject.position = end.lerp(start, interpolate);

                if (remainingTime < 0)
                {
                    sprite.SetAnimation("stand-left");
                    battleManager.TurnComplete(this);
                    done = true;
                }

                Sprite_FrameChanged(sprite);

                return done;
            });
        }

        public void RequestDestruction()
        {
            destructionRequest.RequestDestruction();
        }

        private void Sprite_FrameChanged(FrameEventSprite obj)
        {
            var frame = obj.GetCurrentFrame();

            var scale = sprite.BaseScale * this.sceneObject.scale;
            {
                var primaryAttach = frame.Attachments[this.primaryHand];
                var offset = scale * primaryAttach.translate;
                offset = Quaternion.quatRotate(this.sceneObject.orientation, offset) + this.sceneObject.position;
                sword.SetPosition(offset, this.sceneObject.orientation, scale);
            }

            {
                var secondaryAttach = frame.Attachments[this.secondaryHand];
                var offset = scale * secondaryAttach.translate;
                offset = Quaternion.quatRotate(this.sceneObject.orientation, offset) + this.sceneObject.position;
                shield.SetPosition(offset, this.sceneObject.orientation, scale);
            }
        }

        public void ApplyDamage(IDamageCalculator calculator, long damage)
        {
            
        }
    }
}
