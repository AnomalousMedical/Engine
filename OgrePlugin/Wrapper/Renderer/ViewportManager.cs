using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace OgreWrapper
{
    /// <summary>
    /// This class manages all viewport wrappers created by the system.
    /// </summary>
    class ViewportManager
    {
        private static WrapperCollection<Viewport> viewports = new WrapperCollection<Viewport>(Viewport.createWrapper);

        public static Viewport getViewport(IntPtr viewport)
        {
            return viewports.getObject(viewport);
        }

        public static Viewport getViewportNoCreate(IntPtr viewport)
        {
            Viewport vp = null;
            viewports.getObjectNoCreate(viewport, out vp);
            return vp;
        }

        public static IntPtr destroyViewport(Viewport vp)
        {
            return viewports.destroyObject(vp.OgreViewport);
        }

        private ViewportManager()
        {

        }
    }
}
