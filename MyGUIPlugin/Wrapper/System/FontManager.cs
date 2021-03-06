﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class FontManager
    {
        static FontManager instance;

        public static FontManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FontManager();
                }
                return instance;
            }
        }

        private IntPtr fontManager;

        private FontManager()
        {
            fontManager = FontManager_getInstancePtr();
        }

        public String DefaultFont
        {
            get
            {
                return Marshal.PtrToStringAnsi(FontManager_getDefaultFont(fontManager));
            }
            set
            {
                FontManager_setDefaultFont(fontManager, value);
            }
        }

#region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr FontManager_getInstancePtr();

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void FontManager_setDefaultFont(IntPtr fontManager, String value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr FontManager_getDefaultFont(IntPtr fontManager);

#endregion
    }
}
