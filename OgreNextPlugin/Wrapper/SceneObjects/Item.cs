using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OgreNextPlugin
{
    [NativeSubsystemType]
    public class Item : MovableObject
    {
        internal static Item createWrapper(IntPtr item, object[] args)
        {
            return new Item(item);
        }

        private Item(IntPtr item)
            : base(item)
        {

        }

        //private static extern Ogre::SubItem* Item_getSubItem(Ogre::Item* item, size_t index);

        public int NumSubItems => Item_getNumSubItems(ogreObject).ToInt32();

        //public void Item_setDatablock(Ogre::Item* item, Ogre::HlmsDatablock* datablock);

        public void SetDatablock(String datablockName)
        {
            Item_setDatablockName(ogreObject, datablockName);
        }

        public void SetDatablockOrMaterialName(String name, String groupName = OgreResourceGroupManager.AutodetectResourceGroup)
        {
            Item_setDatablockOrMaterialName(OgreObject, name, groupName);
        }

        public void SetMaterialName(String name, String groupName = OgreResourceGroupManager.AutodetectResourceGroup)
        {
            Item_setMaterialName(ogreObject, name, groupName);
        }

        public bool HasSkeleton => Item_hasSkeleton(ogreObject);

        public void UseSkeletonInstanceFrom(Item master)
        {
            Item_useSkeletonInstanceFrom(ogreObject, master.ogreObject);
        }

        public void StopUsingSkeletonInstanceFromMaster()
        {
            Item_stopUsingSkeletonInstanceFromMaster(ogreObject);
        }

        public bool SharesSkeletonInstance => Item_sharesSkeletonInstance(ogreObject);

        //public bool HasVertexAnimation => Item_hasVertexAnimation(ogreObject);

        public bool IsInitialized => Item_isInitialised(ogreObject);

        #region PInvoke
        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Item_getSubItem(IntPtr item, IntPtr index);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Item_getNumSubItems(IntPtr item);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Item_setDatablock(IntPtr item, IntPtr datablock);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Item_setDatablockName(IntPtr item, String datablockName);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Item_clone(IntPtr item, String newName);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Item_setDatablockOrMaterialName(IntPtr item, String name, String groupName);//= Ogre::ResourceGroupManager::AUTODETECT_RESOURCE_GROUP_NAME);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Item_setMaterialName(IntPtr item, String name, String groupName);// = ResourceGroupManager::AUTODETECT_RESOURCE_GROUP_NAME);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Item_hasSkeleton(IntPtr item);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Item_useSkeletonInstanceFrom(IntPtr item, IntPtr master);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Item_stopUsingSkeletonInstanceFromMaster(IntPtr item);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Item_sharesSkeletonInstance(IntPtr item);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Item_hasVertexAnimation(IntPtr item);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Item_isInitialised(IntPtr item);

        #endregion
    }
}
