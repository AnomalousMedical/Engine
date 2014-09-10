using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public class MultiFingerScrollGesture : Gesture
    {
        public delegate void ScrollDelegate(EventLayer eventLayer, MultiFingerScrollGesture gesture);
        public event ScrollDelegate Scroll;
        public event ScrollDelegate MomentumStarted;
        private int fingerCount;
        private bool didGesture;
        private bool gestureStarted = false;
        private Vector2 momentum = new Vector2();
        private Vector2 momentumDirection = new Vector2();
        private Vector2 deceleration = new Vector2();
        private float decelerationTime;
        private float minimumMomentum;

        public MultiFingerScrollGesture(Object eventLayerKey, int fingerCount, float decelerationTime, float minimumMomentum)
            :base(eventLayerKey)
        {
            this.fingerCount = fingerCount;
            this.decelerationTime = decelerationTime;
            this.minimumMomentum = minimumMomentum;
        }

        /// <summary>
        /// The change in X.
        /// </summary>
        public float DeltaX { get; set; }

        /// <summary>
        /// The change in Y.
        /// </summary>
        public float DeltaY { get; set; }

        /// <summary>
        /// This will be true if the gesture is firing because of fingers being down
        /// and false if it is firing because of momentum.
        /// </summary>
        public bool ActivlyGesturing
        {
            get
            {
                return didGesture;
            }
        }

        protected override bool processFingers(EventLayer eventLayer, Touches touches)
        {
            var fingers = touches.Fingers;
            if (fingers.Count == fingerCount)
            {
                Vector2 primaryFingerVec = new Vector2(fingers[0].NrmlDeltaX, fingers[0].NrmlDeltaY);
                float primaryFingerLen = primaryFingerVec.length();
                Vector2 longestLengthVec = primaryFingerVec;
                float longestLength = primaryFingerLen;
                if (primaryFingerLen > 0)
                {
                    bool allVectorsSameDirection = true;
                    for (int i = 1; i < fingerCount && allVectorsSameDirection; ++i)
                    {
                        Vector2 testFingerVec = new Vector2(fingers[i].NrmlDeltaX, fingers[i].NrmlDeltaY);
                        float testFingerLen = testFingerVec.length();
                        if (testFingerLen > 0)
                        {
                            float cosTheta = primaryFingerVec.dot(ref testFingerVec) / (primaryFingerLen * testFingerLen);
                            if (cosTheta > 0.5f)
                            {
                                if (testFingerLen > longestLength)
                                {
                                    longestLengthVec = testFingerVec;
                                    longestLength = testFingerLen;
                                }
                            }
                            else
                            {
                                allVectorsSameDirection = false;
                            }
                        }
                    }
                    if (allVectorsSameDirection && Scroll != null)
                    {
                        gestureStarted = true;
                        didGesture = true;
                        DeltaX = longestLengthVec.x;
                        DeltaY = longestLengthVec.y;
                        Scroll.Invoke(eventLayer, this);
                        momentum = longestLengthVec;
                        momentumDirection = new Vector2(1.0f, 1.0f);
                        if (momentum.x < 0.0f)
                        {
                            momentum.x = -momentum.x;
                            momentumDirection.x = -1.0f;
                        }
                        if (momentum.y < 0.0f)
                        {
                            momentum.y = -momentum.y;
                            momentumDirection.y = -1.0f;
                        }
                        if (momentum.x < minimumMomentum)
                        {
                            momentum.x = 0.0f;
                        }
                        if (momentum.y < minimumMomentum)
                        {
                            momentum.y = 0.0f;
                        }
                        deceleration = momentum / decelerationTime;
                    }
                }
                else if (gestureStarted)
                {
                    //If we have done the gesture once and the correct number of fingers are down, keep reporting that we did the gesture
                    didGesture = true;
                }
            }

            return didGesture;
        }

        protected override void additionalProcessing(EventLayer eventLayer, Clock clock)
        {
            if (!didGesture)
            {
                if (gestureStarted && MomentumStarted != null)
                {
                    MomentumStarted(eventLayer, this);
                }
                gestureStarted = false;

                if (momentum.length2() != 0.0f)
                {
                    momentum -= deceleration * clock.DeltaSeconds;
                    if (momentum.x < 0.0f)
                    {
                        momentum.x = 0.0f;
                    }
                    if (momentum.y <= 0.0f)
                    {
                        momentum.y = 0.0f;
                    }
                    if (Scroll != null)
                    {
                        Vector2 finalMomentum = momentum * momentumDirection;
                        DeltaX = finalMomentum.x;
                        DeltaY = finalMomentum.y;
                        Scroll.Invoke(eventLayer, this);
                    }
                }
            }

            didGesture = false;
        }
    }
}
