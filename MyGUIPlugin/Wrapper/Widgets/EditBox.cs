using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;
using Anomalous.Interop;

namespace MyGUIPlugin
{
    public class EditBox : TextBox
    {
        public EditBox(IntPtr edit)
            :base(edit)
        {
            
        }

        public void setTextIntervalColor(uint start, uint count, Color color)
        {
            EditBox_setTextIntervalColor(widget, new UIntPtr(start), new UIntPtr(count), color);
        }

        public void setTextSelection(uint start, uint end)
        {
            EditBox_setTextSelection(widget, new UIntPtr(start), new UIntPtr(end));
        }

        public void deleteTextSelection()
        {
            EditBox_deleteTextSelection(widget);
        }

        public void setTextSelectionColor(Color value)
        {
            EditBox_setTextSelectionColor(widget, value);
        }

        public void insertText(String text)
        {
            EditBox_insertText1(widget, text);
        }

        public void insertText(String text, uint index)
        {
            EditBox_insertText2(widget, text, new UIntPtr(index));
        }

        public void addText(String text)
        {
            EditBox_addText(widget, text);
        }

        public void eraseText(uint start)
        {
            EditBox_eraseText1(widget, new UIntPtr(start));
        }

        public void eraseText(uint start, uint count)
        {
            EditBox_eraseText2(widget, new UIntPtr(start), new UIntPtr(count));
        }

        public void cut()
        {
            EditBox_cut(widget);
        }

        public void copy()
        {
            EditBox_copy(widget);
        }

        public void paste()
        {
            EditBox_paste(widget);
        }

        public uint TextSelectionStart
        {
            get
            {
                return EditBox_getTextSelectionStart(widget).horriblyUnsafeToUInt32();
            }
        }

        public uint TextSelectionEnd
        {
            get
            {
                return EditBox_getTextSelectionEnd(widget).horriblyUnsafeToUInt32();
            }
        }

        public uint TextSelectionLength
        {
            get
            {
                return EditBox_getTextSelectionLength(widget).horriblyUnsafeToUInt32();
            }
        }

        public bool IsTextSelection
        {
            get
            {
                return EditBox_isTextSelection(widget);
            }
        }

        public uint TextCursor
        {
            get
            {
                return EditBox_getTextCursor(widget).horriblyUnsafeToUInt32();
            }
            set
            {
                EditBox_setTextCursor(widget, new UIntPtr(value));
            }
        }

        public uint TextLength
        {
            get
            {
                return EditBox_getTextLength(widget).horriblyUnsafeToUInt32();
            }
        }

        public bool OverflowToTheLeft
        {
            get
            {
                return EditBox_getOverflowToTheLeft(widget);
            }
            set
            {
                EditBox_setOverflowToTheLeft(widget, value);
            }
        }

        public uint MaxTextLength
        {
            get
            {
                return EditBox_getMaxTextLength(widget).horriblyUnsafeToUInt32();
            }
            set
            {
                EditBox_setMaxTextLength(widget, new UIntPtr(value));
            }
        }

        public bool EditReadOnly
        {
            get
            {
                return EditBox_getEditReadOnly(widget);
            }
            set
            {
                EditBox_setEditReadOnly(widget, value);
            }
        }

        public bool EditPassword
        {
            get
            {
                return EditBox_getEditPassword(widget);
            }
            set
            {
                EditBox_setEditPassword(widget, value);
            }
        }

        public bool EditMultiLine
        {
            get
            {
                return EditBox_getEditMultiLine(widget);
            }
            set
            {
                EditBox_setEditMultiLine(widget, value);
            }
        }

        public bool EditStatic
        {
            get
            {
                return EditBox_getEditStatic(widget);
            }
            set
            {
                EditBox_setEditStatic(widget, value);
            }
        }

        public char PasswordChar
        {
            get
            {
                return EditBox_getPasswordChar(widget);
            }
            set
            {
                EditBox_setPasswordChar(widget, value);
            }
        }

        public bool EditWordWrap
        {
            get
            {
                return EditBox_getEditWordWrap(widget);
            }
            set
            {
                EditBox_setEditWordWrap(widget, value);
            }
        }

        public bool TabPrinting
        {
            get
            {
                return EditBox_getTabPrinting(widget);
            }
            set
            {
                EditBox_setTabPrinting(widget, value);
            }
        }

        public bool InvertSelected
        {
            get
            {
                return EditBox_getInvertSelected(widget);
            }
            set
            {
                EditBox_setInvertSelected(widget, value);
            }
        }

        public bool VisibleVScroll
        {
            get
            {
                return EditBox_isVisibleVScroll(widget);
            }
            set
            {
                EditBox_setVisibleVScroll(widget, value);
            }
        }

        public uint VScrollRange
        {
            get
            {
                return EditBox_getVScrollRange(widget).horriblyUnsafeToUInt32();
            }
        }

        public uint VScrollPosition
        {
            get
            {
                return EditBox_getVScrollPosition(widget).horriblyUnsafeToUInt32();
            }
            set
            {
                EditBox_setVScrollPosition(widget, new UIntPtr(value));
            }
        }

        public bool VisibleHScroll
        {
            get
            {
                return EditBox_isVisibleHScroll(widget);
            }
            set
            {
                EditBox_setVisibleHScroll(widget, value);
            }
        }

        public uint HScrollRange
        {
            get
            {
                return EditBox_getHScrollRange(widget).horriblyUnsafeToUInt32();
            }
        }

        public uint HScrollPosition
        {
            get
            {
                return EditBox_getHScrollPosition(widget).horriblyUnsafeToUInt32();
            }
            set
            {
                EditBox_setHScrollPosition(widget, new UIntPtr(value));
            }
        }

        public bool AllowMouseScroll
        {
            get
            {
                return EditBox_getAllowMouseScroll(widget);
            }
            set
            {
                EditBox_setAllowMouseScroll(widget, value);
            }
        }

        public String OnlyText
        {
            get
            {
                UnicodeStringRetriever sr = new UnicodeStringRetriever();
                EditBox_getOnlyText(widget, sr.StringCallback);
                return sr.retrieveString();
            }
            set
            {
                if (value != null)
                {
                    EditBox_setOnlyText(widget, value);
                }
                else
                {
                    EditBox_setOnlyText(widget, "");
                }
            }
        }

        #region Events

        public event MyGUIEvent EventEditSelectAccept
        {
            add
            {
                eventManager.addDelegate<EventEditSelectAcceptTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventEditSelectAcceptTranslator>(value);
            }
        }

        public event MyGUIEvent EventEditTextChange
        {
            add
            {
                eventManager.addDelegate<EventEditTextChangeTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventEditTextChangeTranslator>(value);
            }
        }

        #endregion

#region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setTextIntervalColor(IntPtr edit, UIntPtr start, UIntPtr count, Color colour);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setTextSelection(IntPtr edit, UIntPtr start, UIntPtr end);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_deleteTextSelection(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setTextSelectionColor(IntPtr edit, Color value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_insertText1(IntPtr edit, [MarshalAs(UnmanagedType.LPWStr)] String text);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_insertText2(IntPtr edit, [MarshalAs(UnmanagedType.LPWStr)] String text, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_addText(IntPtr edit, [MarshalAs(UnmanagedType.LPWStr)] String text);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_eraseText1(IntPtr edit, UIntPtr start);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_eraseText2(IntPtr edit, UIntPtr start, UIntPtr count);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr EditBox_getTextSelectionStart(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr EditBox_getTextSelectionEnd(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr EditBox_getTextSelectionLength(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_isTextSelection(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setTextCursor(IntPtr edit, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr EditBox_getTextCursor(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr EditBox_getTextLength(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setOverflowToTheLeft(IntPtr edit, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_getOverflowToTheLeft(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setMaxTextLength(IntPtr edit, UIntPtr value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr EditBox_getMaxTextLength(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setEditReadOnly(IntPtr edit, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_getEditReadOnly(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setEditPassword(IntPtr edit, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_getEditPassword(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setEditMultiLine(IntPtr edit, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_getEditMultiLine(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setEditStatic(IntPtr edit, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_getEditStatic(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setPasswordChar(IntPtr edit, char value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern char EditBox_getPasswordChar(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setEditWordWrap(IntPtr edit, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_getEditWordWrap(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setTabPrinting(IntPtr edit, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_getTabPrinting(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_getInvertSelected(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setInvertSelected(IntPtr edit, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setVisibleVScroll(IntPtr edit, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_isVisibleVScroll(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr EditBox_getVScrollRange(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr EditBox_getVScrollPosition(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setVScrollPosition(IntPtr edit, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setVisibleHScroll(IntPtr edit, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_isVisibleHScroll(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr EditBox_getHScrollRange(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr EditBox_getHScrollPosition(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setHScrollPosition(IntPtr edit, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setAllowMouseScroll(IntPtr edit, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool EditBox_getAllowMouseScroll(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_setOnlyText(IntPtr edit, [MarshalAs(UnmanagedType.LPWStr)] String value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void EditBox_getOnlyText(IntPtr edit, UnicodeStringRetriever.Callback callback);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void EditBox_cut(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void EditBox_copy(IntPtr edit);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void EditBox_paste(IntPtr edit);

#endregion
    }
}