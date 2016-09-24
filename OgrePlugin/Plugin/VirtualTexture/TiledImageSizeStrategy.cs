using Engine;
using FreeImageAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.Plugin.VirtualTexture
{
    public class TiledImageSizeStrategy : ImagePageSizeStrategy, IDisposable
    {
        public class Tile : IDisposable
        {
            FreeImageBitmap image;

            public Tile()
            {

            }

            public Tile(FreeImageBitmap image)
            {
                this.image = image;
            }

            public void Dispose()
            {
                image.Dispose();
            }

            public float NodeLeft { get; set; }
            public float NodeTop { get; set; }
            public float NodeRight { get; set; }
            public float NodeBottom { get; set; }

            public float Width { get { return NodeRight - NodeLeft; } }
            public float Height { get { return NodeBottom - NodeTop; } }

            [JsonIgnore]
            public FreeImageBitmap Image
            {
                get
                {
                    return image;
                }
                set
                {
                    image = value;
                }
            }
        }

        private List<Tile> tiles;
        private bool imageNeedsBreakup = true;

        public TiledImageSizeStrategy(List<TiledImageSizeStrategy.Tile> tiles)
        {
            this.tiles = tiles;
        }

        public void Dispose()
        {
            foreach (var tile in tiles)
            {
                tile.Dispose();
            }
            tiles.Clear();
        }

        int imageId = 0;

        public void rescaleImage(FreeImageBitmap image, Size size, FREE_IMAGE_FILTER filter)
        {
            breakUpImage(image);

            //float wPercent = size.Width / (float)firstSeenImageSize.Width;
            //float hPercent = size.Height / (float)firstSeenImageSize.Height;
            //var newTileSize = new Size((int)(wPercent * originalTileSize.Width), (int)(hPercent * originalTileSize.Height));

            image.Rescale(size, filter); //Need to resize the image, but will replace with individually sized tiles
            //image.saveToFile(imageId + "orig.bmp", FREE_IMAGE_FORMAT.FIF_BMP);

            using (var destBox = image.createPixelBox())
            {
                foreach (var tile in tiles)
                {
                    var newTileSize = new Size((int)(size.Width * tile.Width), (int)(size.Height * tile.Height));
                    tile.Image.Rescale(newTileSize, filter);
                    using (var srcBox = tile.Image.createPixelBox(PixelFormat.PF_A8R8G8B8))
                    {
                        var tx = size.Width * tile.NodeRight;
                        var ty = size.Height * tile.NodeLeft;
                        destBox.Left = (uint)tx;
                        destBox.Top = (uint)ty;
                        destBox.Right = (uint)(tx + newTileSize.Width);
                        destBox.Bottom = (uint)(ty + newTileSize.Height);

                        PixelBox.BulkPixelConversion(srcBox, destBox);
                    }
                }
            }

            image.saveToFile(imageId++ + "tiled.bmp", FREE_IMAGE_FORMAT.FIF_BMP);
        }

        private void breakUpImage(FreeImageBitmap image)
        {
            if (imageNeedsBreakup)
            {
                imageNeedsBreakup = false;

                foreach (var tile in tiles)
                {
                    var tileImage = new FreeImageBitmap(
                        (int)(image.Width * tile.Width), 
                        (int)(image.Height * tile.Height), 
                        FreeImageAPI.PixelFormat.Format32bppArgb);

                    using (var destBox = tileImage.createPixelBox(PixelFormat.PF_A8R8G8B8))
                    {
                        using (var sourceBox = image.createPixelBox())
                        {
                            sourceBox.Left = (uint)(image.Width * tile.NodeLeft);
                            sourceBox.Top = (uint)(image.Height * tile.NodeTop);
                            sourceBox.Right = (uint)(sourceBox.Left + tileImage.Width);
                            sourceBox.Bottom = (uint)(sourceBox.Top + tileImage.Height);

                            PixelBox.BulkPixelConversion(sourceBox, destBox);
                        }
                    }

                    //tileImage.saveToFile($"tilesdebug/{tile.NodeLeft}_{tile.NodeTop}.bmp", FREE_IMAGE_FORMAT.FIF_BMP);

                    tile.Image = tileImage;
                }
            }
        }

        public void extractPage(FreeImageBitmap image, int padding, Stream stream, PagedImage pagedImage, int pageSize, IntSize2 fullPageSize, int size, FREE_IMAGE_FORMAT outputFormat, FREE_IMAGE_SAVE_FLAGS saveFlags)
        {
            IntRect serialImageRect = new IntRect();

            MemoryStream[] memoryStreams = new MemoryStream[size];
            FreeImageBitmap[] pages = new FreeImageBitmap[size];
            try
            {
                for (int i = 0; i < size; ++i)
                {
                    memoryStreams[i] = new MemoryStream();
                    pages[i] = new FreeImageBitmap(fullPageSize.Width, fullPageSize.Height, FreeImageAPI.PixelFormat.Format32bppArgb);
                }

                //Calculate pages as tiles, always repeat outer limits
                for (int y = 0; y < size; ++y)
                {
                    serialImageRect.Height = pageSize;
                    serialImageRect.Top = y * pageSize;

                    Parallel.For(0, size, x =>
                    //for (int x = 0; x < size; ++x)
                    {
                        IntRect imageRect = serialImageRect;

                        imageRect.Width = pageSize;
                        imageRect.Left = x * pageSize;

                        using (var pageBox = pages[x].createPixelBox(PixelFormat.PF_A8R8G8B8))
                        {
                            pageBox.Top += (uint)padding;
                            pageBox.Bottom -= (uint)padding;
                            pageBox.Left += (uint)padding;
                            pageBox.Right -= (uint)padding;

                            using (var imageBox = image.createPixelBox())
                            {
                                imageBox.Left = (uint)imageRect.Left;
                                imageBox.Right = (uint)imageRect.Right;
                                imageBox.Top = (uint)imageRect.Top;
                                imageBox.Bottom = (uint)imageRect.Bottom;

                                PixelBox.BulkPixelConversion(imageBox, pageBox);
                            }

                            padFillColor(image, padding, pageSize, fullPageSize, imageRect, pageBox);
                        }
                        //int startPos = (int)stream.Position;
                        pages[x].RotateFlip(RotateFlipType.RotateNoneFlipY); //Have to flip the page over for ogre to be happy
                        pages[x].Save(memoryStreams[x], outputFormat, saveFlags);
                        //pages[x].saveToFile($"page/page{image.Width}_x{x}y_{y}.bmp", FREE_IMAGE_FORMAT.FIF_BMP);
                        memoryStreams[x].Position = 0;
                        //++pagedImage.numImages;
                        //pagedImage.pages.Add(new ImageInfo(startPos, (int)(stream.Position)));
                    });
                    //}

                    for (int x = 0; x < size; ++x)
                    {
                        int startPos = (int)stream.Position;
                        //page.RotateFlip(RotateFlipType.RotateNoneFlipY); //Have to flip the page over for ogre to be happy
                        //page.Save(stream, outputFormat, saveFlags);
                        memoryStreams[x].CopyTo(stream);
                        ++pagedImage.NumImages;
                        pagedImage.Pages.Add(new PagedImage.ImageInfo(startPos, (int)(stream.Position)));
                        memoryStreams[x].Dispose();
                        memoryStreams[x] = new MemoryStream();
                    }
                }
            }
            finally
            {
                for (int i = 0; i < size; ++i)
                {
                    memoryStreams[i].Dispose();
                    pages[i].Dispose();
                }
            }
        }

        /// <summary>
        /// This one seems to work the best, but falls apart at lower mip levels on the virtual texture.
        /// </summary>
        private static void padFillColor(FreeImageBitmap image, int padding, int pageSize, IntSize2 fullPageSize, IntRect imageRect, PixelBox pageBox)
        {
            //Pad by repeating
            //Top
            using (PixelBox altSrcBox = image.createPixelBox())
            {
                altSrcBox.Top = (uint)imageRect.Top;
                altSrcBox.Bottom = (uint)(imageRect.Top + 1);
                altSrcBox.Left = (uint)imageRect.Left;
                altSrcBox.Right = (uint)(imageRect.Left + pageSize);

                for (int i = 0; i < padding; ++i)
                {
                    pageBox.Top = (uint)i;
                    pageBox.Bottom = (uint)(i + 1);
                    pageBox.Left = (uint)padding;
                    pageBox.Right = (uint)(fullPageSize.Width - padding);

                    PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                }
            }

            //Bottom
            using (PixelBox altSrcBox = image.createPixelBox())
            {
                altSrcBox.Top = (uint)(imageRect.Bottom - 1);
                altSrcBox.Bottom = (uint)imageRect.Bottom;
                altSrcBox.Left = (uint)imageRect.Left;
                altSrcBox.Right = (uint)(imageRect.Left + pageSize);

                for (int i = 0; i < padding; ++i)
                {
                    pageBox.Top = (uint)(fullPageSize.Height - i - 1);
                    pageBox.Bottom = (uint)pageBox.Top + 1;
                    pageBox.Left = (uint)padding;
                    pageBox.Right = (uint)(fullPageSize.Width - padding);

                    PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                }
            }

            //Left
            using (PixelBox altSrcBox = image.createPixelBox())
            {
                altSrcBox.Top = (uint)imageRect.Top;
                altSrcBox.Bottom = (uint)(imageRect.Top + pageSize);
                altSrcBox.Left = (uint)imageRect.Left;
                altSrcBox.Right = (uint)(imageRect.Left + 1);

                for (int i = 0; i < padding; ++i)
                {
                    pageBox.Top = (uint)padding;
                    pageBox.Bottom = (uint)(fullPageSize.Height - padding);
                    pageBox.Left = (uint)i;
                    pageBox.Right = (uint)i + 1;

                    PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                }
            }

            //Right
            using (PixelBox altSrcBox = image.createPixelBox())
            {
                altSrcBox.Top = (uint)imageRect.Top;
                altSrcBox.Bottom = (uint)(imageRect.Top + pageSize);
                altSrcBox.Left = (uint)(imageRect.Right - 1);
                altSrcBox.Right = (uint)imageRect.Right;

                for (int i = 0; i < padding; ++i)
                {
                    pageBox.Top = (uint)padding;
                    pageBox.Bottom = (uint)(fullPageSize.Height - padding);
                    pageBox.Left = (uint)(fullPageSize.Width - i - 1);
                    pageBox.Right = (uint)(pageBox.Left + 1);

                    PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                }
            }
        }

        /// <summary>
        /// Works ok but not all tiles repeat like this.
        /// </summary>
        private static void padRepeatOtherSide(FreeImageBitmap image, int padding, int pageSize, IntSize2 fullPageSize, IntRect imageRect, PixelBox pageBox)
        {
            //Pad by repeating
            //Top
            using (PixelBox altSrcBox = image.createPixelBox())
            {
                pageBox.Top = 0;
                pageBox.Bottom = (uint)padding;
                pageBox.Left = (uint)padding;
                pageBox.Right = (uint)(fullPageSize.Width - padding);

                altSrcBox.Top = (uint)(imageRect.Bottom - padding);
                altSrcBox.Bottom = (uint)imageRect.Bottom;
                altSrcBox.Left = (uint)imageRect.Left;
                altSrcBox.Right = (uint)(imageRect.Left + pageSize);

                PixelBox.BulkPixelConversion(altSrcBox, pageBox);
            }

            //Bottom
            using (PixelBox altSrcBox = image.createPixelBox())
            {
                pageBox.Top = (uint)(fullPageSize.Height - padding);
                pageBox.Bottom = (uint)fullPageSize.Height;
                pageBox.Left = (uint)padding;
                pageBox.Right = (uint)(fullPageSize.Width - padding);

                altSrcBox.Top = (uint)imageRect.Top;
                altSrcBox.Bottom = (uint)(imageRect.Top + padding);
                altSrcBox.Left = (uint)imageRect.Left;
                altSrcBox.Right = (uint)(imageRect.Left + pageSize);

                PixelBox.BulkPixelConversion(altSrcBox, pageBox);
            }

            //Left
            using (PixelBox altSrcBox = image.createPixelBox())
            {
                pageBox.Top = (uint)padding;
                pageBox.Bottom = (uint)(fullPageSize.Height - padding);
                pageBox.Left = 0;
                pageBox.Right = (uint)padding;

                altSrcBox.Top = (uint)imageRect.Top;
                altSrcBox.Bottom = (uint)(imageRect.Top + pageSize);
                altSrcBox.Left = (uint)(imageRect.Right - padding);
                altSrcBox.Right = (uint)imageRect.Right;

                PixelBox.BulkPixelConversion(altSrcBox, pageBox);
            }

            //Right
            using (PixelBox altSrcBox = image.createPixelBox())
            {
                pageBox.Top = (uint)padding;
                pageBox.Bottom = (uint)(fullPageSize.Height - padding);
                pageBox.Left = (uint)(fullPageSize.Width - padding);
                pageBox.Right = (uint)fullPageSize.Width;

                altSrcBox.Top = (uint)imageRect.Top;
                altSrcBox.Bottom = (uint)(imageRect.Top + pageSize);
                altSrcBox.Left = (uint)imageRect.Left;
                altSrcBox.Right = (uint)(imageRect.Left + padding);

                PixelBox.BulkPixelConversion(altSrcBox, pageBox);
            }
        }
    }
}
