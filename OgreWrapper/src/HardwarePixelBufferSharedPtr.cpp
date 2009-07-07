//source
#include "stdafx.h"
#include "HardwarePixelBufferSharedPtr.h"
#include "HardwarePixelBuffer.h"

namespace OgreWrapper{

HardwarePixelBuffer^ HardwarePixelBufferSharedPtrCollection::createWrapper(const void* sharedPtr, array<System::Object^>^ args)
{
	return gcnew HardwarePixelBuffer(*(const Ogre::HardwarePixelBufferSharedPtr*)sharedPtr);
}

HardwarePixelBufferSharedPtr^ HardwarePixelBufferSharedPtrCollection::getObject(const Ogre::HardwarePixelBufferSharedPtr& meshPtr)
{
	return gcnew HardwarePixelBufferSharedPtr(getObjectVoid(meshPtr.get(), &meshPtr));
}

HardwarePixelBufferSharedPtr::HardwarePixelBufferSharedPtr(SharedPtr<HardwarePixelBuffer^>^ sharedPtr)
:sharedPtr(sharedPtr), object(sharedPtr->Value)
{

}

HardwarePixelBufferSharedPtr::~HardwarePixelBufferSharedPtr()
{
	delete sharedPtr;
}

}