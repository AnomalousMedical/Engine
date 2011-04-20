using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Engine.Attributes;

namespace OgreWrapper
{
    /// <summary>
    /// Enum describing how the position / size of an element is to be recorded. 
    /// </summary>
    public enum GuiMetricsMode : uint
    {
	    /// <summary>
	    /// 'left', 'top', 'height' and 'width' are parametrics from 0.0 to 1.0
	    /// </summary>
	    GMM_RELATIVE,
	    /// <summary>
	    /// Positions &amp; sizes are in absolute pixels
	    /// </summary>
	    GMM_PIXELS,
	    /// <summary>
	    /// Positions &amp; sizes are in virtual pixels
	    /// </summary>
        GMM_RELATIVE_ASPECT_ADJUSTED
    };

    /// <summary>
    /// Enum describing where '0' is in relation to the parent in the horizontal
    /// dimension. Affects how 'left' is interpreted.
    /// </summary>
    public enum GuiHorizontalAlignment : uint
    {
        GHA_LEFT,
        GHA_CENTER,
        GHA_RIGHT
    };

    /// <summary>
    /// Enum describing where '0' is in relation to the parent in the vertical
    /// dimension. Affects how 'top' is interpreted.
    /// </summary>
    public enum GuiVerticalAlignment : uint
    {
        GVA_TOP,
        GVA_CENTER,
        GVA_BOTTOM
    };

    [NativeSubsystemType]
    public class OverlayElement : IDisposable
    {
        internal static OverlayElement createWrapper(IntPtr nativeObject, object[] args)
        {
	        String type = Marshal.PtrToStringAnsi(OverlayElement_getTypeName(nativeObject));
	        if(type.Equals(TextAreaOverlayElement.TypeName))
	        {
		        return new TextAreaOverlayElement(nativeObject);
	        }
	        else if(type.Equals(PanelOverlayElement.TypeName))
	        {
		        return new PanelOverlayElement(nativeObject);
	        }
	        else if(type.Equals(BorderPanelOverlayElement.TypeName))
	        {
		        return new BorderPanelOverlayElement(nativeObject);
	        }
	        throw new NotImplementedException();
        }

        private static UTF8Encoding utf8Encoder = new UTF8Encoding();

        protected IntPtr overlayElement;

        protected OverlayElement(IntPtr overlayElement)
        {
            this.overlayElement = overlayElement;
        }

        public void Dispose()
        {
            overlayElement = IntPtr.Zero;
        }

        internal IntPtr OgreObject
        {
            get
            {
                return overlayElement;
            }
        }

        public void initialize()
        {
            OverlayElement_initialize(overlayElement);
        }

        public String getName()
        {
            return Marshal.PtrToStringAnsi(OverlayElement_getName(overlayElement));
        }

        public void show()
        {
            OverlayElement_show(overlayElement);
        }

        public void hide()
        {
            OverlayElement_hide(overlayElement);
        }

        public bool isVisible()
        {
            return OverlayElement_isVisible(overlayElement);
        }

        public bool isEnabled()
        {
            return OverlayElement_isEnabled(overlayElement);
        }

        public void setEnabled(bool b)
        {
            OverlayElement_setEnabled(overlayElement, b);
        }

        public void setDimensions(float width, float height)
        {
            OverlayElement_setDimensions(overlayElement, width, height);
        }

        public void setPosition(float left, float top)
        {
            OverlayElement_setPosition(overlayElement, left, top);
        }

        public void setWidth(float width)
        {
            OverlayElement_setWidth(overlayElement, width);
        }

        public float getWidth()
        {
            return OverlayElement_getWidth(overlayElement);
        }

        public void setHeight(float height)
        {
            OverlayElement_setHeight(overlayElement, height);
        }

        public float getHeight()
        {
            return OverlayElement_getHeight(overlayElement);
        }

        public void setLeft(float left)
        {
            OverlayElement_setLeft(overlayElement, left);
        }

        public float getLeft()
        {
            return OverlayElement_getLeft(overlayElement);
        }

        public void setTop(float top)
        {
            OverlayElement_setTop(overlayElement, top);
        }

        public float getTop()
        {
            return OverlayElement_getTop(overlayElement);
        }

        public String getMaterialName()
        {
            return Marshal.PtrToStringAnsi(OverlayElement_getMaterialName(overlayElement));
        }

        public void setMaterialName(String matName)
        {
            OverlayElement_setMaterialName(overlayElement, matName);
        }

        public MaterialPtr getMaterial()
        {
            MaterialManager matManager = MaterialManager.getInstance();
            return matManager.getObject(OverlayElement_getMaterial(overlayElement, matManager.ProcessWrapperObjectCallback));
        }

        public float getDerivedLeft()
        {
            return OverlayElement_getDerivedLeft(overlayElement);
        }

        public float getDerivedTop()
        {
            return OverlayElement_getDerivedTop(overlayElement);
        }

        public String getTypeName()
        {
            return Marshal.PtrToStringAnsi(OverlayElement_getTypeName(overlayElement));
        }

        public unsafe void setCaption(String displayString)
        {
            byte[] utf8DisplayString = utf8Encoder.GetBytes(displayString);
            fixed (byte* b = &utf8DisplayString[0])
            {
                OverlayElement_setCaption(overlayElement, b);
            }
        }

        public String getCaption()
        {
            return Marshal.PtrToStringUni(OverlayElement_getCaption(overlayElement));
        }

        public void setColor(Color color)
        {
            OverlayElement_setColor(overlayElement, color);
        }

        public Color getColor()
        {
            return OverlayElement_getColor(overlayElement);
        }

        public void setMetricsMode(GuiMetricsMode mode)
        {
            OverlayElement_setMetricsMode(overlayElement, mode);
        }

        public GuiMetricsMode getMetricsMode()
        {
            return OverlayElement_getMetricsMode(overlayElement);
        }

        public void setHorizontalAlignment(GuiHorizontalAlignment gha)
        {
            OverlayElement_setHorizontalAlignment(overlayElement, gha);
        }

        public GuiHorizontalAlignment getHorizontalAlignment()
        {
            return OverlayElement_getHorizontalAlignment(overlayElement);
        }

        public void setVerticalAlignment(GuiVerticalAlignment gva)
        {
            OverlayElement_setVerticalAlignment(overlayElement, gva);
        }

        public GuiVerticalAlignment getVerticalAlignment()
        {
            return OverlayElement_getVerticalAlignment(overlayElement);
        }

        public bool contains(float x, float y)
        {
            return OverlayElement_contains(overlayElement, x, y);
        }

        public OverlayElement findElementAt(float x, float y)
        {
            return OverlayManager.getInstance().getObject(OverlayElement_findElementAt(overlayElement, x, y));
        }

        public bool isContainer()
        {
            return OverlayElement_isContainer(overlayElement);
        }

        public bool isKeyEnabled()
        {
            return OverlayElement_isKeyEnabled(overlayElement);
        }

        public bool isCloneable()
        {
            return OverlayElement_isCloneable(overlayElement);
        }

        public void setCloneable(bool c)
        {
            OverlayElement_setCloneable(overlayElement, c);
        }

        public OverlayContainer getParent()
        {
            return OverlayManager.getInstance().getObject(OverlayElement_getParent(overlayElement)) as OverlayContainer;
        }

        public ushort getZOrder()
        {
            return OverlayElement_getZOrder(overlayElement);
        }

        public void copyFromTemplate(OverlayElement templateOverlay)
        {
            OverlayElement_copyFromTemplate(overlayElement, templateOverlay.OgreObject);
        }

        public OverlayElement clone(String instanceName)
        {
            return OverlayManager.getInstance().getObject(OverlayElement_clone(overlayElement, instanceName));
        }

        public OverlayElement getSourceTemplate()
        {
            return OverlayManager.getInstance().getObject(OverlayElement_getSourceTemplate(overlayElement));
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_initialize(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayElement_getName(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_show(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_hide(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool OverlayElement_isVisible(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool OverlayElement_isEnabled(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setEnabled(IntPtr overlayElement, bool b);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setDimensions(IntPtr overlayElement, float width, float height);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setPosition(IntPtr overlayElement, float left, float top);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setWidth(IntPtr overlayElement, float width);

        [DllImport("OgreCWrapper")]
        private static extern float OverlayElement_getWidth(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setHeight(IntPtr overlayElement, float height);

        [DllImport("OgreCWrapper")]
        private static extern float OverlayElement_getHeight(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setLeft(IntPtr overlayElement, float left);

        [DllImport("OgreCWrapper")]
        private static extern float OverlayElement_getLeft(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setTop(IntPtr overlayElement, float top);

        [DllImport("OgreCWrapper")]
        private static extern float OverlayElement_getTop(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayElement_getMaterialName(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setMaterialName(IntPtr overlayElement, String matName);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayElement_getMaterial(IntPtr overlayElement, ProcessWrapperObjectDelegate processWrapper);

        [DllImport("OgreCWrapper")]
        private static extern float OverlayElement_getDerivedLeft(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern float OverlayElement_getDerivedTop(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayElement_getTypeName(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern unsafe void OverlayElement_setCaption(IntPtr overlayElement, byte* displayString);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayElement_getCaption(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setColor(IntPtr overlayElement, Color color);

        [DllImport("OgreCWrapper")]
        private static extern Color OverlayElement_getColor(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setMetricsMode(IntPtr overlayElement, GuiMetricsMode mode);

        [DllImport("OgreCWrapper")]
        private static extern GuiMetricsMode OverlayElement_getMetricsMode(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setHorizontalAlignment(IntPtr overlayElement, GuiHorizontalAlignment gha);

        [DllImport("OgreCWrapper")]
        private static extern GuiHorizontalAlignment OverlayElement_getHorizontalAlignment(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setVerticalAlignment(IntPtr overlayElement, GuiVerticalAlignment gva);

        [DllImport("OgreCWrapper")]
        private static extern GuiVerticalAlignment OverlayElement_getVerticalAlignment(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool OverlayElement_contains(IntPtr overlayElement, float x, float y);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayElement_findElementAt(IntPtr overlayElement, float x, float y);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool OverlayElement_isContainer(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool OverlayElement_isKeyEnabled(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool OverlayElement_isCloneable(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_setCloneable(IntPtr overlayElement, bool c);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayElement_getParent(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern ushort OverlayElement_getZOrder(IntPtr overlayElement);

        [DllImport("OgreCWrapper")]
        private static extern void OverlayElement_copyFromTemplate(IntPtr overlayElement, IntPtr templateOverlay);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayElement_clone(IntPtr overlayElement, String instanceName);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayElement_getSourceTemplate(IntPtr overlayElement);

#endregion
    }
}
