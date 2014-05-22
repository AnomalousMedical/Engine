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
        private static ContextDestructorCallback contextDestructorFunc;

        private static WrapperCollection<Element> elements = new WrapperCollection<Element>(elementCreate, elementDelete);
        private static WrapperCollection<Context> contexts = new WrapperCollection<Context>(contextCreate, contextDelete);

        static ElementManager()
        {
            elementDestructorFunc = new ElementDestructorCallback(elementDestructor);
            contextDestructorFunc = new ContextDestructorCallback(contextDestructor);
            elementManager = ElementManager_create(elementDestructorFunc, contextDestructorFunc);
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

        internal static Context getContext(IntPtr context)
        {
            if (context != IntPtr.Zero)
            {
                return contexts.getObject(context);
            }
            return null;
        }

        /// <summary>
        /// This is called by the c++ destructor for the element. It will erase the wrapper object.
        /// </summary>
        /// <param name="element"></param>
        static void elementDestructor(IntPtr element)
        {
            elements.destroyObject(element);
        }

        /// <summary>
        /// This is called by the c++ destructor for the element. It will erase the wrapper object.
        /// </summary>
        /// <param name="element"></param>
        static void contextDestructor(IntPtr context)
        {
            contexts.destroyObject(context);
        }

        public static void destroyAllWrappers()
        {
            elements.clearObjects();
            contexts.clearObjects();
        }

        private static Element elementCreate(IntPtr element, object[] args)
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

        private static Context contextCreate(IntPtr context, object[] args)
        {
            return new Context(context);
        }

        private static void contextDelete(Context wrapper)
        {
            //Since this just calls the reference decrement, do not do anything in the destructor callback.
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ElementDestructorCallback(IntPtr element);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ContextDestructorCallback(IntPtr element);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ElementManager_create(ElementDestructorCallback elementDestroyed, ContextDestructorCallback contextDestroyed);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern ElementType ElementManager_getType(IntPtr element);

        #endregion
    }
}
