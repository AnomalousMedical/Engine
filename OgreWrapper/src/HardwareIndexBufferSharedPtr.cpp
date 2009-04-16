//source
#include "stdafx.h"
#include "HardwareIndexBufferSharedPtr.h"
#include "HardwareIndexBuffer.h"

namespace Rendering{

HardwareIndexBuffer^ HardwareIndexBufferSharedPtrCollection::createWrapper(const void* sharedPtr, array<System::Object^>^ args)
{
	return gcnew HardwareIndexBuffer(*(const Ogre::HardwareIndexBufferSharedPtr*)sharedPtr);
}

HardwareIndexBufferSharedPtr^ HardwareIndexBufferSharedPtrCollection::getObject(const Ogre::HardwareIndexBufferSharedPtr& meshPtr)
{
	return gcnew HardwareIndexBufferSharedPtr(getObjectVoid(meshPtr.get(), &meshPtr));
}

HardwareIndexBufferSharedPtr::HardwareIndexBufferSharedPtr(SharedPtr<HardwareIndexBuffer^>^ sharedPtr)
:sharedPtr(sharedPtr), object(sharedPtr->Value)
{

}

HardwareIndexBufferSharedPtr::~HardwareIndexBufferSharedPtr()
{
	delete sharedPtr;
}

}