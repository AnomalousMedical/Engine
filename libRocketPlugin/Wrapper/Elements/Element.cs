using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace libRocketPlugin
{
    public class Element : ReferenceCountable
    {
        private static StringRetriever stringRetriever = new StringRetriever();
        private static StringRetriever stringRetriever2 = new StringRetriever();

        internal Element(IntPtr ptr)
            : base(ptr)
        {

        }

        public void SetAttribute(String name, String value)
        {
            Element_SetAttribute(ptr, name, value);
        }

        public Variant GetAttribute(String name)
        {
            return VariantWrapped.Construct(Element_GetAttribute(ptr, name));
        }

        public bool HasAttribute(String name)
        {
            return Element_HasAttribute(ptr, name);
        }

        public void RemoveAttribute(String name)
        {
            Element_RemoveAttribute(ptr, name);
        }

        public bool IterateAttributes(ref int index, ref String name, ref String value)
        {
            bool retVal = Element_IterateAttributes(ptr, ref index, stringRetriever.StringCallback, stringRetriever2.StringCallback);
            name = stringRetriever.retrieveString();
            value = stringRetriever2.retrieveString();
            return retVal;
        }

        public int NumAttributes
        {
            get
            {
                return Element_GetNumAttributes(ptr);
            }
        }

        public bool Focus()
        {
            return Element_Focus(ptr);
        }

        public void Blur()
        {
            Element_Blur(ptr);
        }

        public Element GetElementById(String id)
        {
            return ElementManager.getElement(Element_GetElementById(ptr, id));
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
        private static extern void Element_SetAttribute(IntPtr element, String name, String value);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetAttribute(IntPtr element, String name);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Element_HasAttribute(IntPtr element, String name);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_RemoveAttribute(IntPtr element, String name);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Element_IterateAttributes(IntPtr element, ref int index, StringRetriever.Callback keyRetrieve, StringRetriever.Callback valueRetrieve);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Element_GetNumAttributes(IntPtr element);

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

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetElementById(IntPtr element, String id);

        #endregion
    }
}
