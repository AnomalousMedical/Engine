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
        void SetCameraPosition(Vector3 position, Quaternion rotation, ref Matrix4x4 preTransformMatrix, ref Matrix4x4 CameraProj);

        /// <summary>
        /// Set the camera from a set of matrices.
        /// </summary>
        /// <param name="CameraProj"></param>
        /// <param name="CameraViewProj"></param>
        /// <param name="CameraWorldPos"></param>
        void SetCameraMatrices(ref Matrix4x4 CameraProj, ref Matrix4x4 CameraViewProj, ref Vector3 CameraWorldPos);

        void SetLight(ref Vector3 direction, ref Vector4 lightColor, float intensity);

        void SetLightAndShadow(ref Vector3 direction, ref Vector4 lightColor, float intensity, ref Matrix4x4 WorldToShadowMapUVDepth);
    }
}