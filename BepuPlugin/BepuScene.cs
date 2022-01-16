using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin.Characters;
using BepuPlugin.Demo;
using BepuUtilities.Collections;
using BepuUtilities.Memory;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vector3 = System.Numerics.Vector3;

namespace BepuPlugin
{
    public class BepuScene : IDisposable, IBepuScene
    {
        public class Description
        {
            public float TimestepSeconds { get; set; } = 1f / 60f;
        }

        Simulation simulation;
        SimpleThreadDispatcher threadDispatcher;
        BufferPool bufferPool;
        CharacterControllers characterControllers;
        ContactEvents<CollisionEventHandler> events;
        private List<CharacterMover> characterMovers = new List<CharacterMover>();
        private FastIteratorMap<BodyHandle, BodyPosition> interpolatedPositions = new FastIteratorMap<BodyHandle, BodyPosition>();
        private CollisionEventHandler collisionEventHandler = new CollisionEventHandler(true);

        public event Action<IBepuScene> OnUpdated;

        private float timestepSeconds;
        private long timestepMicro;
        private long timestepAccumulator;
        private float interpolationValue;

        public BepuScene(Description description)
        {
            timestepMicro = (long)(description.TimestepSeconds * (float)Clock.SecondsToMicro);
            timestepSeconds = description.TimestepSeconds;
            timestepAccumulator = 0;

            //Taking off 1 thread could help stability https://github.com/bepu/bepuphysics2/blob/master/Documentation/PerformanceTips.md#general
            var numThreads = Math.Max(Environment.ProcessorCount - 1, 1);
            threadDispatcher = new SimpleThreadDispatcher(numThreads);

            //The buffer pool is a source of raw memory blobs for the engine to use.
            bufferPool = new BufferPool();

            characterControllers = new CharacterControllers(bufferPool);
            events = new ContactEvents<CollisionEventHandler>(collisionEventHandler, bufferPool, threadDispatcher);

            //The PositionFirstTimestepper is the simplest timestepping mode, but since it integrates velocity into position at the start of the frame, directly modified velocities outside of the timestep
            //will be integrated before collision detection or the solver has a chance to intervene. That's fine in this demo. Other built-in options include the PositionLastTimestepper and the SubsteppingTimestepper.
            //Note that the timestepper also has callbacks that you can use for executing logic between processing stages, like BeforeCollisionDetection.
            simulation = Simulation.Create(bufferPool, 
                new CharacterNarrowphaseCallbacks<CollisionEventHandler>(characterControllers, events), 
                new DemoPoseIntegratorCallbacks(new Vector3(0, -10, 0)), 
                new PositionFirstTimestepper());

            events.EventHandler.Simulation = simulation;
        }

        public void Dispose()
        {
            //If you intend to reuse the BufferPool, disposing the simulation is a good idea- it returns all the buffers to the pool for reuse.
            //Here, we dispose it, but it's not really required; we immediately thereafter clear the BufferPool of all held memory.
            //Note that failing to dispose buffer pools can result in memory leaks.
            simulation.Dispose();
            threadDispatcher.Dispose();
            characterControllers.Dispose();
            bufferPool.Clear();
        }

        public void Update(Clock clock, in Vector3 cameraForward)
        {
            timestepAccumulator += clock.DeltaTimeMicro;

            //Tick physics until the accumulator is below the timestep value.
            //This will both skip frames in case of a faster framerate and run
            //them extra on a slowdown. If the computer is too slow this can
            //spiral until the fps are very low, but it is unlikely to happen.
            //This also will always end since time is not added to it.
            while (timestepAccumulator > timestepMicro)
            {
                //Both the movers and the physics tick are 1 physics update
                foreach (var mover in characterMovers)
                {
                    mover.UpdateCharacterGoals(cameraForward, timestepSeconds);
                }

                //Tick physics
                simulation.Timestep(timestepSeconds, threadDispatcher);

                timestepAccumulator -= timestepMicro;

                //Sync interpolation
                foreach (var position in interpolatedPositions)
                {
                    var bodyRef = simulation.Bodies.GetBodyReference(position.BodyHandle);
                    var pose = bodyRef.Pose;
                    position.LastPosition = position.Position;
                    position.LastOrientation = position.Orientation;
                    position.Position = pose.Position;
                    position.Orientation = pose.Orientation;
                }

                events.Flush();
                collisionEventHandler.FireEvents();
            }

            interpolationValue = (float)timestepAccumulator / timestepMicro;

            this.OnUpdated?.Invoke(this);
        }

        /// <summary>
        /// Create a character mover. Must call DestroyCharacterMover when finished with it.
        /// </summary>
        /// <param name="bodyDescription"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public CharacterMover CreateCharacterMover(in BodyDescription bodyDescription, CharacterMoverDescription desc)
        {
            var mover = new CharacterMover(characterControllers, bodyDescription, desc);
            characterMovers.Add(mover);
            return mover;
        }

        /// <summary>
        /// Destroy a character mover.
        /// </summary>
        /// <param name="mover"></param>
        public void DestroyCharacterMover(CharacterMover mover)
        {
            characterMovers.Remove(mover);
            mover.Dispose();
        }

        /// <summary>
        /// Add a body handle to the interpolated body collection. This is good for most moving bodies to belong
        /// to so an interpolated position can be aquired between timesteps. Please call <see cref="RemoveFromInterpolation(in BodyHandle)"/>
        /// when interpolated positions are no longer needed.
        /// </summary>
        /// <param name="body"></param>
        public void AddToInterpolation(in BodyHandle body)
        {
            var bodyRef = simulation.Bodies.GetBodyReference(body);
            var pose = bodyRef.Pose;
            interpolatedPositions.Add(body, new BodyPosition(body, pose.Position, pose.Orientation));
        }

        /// <summary>
        /// Remove a body from the interpolated positions collection.
        /// </summary>
        /// <param name="body"></param>
        public void RemoveFromInterpolation(in BodyHandle body)
        {
            interpolatedPositions.Remove(body);
        }

        /// <summary>
        /// Given a handle to a body get the interpolated position it will occupy based on the time left in the accumulator.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="position"></param>
        /// <param name="orientation"></param>
        public void GetInterpolatedPosition(in BodyHandle body, ref Engine.Vector3 position, ref Engine.Quaternion orientation)
        {
            var bodyPosition = interpolatedPositions[body];
            var posLerp = System.Numerics.Vector3.Lerp(bodyPosition.LastPosition, bodyPosition.Position, interpolationValue);
            position.x = posLerp.X;
            position.y = posLerp.Y;
            position.z = posLerp.Z;

            var orienSlerp = System.Numerics.Quaternion.Slerp(bodyPosition.LastOrientation, bodyPosition.Orientation, interpolationValue);
            orientation.x = orienSlerp.X;
            orientation.y = orienSlerp.Y;
            orientation.z = orienSlerp.Z;
            orientation.w = orienSlerp.W;
        }

        public void RegisterCollisionListener(CollidableReference collidable, Action<CollisionEvent> collisionEvent, Action<CollisionEvent> continueEvent = null)
        {
            events.RegisterListener(collidable);
            collisionEventHandler.AddContactHandler(collidable, collisionEvent);
            if(continueEvent != null)
            {
                collisionEventHandler.AddContinueHandler(collidable, continueEvent);
            }
        }

        public void UnregisterCollisionListener(CollidableReference collidable)
        {
            events.UnregisterListener(collidable);
            collisionEventHandler.RemoveContactHandler(collidable);
            collisionEventHandler.RemoveContinueHandler(collidable);
        }

        public Simulation Simulation => simulation;
    }
}
