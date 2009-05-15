//Source
#include "stdafx.h"
#include "TextureUnitStateCollection.h"
#include "TextureUnitState.h"

namespace OgreWrapper
{

TextureUnitState^ TextureUnitStateCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew TextureUnitState(static_cast<Ogre::TextureUnitState*>(nativeObject));
}

TextureUnitState^ TextureUnitStateCollection::getObject(Ogre::TextureUnitState* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void TextureUnitStateCollection::destroyObject(Ogre::TextureUnitState* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}