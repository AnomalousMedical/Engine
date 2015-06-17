#include "Stdafx.h"

extern "C" _AnomalousExport String TextureUnitState_getTextureName(Ogre::TextureUnitState* textureUnit)
{
	return textureUnit->getTextureName().c_str();
}

extern "C" _AnomalousExport void TextureUnitState_setTextureName(Ogre::TextureUnitState* textureUnit, String name)
{
	textureUnit->setTextureName(name);
}

extern "C" _AnomalousExport String TextureUnitState_getName(Ogre::TextureUnitState* textureUnit)
{
	return textureUnit->getName().c_str();
}

extern "C" _AnomalousExport Ogre::FilterOptions TextureUnitState_getTextureFiltering(Ogre::TextureUnitState* textureUnit, Ogre::FilterType filterType)
{
	return textureUnit->getTextureFiltering(filterType);
}

extern "C" _AnomalousExport void TextureUnitState_setTextureFiltering(Ogre::TextureUnitState* textureUnit, Ogre::FilterType filterType, Ogre::FilterOptions option)
{
	return textureUnit->setTextureFiltering(filterType, option);
}

extern "C" _AnomalousExport void TextureUnitState_setTextureFiltering2(Ogre::TextureUnitState* textureUnit, Ogre::FilterOptions minFilter, Ogre::FilterOptions magFilter, Ogre::FilterOptions mipFilter)
{
	return textureUnit->setTextureFiltering(minFilter, magFilter, mipFilter);
}