using Engine;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.Plugin.VirtualTexture
{
    public class TiledImageSizeStrategy : ImagePageSizeStrategy
    {
        public void rescaleImage(FreeImageBitmap image, Size size, FREE_IMAGE_FILTER filter)
        {
            image.Rescale(size, filter);
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

                            //Pad by repeating
                            using (PixelBox altSrcBox = image.createPixelBox())
                            {
                                pageBox.Top = 0;
                                pageBox.Bottom = (uint)padding;
                                pageBox.Left = (uint)padding;
                                pageBox.Right = (uint)(fullPageSize.Width - padding);

                                altSrcBox.Top = (uint)imageRect.Top;
                                altSrcBox.Bottom = (uint)(imageRect.Top + padding);
                                altSrcBox.Left = (uint)imageRect.Left;
                                altSrcBox.Right = (uint)(imageRect.Left + pageSize);

                                PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                            }

                            using (PixelBox altSrcBox = image.createPixelBox())
                            {
                                pageBox.Top = (uint)(fullPageSize.Height - padding);
                                pageBox.Bottom = (uint)fullPageSize.Height;
                                pageBox.Left = (uint)padding;
                                pageBox.Right = (uint)(fullPageSize.Width - padding);

                                altSrcBox.Top = (uint)(imageRect.Bottom - padding);
                                altSrcBox.Bottom = (uint)imageRect.Bottom;
                                altSrcBox.Left = (uint)imageRect.Left;
                                altSrcBox.Right = (uint)(imageRect.Left + pageSize);

                                PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                            }

                            using (PixelBox altSrcBox = image.createPixelBox())
                            {
                                pageBox.Top = 1;
                                pageBox.Bottom = (uint)(fullPageSize.Height - padding);
                                pageBox.Left = 0;
                                pageBox.Right = (uint)padding;

                                altSrcBox.Top = (uint)imageRect.Top;
                                altSrcBox.Bottom = (uint)(imageRect.Top + pageSize);
                                altSrcBox.Left = (uint)imageRect.Left;
                                altSrcBox.Right = (uint)(imageRect.Left + padding);

                                PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                            }

                            using (PixelBox altSrcBox = image.createPixelBox())
                            {
                                pageBox.Top = 1;
                                pageBox.Bottom = (uint)(fullPageSize.Height - padding);
                                pageBox.Left = (uint)(fullPageSize.Width - padding);
                                pageBox.Right = (uint)fullPageSize.Width;

                                altSrcBox.Top = (uint)imageRect.Top;
                                altSrcBox.Bottom = (uint)(imageRect.Top + pageSize);
                                altSrcBox.Left = (uint)(imageRect.Right - padding);
                                altSrcBox.Right = (uint)imageRect.Right;

                                PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                            }
                        }
                        //int startPos = (int)stream.Position;
                        pages[x].RotateFlip(RotateFlipType.RotateNoneFlipY); //Have to flip the page over for ogre to be happy
                        pages[x].Save(memoryStreams[x], outputFormat, saveFlags);
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
    }
}
