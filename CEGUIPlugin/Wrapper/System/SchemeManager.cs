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

        private SchemeManager()
        {
            
        }

        public void create(String scheme)
        {
            SchemeManager_create(scheme);
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr SchemeManager_create(String scheme);

#endregion
    }
}
