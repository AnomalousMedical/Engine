﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class OverlayContainer : OverlayElement
    {
        public OverlayContainer(IntPtr overlayContainer)
            :base(overlayContainer)
        {

        }

        public void addChild(OverlayElement elem)
        {
            OverlayContainer_addChild(overlayElement, elem.OgreObject);
        }

        public void removeChild(String name)
        {
            OverlayContainer_removeChild(overlayElement, name);
        }

        public OverlayElement getChild(String name)
        {
            return OverlayManager.getInstance().getObject(OverlayContainer_getChild(overlayElement, name));
        }

#region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OverlayContainer_addChild(IntPtr overlayContainer, IntPtr elem);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OverlayContainer_removeChild(IntPtr overlayContainer, String name);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OverlayContainer_getChild(IntPtr overlayContainer, String name);

#endregion
    }
}
