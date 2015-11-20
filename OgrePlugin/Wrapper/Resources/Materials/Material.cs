using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using Engine;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    /// <summary>
    /// Comparison functions used for the depth/stencil buffer operations and others. 
    /// </summary>
    public enum CompareFunction : uint
    {
        CMPF_ALWAYS_FAIL,
        CMPF_ALWAYS_PASS,
        CMPF_LESS,
        CMPF_LESS_EQUAL,
        CMPF_EQUAL,
        CMPF_NOT_EQUAL,
        CMPF_GREATER_EQUAL,
        CMPF_GREATER
    };

    /// <summary>
    /// Hardware culling modes based on vertex winding.
    /// This setting applies to how the hardware API culls triangles it is sent.
    /// </summary>
    public enum CullingMode : uint
    {
	    /// <summary>
	    /// Hardware never culls triangles and renders everything it receives.
	    /// </summary>
	    CULL_NONE = 1,

	    /// <summary>
	    /// Hardware culls triangles whose vertices are listed clockwise in the view (default).
	    /// </summary>
	    CULL_CLOCKWISE = 2,

	    /// <summary>
	    /// Hardware culls triangles whose vertices are listed anticlockwise in the view.
	    /// </summary>
        CULL_ANTICLOCKWISE = 3
    };

    /// <summary>
    /// Manual culling modes based on vertex normals. This setting applies to how
    /// the software culls triangles before sending them to the hardware API. This
    /// culling mode is used by scene managers which choose to implement it
    /// -normally those which deal with large amounts of fixed world geometry which
    /// is often planar (software culling movable variable geometry is expensive).
    /// </summary>
    public enum ManualCullingMode
    {
	    /// <summary>
	    /// No culling so everything is sent to the hardware.
	    /// </summary>
	    MANUAL_CULL_NONE = 1,
	    /// <summary>
	    /// Cull triangles whose normal is pointing away from the camera (default).
	    /// </summary>
	    MANUAL_CULL_BACK = 2,
	    /// <summary>
	    /// Cull triangles whose normal is pointing towards the camera.
	    /// </summary>
        MANUAL_CULL_FRONT = 3
    };


    /// <summary>
    /// Lighting shading options.
    /// </summary>
    public enum ShadeOptions
    {
        SO_FLAT,
        SO_GOURAUD,
        SO_PHONG
    };

    /// <summary>
    /// Fog modes. 
    /// </summary>
    public enum FogMode
    {
	    /// <summary>
	    /// No fog.
	    /// </summary>
	    FOG_NONE,
	    /// <summary>
	    /// Fog density increases  exponentially from the camera (fog = 1/e^(distance * density))
	    /// </summary>
	    FOG_EXP,
	    /// <summary>
	    /// Fog density increases at the square of FOG_EXP, i.e. even quicker (fog = 1/e^(distance * density)^2)
	    /// </summary>
	    FOG_EXP2,
	    /// <summary>
	    /// Fog density increases linearly between the start and end distances
	    /// </summary>
        FOG_LINEAR
    };

    /// <summary>
    /// High-level filtering options providing shortcuts to settings the
    /// minification, magnification and mip filters.
    /// </summary>
    public enum TextureFilterOptions
    {
	    /// <summary>
	    /// Equal to: min=FO_POINT, mag=FO_POINT, mip=FO_NONE
	    /// </summary>
	    TFO_NONE,
	    /// <summary>
	    /// Equal to: min=FO_LINEAR, mag=FO_LINEAR, mip=FO_POINT
	    /// </summary>
	    TFO_BILINEAR,
	    /// <summary>
	    /// Equal to: min=FO_LINEAR, mag=FO_LINEAR, mip=FO_LINEAR
	    /// </summary>
	    TFO_TRILINEAR,
	    /// <summary>
	    /// Equal to: min=FO_ANISOTROPIC, max=FO_ANISOTROPIC, mip=FO_LINEAR
	    /// </summary>
	    TFO_ANISOTROPIC
    };


    /// <summary>
    /// Types of blending that you can specify between an object and the existing
    /// contents of the scene.
    /// <para>
    /// As opposed to the LayerBlendType, which classifies blends between texture
    /// layers, these blending types blend between the output of the texture units
    /// and the pixels already in the viewport, allowing for object transparency,
    /// glows, etc. 
    /// </para>
    /// <para>
    /// These types are provided to give quick and easy access to common effects.
    /// You can also use the more manual method of supplying source and destination
    /// blending factors. See Material::setSceneBlending for more details. 
    /// </para>
    /// </summary>
    public enum SceneBlendType
    {
	    /// <summary>
	    /// Make the object transparent based on the final alpha values in the texture
	    /// </summary>
	    SBT_TRANSPARENT_ALPHA,
	    /// <summary>
	    /// Make the object transparent based on the colour values in the texture (brighter = more opaque)
	    /// </summary>
	    SBT_TRANSPARENT_COLOUR,
	    /// <summary>
	    /// Add the texture values to the existing scene content
	    /// </summary>
	    SBT_ADD,
	    /// <summary>
	    /// Multiply the 2 colours together
	    /// </summary>
	    SBT_MODULATE,
	    /// <summary>
	    /// The default blend mode where source replaces destination
	    /// </summary>
        SBT_REPLACE
    };

    /// <summary>
    /// Blending factors for manually blending objects with the scene. If there
    /// isn't a predefined SceneBlendType that you like, then you can specify the
    /// blending factors directly to affect the combination of object and the
    /// existing scene. See Material::setSceneBlending for more details.
    /// </summary>
    public enum SceneBlendFactor
    {
        SBF_ONE,
        SBF_ZERO,
        SBF_DEST_COLOUR,
        SBF_SOURCE_COLOUR,
        SBF_ONE_MINUS_DEST_COLOUR,
        SBF_ONE_MINUS_SOURCE_COLOUR,
        SBF_DEST_ALPHA,
        SBF_SOURCE_ALPHA,
        SBF_ONE_MINUS_DEST_ALPHA,
        SBF_ONE_MINUS_SOURCE_ALPHA
    };

    [NativeSubsystemType]
    public class Material : Resource
    {
        private WrapperCollection<Technique> techniques = new WrapperCollection<Technique>(Technique.createWrapper);

        internal static Material createWrapper(IntPtr material)
        {
            return new Material(material);
        }

        private Material(IntPtr material)
            :base(material)
        {
            
        }

        public override void Dispose()
        {
            techniques.Dispose();
            base.Dispose();
        }

        public bool isTransparent()
        {
            return Material_isTransparent(resource);
        }

        public void setReceiveShadows(bool enabled)
        {
            Material_setReceiveShadows(resource, enabled);
        }

        public bool getReceiveShadows()
        {
            return Material_getReceiveShadows(resource);
        }

        public void setTransparencyCastsShadows(bool enabled)
        {
            Material_setTransparencyCastsShadows(resource, enabled);
        }

        public bool getTransparencyCastsShadows()
        {
            return Material_getTransparencyCastsShadows(resource);
        }

        public Technique createTechnique()
        {
            return techniques.getObject(Material_createTechnique(resource), this);
        }

        public Technique getTechnique(ushort index)
        {
            return techniques.getObject(Material_getTechniqueIndex(resource, index), this);
        }

        public Technique getTechnique(String name)
        {
            return techniques.getObject(Material_getTechnique(resource, name), this);
        }

        public ushort getNumTechniques()
        {
            return Material_getNumTechniques(resource);
        }

        public void removeTechnique(ushort index)
        {
            techniques.destroyObject(Material_getTechniqueIndex(resource, index));
            Material_removeTechnique(resource, index);
        }

        public void removeAllTechniques()
        {
            techniques.clearObjects();
            Material_removeAllTechniques(resource);
        }

        public Technique getSupportedTechnique(ushort index)
        {
            return techniques.getObject(Material_getSupportedTechnique(resource, index), this);
        }

        public ushort getNumSupportedTechniques()
        {
            return Material_getNumSupportedTechniques(resource);
        }

        public String getUnsupportedTechniquesExplanation()
        {
            return Marshal.PtrToStringAnsi(Material_getUnsupportedTechniquesExplanation(resource));
        }

        public ushort getNumLodLevels(ushort schemeIndex)
        {
            return Material_getNumLodLevels(resource, schemeIndex);
        }

        public ushort getNumLodLevels(String schemeName)
        {
            return Material_getNumLodLevelsName(resource, schemeName);
        }

        public Technique getBestTechnique()
        {
            return techniques.getObject(Material_getBestTechnique(resource), this);
        }

        public Technique getBestTechnique(ushort lodIndex)
        {
            return techniques.getObject(Material_getBestTechniqueLod(resource, lodIndex), this);
        }

        public MaterialPtr clone(String newName)
        {
            MaterialManager matManager = MaterialManager.getInstance();
            return matManager.getObject(Material_clone(resource, newName, matManager.ProcessWrapperObjectCallback));
        }

        public MaterialPtr clone(String newName, bool changeGroup, String newGroup)
        {
            MaterialManager matManager = MaterialManager.getInstance();
            return matManager.getObject(Material_cloneChangeGroup(resource, newName, changeGroup, newGroup, matManager.ProcessWrapperObjectCallback));
        }

        public void copyDetailsTo(Material material)
        {
            Material_copyDetailsTo(this.resource, material.resource);
        }

        public void compile()
        {
            Material_compile(resource);
        }

        public void compile(bool autoManageTextureUnits)
        {
            Material_compileAutoManage(resource, autoManageTextureUnits);
        }

        public void setPointSize(float ps)
        {
            Material_setPointSize(resource, ps);
        }

        public void setAmbient(float red, float green, float blue)
        {
            Material_setAmbient(resource, red, green, blue);
        }

        public void setAmbient(Color color)
        {
            Material_setAmbientColor(resource, color);
        }

        public void setDiffuse(float red, float green, float blue, float alpha)
        {
            Material_setDiffuse(resource, red, green, blue, alpha);
        }

        public void setDiffuse(Color color)
        {
            Material_setDiffuseColor(resource, color);
        }

        public void setSpecular(float red, float green, float blue, float alpha)
        {
            Material_setSpecular(resource, red, green, blue, alpha);
        }

        public void setSpecular(Color color)
        {
            Material_setSpecularColor(resource, color);
        }

        public void setShininess(float value)
        {
            Material_setShininess(resource, value);
        }

        public void setSelfIllumination(float red, float green, float blue)
        {
            Material_setSelfIllumination(resource, red, green, blue);
        }

        public void setSelfIllumination(Color color)
        {
            Material_setSelfIlluminationColor(resource, color);
        }

        public void setDepthCheckEnabled(bool enabled)
        {
            Material_setDepthCheckEnabled(resource, enabled);
        }

        public void setDepthWriteEnabled(bool enabled)
        {
            Material_setDepthWriteEnabled(resource, enabled);
        }

        public void setDepthFunction(CompareFunction func)
        {
            Material_setDepthFunction(resource, func);
        }

        public void setColorWriteEnabled(bool enabled)
        {
            Material_setColorWriteEnabled(resource, enabled);
        }

        public void setCullingMode(CullingMode mode)
        {
            Material_setCullingMode(resource, mode);
        }

        public void setManualCullingMode(ManualCullingMode mode)
        {
            Material_setManualCullingMode(resource, mode);
        }

        public void setLightingEnabled(bool enabled)
        {
            Material_setLightingEnabled(resource, enabled);
        }

        public void setShadingMode(ShadeOptions mode)
        {
            Material_setShadingMode(resource, mode);
        }

        public void setFog(bool overrideScene, FogMode mode, Color color)
        {
            Material_setFog(resource, overrideScene, mode, color);
        }

        public void setFog(bool overrideScene, FogMode mode, Color color, float expDensity, float linearStart, float linearEnd)
        {
            Material_setFog2(resource, overrideScene, mode, color, expDensity, linearStart, linearEnd);
        }

        public void setDepthBias(float constantBias, float slopeScaleBias)
        {
            Material_setDepthBias(resource, constantBias, slopeScaleBias);
        }

        public void setTextureFiltering(TextureFilterOptions filterType)
        {
            Material_setTextureFiltering(resource, filterType);
        }

        public void setTextureAnisotropy(int maxAniso)
        {
            Material_setTextureAnisotropy(resource, maxAniso);
        }

        public void setSceneBlending(SceneBlendType sbt)
        {
            Material_setSceneBlending(resource, sbt);
        }

        public void setSeparateSceneBlending(SceneBlendType sbt, SceneBlendType sbta)
        {
            Material_setSeparateSceneBlending(resource, sbt, sbta);
        }

        public void setSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor)
        {
            Material_setSceneBlending2(resource, sourceFactor, destFactor);
        }

        public void setSeparateSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha)
        {
            Material_setSeparateSceneBlending2(resource, sourceFactor, destFactor, sourceFactorAlpha, destFactorAlpha);
        }

        public IEnumerable<Technique> Techniques
        {
            get
            {
                ushort count = getNumTechniques();
                for(ushort i = 0; i < count; ++i)
                {
                    yield return getTechnique(i);
                }
            }
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Material_isTransparent(IntPtr material);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setReceiveShadows(IntPtr material, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Material_getReceiveShadows(IntPtr material);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setTransparencyCastsShadows(IntPtr material, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Material_getTransparencyCastsShadows(IntPtr material);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Material_createTechnique(IntPtr material);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Material_getTechniqueIndex(IntPtr material, ushort index);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Material_getTechnique(IntPtr material, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Material_getNumTechniques(IntPtr material);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_removeTechnique(IntPtr material, ushort index);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_removeAllTechniques(IntPtr material);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Material_getSupportedTechnique(IntPtr material, ushort index);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Material_getNumSupportedTechniques(IntPtr material);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Material_getUnsupportedTechniquesExplanation(IntPtr material);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Material_getNumLodLevels(IntPtr material, ushort schemeIndex);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Material_getNumLodLevelsName(IntPtr material, String schemeName);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Material_getBestTechnique(IntPtr material);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Material_getBestTechniqueLod(IntPtr material, ushort lodIndex);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Material_clone(IntPtr material, String newName, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Material_cloneChangeGroup(IntPtr material, String newName, bool changeGroup, String newGroup, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_copyDetailsTo(IntPtr material, IntPtr materialToCopy);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_compile(IntPtr material);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_compileAutoManage(IntPtr material, bool autoManageTextureUnits);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setPointSize(IntPtr material, float ps);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setAmbient(IntPtr material, float red, float green, float blue);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setAmbientColor(IntPtr material, Color color);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setDiffuse(IntPtr material, float red, float green, float blue, float alpha);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setDiffuseColor(IntPtr material, Color color);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setSpecular(IntPtr material, float red, float green, float blue, float alpha);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setSpecularColor(IntPtr material, Color color);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setShininess(IntPtr material, float value);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setSelfIllumination(IntPtr material, float red, float green, float blue);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setSelfIlluminationColor(IntPtr material, Color color);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setDepthCheckEnabled(IntPtr material, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setDepthWriteEnabled(IntPtr material, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setDepthFunction(IntPtr material, CompareFunction func);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setColorWriteEnabled(IntPtr material, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setCullingMode(IntPtr material, CullingMode mode);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setManualCullingMode(IntPtr material, ManualCullingMode mode);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setLightingEnabled(IntPtr material, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setShadingMode(IntPtr material, ShadeOptions mode);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setFog(IntPtr material, bool overrideScene, FogMode mode, Color color);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setFog2(IntPtr material, bool overrideScene, FogMode mode, Color color, float expDensity, float linearStart, float linearEnd);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setDepthBias(IntPtr material, float constantBias, float slopeScaleBias);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setTextureFiltering(IntPtr material, TextureFilterOptions filterType);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setTextureAnisotropy(IntPtr material, int maxAniso);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setSceneBlending(IntPtr material, SceneBlendType sbt);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setSeparateSceneBlending(IntPtr material, SceneBlendType sbt, SceneBlendType sbta);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setSceneBlending2(IntPtr material, SceneBlendFactor sourceFactor, SceneBlendFactor destFactor);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Material_setSeparateSceneBlending2(IntPtr material, SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha);

#endregion
    }
}
