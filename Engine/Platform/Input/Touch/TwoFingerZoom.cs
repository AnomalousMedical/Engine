using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public class TwoFingerZoom : MessageEvent
    {
        public delegate void ZoomDelegate(EventLayer eventLayer, TwoFingerZoom zoomGesture);

        public event ZoomDelegate Zoom;
        public float lastPinchDistance = 0;
        bool didGesture;
        bool momentumStarted = false;
        bool gestureStarted = false;
        float momentum = 0.0f;
        float momentumDirection = 1.0f;
        float deceleration = 0.0f;
        float decelerationTime;
        float minimumMomentum;
        float zoomDelta = 0.0f;

        /// <summary>
        /// This event is fired when the gesture starts running.
        /// </summary>
        public event ZoomDelegate GestureStarted;

        /// <summary>
        /// This event is fired when momentum starts for this gesture. This will always fire the first frame momentum
        /// would start, even if this instance has been configured not to use momentum.
        /// </summary>
        public event ZoomDelegate MomentumStarted;

        /// <summary>
        /// This event is fired when momentum ends. This will always fire the first frame momentum
        /// would end, even if this instance has been configured not to use momentum.
        /// </summary>
        public event ZoomDelegate MomentumEnded;

        public TwoFingerZoom(Object eventLayerKey, float decelerationTime, float minimumMomentum)
            :base(eventLayerKey)
        {
            this.decelerationTime = decelerationTime;
            this.minimumMomentum = minimumMomentum;
        }

        /// <summary>
        /// Clear the momentum for this gesture.
        /// </summary>
        public void cancelMomentum()
        {
            momentum = 0.0f;
        }

        public float ZoomDelta
        {
            get
            {
                return zoomDelta;
            }
        }

        protected internal override void update(EventLayer eventLayer, bool allowProcessing, Clock clock)
        {
            var touches = eventLayer.EventManager.Touches;
            var fingers = touches.Fingers;
            if (fingers.Count == 2)
            {
                Finger finger1 = fingers[0];
                Finger finger2 = fingers[1];
                Vector2 finger1Vec = new Vector2(finger1.NrmlDeltaX, finger1.NrmlDeltaY);
                Vector2 finger2Vec = new Vector2(finger2.NrmlDeltaX, finger2.NrmlDeltaY);
                float finger1Len = finger1Vec.length2();
                float finger2Len = finger2Vec.length2();
                if (finger1Len > 0 || finger2Len > 0)
                {
                    if (finger1Len > 0 && finger2Len > 0)
                    {
                        float cosTheta = finger1Vec.dot(ref finger2Vec) / (finger1Vec.length() * finger2Vec.length());
                        if (cosTheta < -0.5f && Zoom != null)
                        {
                            computeZoom(eventLayer, ref didGesture, finger1, finger2, ref finger1Vec, ref finger2Vec);
                        }
                    }
                    else if (finger1Len == 0 && finger2Len > 0)
                    {
                        computeZoom(eventLayer, ref didGesture, finger1, finger2, ref finger1Vec, ref finger2Vec);
                    }
                    else if (finger2Len == 0 && finger1Len > 0)
                    {
                        computeZoom(eventLayer, ref didGesture, finger1, finger2, ref finger1Vec, ref finger2Vec);
                    }
                }
            }

            //Momentum
            if (!didGesture)
            {
                if (momentum > 0.0f)
                {
                    zoomDelta = momentum * momentumDirection;

                    if (!momentumStarted)
                    {
                        momentumStarted = true;
                        if (MomentumStarted != null)
                        {
                            MomentumStarted.Invoke(eventLayer, this);
                        }
                    }

                    momentum -= deceleration * clock.DeltaSeconds;
                    if (momentum < 0.0f)
                    {
                        momentum = 0.0f;
                    }
                    if (Zoom != null)
                    {
                        Zoom.Invoke(eventLayer, this);
                    }
                }
                else if(momentumStarted)
                {
                    zoomDelta = 0.0f;

                    momentumStarted = false;
                    gestureStarted = false;
                    if (MomentumEnded != null)
                    {
                        MomentumEnded.Invoke(eventLayer, this);
                    }
                }
            }
            didGesture = false;
        }

        private void computeZoom(EventLayer eventLayer, ref bool didGesture, Finger finger1, Finger finger2, ref Vector2 finger1Vec, ref Vector2 finger2Vec)
        {
            Vector2 finger1Pos = new Vector2(finger1.NrmlX, finger1.NrmlY);
            Vector2 finger2Pos = new Vector2(finger2.NrmlX, finger2.NrmlY);
            float currentPinchDistance = (finger1Pos - finger2Pos).length2();

            Vector2 vectorSum = finger1Vec - finger2Vec;
            momentumDirection = 1.0f;
            zoomDelta = vectorSum.length();
            momentum = zoomDelta;
            deceleration = momentum / decelerationTime;

            if (currentPinchDistance > lastPinchDistance)
            {
                zoomDelta = -zoomDelta;
                momentumDirection = -1.0f;
            }
            didGesture = true;
            lastPinchDistance = currentPinchDistance;

            if (!gestureStarted)
            {
                gestureStarted = true;
                if (GestureStarted != null)
                {
                    GestureStarted.Invoke(eventLayer, this);
                }
            }

            if (Zoom != null)
            {
                Zoom.Invoke(eventLayer, this);
            }
        }
    }
}
