﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;
using Engine;

namespace OgrePlugin
{
    public enum PixelFormat
{
        /// Unknown pixel format.
        PF_UNKNOWN = 0,
        /// 8-bit pixel format, all bits luminance.
        PF_L8 = 1,
        PF_BYTE_L = PF_L8,
        /// 16-bit pixel format, all bits luminance.
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
        /// 16-bit pixel format, 16 bits (float) for red
        PF_FLOAT16_R = 32,
        /// 48-bit pixel format, 16 bits (float) for red, 16 bits (float) for green, 16 bits (float) for blue
        PF_FLOAT16_RGB = 22,
        /// 64-bit pixel format, 16 bits (float) for red, 16 bits (float) for green, 16 bits (float) for blue, 16 bits (float) for alpha
        PF_FLOAT16_RGBA = 23,
        // 32-bit pixel format, 32 bits (float) for red
        PF_FLOAT32_R = 33,
        /// 96-bit pixel format, 32 bits (float) for red, 32 bits (float) for green, 32 bits (float) for blue
        PF_FLOAT32_RGB = 24,
        /// 128-bit pixel format, 32 bits (float) for red, 32 bits (float) for green, 32 bits (float) for blue, 32 bits (float) for alpha
        PF_FLOAT32_RGBA = 25,
        /// 32-bit, 2-channel s10e5 floating point pixel format, 16-bit green, 16-bit red
        PF_FLOAT16_GR = 35,
        /// 64-bit, 2-channel floating point pixel format, 32-bit green, 32-bit red
        PF_FLOAT32_GR = 36,
        /// Depth texture format
        PF_DEPTH = 29,
        /// 64-bit pixel format, 16 bits for red, green, blue and alpha
        PF_SHORT_RGBA = 30,
        /// 32-bit pixel format, 16-bit green, 16-bit red
        PF_SHORT_GR = 34,
        /// 48-bit pixel format, 16 bits for red, green and blue
        PF_SHORT_RGB = 37,
        /// PVRTC (PowerVR) RGB 2 bpp
        PF_PVRTC_RGB2 = 38,
        /// PVRTC (PowerVR) RGBA 2 bpp
        PF_PVRTC_RGBA2 = 39,
        /// PVRTC (PowerVR) RGB 4 bpp
        PF_PVRTC_RGB4 = 40,
        /// PVRTC (PowerVR) RGBA 4 bpp
        PF_PVRTC_RGBA4 = 41,
        /// PVRTC (PowerVR) Version 2, 2 bpp
        PF_PVRTC2_2BPP = 42,
        /// PVRTC (PowerVR) Version 2, 4 bpp
        PF_PVRTC2_4BPP = 43,
        /// 32-bit pixel format, 11 bits (float) for red, 11 bits (float) for green, 10 bits (float) for blue
        PF_R11G11B10_FLOAT = 44,
        /// 8-bit pixel format, 8 bits red (unsigned int).
        PF_R8_UINT = 45,
        /// 16-bit pixel format, 8 bits red (unsigned int), 8 bits blue (unsigned int).
        PF_R8G8_UINT = 46,
        /// 24-bit pixel format, 8 bits red (unsigned int), 8 bits blue (unsigned int), 8 bits green (unsigned int).
        PF_R8G8B8_UINT = 47,
        /// 32-bit pixel format, 8 bits red (unsigned int), 8 bits blue (unsigned int), 8 bits green (unsigned int), 8 bits alpha (unsigned int).
        PF_R8G8B8A8_UINT = 48,
        /// 16-bit pixel format, 16 bits red (unsigned int).
        PF_R16_UINT = 49,
        /// 32-bit pixel format, 16 bits red (unsigned int), 16 bits blue (unsigned int).
        PF_R16G16_UINT = 50,
        /// 48-bit pixel format, 16 bits red (unsigned int), 16 bits blue (unsigned int), 16 bits green (unsigned int).
        PF_R16G16B16_UINT = 51,
        /// 64-bit pixel format, 16 bits red (unsigned int), 16 bits blue (unsigned int), 16 bits green (unsigned int), 16 bits alpha (unsigned int).
        PF_R16G16B16A16_UINT = 52,
        /// 32-bit pixel format, 32 bits red (unsigned int).
        PF_R32_UINT = 53,
        /// 64-bit pixel format, 32 bits red (unsigned int), 32 bits blue (unsigned int).
        PF_R32G32_UINT = 54,
        /// 96-bit pixel format, 32 bits red (unsigned int), 32 bits blue (unsigned int), 32 bits green (unsigned int).
        PF_R32G32B32_UINT = 55,
        /// 128-bit pixel format, 32 bits red (unsigned int), 32 bits blue (unsigned int), 32 bits green (unsigned int), 32 bits alpha (unsigned int).
        PF_R32G32B32A32_UINT = 56,
        /// 8-bit pixel format, 8 bits red (signed int).
        PF_R8_SINT = 57,
        /// 16-bit pixel format, 8 bits red (signed int), 8 bits blue (signed int).
        PF_R8G8_SINT = 58,
        /// 24-bit pixel format, 8 bits red (signed int), 8 bits blue (signed int), 8 bits green (signed int).
        PF_R8G8B8_SINT = 59,
        /// 32-bit pixel format, 8 bits red (signed int), 8 bits blue (signed int), 8 bits green (signed int), 8 bits alpha (signed int).
        PF_R8G8B8A8_SINT = 60,
        /// 16-bit pixel format, 16 bits red (signed int).
        PF_R16_SINT = 61,
        /// 32-bit pixel format, 16 bits red (signed int), 16 bits blue (signed int).
        PF_R16G16_SINT = 62,
        /// 48-bit pixel format, 16 bits red (signed int), 16 bits blue (signed int), 16 bits green (signed int).
        PF_R16G16B16_SINT = 63,
        /// 64-bit pixel format, 16 bits red (signed int), 16 bits blue (signed int), 16 bits green (signed int), 16 bits alpha (signed int).
        PF_R16G16B16A16_SINT = 64,
        /// 32-bit pixel format, 32 bits red (signed int).
        PF_R32_SINT = 65,
        /// 64-bit pixel format, 32 bits red (signed int), 32 bits blue (signed int).
        PF_R32G32_SINT = 66,
        /// 96-bit pixel format, 32 bits red (signed int), 32 bits blue (signed int), 32 bits green (signed int).
        PF_R32G32B32_SINT = 67,
        /// 128-bit pixel format, 32 bits red (signed int), 32 bits blue (signed int), 32 bits green (signed int), 32 bits alpha (signed int).
        PF_R32G32B32A32_SINT = 68,
        /// 32-bit pixel format, 9 bits for blue, green, red plus a 5 bit exponent.
        PF_R9G9B9E5_SHAREDEXP = 69,
        /// DDS (DirectDraw Surface) BC4 format (unsigned normalised)
        PF_BC4_UNORM = 70,
        /// DDS (DirectDraw Surface) BC4 format (signed normalised)
        PF_BC4_SNORM = 71,
        /// DDS (DirectDraw Surface) BC5 format (unsigned normalised)
        PF_BC5_UNORM = 72,
        /// DDS (DirectDraw Surface) BC5 format (signed normalised)
        PF_BC5_SNORM = 73,
        /// DDS (DirectDraw Surface) BC6H format (unsigned 16 bit float)
        PF_BC6H_UF16 = 74,
        /// DDS (DirectDraw Surface) BC6H format (signed 16 bit float)
        PF_BC6H_SF16 = 75,
        /// DDS (DirectDraw Surface) BC7 format (unsigned normalised)
        PF_BC7_UNORM = 76,
        /// DDS (DirectDraw Surface) BC7 format (unsigned normalised sRGB)
        PF_BC7_UNORM_SRGB = 77,
        /// 8-bit pixel format, all bits red.
        PF_R8 = 78,
        /// 16-bit pixel format, 8 bits red, 8 bits green.
        PF_RG8 = 79,
        /// 8-bit pixel format, 8 bits red (signed normalised int).
        PF_R8_SNORM = 80,
        /// 16-bit pixel format, 8 bits red (signed normalised int), 8 bits blue (signed normalised int).
        PF_R8G8_SNORM = 81,
        /// 24-bit pixel format, 8 bits red (signed normalised int), 8 bits blue (signed normalised int), 8 bits green (signed normalised int).
        PF_R8G8B8_SNORM = 82,
        /// 32-bit pixel format, 8 bits red (signed normalised int), 8 bits blue (signed normalised int), 8 bits green (signed normalised int), 8 bits alpha (signed normalised int).
        PF_R8G8B8A8_SNORM = 83,
        /// 16-bit pixel format, 16 bits red (signed normalised int).
        PF_R16_SNORM = 84,
        /// 32-bit pixel format, 16 bits red (signed normalised int), 16 bits blue (signed normalised int).
        PF_R16G16_SNORM = 85,
        /// 48-bit pixel format, 16 bits red (signed normalised int), 16 bits blue (signed normalised int), 16 bits green (signed normalised int).
        PF_R16G16B16_SNORM = 86,
        /// 64-bit pixel format, 16 bits red (signed normalised int), 16 bits blue (signed normalised int), 16 bits green (signed normalised int), 16 bits alpha (signed normalised int).
        PF_R16G16B16A16_SNORM = 87,
        /// ETC1 (Ericsson Texture Compression)
        PF_ETC1_RGB8 = 88,
        /// ETC2 (Ericsson Texture Compression)
        PF_ETC2_RGB8 = 89,
        /// ETC2 (Ericsson Texture Compression)
        PF_ETC2_RGBA8 = 90,
        /// ETC2 (Ericsson Texture Compression)
        PF_ETC2_RGB8A1 = 91,
        /// ATC (AMD_compressed_ATC_texture)
        PF_ATC_RGB = 92,
        /// ATC (AMD_compressed_ATC_texture)
        PF_ATC_RGBA_EXPLICIT_ALPHA = 93,
        /// ATC (AMD_compressed_ATC_texture)
        PF_ATC_RGBA_INTERPOLATED_ALPHA = 94,
        // Number of pixel formats currently defined
        PF_COUNT = 95
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
        public PixelBox(int left, int top, int right, int bottom, PixelFormat pixelFormat)
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
        public PixelBox(int left, int top, int right, int bottom, PixelFormat pixelFormat, void* pixelData)
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
        public PixelBox(int width, int height, int depth, PixelFormat pixelFormat)
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
        public PixelBox(int width, int height, int depth, PixelFormat pixelFormat, void* pixelData)
        {
            this.pixelBox = PixelBox_Create4(width, height, depth, pixelFormat, pixelData);
        }

        /// <summary>
        /// Constructor, uses existing pointer. Should only really use if the user should delete the box. Putting a pointer
        /// into a wrapper instance like this implies that it should take ownership of the pointer.
        /// </summary>
        /// <param name="pixelBox">The pointer to the existing pixel box.</param>
        internal PixelBox(IntPtr pixelBox)
        {
            this.pixelBox = pixelBox;
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
        public void setConsecutive()
        {
            PixelBox_setConsecutive(pixelBox);
        }

	    /// <summary>
	    /// Get the number of elements between one past the rightmost pixel of one
        /// row and the leftmost pixel of the next row. 
	    /// </summary>
	    /// <returns>The row skip or 0 if rows are consecutive.</returns>
        public int getRowSkip()
        {
            return PixelBox_getRowSkip(pixelBox);
        }

	    /// <summary>
	    /// Get the number of elements between one past the right bottom pixel of
        /// one slice and the left top pixel of the next slice. 
	    /// </summary>
	    /// <returns>The number of pixels or 0 if the slices are consecutive.</returns>
        public int getSliceSkip()
        {
            return PixelBox_getSliceSkip(pixelBox);
        }

	    /// <summary>
	    /// Return whether this buffer is laid out consecutive in memory (ie the
        /// pitches are equal to the dimensions).
	    /// </summary>
	    /// <returns>True if consecutive.</returns>
        public bool isConsecutive()
        {
            return PixelBox_isConsecutive(pixelBox);
        }

	    /// <summary>
	    /// Return the size (in bytes) this image would take if it was laid out
        /// consecutive in memory.
	    /// </summary>
	    /// <returns>The size.</returns>
        public int getConsecutiveSize()
        {
            return PixelBox_getConsecutiveSize(pixelBox);
        }

	    /// <summary>
	    /// Get the width of this box. 
	    /// </summary>
	    /// <returns>The width.</returns>
        public int getWidth()
        {
            return PixelBox_getWidth(pixelBox);
        }

	    /// <summary>
	    /// Get the height of this box. 
	    /// </summary>
	    /// <returns>The height.</returns>
        public int getHeight()
        {
            return PixelBox_getHeight(pixelBox);
        }

	    /// <summary>
	    /// Get the depth of this box. 
	    /// </summary>
	    /// <returns>The depth.</returns>
        public int getDepth()
        {
            return PixelBox_getDepth(pixelBox);
        }

	    /// <summary>
	    /// The data pointer.
	    /// </summary>
        public void* Data 
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
        public PixelFormat Format 
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
        public int RowPitch 
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
        public int SlicePitch 
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

        public uint Left
        {
            get
            {
                return PixelBox_getLeft(pixelBox);
            }
            set
            {
                PixelBox_setLeft(pixelBox, value);
            }
        }

        public uint Top
        {
            get
            {
                return PixelBox_getTop(pixelBox);
            }
            set
            {
                PixelBox_setTop(pixelBox, value);
            }
        }

        public uint Right
        {
            get
            {
                return PixelBox_getRight(pixelBox);
            }
            set
            {
                PixelBox_setRight(pixelBox, value);
            }
        }

        public uint Bottom
        {
            get
            {
                return PixelBox_getBottom(pixelBox);
            }
            set
            {
                PixelBox_setBottom(pixelBox, value);
            }
        }

        public IntRect Rect
        {
            get
            {
                return new IntRect((int)Left, (int)Top, (int)getWidth(), (int)getHeight());
            }
            set
            {
                Left = (uint)value.Left;
                Right = (uint)value.Right;
                Top = (uint)value.Top;
                Bottom = (uint)value.Bottom;
            }
        }

        public static void BulkPixelConversion(PixelBox src, PixelBox dst)
        {
            PixelUtil_bulkPixelConversion(src.OgreBox, dst.OgreBox);
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr PixelBox_Create();

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr PixelBox_Create1(int left, int top, int right, int bottom, PixelFormat pixelFormat);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr PixelBox_Create2(int left, int top, int right, int bottom, PixelFormat pixelFormat, void* pixelData);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr PixelBox_Create3(int width, int height, int depth, PixelFormat pixelFormat);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr PixelBox_Create4(int width, int height, int depth, PixelFormat pixelFormat, void* pixelData);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PixelBox_Delete(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PixelBox_setConsecutive(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern int PixelBox_getRowSkip(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern int PixelBox_getSliceSkip(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool PixelBox_isConsecutive(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern int PixelBox_getConsecutiveSize(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern int PixelBox_getWidth(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern int PixelBox_getHeight(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern int PixelBox_getDepth(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void* PixelBox_getData(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PixelBox_setData(IntPtr pixelBox, void* data);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern PixelFormat PixelBox_getFormat(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PixelBox_setFormat(IntPtr pixelBox, PixelFormat format);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern int PixelBox_getRowPitch(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PixelBox_setRowPitch(IntPtr pixelBox, int rowPitch);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern int PixelBox_getSlicePitch(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PixelBox_setSlicePitch(IntPtr pixelBox, int slicePitch);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern UInt32 PixelBox_getLeft(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PixelBox_setLeft(IntPtr pixelBox, UInt32 value);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern UInt32 PixelBox_getTop(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PixelBox_setTop(IntPtr pixelBox, UInt32 value);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern UInt32 PixelBox_getRight(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void PixelBox_setRight(IntPtr pixelBox, UInt32 value);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern UInt32 PixelBox_getBottom(IntPtr pixelBox);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void PixelBox_setBottom(IntPtr pixelBox, UInt32 value);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void PixelUtil_bulkPixelConversion(IntPtr src, IntPtr dst);

        #endregion
    }
}
