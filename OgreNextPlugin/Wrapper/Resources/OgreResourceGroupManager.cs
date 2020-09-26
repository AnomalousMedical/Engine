using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgreNextPlugin
{
    public delegate void ResourcesInitialized();

    [NativeSubsystemType]
    public class OgreResourceGroupManager
    {
        public const String AutodetectResourceGroup = "Autodetect";
        public const String DefaultResourceGroup = "General";
        public const String InternalResourceGroup = "Internal";

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
        /// <param name="changeLocaleTemporarily">
        /// Ogre regrettably relies on locale conversion for parsing scripts, which means
        /// a script with value "1.76" will be read as "1" in some systems because the
        /// locale is configured to expect "1,76" instead.
        /// When you pass true to this function, we will temporarily change the locale
        /// to "C" and then restore it back when we're done.
        /// However this is not ideal as it may affect your program if you have
        /// other threads; or it could affect your code if a listener is triggered
        /// and you expect a particular locale; therefore the final decision of changing
        /// the locale is left to you.
        /// </param>
        public void initializeAllResourceGroups(bool changeLocaleTemporarily)
        {
            if(ResourceGroupManager_initializeAllResourceGroups(changeLocaleTemporarily) && OnResourcesInitialized != null)
            {
                OnResourcesInitialized.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">The name of the resource group.</param>
        /// <param name="changeLocaleTemporarily">
        /// Ogre regrettably relies on locale conversion for parsing scripts, which means
        /// a script with value "1.76" will be read as "1" in some systems because the
        /// locale is configured to expect "1,76" instead.
        /// When you pass true to this function, we will temporarily change the locale
        /// to "C" and then restore it back when we're done.
        /// However this is not ideal as it may affect your program if you have
        /// other threads; or it could affect your code if a listener is triggered
        /// and you expect a particular locale; therefore the final decision of changing
        /// the locale is left to you.
        /// </param>
        public void initializeResourceGroup(String name, bool changeLocaleTemporarily)
        {
            if (ResourceGroupManager_initializeResourceGroup(name, changeLocaleTemporarily) && OnResourcesInitialized != null)
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

        public void declareResource(String name, String resourceType, String groupName)
        {
            ResourceGroupManager_declareResource(name, resourceType, groupName);
        }

        public OgreDataStreamPtr openResource(String resourceName, String groupName, bool searchGroupsIfNotFound)
        {
            IntPtr ptr = ResourceGroupManager_openResource(resourceName, groupName, searchGroupsIfNotFound, OgreDataStream.ProcessWrapperObjectCallback);
            OgreExceptionManager.fireAnyException();
            return OgreDataStream.getObject(ptr);
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ResourceGroupManager_createResourceGroup(String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ResourceGroupManager_initializeAllResourceGroups(bool changeLocaleTemporarily);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ResourceGroupManager_initializeResourceGroup(String name, bool changeLocaleTemporarily);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ResourceGroupManager_destroyResourceGroup(String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ResourceGroupManager_addResourceLocation(String name, String locType, String group, bool recursive);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ResourceGroupManager_removeResourceLocation(String name, String resGroup);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ResourceGroupManager_findGroupContainingResource(String resourceName);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ResourceGroupManager_resourceGroupExists(String name);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ResourceGroupManager_declareResource(String name, String resourceType, String groupName);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ResourceGroupManager_openResource(String resourceName, String groupName, bool searchGroupsIfNotFound, ProcessWrapperObjectDelegate processWrapper);

#endregion
    }
}
