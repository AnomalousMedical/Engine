using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using OgrePlugin;
using System.IO;
using Logging;
using FreeImageAPI;

namespace MyGUIPlugin
{
    public enum ImageResizeMode
    {
        Both,
        KeepAspect,
        Off
    }

    public class ImageAtlas : IDisposable
    {
        private String name;
        private IntSize2 imageSize;
        private MemoryArchive memoryArchive;
        private Dictionary<Object, String> guidDictionary = new Dictionary<Object, String>();

        public ImageAtlas(String name, IntSize2 imageSize)
        {
            this.name = name;
            this.imageSize = imageSize;
            MyGUIInterface.Instance.CommonResourceGroup.addResource(name, "Memory", true);
            memoryArchive = MemoryArchiveFactory.Instance.getArchive(name);
            ResizeMode = ImageResizeMode.Both;
        }

        public void Dispose()
        {
            clear();
            MyGUIInterface.Instance.CommonResourceGroup.removeResource(name);
        }

        public String addImage(Object resourceKey, FreeImageBitmap image)
        {
            IntSize2 size;
            return addImage(resourceKey, image, out size);
        }

        public String addImage(Object resourceKey, FreeImageBitmap image, out IntSize2 imageSize)
        {
            String guidStr = UniqueKeyGenerator.generateStringKey();
            guidDictionary.Add(resourceKey, guidStr);
            createImage(guidStr, image, out imageSize);
            return guidStr;
        }

        public void removeImage(Object resourceKey)
        {
            String guid = null;
            if(guidDictionary.TryGetValue(resourceKey, out guid))
            {
                deleteImage(guid);
                guidDictionary.Remove(resourceKey);
            }
        }

        public void replaceImage(Object resourceKey, FreeImageBitmap newImage)
        {
            IntSize2 imageSize;
            replaceImage(resourceKey, newImage, out imageSize);
        }

        public void replaceImage(Object resourceKey, FreeImageBitmap newImage, out IntSize2 imageSize)
        {
            String guid = null;
            if (guidDictionary.TryGetValue(resourceKey, out guid))
            {
                deleteImage(guid);
                createImage(guid, newImage, out imageSize);
            }
            else
            {
                imageSize = new IntSize2();
            }
        }

        public bool containsImage(Object resourceKey)
        {
            return guidDictionary.ContainsKey(resourceKey);
        }

        public String getImageId(Object resourceKey)
        {
            String guid = null;
            guidDictionary.TryGetValue(resourceKey, out guid);
            return guid;
        }

        public void clear()
        {
            foreach (String guid in guidDictionary.Values)
            {
                deleteImage(guid);
            }
            guidDictionary.Clear();
        }

        public IntSize2 ImageSize
        {
            get
            {
                return imageSize;
            }
            set
            {
                imageSize = value;
            }
        }

        public ImageResizeMode ResizeMode { get; set; }

        private void deleteImage(String guid)
        {
            ResourceManager.Instance.removeByName(guid);
            memoryArchive.destroyMemoryStreamResource(guid + ".png");
            memoryArchive.destroyMemoryStreamResource(guid + ".xml");
            MyGUIInterface.Instance.OgrePlatform.getRenderManager().destroyTexture(name + guid + ".png");
        }

        private void createImage(String guidStr, FreeImageBitmap image, out IntSize2 finalSize)
        {
            FreeImageBitmap resizedImage = null;
            try
            {
                //resize the image if it does not match
                Size addImageSize = image.Size;
                if (addImageSize.Width != imageSize.Width || addImageSize.Height != imageSize.Height)
                {
                    Rectangle destRect;
                    switch (ResizeMode)
                    {
                        case ImageResizeMode.KeepAspect:
                            int width = 8;
                            float aspect = (float)addImageSize.Height / addImageSize.Width;
                            int height = (int)((float)imageSize.Width * aspect);
                            if (height < imageSize.Height)
                            {
                                width = imageSize.Width;
                            }
                            else
                            {
                                aspect = (float)image.Width / image.Height;
                                height = imageSize.Height;
                                width = (int)((float)imageSize.Height * aspect);
                            }
                            destRect = new Rectangle(0, 0, width, height);
                            break;
                        default:
                            destRect = new Rectangle(0, 0, imageSize.Width, imageSize.Height);
                            break;
                    }
                    resizedImage = new FreeImageBitmap(image);
                    resizedImage.Rescale(destRect.Width, destRect.Height, FREE_IMAGE_FILTER.FILTER_BILINEAR);
                    image = resizedImage;
                }

                MemoryStream imageStream = null;
                try
                {
                    imageStream = new MemoryStream();
                    image.Save(imageStream, FREE_IMAGE_FORMAT.FIF_PNG);
                    memoryArchive.addMemoryStreamResource(guidStr + ".png", imageStream);

                    String xmlString = String.Format(resourceXML, guidStr, name + guidStr + ".png", imageSize.Width, imageSize.Height);
                    memoryArchive.addMemoryStreamResource(guidStr + ".xml", new MemoryStream(ASCIIEncoding.UTF8.GetBytes(xmlString)));

                    ResourceManager.Instance.load(name + guidStr + ".xml");
                }
                catch (Exception ex)
                {
                    Logging.Log.Error("Exception saving image to atlas {0}", ex.Message);
                    if (imageStream != null)
                    {
                        imageStream.Dispose();
                    }
                }

                finalSize = new IntSize2(image.Width, image.Height);
            }
            finally
            {
                //Dispose the image if it was resized
                if (resizedImage != null)
                {
                    resizedImage.Dispose();
                }
            }
        }

        private const string resourceXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                                "<MyGUI type=\"Resource\" version=\"1.1\">\n" +
                                  "<Resource type=\"ResourceImageSet\" name=\"{0}\">\n" +
                                    "<Group name=\"Icons\" texture=\"{1}\" size=\"{2} {3}\">\n" +
                                      "<Index name=\"Icon\">\n" +
                                        "<Frame point=\"0 0\"/>\n" +
                                      "</Index>\n" +
                                    "</Group>\n" +
                                  "</Resource>\n" +
                                "</MyGUI>";
    }
}