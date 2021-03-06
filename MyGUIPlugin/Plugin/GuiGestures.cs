﻿using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    class GuiGestures : MessageEvent
    {
        private bool didGesture;
        private bool gestureStarted = false;

        private Vector2[] averageSpeed;
        private int averageSpeedCursor = 0;
        private float decelerationTime = 0.5f;
        private float minimumMomentum = 2;

        private Vector2 momentum = new Vector2();
        private Vector2 momentumDirection = new Vector2();
        private Vector2 deceleration = new Vector2();
        private IntVector2 lastAbsolutePosition;

        public GuiGestures(Object eventLayerKey, int averageSpeedFrames = 5)
            :base(eventLayerKey)
        {
            averageSpeed = new Vector2[averageSpeedFrames];
        }

        protected override void update(EventLayer eventLayer, bool allowProcessing, Clock clock)
        {
            var touches = eventLayer.EventManager.Touches;
            var fingers = touches.Fingers;
            if (fingers.Count == 1)
            {
                Vector2 delta = new Vector2(fingers[0].PixelDeltaX, fingers[0].PixelDeltaY);
                lastAbsolutePosition = new IntVector2(fingers[0].PixelX, fingers[0].PixelY);
                if(InputManager.Instance.injectScrollGesture(lastAbsolutePosition.x, lastAbsolutePosition.y, fingers[0].PixelDeltaX, fingers[0].PixelDeltaY))
                {
                    if (!gestureStarted)
                    {
                        for (int i = 0; i < averageSpeed.Length; ++i)
                        {
                            averageSpeed[i] = new Vector2(0, 0);
                        }
                    }

                    didGesture = true;
                    gestureStarted = true;
                    averageSpeed[averageSpeedCursor] = delta;
                    averageSpeedCursor++;
                    averageSpeedCursor %= averageSpeed.Length;
                    eventLayer.alertEventsHandled();
                }
                else //Check to see if something will need the mouse clicked on it, if so block events
                {
                    Widget widget = LayerManager.Instance.getWidgetFromPoint(fingers[0].PixelX, fingers[0].PixelY);
                    if(widget != null && widget.NeedMouseFocus)
                    {
                        eventLayer.alertEventsHandled();
                    }
                }
            }

            if(!didGesture)
            {
                if(gestureStarted)
                {
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
                    Vector2 finalMomentum = momentum * momentumDirection;
                    InputManager.Instance.injectScrollGesture(lastAbsolutePosition.x, lastAbsolutePosition.y, (int)finalMomentum.x, (int)finalMomentum.y);
                }
            }
            didGesture = false;
        }
    }
}
