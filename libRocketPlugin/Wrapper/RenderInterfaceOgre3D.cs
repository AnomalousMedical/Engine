using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OgreWrapper;
using OgrePlugin;

namespace libRocketPlugin
{
    public class RenderInterfaceOgre3D : RenderInterface
    {
        private CommonResourcesArchiveFactory commonResourcesArchiveFactory = null;

        public RenderInterfaceOgre3D(int width, int height)
        {
            commonResourcesArchiveFactory = new CommonResourcesArchiveFactory();
            Root.getSingleton().addArchiveFactory(commonResourcesArchiveFactory);
            OgreInterface.Instance.Disposed += Ogre_Disposed;

            renderInterface = RenderInterfaceOgre3D_Create(width, height);
        }

        public override void Dispose()
        {
            RenderInterfaceOgre3D_Delete(renderInterface);
            //Note that some stuff is disposed in the Ogre_Disposed function below
        }

        void Ogre_Disposed(OgreInterface obj)
        {
            //Have to do this after ogre is disposed
            if (commonResourcesArchiveFactory != null)
            {
                commonResourcesArchiveFactory.Dispose();
                commonResourcesArchiveFactory = null;
            }
        }

        public void ConfigureRenderSystem(int windowWidth, int windowHeight, bool requiresTextureFlipping)
        {
            RenderInterfaceOgre3D_ConfigureRenderSystem(renderInterface, ref windowWidth, ref windowHeight, ref requiresTextureFlipping);
        }

        public float PixelsPerInch
        {
            get
            {
                return RenderInterfaceOgre3D_GetPixelsPerInch(renderInterface);
            }
            set
            {
                RenderInterfaceOgre3D_SetPixelsPerInch(renderInterface, value);
            }
        }

        public float PixelScale
        {
            get
            {
                return RenderInterfaceOgre3D_GetPixelScale(renderInterface);
            }
            set
            {
                RenderInterfaceOgre3D_SetPixelScale(renderInterface, value);
            }
        }

        #region PInvoke

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderInterfaceOgre3D_Create(int width, int height);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_Delete(IntPtr renderInterface);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_ConfigureRenderSystem(IntPtr renderInterface, ref int renderWidth, ref int renderHeight, ref bool requiresTextureFlipping);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern float RenderInterfaceOgre3D_GetPixelsPerInch(IntPtr renderInterface);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_SetPixelsPerInch(IntPtr renderInterface, float ppi);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern float RenderInterfaceOgre3D_GetPixelScale(IntPtr renderInterface);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_SetPixelScale(IntPtr renderInterface, float scale);

        #endregion
    }
}
