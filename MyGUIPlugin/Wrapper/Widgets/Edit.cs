using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class Edit : StaticText
    {
        TempStringCallback onlyTextDelegate;

        public Edit(IntPtr edit)
            :base(edit)
        {
            onlyTextDelegate = new TempStringCallback(onlyTextCallback);
        }

        public void setTextIntervalColor(uint start, uint count, Color color)
        {
            Edit_setTextIntervalColor(widget, new UIntPtr(start), new UIntPtr(count), color);
        }

        public void setTextSelection(uint start, uint end)
        {
            Edit_setTextSelection(widget, new UIntPtr(start), new UIntPtr(end));
        }

        public void deleteTextSelection()
        {
            Edit_deleteTextSelection(widget);
        }

        public void setTextSelectionColor(Color value)
        {
            Edit_setTextSelectionColor(widget, value);
        }

        public void insertText(String text)
        {
            Edit_insertText1(widget, text);
        }

        public void insertText(String text, uint index)
        {
            Edit_insertText2(widget, text, new UIntPtr(index));
        }

        public void addText(String text)
        {
            Edit_addText(widget, text);
        }

        public void eraseText(uint start)
        {
            Edit_eraseText1(widget, new UIntPtr(start));
        }

        public void eraseText(uint start, uint count)
        {
            Edit_eraseText2(widget, new UIntPtr(start), new UIntPtr(count));
        }

        public uint TextSelectionStart
        {
            get
            {
                return Edit_getTextSelectionStart(widget).ToUInt32();
            }
        }

        public uint TextSelectionEnd
        {
            get
            {
                return Edit_getTextSelectionEnd(widget).ToUInt32();
            }
        }

        public uint TextSelectionLength
        {
            get
            {
                return Edit_getTextSelectionLength(widget).ToUInt32();
            }
        }

        public bool IsTextSelection
        {
            get
            {
                return Edit_isTextSelection(widget);
            }
        }

        public uint TextCursor
        {
            get
            {
                return Edit_getTextCursor(widget).ToUInt32();
            }
            set
            {
                Edit_setTextCursor(widget, new UIntPtr(value));
            }
        }

        public uint TextLength
        {
            get
            {
                return Edit_getTextLength(widget).ToUInt32();
            }
        }

        public bool OverflowToTheLeft
        {
            get
            {
                return Edit_getOverflowToTheLeft(widget);
            }
            set
            {
                Edit_setOverflowToTheLeft(widget, value);
            }
        }

        public uint MaxTextLength
        {
            get
            {
                return Edit_getMaxTextLength(widget).ToUInt32();
            }
            set
            {
                Edit_setMaxTextLength(widget, new UIntPtr(value));
            }
        }

        public bool EditReadOnly
        {
            get
            {
                return Edit_getEditReadOnly(widget);
            }
            set
            {
                Edit_setEditReadOnly(widget, value);
            }
        }

        public bool EditPassword
        {
            get
            {
                return Edit_getEditPassword(widget);
            }
            set
            {
                Edit_setEditPassword(widget, value);
            }
        }

        public bool EditMultiLine
        {
            get
            {
                return Edit_getEditMultiLine(widget);
            }
            set
            {
                Edit_setEditMultiLine(widget, value);
            }
        }

        public bool EditStatic
        {
            get
            {
                return Edit_getEditStatic(widget);
            }
            set
            {
                Edit_setEditStatic(widget, value);
            }
        }

        public char PasswordChar
        {
            get
            {
                return Edit_getPasswordChar(widget);
            }
            set
            {
                Edit_setPasswordChar(widget, value);
            }
        }

        public bool EditWordWrap
        {
            get
            {
                return Edit_getEditWordWrap(widget);
            }
            set
            {
                Edit_setEditWordWrap(widget, value);
            }
        }

        public bool TabPrinting
        {
            get
            {
                return Edit_getTabPrinting(widget);
            }
            set
            {
                Edit_setTabPrinting(widget, value);
            }
        }

        public bool InvertSelected
        {
            get
            {
                return Edit_getInvertSelected(widget);
            }
            set
            {
                Edit_setInvertSelected(widget, value);
            }
        }

        public bool VisibleVScroll
        {
            get
            {
                return Edit_isVisibleVScroll(widget);
            }
            set
            {
                Edit_setVisibleVScroll(widget, value);
            }
        }

        public uint VScrollRange
        {
            get
            {
                return Edit_getVScrollRange(widget).ToUInt32();
            }
        }

        public uint VScrollPosition
        {
            get
            {
                return Edit_getVScrollPosition(widget).ToUInt32();
            }
            set
            {
                Edit_setVScrollPosition(widget, new UIntPtr(value));
            }
        }

        public bool VisibleHScroll
        {
            get
            {
                return Edit_isVisibleHScroll(widget);
            }
            set
            {
                Edit_setVisibleHScroll(widget, value);
            }
        }

        public uint HScrollRange
        {
            get
            {
                return Edit_getHScrollRange(widget).ToUInt32();
            }
        }

        public uint HScrollPosition
        {
            get
            {
                return Edit_getHScrollPosition(widget).ToUInt32();
            }
            set
            {
                Edit_setHScrollPosition(widget, new UIntPtr(value));
            }
        }

        public bool AllowMouseScroll
        {
            get
            {
                return Edit_getAllowMouseScroll(widget);
            }
            set
            {
                Edit_setAllowMouseScroll(widget, value);
            }
        }

        private String onlyTextBuffer;

        public String OnlyText
        {
            get
            {
                Edit_getOnlyText(widget, onlyTextDelegate);
                return onlyTextBuffer;
            }
            set
            {
                if (value != null)
                {
                    Edit_setOnlyText(widget, value);
                }
                else
                {
                    Edit_setOnlyText(widget, "");
                }
            }
        }

        private void onlyTextCallback(IntPtr str)
        {
            onlyTextBuffer = Marshal.PtrToStringUni(str);
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

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setTextIntervalColor(IntPtr edit, UIntPtr start, UIntPtr count, Color colour);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setTextSelection(IntPtr edit, UIntPtr start, UIntPtr end);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_deleteTextSelection(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setTextSelectionColor(IntPtr edit, Color value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_insertText1(IntPtr edit, [MarshalAs(UnmanagedType.LPWStr)] String text);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_insertText2(IntPtr edit, [MarshalAs(UnmanagedType.LPWStr)] String text, UIntPtr index);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_addText(IntPtr edit, [MarshalAs(UnmanagedType.LPWStr)] String text);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_eraseText1(IntPtr edit, UIntPtr start);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_eraseText2(IntPtr edit, UIntPtr start, UIntPtr count);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Edit_getTextSelectionStart(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Edit_getTextSelectionEnd(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Edit_getTextSelectionLength(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_isTextSelection(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setTextCursor(IntPtr edit, UIntPtr index);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Edit_getTextCursor(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Edit_getTextLength(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setOverflowToTheLeft(IntPtr edit, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_getOverflowToTheLeft(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setMaxTextLength(IntPtr edit, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Edit_getMaxTextLength(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setEditReadOnly(IntPtr edit, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_getEditReadOnly(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setEditPassword(IntPtr edit, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_getEditPassword(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setEditMultiLine(IntPtr edit, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_getEditMultiLine(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setEditStatic(IntPtr edit, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_getEditStatic(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setPasswordChar(IntPtr edit, char value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern char Edit_getPasswordChar(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setEditWordWrap(IntPtr edit, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_getEditWordWrap(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setTabPrinting(IntPtr edit, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_getTabPrinting(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_getInvertSelected(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setInvertSelected(IntPtr edit, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setVisibleVScroll(IntPtr edit, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_isVisibleVScroll(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Edit_getVScrollRange(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Edit_getVScrollPosition(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setVScrollPosition(IntPtr edit, UIntPtr index);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setVisibleHScroll(IntPtr edit, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_isVisibleHScroll(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Edit_getHScrollRange(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Edit_getHScrollPosition(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setHScrollPosition(IntPtr edit, UIntPtr index);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setAllowMouseScroll(IntPtr edit, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Edit_getAllowMouseScroll(IntPtr edit);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_setOnlyText(IntPtr edit, [MarshalAs(UnmanagedType.LPWStr)] String value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Edit_getOnlyText(IntPtr edit, TempStringCallback onlyTextDelegate);

#endregion
    }
}

        //need way to handle strings not returned by reference for these
        //UString getTextInterval(sizet start, sizet count);
        //UString getTextSelection();
        //void setOnlyText(const UString& value);
        //UString getOnlyText();
        //end