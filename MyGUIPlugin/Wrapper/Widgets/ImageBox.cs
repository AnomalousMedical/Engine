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

#region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setItemResource(IntPtr staticImage, String value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setItemGroup(IntPtr staticImage, String value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setItemName(IntPtr staticImage, String value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setImageTexture(IntPtr staticImage, String value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setImageCoord(IntPtr staticImage, ref IntCoord coord);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ImageBox_setImageTile(IntPtr staticImage, ref IntSize2 _value);

#endregion
    }
}
