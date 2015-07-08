using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utility
{
    public static class ImageUtility
    {
        public enum ImageFormat
        {
            Invalid = 0,
            Jpeg,
            Gif,
            Png,
            Bmp,
        }

        private static string[] imageHeaders = new[]{
                Encoding.ASCII.GetString(new byte[]{255,216,255,224}), //JPEG
                "GIF",      // GIF
                Encoding.ASCII.GetString(new byte[]{137, 80, 78, 71}), // PNG
                "BM",       // BMP
        };

        private static ImageFormat GetFormat(byte[] header)
        {
            String headerString = Encoding.ASCII.GetString(header);
            ImageFormat format = ImageFormat.Invalid;
            for (int i = 0; i < imageHeaders.Length; ++i)
            {
                if (headerString.StartsWith(imageHeaders[i]))
                {
                    format = (ImageFormat)(i + 1);
                }
            }
            return format;
        }

        /// <summary>
        /// Get the info for an image without loading the entire image stream.
        /// </summary>
        /// <param name="imageStream">The stream, should be seekable.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>The ImageFormat for the image stream.</returns>
        public static ImageFormat GetImageInfo(Stream imageStream, out int width, out int height)
        {
            byte[] header = new byte[4];
            imageStream.Read(header, 0, header.Length);
            ImageFormat format = GetFormat(header);
            imageStream.Seek(0, SeekOrigin.Begin);
            switch (format)
            {
                case ImageFormat.Jpeg:
                    int iWidth, iHeight;
                    GetJpegInfo(imageStream, out iWidth, out iHeight);
                    width = iWidth;
                    height = iHeight;
                    break;
                case ImageFormat.Gif:
                    String type, version;
                    GetGifInfo(imageStream, out type, out version, out iWidth, out iHeight);
                    width = iWidth;
                    height = iHeight;
                    break;
                case ImageFormat.Png:
                    long lWidth, lheight;
                    GetPngInfo(imageStream, out lWidth, out lheight);
                    if (lWidth < int.MaxValue)
                    {
                        width = (int)lWidth;
                    }
                    else
                    {
                        width = int.MaxValue;
                    }
                    if (lheight < int.MaxValue)
                    {
                        height = (int)lheight;
                    }
                    else
                    {
                        height = int.MaxValue;
                    }
                    break;
                case ImageFormat.Bmp:
                    GetBmpInfo(imageStream, out iWidth, out iHeight);
                    width = iWidth;
                    height = iHeight;
                    break;
                default:
                    width = -1;
                    height = -1;
                    break;
            }
            imageStream.Seek(0, SeekOrigin.Begin);
            return format;
        }

        //Image reading methods taken from stack overflow thread
        //http://stackoverflow.com/questions/111345/getting-image-dimensions-without-reading-the-entire-file

        static void GetGifInfo(Stream f, out String type, out String version, out int width, out int height)
        {
            type = ((char)f.ReadByte()).ToString();
            type += ((char)f.ReadByte()).ToString();
            type += ((char)f.ReadByte()).ToString();

            version = ((char)f.ReadByte()).ToString();
            version += ((char)f.ReadByte()).ToString();
            version += ((char)f.ReadByte()).ToString();

            int lower = f.ReadByte();
            int upper = f.ReadByte();
            width = lower | upper << 8;

            lower = f.ReadByte();
            upper = f.ReadByte();
            height = lower | upper << 8;
        }

        static void GetPngInfo(Stream f, out long width, out long height)
        {
            byte[] size = new byte[24];

            f.Read(size, 0, 24);

            width = 0;

            width = width | size[16];
            width = width << 8;
            width = width | size[17];
            width = width << 8;
            width = width | size[18];
            width = width << 8;
            width = width | size[19];

            height = 0;

            height = height | size[20];
            height = height << 8;
            height = height | size[21];
            height = height << 8;
            height = height | size[22];
            height = height << 8;
            height = height | size[23];
        }

        static void GetBmpInfo(Stream stream, out int width, out int height)
        {
            BinaryReader reader = new BinaryReader(stream); //Note that this does not need to be closed or disposed because we don't want to close the stream passed in

            reader.BaseStream.Seek(0x12, SeekOrigin.Begin);
            width = reader.ReadInt32();
            height = reader.ReadInt32();
        }

        static bool GetJpegInfo(Stream stream, out int width, out int height)
        {
            height = width = 0;
            bool found = false;
            bool eof = false;

            BinaryReader reader = new BinaryReader(stream); //Note that this does not need to be closed or disposed because we don't want to close the stream passed in

            while (!found || eof)
            {

                // read 0xFF and the type
                reader.ReadByte();
                byte type = reader.ReadByte();

                // get length
                int len = 0;
                switch (type)
                {
                    // start and end of the image
                    case 0xD8:
                    case 0xD9:
                        len = 0;
                        break;

                    // restart interval
                    case 0xDD:
                        len = 2;
                        break;

                    // the next two bytes is the length
                    default:
                        int lenHi = reader.ReadByte();
                        int lenLo = reader.ReadByte();
                        len = (lenHi << 8 | lenLo) - 2;
                        break;
                }

                // EOF?
                if (type == 0xD9)
                    eof = true;

                // process the data
                if (len > 0)
                {

                    // read the data
                    byte[] data = reader.ReadBytes(len);

                    // this is what we are looking for
                    if (type == 0xC0)
                    {
                        height = data[1] << 8 | data[2];
                        width = data[3] << 8 | data[4];
                        found = true;
                    }

                }

            }

            return found;
        }
    }
}
