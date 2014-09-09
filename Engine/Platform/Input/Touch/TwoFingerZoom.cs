﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public class TwoFingerZoom : Gesture
    {
        public delegate void ZoomDelegate(EventLayer eventLayer, float zoomDelta);

        public event ZoomDelegate Zoom;
        public float lastPinchDistance = 0;
        bool didGesture;
        float momentum = 0.0f;
        float momentumDirection = 1.0f;
        float deceleration = 0.0f;
        float decelerationTime;
        float minimumMomentum;

        public TwoFingerZoom(Object eventLayerKey, float decelerationTime, float minimumMomentum)
            :base(eventLayerKey)
        {
            this.decelerationTime = decelerationTime;
            this.minimumMomentum = minimumMomentum;
        }

        protected override bool processFingers(EventLayer eventLayer, Touches touches)
        {
            var fingers = touches.Fingers;
            if (fingers.Count == 2)
            {
                Finger finger1 = fingers[0];
                Finger finger2 = fingers[1];
                Vector2 finger1Vec = new Vector2(finger1.NrmlDeltaX, finger1.NrmlDeltaY);
                Vector2 finger2Vec = new Vector2(finger2.NrmlDeltaX, finger2.NrmlDeltaY);
                float finger1Len = finger1Vec.length2();
                float finger2Len = finger2Vec.length2();
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

            return didGesture;
        }

        protected override void additionalProcessing(EventLayer eventLayer, Clock clock)
        {
            if (!didGesture)
            {
                if (momentum > 0.0f)
                {
                    momentum -= deceleration * clock.DeltaSeconds;
                    if (momentum < 0.0f)
                    {
                        momentum = 0.0f;
                    }
                    if (Zoom != null)
                    {
                        Zoom.Invoke(eventLayer, momentum * momentumDirection);
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
            float sumLength = vectorSum.length();
            momentum = sumLength;
            if (momentum < minimumMomentum)
            {
                momentum = 0.0f;
            }
            deceleration = momentum / decelerationTime;

            if (currentPinchDistance > lastPinchDistance)
            {
                sumLength = -sumLength;
                momentumDirection = -1.0f;
            }
            didGesture = true;
            lastPinchDistance = currentPinchDistance;
            if (Zoom != null)
            {
                Zoom.Invoke(eventLayer, sumLength);
            }
        }
    }
}