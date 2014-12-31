﻿using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using Engine.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Anomalous.GuiFramework.Cameras
{
    public class CameraInputController : IDisposable
    {
        private const int UPDATE_DELAY = 500;

        enum Gesture : int
        {
            None = 0,
            Rotate = 1,
            Zoom = 2,
            Pan = 3
        }

        private static MouseButtonCode currentMouseButton = CamerasInterface.DefaultCameraButton;
        private static ButtonEvent RotateCamera;
        private static ButtonEvent PanCamera;
        private static ButtonEvent ZoomCamera;
        private static ButtonEvent ZoomInCamera;
        private static ButtonEvent ZoomOutCamera;
        private static ButtonEvent LockX;
        private static ButtonEvent LockY;

        private static FingerDragGesture rotateGesture;
        private static FingerDragGesture panGesture;
        private static FingerDragGesture zoomGesture;

        static CameraInputController()
        {
            RotateCamera = new ButtonEvent(CamerasInterface.MoveCameraEventLayer);
            RotateCamera.addButton(currentMouseButton);
            DefaultEvents.registerDefaultEvent(RotateCamera);

            PanCamera = new ButtonEvent(CamerasInterface.MoveCameraEventLayer);
            PanCamera.addButton(currentMouseButton);
            PanCamera.addButton(CamerasInterface.PanKey);
            DefaultEvents.registerDefaultEvent(PanCamera);

            ZoomCamera = new ButtonEvent(CamerasInterface.MoveCameraEventLayer);
            ZoomCamera.addButton(currentMouseButton);
            ZoomCamera.addButton(KeyboardButtonCode.KC_LMENU);
            DefaultEvents.registerDefaultEvent(ZoomCamera);

            ZoomInCamera = new ButtonEvent(CamerasInterface.MoveCameraEventLayer);
            ZoomInCamera.MouseWheelDirection = MouseWheelDirection.Up;
            DefaultEvents.registerDefaultEvent(ZoomInCamera);

            ZoomOutCamera = new ButtonEvent(CamerasInterface.MoveCameraEventLayer);
            ZoomOutCamera.MouseWheelDirection = MouseWheelDirection.Down;
            DefaultEvents.registerDefaultEvent(ZoomOutCamera);

            LockX = new ButtonEvent(CamerasInterface.MoveCameraEventLayer);
            LockX.addButton(KeyboardButtonCode.KC_C);
            DefaultEvents.registerDefaultEvent(LockX);

            LockY = new ButtonEvent(CamerasInterface.MoveCameraEventLayer);
            LockY.addButton(KeyboardButtonCode.KC_X);
            DefaultEvents.registerDefaultEvent(LockY);

            switch (CamerasInterface.TouchType)
            { 
                case TouchType.Screen:
                    zoomGesture = new FingerDragGesture(CamerasInterface.MoveCameraEventLayer, 2, 0.5f, 2f, 5);
                    DefaultEvents.registerDefaultEvent(zoomGesture);

                    rotateGesture = new FingerDragGesture(CamerasInterface.MoveCameraEventLayer, 1, 0.5f, 2f, 5);
                    DefaultEvents.registerDefaultEvent(rotateGesture);

                    panGesture = new FingerDragGesture(CamerasInterface.MoveCameraEventLayer, 3, 0.5f, 2f, 5);
                    DefaultEvents.registerDefaultEvent(panGesture);
                break;
            }
        }

        public static void changeMouseButton(MouseButtonCode newMouseButton)
        {
            RotateCamera.removeButton(currentMouseButton);
            PanCamera.removeButton(currentMouseButton);
            ZoomCamera.removeButton(currentMouseButton);

            currentMouseButton = newMouseButton;

            RotateCamera.addButton(currentMouseButton);
            PanCamera.addButton(currentMouseButton);
            ZoomCamera.addButton(currentMouseButton);
        }

        private bool currentlyInMotion;
        private EventManager eventManager;
        private TravelTracker travelTracker = new TravelTracker();
        private SceneViewController sceneViewController;
        private Gesture currentGesture = Gesture.None;
        private Timer mouseWheelTimer;
        private CameraPosition mouseUndoPosition;
        private CameraPosition touchUndoPosition;

        internal CameraInputController(SceneViewController sceneViewController, EventManager eventManager)
        {
            this.sceneViewController = sceneViewController;
            this.eventManager = eventManager;
            eventManager[CamerasInterface.MoveCameraEventLayer].OnUpdate += processInputEvents;

            mouseWheelTimer = new Timer(UPDATE_DELAY);
            mouseWheelTimer.SynchronizingObject = new ThreadManagerSynchronizeInvoke();
            mouseWheelTimer.AutoReset = false;
            mouseWheelTimer.Elapsed += mouseWheelTimer_Elapsed;

            RotateCamera.FirstFrameDownEvent += rotateCamera_FirstFrameDownEvent;
            RotateCamera.FirstFrameUpEvent += rotateCamera_FirstFrameUpEvent;

            if (zoomGesture != null)
            {
                zoomGesture.GestureStarted += zoomGesture_GestureStarted;
                zoomGesture.Dragged += zoomGesture_Dragged;
                zoomGesture.MomentumStarted += zoomGesture_MomentumStarted;
                zoomGesture.MomentumEnded += zoomGesture_MomentumEnded;

                rotateGesture.GestureStarted += rotateGesture_GestureStarted;
                rotateGesture.Dragged += rotateGesture_Dragged;
                rotateGesture.MomentumStarted += rotateGesture_MomentumStarted;
                rotateGesture.MomentumEnded += rotateGesture_MomentumEnded;

                panGesture.GestureStarted += panGesture_GestureStarted;
                panGesture.Dragged += panGesture_Dragged;
                panGesture.MomentumStarted += panGesture_MomentumStarted;
                panGesture.MomentumEnded += panGesture_MomentumEnded;
            }
        }

        public void Dispose()
        {
            eventManager[CamerasInterface.MoveCameraEventLayer].OnUpdate -= processInputEvents;

            RotateCamera.FirstFrameDownEvent -= rotateCamera_FirstFrameDownEvent;
            RotateCamera.FirstFrameUpEvent -= rotateCamera_FirstFrameUpEvent;

            if (zoomGesture != null)
            {
                zoomGesture.GestureStarted -= zoomGesture_GestureStarted;
                zoomGesture.Dragged -= zoomGesture_Dragged;
                zoomGesture.MomentumStarted -= zoomGesture_MomentumStarted;
                zoomGesture.MomentumEnded -= zoomGesture_MomentumEnded;

                rotateGesture.GestureStarted -= rotateGesture_GestureStarted;
                rotateGesture.Dragged -= rotateGesture_Dragged;
                rotateGesture.MomentumStarted -= rotateGesture_MomentumStarted;
                rotateGesture.MomentumEnded -= rotateGesture_MomentumEnded;

                panGesture.GestureStarted -= panGesture_GestureStarted;
                panGesture.Dragged -= panGesture_Dragged;
                panGesture.MomentumStarted -= panGesture_MomentumStarted;
                panGesture.MomentumEnded -= panGesture_MomentumEnded;
            }
            mouseWheelTimer.Dispose();
        }

        void rotateCamera_FirstFrameDownEvent(EventLayer eventLayer)
        {
            travelTracker.reset();
        }

        void rotateCamera_FirstFrameUpEvent(EventLayer eventLayer)
        {
            if(travelTracker.TraveledOverLimit)
            {
                eventLayer.alertEventsHandled();
            }
        }

        void processInputEvents(EventLayer eventLayer)
        {
            if (currentGesture == Gesture.None)
            {
                SceneViewWindow activeWindow = sceneViewController.ActiveWindow;
                if (activeWindow != null && eventLayer.EventProcessingAllowed)
                {
                    CameraMover cameraMover = activeWindow.CameraMover;
                    if (cameraMover.AllowManualMovement)
                    {
                        IntVector3 mouseCoords = eventLayer.Mouse.AbsolutePosition;

                        if (RotateCamera.FirstFrameDown)
                        {
                            eventLayer.makeFocusLayer();
                            currentlyInMotion = true;
                            eventLayer.alertEventsHandled();
                            if (mouseUndoPosition == null)
                            {
                                mouseUndoPosition = activeWindow.createCameraPosition();
                            }
                        }
                        else if (RotateCamera.FirstFrameUp)
                        {
                            eventLayer.clearFocusLayer();
                            currentlyInMotion = false;

                            if (mouseUndoPosition != null)
                            {
                                activeWindow.pushUndoState(mouseUndoPosition);
                                mouseUndoPosition = null;
                            }
                        }
                        mouseCoords = eventLayer.Mouse.RelativePosition;
                        if (currentlyInMotion)
                        {
                            if (PanCamera.HeldDown)
                            {
                                travelTracker.traveled(mouseCoords);
                                int x = mouseCoords.x;
                                int y = mouseCoords.y;
                                if (LockX.Down)
                                {
                                    x = 0;
                                }
                                if (LockY.Down)
                                {
                                    y = 0;
                                }
                                cameraMover.panFromMotion(x, y, eventLayer.Mouse.AreaWidth, eventLayer.Mouse.AreaHeight);
                                eventLayer.alertEventsHandled();
                            }
                            else if (cameraMover.AllowZoom && ZoomCamera.HeldDown)
                            {
                                travelTracker.traveled(mouseCoords);
                                cameraMover.zoomFromMotion(mouseCoords.y);
                                eventLayer.alertEventsHandled();
                            }
                            else if (cameraMover.AllowRotation && RotateCamera.HeldDown)
                            {
                                travelTracker.traveled(mouseCoords);
                                int x = mouseCoords.x;
                                int y = mouseCoords.y;
                                if (LockX.Down)
                                {
                                    x = 0;
                                }
                                if (LockY.Down)
                                {
                                    y = 0;
                                }
                                cameraMover.rotateFromMotion(x, y);
                                eventLayer.alertEventsHandled();
                            }
                        }
                        if (cameraMover.AllowZoom)
                        {
                            if (ZoomInCamera.Down)
                            {
                                if (mouseUndoPosition == null)
                                {
                                    mouseUndoPosition = activeWindow.createCameraPosition();
                                }
                                mouseWheelTimer.Stop();
                                mouseWheelTimer.Start();
                                cameraMover.incrementZoom(-1);
                                eventLayer.alertEventsHandled();
                            }
                            else if (ZoomOutCamera.Down)
                            {
                                if (mouseUndoPosition == null)
                                {
                                    mouseUndoPosition = activeWindow.createCameraPosition();
                                }
                                mouseWheelTimer.Stop();
                                mouseWheelTimer.Start();
                                cameraMover.incrementZoom(1);
                                eventLayer.alertEventsHandled();
                            }
                        }
                    }
                }
            }
        }

        void rotateGesture_GestureStarted(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (eventLayer.EventProcessingAllowed && currentGesture <= Gesture.Rotate)
            {
                createTouchUndo();
                travelTracker.reset();
            }
        }

        void rotateGesture_Dragged(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (eventLayer.EventProcessingAllowed && currentGesture <= Gesture.Rotate)
            {
                currentGesture = Gesture.Rotate;
                SceneViewWindow sceneView = sceneViewController.ActiveWindow;
                if (sceneView != null)
                {
                    sceneView.CameraMover.rotateFromMotion((int)gesture.DeltaX, (int)gesture.DeltaY);
                }
                travelTracker.traveled((int)gesture.DeltaX, (int)gesture.DeltaY);
                eventLayer.alertEventsHandled();
            }
        }

        void rotateGesture_MomentumStarted(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (eventLayer.EventProcessingAllowed)
            {
                if (currentGesture <= Gesture.Rotate && travelTracker.TraveledOverLimit)
                {
                    eventLayer.alertEventsHandled();
                }
            }
            else
            {
                gesture.cancelMomentum();
            }
        }

        void rotateGesture_MomentumEnded(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (currentGesture <= Gesture.Rotate)
            {
                commitTouchUndo();
                currentGesture = Gesture.None;
            }
        }

        void panGesture_GestureStarted(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (eventLayer.EventProcessingAllowed)
            {
                createTouchUndo();
            }
        }

        void panGesture_Dragged(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (eventLayer.EventProcessingAllowed && currentGesture <= Gesture.Pan)
            {
                currentGesture = Gesture.Pan;
                SceneViewWindow sceneView = sceneViewController.ActiveWindow;
                if (sceneView != null)
                {
                    sceneView.CameraMover.panFromMotion((int)gesture.DeltaX, (int)gesture.DeltaY, eventLayer.Mouse.AreaWidth, eventLayer.Mouse.AreaHeight);
                }
                eventLayer.alertEventsHandled();
            }
        }

        void panGesture_MomentumStarted(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (eventLayer.EventProcessingAllowed)
            {
                if (currentGesture <= Gesture.Pan)
                {
                    eventLayer.alertEventsHandled();
                }
            }
            else
            {
                gesture.cancelMomentum();
            }
        }

        void panGesture_MomentumEnded(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (currentGesture <= Gesture.Pan)
            {
                commitTouchUndo();
                currentGesture = Gesture.None;
            }
        }

        void zoomGesture_GestureStarted(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (eventLayer.EventProcessingAllowed)
            {
                createTouchUndo();
            }
        }

        void zoomGesture_Dragged(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (eventLayer.EventProcessingAllowed && currentGesture <= Gesture.Zoom)
            {
                currentGesture = Gesture.Zoom;
                SceneViewWindow sceneView = sceneViewController.ActiveWindow;
                if (sceneView != null)
                {
                    sceneView.CameraMover.zoomFromMotion((int)gesture.DeltaY);
                }
                eventLayer.alertEventsHandled();
            }
        }

        void zoomGesture_MomentumStarted(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (eventLayer.EventProcessingAllowed)
            {
                if (currentGesture <= Gesture.Zoom)
                {
                    eventLayer.alertEventsHandled();
                }
            }
            else
            {
                gesture.cancelMomentum();
            }
        }

        void zoomGesture_MomentumEnded(EventLayer eventLayer, FingerDragGesture gesture)
        {
            if (currentGesture <= Gesture.Zoom)
            {
                commitTouchUndo();
                currentGesture = Gesture.None;
            }
        }

        void Touches_AllFingersReleased(Touches obj)
        {
            currentGesture = Gesture.None;
        }

        private void createTouchUndo()
        {
            SceneViewWindow sceneView = sceneViewController.ActiveWindow;
            if (touchUndoPosition == null && sceneView != null)
            {
                touchUndoPosition = sceneView.createCameraPosition();
            }
        }

        private void commitTouchUndo()
        {
            if(touchUndoPosition != null)
            {
                SceneViewWindow activeWindow = sceneViewController.ActiveWindow;
                if (activeWindow != null)
                {
                    activeWindow.pushUndoState(touchUndoPosition);
                }
                touchUndoPosition = null;
            }
        }

        void mouseWheelTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!currentlyInMotion && mouseUndoPosition != null)
            {
                SceneViewWindow activeWindow = sceneViewController.ActiveWindow;
                if (activeWindow != null)
                {
                    activeWindow.pushUndoState(mouseUndoPosition);
                }
                mouseUndoPosition = null;
            }
        }
    }
}
