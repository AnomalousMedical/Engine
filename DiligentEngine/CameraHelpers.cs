using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngine
{
    public static class CameraHelpers
    {
        public static Matrix4x4 GetAdjustedProjectionMatrix(float FOV, float NearPlane, float FarPlane, float Width, float Height, SURFACE_TRANSFORM PreTransform)
        {
            //From Diligent Engine Samples
            if (Height == 0.0f)
            {
                Height = 1.0f;
            }

            float AspectRatio = Width / Height;
            float XScale, YScale;
            if (PreTransform == SURFACE_TRANSFORM.SURFACE_TRANSFORM_ROTATE_90 ||
                PreTransform == SURFACE_TRANSFORM.SURFACE_TRANSFORM_ROTATE_270 ||
                PreTransform == SURFACE_TRANSFORM.SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_90 ||
                PreTransform == SURFACE_TRANSFORM.SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_270)
            {
                // When the screen is rotated, vertical FOV becomes horizontal FOV
                XScale = 1f / (float)Math.Tan(FOV / 2f);
                // Aspect ratio is inversed
                YScale = XScale * AspectRatio;
            }
            else
            {
                YScale = 1f / (float)Math.Tan(FOV / 2f);
                XScale = YScale / AspectRatio;
            }

            Matrix4x4 Proj = new Matrix4x4();
            Proj.m00 = XScale;
            Proj.m11 = YScale;
            Proj.SetNearFarClipPlanes(NearPlane, FarPlane, false);// genericEngineFactory.RenderDevice.GetDeviceCaps().IsGLDevice());
            return Proj;
        }

        public static Matrix4x4 GetSurfacePretransformMatrix(Vector3 f3CameraViewAxis, SURFACE_TRANSFORM PreTransform)
        {
            //From Diligent Engine Samples
            switch (PreTransform)
            {
                case SURFACE_TRANSFORM.SURFACE_TRANSFORM_ROTATE_90:
                    // The image content is rotated 90 degrees clockwise.
                    return Matrix4x4.RotationArbitrary(f3CameraViewAxis, -MathFloat.PI / 2.0f);

                case SURFACE_TRANSFORM.SURFACE_TRANSFORM_ROTATE_180:
                    // The image content is rotated 180 degrees clockwise.
                    return Matrix4x4.RotationArbitrary(f3CameraViewAxis, -MathFloat.PI);

                case SURFACE_TRANSFORM.SURFACE_TRANSFORM_ROTATE_270:
                    // The image content is rotated 270 degrees clockwise.
                    return Matrix4x4.RotationArbitrary(f3CameraViewAxis, -MathFloat.PI * 3.0f / 2.0f);

                case SURFACE_TRANSFORM.SURFACE_TRANSFORM_OPTIMAL:
                    //UNEXPECTED("SURFACE_TRANSFORM_OPTIMAL is only valid as parameter during swap chain initialization.");
                    return Matrix4x4.Identity;

                case SURFACE_TRANSFORM.SURFACE_TRANSFORM_HORIZONTAL_MIRROR:
                case SURFACE_TRANSFORM.SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_90:
                case SURFACE_TRANSFORM.SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_180:
                case SURFACE_TRANSFORM.SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_270:
                    //UNEXPECTED("Mirror transforms are not supported");
                    return Matrix4x4.Identity;

                default:
                    return Matrix4x4.Identity;
            }
        }
    }
}
