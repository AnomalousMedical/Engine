//header
#pragma once

#include "SharedPtr.h"

namespace Ogre
{
	class MaterialPtr;
}

namespace OgreWrapper{

ref class RenderMaterial;
ref class RenderMaterialPtr;

ref class RenderMaterialPtrCollection : public SharedPtrCollection<RenderMaterial^>
{
protected:
	virtual RenderMaterial^ createWrapper(const void* sharedPtr, array<System::Object^>^ args) override;

public:
	RenderMaterialPtr^ getObject(const Ogre::MaterialPtr& meshPtr);
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
public ref class RenderMaterialPtr
{
private:
	SharedPtr<RenderMaterial^>^ sharedPtr;
	RenderMaterial^ mesh;

internal:
	RenderMaterialPtr(SharedPtr<RenderMaterial^>^ sharedPtr);

public:
	virtual ~RenderMaterialPtr();

	property RenderMaterial^ Value
	{
		RenderMaterial^ get()
		{
			return mesh;
		}
	}
};

}