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

        private ConcurrentBag<CollisionEvent> contactEvents;
        private ConcurrentBag<CollisionEvent> continueEvents;
        private ConcurrentBag<CollisionEvent> endEvents;
        private ThreadSafePool<CollisionEvent> eventPool;
        private Dictionary<CollidableReference, Action<CollisionEvent>> collisionEventHandlers;
        private Dictionary<CollidableReference, Action<CollisionEvent>> continueEventHandlers;
        private Dictionary<CollidableReference, Action<CollisionEvent>> endEventHandlers;

        /// <summary>
        /// You must call this constructor. The value of callMe does not matter.
        /// </summary>
        /// <param name="callMe"></param>
        public CollisionEventHandler(bool callMe)
        {
            contactEvents = new ConcurrentBag<CollisionEvent>();
            continueEvents = new ConcurrentBag<CollisionEvent>();
            endEvents = new ConcurrentBag<CollisionEvent>();
            eventPool = new ThreadSafePool<CollisionEvent>(() => new CollisionEvent());
            collisionEventHandlers = new Dictionary<CollidableReference, Action<CollisionEvent>>();
            continueEventHandlers = new Dictionary<CollidableReference, Action<CollisionEvent>>();
            endEventHandlers = new Dictionary<CollidableReference, Action<CollisionEvent>>();
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
            contactEvents.Add(evt);
        }

        public void OnContactContinues<TManifold>(CollidableReference eventSource, CollidablePair pair, ref TManifold contactManifold,
            in Vector3 contactOffset, in Vector3 contactNormal, float depth, int featureId, int contactIndex, int workerIndex) where TManifold : struct, IContactManifold<TManifold>
        {
            var evt = eventPool.Get();
            evt.Update(eventSource, pair, contactOffset, contactNormal, depth, featureId, contactIndex, workerIndex);
            continueEvents.Add(evt);
        }

        public void OnContactEnd(CollidableReference eventSource)
        {
            var evt = eventPool.Get();
            evt.Update(eventSource, default, default, default, default, default, default, default);
            endEvents.Add(evt);
        }

        public void AddContactHandler(CollidableReference collidable, Action<CollisionEvent> handler)
        {
            collisionEventHandlers.Add(collidable, handler);
        }

        public void RemoveContactHandler(CollidableReference collidable)
        {
            collisionEventHandlers.Remove(collidable);
        }

        public void AddContinueHandler(CollidableReference collidable, Action<CollisionEvent> handler)
        {
            continueEventHandlers.Add(collidable, handler);
        }

        public void RemoveContinueHandler(CollidableReference collidable)
        {
            continueEventHandlers.Remove(collidable);
        }

        public void AddEndHandler(CollidableReference collidable, Action<CollisionEvent> handler)
        {
            endEventHandlers.Add(collidable, handler);
        }

        public void RemoveEndHandler(CollidableReference collidable)
        {
            endEventHandlers.Remove(collidable);
        }

        public void FireEvents()
        {
            while(contactEvents.TryTake(out var evt))
            {
                if(collisionEventHandlers.TryGetValue(evt.EventSource, out var handler))
                {
                    handler.Invoke(evt);
                }
                eventPool.Return(evt);
            }

            while(continueEvents.TryTake(out var evt))
            {
                if (continueEventHandlers.TryGetValue(evt.EventSource, out var handler))
                {
                    handler.Invoke(evt);
                }
                eventPool.Return(evt);
            }

            while (endEvents.TryTake(out var evt))
            {
                if (endEventHandlers.TryGetValue(evt.EventSource, out var handler))
                {
                    handler.Invoke(evt);
                }
                eventPool.Return(evt);
            }
        }
    }
}
