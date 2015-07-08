using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    class Image : IDisposable
    {
        private IntPtr ptr;

        public Image()
        {
            ptr = Image_create();
        }

        public void Dispose()
        {
            Image_delete(ptr);
        }

        public void flipAroundY()
        {
            Image_flipAroundY(ptr);
        }

        public void flipAroundX()
        {
            Image_flipAroundX(ptr);
        }

        public void load(String filename, String groupName)
        {
            Image_load(ptr, filename, groupName);
        }

        /// <summary>
        /// Get a pixelbox for this image, you must dipose the returned pixel box.
        /// </summary>
        /// <returns>A pixel box the caller takes ownership of.</returns>
        public PixelBox getPixelBox(uint face = 0, uint mipmap = 0)
        {
            return new PixelBox(Image_getPixelBox(ptr, new UIntPtr(face), new UIntPtr(mipmap)));
        }

        public UInt64 Size
        {
            get
            {
                return Image_getSize(ptr).ToUInt64();
            }
        }

        public byte NumMipmaps
        {
            get
            {
                return Image_getNumMipmaps(ptr);
            }
        }

        public UInt32 Width
        {
            get
            {
                return Image_getWidth(ptr);
            }
        }

        public UInt32 Height
        {
            get
            {
                return Image_getHeight(ptr);
            }
        }

        public UInt32 Depth
        {
            get
            {
                return Image_getDepth(ptr);
            }
        }

        public UIntPtr NumFaces
        {
            get
            {
                return Image_getNumFaces(ptr);
            }
        }

        public UIntPtr RowSpan
        {
            get
            {
                return Image_getRowSpan(ptr);
            }
        }

        public PixelFormat Format
        {
            get
            {
                return Image_getFormat(ptr);
            }
        }

        public byte BPP
        {
            get
            {
                return Image_getBPP(ptr);
            }
        }

        public bool HasAlpha
        {
            get
            {
                return Image_getHasAlpha(ptr);
            }
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Image_create();

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Image_delete(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Image_flipAroundY(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Image_flipAroundX(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Image_load(IntPtr image, String filename, String groupName);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr Image_getSize(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern byte Image_getNumMipmaps(IntPtr image);

        //[DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.I1)]
        //private static extern bool Image_hasFlag(IntPtr image, ImageFlags imgFlag);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 Image_getWidth(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 Image_getHeight(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 Image_getDepth(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr Image_getNumFaces(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr Image_getRowSpan(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern PixelFormat Image_getFormat(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern byte Image_getBPP(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Image_getHasAlpha(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Image_getPixelBox(IntPtr image, UIntPtr face, UIntPtr mipmap);

        #endregion
    }
}
