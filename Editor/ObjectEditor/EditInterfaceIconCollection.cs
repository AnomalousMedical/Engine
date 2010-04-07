using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Editor.Properties;
using System.Windows.Forms;

namespace Editor
{
    public enum EditorIcons
    {
        Folder,
        QuestionMark,
    }

    public class EditInterfaceIconCollection
    {
        private static Dictionary<Object, String> iconDictionary = new Dictionary<object, String>();
        private static ImageList imageList = new ImageList();

        static EditInterfaceIconCollection()
        {
            imageList.Images.Add("QuestionMark", Resources.QuestionMark);
            imageList.Images.Add("Folder", Resources.Folder);

            iconDictionary.Add(EditorIcons.Folder, "Folder");
            iconDictionary.Add(EditorIcons.QuestionMark, "QuestionMark");
        }

        public static String getImageKey(Object key)
        {
            String returnValue = "QuestionMark";
            if (key != null)
            {
                iconDictionary.TryGetValue(key, out returnValue);
            }
            return returnValue;
        }

        public static void setupTreeIcons(TreeView tree)
        {
            tree.ImageList = imageList;
        }
    }
}
