using DiligentEngine.RT;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class CameraProjector : ICameraProjector
    {
        private readonly RTCameraAndLight cameraAndLight;
        private readonly OSWindow osWindow;

        public CameraProjector(RTCameraAndLight cameraAndLight, OSWindow osWindow)
        {
            this.cameraAndLight = cameraAndLight;
            this.osWindow = osWindow;
        }

        public Vector2 Project(in Vector3 position)
        {
            //Need to adjust to diligent coords by negating
            var clipPos = new Vector4(-position.x, -position.y, -position.z, 1.0f) * cameraAndLight.CurrentViewProj;
            clipPos /= clipPos.w;
            return new Vector2
            (
                (clipPos.x + 1f) / 2f * osWindow.WindowWidth,
                (1f - (clipPos.y + 1f) / 2f) * osWindow.WindowHeight
            );
        }
    }
}
