using BulletPlugin;
using Engine;
using Engine.Attributes;
using Engine.Editing;
using Engine.Platform;
using Engine.Platform.Input;
using Engine.Renderer;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.SidescrollerCore
{
    public class PlayerControls
    {
        public ButtonEvent MoveRightEvent { get; private set; }
        public ButtonEvent MoveLeftEvent { get; private set; }
        public ButtonEvent MoveUpEvent { get; private set; }
        public ButtonEvent MoveDownEvent { get; private set; }
        public ButtonEvent JumpEvent { get; private set; }

        public PlayerControls(IEventLayerKeyInjector<PlayerControls> injector)
        {
            MoveRightEvent = new ButtonEvent(injector.Key);
            MoveLeftEvent = new ButtonEvent(injector.Key);
            MoveUpEvent = new ButtonEvent(injector.Key);
            MoveDownEvent = new ButtonEvent(injector.Key);
            JumpEvent = new ButtonEvent(injector.Key);
        }

        public void Build(EventManager eventManager)
        {
            try
            {
                eventManager.addEvent(MoveRightEvent);
                eventManager.addEvent(MoveLeftEvent);
                eventManager.addEvent(MoveUpEvent);
                eventManager.addEvent(MoveDownEvent);
                eventManager.addEvent(JumpEvent);
            }
            catch (Exception) { }
        }
    }

    /// <summary>
    /// This class allows us to inject player controls based on a type.
    /// This allows us to inject controls into different player instances based on the PlayerType and PlayerId.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PlayerControls<T> : PlayerControls
    {
        public PlayerControls(IEventLayerKeyInjector<PlayerControls> injector) : base(injector)
        {
        }
    }

    partial class PlayerController : Behavior
    {
        [Editable]
        public PlayerId PlayerId { get; set; } = PlayerId.Player1;

        [Editable]
        public String RigidBodyName { get; set; }

        [Editable]
        public String NodeName { get; set; }

        [Editable]
        public String EntityName { get; set; }

        [Editable]
        public String RunBaseAnimationName { get; set; }

        [Editable]
        public String RunTopAnimationName { get; set; }

        [Editable]
        public String IdleBaseAnimationName { get; set; }

        [Editable]
        public String IdleTopAnimationName { get; set; }

        [Editable]
        public String JumpStartAnimationName { get; set; }

        [Editable]
        public String JumpLoopAnimationName { get; set; }

        [Editable]
        public String JumpEndAnimationName { get; set; }

        [Editable]
        public float MaxRunSpeed { get; set; }

        [Editable]
        public float MaxJumpSpeed { get; set; }

        [Editable]
        public float RunAcceleration { get; set; }

        [Editable]
        public float JumpAcceleration { get; set; }

        [Editable]
        public float JumpMaxDuration { get; set; }

        public PlayerControls Controls { get { return playerControls; } }

        [DoNotCopy]
        [DoNotSave]
        private PlayerControls playerControls;

        [DoNotCopy]
        [DoNotSave]
        private RigidBody rigidBody;

        [DoNotCopy]
        [DoNotSave]
        private SceneNodeElement node;

        [DoNotCopy]
        [DoNotSave]
        private Entity entity;

        [DoNotCopy]
        [DoNotSave]
        private AnimationState runBase;

        [DoNotCopy]
        [DoNotSave]
        private AnimationState runTop;

        [DoNotCopy]
        [DoNotSave]
        private AnimationState idleBase;

        [DoNotCopy]
        [DoNotSave]
        private AnimationState idleTop;

        [DoNotCopy]
        [DoNotSave]
        private AnimationState jumpStart;

        [DoNotCopy]
        [DoNotSave]
        private AnimationState jumpLoop;

        [DoNotCopy]
        [DoNotSave]
        private AnimationState jumpEnd;

        [DoNotCopy]
        [DoNotSave]
        private bool onGround = true;

        [DoNotCopy]
        [DoNotSave]
        private bool releasedJumpInAir = false;

        [DoNotCopy]
        [DoNotSave]
        private float jumpTime = float.MaxValue;

        [DoNotCopy]
        [DoNotSave]
        ClosestRayResultCallback groundRayCenter;

        [DoNotCopy]
        [DoNotSave]
        ClosestRayResultCallback groundRayFront;

        [DoNotCopy]
        [DoNotSave]
        ClosestRayResultCallback groundRayRear;

        [DoNotCopy]
        [DoNotSave]
        PlayerControllerState __currentState;

        [DoNotCopy]
        [DoNotSave]
        RunningPlayerState runningState;

        [DoNotCopy]
        [DoNotSave]
        IdlePlayerState idleState;

        [DoNotCopy]
        [DoNotSave]
        JumpUpPlayerState jumpUpState;

        [DoNotCopy]
        [DoNotSave]
        FallingPlayerState fallingState;

        [DoNotCopy]
        [DoNotSave]
        private Vector3 frontRearRayOffset = new Vector3(1.1f, 0f, 0f);

        public PlayerController()
        {
            NodeName = "Node";
            EntityName = "Entity";
            RunBaseAnimationName = "RunBase";
            RunTopAnimationName = "RunTop";
            IdleBaseAnimationName = "IdleBase";
            IdleTopAnimationName = "IdleTop";
            JumpStartAnimationName = "JumpStart";
            JumpLoopAnimationName = "JumpLoop";
            JumpEndAnimationName = "JumpEnd";
            MaxRunSpeed = 7.0f;
            MaxJumpSpeed = 10.0f;
            RunAcceleration = 3.0f;
            JumpAcceleration = 3.0f;
            JumpMaxDuration = 0.75f;
        }

        protected override void link()
        {
            base.link();

            var controlsType = this.PlayerId.GetPlayerKeyedType(typeof(PlayerControls<>));
            playerControls = Scope.ServiceProvider.GetService(controlsType) as PlayerControls;

            rigidBody = Owner.getElement(RigidBodyName) as RigidBody;
            if (rigidBody == null)
            {
                blacklist("Cannot find player rigid body '{0}'", RigidBodyName);
            }

            node = Owner.getElement(NodeName) as SceneNodeElement;
            if (node == null)
            {
                blacklist("Cannot find player node '{0}'");
            }
            entity = node.getNodeObject(EntityName) as Entity;
            if (entity == null)
            {
                blacklist("Cannot find entity '{0}' on node '{1}'", EntityName, NodeName);
            }

            runBase = entity.getAnimationState(RunBaseAnimationName);
            if (runBase == null)
            {
                blacklist("Cannot find animation for run base '{0}' on entity '{1}'", RunBaseAnimationName, EntityName);
            }

            runTop = entity.getAnimationState(RunTopAnimationName);
            if (runTop == null)
            {
                blacklist("Cannot find animation for run top '{0}' on entity '{1}'", RunTopAnimationName, EntityName);
            }

            idleBase = entity.getAnimationState(IdleBaseAnimationName);
            if (idleBase == null)
            {
                blacklist("Cannot find animation for idle base '{0}' on entity '{1}'", IdleBaseAnimationName, EntityName);
            }

            idleTop = entity.getAnimationState(IdleTopAnimationName);
            if (idleTop == null)
            {
                blacklist("Cannot find animation for idle top '{0}' on entity '{1}'", IdleTopAnimationName, EntityName);
            }

            jumpStart = entity.getAnimationState(JumpStartAnimationName);
            if (jumpStart == null)
            {
                blacklist("Cannot find animation for jump start '{0}' on entity '{1}'", JumpStartAnimationName, EntityName);
            }

            jumpLoop = entity.getAnimationState(JumpLoopAnimationName);
            if (jumpLoop == null)
            {
                blacklist("Cannot find animation for jump loop '{0}' on entity '{1}'", JumpLoopAnimationName, EntityName);
            }

            jumpEnd = entity.getAnimationState(JumpLoopAnimationName);
            if (jumpEnd == null)
            {
                blacklist("Cannot find animation for jump end '{0}' on entity '{1}'", JumpEndAnimationName, EntityName);
            }

            groundRayCenter = new ClosestRayResultCallback(Owner.Translation, Owner.Translation + Vector3.Down);
            groundRayCenter.CollisionFilterMask = ~2;

            groundRayFront = new ClosestRayResultCallback(Owner.Translation + frontRearRayOffset, Owner.Translation + Vector3.Down);
            groundRayFront.CollisionFilterMask = ~2;

            groundRayRear = new ClosestRayResultCallback(Owner.Translation - frontRearRayOffset, Owner.Translation + Vector3.Down);
            groundRayRear.CollisionFilterMask = ~2;

            rigidBody.Scene.Tick += Scene_Tick;

            runningState = new RunningPlayerState();
            runningState.link(this);

            idleState = new IdlePlayerState();
            idleState.link(this);

            jumpUpState = new JumpUpPlayerState();
            jumpUpState.link(this);

            fallingState = new FallingPlayerState();
            fallingState.link(this);

            CurrentState = idleState;

            addToDebugDrawing();
        }

        protected override void willDestroy()
        {
            rigidBody.Scene.Tick -= Scene_Tick;

            base.willDestroy();
        }

        protected override void destroy()
        {
            groundRayCenter.Dispose();

            base.destroy();
        }

        public override void update(Clock clock, EventManager eventManager)
        {
            if (Controls.MoveLeftEvent.FirstFrameDown)
            {
                this.updateRotation(new Quaternion(-1.57079633f, 0, 0));
            }
            else if (Controls.MoveRightEvent.FirstFrameDown)
            {
                this.updateRotation(new Quaternion(1.57079633f, 0, 0));
            }
            else if (Controls.MoveUpEvent.FirstFrameDown)
            {
                this.updateRotation(new Quaternion(3.14159f, 0, 0));
            }
            else if (Controls.MoveDownEvent.FirstFrameDown)
            {
                this.updateRotation(new Quaternion(0, 0, 0));
            }

            __currentState.update(clock, eventManager);
            return;
        }

        const float fallSpeed = -30;
        const float jumpSpeed = 30;

        void Scene_Tick(float timeSpan)
        {
            __currentState.physicsTick(timeSpan);
        }

        bool isOnGround()
        {
            groundRayCenter.reset();
            Vector3 origin = Owner.Translation;
            groundRayCenter.RayFromWorld = origin;
            groundRayCenter.RayToWorld = origin + Vector3.Down * 5.1f;
            rigidBody.Scene.raycast(groundRayCenter);

            if (groundRayCenter.HasHit)
            {
                return true;
            }
            else
            {
                groundRayRear.reset();
                origin = Owner.Translation - frontRearRayOffset;
                groundRayRear.RayFromWorld = origin;
                groundRayRear.RayToWorld = origin + Vector3.Down * 5.1f;
                rigidBody.Scene.raycast(groundRayRear);

                if (groundRayRear.HasHit)
                {
                    return true;
                }
                else
                {
                    groundRayFront.reset();
                    origin = Owner.Translation + frontRearRayOffset;
                    groundRayFront.RayFromWorld = origin;
                    groundRayFront.RayToWorld = origin + Vector3.Down * 5.1f;
                    rigidBody.Scene.raycast(groundRayFront);

                    return groundRayFront.HasHit;
                }
            }
        }

        [Editable]
        public Vector3 LeftDirVector { get; set; } = new Vector3(-1.0f, 0.0f, 0.0f);

        [Editable]
        public Vector3 RightDirVector { get; set; } = new Vector3(1.0f, 0.0f, 0.0f);

        [Editable]
        public Vector3 UpDirVector { get; set; } = new Vector3(0.0f, 0.0f, -1.0f);

        [Editable]
        public Vector3 DownDirVector { get; set; } = new Vector3(0.0f, 0.0f, 1.0f);

        private void moveDuringPhysics()
        {
            bool applyImpulse = false;
            Vector3 impulse = Vector3.Zero;

            if (Controls.MoveLeftEvent.Down)
            {
                impulse += LeftDirVector * RunAcceleration;
            }
            else if (Controls.MoveRightEvent.Down)
            {
                impulse += RightDirVector * RunAcceleration;
            }

            if (Controls.MoveUpEvent.Down)
            {
                impulse += UpDirVector * RunAcceleration;
            }
            else if (Controls.MoveDownEvent.Down)
            {
                impulse += DownDirVector * RunAcceleration;
            }

            if(impulse != Vector3.Zero)
            {
                rigidBody.applyCentralImpulse(impulse);
            }

            rigidBody.capLinearVelocity(MaxRunSpeed);
        }

        public override void drawDebugInfo(DebugDrawingSurface debugDrawing)
        {
            debugDrawing.begin(Owner.Name, DrawingType.LineList);
            debugDrawing.Color = Color.HotPink;
            debugDrawing.drawLine(groundRayCenter.RayFromWorld, groundRayCenter.RayToWorld);
            debugDrawing.drawLine(groundRayFront.RayFromWorld, groundRayFront.RayToWorld);
            debugDrawing.drawLine(groundRayRear.RayFromWorld, groundRayRear.RayToWorld);
            debugDrawing.end();
        }

        internal ButtonEvent Jump
        {
            get
            {
                return Controls.JumpEvent;
            }
        }

        private PlayerControllerState CurrentState
        {
            get
            {
                return __currentState;
            }
            set
            {
                if (__currentState != null)
                {
                    __currentState.stop();
                }
                __currentState = value;
                if (__currentState != null)
                {
                    __currentState.start();
                }
            }
        }
    }
}
