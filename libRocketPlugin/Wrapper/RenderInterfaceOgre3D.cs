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
        //These consts are also defined in RenderInterfaceOgre3D.cpp.
        public const String SHARED_RESOURCE_GROUP = "Rocket.Common";
        public const String MAIN_RESOURCE_GROUP = "Rocket";

        private CommonResourcesArchiveFactory commonResourcesArchiveFactory = null;

        public RenderInterfaceOgre3D(int width, int height)
        {
            commonResourcesArchiveFactory = new CommonResourcesArchiveFactory();
            Root.getSingleton().addArchiveFactory(commonResourcesArchiveFactory);
            OgreInterface.Instance.Disposed += Ogre_Disposed;

            renderInterface = RenderInterfaceOgre3D_Create(width, height);

            OgreResourceGroupManager.getInstance().addResourceLocation("__LibRocketCommonResourcesFilesystem__", CommonResourcesArchiveFactory.Name, SHARED_RESOURCE_GROUP, false);
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

        public void ConfigureRenderSystem(int windowWidth, int windowHeight)
        {
            RenderInterfaceOgre3D_ConfigureRenderSystem(renderInterface, ref windowWidth, ref windowHeight);
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

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderInterfaceOgre3D_Create(int width, int height);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_Delete(IntPtr renderInterface);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_ConfigureRenderSystem(IntPtr renderInterface, ref int renderWidth, ref int renderHeight);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float RenderInterfaceOgre3D_GetPixelsPerInch(IntPtr renderInterface);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_SetPixelsPerInch(IntPtr renderInterface, float ppi);

        #endregion
    }
}
