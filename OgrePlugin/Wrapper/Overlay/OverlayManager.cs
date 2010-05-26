using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class OverlayManager
    {
        WrapperCollection<Overlay> overlays = new WrapperCollection<Overlay>(Overlay.createWrapper);
        WrapperCollection<OverlayElement> overlayElements = new WrapperCollection<OverlayElement>(OverlayElement.createWrapper);
        IntPtr overlayManager;

        static OverlayManager instance = new OverlayManager();

        public static OverlayManager getInstance()
        {
            return instance;
        }

        private OverlayManager()
        {
            overlayManager = OverlayManager_getSingletonPtr();
        }

        internal OverlayElement getObject(IntPtr overlayElement)
        {
            return overlayElements.getObject(overlayElement);
        }

        public Overlay create(String name)
        {
            return overlays.getObject(OverlayManager_create(overlayManager, name));
        }

        public Overlay getByName(String name)
        {
            return overlays.getObject(OverlayManager_getByName(overlayManager, name));
        }

        public void destroy(String name)
        {
            overlays.destroyObject(OverlayManager_getByName(overlayManager, name));
            OverlayManager_destroyName(overlayManager, name);
        }

        public void destroy(Overlay overlay)
        {
            overlays.destroyObject(overlay.OgreObject);
            OverlayManager_destroy(overlayManager, overlay.OgreObject);
        }

        public void destroyAll()
        {
            OverlayManager_destroyAll(overlayManager);
        }

        public bool hasViewportChanged()
        {
            return OverlayManager_hasViewportChanged(overlayManager);
        }

        public int getViewportHeight()
        {
            return OverlayManager_getViewportHeight(overlayManager);
        }

        public int getViewportWidth()
        {
            return OverlayManager_getViewportWidth(overlayManager);
        }

        public float getViewportAspectRatio()
        {
            return OverlayManager_getViewportAspectRatio(overlayManager);
        }

        public OverlayElement createOverlayElement(String typeName, String instanceName)
        {
            return overlayElements.getObject(OverlayManager_createOverlayElementTypeInstance(overlayManager, typeName, instanceName));
        }

        public OverlayElement createOverlayElement(String typeName, String instanceName, bool isTemplate)
        {
            return overlayElements.getObject(OverlayManager_createOverlayElement(overlayManager, typeName, instanceName, isTemplate));
        }

        public OverlayElement getOverlayElement(String name)
        {
            return overlayElements.getObject(OverlayManager_getOverlayElementName(overlayManager, name));
        }

        public OverlayElement getOverlayElement(String name, bool isTemplate)
        {
            return overlayElements.getObject(OverlayManager_getOverlayElement(overlayManager, name, isTemplate));
        }

        public void destroyOverlayElement(String name)
        {
            overlayElements.destroyObject(OverlayManager_getOverlayElementName(overlayManager, name));
            OverlayManager_destroyOverlayElementName(overlayManager, name);
        }

        public void destroyOverlayElement(String name, bool isTemplate)
        {
            overlayElements.destroyObject(OverlayManager_getOverlayElement(overlayManager, name, isTemplate));
            OverlayManager_destroyOverlayElementNameTemplate(overlayManager, name, isTemplate);
        }

        public void destroyOverlayElement(OverlayElement element)
        {
            overlayElements.destroyObject(element.OgreObject);
            OverlayManager_destroyOverlayElement(overlayManager, element.OgreObject);
        }

        public void destroyOverlayElement(OverlayElement element, bool isTemplate)
        {
            overlayElements.destroyObject(element.OgreObject);
            OverlayManager_destroyOverlayElementTemplate(overlayManager, element.OgreObject, isTemplate);
        }

        public void destroyAllOverlayElements()
        {
            overlayElements.clearObjects();
            OverlayManager_destroyAllOverlayElements(overlayManager);
        }

        public void destroyAllOverlayElements(bool isTemplate)
        {
            overlayElements.clearObjects();
            OverlayManager_destroyAllOverlayElementsTemplate(overlayManager, isTemplate);
        }

        public OverlayElement createOverlayElementFromTemplate(String templateName, String typeName, String instanceName)
        {
            return overlayElements.getObject(OverlayManager_createOverlayElementFromTemplate1(overlayManager, templateName, typeName, instanceName));
        }

        public OverlayElement createOverlayElementFromTemplate(String templateName, String typeName, String instanceName, bool isTemplate)
        {
            return overlayElements.getObject(OverlayManager_createOverlayElementFromTemplate2(overlayManager, templateName, typeName, instanceName, isTemplate));
        }

        public OverlayElement cloneOverlayElementFromTemplate(String templateName, String instanceName)
        {
            return overlayElements.getObject(OverlayManager_cloneOverlayElementFromTemplate(overlayManager, templateName, instanceName));
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayManager_getSingletonPtr();

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayManager_create(IntPtr overlayManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayManager_getByName(IntPtr overlayManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayManager_destroyName(IntPtr overlayManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayManager_destroy(IntPtr overlayManager, IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayManager_destroyAll(IntPtr overlayManager);

        [DllImport("OgreCWrapper")]
        private static extern bool OverlayManager_hasViewportChanged(IntPtr overlayManager);

        [DllImport("OgreCWrapper")]
        private static extern int OverlayManager_getViewportHeight(IntPtr overlayManager);

        [DllImport("OgreCWrapper")]
        private static extern int OverlayManager_getViewportWidth(IntPtr overlayManager);

        [DllImport("OgreCWrapper")]
        private static extern float OverlayManager_getViewportAspectRatio(IntPtr overlayManager);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayManager_createOverlayElementTypeInstance(IntPtr overlayManager, String typeName, String instanceName);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayManager_createOverlayElement(IntPtr overlayManager, String typeName, String instanceName, bool isTemplate);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayManager_getOverlayElementName(IntPtr overlayManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayManager_getOverlayElement(IntPtr overlayManager, String name, bool isTemplate);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayManager_destroyOverlayElementName(IntPtr overlayManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayManager_destroyOverlayElementNameTemplate(IntPtr overlayManager, String name, bool isTemplate);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayManager_destroyOverlayElement(IntPtr overlayManager, IntPtr element);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayManager_destroyOverlayElementTemplate(IntPtr overlayManager, IntPtr element, bool isTemplate);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayManager_destroyAllOverlayElements(IntPtr overlayManager);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayManager_destroyAllOverlayElementsTemplate(IntPtr overlayManager, bool isTemplate);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayManager_createOverlayElementFromTemplate1(IntPtr overlayManager, String templateName, String typeName, String instanceName);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayManager_createOverlayElementFromTemplate2(IntPtr overlayManager, String templateName, String typeName, String instanceName, bool isTemplate);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayManager_cloneOverlayElementFromTemplate(IntPtr overlayManager, String templateName, String instanceName);

#endregion
    }
}
