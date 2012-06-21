﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace libRocketPlugin
{
    public class Element : ReferenceCountable
    {
        StringRetriever stringRetriever = new StringRetriever();

        internal Element(IntPtr ptr)
            : base(ptr)
        {

        }

        public bool Focus()
        {
            return Element_Focus(ptr);
        }

        public void Blur()
        {
            Element_Blur(ptr);
        }

        public String TagName
        {
            get
            {
                return Marshal.PtrToStringAnsi(Element_GetTagName(ptr));
            }
        }

        public float AbsoluteLeft
        {
            get
            {
                return Element_GetAbsoluteLeft(ptr);
            }
        }

        public float AbsoluteTop
        {
            get
            {
                return Element_GetAbsoluteTop(ptr);
            }
        }

        public float ClientLeft
        {
            get
            {
                return Element_GetClientLeft(ptr);
            }
        }

        public float ClientTop
        {
            get
            {
                return Element_GetClientTop(ptr);
            }
        }

        public float ClientWidth
        {
            get
            {
                return Element_GetClientWidth(ptr);
            }
        }

        public float ClientHeight
        {
            get
            {
                return Element_GetClientHeight(ptr);
            }
        }

        public float OffsetLeft
        {
            get
            {
                return Element_GetOffsetLeft(ptr);
            }
        }

        public float OffsetTop
        {
            get
            {
                return Element_GetOffsetTop(ptr);
            }
        }

        public float OffsetWidth
        {
            get
            {
                return Element_GetOffsetWidth(ptr);
            }
        }

        public float OffsetHeight
        {
            get
            {
                return Element_GetOffsetHeight(ptr);
            }
        }

        public float ScrollLeft
        {
            get
            {
                return Element_GetScrollLeft(ptr);
            }
            set
            {
                Element_SetScrollLeft(ptr, value);
            }
        }

        public float ScrollTop
        {
            get
            {
                return Element_GetScrollTop(ptr);
            }
            set
            {
                Element_SetScrollTop(ptr, value);
            }
        }

        public float ScrollWidth
        {
            get
            {
                return Element_GetScrollWidth(ptr);
            }
        }

        public float ScrollHeight
        {
            get
            {
                return Element_GetScrollHeight(ptr);
            }
        }

        public Element ParentNode
        {
            get
            {
                return ElementManager.getElement(Element_GetParentNode(ptr));
            }
        }

        public String InnerRml
        {
            get
            {
                Element_GetInnerRML(ptr, stringRetriever.StringCallback);
                return stringRetriever.retrieveString();
            }
            set
            {
                Element_SetInnerRML(ptr, value);
            }
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetTagName(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetAbsoluteLeft(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetAbsoluteTop(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetClientLeft(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetClientTop(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetClientWidth(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetClientHeight(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetOffsetLeft(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetOffsetTop(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetOffsetWidth(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetOffsetHeight(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetScrollLeft(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_SetScrollLeft(IntPtr element, float scroll_left);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetScrollTop(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_SetScrollTop(IntPtr element, float scroll_top);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetScrollWidth(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Element_GetScrollHeight(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetParentNode(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_GetInnerRML(IntPtr element, StringRetriever.Callback retrieve);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_SetInnerRML(IntPtr element, String rml);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Element_Focus(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_Blur(IntPtr element);

        #endregion
    }
}
