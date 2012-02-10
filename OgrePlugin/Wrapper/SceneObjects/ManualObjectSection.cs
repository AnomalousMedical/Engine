using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;
using Engine;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class ManualObjectSection : IDisposable
    {
        private IntPtr ogreSection;

        internal static ManualObjectSection createWrapper(IntPtr nativeObject, object[] args)
        {
            return new ManualObjectSection(nativeObject);
        }

        internal ManualObjectSection(IntPtr ogreSection)
        {
            this.ogreSection = ogreSection;
        }

        public void Dispose()
        {
            ogreSection = IntPtr.Zero;
        }

        /// <summary>
	    /// Retrieve the material name in use. 
	    /// </summary>
	    /// <returns>The material name.</returns>
	    public String getMaterialName()
        {
            return Marshal.PtrToStringAnsi(ManualObjectSection_getMaterialName(ogreSection));
        }

	    /// <summary>
	    /// Update the material name in use.
	    /// </summary>
	    /// <param name="name">The new material to use.</param>
        public void setMaterialName(String name)
        {
            ManualObjectSection_setMaterialName(ogreSection, name);
        }

	    /// <summary>
	    /// Set whether we need 32-bit indices. 
	    /// </summary>
	    /// <param name="n32">True to use 32 bit indices.  False to not use them.</param>
        public void set32BitIndices(bool n32)
        {
            ManualObjectSection_set32BitIndices(ogreSection, n32);
        }

	    /// <summary>
	    /// Get whether we need 32-bit indices.
	    /// </summary>
	    /// <returns>True if 32 bit indicies are in use.</returns>
        public bool get32BitIndices()
        {
            return ManualObjectSection_get32BitIndices(ogreSection);
        }

        public void setCustomParameter(int index, Quaternion value)
        {
            ManualObject_setCustomParameter(ogreSection, new IntPtr(index), value);
        }

        public Quaternion getCustomParameter(int index)
        {
            return ManualObject_getCustomParameter(ogreSection, new IntPtr(index));
        }

        #region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ManualObjectSection_getMaterialName(IntPtr ogreSection);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObjectSection_setMaterialName(IntPtr ogreSection, String name);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObjectSection_set32BitIndices(IntPtr ogreSection, bool n32);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ManualObjectSection_get32BitIndices(IntPtr ogreSection);

        [DllImport("OgreCWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManualObject_setCustomParameter(IntPtr subEntity, IntPtr index, Quaternion value);

        [DllImport("OgreCWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern Quaternion ManualObject_getCustomParameter(IntPtr subEntity, IntPtr index);

        #endregion
    }
}
