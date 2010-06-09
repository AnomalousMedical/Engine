using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CEGUIPlugin
{
    public class SchemeManager
    {
        static SchemeManager instance = new SchemeManager();

        public static SchemeManager Singleton
        {
            get
            {
                return instance;
            }
        }

        private IntPtr schemeManager;

        private SchemeManager()
        {
            schemeManager = SchemeManager_getSingletonPtr();
        }

        public void create(String scheme)
        {
            SchemeManager_create(schemeManager, scheme);
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr SchemeManager_getSingletonPtr();

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr SchemeManager_create(IntPtr schemeManager, String scheme);

#endregion
    }
}
