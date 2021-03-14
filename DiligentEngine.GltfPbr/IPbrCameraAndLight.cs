using Engine;

namespace DiligentEngine.GltfPbr
{
    public interface IPbrCameraAndLight
    {
        IBuffer CameraAttribs { get; }
        IBuffer LightAttribs { get; }
        IBuffer EnvMapRenderAttribs { get; }

        /// <summary>
        /// Set the camera from a given position and rotation. Must include the pretransform and projection matrix as well.
        /// You do not need to call this and SetCameraMatrices. That version just allows you more direct access to the buffers.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="preTransformMatrix"></param>
        /// <param name="CameraProj"></param>
        void SetCameraPosition(Vector3 position, Quaternion rotation, in Matrix4x4 preTransformMatrix, in Matrix4x4 CameraProj);

        /// <summary>
        /// Set the camera from a set of matrices.
        /// </summary>
        /// <param name="CameraProj"></param>
        /// <param name="CameraViewProj"></param>
        /// <param name="CameraWorldPos"></param>
        void SetCameraMatrices(in Matrix4x4 CameraProj, in Matrix4x4 CameraViewProj, in Vector3 CameraWorldPos);

        void SetLight(in Vector3 direction, in Vector4 lightColor, float intensity);

        void SetLightAndShadow(in Vector3 direction, in Vector4 lightColor, float intensity, in Matrix4x4 WorldToShadowMapUVDepth);
    }
}