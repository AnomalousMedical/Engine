using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class ResourceManager
    {
        private static ResourceManager instance = null;

        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResourceManager();
                }
                return instance;
            }
        }

        private IntPtr resourceManager;

        private ResourceManager()
        {
            resourceManager = ResourceManager_getInstance();
        }

        public bool load(String file)
        {
            return ResourceManager_load(resourceManager, file);
        }

        public bool remove(String name)
        {
            return ResourceManager_remove(resourceManager, name);
        }

        public void clear()
        {
            ResourceManager_clear(resourceManager);
        }

        public uint getCount()
        {
            return ResourceManager_getCount(resourceManager).ToUInt32();
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr ResourceManager_getInstance();

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ResourceManager_load(IntPtr resourceManager, String file);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ResourceManager_remove(IntPtr resourceManager, String name);

        [DllImport("MyGUIWrapper")]
        private static extern void ResourceManager_clear(IntPtr resourceManager);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr ResourceManager_getCount(IntPtr resourceManager);

        #endregion
    }
}
