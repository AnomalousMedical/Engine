using Engine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OgreNextPlugin
{
    public class TextureGpu
    {
        IntPtr ptr;

        internal TextureGpu(IntPtr ptr)
        {
            this.ptr = ptr;
        }

        public IntPtr NativePtr
        {
            get
            {
                return ptr;
            }
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CompositorManager2_createBasicWorkspaceDef(IntPtr compMan, String workspaceDefName, Color backgroundColour);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CompositorManager2_addWorkspace(IntPtr compMan,
            IntPtr sceneManager, IntPtr finalRenderTarget,
            IntPtr defaultCam, String definitionName, bool bEnabled,
            int position
            /*, const UavBufferPackedVec * uavBuffers = 0,
ResourceLayoutMap * initialLayouts = 0,
ResourceAccessMap * initialUavAccess = 0,
Vector4 vpOffsetScale = Vector4::ZERO,
uint8 vpModifierMask = 0x00, uint8 executionMask = 0xFF*/
            );

        #endregion
    }
}
