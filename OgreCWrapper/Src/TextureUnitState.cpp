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

extern "C" _AnomalousExport Ogre::TextureUnitState::TextureAddressingMode TextureUnitState_getTextureAddressingModeU(Ogre::TextureUnitState* textureUnit)
{
	return textureUnit->getTextureAddressingMode().u;
}

extern "C" _AnomalousExport Ogre::TextureUnitState::TextureAddressingMode TextureUnitState_getTextureAddressingModeV(Ogre::TextureUnitState* textureUnit)
{
	return textureUnit->getTextureAddressingMode().v;
}

extern "C" _AnomalousExport Ogre::TextureUnitState::TextureAddressingMode TextureUnitState_getTextureAddressingModeW(Ogre::TextureUnitState* textureUnit)
{
	return textureUnit->getTextureAddressingMode().w;
}

extern "C" _AnomalousExport void TextureUnitState_setTextureAddressingModeU(Ogre::TextureUnitState* textureUnit, Ogre::TextureUnitState::TextureAddressingMode tam)
{
	Ogre::TextureUnitState::UVWAddressingMode mode = textureUnit->getTextureAddressingMode();
	return textureUnit->setTextureAddressingMode(tam, mode.v, mode.w);
}

extern "C" _AnomalousExport void TextureUnitState_setTextureAddressingModeV(Ogre::TextureUnitState* textureUnit, Ogre::TextureUnitState::TextureAddressingMode tam)
{
	Ogre::TextureUnitState::UVWAddressingMode mode = textureUnit->getTextureAddressingMode();
	return textureUnit->setTextureAddressingMode(mode.u, tam, mode.w);
}

extern "C" _AnomalousExport void TextureUnitState_setTextureAddressingModeW(Ogre::TextureUnitState* textureUnit, Ogre::TextureUnitState::TextureAddressingMode tam)
{
	Ogre::TextureUnitState::UVWAddressingMode mode = textureUnit->getTextureAddressingMode();
	return textureUnit->setTextureAddressingMode(mode.u, mode.v, tam);
}

extern "C" _AnomalousExport void TextureUnitState_setTextureAddressingModeUVW(Ogre::TextureUnitState* textureUnit, Ogre::TextureUnitState::TextureAddressingMode u, Ogre::TextureUnitState::TextureAddressingMode v, Ogre::TextureUnitState::TextureAddressingMode w)
{
	return textureUnit->setTextureAddressingMode(u, v, w);
}

extern "C" _AnomalousExport void TextureUnitState_setTextureAddressingMode(Ogre::TextureUnitState* textureUnit, Ogre::TextureUnitState::TextureAddressingMode tam)
{
	return textureUnit->setTextureAddressingMode(tam);
}