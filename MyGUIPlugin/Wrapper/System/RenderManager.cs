﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class RenderManager
    {
        private static RenderManager instance = null;

        public static RenderManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RenderManager();
                }
                return instance;
            }
        }

        private IntPtr renderManager;

        private RenderManager()
        {
            renderManager = RenderManager_getInstance();
        }

        public void destroyTexture(String name)
        {
            RenderManager_destroyTextureByName(renderManager, name);
        }

        public int ViewWidth
        {
            get
            {
                return RenderManager_getViewWidth(renderManager);
            }
        }

        public int ViewHeight
        {
            get
            {
                return RenderManager_getViewHeight(renderManager);
            }
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderManager_getInstance();

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int RenderManager_getViewWidth(IntPtr gui);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int RenderManager_getViewHeight(IntPtr gui);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderManager_destroyTextureByName(IntPtr renderManager, String name);

        #endregion
    }
}
