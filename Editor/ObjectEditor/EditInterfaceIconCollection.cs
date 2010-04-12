using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Editor.Properties;
using System.Windows.Forms;
using Engine.Editing;

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
            addIcon(EditorIcons.Folder, Resources.Folder);
            addIcon(EditorIcons.QuestionMark, Resources.QuestionMark);
            
            addIcon(EngineIcons.Entity, Resources.Entity);
            addIcon(EngineIcons.Joint, Resources.Joint);
            addIcon(EngineIcons.Light, Resources.Light);
            addIcon(EngineIcons.Node, Resources.Node);
            addIcon(EngineIcons.RigidBody, Resources.RigidBody);
            addIcon(EngineIcons.Camera, Resources.Camera);
            addIcon(EngineIcons.ManualObject, Resources.ManualObject);
            addIcon(EngineIcons.Scene, Resources.Scene);
            addIcon(EngineIcons.SimObject, Resources.SimObject);
            addIcon(EngineIcons.Behavior, Resources.Behavior);
            addIcon(EngineIcons.Resources, Resources.Resource);
        }

        static public void addIcon(Object key, Image image)
        {
            imageList.Images.Add(key.ToString(), image);
            iconDictionary.Add(key, key.ToString());
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
