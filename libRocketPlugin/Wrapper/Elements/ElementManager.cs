using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Logging;

namespace libRocketPlugin
{
    static class ElementManager
    {
        enum ElementType
        {
            Element = 0,
            ElementDocument = 1,
            ElementFormControl = 2
        };

        private static IntPtr elementManager;
        private static ElementDestructorCallback elementDestructorFunc;

        private static WrapperCollection<Element> elements = new WrapperCollection<Element>(createWrapper, elementDelete);

        static ElementManager()
        {
            elementDestructorFunc = new ElementDestructorCallback(elementDestructor);
            elementManager = ElementManager_create(elementDestructorFunc);
        }

        internal static Element getElement(IntPtr element)
        {
            if (element != IntPtr.Zero)
            {
                Element returnedElement = elements.getObject(element);
                return returnedElement;
            }
            return null;
        }

        internal static T getElement<T>(IntPtr element)
            where T : Element
        {
            return (T)getElement(element);
        }

        /// <summary>
        /// This is called by the c++ destructor for the element. It will erase the wrapper object.
        /// </summary>
        /// <param name="element"></param>
        static void elementDestructor(IntPtr element)
        {
            elements.destroyObject(element);
        }

        public static void destroyAllWrappers()
        {
            elements.clearObjects();
        }

        private static Element createWrapper(IntPtr element, object[] args)
        {
            ElementType elementType = ElementManager_getType(element);
            switch (elementType)
            {
                case ElementType.Element:
                    return new Element(element);

                case ElementType.ElementDocument:
                    return new ElementDocument(element);

                case ElementType.ElementFormControl:
                    return new ElementFormControl(element);
            }
            Log.Warning("Could not identify element type for element {0}. Type given was {1}. Will return a Element in its place.", element.ToString(), elementType);
            return new Element(element);
        }

        private static void elementDelete(Element wrapper)
        {
            //Since this just calls the reference decrement, do not do anything in the destructor callback.
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ElementDestructorCallback(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ElementManager_create(ElementDestructorCallback elementDestroyed);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern ElementType ElementManager_getType(IntPtr element);

        #endregion
    }
}
