#include "StdAfx.h"
#include "..\include\TextureUnitState.h"
#include "MarshalUtils.h"

#include "TextureUnitState.h"

namespace OgreWrapper
{

TextureUnitState::TextureUnitState(Ogre::TextureUnitState* textureUnitState)
:textureUnitState( textureUnitState )
{

}

TextureUnitState::~TextureUnitState()
{
	textureUnitState = 0;
}

Ogre::TextureUnitState* TextureUnitState::getTextureUnitState()
{
	return textureUnitState;
}

System::String^ TextureUnitState::getTextureName()
{
	return MarshalUtils::convertString(textureUnitState->getTextureName());
}

void TextureUnitState::setTextureName(System::String^ name)
{
	return textureUnitState->setTextureName(MarshalUtils::convertString(name));
}

}