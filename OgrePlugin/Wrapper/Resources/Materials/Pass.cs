using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    public enum TrackVertexColorEnum 
    {
         TVC_NONE        = 0x0,
         TVC_AMBIENT     = 0x1,        
         TVC_DIFFUSE     = 0x2,
         TVC_SPECULAR    = 0x4,
         TVC_EMISSIVE    = 0x8
    };

    /// <summary>
    /// Categorisation of passes for the purpose of additive lighting. 
    /// </summary>
    public enum IlluminationStage
    {
	    /// <summary>
	    /// Part of the rendering which occurs without any kind of direct lighting
	    /// </summary>
	    IS_AMBIENT,
	    /// <summary>
	    /// Part of the rendering which occurs per light
	    /// </summary>
	    IS_PER_LIGHT,
	    /// <summary>
	    /// Post-lighting rendering
	    /// </summary>
	    IS_DECAL, 
	    /// <summary>
	    /// Not determined
	    /// </summary>
	    IS_UNKNOWN
    };

    public class Pass : IDisposable
    {
        internal static Pass createWrapper(IntPtr nativePtr, object[] args)
        {
            return new Pass(nativePtr, args[0] as Technique);
        }

        IntPtr pass;
        Technique parent;
        WrapperCollection<TextureUnitState> textureUnits = new WrapperCollection<TextureUnitState>(TextureUnitState.createWrapper);

        private Pass(IntPtr pass, Technique parent)
        {
            this.pass = pass;
            this.parent = parent;
        }

        public void Dispose()
        {
            textureUnits.Dispose();
            pass = IntPtr.Zero;
        }

        public bool isProgrammable()
        {
            return Pass_isProgrammable(pass);
        }

        public bool hasVertexProgram()
        {
            return Pass_hasVertexProgram(pass);
        }

        public bool hasFragmentProgram()
        {
            return Pass_hasFragmentProgram(pass);
        }

        public bool hasGeometryProgram()
        {
            return Pass_hasGeometryProgram(pass);
        }

        public bool hasShadowCasterVertexProgram()
        {
            return Pass_hasShadowCasterVertexProgram(pass);
        }

        public bool hasShadowReceiverVertexProgram()
        {
            return Pass_hasShadowReceiverVertexProgram(pass);
        }

        public bool hasShadowReceiverFragmentProgram()
        {
            return Pass_hasShadowReceiverFragmentProgram(pass);
        }

        public ushort getIndex()
        {
            return Pass_getIndex(pass);
        }

        public void setName(String name)
        {
            Pass_setName(pass, name);
        }

        public String getName()
        {
            return Marshal.PtrToStringAnsi(Pass_getName(pass));
        }

        public void setAmbient(float red, float green, float blue)
        {
            Pass_setAmbientRGB(pass, red, green, blue);
        }

        public void setAmbient(Color color)
        {
            Pass_setAmbient(pass, color);
        }

        public void setDiffuse(float red, float green, float blue, float alpha)
        {
            Pass_setDiffuseRGB(pass, red, green, blue, alpha);
        }

        public void setDiffuse(Color color)
        {
            Pass_setDiffuse(pass, color);
        }

        public void setSpecular(float red, float green, float blue, float alpha)
        {
            Pass_setSpecularRGB(pass, red, green, blue, alpha);
        }

        public void setSpecular(Color color)
        {
            Pass_setSpecular(pass, color);
        }

        public void setEmissive(Color color)
        {
            Pass_setEmissive(pass, color);
        }

        public void setShininess(float value)
        {
            Pass_setShininess(pass, value);
        }

        public void setSelfIllumination(float red, float green, float blue)
        {
            Pass_setSelfIlluminationRGB(pass, red, green, blue);
        }

        public void setSelfIllumination(Color color)
        {
            Pass_setSelfIllumination(pass, color);
        }

        public void setVertexColorTracking(TrackVertexColorEnum tracking)
        {
            Pass_setVertexColorTracking(pass, tracking);
        }

        public float getPointSize()
        {
            return Pass_getPointSize(pass);
        }

        public void setPointSize(float ps)
        {
            Pass_setPointSize(pass, ps);
        }

        public void setPointSpritesEnabled(bool enabled)
        {
            Pass_setPointSpritesEnabled(pass, enabled);
        }

        public bool getPointSpritesEnabled()
        {
            return Pass_getPointSpritesEnabled(pass);
        }

        public void setPointAttenuation(bool enabled)
        {
            Pass_setPointAttenuation(pass, enabled);
        }

        public void setPointAttenuation(bool enabled, float constant, float linear, float quadratic)
        {
            Pass_setPointAttenuation2(pass, enabled, constant, linear, quadratic);
        }

        public bool isPointAttenuationEnabled()
        {
            return Pass_isPointAttenuationEnabled(pass);
        }

        public float getPointAttenuationConstant()
        {
            return Pass_getPointAttenuationConstant(pass);
        }

        public float getPointAttenuationLinear()
        {
            return Pass_getPointAttenuationLinear(pass);
        }

        public float getPointAttenuationQuadratic()
        {
            return Pass_getPointAttenuationQuadratic(pass);
        }

        public void setPointMinSize(float min)
        {
            Pass_setPointMinSize(pass, min);
        }

        public float getPointMinSize()
        {
            return Pass_getPointMinSize(pass);
        }

        public void setPointMaxSize(float max)
        {
            Pass_setPointMaxSize(pass, max);
        }

        public float getPointMaxSize()
        {
            return Pass_getPointMaxSize(pass);
        }

        public Color getAmbient()
        {
            return Pass_getAmbient(pass);
        }

        public Color getDiffuse()
        {
            return Pass_getDiffuse(pass);
        }

        public Color getSpecular()
        {
            return Pass_getSpecular(pass);
        }

        public Color getEmissive()
        {
            return Pass_getEmissive(pass);
        }

        public Color getSelfIllumination()
        {
            return Pass_getSelfIllumination(pass);
        }

        public float getShininess()
        {
            return Pass_getShininess(pass);
        }

        public TrackVertexColorEnum getVertexColorTracking()
        {
            return Pass_getVertexColorTracking(pass);
        }

        public TextureUnitState createTextureUnitState()
        {
            return textureUnits.getObject(Pass_createTextureUnitState(pass));
        }

        public TextureUnitState createTextureUnitState(String textureName)
        {
            return textureUnits.getObject(Pass_createTextureUnitStateName(pass, textureName));
        }

        public TextureUnitState createTextureUnitState(String textureName, ushort texCoordSet)
        {
            return textureUnits.getObject(Pass_createTextureUnitStateNameCoord(pass, textureName, texCoordSet));
        }

        public TextureUnitState getTextureUnitState(ushort index)
        {
            return textureUnits.getObject(Pass_getTextureUnitStateIdx(pass, index));
        }

        public TextureUnitState getTextureUnitState(String name)
        {
            return textureUnits.getObject(Pass_getTextureUnitState(pass, name));
        }

        public ushort getTextureUnitStateIndex(TextureUnitState state)
        {
            return Pass_getTextureUnitStateIndex(pass, state.OgreState);
        }

        public void removeTextureUnitState(ushort index)
        {
            textureUnits.destroyObject(Pass_getTextureUnitStateIdx(pass, index));
            Pass_removeTextureUnitState(pass, index);
        }

        public void removeAllTextureUnitStates()
        {
            textureUnits.clearObjects();
            Pass_removeAllTextureUnitStates(pass);
        }

        public ushort getNumTextureUnitStates()
        {
            return Pass_getNumTextureUnitStates(pass);
        }

        public void setSceneBlending(SceneBlendType sbt)
        {
            Pass_setSceneBlending(pass, sbt);
        }

        public void setSeparateSceneBlending(SceneBlendType sbt, SceneBlendType sbta)
        {
            Pass_setSeparateSceneBlending(pass, sbt, sbta);
        }

        public void setSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor)
        {
            Pass_setSceneBlending2(pass, sourceFactor, destFactor);
        }

        public void setSeparateSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha)
        {
            Pass_setSeparateSceneBlending2(pass, sourceFactor, destFactor, sourceFactorAlpha, destFactorAlpha);
        }

        public bool hasSeparateSceneBlending()
        {
            return Pass_hasSeparateSceneBlending(pass);
        }

        public SceneBlendFactor getSourceBlendFactor()
        {
            return Pass_getSourceBlendFactor(pass);
        }

        public SceneBlendFactor getDestBlendFactor()
        {
            return Pass_getDestBlendFactor(pass);
        }

        public SceneBlendFactor getSourceBlendFactorAlpha()
        {
            return Pass_getSourceBlendFactorAlpha(pass);
        }

        public SceneBlendFactor getDestBlendFactorAlpha()
        {
            return Pass_getDestBlendFactorAlpha(pass);
        }

        public bool isTransparent()
        {
            return Pass_isTransparent(pass);
        }

        public void setDepthCheckEnabled(bool enabled)
        {
            Pass_setDepthCheckEnabled(pass, enabled);
        }

        public bool getDepthCheckEnabled()
        {
            return Pass_getDepthCheckEnabled(pass);
        }

        public void setDepthWriteEnabled(bool enabled)
        {
            Pass_setDepthWriteEnabled(pass, enabled);
        }

        public bool getDepthWriteEnabled()
        {
            return Pass_getDepthWriteEnabled(pass);
        }

        public void setDepthFunction(CompareFunction func)
        {
            Pass_setDepthFunction(pass, func);
        }

        public CompareFunction getDepthFunction()
        {
            return Pass_getDepthFunction(pass);
        }

        public void setColorWriteEnabled(bool enabled)
        {
            Pass_setColorWriteEnabled(pass, enabled);
        }

        public bool getColorWriteEnabled()
        {
            return Pass_getColorWriteEnabled(pass);
        }

        public void setCullingMode(CullingMode mode)
        {
            Pass_setCullingMode(pass, mode);
        }

        public CullingMode getCullingMode()
        {
            return Pass_getCullingMode(pass);
        }

        public void setManualCullingMode(ManualCullingMode mode)
        {
            Pass_setManualCullingMode(pass, mode);
        }

        public ManualCullingMode getManualCullingMode()
        {
            return Pass_getManualCullingMode(pass);
        }

        public void setLightingEnabled(bool enabled)
        {
            Pass_setLightingEnabled(pass, enabled);
        }

        public bool getLightingEnabled()
        {
            return Pass_getLightingEnabled(pass);
        }

        public void setMaxSimultaneousLights(ushort max)
        {
            Pass_setMaxSimultaneousLights(pass, max);
        }

        public ushort getMaxSimultaneousLights()
        {
            return Pass_getMaxSimultaneousLights(pass);
        }

        public void setStartLight(ushort startLight)
        {
            Pass_setStartLight(pass, startLight);
        }

        public ushort getStartLight()
        {
            return Pass_getStartLight(pass);
        }

        public void setShadingMode(ShadeOptions mode)
        {
            Pass_setShadingMode(pass, mode);
        }

        public ShadeOptions getShadingMode()
        {
            return Pass_getShadingMode(pass);
        }

        public void setPolygonMode(PolygonMode mode)
        {
            Pass_setPolygonMode(pass, mode);
        }

        public PolygonMode getPolygonMode()
        {
            return Pass_getPolygonMode(pass);
        }

        public void setPolygonModeOverrideable(bool over)
        {
            Pass_setPolygonModeOverrideable(pass, over);
        }

        public bool getPolygonModeOverrideable()
        {
            return Pass_getPolygonModeOverrideable(pass);
        }

        public void setFog(bool overrideScene, FogMode mode, Color color)
        {
            Pass_setFog(pass, overrideScene, mode, color);
        }

        public void setFog(bool overrideScene, FogMode mode, Color color, float expDensity, float linearStart, float linearEnd)
        {
            Pass_setFog2(pass, overrideScene, mode, color, expDensity, linearStart, linearEnd);
        }

        public bool getFogOverride()
        {
            return Pass_getFogOverride(pass);
        }

        public FogMode getFogMode()
        {
            return Pass_getFogMode(pass);
        }

        public Color getFogColor()
        {
            return Pass_getFogColor(pass);
        }

        public float getFogStart()
        {
            return Pass_getFogStart(pass);
        }

        public float getFogEnd()
        {
            return Pass_getFogEnd(pass);
        }

        public float getFogDensity()
        {
            return Pass_getFogDensity(pass);
        }

        public void setDepthBias(float bias)
        {
            Pass_setDepthBias(pass, bias);
        }

        public float getDepthBiasConstant()
        {
            return Pass_getDepthBiasConstant(pass);
        }

        public float getDepthBiasSlopeScale()
        {
            return Pass_getDepthBiasSlopeScale(pass);
        }

        public void setIterationDepthBias(float bias)
        {
            Pass_setIterationDepthBias(pass, bias);
        }

        public float getIterationDepthBias()
        {
            return Pass_getIterationDepthBias(pass);
        }

        public void setAlphaRejectSettings(CompareFunction func, byte value)
        {
            Pass_setAlphaRejectSettings(pass, func, value);
        }

        public void setAlphaRejectSettings(CompareFunction func, byte value, bool alphaToCoverageEnabled)
        {
            Pass_setAlphaRejectSettings2(pass, func, value, alphaToCoverageEnabled);
        }

        public void setAlphaRejectFunction(CompareFunction func)
        {
            Pass_setAlphaRejectFunction(pass, func);
        }

        public CompareFunction getAlphaRejectFunction()
        {
            return Pass_getAlphaRejectFunction(pass);
        }

        public byte getAlphaRejectValue()
        {
            return Pass_getAlphaRejectValue(pass);
        }

        public void setAlphaToCoverageEnabled(bool enabled)
        {
            Pass_setAlphaToCoverageEnabled(pass, enabled);
        }

        public bool isAlphaToCoverageEnabled()
        {
            return Pass_isAlphaToCoverageEnabled(pass);
        }

        public void setTransparentSortingEnabled(bool enabled)
        {
            Pass_setTransparentSortingEnabled(pass, enabled);
        }

        public bool getTransparentSortingEnabled()
        {
            return Pass_getTransparentSortingEnabled(pass);
        }

        public void setIteratePerLight(bool enabled)
        {
            Pass_setIteratePerLight(pass, enabled);
        }

        public void setIteratePerLight(bool enabled, bool onlyForOneLightType, Light.LightTypes lightType)
        {
            Pass_setIteratePerLight2(pass, enabled, onlyForOneLightType, lightType);
        }

        public bool getIteratePerLight()
        {
            return Pass_getIteratePerLight(pass);
        }

        public bool getRunOnlyForOneLightType()
        {
            return Pass_getRunOnlyForOneLightType(pass);
        }

        public Light.LightTypes getOnlyLightType()
        {
            return Pass_getOnlyLightType(pass);
        }

        public void setLightCountPerIteration(ushort c)
        {
            Pass_setLightCountPerIteration(pass, c);
        }

        public ushort getLightCountPerIteration()
        {
            return Pass_getLightCountPerIteration(pass);
        }

        public Technique getParent()
        {
            return parent;
        }

        public String getResourceGroup()
        {
            return Marshal.PtrToStringAnsi(Pass_getResourceGroup(pass));
        }

        public void setVertexProgram(String name)
        {
            Pass_setVertexProgram(pass, name);
        }

        public void setVertexProgram(String name, bool resetParams)
        {
            Pass_setVertexProgramReset(pass, name, resetParams);
        }

        public String getVertexProgramName()
        {
            return Marshal.PtrToStringAnsi(Pass_getVertexProgramName(pass));
        }

        public void setShadowCasterVertexProgram(String name)
        {
            Pass_setShadowCasterVertexProgram(pass, name);
        }

        public String getShadowCasterVertexProgramName()
        {
            return Marshal.PtrToStringAnsi(Pass_getShadowCasterVertexProgramName(pass));
        }

        public void setShadowReceiverVertexProgram(String name)
        {
            Pass_setShadowReceiverVertexProgram(pass, name);
        }

        public String getShadowReceiverVertexProgramName()
        {
            return Marshal.PtrToStringAnsi(Pass_getShadowReceiverVertexProgramName(pass));
        }

        public void setShadowReceiverFragmentProgram(String name)
        {
            Pass_setShadowReceiverFragmentProgram(pass, name);
        }

        public String getShadowReceiverFragmentProgramName()
        {
            return Marshal.PtrToStringAnsi(Pass_getShadowReceiverFragmentProgramName(pass));
        }

        public void setFragmentProgram(String name)
        {
            Pass_setFragmentProgram(pass, name);
        }

        public void setFragmentProgram(String name, bool resetParams)
        {
            Pass_setFragmentProgramReset(pass, name, resetParams);
        }

        public String getFragmentProgramName()
        {
            return Marshal.PtrToStringAnsi(Pass_getFragmentProgramName(pass));
        }

        public void setGeometryProgram(String name)
        {
            Pass_setGeometryProgram(pass, name);
        }

        public void setGeometryProgram(String name, bool resetParams)
        {
            Pass_setGeometryProgramReset(pass, name, resetParams);
        }

        public String getGeometryProgramName()
        {
            return Marshal.PtrToStringAnsi(Pass_getGeometryProgramName(pass));
        }

        public void setTextureFiltering(TextureFilterOptions filterType)
        {
            Pass_setTextureFiltering(pass, filterType);
        }

        public void setTextureAnisotropy(uint maxAniso)
        {
            Pass_setTextureAnisotropy(pass, maxAniso);
        }

        public void setNormalizeNormals(bool normalize)
        {
            Pass_setNormalizeNormals(pass, normalize);
        }

        public bool getNormalizeNormals()
        {
            return Pass_getNormalizeNormals(pass);
        }

        public bool isAmbientOnly()
        {
            return Pass_isAmbientOnly(pass);
        }

        public void setPassIterationCount(int count)
        {
            Pass_setPassIterationCount(pass, count);
        }

        public int getPassIterationCount()
        {
            return Pass_getPassIterationCount(pass);
        }

        public void setIlluminationStage(IlluminationStage irs)
        {
            Pass_setIlluminationStage(pass, irs);
        }

        public IlluminationStage getIlluminationStage()
        {
            return Pass_getIlluminationStage(pass);
        }

        /// <summary>
        /// Get a SharedPtr to the vertex program parameters, you must dispose the returned pointer yourself.
        /// </summary>
        /// <returns>A SharedPtr to the GpuProgramParameters for this pass, you must dispose the SharedPtr instance returned.</returns>
        public GpuProgramParametersSharedPtr getVertexProgramParameters()
        {
            return GpuProgramManager.Instance.getGpuProgramParametersWrapper(Pass_getVertexProgramParameters(pass, GpuProgramManager.Instance.ProcessWrapperObjectCallback));
        }

        /// <summary>
        /// Get a SharedPtr to the fragment program parameters, you must dispose the returned pointer yourself.
        /// </summary>
        /// <returns>A SharedPtr to the GpuProgramParameters for this pass, you must dispose the SharedPtr instance returned.</returns>
        public GpuProgramParametersSharedPtr getFragmentProgramParameters()
        {
            return GpuProgramManager.Instance.getGpuProgramParametersWrapper(Pass_getFragmentProgramParameters(pass, GpuProgramManager.Instance.ProcessWrapperObjectCallback));
        }

        public IEnumerable<TextureUnitState> TextureUnitStates
        {
            get
            {
                ushort count = getNumTextureUnitStates();
                for(ushort i = 0; i < count; ++i)
                {
                    yield return getTextureUnitState(i);
                }
            }
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_isProgrammable(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_hasVertexProgram(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_hasFragmentProgram(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_hasGeometryProgram(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_hasShadowCasterVertexProgram(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_hasShadowReceiverVertexProgram(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_hasShadowReceiverFragmentProgram(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Pass_getIndex(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setName(IntPtr pass, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getName(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setAmbientRGB(IntPtr pass, float red, float green, float blue);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setAmbient(IntPtr pass, Color color);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setDiffuseRGB(IntPtr pass, float red, float green, float blue, float alpha);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setDiffuse(IntPtr pass, Color color);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setSpecularRGB(IntPtr pass, float red, float green, float blue, float alpha);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setSpecular(IntPtr pass, Color color);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Pass_setEmissive(IntPtr pass, Color color);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setShininess(IntPtr pass, float value);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setSelfIlluminationRGB(IntPtr pass, float red, float green, float blue);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setSelfIllumination(IntPtr pass, Color color);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setVertexColorTracking(IntPtr pass, TrackVertexColorEnum tracking);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getPointSize(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setPointSize(IntPtr pass, float ps);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setPointSpritesEnabled(IntPtr pass, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_getPointSpritesEnabled(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setPointAttenuation(IntPtr pass, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setPointAttenuation2(IntPtr pass, bool enabled, float constant, float linear, float quadratic);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_isPointAttenuationEnabled(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getPointAttenuationConstant(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getPointAttenuationLinear(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getPointAttenuationQuadratic(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setPointMinSize(IntPtr pass, float min);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getPointMinSize(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setPointMaxSize(IntPtr pass, float max);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getPointMaxSize(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern Color Pass_getAmbient(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern Color Pass_getDiffuse(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern Color Pass_getSpecular(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern Color Pass_getSelfIllumination(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getShininess(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern Color Pass_getEmissive(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern TrackVertexColorEnum Pass_getVertexColorTracking(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_createTextureUnitState(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_createTextureUnitStateName(IntPtr pass, String textureName);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_createTextureUnitStateNameCoord(IntPtr pass, String textureName, ushort texCoordSet);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getTextureUnitStateIdx(IntPtr pass, ushort index);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getTextureUnitState(IntPtr pass, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Pass_getTextureUnitStateIndex(IntPtr pass, IntPtr state);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_removeTextureUnitState(IntPtr pass, ushort index);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_removeAllTextureUnitStates(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Pass_getNumTextureUnitStates(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setSceneBlending(IntPtr pass, SceneBlendType sbt);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setSeparateSceneBlending(IntPtr pass, SceneBlendType sbt, SceneBlendType sbta);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setSceneBlending2(IntPtr pass, SceneBlendFactor sourceFactor, SceneBlendFactor destFactor);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setSeparateSceneBlending2(IntPtr pass, SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_hasSeparateSceneBlending(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern SceneBlendFactor Pass_getSourceBlendFactor(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern SceneBlendFactor Pass_getDestBlendFactor(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern SceneBlendFactor Pass_getSourceBlendFactorAlpha(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern SceneBlendFactor Pass_getDestBlendFactorAlpha(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_isTransparent(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern void Pass_setDepthCheckEnabled(IntPtr pass, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_getDepthCheckEnabled(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setDepthWriteEnabled(IntPtr pass, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_getDepthWriteEnabled(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setDepthFunction(IntPtr pass, CompareFunction func);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern CompareFunction Pass_getDepthFunction(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setColorWriteEnabled(IntPtr pass, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_getColorWriteEnabled(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setCullingMode(IntPtr pass, CullingMode mode);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern CullingMode Pass_getCullingMode(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setManualCullingMode(IntPtr pass, ManualCullingMode mode);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ManualCullingMode Pass_getManualCullingMode(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setLightingEnabled(IntPtr pass, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_getLightingEnabled(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setMaxSimultaneousLights(IntPtr pass, ushort max);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Pass_getMaxSimultaneousLights(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setStartLight(IntPtr pass, ushort startLight);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Pass_getStartLight(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setShadingMode(IntPtr pass, ShadeOptions mode);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ShadeOptions Pass_getShadingMode(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setPolygonMode(IntPtr pass, PolygonMode mode);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern PolygonMode Pass_getPolygonMode(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setPolygonModeOverrideable(IntPtr pass, bool over);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_getPolygonModeOverrideable(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setFog(IntPtr pass, bool overrideScene, FogMode mode, Color color);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setFog2(IntPtr pass, bool overrideScene, FogMode mode, Color color, float expDensity, float linearStart, float linearEnd);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_getFogOverride(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern FogMode Pass_getFogMode(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern Color Pass_getFogColor(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getFogStart(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getFogEnd(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getFogDensity(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setDepthBias(IntPtr pass, float bias);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getDepthBiasConstant(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getDepthBiasSlopeScale(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setIterationDepthBias(IntPtr pass, float bias);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Pass_getIterationDepthBias(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setAlphaRejectSettings(IntPtr pass, CompareFunction func, byte value);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setAlphaRejectSettings2(IntPtr pass, CompareFunction func, byte value, bool alphaToCoverageEnabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setAlphaRejectFunction(IntPtr pass, CompareFunction func);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern CompareFunction Pass_getAlphaRejectFunction(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern byte Pass_getAlphaRejectValue(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setAlphaToCoverageEnabled(IntPtr pass, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_isAlphaToCoverageEnabled(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setTransparentSortingEnabled(IntPtr pass, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_getTransparentSortingEnabled(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setIteratePerLight(IntPtr pass, bool enabled);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setIteratePerLight2(IntPtr pass, bool enabled, bool onlyForOneLightType, Light.LightTypes lightType);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_getIteratePerLight(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_getRunOnlyForOneLightType(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern Light.LightTypes Pass_getOnlyLightType(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setLightCountPerIteration(IntPtr pass, ushort c);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Pass_getLightCountPerIteration(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getResourceGroup(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setVertexProgram(IntPtr pass, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setVertexProgramReset(IntPtr pass, String name, bool resetParams);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getVertexProgramName(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setShadowCasterVertexProgram(IntPtr pass, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getShadowCasterVertexProgramName(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setShadowReceiverVertexProgram(IntPtr pass, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getShadowReceiverVertexProgramName(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setShadowReceiverFragmentProgram(IntPtr pass, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getShadowReceiverFragmentProgramName(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setFragmentProgram(IntPtr pass, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setFragmentProgramReset(IntPtr pass, String name, bool resetParams);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getFragmentProgramName(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setGeometryProgram(IntPtr pass, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setGeometryProgramReset(IntPtr pass, String name, bool resetParams);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getGeometryProgramName(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setTextureFiltering(IntPtr pass, TextureFilterOptions filterType);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setTextureAnisotropy(IntPtr pass, uint maxAniso);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setNormalizeNormals(IntPtr pass, bool normalize);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_getNormalizeNormals(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Pass_isAmbientOnly(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setPassIterationCount(IntPtr pass, int count);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern int Pass_getPassIterationCount(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Pass_setIlluminationStage(IntPtr pass, IlluminationStage irs);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IlluminationStage Pass_getIlluminationStage(IntPtr pass);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getVertexProgramParameters(IntPtr pass, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Pass_getFragmentProgramParameters(IntPtr pass, ProcessWrapperObjectDelegate processWrapper);

#endregion
    }
}
