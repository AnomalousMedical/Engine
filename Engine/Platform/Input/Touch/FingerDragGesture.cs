using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    /// <summary>
    /// A Gesture for one or more fingers traveling in the same direction.
    /// </summary>
    public class FingerDragGesture : Gesture
    {
        public delegate void Delegate(EventLayer eventLayer, FingerDragGesture gesture);

        /// <summary>
        /// This event is fired when the gesture is dragged, this will fire for both fingers down and momentum.
        /// </summary>
        public event Delegate Dragged;

        /// <summary>
        /// This event is fired when the gesture starts running.
        /// </summary>
        public event Delegate GestureStarted;

        /// <summary>
        /// This event is fired when momentum starts for this gesture. This will always fire the first frame momentum
        /// would start, even if this instance has been configured not to use momentum.
        /// </summary>
        public event Delegate MomentumStarted;

        /// <summary>
        /// This event is fired when momentum ends. This will always fire the first frame momentum
        /// would end, even if this instance has been configured not to use momentum.
        /// </summary>
        public event Delegate MomentumEnded;
        
        private int fingerCount;
        private bool didGesture;
        private bool gestureStarted = false;
        private bool momentumStarted = false;
        private Vector2 momentum = new Vector2();
        private Vector2 momentumDirection = new Vector2();
        private Vector2 deceleration = new Vector2();
        private Vector2[] averageSpeed;
        private int averageSpeedCursor = 0;
        private float decelerationTime;
        private float minimumMomentum;

        public FingerDragGesture(Object eventLayerKey, int fingerCount, float decelerationTime, float minimumMomentum, int averageSpeedFrames)
            :base(eventLayerKey)
        {
            this.fingerCount = fingerCount;
            this.decelerationTime = decelerationTime;
            this.minimumMomentum = minimumMomentum;
            averageSpeed = new Vector2[averageSpeedFrames];
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
                Vector2 primaryFingerVec = new Vector2(fingers[0].PixelDeltaX, fingers[0].PixelDeltaY);
                float primaryFingerLen = primaryFingerVec.length();
                Vector2 longestLengthVec = primaryFingerVec;
                float longestLength = primaryFingerLen;
                if (primaryFingerLen > 0)
                {
                    bool allVectorsSameDirection = true;
                    for (int i = 1; i < fingerCount && allVectorsSameDirection; ++i)
                    {
                        Vector2 testFingerVec = new Vector2(fingers[i].PixelDeltaX, fingers[i].PixelDeltaY);
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
                    if (allVectorsSameDirection && Dragged != null)
                    {
                        if (!gestureStarted)
                        {
                            for (int i = 0; i < averageSpeed.Length; ++i)
                            {
                                averageSpeed[i] = new Vector2(0, 0);
                            }

                            gestureStarted = true;
                            if(GestureStarted != null)
                            {
                                GestureStarted.Invoke(eventLayer, this);
                            }
                        }

                        didGesture = true;
                        DeltaX = longestLengthVec.x;
                        DeltaY = longestLengthVec.y;
                        averageSpeed[averageSpeedCursor] = longestLengthVec;
                        averageSpeedCursor++;
                        averageSpeedCursor %= averageSpeed.Length;
                        Dragged.Invoke(eventLayer, this);
                        
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
                if (gestureStarted)
                {
                    momentumStarted = true;
                    momentum = new IntVector2(0, 0);
                    for (int i = 0; i < averageSpeed.Length; ++i)
                    {
                        momentum += averageSpeed[i];
                    }
                    momentum /= averageSpeed.Length;

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

                    if (MomentumStarted != null)
                    {
                        MomentumStarted(eventLayer, this);
                    }
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
                    if (Dragged != null)
                    {
                        Vector2 finalMomentum = momentum * momentumDirection;
                        DeltaX = finalMomentum.x;
                        DeltaY = finalMomentum.y;
                        Dragged.Invoke(eventLayer, this);
                    }
                }
                else if(momentumStarted)
                {
                    momentumStarted = false;
                    if(MomentumEnded != null)
                    {
                        MomentumEnded.Invoke(eventLayer, this);
                    }
                }
            }

            didGesture = false;
        }
    }
}
