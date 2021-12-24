using DiligentEngine;
using DiligentEngine.RT;
using DiligentEngine.RT.Sprites;
using Engine;
using Engine.Platform;
using FreeImageAPI;
using RpgMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Battle
{
    class Enemy : IDisposable, IBattleTarget
    {
        public class Desc : SceneObjectDesc
        {
            public Sprite Sprite { get; set; }

            public SpriteMaterialDescription SpriteMaterial { get; set; }

            public BattleStats BattleStats { get; set; }

            public long XpReward { get; set; }

            public long GoldReward { get; set; }
        }

        private readonly RTInstances<IBattleManager> rtInstances;
        private readonly IDestructionRequest destructionRequest;
        private readonly SpriteInstanceFactory spriteInstanceFactory;
        private SpriteInstance spriteInstance;
        private readonly Sprite sprite;
        private readonly TLASBuildInstanceData tlasData;
        private bool disposed;
        private readonly ICharacterTimer characterTimer;
        private readonly IBattleManager battleManager;
        private readonly ITurnTimer turnTimer;

        private Vector3 startPosition;
        private Vector3 currentPosition;
        private Quaternion currentOrientation;
        private Vector3 currentScale;

        public Vector3 MeleeAttackLocation => this.currentPosition + new Vector3(sprite.BaseScale.x * currentScale.x, 0, 0);

        public Vector3 MagicHitLocation => this.currentPosition + new Vector3(0f, 0f, -0.1f);

        public ICharacterTimer CharacterTimer => characterTimer;

        private long currentHp;
        private long currentMp;

        public Enemy(
            RTInstances<IBattleManager> rtInstances,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            SpriteInstanceFactory spriteInstanceFactory,
            Desc description,
            ICharacterTimer characterTimer,
            IBattleManager battleManager,
            ITurnTimer turnTimer)
        {
            this.rtInstances = rtInstances;
            this.destructionRequest = destructionRequest;
            this.spriteInstanceFactory = spriteInstanceFactory;
            this.characterTimer = characterTimer;
            this.battleManager = battleManager;
            this.turnTimer = turnTimer;
            this.sprite = description.Sprite;
            this.Stats = description.BattleStats ?? throw new InvalidOperationException("You must include battle stats in an enemy description.");
            this.currentHp = Stats.Hp;
            this.currentMp = Stats.Mp;
            this.XpReward = description.XpReward;
            this.GoldReward = description.GoldReward;

            turnTimer.AddTimer(characterTimer);
            characterTimer.TurnReady += CharacterTimer_TurnReady;
            characterTimer.TotalDex = Stats.Dexterity;

            this.currentPosition = description.Translation;
            this.currentOrientation = description.Orientation;
            this.currentScale = sprite.BaseScale * description.Scale;
            this.startPosition = currentPosition;

            this.tlasData = new TLASBuildInstanceData()
            {
                InstanceName = Guid.NewGuid().ToString("N"),
                Mask = RtStructures.OPAQUE_GEOM_MASK,
                Transform = new InstanceMatrix(currentPosition, currentOrientation, currentScale)
            };

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until coroutine is finished and this is disposed.

                this.spriteInstance = await spriteInstanceFactory.Checkout(description.SpriteMaterial);

                if (this.disposed)
                {
                    this.spriteInstanceFactory.TryReturn(spriteInstance);
                    return; //Stop loading
                }

                if (!destructionRequest.DestructionRequested) //This is more to prevent a flash for 1 frame of the object
                {
                    this.tlasData.pBLAS = spriteInstance.Instance.BLAS.Obj;
                    rtInstances.AddTlasBuild(tlasData);
                    rtInstances.AddShaderTableBinder(Bind);
                    rtInstances.AddSprite(sprite);
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
                    end = GetAttackLocation(target);
                    interpolate = (remainingTime - standStartTime) / (float)standStartTime;
                }
                else if (remainingTime > standEndTime)
                {
                    var slerpAmount = (remainingTime - standEndTime) / (float)standEndTime;
                    //sword.SetAdditionalRotation(swingStart.slerp(swingEnd, slerpAmount));
                    //sprite.SetAnimation("stand-left");
                    interpolate = 0.0f;
                    start = end = GetAttackLocation(target);

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

                    start = GetAttackLocation(target);
                    end = this.startPosition;
                    interpolate = remainingTime / (float)standEndTime;
                }

                this.currentPosition = end.lerp(start, interpolate);

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

        private Vector3 GetAttackLocation(IBattleTarget target)
        {
            var totalScale = sprite.BaseScale * currentScale;
            var targetAttackLocation = target.MeleeAttackLocation;
            targetAttackLocation.x -= totalScale.x / 2;
            targetAttackLocation.y = totalScale.y / 2.0f;
            return targetAttackLocation;
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
            this.spriteInstanceFactory.TryReturn(spriteInstance);
            rtInstances.RemoveSprite(sprite);
            rtInstances.RemoveShaderTableBinder(Bind);
            rtInstances.RemoveTlasBuild(tlasData);
        }

        public void ApplyDamage(IDamageCalculator calculator, long damage)
        {
            currentHp = calculator.ApplyDamage(damage, currentHp, Stats.Hp);
        }

        public Vector3 DamageDisplayLocation => currentPosition + new Vector3(0.5f * currentScale.x, 0.5f * currentScale.y, 0f);

        public IBattleStats Stats { get; }

        public Vector3 CursorDisplayLocation => DamageDisplayLocation;

        public bool IsDead => this.currentHp == 0;

        public BattleTargetType BattleTargetType => BattleTargetType.Enemy;

        public long XpReward { get; private set; }

        public long GoldReward { get; private set; }

        private void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            spriteInstance.Bind(this.tlasData.InstanceName, sbt, tlas);
        }
    }
}
