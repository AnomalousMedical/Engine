#include "Stdafx.h"

extern "C" _AnomalousExport bool Material_isTransparent(Ogre::Material* material)
{
	return material->isTransparent();
}

extern "C" _AnomalousExport void Material_setReceiveShadows(Ogre::Material* material, bool enabled)
{
	material->setReceiveShadows(enabled);
}

extern "C" _AnomalousExport bool Material_getReceiveShadows(Ogre::Material* material)
{
	return material->getReceiveShadows();
}

extern "C" _AnomalousExport void Material_setTransparencyCastsShadows(Ogre::Material* material, bool enabled)
{
	material->setTransparencyCastsShadows(enabled);
}

extern "C" _AnomalousExport bool Material_getTransparencyCastsShadows(Ogre::Material* material)
{
	return material->getTransparencyCastsShadows();
}

extern "C" _AnomalousExport Ogre::Technique* Material_createTechnique(Ogre::Material* material)
{
	return material->createTechnique();
}

extern "C" _AnomalousExport Ogre::Technique* Material_getTechniqueIndex(Ogre::Material* material, ushort index)
{
	return material->getTechnique(index);
}

extern "C" _AnomalousExport Ogre::Technique* Material_getTechnique(Ogre::Material* material, String name)
{
	return material->getTechnique(name);
}

extern "C" _AnomalousExport ushort Material_getNumTechniques(Ogre::Material* material)
{
	return material->getNumTechniques();
}

extern "C" _AnomalousExport void Material_removeTechnique(Ogre::Material* material, ushort index)
{
	material->removeTechnique(index);
}

extern "C" _AnomalousExport void Material_removeAllTechniques(Ogre::Material* material)
{
	material->removeAllTechniques();
}

extern "C" _AnomalousExport Ogre::Technique* Material_getSupportedTechnique(Ogre::Material* material, ushort index)
{
	return material->getSupportedTechnique(index);
}

extern "C" _AnomalousExport ushort Material_getNumSupportedTechniques(Ogre::Material* material)
{
	return material->getNumSupportedTechniques();
}

extern "C" _AnomalousExport String Material_getUnsupportedTechniquesExplanation(Ogre::Material* material)
{
	return material->getUnsupportedTechniquesExplanation().c_str();
}

extern "C" _AnomalousExport ushort Material_getNumLodLevels(Ogre::Material* material, ushort schemeIndex)
{
	return material->getNumLodLevels(schemeIndex);
}

extern "C" _AnomalousExport ushort Material_getNumLodLevelsName(Ogre::Material* material, String schemeName)
{
	return material->getNumLodLevels(schemeName);
}

extern "C" _AnomalousExport Ogre::Technique* Material_getBestTechnique(Ogre::Material* material)
{
	return material->getBestTechnique();
}

extern "C" _AnomalousExport Ogre::Technique* Material_getBestTechniqueLod(Ogre::Material* material, ushort lodIndex)
{
	return material->getBestTechnique(lodIndex);
}

extern "C" _AnomalousExport Ogre::Material* Material_clone(Ogre::Material* material, String newName, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::MaterialPtr& ptr = material->clone(newName);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport Ogre::Material* Material_cloneChangeGroup(Ogre::Material* material, String newName, bool changeGroup, String newGroup, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::MaterialPtr& ptr = material->clone(newName, changeGroup, newGroup);
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport void Material_copyDetailsTo(Ogre::Material* material, Ogre::Material* materialToCopy)
{
	//material->copyDetailsTo(Ogre::MaterialPtr(materialToCopy));
}

extern "C" _AnomalousExport void Material_compile(Ogre::Material* material)
{
	material->compile();
}

extern "C" _AnomalousExport void Material_compileAutoManage(Ogre::Material* material, bool autoManageTextureUnits)
{
	material->compile(autoManageTextureUnits);
}

extern "C" _AnomalousExport void Material_setPointSize(Ogre::Material* material, float ps)
{
	material->setPointSize(ps);
}

extern "C" _AnomalousExport void Material_setAmbient(Ogre::Material* material, float red, float green, float blue)
{
	material->setAmbient(red, green, blue);
}

extern "C" _AnomalousExport void Material_setAmbientColor(Ogre::Material* material, Color color)
{
	material->setAmbient(color.toOgre());
}

extern "C" _AnomalousExport void Material_setDiffuse(Ogre::Material* material, float red, float green, float blue, float alpha)
{
	material->setDiffuse(red, green, blue, alpha);
}

extern "C" _AnomalousExport void Material_setDiffuseColor(Ogre::Material* material, Color color)
{
	material->setDiffuse(color.toOgre());
}

extern "C" _AnomalousExport void Material_setSpecular(Ogre::Material* material, float red, float green, float blue, float alpha)
{
	material->setSpecular(red, green, blue, alpha);
}

extern "C" _AnomalousExport void Material_setSpecularColor(Ogre::Material* material, Color color)
{
	material->setSpecular(color.toOgre());
}

extern "C" _AnomalousExport void Material_setShininess(Ogre::Material* material, float value)
{
	material->setShininess(value);
}

extern "C" _AnomalousExport void Material_setSelfIllumination(Ogre::Material* material, float red, float green, float blue)
{
	material->setSelfIllumination(red, green, blue);
}

extern "C" _AnomalousExport void Material_setSelfIlluminationColor(Ogre::Material* material, Color color)
{
	material->setSelfIllumination(color.toOgre());
}

extern "C" _AnomalousExport void Material_setDepthCheckEnabled(Ogre::Material* material, bool enabled)
{
	material->setDepthCheckEnabled(enabled);
}

extern "C" _AnomalousExport void Material_setDepthWriteEnabled(Ogre::Material* material, bool enabled)
{
	material->setDepthWriteEnabled(enabled);
}

extern "C" _AnomalousExport void Material_setDepthFunction(Ogre::Material* material, Ogre::CompareFunction func)
{
	material->setDepthFunction(func);
}

extern "C" _AnomalousExport void Material_setColorWriteEnabled(Ogre::Material* material, bool enabled)
{
	material->setColourWriteEnabled(enabled);
}

extern "C" _AnomalousExport void Material_setCullingMode(Ogre::Material* material, Ogre::CullingMode mode)
{
	material->setCullingMode(mode);
}

extern "C" _AnomalousExport void Material_setManualCullingMode(Ogre::Material* material, Ogre::ManualCullingMode mode)
{
	material->setManualCullingMode(mode);
}

extern "C" _AnomalousExport void Material_setLightingEnabled(Ogre::Material* material, bool enabled)
{
	material->setLightingEnabled(enabled);
}

extern "C" _AnomalousExport void Material_setShadingMode(Ogre::Material* material, Ogre::ShadeOptions mode)
{
	material->setShadingMode(mode);
}

extern "C" _AnomalousExport void Material_setFog(Ogre::Material* material, bool overrideScene, Ogre::FogMode mode, Color color)
{
	material->setFog(overrideScene, mode, color.toOgre());
}

extern "C" _AnomalousExport void Material_setFog2(Ogre::Material* material, bool overrideScene, Ogre::FogMode mode, Color color, float expDensity, float linearStart, float linearEnd)
{
	material->setFog(overrideScene, mode, color.toOgre(), expDensity, linearStart, linearEnd);
}

extern "C" _AnomalousExport void Material_setDepthBias(Ogre::Material* material, float constantBias, float slopeScaleBias)
{
	material->setDepthBias(constantBias, slopeScaleBias);
}

extern "C" _AnomalousExport void Material_setTextureFiltering(Ogre::Material* material, Ogre::TextureFilterOptions filterType)
{
	material->setTextureFiltering(filterType);
}

extern "C" _AnomalousExport void Material_setTextureAnisotropy(Ogre::Material* material, int maxAniso)
{
	material->setTextureAnisotropy(maxAniso);
}

extern "C" _AnomalousExport void Material_setSceneBlending(Ogre::Material* material, Ogre::SceneBlendType sbt)
{
	material->setSceneBlending(sbt);
}

extern "C" _AnomalousExport void Material_setSeparateSceneBlending(Ogre::Material* material, Ogre::SceneBlendType sbt, Ogre::SceneBlendType sbta)
{
	material->setSeparateSceneBlending(sbt, sbta);
}

extern "C" _AnomalousExport void Material_setSceneBlending2(Ogre::Material* material, Ogre::SceneBlendFactor sourceFactor, Ogre::SceneBlendFactor destFactor)
{
	material->setSceneBlending(sourceFactor, destFactor);
}

extern "C" _AnomalousExport void Material_setSeparateSceneBlending2(Ogre::Material* material, Ogre::SceneBlendFactor sourceFactor, Ogre::SceneBlendFactor destFactor, Ogre::SceneBlendFactor sourceFactorAlpha, Ogre::SceneBlendFactor destFactorAlpha)
{
	material->setSeparateSceneBlending(sourceFactor, destFactor, sourceFactorAlpha, destFactorAlpha);
}