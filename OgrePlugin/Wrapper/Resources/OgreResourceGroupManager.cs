using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgreWrapper
{
    public delegate void ResourcesInitialized();

    [NativeSubsystemType]
    public class OgreResourceGroupManager
    {
        /// <summary>
        /// Called when the resources are initialized.
        /// </summary>
        public event ResourcesInitialized OnResourcesInitialized;

        static OgreResourceGroupManager instance = null;

        public static OgreResourceGroupManager getInstance()
        {
            if(instance == null)
            {
                instance = new OgreResourceGroupManager();
            }
            return instance;
        }

        private OgreResourceGroupManager()
        {

        }

        /// <summary>
	    /// Create a resource group.
	    /// </summary>
	    /// <param name="name">The name to give the resource group. </param>
        public void createResourceGroup(String name)
        {
            ResourceGroupManager_createResourceGroup(name);
        }

	    /// <summary>
	    /// Initialize all resources that need it.  Should be called whenever the resources are changed.
	    /// </summary>
        public void initializeAllResourceGroups()
        {
            if(ResourceGroupManager_initializeAllResourceGroups() && OnResourcesInitialized != null)
            {
                OnResourcesInitialized.Invoke();
            }
        }

        public void initializeResourceGroup(String name)
        {
            if (ResourceGroupManager_initializeResourceGroup(name) && OnResourcesInitialized != null)
            {
                OnResourcesInitialized.Invoke();
            }
        }

        public void destroyResourceGroup(String name)
        {
            ResourceGroupManager_destroyResourceGroup(name);
        }

	    /// <summary>
	    /// Returns the group the specified resource belongs to or null if it does not exist.
	    /// </summary>
	    /// <param name="name">The fully qualified name of the resource.</param>
	    /// <param name="locType">The type of location the resource is located on.  Typically "FileSystem"</param>
	    /// <param name="group">The group the resource belongs to.</param>
	    /// <param name="recursive">True to search subdirectories.</param>
        public void addResourceLocation(String name, String locType, String group, bool recursive)
        {
            ResourceGroupManager_addResourceLocation(name, locType, group, recursive);
        }

	    /// <summary>
	    /// Removes a resource location from the search path. 
	    /// </summary>
	    /// <param name="name">The location to remove.</param>
	    /// <param name="resGroup">The resource group of the location to remove.</param>
        public void removeResourceLocation(String name, String resGroup)
        {
            ResourceGroupManager_removeResourceLocation(name, resGroup);
        }

	    /// <summary>
	    /// Returns the group the specified resource belongs to or null if it does not exist.
	    /// </summary>
	    /// <param name="resourceName">The fully qualified name of the resource.</param>
	    /// <returns>The name of the group containg the resource or null if the resource was not found.</returns>
        public String findGroupContainingResource(String resourceName)
        {
            if(resourceName != null)
            {
                IntPtr strPtr = ResourceGroupManager_findGroupContainingResource(resourceName);
                if (strPtr != IntPtr.Zero)
                {
                    return Marshal.PtrToStringAnsi(strPtr);
                }
            }
            return null;
        }

        public bool resourceGroupExists(String name)
        {
            return ResourceGroupManager_resourceGroupExists(name);
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern void ResourceGroupManager_createResourceGroup(String name);

        [DllImport("OgreCWrapper")]
        private static extern bool ResourceGroupManager_initializeAllResourceGroups();

        [DllImport("OgreCWrapper")]
        private static extern bool ResourceGroupManager_initializeResourceGroup(String name);

        [DllImport("OgreCWrapper")]
        private static extern void ResourceGroupManager_destroyResourceGroup(String name);

        [DllImport("OgreCWrapper")]
        private static extern void ResourceGroupManager_addResourceLocation(String name, String locType, String group, bool recursive);

        [DllImport("OgreCWrapper")]
        private static extern void ResourceGroupManager_removeResourceLocation(String name, String resGroup);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr ResourceGroupManager_findGroupContainingResource(String resourceName);

        [DllImport("OgreCWrapper")]
        private static extern bool ResourceGroupManager_resourceGroupExists(String name);

#endregion
    }
}
