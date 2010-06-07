#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport bool Pass_isProgrammable(Ogre::Pass* pass)
{
	return pass->isProgrammable();
}

extern "C" _AnomalousExport bool Pass_hasVertexProgram(Ogre::Pass* pass)
{
	return pass->hasVertexProgram();
}

extern "C" _AnomalousExport bool Pass_hasFragmentProgram(Ogre::Pass* pass)
{
	return pass->hasFragmentProgram();
}

extern "C" _AnomalousExport bool Pass_hasGeometryProgram(Ogre::Pass* pass)
{
	return pass->hasGeometryProgram();
}

extern "C" _AnomalousExport bool Pass_hasShadowCasterVertexProgram(Ogre::Pass* pass)
{
	return pass->hasShadowCasterVertexProgram();
}

extern "C" _AnomalousExport bool Pass_hasShadowReceiverVertexProgram(Ogre::Pass* pass)
{
	return pass->hasShadowReceiverVertexProgram();
}

extern "C" _AnomalousExport bool Pass_hasShadowReceiverFragmentProgram(Ogre::Pass* pass)
{
	return pass->hasShadowReceiverFragmentProgram();
}

extern "C" _AnomalousExport ushort Pass_getIndex(Ogre::Pass* pass)
{
	return pass->getIndex();
}

extern "C" _AnomalousExport void Pass_setName(Ogre::Pass* pass, String name)
{
	pass->setName(name);
}

extern "C" _AnomalousExport String Pass_getName(Ogre::Pass* pass)
{
	return pass->getName().c_str();
}

extern "C" _AnomalousExport void Pass_setAmbientRGB(Ogre::Pass* pass, float red, float green, float blue)
{
	pass->setAmbient(red, green, blue);
}

extern "C" _AnomalousExport void Pass_setAmbient(Ogre::Pass* pass, Color color)
{
	pass->setAmbient(color.toOgre());
}

extern "C" _AnomalousExport void Pass_setDiffuseRGB(Ogre::Pass* pass, float red, float green, float blue, float alpha)
{
	pass->setDiffuse(red, green, blue, alpha);
}

extern "C" _AnomalousExport void Pass_setDiffuse(Ogre::Pass* pass, Color color)
{
	pass->setDiffuse(color.toOgre());
}

extern "C" _AnomalousExport void Pass_setSpecularRGB(Ogre::Pass* pass, float red, float green, float blue, float alpha)
{
	pass->setSpecular(red, green, blue, alpha);
}

extern "C" _AnomalousExport void Pass_setSpecular(Ogre::Pass* pass, Color color)
{
	pass->setSpecular(color.toOgre());
}

extern "C" _AnomalousExport void Pass_setShininess(Ogre::Pass* pass, float value)
{
	pass->setShininess(value);
}

extern "C" _AnomalousExport void Pass_setSelfIlluminationRGB(Ogre::Pass* pass, float red, float green, float blue)
{
	pass->setSelfIllumination(red, green, blue);
}

extern "C" _AnomalousExport void Pass_setSelfIllumination(Ogre::Pass* pass, Color color)
{
	pass->setSelfIllumination(color.toOgre());
}

extern "C" _AnomalousExport void Pass_setVertexColorTracking(Ogre::Pass* pass, Ogre::TrackVertexColourEnum tracking)
{
	pass->setVertexColourTracking(tracking);
}

extern "C" _AnomalousExport float Pass_getPointSize(Ogre::Pass* pass)
{
	return pass->getPointSize();
}

extern "C" _AnomalousExport void Pass_setPointSize(Ogre::Pass* pass, float ps)
{
	pass->setPointSize(ps);
}

extern "C" _AnomalousExport void Pass_setPointSpritesEnabled(Ogre::Pass* pass, bool enabled)
{
	pass->setPointSpritesEnabled(enabled);
}

extern "C" _AnomalousExport bool Pass_getPointSpritesEnabled(Ogre::Pass* pass)
{
	return pass->getPointSpritesEnabled();
}

extern "C" _AnomalousExport void Pass_setPointAttenuation(Ogre::Pass* pass, bool enabled)
{
	pass->setPointAttenuation(enabled);
}

extern "C" _AnomalousExport void Pass_setPointAttenuation2(Ogre::Pass* pass, bool enabled, float constant, float linear, float quadratic)
{
	pass->setPointAttenuation(enabled, constant, linear, quadratic);
}

extern "C" _AnomalousExport bool Pass_isPointAttenuationEnabled(Ogre::Pass* pass)
{
	return pass->isPointAttenuationEnabled();
}

extern "C" _AnomalousExport float Pass_getPointAttenuationConstant(Ogre::Pass* pass)
{
	return pass->getPointAttenuationConstant();
}

extern "C" _AnomalousExport float Pass_getPointAttenuationLinear(Ogre::Pass* pass)
{
	return pass->getPointAttenuationLinear();
}

extern "C" _AnomalousExport float Pass_getPointAttenuationQuadratic(Ogre::Pass* pass)
{
	return pass->getPointAttenuationQuadratic();
}

extern "C" _AnomalousExport void Pass_setPointMinSize(Ogre::Pass* pass, float min)
{
	return pass->setPointMinSize(min);
}

extern "C" _AnomalousExport float Pass_getPointMinSize(Ogre::Pass* pass)
{
	return pass->getPointMinSize();
}

extern "C" _AnomalousExport void Pass_setPointMaxSize(Ogre::Pass* pass, float max)
{
	pass->setPointMaxSize(max);
}

extern "C" _AnomalousExport float Pass_getPointMaxSize(Ogre::Pass* pass)
{
	return pass->getPointMaxSize();
}

extern "C" _AnomalousExport Color Pass_getAmbient(Ogre::Pass* pass)
{
	return pass->getAmbient();
}

extern "C" _AnomalousExport Color Pass_getDiffuse(Ogre::Pass* pass)
{
	return pass->getDiffuse();
}

extern "C" _AnomalousExport Color Pass_getSpecular(Ogre::Pass* pass)
{
	return pass->getSpecular();
}

extern "C" _AnomalousExport Color Pass_getSelfIllumination(Ogre::Pass* pass)
{
	return pass->getSelfIllumination();
}

extern "C" _AnomalousExport float Pass_getShininess(Ogre::Pass* pass)
{
	return pass->getShininess();
}

extern "C" _AnomalousExport Ogre::TrackVertexColourEnum Pass_getVertexColorTracking(Ogre::Pass* pass)
{
	return static_cast<Ogre::TrackVertexColourEnum>(pass->getVertexColourTracking());
}

extern "C" _AnomalousExport Ogre::TextureUnitState* Pass_createTextureUnitState(Ogre::Pass* pass)
{
	return pass->createTextureUnitState();
}

extern "C" _AnomalousExport Ogre::TextureUnitState* Pass_createTextureUnitStateName(Ogre::Pass* pass, String textureName)
{
	return pass->createTextureUnitState(textureName);
}

extern "C" _AnomalousExport Ogre::TextureUnitState* Pass_createTextureUnitStateNameCoord(Ogre::Pass* pass, String textureName, ushort texCoordSet)
{
	return pass->createTextureUnitState(textureName, texCoordSet);
}

extern "C" _AnomalousExport Ogre::TextureUnitState* Pass_getTextureUnitStateIdx(Ogre::Pass* pass, ushort index)
{
	return pass->getTextureUnitState(index);
}

extern "C" _AnomalousExport Ogre::TextureUnitState* Pass_getTextureUnitState(Ogre::Pass* pass, String name)
{
	return pass->getTextureUnitState(name);
}

extern "C" _AnomalousExport ushort Pass_getTextureUnitStateIndex(Ogre::Pass* pass, Ogre::TextureUnitState* state)
{
	return pass->getTextureUnitStateIndex(state);
}

extern "C" _AnomalousExport void Pass_removeTextureUnitState(Ogre::Pass* pass, ushort index)
{
	pass->removeTextureUnitState(index);
}

extern "C" _AnomalousExport void Pass_removeAllTextureUnitStates(Ogre::Pass* pass)
{
	pass->removeAllTextureUnitStates();
}

extern "C" _AnomalousExport ushort Pass_getNumTextureUnitStates(Ogre::Pass* pass)
{
	return pass->getNumTextureUnitStates();
}

extern "C" _AnomalousExport void Pass_setSceneBlending(Ogre::Pass* pass, Ogre::SceneBlendType sbt)
{
	pass->setSceneBlending(sbt);
}

extern "C" _AnomalousExport void Pass_setSeparateSceneBlending(Ogre::Pass* pass, Ogre::SceneBlendType sbt, Ogre::SceneBlendType sbta)
{
	pass->setSeparateSceneBlending(sbt, sbta);
}

extern "C" _AnomalousExport void Pass_setSceneBlending2(Ogre::Pass* pass, Ogre::SceneBlendFactor sourceFactor, Ogre::SceneBlendFactor destFactor)
{
	pass->setSceneBlending(sourceFactor, destFactor);
}

extern "C" _AnomalousExport void Pass_setSeparateSceneBlending2(Ogre::Pass* pass, Ogre::SceneBlendFactor sourceFactor, Ogre::SceneBlendFactor destFactor, Ogre::SceneBlendFactor sourceFactorAlpha, Ogre::SceneBlendFactor destFactorAlpha)
{
	pass->setSeparateSceneBlending(sourceFactor, destFactor, sourceFactorAlpha, destFactorAlpha);
}

extern "C" _AnomalousExport bool Pass_hasSeparateSceneBlending(Ogre::Pass* pass)
{
	return pass->hasSeparateSceneBlending();
}

extern "C" _AnomalousExport Ogre::SceneBlendFactor Pass_getSourceBlendFactor(Ogre::Pass* pass)
{
	return pass->getSourceBlendFactor();
}

extern "C" _AnomalousExport Ogre::SceneBlendFactor Pass_getDestBlendFactor(Ogre::Pass* pass)
{
	return pass->getDestBlendFactor();
}

extern "C" _AnomalousExport Ogre::SceneBlendFactor Pass_getSourceBlendFactorAlpha(Ogre::Pass* pass)
{
	return pass->getSourceBlendFactorAlpha();
}

extern "C" _AnomalousExport Ogre::SceneBlendFactor Pass_getDestBlendFactorAlpha(Ogre::Pass* pass)
{
	return pass->getDestBlendFactorAlpha();
}

extern "C" _AnomalousExport bool Pass_isTransparent(Ogre::Pass* pass)
{
	return pass->isTransparent();
}

extern "C" _AnomalousExport void Pass_setDepthCheckEnabled(Ogre::Pass* pass, bool enabled)
{
	pass->setDepthCheckEnabled(enabled);
}

extern "C" _AnomalousExport bool Pass_getDepthCheckEnabled(Ogre::Pass* pass)
{
	return pass->getDepthCheckEnabled();
}

extern "C" _AnomalousExport void Pass_setDepthWriteEnabled(Ogre::Pass* pass, bool enabled)
{
	pass->setDepthWriteEnabled(enabled);
}

extern "C" _AnomalousExport bool Pass_getDepthWriteEnabled(Ogre::Pass* pass)
{
	return pass->getDepthWriteEnabled();
}

extern "C" _AnomalousExport void Pass_setDepthFunction(Ogre::Pass* pass, Ogre::CompareFunction func)
{
	pass->setDepthFunction(func);
}

extern "C" _AnomalousExport Ogre::CompareFunction Pass_getDepthFunction(Ogre::Pass* pass)
{
	return pass->getDepthFunction();
}

extern "C" _AnomalousExport void Pass_setColorWriteEnabled(Ogre::Pass* pass, bool enabled)
{
	pass->setColourWriteEnabled(enabled);
}

extern "C" _AnomalousExport bool Pass_getColorWriteEnabled(Ogre::Pass* pass)
{
	return pass->getColourWriteEnabled();
}

extern "C" _AnomalousExport void Pass_setCullingMode(Ogre::Pass* pass, Ogre::CullingMode mode)
{
	pass->setCullingMode(mode);
}

extern "C" _AnomalousExport Ogre::CullingMode Pass_getCullingMode(Ogre::Pass* pass)
{
	return pass->getCullingMode();
}

extern "C" _AnomalousExport void Pass_setManualCullingMode(Ogre::Pass* pass, Ogre::ManualCullingMode mode)
{
	pass->setManualCullingMode(mode);
}

extern "C" _AnomalousExport Ogre::ManualCullingMode Pass_getManualCullingMode(Ogre::Pass* pass)
{
	return pass->getManualCullingMode();
}

extern "C" _AnomalousExport void Pass_setLightingEnabled(Ogre::Pass* pass, bool enabled)
{
	pass->setLightingEnabled(enabled);
}

extern "C" _AnomalousExport bool Pass_getLightingEnabled(Ogre::Pass* pass)
{
	return pass->getLightingEnabled();
}

extern "C" _AnomalousExport void Pass_setMaxSimultaneousLights(Ogre::Pass* pass, ushort max)
{
	pass->setMaxSimultaneousLights(max);
}

extern "C" _AnomalousExport ushort Pass_getMaxSimultaneousLights(Ogre::Pass* pass)
{
	return pass->getMaxSimultaneousLights();
}

extern "C" _AnomalousExport void Pass_setStartLight(Ogre::Pass* pass, ushort startLight)
{
	pass->setStartLight(startLight);
}

extern "C" _AnomalousExport ushort Pass_getStartLight(Ogre::Pass* pass)
{
	return pass->getStartLight();
}

extern "C" _AnomalousExport void Pass_setShadingMode(Ogre::Pass* pass, Ogre::ShadeOptions mode)
{
	pass->setShadingMode(mode);
}

extern "C" _AnomalousExport Ogre::ShadeOptions Pass_getShadingMode(Ogre::Pass* pass)
{
	return pass->getShadingMode();
}

extern "C" _AnomalousExport void Pass_setPolygonMode(Ogre::Pass* pass, Ogre::PolygonMode mode)
{
	pass->setPolygonMode(mode);
}

extern "C" _AnomalousExport Ogre::PolygonMode Pass_getPolygonMode(Ogre::Pass* pass)
{
	return pass->getPolygonMode();
}

extern "C" _AnomalousExport void Pass_setPolygonModeOverrideable(Ogre::Pass* pass, bool over)
{
	pass->setPolygonModeOverrideable(over);
}

extern "C" _AnomalousExport bool Pass_getPolygonModeOverrideable(Ogre::Pass* pass)
{
	return pass->getPolygonModeOverrideable();
}

extern "C" _AnomalousExport void Pass_setFog(Ogre::Pass* pass, bool overrideScene, Ogre::FogMode mode, Color color)
{
	pass->setFog(overrideScene, mode, color.toOgre());
}

extern "C" _AnomalousExport void Pass_setFog2(Ogre::Pass* pass, bool overrideScene, Ogre::FogMode mode, Color color, float expDensity, float linearStart, float linearEnd)
{
	pass->setFog(overrideScene, mode, color.toOgre(), expDensity, linearStart, linearEnd);
}

extern "C" _AnomalousExport bool Pass_getFogOverride(Ogre::Pass* pass)
{
	return pass->getFogOverride();
}

extern "C" _AnomalousExport Ogre::FogMode Pass_getFogMode(Ogre::Pass* pass)
{
	return pass->getFogMode();
}

extern "C" _AnomalousExport Color Pass_getFogColor(Ogre::Pass* pass)
{
	return pass->getFogColour();
}

extern "C" _AnomalousExport float Pass_getFogStart(Ogre::Pass* pass)
{
	return pass->getFogStart();
}

extern "C" _AnomalousExport float Pass_getFogEnd(Ogre::Pass* pass)
{
	return pass->getFogEnd();
}

extern "C" _AnomalousExport float Pass_getFogDensity(Ogre::Pass* pass)
{
	return pass->getFogDensity();
}

extern "C" _AnomalousExport void Pass_setDepthBias(Ogre::Pass* pass, float bias)
{
	pass->setDepthBias(bias);
}

extern "C" _AnomalousExport float Pass_getDepthBiasConstant(Ogre::Pass* pass)
{
	return pass->getDepthBiasConstant();
}

extern "C" _AnomalousExport float Pass_getDepthBiasSlopeScale(Ogre::Pass* pass)
{
	return pass->getDepthBiasSlopeScale();
}

extern "C" _AnomalousExport void Pass_setIterationDepthBias(Ogre::Pass* pass, float bias)
{
	pass->setIterationDepthBias(bias);
}

extern "C" _AnomalousExport float Pass_getIterationDepthBias(Ogre::Pass* pass)
{
	return pass->getIterationDepthBias();
}

extern "C" _AnomalousExport void Pass_setAlphaRejectSettings(Ogre::Pass* pass, Ogre::CompareFunction func, byte value)
{
	pass->setAlphaRejectSettings(func, value);
}

extern "C" _AnomalousExport void Pass_setAlphaRejectSettings2(Ogre::Pass* pass, Ogre::CompareFunction func, byte value, bool alphaToCoverageEnabled)
{
	pass->setAlphaRejectSettings(func, value, alphaToCoverageEnabled);
}

extern "C" _AnomalousExport void Pass_setAlphaRejectFunction(Ogre::Pass* pass, Ogre::CompareFunction func)
{
	pass->setAlphaRejectFunction(func);
}

extern "C" _AnomalousExport Ogre::CompareFunction Pass_getAlphaRejectFunction(Ogre::Pass* pass)
{
	return pass->getAlphaRejectFunction();
}

extern "C" _AnomalousExport byte Pass_getAlphaRejectValue(Ogre::Pass* pass)
{
	return pass->getAlphaRejectValue();
}

extern "C" _AnomalousExport void Pass_setAlphaToCoverageEnabled(Ogre::Pass* pass, bool enabled)
{
	pass->setAlphaToCoverageEnabled(enabled);
}

extern "C" _AnomalousExport bool Pass_isAlphaToCoverageEnabled(Ogre::Pass* pass)
{
	return pass->isAlphaToCoverageEnabled();
}

extern "C" _AnomalousExport void Pass_setTransparentSortingEnabled(Ogre::Pass* pass, bool enabled)
{
	pass->setTransparentSortingEnabled(enabled);
}

extern "C" _AnomalousExport bool Pass_getTransparentSortingEnabled(Ogre::Pass* pass)
{
	return pass->getTransparentSortingEnabled();
}

extern "C" _AnomalousExport void Pass_setIteratePerLight(Ogre::Pass* pass, bool enabled)
{
	pass->setIteratePerLight(enabled);
}

extern "C" _AnomalousExport void Pass_setIteratePerLight2(Ogre::Pass* pass, bool enabled, bool onlyForOneLightType, Ogre::Light::LightTypes lightType)
{
	pass->setIteratePerLight(enabled, onlyForOneLightType, lightType);
}

extern "C" _AnomalousExport bool Pass_getIteratePerLight(Ogre::Pass* pass)
{
	return pass->getIteratePerLight();
}

extern "C" _AnomalousExport bool Pass_getRunOnlyForOneLightType(Ogre::Pass* pass)
{
	return pass->getRunOnlyForOneLightType();
}

extern "C" _AnomalousExport Ogre::Light::LightTypes Pass_getOnlyLightType(Ogre::Pass* pass)
{
	return pass->getOnlyLightType();
}

extern "C" _AnomalousExport void Pass_setLightCountPerIteration(Ogre::Pass* pass, ushort c)
{
	pass->setLightCountPerIteration(c);
}

extern "C" _AnomalousExport ushort Pass_getLightCountPerIteration(Ogre::Pass* pass)
{
	return pass->getLightCountPerIteration();
}

extern "C" _AnomalousExport String Pass_getResourceGroup(Ogre::Pass* pass)
{
	return pass->getResourceGroup().c_str();
}

extern "C" _AnomalousExport void Pass_setVertexProgram(Ogre::Pass* pass, String name)
{
	pass->setVertexProgram(name);
}

extern "C" _AnomalousExport void Pass_setVertexProgramReset(Ogre::Pass* pass, String name, bool resetParams)
{
	pass->setVertexProgram(name, resetParams);
}

extern "C" _AnomalousExport String Pass_getVertexProgramName(Ogre::Pass* pass)
{
	return pass->getVertexProgramName().c_str();
}

extern "C" _AnomalousExport void Pass_setShadowCasterVertexProgram(Ogre::Pass* pass, String name)
{
	pass->setShadowCasterVertexProgram(name);
}

extern "C" _AnomalousExport String Pass_getShadowCasterVertexProgramName(Ogre::Pass* pass)
{
	return pass->getShadowCasterVertexProgramName().c_str();
}

extern "C" _AnomalousExport void Pass_setShadowReceiverVertexProgram(Ogre::Pass* pass, String name)
{
	pass->setShadowReceiverVertexProgram(name);
}

extern "C" _AnomalousExport String Pass_getShadowReceiverVertexProgramName(Ogre::Pass* pass)
{
	return pass->getShadowReceiverVertexProgramName().c_str();
}

extern "C" _AnomalousExport void Pass_setShadowReceiverFragmentProgram(Ogre::Pass* pass, String name)
{
	pass->setShadowReceiverFragmentProgram(name);
}

extern "C" _AnomalousExport String Pass_getShadowReceiverFragmentProgramName(Ogre::Pass* pass)
{
	return pass->getShadowReceiverFragmentProgramName().c_str();
}

extern "C" _AnomalousExport void Pass_setFragmentProgram(Ogre::Pass* pass, String name)
{
	pass->setFragmentProgram(name);
}

extern "C" _AnomalousExport void Pass_setFragmentProgramReset(Ogre::Pass* pass, String name, bool resetParams)
{
	pass->setFragmentProgram(name, resetParams);
}

extern "C" _AnomalousExport String Pass_getFragmentProgramName(Ogre::Pass* pass)
{
	return pass->getFragmentProgramName().c_str();
}

extern "C" _AnomalousExport void Pass_setGeometryProgram(Ogre::Pass* pass, String name)
{
	pass->setGeometryProgram(name);
}

extern "C" _AnomalousExport void Pass_setGeometryProgramReset(Ogre::Pass* pass, String name, bool resetParams)
{
	pass->setGeometryProgram(name, resetParams);
}

extern "C" _AnomalousExport String Pass_getGeometryProgramName(Ogre::Pass* pass)
{
	return pass->getGeometryProgramName().c_str();
}

extern "C" _AnomalousExport void Pass_setTextureFiltering(Ogre::Pass* pass, Ogre::TextureFilterOptions filterType)
{
	pass->setTextureFiltering(filterType);
}

extern "C" _AnomalousExport void Pass_setTextureAnisotropy(Ogre::Pass* pass, uint maxAniso)
{
	pass->setTextureAnisotropy(maxAniso);
}

extern "C" _AnomalousExport void Pass_setNormalizeNormals(Ogre::Pass* pass, bool normalize)
{
	pass->setNormaliseNormals(normalize);
}

extern "C" _AnomalousExport bool Pass_getNormalizeNormals(Ogre::Pass* pass)
{
	return pass->getNormaliseNormals();
}

extern "C" _AnomalousExport bool Pass_isAmbientOnly(Ogre::Pass* pass)
{
	return pass->isAmbientOnly();
}

extern "C" _AnomalousExport void Pass_setPassIterationCount(Ogre::Pass* pass, int count)
{
	pass->setPassIterationCount(count);
}

extern "C" _AnomalousExport int Pass_getPassIterationCount(Ogre::Pass* pass)
{
	return pass->getPassIterationCount();
}

extern "C" _AnomalousExport void Pass_setIlluminationStage(Ogre::Pass* pass, Ogre::IlluminationStage irs)
{
	pass->setIlluminationStage(irs);
}

extern "C" _AnomalousExport Ogre::IlluminationStage Pass_getIlluminationStage(Ogre::Pass* pass)
{
	return pass->getIlluminationStage();
}

#pragma warning(pop)