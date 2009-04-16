#include "StdAfx.h"
#include "..\include\RenderMaterial.h"
#include "Technique.h"
#include "OgreMaterial.h"
#include "MarshalUtils.h"
#include "MathUtils.h"
#include "Color.h"
#include "RenderMaterialManager.h"

namespace Rendering{

RenderMaterial::RenderMaterial(const Ogre::MaterialPtr& ogreMaterial)
:RenderResource(ogreMaterial.get()),
autoMaterialPtr(new Ogre::MaterialPtr(ogreMaterial)),
ogreMaterial(ogreMaterial.getPointer())
{

}

RenderMaterial::~RenderMaterial(void)
{
	ogreMaterial = 0;
}

bool RenderMaterial::isTransparent()
{
	return ogreMaterial->isTransparent();
}

void RenderMaterial::setReceiveShadows(bool enabled)
{
	return ogreMaterial->setReceiveShadows(enabled);
}

bool RenderMaterial::getReceiveShadows()
{
	return ogreMaterial->getReceiveShadows();
}

void RenderMaterial::setTransparencyCastsShadows(bool enabled)
{
	return ogreMaterial->setTransparencyCastsShadows(enabled);
}

bool RenderMaterial::getTransparencyCastsShadows()
{
	return ogreMaterial->getTransparencyCastsShadows();
}

Technique^ RenderMaterial::createTechnique()
{
	return techniques.getObject(ogreMaterial->createTechnique(), this);
}

Technique^ RenderMaterial::getTechnique(unsigned short index)
{
	return techniques.getObject(ogreMaterial->getTechnique(index), this);
}

Technique^ RenderMaterial::getTechnique(System::String^ name)
{
	return techniques.getObject(ogreMaterial->getTechnique(MarshalUtils::convertString(name)), this);
}

unsigned short RenderMaterial::getNumTechniques()
{
	return ogreMaterial->getNumTechniques();
}

void RenderMaterial::removeTechnique(unsigned short index)
{
	techniques.destroyObject(ogreMaterial->getTechnique(index));
	return ogreMaterial->removeTechnique(index);
}

void RenderMaterial::removeAllTechniques()
{
	techniques.clearObjects();
	return ogreMaterial->removeAllTechniques();
}

Technique^ RenderMaterial::getSupportedTechnique(unsigned short index)
{
	return techniques.getObject(ogreMaterial->getSupportedTechnique(index), this);
}

unsigned short RenderMaterial::getNumSupportedTechniques()
{
	return ogreMaterial->getNumSupportedTechniques();
}

System::String^ RenderMaterial::getUnsupportedTechniquesExplanation()
{
	return MarshalUtils::convertString(ogreMaterial->getUnsupportedTechniquesExplanation());
}

unsigned short RenderMaterial::getNumLodLevels(unsigned short schemeIndex)
{
	return ogreMaterial->getNumLodLevels(schemeIndex);
}

unsigned short RenderMaterial::getNumLodLevels(System::String^ schemeName)
{
	return ogreMaterial->getNumLodLevels(MarshalUtils::convertString(schemeName));
}

Technique^ RenderMaterial::getBestTechnique()
{
	return techniques.getObject(ogreMaterial->getBestTechnique(), this);
}

Technique^ RenderMaterial::getBestTechnique(unsigned short lodIndex)
{
	return techniques.getObject(ogreMaterial->getBestTechnique(lodIndex), this);
}

RenderMaterialPtr^ RenderMaterial::clone(System::String^ newName)
{
	return RenderMaterialManager::getInstance()->getObject(ogreMaterial->clone(MarshalUtils::convertString(newName)));
}

RenderMaterialPtr^ RenderMaterial::clone(System::String^ newName, bool changeGroup, System::String^ newGroup)
{
	return RenderMaterialManager::getInstance()->getObject(ogreMaterial->clone(MarshalUtils::convertString(newName), changeGroup, MarshalUtils::convertString(newGroup)));
}

void RenderMaterial::copyDetailsTo(RenderMaterial^ material)
{
	return ogreMaterial->copyDetailsTo(Ogre::MaterialPtr(material->ogreMaterial));
}

void RenderMaterial::compile()
{
	return ogreMaterial->compile();
}

void RenderMaterial::compile(bool autoManageTextureUnits)
{
	return ogreMaterial->compile(autoManageTextureUnits);
}

void RenderMaterial::setPointSize(float ps)
{
	return ogreMaterial->setPointSize(ps);
}

void RenderMaterial::setAmbient(float red, float green, float blue)
{
	return ogreMaterial->setAmbient(red, green, blue);
}

void RenderMaterial::setAmbient(Color color)
{
	return ogreMaterial->setAmbient(MathUtils::copyColor(color));
}

void RenderMaterial::setAmbient(Color% color)
{
	return ogreMaterial->setAmbient(MathUtils::copyColor(color));
}

void RenderMaterial::setDiffuse(float red, float green, float blue, float alpha)
{
	return ogreMaterial->setDiffuse(red, green, blue, alpha);
}

void RenderMaterial::setDiffuse(Color color)
{
	return ogreMaterial->setDiffuse(MathUtils::copyColor(color));
}

void RenderMaterial::setDiffuse(Color% color)
{
	return ogreMaterial->setDiffuse(MathUtils::copyColor(color));
}

void RenderMaterial::setSpecular(float red, float green, float blue, float alpha)
{
	return ogreMaterial->setSpecular(red, green, blue, alpha);
}

void RenderMaterial::setSpecular(Color color)
{
	return ogreMaterial->setSpecular(MathUtils::copyColor(color));
}

void RenderMaterial::setSpecular(Color% color)
{
	return ogreMaterial->setSpecular(MathUtils::copyColor(color));
}

void RenderMaterial::setShininess(float value)
{
	return ogreMaterial->setShininess(value);
}

void RenderMaterial::setSelfIllumination(float red, float green, float blue)
{
	return ogreMaterial->setSelfIllumination(red, green, blue);
}

void RenderMaterial::setSelfIllumination(Color color)
{
	return ogreMaterial->setSelfIllumination(MathUtils::copyColor(color));
}

void RenderMaterial::setSelfIllumination(Color% color)
{
	return ogreMaterial->setSelfIllumination(MathUtils::copyColor(color));
}

void RenderMaterial::setDepthCheckEnabled(bool enabled)
{
	return ogreMaterial->setDepthCheckEnabled(enabled);
}

void RenderMaterial::setDepthWriteEnabled(bool enabled)
{
	return ogreMaterial->setDepthWriteEnabled(enabled);
}

void RenderMaterial::setDepthFunction(CompareFunction func)
{
	return ogreMaterial->setDepthFunction((Ogre::CompareFunction)func);
}

void RenderMaterial::setColorWriteEnabled(bool enabled)
{
	return ogreMaterial->setColourWriteEnabled(enabled);
}

void RenderMaterial::setCullingMode(CullingMode mode)
{
	return ogreMaterial->setCullingMode((Ogre::CullingMode)mode);
}

void RenderMaterial::setManualCullingMode(ManualCullingMode mode)
{
	return ogreMaterial->setManualCullingMode((Ogre::ManualCullingMode)mode);
}

void RenderMaterial::setLightingEnabled(bool enabled)
{
	return ogreMaterial->setLightingEnabled(enabled);
}

void RenderMaterial::setShadingMode(ShadeOptions mode)
{
	return ogreMaterial->setShadingMode((Ogre::ShadeOptions)mode);
}

void RenderMaterial::setFog(bool overrideScene, FogMode mode, Color% color)
{
	return ogreMaterial->setFog(overrideScene, (Ogre::FogMode)mode, MathUtils::copyColor(color));
}

void RenderMaterial::setFog(bool overrideScene, FogMode mode, Color% color, float expDensity, float linearStart, float linearEnd)
{
	return ogreMaterial->setFog(overrideScene, (Ogre::FogMode)mode, MathUtils::copyColor(color), expDensity, linearStart, linearEnd);
}

void RenderMaterial::setDepthBias(float constantBias, float slopeScaleBias)
{
	return ogreMaterial->setDepthBias(constantBias, slopeScaleBias);
}

void RenderMaterial::setTextureFiltering(TextureFilterOptions filterType)
{
	return ogreMaterial->setTextureFiltering((Ogre::TextureFilterOptions)filterType);
}

void RenderMaterial::setTextureAnisotropy(int maxAniso)
{
	return ogreMaterial->setTextureAnisotropy(maxAniso);
}

void RenderMaterial::setSceneBlending(SceneBlendType sbt)
{
	return ogreMaterial->setSceneBlending((Ogre::SceneBlendType)sbt);
}

void RenderMaterial::setSeparateSceneBlending(SceneBlendType sbt, SceneBlendType sbta)
{
	return ogreMaterial->setSeparateSceneBlending((Ogre::SceneBlendType)sbt, (Ogre::SceneBlendType)sbta);
}

void RenderMaterial::setSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor)
{
	return ogreMaterial->setSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor);
}

void RenderMaterial::setSeparateSceneBlending(SceneBlendFactor sourceFactor, SceneBlendFactor destFactor, SceneBlendFactor sourceFactorAlpha, SceneBlendFactor destFactorAlpha)
{
	return ogreMaterial->setSeparateSceneBlending((Ogre::SceneBlendFactor)sourceFactor, (Ogre::SceneBlendFactor)destFactor, (Ogre::SceneBlendFactor)sourceFactorAlpha, (Ogre::SceneBlendFactor)destFactorAlpha);
}

}