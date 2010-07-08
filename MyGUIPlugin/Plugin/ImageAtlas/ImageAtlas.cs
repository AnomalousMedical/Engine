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
    public class ImageAtlas : IDisposable
    {
        private String name;
        private Size2 imageSize;
        private Size2 atlasPageSize;
        private MemoryArchive memoryArchive;
        private Dictionary<String, String> guidDictionary = new Dictionary<String, String>();

        public ImageAtlas(String name, Size2 imageSize, Size2 atlasPageSize)
        {
            this.name = name;
            this.imageSize = imageSize;
            this.atlasPageSize = atlasPageSize;
            OgreResourceGroupManager.getInstance().addResourceLocation(name, "Memory", "MyGUI", true);
            memoryArchive = MemoryArchiveFactory.Instance.getArchive(name);
        }

        public void Dispose()
        {
            clear();
            OgreResourceGroupManager.getInstance().removeResourceLocation(name, "MyGUI");
        }

        public String addImage(String resourceName, Image image)
        {
            Guid guid = Guid.NewGuid();
            String guidStr = Guid.NewGuid().ToString();
            guidDictionary.Add(resourceName, guidStr);

            //resize the image if it does not match
            bool resizedImage = false;
            Size addImageSize = image.Size;
            if (addImageSize.Width != imageSize.Width || imageSize.Height != addImageSize.Height)
            {
                image = new Bitmap(image, new Size((int)imageSize.Width, (int)imageSize.Height));
                resizedImage = true;
            }

            MemoryStream imageStream = new MemoryStream();
            image.Save(imageStream, ImageFormat.Png);
            memoryArchive.addMemoryStreamResource(guidStr + ".png", imageStream);

            String xmlString = String.Format(resourceXML, guidStr, name + guidStr + ".png", imageSize.Width, imageSize.Height);
            memoryArchive.addMemoryStreamResource(guidStr + ".xml", new MemoryStream(ASCIIEncoding.UTF8.GetBytes(xmlString)));

            ResourceManager.Instance.load(name + guidStr + ".xml");

            //Dispose the image if it was resized
            if (resizedImage)
            {
                image.Dispose();
            }
            return guidStr;
        }

        public void removeImage(String resourceName)
        {
            String guid = null;
            if(guidDictionary.TryGetValue(resourceName, out guid))
            {
                deleteImage(guid);
                guidDictionary.Remove(resourceName);
            }
        }

        public bool containsImage(String resourceName)
        {
            return guidDictionary.ContainsKey(resourceName);
        }

        public String getImageId(String resourceName)
        {
            String guid = null;
            guidDictionary.TryGetValue(resourceName, out guid);
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

        private void deleteImage(String guid)
        {
            ResourceManager.Instance.remove(guid);
            memoryArchive.destroyMemoryStreamResource(guid + ".png");
            memoryArchive.destroyMemoryStreamResource(guid + ".xml");
        }

        private string resourceXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                                "<MyGUI type=\"Resource\" version=\"1.1\">\n" +
                                  "<Resource type=\"ResourceImageSet\" name=\"{0}\">\n" +
                                    "<Group name=\"Icons\" texture=\"{1}\" size=\"{2} {3}\">\n" +
                                      "<Index name=\"Skin\">\n" +
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