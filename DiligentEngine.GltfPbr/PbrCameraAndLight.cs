using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{
    public class PbrCameraAndLight : IPbrCameraAndLight, IDisposable
    {
        private AutoPtr<IBuffer> m_CameraAttribsCB;
        private AutoPtr<IBuffer> m_LightAttribsCB;
        private AutoPtr<IBuffer> m_EnvMapRenderAttribsCB;

        IDeviceContext m_pImmediateContext;

        public unsafe PbrCameraAndLight(GraphicsEngine graphicsEngine)
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            m_pImmediateContext = graphicsEngine.ImmediateContext;

            {
                BufferDesc CBDesc = new BufferDesc();
                CBDesc.Name = "Camera attribs buffer";
                CBDesc.uiSizeInBytes = (uint)sizeof(CameraAttribs);
                CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                m_CameraAttribsCB = m_pDevice.CreateBuffer(CBDesc);
            }

            {
                BufferDesc CBDesc = new BufferDesc();
                CBDesc.Name = "Light attribs buffer";
                CBDesc.uiSizeInBytes = (uint)sizeof(LightAttribs);
                CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                m_LightAttribsCB = m_pDevice.CreateBuffer(CBDesc);
            }

            {
                BufferDesc CBDesc = new BufferDesc();
                CBDesc.Name = "Env map render attribs buffer";
                CBDesc.uiSizeInBytes = (uint)sizeof(EnvMapRenderAttribs);
                CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                m_EnvMapRenderAttribsCB = m_pDevice.CreateBuffer(CBDesc);
            }

            var Barriers = new List<StateTransitionDesc>
            {
                new StateTransitionDesc{pResource = m_CameraAttribsCB.Obj,        OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER, UpdateResourceState = true},
                new StateTransitionDesc{pResource = m_LightAttribsCB.Obj,         OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER, UpdateResourceState = true},
                new StateTransitionDesc{pResource = m_EnvMapRenderAttribsCB.Obj,  OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER, UpdateResourceState = true},
            };
            m_pImmediateContext.TransitionResourceStates(Barriers);
        }

        public void Dispose()
        {
            m_CameraAttribsCB.Dispose();
            m_LightAttribsCB.Dispose();
            m_EnvMapRenderAttribsCB.Dispose();
        }

        public IBuffer CameraAttribs => m_CameraAttribsCB.Obj;
        public IBuffer LightAttribs => m_LightAttribsCB.Obj;
        public IBuffer EnvMapRenderAttribs => m_EnvMapRenderAttribsCB.Obj;

        public unsafe void SetCamera(ref Matrix4x4 CameraProj, ref Matrix4x4 CameraViewProj, ref Vector3 CameraWorldPos)
        {
            IntPtr data = m_pImmediateContext.MapBuffer(m_CameraAttribsCB.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);

            var CamAttribs = (CameraAttribs*)data.ToPointer();
            CamAttribs->mProjT = CameraProj.Transpose();
            CamAttribs->mViewProjT = CameraViewProj.Transpose();
            CamAttribs->mViewProjInvT = CameraViewProj.inverse().Transpose();
            CamAttribs->f4Position = new Vector4(CameraWorldPos.x, CameraWorldPos.y, CameraWorldPos.z, 1);

            m_pImmediateContext.UnmapBuffer(m_CameraAttribsCB.Obj, MAP_TYPE.MAP_WRITE);
        }

        public unsafe void SetLight(ref Vector3 direction, ref Vector4 lightColor, float intensity)
        {
            IntPtr data = m_pImmediateContext.MapBuffer(m_LightAttribsCB.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);

            //Looks like only direction and intensity matter here, setting more did not help
            var lightAttribs = (LightAttribs*)data.ToPointer();
            lightAttribs->f4Direction = direction.ToVector4();
            lightAttribs->f4Intensity = lightColor * intensity;

            m_pImmediateContext.UnmapBuffer(m_LightAttribsCB.Obj, MAP_TYPE.MAP_WRITE);
        }
    }
}
