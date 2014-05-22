using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public class ElementDocument : Element
    {
        public enum FocusFlags
        {
            NONE = 0,
            FOCUS = (1 << 1),
            MODAL = (1 << 2)
        };

        internal ElementDocument(IntPtr ptr)
            : base(ptr)
        {

        }
        
        public void PullToFront()
        {
            ElementDocument_PullToFront(ptr);
        }
        
        public void PushToBack()
        {
            ElementDocument_PushToBack(ptr);
        }

        public void Show(FocusFlags focusFlags = FocusFlags.FOCUS)
        {
            ElementDocument_Show(ptr, focusFlags);
        }

        public void Hide()
        {
            ElementDocument_Hide(ptr);
        }

        public void Close()
        {
            ElementDocument_Close(ptr);
        }

        public Element CreateElement(String name)
        {
            return ElementManager.getElement(ElementDocument_CreateElement(ptr, name));
        }

        public Element/*Should be ElementText (not wrapped yet)*/ CreateTextNode(String text)
        {
            return ElementManager.getElement(ElementDocument_CreateTextNode(ptr, text));
        }

        /// <summary>
        /// Dirties any element properties that need to be dirtied to respond correctly to a scale change
        /// on the context.
        /// </summary>
        public void MakeDirtyForScaleChange()
        {
            ElementDocument_MakeDirtyForScaleChange(ptr);
        }

        public void UpdatePosition()
        {
            ElementDocument_UpdatePosition(ptr);
        }

        public void UpdateLayout()
        {
            ElementDocument_UpdateLayout(ptr);
        }

        //properties
        public String Title
        {
            get
            {
                return Marshal.PtrToStringAnsi(ElementDocument_GetTitle(ptr));
            }
            set
            {
                ElementDocument_SetTitle(ptr, value);
            }
        }

        public String SourceURL
        {
            get
            {
                return Marshal.PtrToStringAnsi(ElementDocument_GetSourceURL(ptr));
            }
        }

        public bool IsModal
        {
            get
            {
                return ElementDocument_IsModal(ptr);
            }
        }

        #region PInvoke
        
        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementDocument_SetTitle(IntPtr elementDocument, String title);
        
        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ElementDocument_GetTitle(IntPtr elementDocument);
        
        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ElementDocument_GetSourceURL(IntPtr elementDocument);
        
        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementDocument_PullToFront(IntPtr elementDocument);
        
        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementDocument_PushToBack(IntPtr elementDocument);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementDocument_Show(IntPtr elementDocument, FocusFlags focusFlags);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementDocument_Hide(IntPtr elementDocument);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementDocument_Close(IntPtr elementDocument);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ElementDocument_CreateElement(IntPtr elementDocument, String name);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ElementDocument_CreateTextNode(IntPtr elementDocument, String text);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ElementDocument_IsModal(IntPtr elementDocument);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementDocument_UpdatePosition(IntPtr elementDocument);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementDocument_UpdateLayout(IntPtr elementDocument);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ElementDocument_MakeDirtyForScaleChange(IntPtr elementDocument);

        #endregion
    }
}
