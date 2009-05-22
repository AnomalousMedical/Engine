#pragma once

#include "Enums.h"
#include "SkeletonPtr.h"

namespace Ogre
{
	class SkeletonManager;
	class Mesh;
}

namespace OgreWrapper{

ref class Skeleton;

[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class SkeletonManager
{
private:
	SkeletonPtrCollection skeletonPtrs;

	Ogre::SkeletonManager* skeletonManager;
	static SkeletonManager^ instance = gcnew SkeletonManager();

	SkeletonManager();

internal:
	SkeletonPtr^ getObject(const Ogre::SkeletonPtr& skeleton);

public:
	virtual ~SkeletonManager();

	/// <summary>
	/// Get the instance of this SkeletonManager.
	/// </summary>
	/// <returns>The SkeletonManager instance.</returns>
	static SkeletonManager^ getInstance();
};

}