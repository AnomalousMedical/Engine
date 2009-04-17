#include "StdAfx.h"
#include "..\include\Material.h"
#include "Technique.h"
#include "OgreMaterial.h"
#include "MarshalUtils.h"
#include "MathUtils.h"
#include "MaterialManager.h"

namespace OgreWrapper{

Material::Material(const Ogre::MaterialPtr& ogreMaterial)
:Resource(ogreMaterial.get()),
autoMaterialPtr(new Ogre::MaterialPtr(ogreMaterial)),
ogreMaterial(ogreMaterial.getPointer())
{

}

Material::~Material(void)
{
	ogreMaterial = 0;
}

bool Material::isTransparent()
{
	return ogreMaterial->isTransparent();
}

void Material::setReceiveShadows(bool enabled)
{
	return ogreMaterial->setReceiveShadows(enabled);
}

bool Material::getReceiveShadows()
{
	return ogreMaterial->getReceiveShadows();
}

void Material::setTransparencyCastsShadows(bool enabled)
{
	return ogreMaterial->setTransparencyCastsShadows(enabled);
}

bool Material::getTransparencyCastsShadows()
{
	return ogreMaterial->getTransparencyCastsShadows();
}

Technique^ Material::createTechnique()
{
	return techniques.getObject(ogreMaterial->createTechnique(), this);
}

Technique^ Material::getTechnique(unsigned short index)
{
	return techniques.getObject(ogreMaterial->getTechnique(index), this);
}

Technique^ Material::getTechnique(System::String^ name)
{
	return techniques.getObject(ogreMaterial->getTechnique(MarshalUtils::convertString(name)), this);
}

unsigned short Material::getNumTechniques()
{
	return ogreMaterial->getNumTechniques();
}

void Material::removeTechnique(unsigned short index)
{
	techniques.destroyObject(ogreMaterial->getTechnique(index));
	return ogreMaterial->removeTechnique(index);
}

void Material::removeAllTechniques()
{
	techniques.clearObjects();
	return ogreMaterial->removeAllTechniques();
}

Technique^ Material::getSupportedTechnique(unsigned short index)
{
	return techniques.getObject(ogreMaterial->getSupportedTechnique(index), this);
}

unsigned short Material::getNumSupportedTechniques()
{
	return ogreMaterial->getNumSupportedTechniques();
}

System::String^ Material::getUnsupportedTechniquesExplanation()
{
	return MarshalUtils::convertString(ogreMaterial->getUnsupportedTechniquesExplanation());
}

unsigned short Material::getNumLodLevels(unsigned short schemeIndex)
{
	return ogreMaterial->getNumLodLevels(schemeIndex);
}

unsigned short Material::getNumLodLevels(System::String^ schemeName)
{
	return ogreMaterial->getNumLodLevels(MarshalUtils::convertString(schemeName));
}

Technique^ Material::getBestTechnique()
{
	return techniques.getObject(ogreMaterial->getBestTechnique(), this);
}

Technique^ Material::getBestTechnique(unsigned short lodIndex)
{
	return techniques.getObject(ogreMaterial->getBestTechnique(lodIndex), this);
}

MaterialPtr^ Material::clone(System::String^ newName)
{
	return MaterialManager::getInstance()->getObject(ogreMaterial->clone(MarshalUtils::convertString(newName)));
}

MaterialPtr^ Material::clone(System::String^ newName, bool changeGroup, System::String^ newGroup)
{
	return MaterialManager::getInstance()->getObject(ogreMaterial->clone(MarshalUtils::convertString(newName), changeGroup, MarshalUtils::convertString(newGroup)));
}

void Material::copyDetailsTo(Material^ material)
{
	return ogreMaterial->copyDetailsTo(Ogre::MaterialPtr(material->ogreMaterial));
}

void Material::compile()
{
	return ogreMaterial->compile();
}

void Material::compile(bool autoManageTextureUnits)
{
	return ogreMaterial->compile(autoManageTextureUnits);
}

void Material::setPointSize(float ps)
{
	return ogreMaterial->setPointSize(ps);
}

void Material::setAmbient(float red, float green, float blue)
{
	return ogreMaterial->setAmbient(red, green, blue);
}

void Material::setAmbient(EngineMath::Color color)
{
	return ogreMaterial->setAmbient(MathUtils::copyColor(color));
}

void Material::setAmbient(EngineMath::Color% color)
{
	return ogreMaterial->setAmbient(MathUtils::copyColor(color));
}

void Material::setDiffuse(float red, float green, float blue, float alpha)
{
	return ogreMaterial->setDiffuse(red, green, blue, alpha);
}

void Material::setDiffuse(EngineMath::Color color)
{
	return ogreMaterial->setDiffuse(MathUtils::copyColor(color));
}

void Material::setDiffuse(EngineMath::Color% color)
{
	return ogreMaterial->setDiffuse(MathUtils::copyColor(color));
}

void Material::setSpecular(float red, float green, float blue, float alpha)
{
	return ogreMaterial->setSpecular(red, green, blue, alpha);
}

void Material::setSpecular(EngineMath::Color color)
{
	return ogreMaterial->setSpecular(MathUtils::copyColor(color));
}

void Material::setSpecular(EngineMath::Color% color)
{
	return ogreMaterial->setSpecular(MathUtils::copyColor(color));
}

void Material::setShininess(float value)
{
	return ogreMaterial->setShininess(value);
}

void Material::setSelfIllumination(float red, float green, float blue)
{
	return ogreMaterial->setSelfIllumination(red, green, blue);
}

void Material::setSelfIllumination(EngineMath::Color color)
{
	return ogreMaterial->setSelfIllumination(MathUtils::copyColor(color));
}

void Material::setSelfIllumination(EngineMath::Color% color)
{
	return ogreMaterial->setSelfIllumination(MathUtils::copyColor(color));
}

void Material::setDepthCheckEnabled(bool enabled)
{
	return ogreMaterial->setDepthCheckEnabled(enabled);
}

void Material::setDepthWriteEnabled(bool enabled)
{
	return ogreMaterial->setDepthWriteEnabled(enabled);
}

void Material::setDepthFunction(CompareFunction func)
{
	return ogreMaterial->setDepthFunction((Ogre::CompareFunction)func);
}

void Material::setColorWriteEnabled(bool enabled)
{
	return ogreMaterial->setColourWriteEnabled(enabled);
}

void Material::setCullingMode(CullingMode mode)
{
	return ogreMaterial->setCullingMode((Ogre::CullingMode)mode);
}

void Material::setManualCullingMode(ManualCullingMode mode)
{
	return ogreMaterial->setManualCullingMode((Ogre::ManualCullingMode)mode);
}

void Material::setLightingEnabled(bool enabled)
{
	return ogreMaterial->setLightingEnabled(enabled);
}

void Material::setShadingMode(ShadeOptions mode)
{
	return ogreMaterial->setShadingMode((Ogre::ShadeOptions)mode);
}

void Material::setFog(bool overrideScene, FogMode mode, EngineMath::Color% color)
{
	return ogreMaterial->setFog(overrideScene, (Ogre::FogMode)mode, MathUtils::copyColor(color));
}

void Material::setFog(bool overrideScene, FogMode mode, EngineMath::Color% color, float expDensity, float linearStart, float linearEnd)
{
	return ogreMaterial->setFog(overrideScene, (Ogre::FogMode)mode, MathUtils::copyColor(color), expDensity, linearStart, linearEnd);
}

void Material::setDepthBias(float constantBias, float slopeScaleBias)
{
	return ogreMaterial->setDepthBias(constantBias, slopeScaleBias);
}

void Material::setTextureFiltering(TextureFilterOptions filterType)
{
	return ogreMaterial->setTextureFiltering((Ogre::TextureFilterOptions)filterType);
}

void Material::setTextureAnisotropy(int maxAniso)
{
	return ogreMaterial->setTextureAnisotropy(maxAniso);
}

void Material::setSceneBlending(SceneBlendType sbt)
{
	return ogreMaterial->setSceneBlending((Ogre::SceneBlendType)sbt);
}

void Material::setSeparateSceneBlending(SceneBlendType sbt, SceneBlendType sbta)
{
	return ogreMaterial->setSeparateSceneBlending((Ogre::SceneBlendType)sbt, (Ogre::SceneBlendType)sbta);
}

void Material::setSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor)
{
	return ogreMaterial->setSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor);
}

void Material::setSeparateSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha)
{
	return ogreMaterial->setSeparateSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor, (Ogre::SceneBlendFactor)sourceFactorAlpha, (Ogre::SceneBlendFactor)destFactorAlpha);
}

}