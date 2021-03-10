using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BepuPlugin
{
    public class CollisionEvent
    {
        CollidableReference eventSource;
        CollidablePair pair;
        Vector3 contactOffset;
        Vector3 contactNormal;
        float depth;
        int featureId;
        int contactIndex;
        int workerIndex;

        public CollidableReference EventSource => eventSource;
        public CollidablePair Pair => pair;
        public Vector3 ContactOffset => contactOffset;
        public Vector3 ContactNormal => contactNormal;
        public float Depth => depth;
        public int FeatureId => featureId;
        public int ContactIndex => contactIndex;
        public int WorkerIndex => workerIndex;

        internal void Update(CollidableReference eventSource, CollidablePair pair,
            in Vector3 contactOffset, in Vector3 contactNormal, float depth, int featureId, int contactIndex, int workerIndex)
        {
            this.eventSource = eventSource;
            this.pair = pair;
            this.contactOffset = contactOffset;
            this.contactNormal = contactNormal;
            this.depth = depth;
            this.featureId = featureId;
            this.contactIndex = contactIndex;
            this.workerIndex = workerIndex;
        }
    }
}
