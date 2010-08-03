using System;
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

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr FontManager_getInstancePtr();

#endregion
    }
}
