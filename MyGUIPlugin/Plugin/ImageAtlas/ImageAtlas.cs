using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class ImageAtlas
    {
        //public 
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