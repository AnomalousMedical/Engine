using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class PointerManager : IDisposable
    {
        public const String ARROW = "arrow";
        public const String BEAM = "beam";
        public const String SIZE_LEFT = "size_left";
        public const String SIZE_RIGHT = "size_right";
        public const String SIZE_HORZ = "size_horz";
        public const String SIZE_VERT = "size_vert";
        public const String HAND = "hand";
        public const String LINK = "link";

        static PointerManager instance;

        public static PointerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PointerManager();
                }
                return instance;
            }
        }

        private IntPtr pointerManager;
        private EventChangeMousePointerTranslator changeMousePointerTranslator;

        private PointerManager()
        {
            pointerManager = PointerManager_getInstancePtr();
        }

        public void Dispose()
        {
            if (changeMousePointerTranslator != null)
            {
                changeMousePointerTranslator.Dispose();
                changeMousePointerTranslator = null;
            }
        }

        internal IntPtr PointerManagerPtr
        {
            get
            {
                return pointerManager;
            }
        }

        public void setPointer(String name)
        {
            PointerManager_setPointer(pointerManager, name);
        }

        public void resetToDefaultPointer()
        {
            PointerManager_resetToDefaultPointer(pointerManager);
        }

        public bool Visible
        {
            get
            {
                return PointerManager_isVisible(pointerManager);
            }
            set
            {
                PointerManager_setVisible(pointerManager, value);
            }
        }

        public String DefaultPointer
        {
            get
            {
                return Marshal.PtrToStringAnsi(PointerManager_getDefaultPointer(pointerManager));
            }
            set
            {
                PointerManager_setDefaultPointer(pointerManager, value);
            }
        }

        public String LayerName
        {
            get
            {
                return Marshal.PtrToStringAnsi(PointerManager_getLayerName(pointerManager));
            }
            set
            {
                PointerManager_setLayerName(pointerManager, value);
            }
        }

        public event MousePointerChanged ChangeMousePointer
        {
            add
            {
                if (changeMousePointerTranslator == null)
                {
                    changeMousePointerTranslator = new EventChangeMousePointerTranslator(this);
                }
                changeMousePointerTranslator.BoundEvent += value;
            }
            remove
            {
                if (changeMousePointerTranslator != null)
                {
                    changeMousePointerTranslator.BoundEvent -= value;
                }
            }
        }

        //IPointer* getByName(String _name) const;

#region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr PointerManager_getInstancePtr();

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PointerManager_setPointer(IntPtr pointerManager, String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PointerManager_resetToDefaultPointer(IntPtr pointerManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PointerManager_setVisible(IntPtr pointerManager, bool visible);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool PointerManager_isVisible(IntPtr pointerManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr PointerManager_getDefaultPointer(IntPtr pointerManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PointerManager_setDefaultPointer(IntPtr pointerManager, String value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr PointerManager_getLayerName(IntPtr pointerManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PointerManager_setLayerName(IntPtr pointerManager, String value);

#endregion
    }
}
