//header
#pragma once

#include "SharedPtr.h"

namespace Ogre
{
	class MaterialPtr;
}

namespace OgreWrapper{

ref class Material;
ref class MaterialPtr;

ref class RenderMaterialPtrCollection : public SharedPtrCollection<Material^>
{
protected:
	virtual Material^ createWrapper(const void* sharedPtr, array<System::Object^>^ args) override;

public:
	MaterialPtr^ getObject(const Ogre::MaterialPtr& meshPtr);
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
public ref class MaterialPtr
{
private:
	SharedPtr<Material^>^ sharedPtr;
	Material^ mesh;

internal:
	MaterialPtr(SharedPtr<Material^>^ sharedPtr);

public:
	virtual ~MaterialPtr();

	property Material^ Value
	{
		Material^ get()
		{
			return mesh;
		}
	}
};

}