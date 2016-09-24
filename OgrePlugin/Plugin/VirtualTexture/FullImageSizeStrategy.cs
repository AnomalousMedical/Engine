using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using FreeImageAPI;

namespace OgrePlugin.Plugin.VirtualTexture
{
    public class FullImageSizeStrategy : ImagePageSizeStrategy
    {
        public void rescaleImage(FreeImageBitmap image, Size size, FREE_IMAGE_FILTER filter)
        {
            image.Rescale(size, filter);
        }

        public void extractPage(FreeImageBitmap image, int padding, Stream stream, PagedImage pagedImage, int pageSize, IntSize2 fullPageSize, int size, FREE_IMAGE_FORMAT outputFormat, FREE_IMAGE_SAVE_FLAGS saveFlags)
        {
            bool topSide, bottomSide;
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

                //Calculate pages, note that there is overlap by padding between them this is intentional
                for (int y = 0; y < size; ++y)
                {
                    serialImageRect.Height = fullPageSize.Height;
                    serialImageRect.Top = y * pageSize - padding;
                    topSide = serialImageRect.Top < 0;
                    if (topSide)
                    {
                        serialImageRect.Top = 0;
                        serialImageRect.Height -= padding;
                    }

                    bottomSide = serialImageRect.Bottom > image.Height;
                    if (bottomSide)
                    {
                        if (topSide)
                        {
                            //Take entire image
                            serialImageRect.Top = 0;
                            serialImageRect.Height = image.Height;
                        }
                        else
                        {
                            //Extra row on top, bottom flush with texture bottom will add extra pixel row on bottom will add extra pixel row on bottom below
                            serialImageRect.Top = image.Height - pageSize - padding;
                            serialImageRect.Height -= padding;
                        }
                    }

                    Parallel.For(0, size, x =>
                    //for (int x = 0; x < size; ++x)
                    {
                        bool leftSide, rightSide;
                        IntRect imageRect = serialImageRect;

                        imageRect.Width = fullPageSize.Width;
                        imageRect.Left = x * pageSize - padding;
                        leftSide = imageRect.Left < 0;
                        if (leftSide)
                        {
                            imageRect.Left = 0;
                            imageRect.Width -= padding;
                        }

                        rightSide = imageRect.Right > image.Width;
                        if (rightSide)
                        {
                            if (leftSide)
                            {
                                //Take entire image
                                imageRect.Left = 0;
                                imageRect.Width = image.Width;
                            }
                            else
                            {
                                //Extra row on left, right flush with texture right will add extra pixel row on right below
                                imageRect.Left = image.Width - pageSize - padding;
                                imageRect.Width -= padding;
                            }
                        }

                        using (var pageBox = pages[x].createPixelBox(PixelFormat.PF_A8R8G8B8))
                        {
                            if (topSide)
                            {
                                pageBox.Top += (uint)padding;
                            }

                            if (bottomSide)
                            {
                                pageBox.Bottom -= (uint)padding;
                            }

                            if (leftSide)
                            {
                                pageBox.Left += (uint)padding;
                            }

                            if (rightSide)
                            {
                                pageBox.Right -= (uint)padding;
                            }

                            using (var imageBox = image.createPixelBox())
                            {
                                imageBox.Left = (uint)imageRect.Left;
                                imageBox.Right = (uint)imageRect.Right;
                                imageBox.Top = (uint)imageRect.Top;
                                imageBox.Bottom = (uint)imageRect.Bottom;

                                PixelBox.BulkPixelConversion(imageBox, pageBox);
                            }

                            if (topSide)
                            {
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
                            }

                            if (bottomSide)
                            {
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
                            }

                            if (leftSide)
                            {
                                using (PixelBox altSrcBox = image.createPixelBox())
                                {
                                    pageBox.Top = (uint)padding;
                                    pageBox.Bottom = (uint)(fullPageSize.Height - padding);
                                    pageBox.Left = 0;
                                    pageBox.Right = (uint)padding;

                                    altSrcBox.Top = (uint)imageRect.Top;
                                    altSrcBox.Bottom = (uint)(imageRect.Top + pageSize);
                                    altSrcBox.Left = (uint)imageRect.Left;
                                    altSrcBox.Right = (uint)(imageRect.Left + padding);

                                    PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                                }
                            }

                            if (rightSide)
                            {
                                using (PixelBox altSrcBox = image.createPixelBox())
                                {
                                    pageBox.Top = (uint)padding;
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
