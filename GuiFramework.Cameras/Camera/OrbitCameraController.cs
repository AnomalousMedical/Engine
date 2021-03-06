﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;
using Logging;
using MyGUIPlugin;
using OgrePlugin;

namespace Anomalous.GuiFramework.Cameras
{
    public class OrbitCameraController : CameraMover
    {
        private const float HALF_PI = (float)Math.PI / 2.0f - 0.001f;
        private const float SCROLL_SCALE = 5.0f;

        private Vector3 boundMax;
        private Vector3 boundMin;
        private float minOrbitDistance;
        private float maxOrbitDistance;

        private CameraPositioner camera;

        //These three vectors form the axis relative to the current rotation.
        private Vector3 normalDirection; //z
        private Vector3 rotatedLeft; //x
        private Vector3 rotatedUp; //y

        private float orbitDistance;
        private float yaw;
        private float pitch;
        private Vector3 lookAt;
        private Vector3 translation;

        private bool automaticMovement = false;
        private float totalTime = 0.0f;
        private float animationDuration = 0.0f;
        private Vector3 startLookAt;
        private float startOrbitDistance;
        private Quaternion startRotation;
        private Vector3 targetLookAt;
        private Vector3 targetTranslation;
        private float targetOrbitDistance;
        private Quaternion targetRotation;
        float targetYaw;
        float targetPitch;
        Vector3 targetNormal;
        Vector3 targetRotatedUp;
        Vector3 targetRotatedLeft;

        private bool allowRotation = true;
        private bool allowZoom = true;
        private EasingFunction easingFunction = EasingFunction.EaseOutQuadratic;
        private UpdateTimer timer;

        public OrbitCameraController(Vector3 translation, Vector3 lookAt, Vector3 boundMin, Vector3 boundMax, float minOrbitDistance, float maxOrbitDistance)
        {
            this.camera = null;
            this.translation = targetTranslation = translation;
            this.lookAt = targetLookAt = lookAt;
            this.boundMax = boundMax;
            this.boundMin = boundMin;
            this.minOrbitDistance = minOrbitDistance;
            this.maxOrbitDistance = maxOrbitDistance;
            computeStartingValues(translation - lookAt, out orbitDistance, out yaw, out pitch, out normalDirection, out rotatedUp, out rotatedLeft);
        }

        public override void Dispose()
        {
            if (timer != null)
            {
                timer.removeUpdateListener(this);
            }
            base.Dispose();
        }

        public void setUpdateTimer(UpdateTimer timer)
        {
            if(timer != null)
            {
                timer.removeUpdateListener(this);
            }
            this.timer = timer;
            if (timer != null)
            {
                timer.addUpdateListener(this);
            }
        }

        public override void sendUpdate(Clock clock)
        {
            if (camera != null)
            {
                if (automaticMovement)
                {
                    totalTime += clock.DeltaSeconds;
                    if (totalTime > animationDuration)
                    {
                        totalTime = animationDuration;
                        automaticMovement = false;

                        orbitDistance = targetOrbitDistance;
                        yaw = targetYaw;
                        pitch = targetPitch;
                        normalDirection = targetNormal;
                        rotatedUp = targetRotatedUp;
                        rotatedLeft = targetRotatedLeft;
                    }
                    float slerpAmount = EasingFunctions.Ease(easingFunction, 0f, 1f, totalTime, animationDuration);
                    this.lookAt = startLookAt.lerp(ref targetLookAt, ref slerpAmount);
                    Quaternion rotation = startRotation.slerp(ref targetRotation, slerpAmount);
                    //If the rotation is not a valid number just use the target rotation
                    if (!rotation.isNumber())
                    {
                        rotation = targetRotation;
                    }
                    Vector3 currentNormalDirection = Quaternion.quatRotate(ref rotation, ref Vector3.Backward);
                    float currentOrbit = startOrbitDistance + (targetOrbitDistance - startOrbitDistance) * slerpAmount;
                    updateTranslation(currentNormalDirection * currentOrbit + lookAt);
                    camera.LookAt = lookAt;
                }
            }
        }

        public override void panFromMotion(int x, int y, int areaWidth, int areaHeight)
        {
            float scaleFactor = orbitDistance > 5.0f ? orbitDistance : 5.0f;
            lookAt += rotatedLeft * (x / (areaWidth * SCROLL_SCALE) * scaleFactor);
            lookAt += rotatedUp * (y / (areaHeight * SCROLL_SCALE) * scaleFactor);
            moveLookAt();
            stopMaintainingIncludePoint();
            targetTranslation = translation;
            targetLookAt = lookAt;
        }

        public override void zoomFromMotion(int y)
        {
            orbitDistance += ZoomMultiple * y + y;
            moveZoom();
            stopMaintainingIncludePoint();
            targetTranslation = translation;
            targetLookAt = lookAt;
        }

        public override void rotateFromMotion(int x, int y)
        {
            yaw += x / -100.0f;
            pitch += y / 100.0f;
            moveCameraYawPitch();
            stopMaintainingIncludePoint();
            targetTranslation = translation;
            targetLookAt = lookAt;
        }

        public override void incrementZoom(int zoomDirection)
        {
            float zoomAmount = (ZoomMultiple * 60f + 3.6f) * zoomDirection;
            orbitDistance += zoomAmount;
            moveZoom();
            stopMaintainingIncludePoint();
            targetTranslation = translation;
            targetLookAt = lookAt;
        }

        public float ZoomMultiple
        {
            get
            {
                return (Translation - LookAt).length2() / 10000f * 0.0132529322204644f;
            }
        }

        private void moveZoom()
        {
            if (orbitDistance < minOrbitDistance)
            {
                orbitDistance = minOrbitDistance;
            }
            if (orbitDistance > maxOrbitDistance)
            {
                orbitDistance = maxOrbitDistance;
            }
            updateTranslation(normalDirection * orbitDistance + lookAt);
        }

        private void moveLookAt()
        {
            //Restrict look at position
            if (lookAt.x > boundMax.x)
            {
                lookAt.x = boundMax.x;
            }
            else if (lookAt.x < boundMin.x)
            {
                lookAt.x = boundMin.x;
            }
            if (lookAt.y > boundMax.y)
            {
                lookAt.y = boundMax.y;
            }
            else if (lookAt.y < boundMin.y)
            {
                lookAt.y = boundMin.y;
            }
            if (lookAt.z > boundMax.z)
            {
                lookAt.z = boundMax.z;
            }
            else if (lookAt.z < boundMin.z)
            {
                lookAt.z = boundMin.z;
            }

            updateTranslation(lookAt + normalDirection * orbitDistance);
        }

        /// <summary>
        /// Helper funciton to move the camera when the yaw and pitch have changed.
        /// </summary>
        private void moveCameraYawPitch()
        {
            if (pitch > HALF_PI)
            {
                pitch = HALF_PI;
            }
            if (pitch < -HALF_PI)
            {
                pitch = -HALF_PI;
            }

            Quaternion yawRot = new Quaternion(Vector3.Up, yaw);
            Quaternion pitchRot = new Quaternion(Vector3.Left, pitch);

            Quaternion rotation = yawRot * pitchRot;
            normalDirection = Quaternion.quatRotate(ref rotation, ref Vector3.Backward);
            rotatedUp = Quaternion.quatRotate(ref rotation, ref Vector3.Up);
            rotatedLeft = normalDirection.cross(ref rotatedUp);

            updateTranslation(normalDirection * orbitDistance + lookAt);
            camera.LookAt = lookAt;
        }

        /// <summary>
        /// set the current camera for this controller. This can be set to null to disable the controller.
        /// </summary>
        /// <param name="camera">The camera to use.</param>
        public override void setCamera(CameraPositioner camera)
        {
            this.camera = camera;
        }

        public override void setNewPosition(Vector3 translation, Vector3 lookAt, float duration, EasingFunction easingFunction)
        {
            this.easingFunction = easingFunction;
            animationDuration = duration;
            if (animationDuration < 0.001f) //Be sure the duration is not 0.
            {
                animationDuration = 0.001f;
            }
            if (camera != null)
            {
                //If the camera is currently moving the final positions are not yet recorded so do that now.
                if (automaticMovement)
                {
                    computeStartingValues(this.translation - this.lookAt, out orbitDistance, out yaw, out pitch, out normalDirection, out rotatedUp, out rotatedLeft);
                }

                //Starting position
                startLookAt = this.lookAt;
                startOrbitDistance = orbitDistance;

                //Target position
                Vector3 localVec = translation - lookAt;
                computeStartingValues(localVec, out targetOrbitDistance, out targetYaw, out targetPitch, out targetNormal, out targetRotatedUp, out targetRotatedLeft);
                this.targetLookAt = lookAt;
                this.targetTranslation = translation;

                //Rotations
                Quaternion yawRot = new Quaternion(Vector3.Up, yaw);
                Quaternion pitchRot = new Quaternion(Vector3.Left, pitch);
                Quaternion targetYawRot = new Quaternion(Vector3.Up, targetYaw);
                Quaternion targetPitchRot = new Quaternion(Vector3.Left, targetPitch);
                if (targetYaw < yaw)
                {
                    float oneRotYaw = targetYaw + 2f * (float)Math.PI;
                    float oneRotYawMinus180 = oneRotYaw - (float)Math.PI;
                    if (yaw > oneRotYawMinus180 && yaw < oneRotYaw)
                    {
                        targetYawRot = new Quaternion(Vector3.Up, oneRotYaw);
                    }
                }
                if (targetYaw > yaw)
                {
                    float yawDifference = targetYaw - yaw;
                    if (yawDifference > Math.PI)
                    {
                        float negRot = targetYaw - 2f * (float)Math.PI;
                        targetYawRot = new Quaternion(Vector3.Up, negRot);
                    }
                }
                startRotation = yawRot * pitchRot;
                targetRotation = targetYawRot * targetPitchRot;
                automaticMovement = true;
                totalTime = 0.0f;
            }
        }

        public override void immediatlySetPosition(Vector3 translation, Vector3 lookAt)
        {
            this.translation = translation;
            this.lookAt = lookAt;
            computeStartingValues(translation - lookAt, out orbitDistance, out yaw, out pitch, out normalDirection, out rotatedUp, out rotatedLeft);
            if(camera != null)
            {
                updateTranslation(translation);
                camera.LookAt = lookAt;
            }
            targetTranslation = translation;
            targetLookAt = lookAt;
        }

        public override void processIncludePoint(Camera camera)
        {
            if (currentIncludePoint.HasValue)
            {
                float duration = GuiFrameworkCamerasInterface.CameraTransitionTime;
                Vector3 inclLookAt = LookAt;
                Vector3 inclTrans = Translation;
                if (automaticMovement)
                {
                    duration = animationDuration - totalTime;
                    inclLookAt = targetLookAt;
                    inclTrans = targetTranslation;
                }
                setNewPosition(SceneViewWindow.computeIncludePointAdjustedPosition(camera.getAspectRatio(), camera.getFOVy(), camera.getProjectionMatrix(), inclTrans, inclLookAt, currentIncludePoint.Value), inclLookAt, duration, easingFunction);
            }
        }

        /// <summary>
        /// Helper function to compute the starting orbitDistance, yaw, pitch,
        /// normalDirection and left values.
        /// </summary>
        /// <param name="localTrans">The translation of the camera relative to the look at point.</param>
        private static void computeStartingValues(Vector3 localTrans, out float orbitDistance, out float yaw, out float pitch, out Vector3 normalDirection, out Vector3 rotatedUp, out Vector3 rotatedLeft)
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
            localTrans = Quaternion.quatRotate(new Quaternion(Vector3.Up, -yaw), localTrans);
            localTrans.x = 0;
            pitch = Vector3.Backward.angle(ref localTrans);
            if (localTrans.y < 0)
            {
                pitch = -pitch;
            }

            //Compute the normal direction and the left vector.
            Quaternion yawRot = new Quaternion(Vector3.Up, yaw);
            Quaternion pitchRot = new Quaternion(Vector3.Left, pitch);
            Quaternion rotation = yawRot * pitchRot;
            normalDirection = Quaternion.quatRotate(ref rotation, ref Vector3.Backward);
            rotatedUp = Quaternion.quatRotate(ref rotation, ref Vector3.Up);
            rotatedLeft = normalDirection.cross(ref rotatedUp);
        }

        private void updateTranslation(Vector3 translation)
        {
            camera.Translation = translation;
            this.translation = translation;
        }

        public override Vector3 Translation
        {
            get
            {
                return translation;
            }
        }

        public override Vector3 LookAt
        {
            get
            {
                return lookAt;
            }
        }

        public override Vector3 TargetLookAt
        {
            get
            {
                return targetLookAt;
            }
        }

        public override Vector3 TargetTranslation
        {
            get
            {
                return targetTranslation;
            }
        }

        public override bool AllowRotation
        {
            get
            {
                return allowRotation;
            }
            set
            {
                allowRotation = value;
            }
        }

        public override bool AllowZoom
        {
            get
            {
                return allowZoom;
            }
            set
            {
                allowZoom = value;
            }
        }

        public Vector3 BoundMax
        {
            get
            {
                return boundMax;
            }
            set
            {
                boundMax = value;
            }
        }

        public Vector3 BoundMin
        {
            get
            {
                return boundMin;
            }
            set
            {
                boundMin = value;
            }
        }

        public override bool AllowManualMovement
        {
            get
            {
                return camera != null && !automaticMovement;
            }
        }
    }
}
