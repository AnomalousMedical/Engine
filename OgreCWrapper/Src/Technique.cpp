#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::Pass* Technique_createPass(Ogre::Technique* technique)
{
	return technique->createPass();
}

extern "C" __declspec(dllexport) Ogre::Pass* Technique_getPass(Ogre::Technique* technique, ushort index)
{
	return technique->getPass(index);
}

extern "C" __declspec(dllexport) Ogre::Pass* Technique_getPassName(Ogre::Technique* technique, String name)
{
	return technique->getPass(name);
}

extern "C" __declspec(dllexport) ushort Technique_getNumPasses(Ogre::Technique* technique)
{
	return technique->getNumPasses();
}

extern "C" __declspec(dllexport) void Technique_removePass(Ogre::Technique* technique, ushort index)
{
	technique->removePass(index);
}

extern "C" __declspec(dllexport) void Technique_removeAllPasses(Ogre::Technique* technique)
{
	technique->removeAllPasses();
}

extern "C" __declspec(dllexport) bool Technique_movePass(Ogre::Technique* technique, ushort sourceIndex, ushort destinationIndex)
{
	return technique->movePass(sourceIndex, destinationIndex);
}

extern "C" __declspec(dllexport) String Technique_getResourceGroup(Ogre::Technique* technique)
{
	return technique->getResourceGroup().c_str();
}

extern "C" __declspec(dllexport) bool Technique_isTransparent(Ogre::Technique* technique)
{
	return technique->isTransparent();
}

extern "C" __declspec(dllexport) bool Technique_isTransparentSortingEnabled(Ogre::Technique* technique)
{
	return technique->isTransparentSortingEnabled();
}

extern "C" __declspec(dllexport) void Technique_setPointSize(Ogre::Technique* technique, float ps)
{
	technique->setPointSize(ps);
}

extern "C" __declspec(dllexport) void Technique_setAmbientRGB(Ogre::Technique* technique, float red, float green, float blue)
{
	technique->setAmbient(red, green, blue);
}

extern "C" __declspec(dllexport) void Technique_setAmbient(Ogre::Technique* technique, Color color)
{
	technique->setAmbient(color.toOgre());
}

extern "C" __declspec(dllexport) void Technique_setDiffuseRGBA(Ogre::Technique* technique, float red, float green, float blue, float alpha)
{
	technique->setDiffuse(red, green, blue, alpha);
}

extern "C" __declspec(dllexport) void Technique_setDiffuse(Ogre::Technique* technique, Color color)
{
	technique->setDiffuse(color.toOgre());
}

extern "C" __declspec(dllexport) void Technique_setSpecularRGBA(Ogre::Technique* technique, float red, float green, float blue, float alpha)
{
	technique->setSpecular(red, green, blue, alpha);
}

extern "C" __declspec(dllexport) void Technique_setSpecular(Ogre::Technique* technique, Color color)
{
	technique->setSpecular(color.toOgre());
}

extern "C" __declspec(dllexport) void Technique_setShininess(Ogre::Technique* technique, float value)
{
	technique->setShininess(value);
}

extern "C" __declspec(dllexport) void Technique_setSelfIlluminationRGB(Ogre::Technique* technique, float red, float green, float blue)
{
	technique->setSelfIllumination(red, green, blue);
}

extern "C" __declspec(dllexport) void Technique_setSelfIllumination(Ogre::Technique* technique, Color color)
{
	technique->setSelfIllumination(color.toOgre());
}

extern "C" __declspec(dllexport) void Technique_setDepthCheckEnabled(Ogre::Technique* technique, bool enabled)
{
	technique->setDepthCheckEnabled(enabled);
}

extern "C" __declspec(dllexport) void Technique_setDepthWriteEnabled(Ogre::Technique* technique, bool enabled)
{
	technique->setDepthWriteEnabled(enabled);
}

extern "C" __declspec(dllexport) void Technique_setDepthFunction(Ogre::Technique* technique, Ogre::CompareFunction func)
{
	technique->setDepthFunction(func);
}

extern "C" __declspec(dllexport) void Technique_setColorWriteEnabled(Ogre::Technique* technique, bool enabled)
{
	technique->setColourWriteEnabled(enabled);
}

extern "C" __declspec(dllexport) void Technique_setCullingMode(Ogre::Technique* technique, Ogre::CullingMode mode)
{
	technique->setCullingMode(mode);
}

extern "C" __declspec(dllexport) void Technique_setManualCullingMode(Ogre::Technique* technique, Ogre::ManualCullingMode mode)
{
	technique->setManualCullingMode(mode);
}

extern "C" __declspec(dllexport) void Technique_setLightingEnabled(Ogre::Technique* technique, bool enabled)
{
	technique->setLightingEnabled(enabled);
}

extern "C" __declspec(dllexport) void Technique_setShadingMode(Ogre::Technique* technique, Ogre::ShadeOptions mode)
{
	technique->setShadingMode(mode);
}

extern "C" __declspec(dllexport) void Technique_setFog(Ogre::Technique* technique, bool overrideScene, Ogre::FogMode mode, Color color)
{
	technique->setFog(overrideScene, mode, color.toOgre());
}

extern "C" __declspec(dllexport) void Technique_setFog2(Ogre::Technique* technique, bool overrideScene, Ogre::FogMode mode, Color color, float expDensity, float linearStart, float linearEnd)
{
	technique->setFog(overrideScene, mode, color.toOgre(), expDensity, linearStart, linearEnd);
}

extern "C" __declspec(dllexport) void Technique_setDepthBias(Ogre::Technique* technique, float constantBias, float slopeScaleBias)
{
	technique->setDepthBias(constantBias, slopeScaleBias);
}

extern "C" __declspec(dllexport) void Technique_setTextureFiltering(Ogre::Technique* technique, Ogre::TextureFilterOptions filterType)
{
	technique->setTextureFiltering(filterType);
}

extern "C" __declspec(dllexport) void Technique_setTextureAnisotropy(Ogre::Technique* technique, int maxAniso)
{
	technique->setTextureAnisotropy(maxAniso);
}

extern "C" __declspec(dllexport) void Technique_setSceneBlending(Ogre::Technique* technique, Ogre::SceneBlendType sbt)
{
	technique->setSceneBlending(sbt);
}

extern "C" __declspec(dllexport) void Technique_setSeparateSceneBlending(Ogre::Technique* technique, Ogre::SceneBlendType sbt, Ogre::SceneBlendType sbta)
{
	technique->setSeparateSceneBlending(sbt, sbta);
}

extern "C" __declspec(dllexport) void Technique_setSceneBlending2(Ogre::Technique* technique, Ogre::SceneBlendFactor sourceFactor, Ogre::SceneBlendFactor destFactor)
{
	technique->setSceneBlending(sourceFactor, destFactor);
}

extern "C" __declspec(dllexport) void Technique_setSeparateSceneBlending2(Ogre::Technique* technique, Ogre::SceneBlendFactor sourceFactor, Ogre::SceneBlendFactor destFactor, Ogre::SceneBlendFactor sourceFactorAlpha, Ogre::SceneBlendFactor destFactorAlpha)
{
	technique->setSeparateSceneBlending(sourceFactor, destFactor, sourceFactorAlpha, destFactorAlpha);
}

extern "C" __declspec(dllexport) void Technique_setLodIndex(Ogre::Technique* technique, ushort index)
{
	technique->setLodIndex(index);
}

extern "C" __declspec(dllexport) ushort Technique_getLodIndex(Ogre::Technique* technique)
{
	return technique->getLodIndex();
}

extern "C" __declspec(dllexport) void Technique_setSchemeName(Ogre::Technique* technique, String schemeName)
{
	technique->setSchemeName(schemeName);
}

extern "C" __declspec(dllexport) String Technique_getSchemeName(Ogre::Technique* technique)
{
	return technique->getSchemeName().c_str();
}

extern "C" __declspec(dllexport) bool Technique_isDepthWriteEnabled(Ogre::Technique* technique)
{
	return technique->isDepthWriteEnabled();
}

extern "C" __declspec(dllexport) bool Technique_isDepthCheckEnabled(Ogre::Technique* technique)
{
	return technique->isDepthCheckEnabled();
}

extern "C" __declspec(dllexport) bool Technique_hasColorWriteDisabled(Ogre::Technique* technique)
{
	return technique->hasColourWriteDisabled();
}

extern "C" __declspec(dllexport) void Technique_setName(Ogre::Technique* technique, String name)
{
	technique->setName(name);
}

extern "C" __declspec(dllexport) String Technique_getName(Ogre::Technique* technique)
{
	return technique->getName().c_str();
}