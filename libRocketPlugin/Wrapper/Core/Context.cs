using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace libRocketPlugin
{
    public class Context : ReferenceCountable
    {
        public Context(IntPtr ptr)
            :base(ptr)
        {
            
        }

        public Vector2i Dimensions
        {
            get
            {
                return Context_GetDimensions(ptr).toVector2i();
            }
            set
            {
                Context_SetDimensions(ptr, value);
            }
        }

        public bool Update()
        {
            return Context_Update(ptr);
        }

        public bool Render()
        {
            return Context_Render(ptr);
        }

        public ElementDocument CreateDocument()
        {
            return ElementManager.getElement<ElementDocument>(Context_CreateDocument(ptr));
        }

        public ElementDocument CreateDocument(String tag)
        {
            return ElementManager.getElement<ElementDocument>(Context_CreateDocument_Tag(ptr, tag));
        }

        public ElementDocument LoadDocument(String document_path)
        {
            return ElementManager.getElement<ElementDocument>(Context_LoadDocument(ptr, Core.MakeSafePath(document_path)));
        }

        public ElementDocument LoadDocumentFromMemory(String mem)
        {
            return ElementManager.getElement<ElementDocument>(Context_LoadDocumentFromMemory(ptr, mem));
        }

        public ElementDocument LoadDocumentFromMemory(String mem, String fakePath)
        {
            return ElementManager.getElement<ElementDocument>(Context_LoadDocumentFromMemoryPath(ptr, mem, fakePath));
        }

        public void UnloadDocument(ElementDocument document)
        {
            Context_UnloadDocument(ptr, document.Ptr);
        }

        public void UnloadAllDocuments()
        {
            Context_UnloadAllDocuments(ptr);
        }

        public void AddMouseCursor(ElementDocument cursor_document)
        {
            Context_AddMouseCursor(ptr, cursor_document.Ptr);
        }

        public ElementDocument LoadMouseCursor(String cursor_document_path)
        {
            return ElementManager.getElement<ElementDocument>(Context_LoadMouseCursor(ptr, cursor_document_path));
        }

        public void UnloadMouseCursor(String cursor_name)
        {
            Context_UnloadMouseCursor(ptr, cursor_name);
        }

        public void UnloadAllMouseCursors()
        {
            Context_UnloadAllMouseCursors(ptr);
        }

        public bool SetMouseCursor(String cursor_name)
        {
            return Context_SetMouseCursor(ptr, cursor_name);
        }

        public void ShowMouseCursor(bool show)
        {
            Context_ShowMouseCursor(ptr, show);
        }

        public ElementDocument GetDocument(String id)
        {
            return ElementManager.getElement<ElementDocument>(Context_GetDocument(ptr, id));
        }

        public ElementDocument GetDocument(int index)
        {
            return ElementManager.getElement<ElementDocument>(Context_GetDocument_Index(ptr, index));
        }

        public int GetNumDocuments()
        {
            return Context_GetNumDocuments(ptr);
        }

        public Element GetHoverElement()
        {
            return ElementManager.getElement(Context_GetHoverElement(ptr));
        }

        public Element GetFocusElement()
        {
            return ElementManager.getElement(Context_GetFocusElement(ptr));
        }

        public Element GetRootElement()
        {
            return ElementManager.getElement(Context_GetRootElement(ptr));
        }

        public void PullDocumentToFront(ElementDocument document)
        {
            Context_PullDocumentToFront(ptr, document.Ptr);
        }

        public void PushDocumentToBack(ElementDocument document)
        {
            Context_PushDocumentToBack(ptr, document.Ptr);
        }

        public void AddEventListener(String evt, EventListener listener)
        {
            Context_AddEventListener(ptr, evt, listener.Ptr, false);
        }

        public void AddEventListener(String evt, EventListener listener, bool in_capture_phase/* = false*/)
        {
            Context_AddEventListener(ptr, evt, listener.Ptr, in_capture_phase);
        }

        public void RemoveEventListener(String evt, EventListener listener)
        {
            Context_RemoveEventListener(ptr, evt, listener.Ptr, false);
        }

        public void RemoveEventListener(String evt, EventListener listener, bool in_capture_phase/* = false*/)
        {
            Context_RemoveEventListener(ptr, evt, listener.Ptr, in_capture_phase);
        }

        public bool ProcessKeyDown(KeyIdentifier key_identifier, int key_modifier_state)
        {
            return Context_ProcessKeyDown(ptr, key_identifier, key_modifier_state);
        }

        public bool ProcessKeyUp(KeyIdentifier key_identifier, int key_modifier_state)
        {
            return Context_ProcessKeyUp(ptr, key_identifier, key_modifier_state);
        }

        public bool ProcessTextInput(ushort character)
        {
            return Context_ProcessTextInput_Word(ptr, character);
        }

        public bool ProcessTextInput(String str)
        {
            return Context_ProcessTextInput(ptr, str);
        }

        public void ProcessMouseMove(int x, int y, int key_modifier_state)
        {
            Context_ProcessMouseMove(ptr, x, y, key_modifier_state);
        }

        public void ProcessMouseButtonDown(int button_index, int key_modifier_state)
        {
            Context_ProcessMouseButtonDown(ptr, button_index, key_modifier_state);
        }

        public void ProcessMouseButtonUp(int button_index, int key_modifier_state)
        {
            Context_ProcessMouseButtonUp(ptr, button_index, key_modifier_state);
        }

        public bool ProcessMouseWheel(int wheel_delta, int key_modifier_state)
        {
            return Context_ProcessMouseWheel(ptr, wheel_delta, key_modifier_state);
        }

        public RenderInterface GetRenderInterface()
        {
            throw new NotImplementedException();
            //return Context_GetRenderInterface(ptr);
        }

        public Element FindElementAtPoint(Vector2 point, Element ignoreElement = null)
        {
            return ElementManager.getElement(Context_FindElementAtPoint(ptr, point, ignoreElement.PtrOrNull()));
        }

        public float ZoomLevel
        {
            get
            {
                return Context_GetZoomLevel(ptr);
            }
            set
            {
                Context_SetZoomLevel(ptr, value);
            }
        }

        public IEnumerable<ElementDocument> Documents
        {
            get
            {
                int num = GetNumDocuments();
                for (int i = 0; i < num; ++i)
                {
                    yield return GetDocument(i);
                }
            }
        }

        #region PInvoke

        //extern "C" _AnomalousExport String Context_GetName(IntPtr context)
        //{
        //	return context->GetName().CString();
        //}

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_SetDimensions(IntPtr context, Vector2i dimensions);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern ThreeIntHack Context_GetDimensions(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_Update(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_Render(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_CreateDocument(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_CreateDocument_Tag(IntPtr context, String tag);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_LoadDocument(IntPtr context, String document_path);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_LoadDocumentFromMemory(IntPtr context, String str);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_LoadDocumentFromMemoryPath(IntPtr context, String str, String fakePath);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_UnloadDocument(IntPtr context, IntPtr document);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_UnloadAllDocuments(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_AddMouseCursor(IntPtr context, IntPtr cursor_document);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_LoadMouseCursor(IntPtr context, String cursor_document_path);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_UnloadMouseCursor(IntPtr context, String cursor_name);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_UnloadAllMouseCursors(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_SetMouseCursor(IntPtr context, String cursor_name);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_ShowMouseCursor(IntPtr context, bool show);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetDocument(IntPtr context, String id);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetDocument_Index(IntPtr context, int index);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern int Context_GetNumDocuments(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetHoverElement(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetFocusElement(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetRootElement(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_PullDocumentToFront(IntPtr context, IntPtr document);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_PushDocumentToBack(IntPtr context, IntPtr document);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_AddEventListener(IntPtr context, String evt, IntPtr listener, bool in_capture_phase/* = false*/);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_RemoveEventListener(IntPtr context, String evt, IntPtr listener, bool in_capture_phase/* = false*/);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_ProcessKeyDown(IntPtr context, KeyIdentifier key_identifier, int key_modifier_state);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_ProcessKeyUp(IntPtr context, KeyIdentifier key_identifier, int key_modifier_state);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_ProcessTextInput_Word(IntPtr context, ushort character);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_ProcessTextInput(IntPtr context, String str);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_ProcessMouseMove(IntPtr context, int x, int y, int key_modifier_state);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_ProcessMouseButtonDown(IntPtr context, int button_index, int key_modifier_state);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_ProcessMouseButtonUp(IntPtr context, int button_index, int key_modifier_state);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_ProcessMouseWheel(IntPtr context, int wheel_delta, int key_modifier_state);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetRenderInterface(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Context_FindElementAtPoint(IntPtr context, Vector2 point, IntPtr ignore_element);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern float Context_GetZoomLevel(IntPtr context);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Context_SetZoomLevel(IntPtr context, float value);

        #endregion
    }
}
