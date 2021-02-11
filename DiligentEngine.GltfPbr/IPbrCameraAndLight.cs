using Engine;

namespace DiligentEngine.GltfPbr
{
    public interface IPbrCameraAndLight
    {
        IBuffer CameraAttribs { get; }
        IBuffer LightAttribs { get; }
        IBuffer EnvMapRenderAttribs { get; }

        void SetCamera(ref Matrix4x4 CameraProj, ref Matrix4x4 CameraViewProj, ref Vector3 CameraWorldPos);

        void SetLight(ref Vector3 direction, ref Vector4 lightColor, float intensity);
    }
}