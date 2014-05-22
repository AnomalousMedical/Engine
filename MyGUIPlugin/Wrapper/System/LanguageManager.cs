using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public sealed class LanguageManager
    {
        static LanguageManager instance;

        public static LanguageManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new LanguageManager();
                }
                return instance;
            }
        }

        private IntPtr languageManager;

        private LanguageManager()
        {
            languageManager = LanguageManager_getInstancePtr();
        }

        public void setCurrentLanguage(String name)
        {
            LanguageManager_setCurrentLanguage(languageManager, name);
        }

        public String getCurrentLanguage()
        {
            return LanguageManager_getCurrentLanguage(languageManager);
        }

        public void addUserTag(String tag, String replace)
        {
            LanguageManager_addUserTag(languageManager, tag, replace);
        }

        public void clearUserTags()
        {
            LanguageManager_clearUserTags(languageManager);
        }

        public bool loadUserTags(String file)
        {
            return LanguageManager_loadUserTags(languageManager, file);
        }

#region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr LanguageManager_getInstancePtr();

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void LanguageManager_setCurrentLanguage(IntPtr languageManager, String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern String LanguageManager_getCurrentLanguage(IntPtr languageManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void LanguageManager_addUserTag(IntPtr languageManager, [MarshalAs(UnmanagedType.LPWStr)] String tag, [MarshalAs(UnmanagedType.LPWStr)] String replace);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void LanguageManager_clearUserTags(IntPtr languageManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool LanguageManager_loadUserTags(IntPtr languageManager, String file);

#endregion
    }
}
