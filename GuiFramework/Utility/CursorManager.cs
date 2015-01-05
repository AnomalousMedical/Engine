using Anomalous.OSPlatform;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework
{
    class CursorManager
    {
        private static Dictionary<string, CursorType> cursors = new Dictionary<string, CursorType>();

        static CursorManager()
        {
            cursors.Add(PointerManager.ARROW, CursorType.Arrow);
            cursors.Add(PointerManager.BEAM, CursorType.Beam);
            cursors.Add(PointerManager.SIZE_LEFT, CursorType.SizeLeft);
            cursors.Add(PointerManager.SIZE_RIGHT, CursorType.SizeRight);
            cursors.Add(PointerManager.SIZE_HORZ, CursorType.SizeHorz);
            cursors.Add(PointerManager.SIZE_VERT, CursorType.SizeVert);
            cursors.Add(PointerManager.HAND, CursorType.Hand);
            cursors.Add(PointerManager.LINK, CursorType.Link);
        }

        private NativeOSWindow window;
        private PointerManager pointerManager;

        public CursorManager(NativeOSWindow window, PointerManager pointerManager)
        {
            this.window = window;
            this.pointerManager = pointerManager;
            window.Disposed += window_Disposed;

            pointerManager.ChangeMousePointer += pointerManager_ChangeMousePointer;
        }

        void pointerManager_ChangeMousePointer(string pointerName)
        {
            window.setCursor(cursors[pointerName]);
        }

        void window_Disposed(Engine.Platform.OSWindow window)
        {
            pointerManager.ChangeMousePointer -= pointerManager_ChangeMousePointer;
        }
    }
}
