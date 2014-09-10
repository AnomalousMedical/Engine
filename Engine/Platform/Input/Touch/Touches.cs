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
            GestureProcessed = false;
        }

        public void alertGestureProcessed()
        {
            GestureProcessed = true;
        }

        internal bool GestureProcessed { get; private set; }

        public IReadOnlyList<Finger> Fingers
        {
            get
            {
                return fingers.List;
            }
        }

        internal void fireTouchMoved(TouchInfo info)
        {
            Finger finger;
            if (!fingers.TryGetValue(info.id, out finger))
            {
                fireTouchStarted(info);
                finger = fingers[info.id];
            }
            finger.setCurrentPosition(info.normalizedX, info.normalizedY, info.pixelX, info.pixelY);
            //Log.Debug("GestureEngine Touch moved {0} {1} {2}", info.id, info.normalizedX, info.normalizedY);
        }

        internal void fireTouchEnded(TouchInfo info)
        {
            Finger finger;
            if (fingers.TryGetValue(info.id, out finger))
            {
                fingers.Remove(info.id);
                finger.finished();
            }
            //Log.Debug("GestureEngine Touch ended {0} {1} {2}", info.id, info.normalizedX, info.normalizedY);
        }

        internal void fireTouchStarted(TouchInfo info)
        {
            Finger finger;
            if (!fingers.TryGetValue(info.id, out finger))
            {
                finger = fingerPool.getPooledObject();
                fingers.Add(info.id, finger);
            }
            finger.setInfoOutOfPool(info.id, info.normalizedX, info.normalizedY, info.pixelX, info.pixelY);
            //Log.Debug("GestureEngine Touch started {0} {1} {2}", info.id, info.normalizedX, info.normalizedY);
        }

        internal void fireAllTouchesCanceled()
        {
            fingers.Clear();
            //Log.Debug("All touches canceled");
        }
    }
}
