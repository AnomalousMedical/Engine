#include "Stdafx.h"

extern "C" _AnomalousExport String TextureUnitState_getTextureName(Ogre::TextureUnitState* textureUnit)
{
	return textureUnit->getTextureName().c_str();
}

extern "C" _AnomalousExport void TextureUnitState_setTextureName(Ogre::TextureUnitState* textureUnit, String name)
{
	textureUnit->setTextureName(name);
}