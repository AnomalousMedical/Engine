#include "Stdafx.h"

extern "C" __declspec(dllexport) String TextureUnitState_getTextureName(Ogre::TextureUnitState* textureUnit)
{
	return textureUnit->getTextureName().c_str();
}

extern "C" __declspec(dllexport) void TextureUnitState_setTextureName(Ogre::TextureUnitState* textureUnit, String name)
{
	textureUnit->setTextureName(name);
}