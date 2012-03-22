using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class LayerManager
    {
        private static LayerManager instance;

        public static LayerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LayerManager();
                }
                return instance;
            }
        }

        private IntPtr layerManager;

        private LayerManager()
        {
            layerManager = LayerManager_getSingletonPtr();
        }

        public void attachToLayerNode(String name, Widget item)
        {
            LayerManager_attachToLayerNode(layerManager, name, item.WidgetPtr);
        }

        public void detachFromLayer(Widget item)
        {
            LayerManager_detachFromLayer(layerManager, item.WidgetPtr);
        }

        public void upLayerItem(Widget item)
        {
            LayerManager_upLayerItem(layerManager, item.WidgetPtr);
        }

        public bool isExist(String name)
        {
            return LayerManager_isExist(layerManager, name);
        }

        public Widget getWidgetFromPoint(int left, int top)
        {
            return WidgetManager.getWidget(LayerManager_getWidgetFromPoint(layerManager, left, top));
        }

#region PInvoke
        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr LayerManager_getSingletonPtr();

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void LayerManager_attachToLayerNode(IntPtr layerManager, String name, IntPtr item);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void LayerManager_detachFromLayer(IntPtr layerManager, IntPtr item);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void LayerManager_upLayerItem(IntPtr layerManager, IntPtr item);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool LayerManager_isExist(IntPtr layerManager, String name);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr LayerManager_getWidgetFromPoint(IntPtr layerManager, int left, int top);
#endregion
    }
}
