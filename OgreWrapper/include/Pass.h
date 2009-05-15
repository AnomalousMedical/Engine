#pragma once

#include "Light.h"
#include "TextureUnitStateCollection.h"

namespace Ogre
{
	class Pass;
}

namespace OgreWrapper
{

ref class Technique;

public enum class TrackVertexColorEnum 
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
public enum class IlluminationStage
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

/// <summary>
/// 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class Pass
{
private:
	Ogre::Pass* ogrePass;
	Technique^ parent;
	TextureUnitStateCollection textureUnitStates;

internal:
	/// <summary>
	/// Returns the native Pass
	/// </summary>
	Ogre::Pass* getPass();

public:
	/// <summary>
	/// Constructor
	/// </summary>
	Pass(Ogre::Pass* ogrePass, Technique^ parent);

	/// <summary>
	/// Destructor
	/// </summary>
	~Pass();

	bool isProgrammable();

	bool hasVertexProgram();

	bool hasFragmentProgram();

	bool hasGeometryProgram();

	bool hasShadowCasterVertexProgram();

	bool hasShadowReceiverVertexProgram();

	bool hasShadowReceiverFragmentProgram();

	unsigned short getIndex();

	void setName(System::String^ name);

	System::String^ getName();

	void setAmbient(float red, float green, float blue);

	void setAmbient(Engine::Color color);

	void setAmbient(Engine::Color% color);

	void setDiffuse(float red, float green, float blue, float alpha);

	void setDiffuse(Engine::Color color);

	void setDiffuse(Engine::Color% color);

	void setSpecular(float red, float green, float blue, float alpha);

	void setSpecular(Engine::Color color);

	void setSpecular(Engine::Color% color);

	void setShininess(float value);

	void setSelfIllumination(float red, float green, float blue);

	void setSelfIllumination(Engine::Color color);

	void setSelfIllumination(Engine::Color% color);

	void setVertexColorTracking(TrackVertexColorEnum tracking);

	float getPointSize();

	void setPointSize(float ps);

	void setPointSpritesEnabled(bool enabled);

	bool getPointSpritesEnabled();

	void setPointAttenuation(bool enabled);

	void setPointAttenuation(bool enabled, float constant, float linear, float quadratic);

	bool isPointAttenuationEnabled();

	float getPointAttenuationConstant();

	float getPointAttenuationLinear();

	float getPointAttenuationQuadratic();

	void setPointMinSize(float min);

	float getPointMinSize();

	void setPointMaxSize(float max);

	float getPointMaxSize();

	Engine::Color getAmbient();

	Engine::Color getDiffuse();

	Engine::Color getSpecular();

	Engine::Color getSelfIllumination();

	float getShininess();

	TrackVertexColorEnum getVertexColorTracking();

	TextureUnitState^ createTextureUnitState();

	TextureUnitState^ createTextureUnitState(System::String^ textureName);

	TextureUnitState^ createTextureUnitState(System::String^ textureName, unsigned short texCoordSet);

	TextureUnitState^ getTextureUnitState(unsigned short index);

	TextureUnitState^ getTextureUnitState(System::String^ name);

	unsigned short getTextureUnitStateIndex(TextureUnitState^ state);

	void removeTextureUnitState(unsigned short index);

	void removeAllTextureUnitStates();

	unsigned short getNumTextureUnitStates();

	void setSceneBlending(SceneBlendType sbt);

	void setSeparateSceneBlending(SceneBlendType sbt, SceneBlendType sbta);

	void setSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor);

	void setSeparateSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha);

	bool hasSeparateSceneBlending();

	SceneBlendFactor getSourceBlendFactor();

	SceneBlendFactor getDestBlendFactor();

	SceneBlendFactor getSourceBlendFactorAlpha();

	SceneBlendFactor getDestBlendFactorAlpha();

	bool isTransparent();

	void setDepthCheckEnabled(bool enabled);

	bool getDepthCheckEnabled();

	void setDepthWriteEnabled(bool enabled);

	bool getDepthWriteEnabled();

	void setDepthFunction(CompareFunction func);

	CompareFunction getDepthFunction();

	void setColorWriteEnabled(bool enabled);

	bool getColorWriteEnabled();

	void setCullingMode(CullingMode mode);

	CullingMode getCullingMode();

	void setManualCullingMode(ManualCullingMode mode);

	ManualCullingMode getManualCullingMode();

	void setLightingEnabled(bool enabled);

	bool getLightingEnabled();

	void setMaxSimultaneousLights(unsigned short max);

	unsigned short getMaxSimultaneousLights();

	void setStartLight(unsigned short startLight);

	unsigned short getStartLight();

	void setShadingMode(ShadeOptions mode);

	ShadeOptions getShadingMode();

	void setPolygonMode(PolygonMode mode);

	PolygonMode getPolygonMode();

	void setPolygonModeOverrideable(bool over);

	bool getPolygonModeOverrideable();

	void setFog(bool overrideScene, FogMode mode, Engine::Color% color);

	void setFog(bool overrideScene, FogMode mode, Engine::Color% color, float expDensity, float linearStart, float linearEnd);

	bool getFogOverride();

	FogMode getFogMode();

	Engine::Color getFogColor();

	float getFogStart();

	float getFogEnd();

	float getFogDensity();

	void setDepthBias(float bias);

	float getDepthBiasConstant();

	float getDepthBiasSlopeScale();

	void setIterationDepthBias(float bias);

	float getIterationDepthBias();

	void setAlphaRejectSettings(CompareFunction func, unsigned char value);

	void setAlphaRejectSettings(CompareFunction func, unsigned char value, bool alphaToCoverageEnabled);

	void setAlphaRejectFunction(CompareFunction func);

	CompareFunction getAlphaRejectFunction();

	unsigned char getAlphaRejectValue();

	void setAlphaToCoverageEnabled(bool enabled);

	bool isAlphaToCoverageEnabled();

	void setTransparentSortingEnabled(bool enabled);

	bool getTransparentSortingEnabled();

	void setIteratePerLight(bool enabled);

	void setIteratePerLight(bool enabled, bool onlyForOneLightType, Light::LightTypes lightType);

	bool getIteratePerLight();

	bool getRunOnlyForOneLightType();

	Light::LightTypes getOnlyLightType();

	void setLightCountPerIteration(unsigned short c);

	unsigned short getLightCountPerIteration();

	Technique^ getParent();

	System::String^ getResourceGroup();

	void setVertexProgram(System::String^ name);

	void setVertexProgram(System::String^ name, bool resetParams);

	System::String^ getVertexProgramName();

	void setShadowCasterVertexProgram(System::String^ name);

	System::String^ getShadowCasterVertexProgramName();

	void setShadowReceiverVertexProgram(System::String^ name);

	System::String^ getShadowReceiverVertexProgramName();

	void setShadowReceiverFragmentProgram(System::String^ name);

	System::String^ getShadowReceiverFragmentProgramName();

	void setFragmentProgram(System::String^ name);

	void setFragmentProgram(System::String^ name, bool resetParams);

	System::String^ getFragmentProgramName();

	void setGeometryProgram(System::String^ name);

	void setGeometryProgram(System::String^ name, bool resetParams);

	System::String^ getGeometryProgramName();

	void setTextureFiltering(TextureFilterOptions filterType);

	void setTextureAnisotropy(unsigned int maxAniso);

	void setNormalizeNormals(bool normalize);

	bool getNormalizeNormals();

	bool isAmbientOnly();

	void setPassIterationCount(size_t count);

	size_t getPassIterationCount();

	void setIlluminationStage(IlluminationStage is);

	IlluminationStage getIlluminationStage();
};

}