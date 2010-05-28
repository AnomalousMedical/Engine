using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgreWrapper
{
    public enum PixelFormat
{
    /// Unknown pixel format.
    PF_UNKNOWN = 0,
    /// 8-bit pixel format, all bits luminace.
    PF_L8 = 1,
	PF_BYTE_L = PF_L8,
    /// 16-bit pixel format, all bits luminace.
    PF_L16 = 2,
	PF_SHORT_L = PF_L16,
    /// 8-bit pixel format, all bits alpha.
    PF_A8 = 3,
	PF_BYTE_A = PF_A8,
    /// 8-bit pixel format, 4 bits alpha, 4 bits luminance.
    PF_A4L4 = 4,
	/// 2 byte pixel format, 1 byte luminance, 1 byte alpha
	PF_BYTE_LA = 5,
    /// 16-bit pixel format, 5 bits red, 6 bits green, 5 bits blue.
    PF_R5G6B5 = 6,
	/// 16-bit pixel format, 5 bits red, 6 bits green, 5 bits blue.
    PF_B5G6R5 = 7,
    /// 8-bit pixel format, 2 bits blue, 3 bits green, 3 bits red.
    PF_R3G3B2 = 31,
    /// 16-bit pixel format, 4 bits for alpha, red, green and blue.
    PF_A4R4G4B4 = 8,
    /// 16-bit pixel format, 5 bits for blue, green, red and 1 for alpha.
    PF_A1R5G5B5 = 9,
    /// 24-bit pixel format, 8 bits for red, green and blue.
    PF_R8G8B8 = 10,
    /// 24-bit pixel format, 8 bits for blue, green and red.
    PF_B8G8R8 = 11,
    /// 32-bit pixel format, 8 bits for alpha, red, green and blue.
    PF_A8R8G8B8 = 12,
    /// 32-bit pixel format, 8 bits for blue, green, red and alpha.
    PF_A8B8G8R8 = 13,
    /// 32-bit pixel format, 8 bits for blue, green, red and alpha.
    PF_B8G8R8A8 = 14,
	/// 32-bit pixel format, 8 bits for red, green, blue and alpha.
	PF_R8G8B8A8 = 28,
    /// 32-bit pixel format, 8 bits for red, 8 bits for green, 8 bits for blue
    /// like PF_A8R8G8B8, but alpha will get discarded
    PF_X8R8G8B8 = 26,
    /// 32-bit pixel format, 8 bits for blue, 8 bits for green, 8 bits for red
    /// like PF_A8B8G8R8, but alpha will get discarded
    PF_X8B8G8R8 = 27,
//#if OGRE_ENDIAN == OGRE_ENDIAN_BIG
//    /// 3 byte pixel format, 1 byte for red, 1 byte for green, 1 byte for blue
//    PF_BYTE_RGB = PF_R8G8B8,
//    /// 3 byte pixel format, 1 byte for blue, 1 byte for green, 1 byte for red
//    PF_BYTE_BGR = PF_B8G8R8,
//    /// 4 byte pixel format, 1 byte for blue, 1 byte for green, 1 byte for red and one byte for alpha
//    PF_BYTE_BGRA = PF_B8G8R8A8,
//    /// 4 byte pixel format, 1 byte for red, 1 byte for green, 1 byte for blue, and one byte for alpha
//    PF_BYTE_RGBA = PF_R8G8B8A8,
//#else
	/// 3 byte pixel format, 1 byte for red, 1 byte for green, 1 byte for blue
	PF_BYTE_RGB = PF_B8G8R8,
	/// 3 byte pixel format, 1 byte for blue, 1 byte for green, 1 byte for red
	PF_BYTE_BGR = PF_R8G8B8,
	/// 4 byte pixel format, 1 byte for blue, 1 byte for green, 1 byte for red and one byte for alpha
	PF_BYTE_BGRA = PF_A8R8G8B8,
	/// 4 byte pixel format, 1 byte for red, 1 byte for green, 1 byte for blue, and one byte for alpha
	PF_BYTE_RGBA = PF_A8B8G8R8,
//#endif        
    /// 32-bit pixel format, 2 bits for alpha, 10 bits for red, green and blue.
    PF_A2R10G10B10 = 15,
    /// 32-bit pixel format, 10 bits for blue, green and red, 2 bits for alpha.
    PF_A2B10G10R10 = 16,
    /// DDS (DirectDraw Surface) DXT1 format
    PF_DXT1 = 17,
    /// DDS (DirectDraw Surface) DXT2 format
    PF_DXT2 = 18,
    /// DDS (DirectDraw Surface) DXT3 format
    PF_DXT3 = 19,
    /// DDS (DirectDraw Surface) DXT4 format
    PF_DXT4 = 20,
    /// DDS (DirectDraw Surface) DXT5 format
    PF_DXT5 = 21,
	// 16-bit pixel format, 16 bits (float) for red
    PF_FLOAT16_R = 32,
    // 48-bit pixel format, 16 bits (float) for red, 16 bits (float) for green, 16 bits (float) for blue
    PF_FLOAT16_RGB = 22,
    // 64-bit pixel format, 16 bits (float) for red, 16 bits (float) for green, 16 bits (float) for blue, 16 bits (float) for alpha
    PF_FLOAT16_RGBA = 23,
	// 16-bit pixel format, 16 bits (float) for red
    PF_FLOAT32_R = 33,
   // 96-bit pixel format, 32 bits (float) for red, 32 bits (float) for green, 32 bits (float) for blue
    PF_FLOAT32_RGB = 24,
    // 128-bit pixel format, 32 bits (float) for red, 32 bits (float) for green, 32 bits (float) for blue, 32 bits (float) for alpha
    PF_FLOAT32_RGBA = 25,
	// 32-bit, 2-channel s10e5 floating point pixel format, 16-bit green, 16-bit red
	PF_FLOAT16_GR = 35,
	// 64-bit, 2-channel floating point pixel format, 32-bit green, 32-bit red
	PF_FLOAT32_GR = 36,
	// Depth texture format
	PF_DEPTH = 29,
	// 64-bit pixel format, 16 bits for red, green, blue and alpha
	PF_SHORT_RGBA = 30,
	// 32-bit pixel format, 16-bit green, 16-bit red
	PF_SHORT_GR = 34,
	// 48-bit pixel format, 16 bits for red, green and blue
	PF_SHORT_RGB = 37,
	// Number of pixel formats currently defined
    PF_COUNT = 38
};

    [NativeSubsystemType]
    public unsafe class PixelBox : IDisposable
    {
        private IntPtr pixelBox;

        /// <summary>
        /// Constructor
        /// </summary>
        public PixelBox()
        {
            this.pixelBox = PixelBox_Create();
        }

        /// <summary>
        /// Constructor providing extents in the form of a Box object.
        /// <para>
        /// This constructor assumes the pixel data is laid out consecutively in
        /// memory. (this means row after row, slice after slice, with no space in
        /// between) 
        /// </para>
        /// </summary>
        /// <param name="left">Left side of the box.</param>
        /// <param name="top">Top of the box.</param>
        /// <param name="right">Right side of the box.</param>
        /// <param name="bottom">Bottom of the box.</param>
        /// <param name="pixelFormat">The pixel format to use.</param>
        PixelBox(int left, int top, int right, int bottom, PixelFormat pixelFormat)
        {
            this.pixelBox = PixelBox_Create1(left, top, right, bottom, pixelFormat);
        }

        /// <summary>
        /// Constructor providing extents in the form of a Box object.
        /// <para>
        /// This constructor assumes the pixel data is laid out consecutively in
        /// memory. (this means row after row, slice after slice, with no space in
        /// between) 
        /// </para>
        /// </summary>
        /// <param name="left">Left side of the box.</param>
        /// <param name="top">Top of the box.</param>
        /// <param name="right">Right side of the box.</param>
        /// <param name="bottom">Bottom of the box.</param>
        /// <param name="pixelFormat">The pixel format to use.</param>
        /// <param name="pixelData">A pointer to the buffer data.  Make sure this is pinned if it is managed.</param>
        PixelBox(int left, int top, int right, int bottom, PixelFormat pixelFormat, void* pixelData)
        {
            this.pixelBox = PixelBox_Create2(left, top, right, bottom, pixelFormat, pixelData);
        }

        /// <summary>
        /// Constructor providing width, height and depth.
        /// <para>
        /// This constructor assumes the pixel data is laid out consecutively in
        /// memory. (this means row after row, slice after slice, with no space in
        /// between) 
        /// </para>
        /// </summary>
        /// <param name="width">Width of the region.</param>
        /// <param name="height">Height of the region.</param>
        /// <param name="depth">Depth of the region.</param>
        /// <param name="pixelFormat">Format of this buffer.</param>
        PixelBox(int width, int height, int depth, PixelFormat pixelFormat)
        {
            this.pixelBox = PixelBox_Create3(width, height, depth, pixelFormat);
        }

        /// <summary>
        /// Constructor providing width, height and depth.
        /// <para>
        /// This constructor assumes the pixel data is laid out consecutively in
        /// memory. (this means row after row, slice after slice, with no space in
        /// between) 
        /// </para>
        /// </summary>
        /// <param name="width">Width of the region.</param>
        /// <param name="height">Height of the region.</param>
        /// <param name="depth">Depth of the region.</param>
        /// <param name="pixelFormat">Format of this buffer.</param>
        /// <param name="pixelData">A pointer to the buffer data.  Make sure this is pinned if it is managed.</param>
        PixelBox(int width, int height, int depth, PixelFormat pixelFormat, void* pixelData)
        {
            this.pixelBox = PixelBox_Create4(width, height, depth, pixelFormat, pixelData);
        }

        public void Dispose()
        {
            PixelBox_Delete(pixelBox);
            pixelBox = IntPtr.Zero;
        }

        internal IntPtr OgreBox
        {
            get
            {
                return pixelBox;
            }
        }

        /// <summary>
	    /// Set the rowPitch and slicePitch so that the buffer is laid out consecutive in memory. 
	    /// </summary>
	    void setConsecutive()
        {
            PixelBox_setConsecutive(pixelBox);
        }

	    /// <summary>
	    /// Get the number of elements between one past the rightmost pixel of one
        /// row and the leftmost pixel of the next row. 
	    /// </summary>
	    /// <returns>The row skip or 0 if rows are consecutive.</returns>
        int getRowSkip()
        {
            return PixelBox_getRowSkip(pixelBox);
        }

	    /// <summary>
	    /// Get the number of elements between one past the right bottom pixel of
        /// one slice and the left top pixel of the next slice. 
	    /// </summary>
	    /// <returns>The number of pixels or 0 if the slices are consecutive.</returns>
        int getSliceSkip()
        {
            return PixelBox_getSliceSkip(pixelBox);
        }

	    /// <summary>
	    /// Return whether this buffer is laid out consecutive in memory (ie the
        /// pitches are equal to the dimensions).
	    /// </summary>
	    /// <returns>True if consecutive.</returns>
        bool isConsecutive()
        {
            return PixelBox_isConsecutive(pixelBox);
        }

	    /// <summary>
	    /// Return the size (in bytes) this image would take if it was laid out
        /// consecutive in memory.
	    /// </summary>
	    /// <returns>The size.</returns>
        int getConsecutiveSize()
        {
            return PixelBox_getConsecutiveSize(pixelBox);
        }

	    /// <summary>
	    /// Get the width of this box. 
	    /// </summary>
	    /// <returns>The width.</returns>
        int getWidth()
        {
            return PixelBox_getWidth(pixelBox);
        }

	    /// <summary>
	    /// Get the height of this box. 
	    /// </summary>
	    /// <returns>The height.</returns>
        int getHeight()
        {
            return PixelBox_getHeight(pixelBox);
        }

	    /// <summary>
	    /// Get the depth of this box. 
	    /// </summary>
	    /// <returns>The depth.</returns>
        int getDepth()
        {
            return PixelBox_getDepth(pixelBox);
        }

	    /// <summary>
	    /// The data pointer.
	    /// </summary>
	    void* Data 
	    {
		    get
            {
                return PixelBox_getData(pixelBox);
            }
            set
            {
                PixelBox_setData(pixelBox, value);
            }
	    }

	    /// <summary>
	    /// The pixel format.
	    /// </summary>
	    PixelFormat Format 
	    {
		    get
            {
                return PixelBox_getFormat(pixelBox);
            }
            set
            {
                PixelBox_setFormat(pixelBox, value);
            }
	    }

	    /// <summary>
	    /// Number of elements between the leftmost pixel of one row and the left
        /// pixel of the next. 
	    /// </summary>
	    int RowPitch 
	    {
            get
            {
                return PixelBox_getRowPitch(pixelBox);
            }
            set
            {
                PixelBox_setRowPitch(pixelBox, value);
            }
	    }

	    /// <summary>
	    /// Number of elements between the top left pixel of one (depth) slice and
        /// the top left pixel of the next. 
	    /// </summary>
	    int SlicePitch 
	    {
            get
            {
                return PixelBox_getSlicePitch(pixelBox);
            }
            set
            {
                PixelBox_setSlicePitch(pixelBox, value);
            }
	    }

        #region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern IntPtr PixelBox_Create();

        [DllImport("OgreCWrapper")]
        private static extern IntPtr PixelBox_Create1(int left, int top, int right, int bottom, PixelFormat pixelFormat);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr PixelBox_Create2(int left, int top, int right, int bottom, PixelFormat pixelFormat, void* pixelData);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr PixelBox_Create3(int width, int height, int depth, PixelFormat pixelFormat);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr PixelBox_Create4(int width, int height, int depth, PixelFormat pixelFormat, void* pixelData);

        [DllImport("OgreCWrapper")]
        private static extern void PixelBox_Delete(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern void PixelBox_setConsecutive(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern int PixelBox_getRowSkip(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern int PixelBox_getSliceSkip(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool PixelBox_isConsecutive(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern int PixelBox_getConsecutiveSize(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern int PixelBox_getWidth(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern int PixelBox_getHeight(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern int PixelBox_getDepth(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern void* PixelBox_getData(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern void PixelBox_setData(IntPtr pixelBox, void* data);

        [DllImport("OgreCWrapper")]
        private static extern PixelFormat PixelBox_getFormat(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern void PixelBox_setFormat(IntPtr pixelBox, PixelFormat format);

        [DllImport("OgreCWrapper")]
        private static extern int PixelBox_getRowPitch(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern void PixelBox_setRowPitch(IntPtr pixelBox, int rowPitch);

        [DllImport("OgreCWrapper")]
        private static extern int PixelBox_getSlicePitch(IntPtr pixelBox);

        [DllImport("OgreCWrapper")]
        private static extern void PixelBox_setSlicePitch(IntPtr pixelBox, int slicePitch);

        #endregion
    }
}
