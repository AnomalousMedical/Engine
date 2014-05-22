using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace MyGUIPlugin
{
    public class ImageBox: Widget
    {
        internal ImageBox(IntPtr staticImage)
            : base(staticImage)
        {

        }

        public void setItemResource(String value)
        {
            if (value == null)
            {
                value = "";
            }
            ImageBox_setItemResource(widget, value);
        }

        public void setItemGroup(String value)
        {
            ImageBox_setItemGroup(widget, value);
        }

        public void setItemName(String value)
        {
            ImageBox_setItemName(widget, value);
        }

        public void setImageInfo(String texture, IntCoord coord, IntSize2 tile)
        {
            ImageBox_setImageInfo(WidgetPtr, texture, ref coord, ref tile);
        }

        public void setImageTexture(String value)
        {
            ImageBox_setImageTexture(WidgetPtr, value);
        }

        public void setImageCoord(IntCoord intCoord)
        {
            ImageBox_setImageCoord(WidgetPtr, ref intCoord);
        }

        public void setImageTile(IntSize2 value)
        {
            ImageBox_setImageTile(WidgetPtr, ref value);
        }

        public void deleteAllItems()
        {
            ImageBox_deleteAllItems(WidgetPtr);
        }

        /// <summary>
        /// Get the size of the entire texture used for this image box. If you want the size of just the
        /// current resource use ItemGroupSize.
        /// </summary>
        public IntSize2 ImageSize
        {
            get
            {
                return ImageBox_getImageSize(WidgetPtr).toIntSize2();
            }
        }

        /// <summary>
        /// Return the size of the single item set to this ImageBox.
        /// </summary>
        public IntSize2 ItemGroupSize
        {
            get
            {
                return ImageBox_getItemGroupSize(WidgetPtr).toIntSize2();
            }
        }

#region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setItemResource(IntPtr staticImage, String value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern ThreeIntHack ImageBox_getImageSize(IntPtr staticImage);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern ThreeIntHack ImageBox_getItemGroupSize(IntPtr staticImage);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setItemGroup(IntPtr staticImage, String value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setItemName(IntPtr staticImage, String value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ImageBox_setImageInfo(IntPtr staticImage, String _texture, ref IntCoord _coord, ref IntSize2 _tile);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setImageTexture(IntPtr staticImage, String value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setImageCoord(IntPtr staticImage, ref IntCoord coord);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setImageTile(IntPtr staticImage, ref IntSize2 _value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ImageBox_deleteAllItems(IntPtr staticImage);

#endregion
    }
}
