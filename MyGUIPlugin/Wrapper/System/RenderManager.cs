using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    class RenderManager
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

        private IntPtr resourceManager;

        private RenderManager()
        {
            resourceManager = RenderManager_getInstance();
        }

        public void manualFrameEvent(float time)
        {
            RenderManager_manualFrameEvent(resourceManager, time);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderManager_getInstance();

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderManager_manualFrameEvent(IntPtr resourceManager, float time);

        #endregion
    }
}
