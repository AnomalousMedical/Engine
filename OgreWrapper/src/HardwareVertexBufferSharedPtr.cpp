//source
#include "stdafx.h"
#include "HardwareVertexBufferSharedPtr.h"
#include "HardwareVertexBuffer.h"

namespace OgreWrapper{

HardwareVertexBuffer^ HardwareVertexBufferSharedPtrCollection::createWrapper(const void* sharedPtr, array<System::Object^>^ args)
{
	return gcnew HardwareVertexBuffer(*(const Ogre::HardwareVertexBufferSharedPtr*)sharedPtr);
}

HardwareVertexBufferSharedPtr^ HardwareVertexBufferSharedPtrCollection::getObject(const Ogre::HardwareVertexBufferSharedPtr& meshPtr)
{
	return gcnew HardwareVertexBufferSharedPtr(getObjectVoid(meshPtr.get(), &meshPtr));
}

HardwareVertexBufferSharedPtr::HardwareVertexBufferSharedPtr(SharedPtr<HardwareVertexBuffer^>^ sharedPtr)
:sharedPtr(sharedPtr), object(sharedPtr->Value)
{

}

HardwareVertexBufferSharedPtr::~HardwareVertexBufferSharedPtr()
{
	delete sharedPtr;
}

}