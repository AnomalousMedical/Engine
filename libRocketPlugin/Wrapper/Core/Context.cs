using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

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
            throw new NotImplementedException();
            //return Context_CreateDocument(ptr);
        }

        public ElementDocument CreateDocument_Tag(String tag)
        {
            throw new NotImplementedException();
            //return Context_CreateDocument_Tag(ptr);
        }

        public ElementDocument LoadDocument(String document_path)
        {
            return ElementDocument.Wrap(Context_LoadDocument(ptr, document_path));
        }

        public ElementDocument LoadDocumentFromMemory(String mem)
        {
            throw new NotImplementedException();
            //return Context_LoadDocumentFromMemory(ptr);
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
            throw new NotImplementedException();
            //return Context_LoadMouseCursor(ptr);
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
            throw new NotImplementedException();
            //return Context_GetDocument(ptr);
        }

        public ElementDocument GetDocument_Index(int index)
        {
            throw new NotImplementedException();
            //return Context_GetDocument_Index(ptr);
        }

        public int GetNumDocuments()
        {
            return Context_GetNumDocuments(ptr);
        }

        public Element GetHoverElement()
        {
            throw new NotImplementedException();
            //return Context_GetHoverElement(ptr);
        }

        public Element GetFocusElement()
        {
            throw new NotImplementedException();
            //return Context_GetFocusElement(ptr);
        }

        public Element GetRootElement()
        {
            throw new NotImplementedException();
            //return Context_GetRootElement(ptr);
        }

        public void PullDocumentToFront(ElementDocument document)
        {
            Context_PullDocumentToFront(ptr, document.Ptr);
        }

        public void PushDocumentToBack(ElementDocument document)
        {
            Context_PushDocumentToBack(ptr, document.Ptr);
        }

        public void AddEventListener(String evt, EventListener listener, bool in_capture_phase/* = false*/)
        {
            throw new NotImplementedException();
            //Context_AddEventListener(ptr);
        }

        public void RemoveEventListener(String evt, EventListener listener, bool in_capture_phase/* = false*/)
        {
            throw new NotImplementedException();
            //Context_RemoveEventListener(ptr);
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

        #region PInvoke

        //extern "C" _AnomalousExport String Context_GetName(IntPtr context)
        //{
        //	return context->GetName().CString();
        //}

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_SetDimensions(IntPtr context, Vector2i dimensions);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern ThreeIntHack Context_GetDimensions(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_Update(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_Render(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_CreateDocument(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_CreateDocument_Tag(IntPtr context, String tag);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_LoadDocument(IntPtr context, String document_path);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_LoadDocumentFromMemory(IntPtr context, String str);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_UnloadDocument(IntPtr context, IntPtr document);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_UnloadAllDocuments(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_AddMouseCursor(IntPtr context, IntPtr cursor_document);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_LoadMouseCursor(IntPtr context, String cursor_document_path);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_UnloadMouseCursor(IntPtr context, String cursor_name);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_UnloadAllMouseCursors(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_SetMouseCursor(IntPtr context, String cursor_name);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_ShowMouseCursor(IntPtr context, bool show);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetDocument(IntPtr context, String id);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetDocument_Index(IntPtr context, int index);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int Context_GetNumDocuments(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetHoverElement(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetFocusElement(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetRootElement(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_PullDocumentToFront(IntPtr context, IntPtr document);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_PushDocumentToBack(IntPtr context, IntPtr document);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_AddEventListener(IntPtr context, String evt, IntPtr listener, bool in_capture_phase/* = false*/);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_RemoveEventListener(IntPtr context, String evt, IntPtr listener, bool in_capture_phase/* = false*/);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_ProcessKeyDown(IntPtr context, KeyIdentifier key_identifier, int key_modifier_state);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_ProcessKeyUp(IntPtr context, KeyIdentifier key_identifier, int key_modifier_state);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_ProcessTextInput_Word(IntPtr context, ushort character);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_ProcessTextInput(IntPtr context, String str);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_ProcessMouseMove(IntPtr context, int x, int y, int key_modifier_state);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_ProcessMouseButtonDown(IntPtr context, int button_index, int key_modifier_state);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Context_ProcessMouseButtonUp(IntPtr context, int button_index, int key_modifier_state);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Context_ProcessMouseWheel(IntPtr context, int wheel_delta, int key_modifier_state);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Context_GetRenderInterface(IntPtr context);

        #endregion
    }
}
