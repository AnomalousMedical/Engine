using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using Engine.Platform;
using RpgMath;
using SceneTest.Assets;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Battle
{
    class BattlePlayer : IDisposable, IBattleTarget
    {
        private readonly IPlayerSprite playerSpriteInfo;
        private readonly SceneObjectManager<IBattleManager> sceneObjectManager;
        private readonly SpriteManager sprites;
        private readonly IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private readonly IScopedCoroutine coroutine;
        private readonly IScaleHelper scaleHelper;
        private readonly IBattleScreenLayout battleScreenLayout;
        private readonly ICharacterTimer characterTimer;
        private readonly IBattleManager battleManager;
        private readonly ITurnTimer turnTimer;
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
        private Attachment<IBattleManager> castEffect;

        private SharpButton attackButton = new SharpButton() { Text = "Attack" };
        private SharpButton magicButton = new SharpButton() { Text = "Magic" };
        private SharpButton itemButton = new SharpButton() { Text = "Item" };
        private SharpButton defendButton = new SharpButton() { Text = "Defend" };

        private SharpProgressHorizontal turnProgress = new SharpProgressHorizontal();
        private SharpText name = new SharpText() { Color = Color.White };
        private SharpText currentHp = new SharpText() { Color = Color.White };
        private SharpText currentMp = new SharpText() { Color = Color.White };
        private ILayoutItem infoRowLayout;

        private IMagicAbilities magicAbilities;

        public IBattleStats Stats => this.characterSheet;

        public Vector3 DamageDisplayLocation => this.sceneObject.position;

        public Vector3 CursorDisplayLocation => this.sceneObject.position + new Vector3(-0.5f * sceneObject.scale.x, 0.5f * sceneObject.scale.y, 0f);

        public Vector3 MeleeAttackLocation => this.sceneObject.position - new Vector3(sprite.BaseScale.x, 0, 0);

        public Vector3 MagicHitLocation => this.sceneObject.position + new Vector3(0f, 0f, -0.1f);

        public BattleTargetType BattleTargetType => BattleTargetType.Player;

        public ICharacterTimer CharacterTimer => characterTimer;

        public bool IsDead => characterSheet.CurrentHp == 0;

        public int BaseDexterity { get; internal set; }

        private Vector3 startPosition;

        public class Description : SceneObjectDesc
        {
            public int PrimaryHand = Player.RightHand;
            public int SecondaryHand = Player.LeftHand;
            public EventLayers EventLayer = EventLayers.Battle;
            public GamepadId Gamepad = GamepadId.Pad1;
            public CharacterSheet CharacterSheet;
            public IPlayerSprite PlayerSpriteInfo { get; set; }
            public ISpriteAsset PrimaryHandItem { get; set; }
            public ISpriteAsset SecondaryHandItem { get; set; }
        }

        public BattlePlayer(
            SceneObjectManager<IBattleManager> sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            Description description,
            ISpriteMaterialManager spriteMaterialManager,
            IObjectResolverFactory objectResolverFactory,
            IScopedCoroutine coroutine,
            IScaleHelper scaleHelper,
            IBattleScreenLayout battleScreenLayout,
            ICharacterTimer characterTimer,
            IBattleManager battleManager,
            ITurnTimer turnTimer,
            IMagicAbilities magicAbilities)
        {
            this.characterSheet = description.CharacterSheet ?? throw new InvalidOperationException("You must include a character sheet in the description");
            this.playerSpriteInfo = description.PlayerSpriteInfo ?? throw new InvalidOperationException($"You must include the {nameof(description.PlayerSpriteInfo)} property in your description.");
            this.magicAbilities = magicAbilities;
            this.sceneObjectManager = sceneObjectManager;
            this.sprites = sprites;
            this.destructionRequest = destructionRequest;
            this.spriteMaterialManager = spriteMaterialManager;
            this.coroutine = coroutine;
            this.scaleHelper = scaleHelper;
            this.battleScreenLayout = battleScreenLayout;
            this.characterTimer = characterTimer;
            this.battleManager = battleManager;
            this.turnTimer = turnTimer;
            this.primaryHand = description.PrimaryHand;
            this.secondaryHand = description.SecondaryHand;
            this.gamepadId = description.Gamepad;
            this.objectResolver = objectResolverFactory.Create();

            turnProgress.DesiredSize = scaleHelper.Scaled(new IntSize2(200, 25));
            infoRowLayout = new RowLayout(
                new FixedWidthLayout(scaleHelper.Scaled(240), name),
                new FixedWidthLayout(scaleHelper.Scaled(85), currentHp),
                new FixedWidthLayout(scaleHelper.Scaled(70), currentMp),
                new FixedWidthLayout(scaleHelper.Scaled(210), turnProgress));
            battleScreenLayout.InfoColumn.Add(infoRowLayout);

            name.Text = description.CharacterSheet.Name;
            currentHp.Text = description.CharacterSheet.CurrentHp.ToString();
            currentMp.Text = description.CharacterSheet.CurrentMp.ToString();

            turnTimer.AddTimer(characterTimer);
            characterTimer.TurnReady += CharacterTimer_TurnReady;
            characterTimer.TotalDex = characterSheet.Dexterity;

            sprite = new FrameEventSprite(playerSpriteInfo.Animations);
            sprite.FrameChanged += Sprite_FrameChanged;
            sprite.SetAnimation("stand-left");

            var scale = description.Scale * sprite.BaseScale;
            var halfScale = scale.y / 2f;
            var startPos = description.Translation;
            startPos.y += halfScale;

            if (description.PrimaryHandItem != null)
            {
                sword = objectResolver.Resolve<Attachment<IBattleManager>, Attachment<IBattleManager>.Description>(o =>
                {
                    var asset = description.PrimaryHandItem;
                    o.Orientation = asset.GetOrientation();
                    o.Sprite = asset.CreateSprite();
                    o.SpriteMaterial = asset.CreateMaterial();
                });
            }

            if (description.SecondaryHandItem != null)
            {
                shield = objectResolver.Resolve<Attachment<IBattleManager>, Attachment<IBattleManager>.Description>(o =>
                {
                    var asset = description.SecondaryHandItem;
                    o.Orientation = asset.GetOrientation();
                    o.Sprite = asset.CreateSprite();
                    o.SpriteMaterial = asset.CreateMaterial();
                });
            }

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
            turnTimer.RemoveTimer(characterTimer);
            battleScreenLayout.InfoColumn.Remove(infoRowLayout);
            characterTimer.TurnReady -= CharacterTimer_TurnReady;
            sprite.FrameChanged -= Sprite_FrameChanged;
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.TryReturn(spriteMaterial);
            objectResolver.Dispose();
        }

        public void DrawInfoGui(Clock clock, ISharpGui sharpGui)
        {
            sharpGui.Text(name);
            sharpGui.Text(currentHp);
            sharpGui.Text(currentMp);
            sharpGui.Progress(turnProgress, characterTimer.TurnTimerPct);
        }

        public enum MenuMode
        {
            Root,
            Magic
        }

        private MenuMode currentMenuMode = MenuMode.Root;

        public bool UpdateActivePlayerGui(ISharpGui sharpGui)
        {
            bool didSomething = false;

            switch (currentMenuMode)
            {
                case MenuMode.Root:
                    didSomething = UpdateRootMenu(sharpGui, didSomething);
                    break;
                case MenuMode.Magic:
                    didSomething = magicAbilities.UpdateGui(sharpGui, coroutine, ref currentMenuMode, Cast);
                    break;
            }

            return didSomething;
        }

        private bool UpdateRootMenu(ISharpGui sharpGui, bool didSomething)
        {
            battleScreenLayout.LayoutBattleMenu(attackButton, magicButton, itemButton, defendButton);

            if (sharpGui.Button(attackButton, navUp: defendButton.Id, navDown: magicButton.Id))
            {
                coroutine.RunTask(async () =>
                {
                    var target = await battleManager.GetTarget(false);
                    if (target != null)
                    {
                        Attack(target);
                    }
                });
                didSomething = true;
            }

            if (sharpGui.Button(magicButton, navUp: attackButton.Id, navDown: itemButton.Id))
            {
                currentMenuMode = MenuMode.Magic;
                didSomething = true;
            }

            if (sharpGui.Button(itemButton, navUp: magicButton.Id, navDown: defendButton.Id))
            {
                didSomething = true;
            }

            if (sharpGui.Button(defendButton, navUp: itemButton.Id, navDown: attackButton.Id))
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
            battleManager.DeactivateCurrentPlayer();
            battleManager.QueueTurn(c =>
            {
                if (IsDead)
                {
                    return true;
                }

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
                else if (remainingTime > standEndTime)
                {
                    var slerpAmount = (remainingTime - standEndTime) / (float)standEndTime;
                    sword?.SetAdditionalRotation(swingStart.slerp(swingEnd, slerpAmount));
                    sprite.SetAnimation("stand-left");
                    interpolate = 0.0f;
                    start = target.MeleeAttackLocation;
                    end = target.MeleeAttackLocation;

                    if (needsAttack && remainingTime < swingTime)
                    {
                        needsAttack = false;
                        battleManager.Attack(this, target);
                    }
                }
                else
                {
                    sprite.SetAnimation("right");

                    sword?.SetAdditionalRotation(Quaternion.Identity);

                    start = target.MeleeAttackLocation;
                    end = this.startPosition;
                    interpolate = remainingTime / (float)standEndTime;
                }

                this.sceneObject.position = end.lerp(start, interpolate);

                if (remainingTime < 0)
                {
                    sprite.SetAnimation("stand-left");
                    TurnComplete();
                    done = true;
                }

                Sprite_FrameChanged(sprite);

                return done;
            });
        }

        private void Cast(IBattleTarget target, ISpell spell)
        {
            castEffect?.RequestDestruction();
            castEffect = objectResolver.Resolve<Attachment<IBattleManager>, Attachment<IBattleManager>.Description>(o =>
            {
                ISpriteAsset asset = new Assets.PixelEffects.Nebula();
                o.RenderShadow = false;
                o.Sprite = asset.CreateSprite();
                o.SpriteMaterial = asset.CreateMaterial();
            });

            var swingEnd = Quaternion.Identity;
            var swingStart = new Quaternion(0f, MathF.PI / 2.1f, 0f);

            long remainingTime = (long)(1.8f * Clock.SecondsToMicro);
            long standTime = (long)(0.2f * Clock.SecondsToMicro);
            long standStartTime = remainingTime / 2;
            long swingTime = standStartTime - standTime / 3;
            long standEndTime = standStartTime - standTime;
            bool needsAttack = true;
            battleManager.DeactivateCurrentPlayer();
            battleManager.QueueTurn(c =>
            {
                if (IsDead)
                {
                    return true;
                }

                var done = false;
                remainingTime -= c.DeltaTimeMicro;
                Vector3 start;
                Vector3 end;
                float interpolate;

                if (remainingTime > standStartTime)
                {
                    sprite.SetAnimation("stand-left");
                    target = battleManager.ValidateTarget(this, target);
                    start = this.startPosition;
                    end = target.MeleeAttackLocation;
                    interpolate = (remainingTime - standStartTime) / (float)standStartTime;
                }
                else if (remainingTime > standEndTime)
                {
                    var slerpAmount = (remainingTime - standEndTime) / (float)standEndTime;
                    //sword?.SetAdditionalRotation(swingStart.slerp(swingEnd, slerpAmount));
                    sprite.SetAnimation("cast-left");
                    interpolate = 0.0f;
                    start = target.MeleeAttackLocation;
                    end = target.MeleeAttackLocation;

                    if (needsAttack && remainingTime < swingTime)
                    {
                        needsAttack = false;
                        spell.Apply(battleManager, objectResolver, coroutine, this, target);

                        DestroyCastEffect();
                    }
                }
                else
                {
                    sprite.SetAnimation("stand-left");

                    sword?.SetAdditionalRotation(Quaternion.Identity);

                    start = target.MeleeAttackLocation;
                    end = this.startPosition;
                    interpolate = remainingTime / (float)standEndTime;
                }

                if (remainingTime < 0)
                {
                    sprite.SetAnimation("stand-left");
                    TurnComplete();
                    done = true;
                }

                Sprite_FrameChanged(sprite);

                return done;
            });
        }

        private void DestroyCastEffect()
        {
            castEffect?.RequestDestruction();
            castEffect = null;
        }

        private void CharacterTimer_TurnReady(ICharacterTimer timer)
        {
            battleManager.AddToActivePlayers(this);
        }

        private void TurnComplete()
        {
            characterTimer.Reset();
            characterTimer.TurnTimerActive = true;
        }

        public void RequestDestruction()
        {
            destructionRequest.RequestDestruction();
        }

        private void Sprite_FrameChanged(FrameEventSprite obj)
        {
            var frame = obj.GetCurrentFrame();

            var scale = sprite.BaseScale * this.sceneObject.scale;

            if(sword != null)
            {
                var primaryAttach = frame.Attachments[this.primaryHand];
                var offset = scale * primaryAttach.translate;
                offset = Quaternion.quatRotate(this.sceneObject.orientation, offset) + this.sceneObject.position;
                sword.SetPosition(offset, this.sceneObject.orientation, scale);
            }

            if(shield != null)
            {
                var secondaryAttach = frame.Attachments[this.secondaryHand];
                var offset = scale * secondaryAttach.translate;
                offset = Quaternion.quatRotate(this.sceneObject.orientation, offset) + this.sceneObject.position;
                shield.SetPosition(offset, this.sceneObject.orientation, scale);
            }

            if(castEffect != null)
            {
                var secondaryAttach = frame.Attachments[this.secondaryHand];
                var offset = scale * secondaryAttach.translate;
                offset = Quaternion.quatRotate(this.sceneObject.orientation, offset) + this.sceneObject.position;
                castEffect.SetPosition(offset, this.sceneObject.orientation, scale);
            }
        }

        public void ApplyDamage(IDamageCalculator calculator, long damage)
        {
            if (IsDead) { return; } //Do nothing if dead

            characterSheet.CurrentHp = calculator.ApplyDamage(damage, characterSheet.CurrentHp, characterSheet.Hp);
            currentHp.UpdateText(characterSheet.CurrentHp.ToString());

            //Player died from applied damage
            if (IsDead)
            {
                battleManager.PlayerDead(this);
                characterTimer.TurnTimerActive = false;
                characterTimer.Reset();
            }
        }
    }
}
