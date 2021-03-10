using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using Engine.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;

namespace BepuPlugin
{
    struct CollisionEventHandler : IContactEventHandler
    {
        internal Simulation Simulation;

        private ConcurrentBag<CollisionEvent> currentEvents;
        private ThreadSafePool<CollisionEvent> eventPool;
        private Dictionary<CollidableReference, Action<CollisionEvent>> collisionEventHandlers;

        /// <summary>
        /// You must call this constructor. The value of callMe does not matter.
        /// </summary>
        /// <param name="callMe"></param>
        public CollisionEventHandler(bool callMe)
        {
            currentEvents = new ConcurrentBag<CollisionEvent>();
            eventPool = new ThreadSafePool<CollisionEvent>(() => new CollisionEvent());
            collisionEventHandlers = new Dictionary<CollidableReference, Action<CollisionEvent>>();
            Simulation = null;
        }

        public void OnContactAdded<TManifold>(CollidableReference eventSource, CollidablePair pair, ref TManifold contactManifold,
            in Vector3 contactOffset, in Vector3 contactNormal, float depth, int featureId, int contactIndex, int workerIndex) where TManifold : struct, IContactManifold<TManifold>
        {
            //This can happen from any thread

            //var other = pair.A.Packed == eventSource.Packed ? pair.B : pair.A;
            //Console.WriteLine($"Added contact: ({eventSource}, {other}): {featureId}");
            //Simply ignore any particles beyond the allocated space.
            //var index = Interlocked.Increment(ref Particles.Count) - 1;
            //if (index < Particles.Span.Length)
            //{
            //    ref var particle = ref Particles[index];

            //    //Contact data is calibrated according to the order of the pair, so using A's position is important.
            //    particle.Position = contactOffset + (pair.A.Mobility == CollidableMobility.Static ?
            //        new StaticReference(pair.A.StaticHandle, simulation.Statics).Pose.Position :
            //        new BodyReference(pair.A.BodyHandle, simulation.Bodies).Pose.Position);
            //    particle.Age = 0;
            //    particle.Normal = contactNormal;
            //}
            var evt = eventPool.Get();
            evt.Update(eventSource, pair, contactOffset, contactNormal, depth, featureId, contactIndex, workerIndex);
            currentEvents.Add(evt);
        }

        public void AddEventHandler(CollidableReference collidable, Action<CollisionEvent> handler)
        {
            collisionEventHandlers.Add(collidable, handler);
        }

        public void RemoveEventHandler(CollidableReference collidable)
        {
            collisionEventHandlers.Remove(collidable);
        }

        public void FireCollisionEvents()
        {
            while(currentEvents.TryTake(out var evt))
            {
                if(collisionEventHandlers.TryGetValue(evt.EventSource, out var handler))
                {
                    handler.Invoke(evt);
                }
                eventPool.Return(evt);
            }
        }
    }
}
