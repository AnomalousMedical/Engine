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
        protected static StringRetriever stringRetriever = new StringRetriever();
        protected static StringRetriever stringRetriever2 = new StringRetriever();

        internal Element(IntPtr ptr)
            : base(ptr)
        {

        }

        public void SetClass(String className, bool activate)
        {
            Element_SetClass(ptr, className, activate);
        }

        public bool IsClassSet(String className)
        {
            return Element_IsClassSet(ptr, className);
        }

        public bool SetProperty(String name, String value)
        {
            return Element_SetProperty(ptr, name, value);
        }

        public void RemoveProperty(String name)
        {
            Element_RemoveProperty(ptr, name);
        }

        public String GetPropertyString(String name)
        {
            Element_GetPropertyString(ptr, name, stringRetriever.StringCallback);
            return stringRetriever.retrieveString();
        }

        public Variant GetPropertyVariant(String name)
        {
            return VariantWrapped.Construct(Element_GetPropertyVariant(ptr, name));
        }

        public String GetLocalPropertyString(String name)
        {
            Element_GetLocalPropertyString(ptr, name, stringRetriever.StringCallback);
            return stringRetriever.retrieveString();
        }

        public Variant GetLocalPropertyVariant(String name)
        {
            return VariantWrapped.Construct(Element_GetLocalPropertyVariant(ptr, name));
        }

        public void SetAttribute(String name, String value)
        {
            Element_SetAttribute(ptr, name, value);
        }

        public Variant GetAttribute(String name)
        {
            return VariantWrapped.Construct(Element_GetAttribute(ptr, name));
        }

        public String GetAttributeString(String name)
        {
            Variant variant = GetAttribute(name);
            if (variant != null)
            {
                return variant.StringValue;
            }
            return null;
        }

        public bool HasAttribute(String name)
        {
            return Element_HasAttribute(ptr, name);
        }

        public void RemoveAttribute(String name)
        {
            Element_RemoveAttribute(ptr, name);
        }

        public bool IterateAttributes(ref int index, out String name, out String value)
        {
            bool retVal = Element_IterateAttributes(ptr, ref index, stringRetriever.StringCallback, stringRetriever2.StringCallback);
            name = stringRetriever.retrieveString();
            value = stringRetriever2.retrieveString();
            return retVal;
        }

        public void Click()
        {
            Element_Click(ptr);
        }
        
        public void ScrollIntoView(bool align_with_top = true)
        {
            Element_ScrollIntoView(ptr, align_with_top);
        }
        
        public void AppendChild(Element append, bool dom_element = true)
        {
            Element_AppendChild(ptr, append.Ptr, dom_element);
        }

        public void InsertBefore(Element insert, Element adjacent_element)
        {
            Element_InsertBefore(ptr, insert.Ptr, adjacent_element.Ptr);
        }

        public void InsertAfter(Element insert, Element adjacent_element)
        {
            Element next = adjacent_element.NextSibling;
            if (next != null)
            {
                this.InsertBefore(insert, next);
            }
            else
            {
                this.AppendChild(insert);
            }
        }

        public void Insert(Element insert, Element adjacent_element, bool insertBefore)
        {
            if (insertBefore)
            {
                this.InsertBefore(insert, adjacent_element);
            }
            else
            {
                this.InsertAfter(insert, adjacent_element);
            }
        }

        public bool ReplaceChild(Element inserted_element, Element replaced_element)
        {
            return Element_ReplaceChild(ptr, inserted_element.Ptr, replaced_element.Ptr);
        }

        public bool RemoveChild(Element remove)
        {
            return Element_RemoveChild(ptr, remove.Ptr);
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

        public Element GetChild(int index)
        {
            return ElementManager.getElement(Element_GetChild(ptr, index));
        }

        public IEnumerable<Element> GetElementsByTagName(String tag)
        {
            ElementListIter iter = new ElementListIter();
            Element_GetElementsByTagName(ptr, iter.Ptr, tag);
            iter.startIterator();
            Element element;
            do
            {
                element = iter.getNextElement();
                if (element != null)
                {
                    yield return element;
                }
            } while (element != null);
            iter.Dispose();
        }

        public IEnumerable<Element> GetElementsWithAttribute(String attribute)
        {
            ElementListIter iter = new ElementListIter();
            Element_GetElementsWithAttribute(ptr, iter.Ptr, attribute);
            iter.startIterator();
            Element element;
            do
            {
                element = iter.getNextElement();
                if (element != null)
                {
                    yield return element;
                }
            } while (element != null);
            iter.Dispose();
        }

        public void ClearLocalStyles()
        {
            Element_ClearLocalStyles(ptr);
        }

        public String TagName
        {
            get
            {
                return Marshal.PtrToStringAnsi(Element_GetTagName(ptr));
            }
        }

        public String Id
        {
            get
            {
                return Marshal.PtrToStringAnsi(Element_GetId(ptr));
            }
            set
            {
                Element_SetId(ptr, value);
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

        public Element OffsetParent
        {
            get
            {
                return ElementManager.getElement(Element_GetOffsetParent(ptr));
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

        public ElementDocument OwnerDocument
        {
            get
            {
                return ElementManager.getElement<ElementDocument>(Element_GetOwnerDocument(ptr));
            }
        }

        public Element ParentNode
        {
            get
            {
                return ElementManager.getElement(Element_GetParentNode(ptr));
            }
        }

        public Element NextSibling
        {
            get
            {
                return ElementManager.getElement(Element_GetNextSibling(ptr));
            }
        }

        public Element PreviousSibling
        {
            get
            {
                return ElementManager.getElement(Element_GetPreviousSibling(ptr));
            }
        }

        public Element FirstChild
        {
            get
            {
                return ElementManager.getElement(Element_GetFirstChild(ptr));
            }
        }

        public Element LastChild
        {
            get
            {
                return ElementManager.getElement(Element_GetLastChild(ptr));
            }
        }

        public int NumChildren
        {
            get
            {
                return Element_GetNumChildren(ptr, false);
            }
        }

        public int NumChildrenWithNonDom
        {
            get
            {
                return Element_GetNumChildren(ptr, true);
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

        public String ElementRml
        {
            get
            {
                Element_GetElementRML(ptr, stringRetriever.StringCallback);
                return stringRetriever.retrieveString();
            }
        }

        public bool HasChildNodes
        {
            get
            {
                return Element_HasChildNodes(ptr);
            }
        }

        public IEnumerable<Element> Children
        {
            get
            {
                int numChildren = NumChildren;
                for (int i = 0; i < numChildren; ++i)
                {
                    yield return GetChild(i);
                }
            }
        }

        public Context Context
        {
            get
            {
                return ElementManager.getContext(Element_GetContext(ptr));
            }
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_SetClass(IntPtr element, String class_name, bool activate);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Element_IsClassSet(IntPtr element, String class_name);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Element_SetProperty(IntPtr element, String name, String value);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_RemoveProperty(IntPtr element, String name);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_GetPropertyString(IntPtr element, String name, StringRetriever.Callback strRetriever);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetPropertyVariant(IntPtr element, String name);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_GetLocalPropertyString(IntPtr element, String name, StringRetriever.Callback strRetriever);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetLocalPropertyVariant(IntPtr element, String name);
        
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
        private static extern IntPtr Element_GetContext(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetTagName(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetId(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_SetId(IntPtr element, String id);
        
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
        private static extern IntPtr Element_GetOffsetParent(IntPtr element);

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
        private static extern IntPtr Element_GetOwnerDocument(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetParentNode(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetNextSibling(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetPreviousSibling(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetFirstChild(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetLastChild(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetChild(IntPtr element, int index);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Element_GetNumChildren(IntPtr element, bool include_non_dom_elements/* = false*/);

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
        private static extern void Element_Click(IntPtr element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_ScrollIntoView(IntPtr element, bool align_with_top/* = true*/);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_AppendChild(IntPtr element, IntPtr append, bool dom_element/* = true*/);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_InsertBefore(IntPtr element, IntPtr insert, IntPtr adjacent_element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Element_ReplaceChild(IntPtr element, IntPtr inserted_element, IntPtr replaced_element);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Element_RemoveChild(IntPtr element, IntPtr remove);
        
        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Element_HasChildNodes(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Element_GetElementById(IntPtr element, String id);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_GetElementsByTagName(IntPtr element, IntPtr elements, String tag);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_GetElementRML(IntPtr element, StringRetriever.Callback retrieve);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_GetElementsWithAttribute(IntPtr root_element, IntPtr elementListIter, String attribute);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_ClearLocalStyles(IntPtr element);

        #endregion
    }
}
