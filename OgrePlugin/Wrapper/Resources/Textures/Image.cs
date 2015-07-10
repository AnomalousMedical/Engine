using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public class Image : IDisposable
    {
        public enum Filter
        {
            FILTER_NEAREST,
            FILTER_LINEAR,
            FILTER_BILINEAR,
            FILTER_BOX,
            FILTER_TRIANGLE,
            FILTER_BICUBIC
        };

        private IntPtr ptr;

        public Image()
        {
            ptr = Image_create();
        }

        public Image(uint width, uint height, uint depth, PixelFormat format, uint numFaces, uint numMipMaps)
        {
            ptr = Image_create1(width, height, depth, format, new UIntPtr(numFaces), new UIntPtr(numMipMaps));
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

        /// <summary>
        /// Load an image from a stream. The stream will be controlled by ogre which will close it when 
        /// it is done, however, you should still be able to follow normal using patterns on the stream 
        /// you pass in.
        /// </summary>
        public void load(Stream stream, String type)
        {
            OgreManagedStream managedStream = new OgreManagedStream("Image", stream);
            Image_load(ptr, managedStream.NativeStream, type);
        }

        /// <summary>
        /// Get a pixelbox for this image, you must dipose the returned pixel box.
        /// </summary>
        /// <returns>A pixel box the caller takes ownership of.</returns>
        public PixelBox getPixelBox(uint face = 0, uint mipmap = 0)
        {
            return new PixelBox(Image_getPixelBox(ptr, new UIntPtr(face), new UIntPtr(mipmap)));
        }

        public void resize(uint width, uint height, Filter filter)
        {
            Image_resize(ptr, (ushort)width, (ushort)height, filter);
        }

        public static void Scale(PixelBox src, PixelBox dst, Filter filter = Filter.FILTER_BILINEAR)
        {
            Image_scale(src.OgreBox, dst.OgreBox, filter);
        }

        public static ulong CalculateSize(uint mipmaps, uint faces, uint width, uint height, uint depth, PixelFormat format)
        {
            return Image_calculateSize(new UIntPtr(mipmaps), new UIntPtr(faces), width, height, depth, format).ToUInt64();
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

        public uint NumFaces
        {
            get
            {
                return Image_getNumFaces(ptr).ToUInt32();
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
        private static extern IntPtr Image_create1(uint uWidth, uint uHeight, uint depth, PixelFormat eFormat, UIntPtr numFaces, UIntPtr numMipMaps);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Image_delete(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Image_flipAroundY(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Image_flipAroundX(IntPtr image);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Image_load(IntPtr image, IntPtr stream, String type);

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

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Image_resize(IntPtr image, ushort width, ushort height, Filter filter);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Image_scale(IntPtr src, IntPtr dst, Filter filter);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr Image_calculateSize(UIntPtr mipmaps, UIntPtr faces, uint width, uint height, uint depth, PixelFormat format);

        #endregion
    }
}
