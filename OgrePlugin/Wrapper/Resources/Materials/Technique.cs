using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class Technique : IDisposable
    {
        internal static Technique createWrapper(IntPtr nativeObject, object[] args)
        {
            return new Technique(nativeObject, args[0] as Material);
        }

        private IntPtr technique;
        private Material parent;
        private WrapperCollection<Pass> passes = new WrapperCollection<Pass>(Pass.createWrapper);

        private Technique(IntPtr technique, Material parent)
        {
            this.technique = technique;
            this.parent = parent;
        }

        public void Dispose()
        {
            passes.Dispose();
            technique = IntPtr.Zero;
        }

        public Pass createPass()
        {
            return passes.getObject(Technique_createPass(technique), this);
        }

	    public Pass getPass(ushort index)
        {
            return passes.getObject(Technique_getPass(technique, index), this);
        }

	    public Pass getPass(String name)
        {
            return passes.getObject(Technique_getPassName(technique, name), this);
        }

	    public ushort getNumPasses()
        {
            return Technique_getNumPasses(technique);
        }

	    public void removePass(ushort index)
        {
            passes.destroyObject(Technique_getPass(technique, index));
            Technique_removePass(technique, index);
        }

	    public void removeAllPasses()
        {
            passes.clearObjects();
            Technique_removeAllPasses(technique);
        }

	    public bool movePass(ushort sourceIndex, ushort destinationIndex)
        {
            return Technique_movePass(technique, sourceIndex, destinationIndex);
        }

	    public Material getParent()
        {
            return parent;
        }

	    public String getResourceGroup()
        {
            return Marshal.PtrToStringAnsi(Technique_getResourceGroup(technique));
        }

	    public bool isTransparent()
        {
            return Technique_isTransparent(technique);
        }

	    public bool isTransparentSortingEnabled()
        {
            return Technique_isTransparentSortingEnabled(technique);
        }

	    public void setPointSize(float ps)
        {
            Technique_setPointSize(technique, ps);
        }

	    public void setAmbient(float red, float green, float blue)
        {
            Technique_setAmbientRGB(technique, red, green, blue);
        }

	    public void setAmbient(Color color)
        {
            Technique_setAmbient(technique, color);
        }

	    public void setDiffuse(float red, float green, float blue, float alpha)
        {
            Technique_setDiffuseRGBA(technique, red, green, blue, alpha);
        }

	    public void setDiffuse(Color color)
        {
            Technique_setDiffuse(technique, color);
        }

	    public void setSpecular(float red, float green, float blue, float alpha)
        {
            Technique_setSpecularRGBA(technique, red, green, blue, alpha);
        }

	    public void setSpecular(Color color)
        {
            Technique_setSpecular(technique, color);
        }

	    public void setShininess(float value)
        {
            Technique_setShininess(technique, value);
        }

	    public void setSelfIllumination(float red, float green, float blue)
        {
            Technique_setSelfIlluminationRGB(technique, red, green, blue);
        }

	    public void setSelfIllumination(Color color)
        {
            Technique_setSelfIllumination(technique, color);
        }

	    public void setDepthCheckEnabled(bool enabled)
        {
            Technique_setDepthCheckEnabled(technique, enabled);
        }

	    public void setDepthWriteEnabled(bool enabled)
        {
            Technique_setDepthWriteEnabled(technique, enabled);
        }

	    public void setDepthFunction(CompareFunction func)
        {
            Technique_setDepthFunction(technique, func);
        }

	    public void setColorWriteEnabled(bool enabled)
        {
            Technique_setColorWriteEnabled(technique, enabled);
        }

	    public void setCullingMode(CullingMode mode)
        {
            Technique_setCullingMode(technique, mode);
        }

	    public void setManualCullingMode(ManualCullingMode mode)
        {
            Technique_setManualCullingMode(technique, mode);
        }

	    public void setLightingEnabled(bool enabled)
        {
            Technique_setLightingEnabled(technique, enabled);
        }

	    public void setShadingMode(ShadeOptions mode)
        {
            Technique_setShadingMode(technique, mode);
        }

	    public void setFog(bool overrideScene, FogMode mode, Color color)
        {
            Technique_setFog(technique, overrideScene, mode, color);
        }

	    public void setFog(bool overrideScene, FogMode mode, Color color, float expDensity, float linearStart, float linearEnd)
        {
            Technique_setFog2(technique, overrideScene, mode, color, expDensity, linearStart, linearEnd);
        }

	    public void setDepthBias(float constantBias, float slopeScaleBias)
        {
            Technique_setDepthBias(technique, constantBias, slopeScaleBias);
        }

	    public void setTextureFiltering(TextureFilterOptions filterType)
        {
            Technique_setTextureFiltering(technique, filterType);
        }

	    public void setTextureAnisotropy(int maxAniso)
        {
            Technique_setTextureAnisotropy(technique, maxAniso);
        }

	    public void setSceneBlending(SceneBlendType sbt)
        {
            Technique_setSceneBlending(technique, sbt);
        }

	    public void setSeparateSceneBlending(SceneBlendType sbt, SceneBlendType sbta)
        {
            Technique_setSeparateSceneBlending(technique, sbt, sbta);
        }

	    public void setSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor)
        {
            Technique_setSceneBlending2(technique, sourceFactor, destFactor);
        }

	    public void setSeparateSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha)
        {
            Technique_setSeparateSceneBlending2(technique, sourceFactor, destFactor, sourceFactorAlpha, destFactorAlpha);
        }

	    public void setLodIndex(ushort index)
        {
            Technique_setLodIndex(technique, index);
        }

	    public ushort getLodIndex()
        {
            return Technique_getLodIndex(technique);
        }

	    public void setSchemeName(String schemeName)
        {
            Technique_setSchemeName(technique, schemeName);
        }

	    public String getSchemeName()
        {
            return Marshal.PtrToStringAnsi(Technique_getSchemeName(technique));
        }

	    public bool isDepthWriteEnabled()
        {
            return Technique_isDepthWriteEnabled(technique);
        }

	    public bool isDepthCheckEnabled()
        {
            return Technique_isDepthCheckEnabled(technique);
        }

	    public bool hasColorWriteDisabled()
        {
            return Technique_hasColorWriteDisabled(technique);
        }

	    public void setName(String name)
        {
            Technique_setName(technique, name);
        }

	    public String getName()
        {
            return Marshal.PtrToStringAnsi(Technique_getName(technique));
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Technique_createPass(IntPtr technique);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Technique_getPass(IntPtr technique, ushort index);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Technique_getPassName(IntPtr technique, String name);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Technique_getNumPasses(IntPtr technique);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_removePass(IntPtr technique, ushort index);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_removeAllPasses(IntPtr technique);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Technique_movePass(IntPtr technique, ushort sourceIndex, ushort destinationIndex);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Technique_getResourceGroup(IntPtr technique);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Technique_isTransparent(IntPtr technique);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Technique_isTransparentSortingEnabled(IntPtr technique);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setPointSize(IntPtr technique, float ps);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setAmbientRGB(IntPtr technique, float red, float green, float blue);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setAmbient(IntPtr technique, Color color);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setDiffuseRGBA(IntPtr technique, float red, float green, float blue, float alpha);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setDiffuse(IntPtr technique, Color color);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setSpecularRGBA(IntPtr technique, float red, float green, float blue, float alpha);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setSpecular(IntPtr technique, Color color);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setShininess(IntPtr technique, float value);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setSelfIlluminationRGB(IntPtr technique, float red, float green, float blue);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setSelfIllumination(IntPtr technique, Color color);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setDepthCheckEnabled(IntPtr technique, bool enabled);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setDepthWriteEnabled(IntPtr technique, bool enabled);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setDepthFunction(IntPtr technique, CompareFunction func);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setColorWriteEnabled(IntPtr technique, bool enabled);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setCullingMode(IntPtr technique, CullingMode mode);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setManualCullingMode(IntPtr technique, ManualCullingMode mode);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setLightingEnabled(IntPtr technique, bool enabled);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setShadingMode(IntPtr technique, ShadeOptions mode);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setFog(IntPtr technique, bool overrideScene, FogMode mode, Color color);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setFog2(IntPtr technique, bool overrideScene, FogMode mode, Color color, float expDensity, float linearStart, float linearEnd);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setDepthBias(IntPtr technique, float constantBias, float slopeScaleBias);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setTextureFiltering(IntPtr technique, TextureFilterOptions filterType);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setTextureAnisotropy(IntPtr technique, int maxAniso);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setSceneBlending(IntPtr technique, SceneBlendType sbt);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setSeparateSceneBlending(IntPtr technique, SceneBlendType sbt, SceneBlendType sbta);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setSceneBlending2(IntPtr technique, SceneBlendFactor sourceFactor, SceneBlendFactor destFactor);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setSeparateSceneBlending2(IntPtr technique, SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setLodIndex(IntPtr technique, ushort index);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Technique_getLodIndex(IntPtr technique);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setSchemeName(IntPtr technique, String schemeName);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Technique_getSchemeName(IntPtr technique);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Technique_isDepthWriteEnabled(IntPtr technique);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Technique_isDepthCheckEnabled(IntPtr technique);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Technique_hasColorWriteDisabled(IntPtr technique);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Technique_setName(IntPtr technique, String name);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Technique_getName(IntPtr technique);

#endregion
    }
}
