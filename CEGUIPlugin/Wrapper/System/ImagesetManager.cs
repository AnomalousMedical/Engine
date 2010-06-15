using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace CEGUIPlugin
{
    public class ImagesetManager
    {
        static ImagesetManager instance;

        public static ImagesetManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ImagesetManager();
                }
                return instance;
            }
        }

        IntPtr imagesetManager;
        WrapperCollection<ImageSet> imageSets = new WrapperCollection<ImageSet>(ImageSet.createWrapper);

        private ImagesetManager()
        {
            imagesetManager = ImagesetManager_getSingletonPtr();
        }

        public void Dispose()
        {
            imageSets.Dispose();
        }

        public ImageSet create(String xmlFileName)
        {
            return imageSets.getObject(ImagesetManager_create(imagesetManager, xmlFileName));
        }

        public ImageSet create(String xmlFileName, String resourceGroup)
        {
            return imageSets.getObject(ImagesetManager_create2(imagesetManager, xmlFileName, resourceGroup));
        }

        public void destroy(String objectName)
        {
            imageSets.destroyObject(ImagesetManager_get(imagesetManager, objectName));
            ImagesetManager_destroy(imagesetManager, objectName);
        }

        public void destroy(ImageSet imageSet)
        {
            ImagesetManager_destroy2(imagesetManager, imageSets.destroyObject(imageSet.ImageSetPtr));
        }

        public void destroyAll()
        {
            imageSets.clearObjects();
            ImagesetManager_destroyAll(imagesetManager);
        }

        public ImageSet get(String objectName)
        {
            return imageSets.getObject(ImagesetManager_get(imagesetManager, objectName));
        }

        public bool isDefined(String objectName)
        {
            return ImagesetManager_isDefined(imagesetManager, objectName);
        }

        public void createAll(String pattern, String resourceGroup)
        {
            ImagesetManager_createAll(imagesetManager, pattern, resourceGroup);
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr ImagesetManager_getSingletonPtr();

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr ImagesetManager_create(IntPtr imagesetManager, String xmlFileName);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr ImagesetManager_create2(IntPtr imagesetManager, String xmlFileName, String resourceGroup);

        [DllImport("CEGUIWrapper")]
        private static extern void ImagesetManager_destroy(IntPtr imagesetManager, String objectName);

        [DllImport("CEGUIWrapper")]
        private static extern void ImagesetManager_destroy2(IntPtr imagesetManager, IntPtr imageSet);

        [DllImport("CEGUIWrapper")]
        private static extern void ImagesetManager_destroyAll(IntPtr imagesetManager);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr ImagesetManager_get(IntPtr imagesetManager, String objectName);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ImagesetManager_isDefined(IntPtr imagesetManager, String objectName);

        [DllImport("CEGUIWrapper")]
        private static extern void ImagesetManager_createAll(IntPtr imagesetManager, String pattern, String resourceGroup);

#endregion
    }
}
