//source
#include "stdafx.h"
#include "TexturePtr.h"
#include "Texture.h"

namespace OgreWrapper{

Texture^ TexturePtrCollection::createWrapper(const void* sharedPtr, array<System::Object^>^ args)
{
	return gcnew Texture(*(const Ogre::TexturePtr*)sharedPtr);
}

TexturePtr^ TexturePtrCollection::getObject(const Ogre::TexturePtr& meshPtr)
{
	return gcnew TexturePtr(getObjectVoid(meshPtr.get(), &meshPtr));
}

TexturePtr::TexturePtr(SharedPtr<Texture^>^ sharedPtr)
:sharedPtr(sharedPtr), object(sharedPtr->Value)
{

}

TexturePtr::~TexturePtr()
{
	delete sharedPtr;
}

}