#pragma once

#include "Enums.h"
#include "Skeleton.h"

namespace Ogre
{
	class SkeletonInstance;
}

using namespace System::Collections::Generic;

namespace Engine{

namespace Rendering{

ref class Bone;

/// <summary>
/// A SkeletonInstance is a single instance of a Skeleton used by a world object.
/// 
/// The difference between a Skeleton and a SkeletonInstance is that the Skeleton is the 'master' 
/// version much like Mesh is a 'master' version of Entity. Many SkeletonInstance objects can be 
/// based on a single Skeleton, and are copies of it when created. Any changes made to this are not 
/// reflected in the master copy. The exception is animations; these are shared on the Skeleton 
/// itself and may not be modified here. 
/// </summary>
public ref class SkeletonInstance : public Skeleton
{
private:
	Ogre::SkeletonInstance* skeletonInstance;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="skeletonInstance">The skeleton instance to wrap.</param>
	SkeletonInstance(Ogre::SkeletonInstance* skeletonInstance);

	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~SkeletonInstance(void);

public:
	//create tag point

	//release tag point
};

}

}