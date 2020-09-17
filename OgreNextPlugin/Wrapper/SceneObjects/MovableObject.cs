using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;
using Engine;

namespace OgreNextPlugin
{
    /// <summary>
    /// Enum of possible movement types.
    /// </summary>
    public enum MovableTypes : uint
    {
	    Entity,
	    Light,
	    Camera,
	    ManualObject,
	    BillboardChain,
	    RibbonTrail,
	    BillboardSet,
	    Frustrum,
	    BatchInstance,
	    MovablePlane,
	    ParticleSystem,
	    SimpleRenderable,
	    Other
    };

    [NativeSubsystemType]
    public abstract class MovableObject : IDisposable
    {
        protected IntPtr ogreObject;

        public MovableObject(IntPtr ogreObject)
        {
            this.ogreObject = ogreObject;
        }

        public virtual void Dispose()
        {
            ogreObject = IntPtr.Zero;
        }

        internal IntPtr OgreObject
        {
            get
            {
                return ogreObject;
            }
        }

        /// <summary>
	    /// Returns true if this object is attached to a SceneNode or TagPoint. 
	    /// </summary>
	    /// <returns>Returns true if this object is attached to a SceneNode or TagPoint.</returns>
	    public bool isAttached()
        {
            return MovableObject_isAttached(ogreObject);
        }

	    /// <summary>
	    /// Detaches an object from a parent SceneNode or TagPoint, if attached.
	    /// </summary>
	    public void detachFromParent()
        {
            MovableObject_detachFromParent(ogreObject);
        }

	    /// <summary>
	    /// Get the instance name of the render node object.
	    /// </summary>
	    /// <returns>The name of the render node object.</returns>
        public String getName()
        {
            return Marshal.PtrToStringAnsi(MovableObject_getName(ogreObject));
        }

	    /// <summary>
	    /// Set the object visibility.
	    /// </summary>
	    /// <param name="visible">True for visible false for invisible.</param>
	    public void setVisible(bool visible)
        {
            MovableObject_setVisible(ogreObject, visible);
        }

	    /// <summary>
	    /// Determines if the object is visible.
	    /// </summary>
	    /// <returns>True for visible false for invisible.</returns>
	    public bool isVisible()
        {
            return MovableObject_isVisible(ogreObject);
        }

	    /// <summary>
	    /// Sets the visiblity flags for this object. As well as a simple true/false value for 
	    /// visibility (as seen in setVisible), you can also set visiblity flags which when 'and'ed 
	    /// with the SceneManager's visibility mask can also make an object invisible.
	    /// </summary>
	    /// <param name="flags">The visibilty flags.</param>
	    public void setVisibilityFlags(uint flags)
        {
            MovableObject_setVisibilityFlags(ogreObject, flags);
        }

	    /// <summary>
	    /// As setVisibilityFlags, except the flags passed as parameters are appended to the 
	    /// existing flags on this object.
	    /// </summary>
	    /// <param name="flags">The visibilty flags.</param>
	    public void addVisiblityFlags(uint flags)
        {
            MovableObject_addVisiblityFlags(ogreObject, flags);
        }

	    /// <summary>
	    /// As setVisibilityFlags, except the flags passed as parameters are removed from the 
	    /// existing flags on this object.
	    /// </summary>
	    /// <param name="flags">The visibilty flags.</param>
	    public void removeVisibilityFlags(uint flags)
        {
            MovableObject_removeVisibilityFlags(ogreObject, flags);
        }

	    /// <summary>
	    /// Returns the visibility flags relevant for this object.
	    /// </summary>
	    /// <returns>The visibility flags.</returns>
	    public uint getVisibilityFlags()
        {
            return MovableObject_getVisibilityFlags(ogreObject);
        }

	    /// <summary>
	    /// Get the movable type of this object.
	    /// </summary>
	    /// <returns>The movable type of this object.</returns>
	    public MovableTypes getMovableType()
        {
            return MovableObject_getMovableType(ogreObject);
        }

	    /// <summary>
	    /// Get a string with the movable type of this object.  This is useful if you get
	    /// MovableTypes::Other as a result from getMovableType().  Ogre uses strings internally
	    /// but the enum is more reliable in general.
	    /// </summary>
	    /// <returns>The movable type of this object as a string.</returns>
	    public String getOgreMovableType()
        {
            return Marshal.PtrToStringAnsi(MovableObject_getOgreMovableType(ogreObject));
        }

	    /// <summary>
	    /// Sets the render queue group this entity will be rendered through. 
	    /// </summary>
	    /// <param name="queueID">The queue id to add this object to.</param>
	    public void setRenderQueueGroup(byte queueID)
        {
            MovableObject_setRenderQueueGroup(ogreObject, queueID);
        }

	    /// <summary>
	    /// Gets the queue group for this entity.
	    /// </summary>
	    /// <returns>The render queue group of this object.</returns>
	    public byte getRenderQueueGroup()
        {
            return MovableObject_getRenderQueueGroup(ogreObject);
        }

        #region CWrapper

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MovableObject_isAttached(IntPtr movableObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MovableObject_detachFromParent(IntPtr movableObject);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MovableObject_getName(IntPtr movableObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MovableObject_setVisible(IntPtr movableObject, bool visible);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MovableObject_isVisible(IntPtr movableObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MovableObject_setVisibilityFlags(IntPtr movableObject, uint flags);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MovableObject_addVisiblityFlags(IntPtr movableObject, uint flags);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MovableObject_removeVisibilityFlags(IntPtr movableObject, uint flags);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint MovableObject_getVisibilityFlags(IntPtr movableObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern MovableTypes MovableObject_getMovableType(IntPtr movableObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MovableObject_getOgreMovableType(IntPtr movableObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MovableObject_setRenderQueueGroup(IntPtr movableObject, byte queueID);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern byte MovableObject_getRenderQueueGroup(IntPtr movableObject);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MovableObject_getParentSceneNode(IntPtr movableObject);

        #endregion
    }
}
