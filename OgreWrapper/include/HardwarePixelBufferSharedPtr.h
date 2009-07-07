//header
#pragma once

#include "SharedPtr.h"

namespace Ogre
{
	class HardwarePixelBufferSharedPtr;
}

namespace OgreWrapper{

ref class HardwarePixelBuffer;
ref class HardwarePixelBufferSharedPtr;

ref class HardwarePixelBufferSharedPtrCollection : public SharedPtrCollection<HardwarePixelBuffer^>
{
protected:
	virtual HardwarePixelBuffer^ createWrapper(const void* sharedPtr, array<System::Object^>^ args) override;

public:
	HardwarePixelBufferSharedPtr^ getObject(const Ogre::HardwarePixelBufferSharedPtr& meshPtr);
};

/// <summary>
/// This class is a shared pointer. This means that it keeps the wrapper
/// instance alive by reference counting the number of times the reference is
/// retrieved. When using this class care must be taken to dispose the pointer
/// whenever it is returned. If this is not done the reference will leak and the
/// underlying Ogre object will leak until the program is shut down. Any leaks
/// that are detected will be printed in the log when the program is closed
/// along with a stack trace of where they were created. It is very important to
/// call dispose in the appropriate place if a leak occures.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class HardwarePixelBufferSharedPtr
{
private:
	SharedPtr<HardwarePixelBuffer^>^ sharedPtr;
	HardwarePixelBuffer^ object;

internal:
	HardwarePixelBufferSharedPtr(SharedPtr<HardwarePixelBuffer^>^ sharedPtr);

public:
	virtual ~HardwarePixelBufferSharedPtr();

	property HardwarePixelBuffer^ Value
	{
		HardwarePixelBuffer^ get()
		{
			return object;
		}
	}
};

}