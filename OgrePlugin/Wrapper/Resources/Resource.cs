using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public abstract class Resource : IDisposable
    {
        [SingleEnum]
        public enum LoadingState : uint
        {
	        LOADSTATE_UNLOADED,
	        LOADSTATE_LOADING,
	        LOADSTATE_LOADED,
	        LOADSTATE_UNLOADING,
	        LOADSTATE_PREPARED,
	        LOADSTATE_PREPARING
        };

        protected IntPtr resource;

        protected Resource(IntPtr resource)
        {
            this.resource = resource;
        }

        public virtual void Dispose()
        {
            resource = IntPtr.Zero;
        }

        internal IntPtr OgreResource
        {
            get
            {
                return resource;
            }
        }

        /// <summary>
	    /// Get the name of the resource.
	    /// </summary>
	    /// <returns>The name of the resource</returns>
        public String getName()
        {
            return Marshal.PtrToStringAnsi(Resource_getName(resource));
        }

	    /// <summary>
	    /// Get the resource handle.
	    /// </summary>
	    /// <returns>The resource handle.</returns>
        public ulong getHandle()
        {
            return Resource_getHandle(resource);
        }

	    /// <summary>
	    /// Get the resource group.
	    /// </summary>
	    /// <returns>The resource group.</returns>
        public String getGroup()
        {
            return Marshal.PtrToStringAnsi(Resource_getGroup(resource));
        }

	    /// <summary>
	    /// Prepares the resource for load, if it is not already. 
	    /// </summary>
        public void prepare()
        {
            Resource_prepare(resource);
        }

	    /// <summary>
	    /// Loads the resource, if it is not already. 
	    /// </summary>
	    /// <param name="backgroundThread">True to load in the background thread.</param>
        public void load(bool backgroundThread)
        {
            Resource_load(resource, backgroundThread);
        }

	    /// <summary>
	    /// Reloads the resource, if it is already loaded. 
	    /// </summary>
        public void reload()
        {
            Resource_reload(resource);
        }

	    /// <summary>
	    /// Check to see if the resource can be reloaded.
	    /// </summary>
	    /// <returns>True if the resource can be reloaded.  False if it cannot.</returns>
        public bool isReloadable()
        {
            return Resource_isReloadable(resource);
        }

	    /// <summary>
	    /// Check to see if the resource is manually loaded.
	    /// </summary>
	    /// <returns>True if manually loaded.  False if not.</returns>
        public bool isManuallyLoaded()
        {
            return Resource_isManuallyLoaded(resource);
        }

	    /// <summary>
	    /// Unloads the resource; this is not permanent, the resource can be reloaded later if required. 
	    /// </summary>
        public void unload()
        {
            Resource_unload(resource);
        }

	    /// <summary>
	    /// Retrieves info about the size of the resource. 
	    /// </summary>
	    /// <returns></returns>
        public uint getSize()
        {
            return Resource_getSize(resource);
        }

	    /// <summary>
	    /// 'Touches' the resource to indicate it has been used. 
	    /// </summary>
        public void touch()
        {
            Resource_touch(resource);
        }

	    /// <summary>
	    /// Check to see if the resource is prepared.
	    /// </summary>
	    /// <returns>Returns true if the Resource has been prepared, false otherwise.</returns>
        public bool isPrepared()
        {
            return Resource_isPrepared(resource);
        }

	    /// <summary>
	    /// Check to see if the resource is loaded.
	    /// </summary>
	    /// <returns>Returns true if the Resource has been loaded, false otherwise.</returns>
        public bool isLoaded()
        {
            return Resource_isLoaded(resource);
        }

	    /// <summary>
	    /// Returns whether the resource is currently in the process of background loading. 
	    /// </summary>
	    /// <returns>True if still loading.  False if loaded.</returns>
        public bool isLoading()
        {
            return Resource_isLoading(resource);
        }

	    /// <summary>
	    /// Returns the current loading state. 
	    /// </summary>
	    /// <returns>The current loading state.</returns>
        public Resource.LoadingState getLoadingState()
        {
            return Resource_getLoadingState(resource);
        }

	    /// <summary>
	    /// Returns whether this Resource has been earmarked for background loading. 
	    /// </summary>
	    /// <returns>True if this is set to load in the background.</returns>
        public bool isBackgroundLoaded()
        {
            return Resource_isBackgroundLoaded(resource);
        }

	    /// <summary>
	    /// Tells the resource whether it is background loaded or not. 
	    /// </summary>
	    /// <param name="bl">True to use background loading.  False to use inline loading.</param>
        public void setBackgroundLoaded(bool bl)
        {
            Resource_setBackgroundLoaded(resource, bl);
        }

	    /// <summary>
	    /// Escalates the loading of a background loaded resource. 
	    /// 
	    /// If a resource is set to load in the background, but something needs it before it's been 
	    /// loaded, there could be a problem. If the user of this resource really can't wait, they can 
	    /// escalate the loading which basically pulls the loading into the current thread immediately. 
	    /// If the resource is already being loaded but just hasn't quite finished then this method 
	    /// will simply wait until the background load is complete.
	    /// </summary>
        public void escalateLoading()
        {
            Resource_escalateLoading(resource);
        }

	    /// <summary>
	    /// Get the origin of this resource, e.g. a script file name. 
	    /// </summary>
	    /// <returns>The origin of the resource.</returns>
        public String getOrigin()
        {
            return Marshal.PtrToStringAnsi(Resource_getOrigin(resource));
        }

	    /// <summary>
	    /// Returns the number of times this resource has changed state, which generally means the 
	    /// number of times it has been loaded.
	    /// 
	    /// Objects that build derived data based on the resource can check this value against a copy 
	    /// they kept last time they built this derived data, in order to know whether it needs 
	    /// rebuilding. This is a nice way of monitoring changes without having a tightly-bound callback. 
	    /// </summary>
	    /// <returns>The number of times the resource has changed state.</returns>
        public uint getStateCount()
        {
            return Resource_getStateCount(resource);
        }

	    /// <summary>
	    /// Generic parameter setting method.
	    /// 
        /// Call this method with the name of a parameter and a string version of the value to set. 
	    /// The implementor will convert the string to a native type internally. If in doubt, check the 
	    /// parameter definition in the list returned from StringInterface::getParameters. 
	    /// </summary>
	    /// <param name="name">The name of the parameter to set.</param>
	    /// <param name="value">String value. Must be in the right format for the type specified in the parameter definition. See the StringConverter class for more information.</param>
        public void setParameter(String name, String value)
        {
            Resource_setParameter(resource, name, value);
        }

	    /// <summary>
	    /// Generic parameter retrieval method.
	    /// 
        /// Call this method with the name of a parameter to retrieve a string-format value of the 
	    /// parameter in question. If in doubt, check the parameter definition in the list returned 
	    /// from getParameters for the type of this parameter. If you like you can use StringConverter 
	    /// to convert this string back into a native type. 
	    /// </summary>
	    /// <param name="name">The name of the parameter to get.</param>
	    /// <returns>The parameter specified by name.</returns>
        public String getParameter(String name)
        {
            return Resource_getParameter(resource, name);
        }

#region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Resource_getName(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern uint Resource_getHandle(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Resource_getGroup(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Resource_prepare(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Resource_load(IntPtr resource, bool backgroundThread);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Resource_reload(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Resource_isReloadable(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Resource_isManuallyLoaded(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Resource_unload(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern uint Resource_getSize(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Resource_touch(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Resource_isPrepared(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Resource_isLoaded(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Resource_isLoading(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Resource.LoadingState Resource_getLoadingState(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Resource_isBackgroundLoaded(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Resource_setBackgroundLoaded(IntPtr resource, bool bl);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Resource_escalateLoading(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Resource_getOrigin(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern uint Resource_getStateCount(IntPtr resource);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Resource_setParameter(IntPtr resource, String name, String value);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern String Resource_getParameter(IntPtr resource, String name);

#endregion
    }
}
