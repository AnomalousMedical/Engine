#include "stdafx.h"
#include "MeshPtr.h"
#include "Mesh.h"

namespace Engine{

namespace Rendering{

Mesh^ MeshPtrCollection::createWrapper(const void* sharedPtr, array<System::Object^>^ args)
{
	return gcnew Mesh(*(const Ogre::MeshPtr*)sharedPtr);
}

MeshPtr^ MeshPtrCollection::getObject(const Ogre::MeshPtr& meshPtr)
{
	return gcnew MeshPtr(getObjectVoid(meshPtr.get(), &meshPtr));
}

MeshPtr::MeshPtr(SharedPtr<Mesh^>^ sharedPtr)
:sharedPtr(sharedPtr), mesh(sharedPtr->Value)
{

}

MeshPtr::~MeshPtr()
{
	delete sharedPtr;
}

}

}