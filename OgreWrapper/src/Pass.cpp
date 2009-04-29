#include "StdAfx.h"
#include "..\include\Pass.h"

#include "OgrePass.h"
#include "MathUtils.h"
#include "MarshalUtils.h"

namespace OgreWrapper{

Pass::Pass(Ogre::Pass* ogrePass, Technique^ parent)
:ogrePass( ogrePass ), parent(parent)
{

}

Pass::~Pass()
{
	
}

Ogre::Pass* Pass::getPass()
{
	return ogrePass;
}

bool Pass::isProgrammable()
{
	return ogrePass->isProgrammable();
}

bool Pass::hasVertexProgram()
{
	return ogrePass->hasVertexProgram();
}

bool Pass::hasFragmentProgram()
{
	return ogrePass->hasFragmentProgram();
}

bool Pass::hasGeometryProgram()
{
	return ogrePass->hasGeometryProgram();
}

bool Pass::hasShadowCasterVertexProgram()
{
	return ogrePass->hasShadowCasterVertexProgram();
}

bool Pass::hasShadowReceiverVertexProgram()
{
	return ogrePass->hasShadowReceiverVertexProgram();
}

bool Pass::hasShadowReceiverFragmentProgram()
{
	return ogrePass->hasShadowReceiverFragmentProgram();
}

unsigned short Pass::getIndex()
{
	return ogrePass->getIndex();
}

void Pass::setName(System::String^ name)
{
	return ogrePass->setName(MarshalUtils::convertString(name));
}

System::String^ Pass::getName()
{
	return MarshalUtils::convertString(ogrePass->getName());
}

void Pass::setAmbient(float red, float green, float blue)
{
	return ogrePass->setAmbient(red, green, blue);
}

void Pass::setAmbient(Engine::Color color)
{
	return ogrePass->setAmbient(MathUtils::copyColor(color));
}

void Pass::setAmbient(Engine::Color% color)
{
	return ogrePass->setAmbient(MathUtils::copyColor(color));
}

void Pass::setDiffuse(float red, float green, float blue, float alpha)
{
	return ogrePass->setDiffuse(red, green, blue, alpha);
}

void Pass::setDiffuse(Engine::Color color)
{
	return ogrePass->setDiffuse(MathUtils::copyColor(color));
}

void Pass::setDiffuse(Engine::Color% color)
{
	return ogrePass->setDiffuse(MathUtils::copyColor(color));
}

void Pass::setSpecular(float red, float green, float blue, float alpha)
{
	return ogrePass->setSpecular(red, green, blue, alpha);
}

void Pass::setSpecular(Engine::Color color)
{
	return ogrePass->setSpecular(MathUtils::copyColor(color));
}

void Pass::setSpecular(Engine::Color% color)
{
	return ogrePass->setSpecular(MathUtils::copyColor(color));
}

void Pass::setShininess(float value)
{
	return ogrePass->setShininess(value);
}

void Pass::setSelfIllumination(float red, float green, float blue)
{
	return ogrePass->setSelfIllumination(red, green, blue);
}

void Pass::setSelfIllumination(Engine::Color color)
{
	return ogrePass->setSelfIllumination(MathUtils::copyColor(color));
}

void Pass::setSelfIllumination(Engine::Color% color)
{
	return ogrePass->setSelfIllumination(MathUtils::copyColor(color));
}

void Pass::setVertexColorTracking(TrackVertexColorEnum tracking)
{
	return ogrePass->setVertexColourTracking((Ogre::TrackVertexColourEnum)tracking);
}

float Pass::getPointSize()
{
	return ogrePass->getPointSize();
}

void Pass::setPointSize(float ps)
{
	return ogrePass->setPointSize(ps);
}

void Pass::setPointSpritesEnabled(bool enabled)
{
	return ogrePass->setPointSpritesEnabled(enabled);
}

bool Pass::getPointSpritesEnabled()
{
	return ogrePass->getPointSpritesEnabled();
}

void Pass::setPointAttenuation(bool enabled)
{
	return ogrePass->setPointAttenuation(enabled);
}

void Pass::setPointAttenuation(bool enabled, float constant, float linear, float quadratic)
{
	return ogrePass->setPointAttenuation(enabled, constant, linear, quadratic);
}

bool Pass::isPointAttenuationEnabled()
{
	return ogrePass->isPointAttenuationEnabled();
}

float Pass::getPointAttenuationConstant()
{
	return ogrePass->getPointAttenuationConstant();
}

float Pass::getPointAttenuationLinear()
{
	return ogrePass->getPointAttenuationLinear();
}

float Pass::getPointAttenuationQuadratic()
{
	return ogrePass->getPointAttenuationQuadratic();
}

void Pass::setPointMinSize(float min)
{
	return ogrePass->setPointMinSize(min);
}

float Pass::getPointMinSize()
{
	return ogrePass->getPointMinSize();
}

void Pass::setPointMaxSize(float max)
{
	return ogrePass->setPointMaxSize(max);
}

float Pass::getPointMaxSize()
{
	return ogrePass->getPointMaxSize();
}

Engine::Color Pass::getAmbient()
{
	return MathUtils::copyColor(ogrePass->getAmbient());
}

Engine::Color Pass::getDiffuse()
{
	return MathUtils::copyColor(ogrePass->getDiffuse());
}

Engine::Color Pass::getSpecular()
{
	return MathUtils::copyColor(ogrePass->getSpecular());
}

Engine::Color Pass::getSelfIllumination()
{
	return MathUtils::copyColor(ogrePass->getSelfIllumination());
}

float Pass::getShininess()
{
	return ogrePass->getShininess();
}

TrackVertexColorEnum Pass::getVertexColorTracking()
{
	return (TrackVertexColorEnum)ogrePass->getVertexColourTracking();
}

void Pass::setSceneBlending(SceneBlendType sbt)
{
	return ogrePass->setSceneBlending((Ogre::SceneBlendType)sbt);
}

void Pass::setSeparateSceneBlending(SceneBlendType sbt, SceneBlendType sbta)
{
	return ogrePass->setSeparateSceneBlending((Ogre::SceneBlendType)sbt, (Ogre::SceneBlendType)sbta);
}

void Pass::setSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor)
{
	return ogrePass->setSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor);
}

void Pass::setSeparateSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha)
{
	return ogrePass->setSeparateSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor, (Ogre::SceneBlendFactor)sourceFactorAlpha, (Ogre::SceneBlendFactor)destFactorAlpha);
}

bool Pass::hasSeparateSceneBlending()
{
	return ogrePass->hasSeparateSceneBlending();
}

SceneBlendFactor Pass::getSourceBlendFactor()
{
	return (SceneBlendFactor)ogrePass->getSourceBlendFactor();
}

SceneBlendFactor Pass::getDestBlendFactor()
{
	return (SceneBlendFactor)ogrePass->getDestBlendFactor();
}

SceneBlendFactor Pass::getSourceBlendFactorAlpha()
{
	return (SceneBlendFactor)ogrePass->getSourceBlendFactorAlpha();
}

SceneBlendFactor Pass::getDestBlendFactorAlpha()
{
	return (SceneBlendFactor)ogrePass->getDestBlendFactorAlpha();
}

bool Pass::isTransparent()
{
	return ogrePass->isTransparent();
}

void Pass::setDepthCheckEnabled(bool enabled)
{
	return ogrePass->setDepthCheckEnabled(enabled);
}

bool Pass::getDepthCheckEnabled()
{
	return ogrePass->getDepthCheckEnabled();
}

void Pass::setDepthWriteEnabled(bool enabled)
{
	return ogrePass->setDepthWriteEnabled(enabled);
}

bool Pass::getDepthWriteEnabled()
{
	return ogrePass->getDepthWriteEnabled();
}

void Pass::setDepthFunction(CompareFunction func)
{
	return ogrePass->setDepthFunction((Ogre::CompareFunction)func);
}

CompareFunction Pass::getDepthFunction()
{
	return (CompareFunction)ogrePass->getDepthFunction();
}

void Pass::setColorWriteEnabled(bool enabled)
{
	return ogrePass->setColourWriteEnabled(enabled);
}

bool Pass::getColorWriteEnabled()
{
	return ogrePass->getColourWriteEnabled();
}

void Pass::setCullingMode(CullingMode mode)
{
	return ogrePass->setCullingMode((Ogre::CullingMode)mode);
}

CullingMode Pass::getCullingMode()
{
	return (CullingMode)ogrePass->getCullingMode();
}

void Pass::setManualCullingMode(ManualCullingMode mode)
{
	return ogrePass->setManualCullingMode((Ogre::ManualCullingMode)mode);
}

ManualCullingMode Pass::getManualCullingMode()
{
	return (ManualCullingMode)ogrePass->getManualCullingMode();
}

void Pass::setLightingEnabled(bool enabled)
{
	return ogrePass->setLightingEnabled(enabled);
}

bool Pass::getLightingEnabled()
{
	return ogrePass->getLightingEnabled();
}

void Pass::setMaxSimultaneousLights(unsigned short max)
{
	return ogrePass->setMaxSimultaneousLights(max);
}

unsigned short Pass::getMaxSimultaneousLights()
{
	return ogrePass->getMaxSimultaneousLights();
}

void Pass::setStartLight(unsigned short startLight)
{
	return ogrePass->setStartLight(startLight);
}

unsigned short Pass::getStartLight()
{
	return ogrePass->getStartLight();
}

void Pass::setShadingMode(ShadeOptions mode)
{
	return ogrePass->setShadingMode((Ogre::ShadeOptions)mode);
}

ShadeOptions Pass::getShadingMode()
{
	return (ShadeOptions)ogrePass->getShadingMode();
}

void Pass::setPolygonMode(PolygonMode mode)
{
	return ogrePass->setPolygonMode((Ogre::PolygonMode)mode);
}

PolygonMode Pass::getPolygonMode()
{
	return (PolygonMode)ogrePass->getPolygonMode();
}

void Pass::setPolygonModeOverrideable(bool over)
{
	return ogrePass->setPolygonModeOverrideable(over);
}

bool Pass::getPolygonModeOverrideable()
{
	return ogrePass->getPolygonModeOverrideable();
}

void Pass::setFog(bool overrideScene, FogMode mode, Engine::Color% color)
{
	return ogrePass->setFog(overrideScene, (Ogre::FogMode)mode, MathUtils::copyColor(color));
}

void Pass::setFog(bool overrideScene, FogMode mode, Engine::Color% color, float expDensity, float linearStart, float linearEnd)
{
	return ogrePass->setFog(overrideScene, (Ogre::FogMode)mode, MathUtils::copyColor(color), expDensity, linearStart, linearEnd);
}

bool Pass::getFogOverride()
{
	return ogrePass->getFogOverride();
}

FogMode Pass::getFogMode()
{
	return (FogMode)ogrePass->getFogMode();
}

Engine::Color Pass::getFogColor()
{
	return MathUtils::copyColor(ogrePass->getFogColour());
}

float Pass::getFogStart()
{
	return ogrePass->getFogStart();
}

float Pass::getFogEnd()
{
	return ogrePass->getFogEnd();
}

float Pass::getFogDensity()
{
	return ogrePass->getFogDensity();
}

void Pass::setDepthBias(float bias)
{
	return ogrePass->setDepthBias(bias);
}

float Pass::getDepthBiasConstant()
{
	return ogrePass->getDepthBiasConstant();
}

float Pass::getDepthBiasSlopeScale()
{
	return ogrePass->getDepthBiasSlopeScale();
}

void Pass::setIterationDepthBias(float bias)
{
	return ogrePass->setIterationDepthBias(bias);
}

float Pass::getIterationDepthBias()
{
	return ogrePass->getIterationDepthBias();
}

void Pass::setAlphaRejectSettings(CompareFunction func, unsigned char value)
{
	return ogrePass->setAlphaRejectSettings((Ogre::CompareFunction)func, value);
}

void Pass::setAlphaRejectSettings(CompareFunction func, unsigned char value, bool alphaToCoverageEnabled)
{
	return ogrePass->setAlphaRejectSettings((Ogre::CompareFunction)func, value, alphaToCoverageEnabled);
}

void Pass::setAlphaRejectFunction(CompareFunction func)
{
	return ogrePass->setAlphaRejectFunction((Ogre::CompareFunction)func);
}

CompareFunction Pass::getAlphaRejectFunction()
{
	return (CompareFunction)ogrePass->getAlphaRejectFunction();
}

unsigned char Pass::getAlphaRejectValue()
{
	return ogrePass->getAlphaRejectValue();
}

void Pass::setAlphaToCoverageEnabled(bool enabled)
{
	return ogrePass->setAlphaToCoverageEnabled(enabled);
}

bool Pass::isAlphaToCoverageEnabled()
{
	return ogrePass->isAlphaToCoverageEnabled();
}

void Pass::setTransparentSortingEnabled(bool enabled)
{
	return ogrePass->setTransparentSortingEnabled(enabled);
}

bool Pass::getTransparentSortingEnabled()
{
	return ogrePass->getTransparentSortingEnabled();
}

void Pass::setIteratePerLight(bool enabled)
{
	return ogrePass->setIteratePerLight(enabled);
}

void Pass::setIteratePerLight(bool enabled, bool onlyForOneLightType, Light::LightTypes lightType)
{
	return ogrePass->setIteratePerLight(enabled, onlyForOneLightType, (Ogre::Light::LightTypes)lightType);
}

bool Pass::getIteratePerLight()
{
	return ogrePass->getIteratePerLight();
}

bool Pass::getRunOnlyForOneLightType()
{
	return ogrePass->getRunOnlyForOneLightType();
}

Light::LightTypes Pass::getOnlyLightType()
{
	return (Light::LightTypes)ogrePass->getOnlyLightType();
}

void Pass::setLightCountPerIteration(unsigned short c)
{
	return ogrePass->setLightCountPerIteration(c);
}

unsigned short Pass::getLightCountPerIteration()
{
	return ogrePass->getLightCountPerIteration();
}

Technique^ Pass::getParent()
{
	return parent;
}

System::String^ Pass::getResourceGroup()
{
	return MarshalUtils::convertString(ogrePass->getResourceGroup());
}

void Pass::setVertexProgram(System::String^ name)
{
	return ogrePass->setVertexProgram(MarshalUtils::convertString(name));
}

void Pass::setVertexProgram(System::String^ name, bool resetParams)
{
	return ogrePass->setVertexProgram(MarshalUtils::convertString(name), resetParams);
}

System::String^ Pass::getVertexProgramName()
{
	return MarshalUtils::convertString(ogrePass->getVertexProgramName());
}

void Pass::setShadowCasterVertexProgram(System::String^ name)
{
	return ogrePass->setShadowCasterVertexProgram(MarshalUtils::convertString(name));
}

System::String^ Pass::getShadowCasterVertexProgramName()
{
	return MarshalUtils::convertString(ogrePass->getShadowCasterVertexProgramName());
}

void Pass::setShadowReceiverVertexProgram(System::String^ name)
{
	return ogrePass->setShadowReceiverVertexProgram(MarshalUtils::convertString(name));
}

System::String^ Pass::getShadowReceiverVertexProgramName()
{
	return MarshalUtils::convertString(ogrePass->getShadowReceiverVertexProgramName());
}

void Pass::setShadowReceiverFragmentProgram(System::String^ name)
{
	return ogrePass->setShadowReceiverFragmentProgram(MarshalUtils::convertString(name));
}

System::String^ Pass::getShadowReceiverFragmentProgramName()
{
	return MarshalUtils::convertString(ogrePass->getShadowReceiverFragmentProgramName());
}

void Pass::setFragmentProgram(System::String^ name)
{
	return ogrePass->setFragmentProgram(MarshalUtils::convertString(name));
}

void Pass::setFragmentProgram(System::String^ name, bool resetParams)
{
	return ogrePass->setFragmentProgram(MarshalUtils::convertString(name), resetParams);
}

System::String^ Pass::getFragmentProgramName()
{
	return MarshalUtils::convertString(ogrePass->getFragmentProgramName());
}

void Pass::setGeometryProgram(System::String^ name)
{
	return ogrePass->setGeometryProgram(MarshalUtils::convertString(name));
}

void Pass::setGeometryProgram(System::String^ name, bool resetParams)
{
	return ogrePass->setGeometryProgram(MarshalUtils::convertString(name), resetParams);
}

System::String^ Pass::getGeometryProgramName()
{
	return MarshalUtils::convertString(ogrePass->getGeometryProgramName());
}

void Pass::setTextureFiltering(TextureFilterOptions filterType)
{
	return ogrePass->setTextureFiltering((Ogre::TextureFilterOptions)filterType);
}

void Pass::setTextureAnisotropy(unsigned int maxAniso)
{
	return ogrePass->setTextureAnisotropy(maxAniso);
}

void Pass::setNormalizeNormals(bool normalize)
{
	return ogrePass->setNormaliseNormals(normalize);
}

bool Pass::getNormalizeNormals()
{
	return ogrePass->getNormaliseNormals();
}

bool Pass::isAmbientOnly()
{
	return ogrePass->isAmbientOnly();
}

void Pass::setPassIterationCount(size_t count)
{
	return ogrePass->setPassIterationCount(count);
}

size_t Pass::getPassIterationCount()
{
	return ogrePass->getPassIterationCount();
}

void Pass::setIlluminationStage(IlluminationStage is)
{
	return ogrePass->setIlluminationStage((Ogre::IlluminationStage)is);
}

IlluminationStage Pass::getIlluminationStage()
{
	return (IlluminationStage)ogrePass->getIlluminationStage();
}

}