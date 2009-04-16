//source
#include "stdafx.h"
#include "MaterialPtr.h"
#include "Material.h"

namespace OgreWrapper{

Material^ RenderMaterialPtrCollection::createWrapper(const void* sharedPtr, array<System::Object^>^ args)
{
	return gcnew Material(*(const Ogre::MaterialPtr*)sharedPtr);
}

MaterialPtr^ RenderMaterialPtrCollection::getObject(const Ogre::MaterialPtr& meshPtr)
{
	return gcnew MaterialPtr(getObjectVoid(meshPtr.get(), &meshPtr));
}

MaterialPtr::MaterialPtr(SharedPtr<Material^>^ sharedPtr)
:sharedPtr(sharedPtr), mesh(sharedPtr->Value)
{

}

MaterialPtr::~MaterialPtr()
{
	delete sharedPtr;
}

}