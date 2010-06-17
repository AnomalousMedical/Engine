using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace MyGUIPlugin
{
    public sealed class LayoutManager
    {
        private static LayoutManager instance;

        public static LayoutManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new LayoutManager();
                }
                return instance;
            }
        }

        private IntPtr layoutManager;
        private WrapperCollection<Layout> layouts = new WrapperCollection<Layout>(Layout.createWrapper);

        private LayoutManager()
        {
            layoutManager = LayoutManager_getInstancePtr();
        }

        /// <summary>
        /// Load a layout. It is best for memory management to be sure to call
        /// unloadLayout before your program closes. Otherwise a pointer will
        /// leak inside the returned Layout.
        /// </summary>
        /// <param name="file">The file to load.</param>
        /// <returns>A Layout which is really just a collection of widgets.</returns>
        public Layout loadLayout(String file)
        {
            return layouts.getObject(LayoutManager_loadLayout(layoutManager, file));
        }

        /// <summary>
        /// Load a layout. It is best for memory management to be sure to call
        /// unloadLayout before your program closes. Otherwise a pointer will
        /// leak inside the returned Layout.
        /// </summary>
        /// <param name="file">The file to load.</param>
        /// <returns>A Layout which is really just a collection of widgets.</returns>
        public Layout loadLayout(String file, String prefix)
        {
            return layouts.getObject(LayoutManager_loadLayout2(layoutManager, file, prefix));
        }

        /// <summary>
        /// Load a layout. It is best for memory management to be sure to call
        /// unloadLayout before your program closes. Otherwise a pointer will
        /// leak inside the returned Layout.
        /// </summary>
        /// <param name="file">The file to load.</param>
        /// <returns>A Layout which is really just a collection of widgets.</returns>
        public Layout loadLayout(String file, String prefix, Widget parent)
        {
            return layouts.getObject(LayoutManager_loadLayout3(layoutManager, file, prefix, parent));
        }

        /// <summary>
        /// Unload a layout. This will remove it from the scene and cleanup all
        /// pointers and wrapper classes.
        /// </summary>
        /// <param name="layout">The layout to destroy.</param>
        public void unloadLayout(Layout layout)
        {
            IntPtr vectorWidgetPtr = layout.VectorWidgetPtr;
            LayoutManager_unloadLayout(layoutManager, layouts.destroyObject(vectorWidgetPtr));
            //The loadLayout functions will return a new VectorWidgetPtr on the heap so it must be deleted here.
            VectorWidgetPtr_Delete(vectorWidgetPtr);
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr LayoutManager_getInstancePtr();

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr LayoutManager_loadLayout(IntPtr layoutManager, String file);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr LayoutManager_loadLayout2(IntPtr layoutManager, String file, String prefix);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr LayoutManager_loadLayout3(IntPtr layoutManager, String file, String prefix, Widget parent);

        [DllImport("MyGUIWrapper")]
        private static extern void LayoutManager_unloadLayout(IntPtr layoutManager, IntPtr vectorWidgetPtr);

        [DllImport("MyGUIWrapper")]
        private static extern void VectorWidgetPtr_Delete(IntPtr vectorWidgetPtr);

#endregion
    }
}
