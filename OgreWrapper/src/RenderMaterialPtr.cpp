//source
#include "stdafx.h"
#include "RenderMaterialPtr.h"
#include "RenderMaterial.h"

namespace Engine{

namespace Rendering{

RenderMaterial^ RenderMaterialPtrCollection::createWrapper(const void* sharedPtr, array<System::Object^>^ args)
{
	return gcnew RenderMaterial(*(const Ogre::MaterialPtr*)sharedPtr);
}

RenderMaterialPtr^ RenderMaterialPtrCollection::getObject(const Ogre::MaterialPtr& meshPtr)
{
	return gcnew RenderMaterialPtr(getObjectVoid(meshPtr.get(), &meshPtr));
}

RenderMaterialPtr::RenderMaterialPtr(SharedPtr<RenderMaterial^>^ sharedPtr)
:sharedPtr(sharedPtr), mesh(sharedPtr->Value)
{

}

RenderMaterialPtr::~RenderMaterialPtr()
{
	delete sharedPtr;
}

}

}