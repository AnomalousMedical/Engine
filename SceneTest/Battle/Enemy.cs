using DiligentEngine;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using Engine.Platform;
using FreeImageAPI;
using RpgMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Enemy : IDisposable, IBattleTarget
    {
        public class Desc : SceneObjectDesc
        {
            public Sprite Sprite { get; set; }

            public SpriteMaterialDescription SpriteMaterial { get; set; }

            public BattleStats BattleStats { get; set; }

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
                desc.BattleStats = new BattleStats()
                {
                    Hp = 40,
                    Mp = 54,
                    Attack = 12,
                    AttackPercent = 100,
                    Defense = 10,
                    DefensePercent = 4,
                    MagicAttack = 2,
                    MagicDefensePercent = 0,
                    MagicDefense = 2,
                    Dexterity = 6,
                    Luck = 14,
                    Level = 1,
                };
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
                desc.BattleStats = new BattleStats()
                {
                    Hp = 40,
                    Mp = 54,
                    Attack = 12,
                    AttackPercent = 100,
                    Defense = 10,
                    DefensePercent = 4,
                    MagicAttack = 2,
                    MagicDefensePercent = 0,
                    MagicDefense = 2,
                    Dexterity = 6,
                    Luck = 14,
                    Level = 1,
                };
            }
        }

        private ISpriteMaterial spriteMaterial;
        private SceneObjectManager<IBattleManager> sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private SceneObject sceneObject;
        private Sprite sprite;
        private bool disposed;
        private readonly ICharacterTimer characterTimer;
        private readonly IBattleManager battleManager;
        private readonly ITurnTimer turnTimer;
        private Vector3 startPosition;

        public Vector3 MeleeAttackLocation => this.sceneObject.position + new Vector3(sprite.BaseScale.x, 0, 0);

        public ICharacterTimer CharacterTimer => characterTimer;

        private long currentHp;
        private long currentMp;

        public Enemy(
            SceneObjectManager<IBattleManager> sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ISpriteMaterialManager spriteMaterialManager,
            Desc description,
            ICharacterTimer characterTimer,
            IBattleManager battleManager,
            ITurnTimer turnTimer)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.sprites = sprites;
            this.destructionRequest = destructionRequest;
            this.spriteMaterialManager = spriteMaterialManager;
            this.characterTimer = characterTimer;
            this.battleManager = battleManager;
            this.turnTimer = turnTimer;
            this.sprite = description.Sprite;
            this.Stats = description.BattleStats ?? throw new InvalidOperationException("You must include battle stats in an enemy description.");
            this.currentHp = Stats.Hp;
            this.currentMp = Stats.Mp;

            turnTimer.AddTimer(characterTimer);
            characterTimer.TurnReady += CharacterTimer_TurnReady; ;
            characterTimer.TotalDex = Stats.Dexterity;

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

            this.startPosition = sceneObject.position;

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

        private void CharacterTimer_TurnReady(ICharacterTimer obj)
        {
            var swingEnd = Quaternion.Identity;
            var swingStart = new Quaternion(0f, MathF.PI / 2.1f, 0f);

            long remainingTime = (long)(1.8f * Clock.SecondsToMicro);
            long standTime = (long)(0.2f * Clock.SecondsToMicro);
            long standStartTime = remainingTime / 2;
            long swingTime = standStartTime - standTime / 3;
            long standEndTime = standStartTime - standTime;
            bool needsAttack = true;
            var target = battleManager.GetRandomPlayer();
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
                    //sprite.SetAnimation("left");
                    target = battleManager.ValidateTarget(this, target);
                    start = this.startPosition;
                    end = target.MeleeAttackLocation;
                    interpolate = (remainingTime - standStartTime) / (float)standStartTime;
                }
                else if (remainingTime > standEndTime)
                {
                    var slerpAmount = (remainingTime - standEndTime) / (float)standEndTime;
                    //sword.SetAdditionalRotation(swingStart.slerp(swingEnd, slerpAmount));
                    //sprite.SetAnimation("stand-left");
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
                    //sprite.SetAnimation("right");

                    //sword.SetAdditionalRotation(Quaternion.Identity);

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

                //Sprite_FrameChanged(sprite);

                return done;
            });
        }

        private void TurnComplete()
        {
            characterTimer.Reset();
            characterTimer.TurnTimerActive = true;
        }

        public void RequestDestruction()
        {
            this.destructionRequest.RequestDestruction();
        }

        public void Dispose()
        {
            turnTimer.RemoveTimer(characterTimer);
            disposed = true;
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.TryReturn(spriteMaterial);
        }

        public void ApplyDamage(IDamageCalculator calculator, long damage)
        {
            currentHp = calculator.ApplyDamage(damage, currentHp, Stats.Hp);
        }

        public Vector3 DamageDisplayLocation => sceneObject.position + new Vector3(0.5f * sceneObject.scale.x, 0.5f * sceneObject.scale.y, 0f);

        public IBattleStats Stats { get; }

        public Vector3 CursorDisplayLocation => DamageDisplayLocation;

        public bool IsDead => this.currentHp == 0;

        public BattleTargetType BattleTargetType => BattleTargetType.Enemy;
    }
}
