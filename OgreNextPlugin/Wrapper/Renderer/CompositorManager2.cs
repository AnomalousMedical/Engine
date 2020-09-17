using Engine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OgreNextPlugin
{
    public class CompositorManager2
    {
        IntPtr ptr;

        internal CompositorManager2(IntPtr ptr)
        {
            this.ptr = ptr;
        }

        public void createBasicWorkspaceDef(String workspaceDefName, Color backgroundColour)
        {
            CompositorManager2_createBasicWorkspaceDef(ptr, workspaceDefName, backgroundColour);
        }

        public void addWorkspace(
            SceneManager sceneManager, TextureGpu finalRenderTarget,
            Camera defaultCam, String definitionName, bool bEnabled,
            int position = -1
            /*, const UavBufferPackedVec * uavBuffers = 0,
ResourceLayoutMap * initialLayouts = 0,
ResourceAccessMap * initialUavAccess = 0,
Vector4 vpOffsetScale = Vector4::ZERO,
uint8 vpModifierMask = 0x00, uint8 executionMask = 0xFF*/
            )
        {
            CompositorManager2_addWorkspace(ptr, 
                sceneManager.NativePtr, finalRenderTarget.NativePtr,
                defaultCam.OgreObject, definitionName, bEnabled,
                position);
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
