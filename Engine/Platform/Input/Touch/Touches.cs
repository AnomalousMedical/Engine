using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using Engine.Platform;
using Engine;

namespace Engine.Platform
{
    public class Touches
    {
        GenericObjectPool<Finger> fingerPool = new GenericObjectPool<Finger>();
        private FastIteratorMap<int, Finger> fingers = new FastIteratorMap<int, Finger>();

        public Touches()
        {
            
        }

        public void tick()
        {
            foreach (Finger finger in fingers)
            {
                finger.captureCurrentPositionAsLast();
            }
        }

        public IReadOnlyList<Finger> Fingers
        {
            get
            {
                return fingers.List;
            }
        }

        internal void fireTouchMoved(TouchInfo info)
        {
            Finger finger = findFinger(info);
            finger.setCurrentPosition(info.normalizedX, info.normalizedY, info.pixelX, info.pixelY);
        }

        internal void fireTouchEnded(TouchInfo info)
        {
            Finger finger;
            if (fingers.TryGetValue(info.id, out finger))
            {
                fingers.Remove(info.id);
                finger.finished();
            }
        }

        internal void fireTouchStarted(TouchInfo info)
        {
            findFinger(info);
        }

        internal void fireAllTouchesCanceled()
        {
            fingers.Clear();
        }

        private Finger findFinger(TouchInfo info)
        {
            Finger finger;
            if (!fingers.TryGetValue(info.id, out finger))
            {
                finger = fingerPool.getPooledObject();
                fingers.Add(info.id, finger);
                finger.setInfoOutOfPool(info.id, info.normalizedX, info.normalizedY, info.pixelX, info.pixelY);
            }
            return finger;
        }
    }
}
