﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using EngineMath;

namespace Engine.Renderer
{
    enum CameraEvents
    {
        RotateCamera,
        PanCamera,
        ZoomCamera,
    }

    public class OrbitCameraController : UpdateListener
    {
        #region Static

        static OrbitCameraController()
        {
            MessageEvent rotateCamera = new MessageEvent(CameraEvents.RotateCamera);
            rotateCamera.addButton(MouseButtonCode.MB_BUTTON1);
            DefaultEvents.registerDefaultEvent(rotateCamera);

            MessageEvent panCamera = new MessageEvent(CameraEvents.PanCamera);
            panCamera.addButton(MouseButtonCode.MB_BUTTON1);
            panCamera.addButton(KeyboardButtonCode.KC_LCONTROL);
            DefaultEvents.registerDefaultEvent(panCamera);

            MessageEvent zoomCamera = new MessageEvent(CameraEvents.ZoomCamera);
            zoomCamera.addButton(MouseButtonCode.MB_BUTTON1);
            zoomCamera.addButton(KeyboardButtonCode.KC_LMENU);
            DefaultEvents.registerDefaultEvent(zoomCamera);
        }

        private const float HALF_PI = (float)Math.PI / 2.0f - 0.001f;
        private const int SCROLL_SCALE = 5;

        #endregion Static

        private CameraControl camera;
        private EventManager events;
        private Vector3 normalDirection;
        private Vector3 left;
        private float orbitDistance;
        private float yaw;
        private float pitch;
        private bool currentlyInMotion;
        private Vector3 lookAt;
        private Vector3 translation;

        public OrbitCameraController(CameraControl camera, EventManager eventManager)
        {
            this.camera = camera;
            this.events = eventManager;
            translation = camera.Translation;
            lookAt = camera.LookAt;
            computeStartingValues(translation - lookAt);
        }

        public void sendUpdate(Clock clock)
        {
            Vector3 mouseCoords = events.Mouse.getAbsMouse();
            bool activeWindow = true;// MotionValidator == null || (MotionValidator.allowMotion((int)mouseCoords.x, (int)mouseCoords.y) && MotionValidator.isActiveWindow());
            if (events[CameraEvents.RotateCamera].FirstFrameDown)
            {
                if (activeWindow)
                {
                    currentlyInMotion = true;
                }
            }
            else if (events[CameraEvents.RotateCamera].FirstFrameUp)
            {
                currentlyInMotion = false;
            }
            mouseCoords = events.Mouse.getRelMouse();
            if (currentlyInMotion)
            {
                if (events[CameraEvents.PanCamera].Down)
                {
                    lookAt += left * (mouseCoords.x / (events.Mouse.getMouseAreaWidth() * SCROLL_SCALE) * orbitDistance);
                    Vector3 relUp = left.cross(ref normalDirection);
                    lookAt += relUp * (mouseCoords.y / (events.Mouse.getMouseAreaHeight() * SCROLL_SCALE) * orbitDistance);
                    camera.Translation = lookAt + normalDirection * orbitDistance;
                }
                else if (events[CameraEvents.ZoomCamera].Down)
                {
                    orbitDistance += mouseCoords.y;
                    if (orbitDistance < 0.0f)
                    {
                        orbitDistance = 0.0f;
                    }
                    //camera.setOrthoWindowHeight(orbitDistance);
                    Vector3 newTrans = normalDirection * orbitDistance + lookAt;
                    camera.Translation = newTrans;
                }
                else if (events[CameraEvents.RotateCamera].Down)
                {
                    yaw += mouseCoords.x / -100.0f;
                    pitch += mouseCoords.y / 100.0f;
                    if (pitch > HALF_PI)
                    {
                        pitch = HALF_PI;
                    }
                    if (pitch < -HALF_PI)
                    {
                        pitch = -HALF_PI;
                    }

                    Quaternion yawRot = new Quaternion(ref Vector3.Up, yaw);
                    Quaternion pitchRot = new Quaternion(ref Vector3.Left, pitch);

                    normalDirection = Quaternion.quatRotate(yawRot * pitchRot, Vector3.Backward);
                    Vector3 newTrans = normalDirection * orbitDistance + lookAt;
                    camera.Translation = newTrans;
                    camera.LookAt = lookAt;
                    left = normalDirection.cross(ref Vector3.Up);
                }
            }
            if (activeWindow)
            {
                if (mouseCoords.z != 0)
                {
                    if (mouseCoords.z < 0)
                    {
                        orbitDistance += 3.6f;
                    }
                    else if (mouseCoords.z > 0)
                    {
                        orbitDistance -= 3.6f;
                        if (orbitDistance < 0.0f)
                        {
                            orbitDistance = 0.0f;
                        }
                    }
                    //camera.setOrthoWindowHeight(orbitDistance);
                    Vector3 newTrans = normalDirection * orbitDistance + lookAt;
                    camera.Translation = newTrans;
                }
            }
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }

        /// <summary>
        /// Set the camera to the given position looking at the given point.
        /// </summary>
        /// <param name="position">The position to set the camera at.</param>
        /// <param name="lookAt">The look at point of the camera.</param>
        public void setNewPosition(Vector3 position, Vector3 lookAt)
        {
            this.lookAt = lookAt;
            computeStartingValues(position - lookAt);
            //camera.setOrthoWindowHeight(orbitDistance);
            Vector3 newTrans = normalDirection * orbitDistance + lookAt;
            camera.Translation = newTrans;
            camera.LookAt = lookAt;
        }

        /// <summary>
        /// Helper function to compute the starting orbitDistance, yaw, pitch,
        /// normalDirection and left values.
        /// </summary>
        /// <param name="localTrans">The translation of the camera relative to the look at point.</param>
        private void computeStartingValues(Vector3 localTrans)
        {
            //Compute the orbit distance, this is the distance from the location to the look at point.
            orbitDistance = localTrans.length();

            //Compute the yaw.
            float localY = localTrans.y;
            localTrans.y = 0;
            yaw = Vector3.Backward.angle(ref localTrans);
            if (localTrans.x < 0)
            {
                yaw = -yaw;
            }

            //Compute the pitch by rotating the local translation to -yaw.
            localTrans.y = localY;
            localTrans = Quaternion.quatRotate(new Quaternion(ref Vector3.Up, -yaw), localTrans);
            localTrans.x = 0;
            if (localTrans.z >= 0.0f)
            {
                pitch = Vector3.Backward.angle(ref localTrans);
            }
            else
            {
                pitch = Vector3.Forward.angle(ref localTrans);
            }

            //Compute the normal direction and the left vector.
            Quaternion yawRot = new Quaternion(ref Vector3.Up, yaw);
            Quaternion pitchRot = new Quaternion(ref Vector3.Left, pitch);
            normalDirection = Quaternion.quatRotate(yawRot * pitchRot, Vector3.Backward);
            left = normalDirection.cross(ref Vector3.Up);
        }
    }
}
