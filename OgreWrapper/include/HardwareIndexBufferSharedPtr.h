//header
#pragma once

#include "SharedPtr.h"

namespace Ogre
{
	class HardwareIndexBufferSharedPtr;
}

namespace OgreWrapper{

ref class HardwareIndexBuffer;
ref class HardwareIndexBufferSharedPtr;

ref class HardwareIndexBufferSharedPtrCollection : public SharedPtrCollection<HardwareIndexBuffer^>
{
protected:
	virtual HardwareIndexBuffer^ createWrapper(const void* sharedPtr, array<System::Object^>^ args) override;

public:
	HardwareIndexBufferSharedPtr^ getObject(const Ogre::HardwareIndexBufferSharedPtr& meshPtr);
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
public ref class HardwareIndexBufferSharedPtr
{
private:
	SharedPtr<HardwareIndexBuffer^>^ sharedPtr;
	HardwareIndexBuffer^ object;

internal:
	HardwareIndexBufferSharedPtr(SharedPtr<HardwareIndexBuffer^>^ sharedPtr);

public:
	virtual ~HardwareIndexBufferSharedPtr();

	property HardwareIndexBuffer^ Value
	{
		HardwareIndexBuffer^ get()
		{
			return object;
		}
	}
};

}