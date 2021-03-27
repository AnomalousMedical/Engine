using DiligentEngine.GltfPbr;
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
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private readonly OSWindow osWindow;

        public CameraProjector(IPbrCameraAndLight pbrCameraAndLight, OSWindow osWindow)
        {
            this.pbrCameraAndLight = pbrCameraAndLight;
            this.osWindow = osWindow;
        }

        public Vector2 Project(in Vector3 position)
        {
            var clipPos = new Vector4(position.x, position.y, position.z, 1.0f) * pbrCameraAndLight.CurrentViewProj;
            clipPos /= clipPos.w;
            return new Vector2
            (
                (clipPos.x + 1f) / 2f * osWindow.WindowWidth,
                (1f - (clipPos.y + 1f) / 2f) * osWindow.WindowHeight
            );
        }
    }
}
