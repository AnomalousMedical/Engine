using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace libRocketPlugin
{
    static class ContextManager
    {
        private static WrapperCollection<Context> contexts = new WrapperCollection<Context>(createWrapper);

        internal static Context getContext(IntPtr contextPtr)
        {
            if (contextPtr != IntPtr.Zero)
            {
                Context returnedWidget = contexts.getObject(contextPtr);
                return returnedWidget;
            }
            return null;
        }

        public static void destroyAllWrappers()
        {
            contexts.clearObjects();
        }

        private static Context createWrapper(IntPtr contextPtr, object[] args)
        {
            return new Context(contextPtr);
        }
    }
}
