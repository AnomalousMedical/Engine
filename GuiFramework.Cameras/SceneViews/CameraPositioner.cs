using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anomalous.GuiFramework.Cameras
{
    /// <summary>
    /// This class sits between the CameraMovers and the SceneView, it keeps the near and
    /// far planes in check for the camera, which prevents ugly rendering artifacts.
    /// </summary>
    public class CameraPositioner
    {
        private SceneView sceneView;
        private float nearPlaneWorld;
        private float nearFarLength;
        private float minNearDistance;

        public CameraPositioner(SceneView sceneView, float minNearDistance, float nearPlaneWorld, float nearFarLength)
        {
            this.sceneView = sceneView;
            this.minNearDistance = minNearDistance;
            this.nearPlaneWorld = nearPlaneWorld;
            this.nearFarLength = nearFarLength;
            updateNearFarPlanes();
        }

        public Vector3 LookAt
        {
            get
            {
                return sceneView.LookAt;
            }
            set
            {
                sceneView.LookAt = value;
                updateNearFarPlanes();
            }
        }

        public Vector3 Translation
        {
            get
            {
                return sceneView.Translation;
            }
            set
            {
                sceneView.Translation = value;
                updateNearFarPlanes();
            }
        }

        private void updateNearFarPlanes()
        {
            float near = computeNearClipDistance(Translation.length(), minNearDistance, nearPlaneWorld);
            sceneView.setNearClipDistance(near);
            sceneView.setFarClipDistance(near + nearFarLength);
        }

        public static float computeNearClipDistance(float distance, float minNearDistance, float nearPlaneWorld)
        {
            float near = distance - nearPlaneWorld;
            if (near < minNearDistance)
            {
                near = minNearDistance;
            }
            return near;
        }
    }
}
