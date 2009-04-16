#include "StdAfx.h"
#include "..\include\Technique.h"
#include "Pass.h"
#include "MarshalUtils.h"
#include "MathUtils.h"
#include "Color.h"

namespace OgreWrapper{

Technique::Technique(Ogre::Technique* ogreTechnique, Material^ parent)
:ogreTechnique(ogreTechnique),
parent(parent)
{

}

Technique::~Technique(void)
{
	ogreTechnique = 0;
}

Pass^ Technique::createPass()
{
	return passes.getObject(ogreTechnique->createPass(), this);
}

Pass^ Technique::getPass(unsigned short index)
{
	return passes.getObject(ogreTechnique->getPass(index), this);
}

Pass^ Technique::getPass(System::String^ name)
{
	return passes.getObject(ogreTechnique->getPass(MarshalUtils::convertString(name)), this);
}

unsigned short Technique::getNumPasses()
{
	return ogreTechnique->getNumPasses();
}

void Technique::removePass(unsigned short index)
{
	passes.destroyObject(ogreTechnique->getPass(index));
	return ogreTechnique->removePass(index);
}

void Technique::removeAllPasses()
{
	passes.clearObjects();
	return ogreTechnique->removeAllPasses();
}

bool Technique::movePass(unsigned short sourceIndex, unsigned short destinationIndex)
{
	return ogreTechnique->movePass(sourceIndex, destinationIndex);
}

Material^ Technique::getParent()
{
	return parent;
}

System::String^ Technique::getResourceGroup()
{
	return MarshalUtils::convertString(ogreTechnique->getResourceGroup());
}

bool Technique::isTransparent()
{
	return ogreTechnique->isTransparent();
}

bool Technique::isTransparentSortingEnabled()
{
	return ogreTechnique->isTransparentSortingEnabled();
}

Material^ Technique::getShadowCasterMaterial()
{
	throw gcnew System::NotImplementedException();
}

void Technique::setShadowCasterMaterial(Material^ material)
{
	throw gcnew System::NotImplementedException();
}

void Technique::setShadowCasterMaterial(System::String^ name)
{
	throw gcnew System::NotImplementedException();
}

Material^ Technique::getShadowReceiverMaterial()
{
	throw gcnew System::NotImplementedException();
}

void Technique::setShadowReceiverMaterial(Material^ material)
{
	throw gcnew System::NotImplementedException();
}

void Technique::ShadowReceiverMaterial(System::String^ name)
{
	throw gcnew System::NotImplementedException();
}

void Technique::setPointSize(float ps)
{
	return ogreTechnique->setPointSize(ps);
}

void Technique::setAmbient(float red, float green, float blue)
{
	return ogreTechnique->setAmbient(red, green, blue);
}

void Technique::setAmbient(Color color)
{
	return ogreTechnique->setAmbient(MathUtils::copyColor(color));
}

void Technique::setAmbient(Color% color)
{
	return ogreTechnique->setAmbient(MathUtils::copyColor(color));
}

void Technique::setDiffuse(float red, float green, float blue, float alpha)
{
	return ogreTechnique->setDiffuse(red, green, blue, alpha);
}

void Technique::setDiffuse(Color color)
{
	return ogreTechnique->setDiffuse(MathUtils::copyColor(color));
}

void Technique::setDiffuse(Color% color)
{
	return ogreTechnique->setDiffuse(MathUtils::copyColor(color));
}

void Technique::setSpecular(float red, float green, float blue, float alpha)
{
	return ogreTechnique->setSpecular(red, green, blue, alpha);
}

void Technique::setSpecular(Color color)
{
	return ogreTechnique->setSpecular(MathUtils::copyColor(color));
}

void Technique::setSpecular(Color% color)
{
	return ogreTechnique->setSpecular(MathUtils::copyColor(color));
}

void Technique::setShininess(float value)
{
	return ogreTechnique->setShininess(value);
}

void Technique::setSelfIllumination(float red, float green, float blue)
{
	return ogreTechnique->setSelfIllumination(red, green, blue);
}

void Technique::setSelfIllumination(Color color)
{
	return ogreTechnique->setSelfIllumination(MathUtils::copyColor(color));
}

void Technique::setSelfIllumination(Color% color)
{
	return ogreTechnique->setSelfIllumination(MathUtils::copyColor(color));
}

void Technique::setDepthCheckEnabled(bool enabled)
{
	return ogreTechnique->setDepthCheckEnabled(enabled);
}

void Technique::setDepthWriteEnabled(bool enabled)
{
	return ogreTechnique->setDepthWriteEnabled(enabled);
}

void Technique::setDepthFunction(CompareFunction func)
{
	return ogreTechnique->setDepthFunction((Ogre::CompareFunction)func);
}

void Technique::setColorWriteEnabled(bool enabled)
{
	return ogreTechnique->setColourWriteEnabled(enabled);
}

void Technique::setCullingMode(CullingMode mode)
{
	return ogreTechnique->setCullingMode((Ogre::CullingMode)mode);
}

void Technique::setManualCullingMode(ManualCullingMode mode)
{
	return ogreTechnique->setManualCullingMode((Ogre::ManualCullingMode)mode);
}

void Technique::setLightingEnabled(bool enabled)
{
	return ogreTechnique->setLightingEnabled(enabled);
}

void Technique::setShadingMode(ShadeOptions mode)
{
	return ogreTechnique->setShadingMode((Ogre::ShadeOptions)mode);
}

void Technique::setFog(bool overrideScene, FogMode mode, Color% color)
{
	return ogreTechnique->setFog(overrideScene, (Ogre::FogMode)mode, MathUtils::copyColor(color));
}

void Technique::setFog(bool overrideScene, FogMode mode, Color% color, float expDensity, float linearStart, float linearEnd)
{
	return ogreTechnique->setFog(overrideScene, (Ogre::FogMode)mode, MathUtils::copyColor(color), expDensity, linearStart, linearEnd);
}

void Technique::setDepthBias(float constantBias, float slopeScaleBias)
{
	return ogreTechnique->setDepthBias(constantBias, slopeScaleBias);
}

void Technique::setTextureFiltering(TextureFilterOptions filterType)
{
	return ogreTechnique->setTextureFiltering((Ogre::TextureFilterOptions)filterType);
}

void Technique::setTextureAnisotropy(int maxAniso)
{
	return ogreTechnique->setTextureAnisotropy(maxAniso);
}

void Technique::setSceneBlending(SceneBlendType sbt)
{
	return ogreTechnique->setSceneBlending((Ogre::SceneBlendType)sbt);
}

void Technique::setSeparateSceneBlending(SceneBlendType sbt, SceneBlendType sbta)
{
	return ogreTechnique->setSeparateSceneBlending((Ogre::SceneBlendType)sbt, (Ogre::SceneBlendType)sbta);
}

void Technique::setSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor)
{
	return ogreTechnique->setSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor);
}

void Technique::setSeparateSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha)
{
	return ogreTechnique->setSeparateSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor, (Ogre::SceneBlendFactor)sourceFactorAlpha, (Ogre::SceneBlendFactor)destFactorAlpha);
}

void Technique::setLodIndex(unsigned short index)
{
	return ogreTechnique->setLodIndex(index);
}

unsigned short Technique::getLodIndex()
{
	return ogreTechnique->getLodIndex();
}

void Technique::setSchemeName(System::String^ schemeName)
{
	return ogreTechnique->setSchemeName(MarshalUtils::convertString(schemeName));
}

System::String^ Technique::getSchemeName()
{
	return MarshalUtils::convertString(ogreTechnique->getSchemeName());
}

bool Technique::isDepthWriteEnabled()
{
	return ogreTechnique->isDepthWriteEnabled();
}

bool Technique::isDepthCheckEnabled()
{
	return ogreTechnique->isDepthCheckEnabled();
}

bool Technique::hasColorWriteDisabled()
{
	return ogreTechnique->hasColourWriteDisabled();
}

void Technique::setName(System::String^ name)
{
	return ogreTechnique->setName(MarshalUtils::convertString(name));
}

System::String^ Technique::getName()
{
	return MarshalUtils::convertString(ogreTechnique->getName());
}

}