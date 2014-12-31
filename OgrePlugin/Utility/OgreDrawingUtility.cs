#if !FIXLATER_DISABLED
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgrePlugin
{
    public static class OgreDrawingUtility
    {
        public static PixelFormat getOgreFormat(FreeImageAPI.PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                //
                // Summary:
                //     Specifies that the format is 16 bits per pixel; 5 bits are used for the red
                //     component, 6 bits are used for the green component, and 5 bits are used for
                //     the blue component.
                case FreeImageAPI.PixelFormat.Format16bppRgb565:
                    return PixelFormat.PF_R5G6B5;
                //
                // Summary:
                //     Specifies that the format is 24 bits per pixel; 8 bits each are used for
                //     the red, green, and blue components.
                case FreeImageAPI.PixelFormat.Format24bppRgb:
                    return PixelFormat.PF_R8G8B8;
                //
                // Summary:
                //     Specifies that the format is 32 bits per pixel; 8 bits each are used for
                //     the red, green, and blue components. The remaining 8 bits are not used.
                case FreeImageAPI.PixelFormat.Format32bppRgb:
                    return PixelFormat.PF_R8G8B8;
                //
                // Summary:
                //     The pixel format is 16 bits per pixel. The color information specifies 32,768
                //     shades of color, of which 5 bits are red, 5 bits are green, 5 bits are blue,
                //     and 1 bit is alpha.
                case FreeImageAPI.PixelFormat.Format16bppArgb1555:
                    return PixelFormat.PF_A1R5G5B5;
                //
                // Summary:
                //     Specifies that the format is 32 bits per pixel; 8 bits each are used for
                //     the alpha, red, green, and blue components.
                case FreeImageAPI.PixelFormat.Format32bppArgb:
                    return PixelFormat.PF_A8R8G8B8;
                    
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
#endif