using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public static class FreeImageBitmapExtensions
    {
        /// <summary>
        /// Create a pixel box for this FreeImageBitmap, note that ogre will populate the pixel box upside down, so you will need to flip the image
        /// afterward. Also you must Dispose the PixelBox returned by this function.
        /// </summary>
        /// /// <param name="bitmap">This object.</param>
        /// <returns>A PixelBox with the given format for the FreeImageBitmap.</returns>
        unsafe public static PixelBox createPixelBox(this FreeImageBitmap bitmap)
        {
            OgrePlugin.PixelFormat ogreFormat;
            switch(bitmap.PixelFormat)
            {
                case FreeImageAPI.PixelFormat.Format24bppRgb:
                    ogreFormat = PixelFormat.PF_R8G8B8;
                    break;
                case FreeImageAPI.PixelFormat.Format32bppArgb:
                    ogreFormat = PixelFormat.PF_A8R8G8B8;
                    break;
                default:
                    throw new ArgumentException(String.Format("Bitmap format {0} not supported. Must be Format24bppRgb or Format32bppArgb", bitmap.PixelFormat));
            }
            return FreeImageBitmapExtensions.createPixelBox(bitmap, ogreFormat);
        }

        /// <summary>
        /// Create a pixel box for this FreeImageBitmap, note that ogre will populate the pixel box upside down, so you will need to flip the image
        /// afterward. Also you must Dispose the PixelBox returned by this function.
        /// </summary>
        /// /// <param name="bitmap">This object.</param>
        /// <param name="format">The format of the pixel box.</param>
        /// <returns>A PixelBox with the given format for the FreeImageBitmap.</returns>
        unsafe public static PixelBox createPixelBox(this FreeImageBitmap bitmap, OgrePlugin.PixelFormat format)
        {
            return new PixelBox(0, 0, bitmap.Width, bitmap.Height, format, bitmap.GetScanlinePointer(0).ToPointer());
        }

        /// <summary>
        /// Copy the given RenderTarget with the specified format.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="renderTarget"></param>
        /// <param name="format"></param>
        public static void copyFromRenderTarget(this FreeImageBitmap bitmap, RenderTarget renderTarget, OgrePlugin.PixelFormat format)
        {
            using (PixelBox pixelBox = bitmap.createPixelBox(format))
            {
                renderTarget.copyContentsToMemory(pixelBox, RenderTarget.FrameBuffer.FB_AUTO);
            }
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        /// <summary>
        /// Save the image to a file. Simplifies creating a stream, good for debugging.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="fileName">The name of the file to save.</param>
        /// <param name="format">The format to save the file in.</param>
        public static void saveToFile(this FreeImageBitmap bitmap, String fileName, FREE_IMAGE_FORMAT format)
        {
            using (Stream test = File.Open(fileName, FileMode.Create))
            {
                bitmap.Save(test, format);
            }
        }
    }
}
