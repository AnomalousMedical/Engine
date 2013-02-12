using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using OgreWrapper;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Logging;

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
            OgreResourceGroupManager.getInstance().addResourceLocation(name, "Memory", "MyGUI", true);
            memoryArchive = MemoryArchiveFactory.Instance.getArchive(name);
            ResizeMode = ImageResizeMode.Both;
        }

        public void Dispose()
        {
            clear();
            OgreResourceGroupManager.getInstance().removeResourceLocation(name, "MyGUI");
        }

        public String addImage(Object resourceKey, Image image)
        {
            String guidStr = UniqueKeyGenerator.generateStringKey();
            guidDictionary.Add(resourceKey, guidStr);

            createImage(guidStr, image);
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

        public void replaceImage(Object resourceKey, Image newImage)
        {
            String guid = null;
            if (guidDictionary.TryGetValue(resourceKey, out guid))
            {
                deleteImage(guid);
                createImage(guid, newImage);
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

        private void createImage(String guidStr, Image image)
        {
            //resize the image if it does not match
            bool resizedImage = false;
            Size addImageSize = image.Size;
            if (addImageSize.Width != imageSize.Width || imageSize.Height != addImageSize.Height)
            {
                switch (ResizeMode)
                {
                    case ImageResizeMode.Both:

                        image = new Bitmap(image, new Size(imageSize.Width, imageSize.Height));
                        resizedImage = true;
                        break;
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
                        image = new Bitmap(image, new Size(width, height));
                        resizedImage = true;
                        break;
                }
            }

            MemoryStream imageStream = null;
            try
            {
                imageStream = new MemoryStream();
                image.Save(imageStream, ImageFormat.Png);
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

            //Dispose the image if it was resized
            if (resizedImage)
            {
                image.Dispose();
            }
        }

        private string resourceXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
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
/*
OgreResourceGroupManager.getInstance().addResourceLocation("GUI/PiperJBO/Layouts", "EngineArchive", "MyGUI", true);
            OgreResourceGroupManager.getInstance().addResourceLocation("GUI/PiperJBO/Imagesets", "EngineArchive", "MyGUI", true);
            OgreResourceGroupManager.getInstance().addResourceLocation("mem/", "Memory", "MyGUI", true);

            LanguageManager.Instance.loadUserTags("core_theme_black_orange_tag.xml");
            gui.load("core_skin.xml");
            //gui.load("LayersToolstrip.xml");
            gui.load("TeethButtons.xml");

            string xmlTest = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<MyGUI type=\"Resource\" version=\"1.1\">" + 
                                  "<Resource type=\"ResourceImageSet\" name=\"LayersToolstrip/Skin\">" +
                                    "<Group name=\"Icons\" texture=\"mem/MemoryImage.png\" size=\"32 32\">" + 
                                      "<Index name=\"Skin\">" + 
                                        "<Frame point=\"0 0\"/>" + 
                                      "</Index>" + 
                                    "</Group>" +
                                  "</Resource>" +
                                "</MyGUI>";

            MemoryArchive memArchive = MemoryArchiveFactory.Instance.getArchive("mem/");
            memArchive.addMemoryStreamResource("MemoryXml", new MemoryStream(ASCIIEncoding.Default.GetBytes(xmlTest)));

            MemoryStream imageStream = new MemoryStream();
            Image image = Resources.TestIcon;
            image.Save(imageStream, ImageFormat.Png);
            image.Dispose();
            memArchive.addMemoryStreamResource("MemoryImage.png", imageStream);

            ResourceManager.Instance.load("mem/MemoryXml");
*/