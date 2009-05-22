//header
#pragma once

#include "SharedPtr.h"

namespace Ogre
{
	class HardwareVertexBufferSharedPtr;
}

namespace OgreWrapper{

ref class HardwareVertexBuffer;
ref class HardwareVertexBufferSharedPtr;

ref class HardwareVertexBufferSharedPtrCollection : public SharedPtrCollection<HardwareVertexBuffer^>
{
protected:
	virtual HardwareVertexBuffer^ createWrapper(const void* sharedPtr, array<System::Object^>^ args) override;

public:
	HardwareVertexBufferSharedPtr^ getObject(const Ogre::HardwareVertexBufferSharedPtr& meshPtr);
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
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class HardwareVertexBufferSharedPtr
{
private:
	SharedPtr<HardwareVertexBuffer^>^ sharedPtr;
	HardwareVertexBuffer^ object;

internal:
	HardwareVertexBufferSharedPtr(SharedPtr<HardwareVertexBuffer^>^ sharedPtr);

public:
	virtual ~HardwareVertexBufferSharedPtr();

	property HardwareVertexBuffer^ Value
	{
		HardwareVertexBuffer^ get()
		{
			return object;
		}
	}
};

}