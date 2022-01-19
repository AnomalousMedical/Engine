using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuUtilities;
using BepuUtilities.Collections;
using BepuUtilities.Memory;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BepuPlugin
{
    public unsafe class ContactEvents<TEventHandler> : IDisposable where TEventHandler : IContactEventHandler
    {
        struct PreviousCollisionData
        {
            public CollidableReference Collidable;
            public bool Fresh;
            public int ContactCount;
            //FeatureIds are identifiers encoding what features on the involved shapes contributed to the contact. We store up to 4 feature ids, one for each potential contact.
            //A "feature" is things like a face, vertex, or edge. There is no single interpretation for what a feature is- the mapping is defined on a per collision pair level.
            //In this demo, we only care to check whether a given contact in the current frame maps onto a contact from a previous frame.
            //We can use this to only emit 'contact added' events when a new contact with an unrecognized id is reported.
            public int FeatureId0;
            public int FeatureId1;
            public int FeatureId2;
            public int FeatureId3;
        }

        Bodies bodies;
        public TEventHandler EventHandler;
        BufferPool pool;
        IThreadDispatcher threadDispatcher;


        QuickDictionary<CollidableReference, QuickList<PreviousCollisionData>, CollidableReferenceComparer> listeners;

        //Since the narrow phase works on multiple threads, we can't modify the collision data during execution.
        //The pending changes are stored in per-worker collections and flushed afterwards.
        struct PendingNewEntry
        {
            public int ListenerIndex;
            public PreviousCollisionData Collision;
        }

        QuickList<PendingNewEntry>[] pendingWorkerAdds;

        public ContactEvents(TEventHandler eventHandler, BufferPool pool, IThreadDispatcher threadDispatcher, int initialListenerCapacity = 32)
        {
            this.EventHandler = eventHandler;
            this.pool = pool;
            this.threadDispatcher = threadDispatcher;
            pendingWorkerAdds = new QuickList<PendingNewEntry>[threadDispatcher == null ? 1 : threadDispatcher.ThreadCount];
            listeners = new QuickDictionary<CollidableReference, QuickList<PreviousCollisionData>, CollidableReferenceComparer>(initialListenerCapacity, pool);
        }

        public void Initialize(Bodies bodies)
        {
            this.bodies = bodies;
        }

        /// <summary>
        /// Begins listening for events related to the given collidable.
        /// </summary>
        /// <param name="collidable">Collidable to monitor for events.</param>
        public void RegisterListener(CollidableReference collidable)
        {
            listeners.Add(collidable, default, pool);
        }

        /// <summary>
        /// Stops listening for events related to the given collidable.
        /// </summary>
        /// <param name="collidable">Collidable to stop listening for.</param>
        public void UnregisterListener(CollidableReference collidable)
        {
            var exists = listeners.GetTableIndices(ref collidable, out var tableIndex, out var elementIndex);
            Debug.Assert(exists, "Should only try to unregister listeners that actually exist.");
            var listener = listeners.Values[elementIndex];
            if (listener.Span.Allocated)
            {
                listener.Dispose(pool);
                listeners.FastRemove(tableIndex, elementIndex);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool FeatureIdContained(int featureId, ulong previousFeatureIds)
        {
            var contained0 = (((int)previousFeatureIds ^ featureId) & 0xFFFF) == 0;
            var contained1 = (((int)(previousFeatureIds >> 16) ^ featureId) & 0xFFFF) == 0;
            var contained2 = (((int)(previousFeatureIds >> 32) ^ featureId) & 0xFFFF) == 0;
            var contained3 = (((int)(previousFeatureIds >> 48) ^ featureId) & 0xFFFF) == 0;
            return contained0 | contained1 | contained2 | contained3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void UpdatePreviousCollision<TManifold>(ref PreviousCollisionData collision, ref TManifold manifold) where TManifold : struct, IContactManifold<TManifold>
        {
            Debug.Assert(manifold.Count <= 4, "This demo was built on the assumption that nonconvex manifolds will have a maximum of four contacts, but that might have changed.");
            //If the above assert gets hit because of a change to nonconvex manifold capacities, the packed feature id representation this uses will need to be updated.
            //I very much doubt the nonconvex manifold will ever use more than 8 contacts, so addressing this wouldn't require much of a change.
            for (int j = 0; j < manifold.Count; ++j)
            {
                Unsafe.Add(ref collision.FeatureId0, j) = manifold.GetFeatureId(j);
            }
            collision.ContactCount = manifold.Count;
            collision.Fresh = true;
        }

        void HandleManifoldForCollidable<TManifold>(int workerIndex, CollidableReference source, CollidableReference other, CollidablePair pair, ref TManifold manifold) where TManifold : struct, IContactManifold<TManifold>
        {
            //The "source" refers to the object that an event handler was (potentially) attached to, so we look for listeners registered for it.
            //(This function is called for both orders of the pair, so we'll catch listeners for either.)
            if (listeners.GetTableIndices(ref source, out var tableIndex, out var listenerIndex))
            {
                //This collidable is registered. Is the opposing collidable present?
                ref var previousCollisions = ref listeners.Values[listenerIndex];
                int previousCollisionIndex = -1;
                for (int i = 0; i < previousCollisions.Count; ++i)
                {
                    ref var collision = ref previousCollisions[i];
                    //Since the 'Packed' field contains both the handle type (dynamic, kinematic, or static) and the handle index packed into a single bitfield, an equal value guarantees we are dealing with the same collidable.
                    if (collision.Collidable.Packed == other.Packed)
                    {
                        previousCollisionIndex = i;
                        //This manifold is associated with an existing collision.
                        for (int contactIndex = 0; contactIndex < manifold.Count; ++contactIndex)
                        {
                            //We can check if each contact was already present in the previous frame by looking at contact feature ids. See the 'PreviousCollisionData' for a little more info on FeatureIds.
                            var featureId = manifold.GetFeatureId(contactIndex);
                            var featureIdIsOld = false;
                            for (int previousContactIndex = 0; previousContactIndex < collision.ContactCount; ++previousContactIndex)
                            {
                                if (featureId == Unsafe.Add(ref collision.FeatureId0, previousContactIndex))
                                {
                                    featureIdIsOld = true;
                                    break;
                                }
                            }

                            manifold.GetContact(contactIndex, out var offset, out var normal, out var depth, out _);

                            if (!featureIdIsOld)
                            {    
                                EventHandler.OnContactAdded(source, pair, ref manifold, offset, normal, depth, featureId, contactIndex, workerIndex);
                            }
                            else
                            {
                                EventHandler.OnContactContinues(source, pair, ref manifold, offset, normal, depth, featureId, contactIndex, workerIndex);
                            }
                        }
                        UpdatePreviousCollision(ref collision, ref manifold);
                        break;
                    }
                }
                if (previousCollisionIndex < 0)
                {
                    //There was no collision previously.
                    ref var addsforWorker = ref pendingWorkerAdds[workerIndex];
                    //EnsureCapacity will create the list if it doesn't already exist.
                    addsforWorker.EnsureCapacity(Math.Max(addsforWorker.Count + 1, 64), threadDispatcher != null ? threadDispatcher.GetThreadMemoryPool(workerIndex) : pool);
                    ref var pendingAdd = ref addsforWorker.AllocateUnsafely();
                    pendingAdd.ListenerIndex = listenerIndex;
                    pendingAdd.Collision.Collidable = other;
                    UpdatePreviousCollision(ref pendingAdd.Collision, ref manifold);
                    //Dispatch events for all contacts in this new manifold.
                    for (int i = 0; i < manifold.Count; ++i)
                    {
                        manifold.GetContact(i, out var offset, out var normal, out var depth, out var featureId);
                        EventHandler.OnContactAdded(source, pair, ref manifold, offset, normal, depth, featureId, i, workerIndex);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HandleManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold) where TManifold : struct, IContactManifold<TManifold>
        {
            HandleManifoldForCollidable(workerIndex, pair.A, pair.B, pair, ref manifold);
            HandleManifoldForCollidable(workerIndex, pair.B, pair.A, pair, ref manifold);
        }

        public void Flush()
        {
            //For simplicity, this is completely sequential. Note that it's technically possible to extract more parallelism, but the complexity cost is high and you would need
            //very large numbers of events being processed to make it worth it.

            //Remove any stale collisions. Stale collisions are those which should have received a new manifold update but did not because the manifold is no longer active.
            for (int i = 0; i < listeners.Count; ++i)
            {
                var collidable = listeners.Keys[i];
                //Pairs involved with inactive bodies do not need to be checked for freshness. If we did, it would result in inactive manifolds being considered a removal, and 
                //more contact added events would fire when the bodies woke up.
                if (collidable.Mobility != CollidableMobility.Static && bodies.HandleToLocation[collidable.BodyHandle.Value].SetIndex > 0)
                {
                    continue;
                }

                ref var collisions = ref listeners.Values[i];
                //Note reverse order. We remove during iteration.
                for (int j = collisions.Count - 1; j >= 0; --j)
                {
                    ref var collision = ref collisions[j];
                    //Again, any pair involving inactive bodies does not need to be examined.
                    if (collision.Collidable.Mobility != CollidableMobility.Static && bodies.HandleToLocation[collision.Collidable.BodyHandle.Value].SetIndex > 0)
                    {
                        continue;
                    }

                    if (!collision.Fresh)
                    {
                        EventHandler.OnContactEnd(collidable);
                        //This collision was not updated since the last flush despite being active. It should be removed.
                        collisions.FastRemoveAt(j);
                        if (collisions.Count == 0)
                        {
                            collisions.Dispose(pool);
                            collisions = default;
                        }
                    }
                    else
                    {
                        collision.Fresh = false;
                    }
                }
            }

            for (int i = 0; i < pendingWorkerAdds.Length; ++i)
            {
                ref var pendingAdds = ref pendingWorkerAdds[i];
                for (int j = 0; j < pendingAdds.Count; ++j)
                {
                    ref var add = ref pendingAdds[j];
                    ref var collisions = ref listeners.Values[add.ListenerIndex];
                    //Ensure capacity will initialize the slot if necessary.
                    collisions.EnsureCapacity(Math.Max(8, collisions.Count + 1), pool);
                    collisions.AllocateUnsafely() = pendingAdds[j].Collision;
                }
                if (pendingAdds.Span.Allocated)
                    pendingAdds.Dispose(threadDispatcher == null ? pool : threadDispatcher.GetThreadMemoryPool(i));
                //We rely on zeroing out the count for lazy initialization.
                pendingAdds = default;
            }
        }

        public void Dispose()
        {
            listeners.Dispose(pool);
            for (int i = 0; i < pendingWorkerAdds.Length; ++i)
            {
                Debug.Assert(!pendingWorkerAdds[i].Span.Allocated, "The pending worker adds should have been disposed by the previous flush.");
            }
        }
    }
}
