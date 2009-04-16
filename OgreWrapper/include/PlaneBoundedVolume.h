#pragma once

#include "AutoPtr.h"
#include "Plane.h"

namespace Ogre
{
	class PlaneBoundedVolume;
}

namespace OgreWrapper
{
ref class AxisAlignedBox;

/// <summary>
/// Represents a convex volume bounded by planes. 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PlaneBoundedVolume
{
private:
	AutoPtr<Ogre::PlaneBoundedVolume> ogreVolume;
	Plane::Side outside;

internal:
	/// <summary>
	/// Get the wrapped Ogre::PlaneBoundedVolume.
	/// </summary>
	/// <returns>The wrapped Ogre::PlaneBoundedVolume.</returns>
	Ogre::PlaneBoundedVolume* getVolume();

public:
	/// <summary>
	/// Default constructor.
	/// </summary>
	PlaneBoundedVolume();

	/// <summary>
	/// Constructor, determines which side is deemed to be 'outside'. 
	/// </summary>
	/// <param name="theOutside"></param>
	PlaneBoundedVolume(Plane::Side theOutside);

	/// <summary>
	/// Intersection test with AABB.
	/// 
	/// May return false positives but will never miss an intersection. 
	/// </summary>
	/// <param name="box">The box to test for intersection.</param>
	/// <returns>True on intersection.</returns>
	bool intersects(AxisAlignedBox^ box);

	//sphere intersection

	/// <summary>
	/// Intersection test with a Ray. 
	/// 
	/// May return false positives but will never miss an intersection. 
	/// </summary>
	/// <param name="ray">The ray to test against.</param>
	/// <returns>True on intersection.</returns>
	bool intersects(EngineMath::Ray3 ray);

	/// <summary>
	/// Intersection test with a Ray. 
	/// 
	/// May return false positives but will never miss an intersection. 
	/// </summary>
	/// <param name="ray">The ray to test against.</param>
	/// <returns>True on intersection.</returns>
	bool intersects(EngineMath::Ray3% ray);

	/// <summary>
	/// Add a plane to the collection of planes.
	/// </summary>
	/// <param name="plane">The plane to add.</param>
	void addPlane(Plane^ plane);

	/// <summary>
	/// Clear the list of planes.
	/// </summary>
	void clear();

	/// <summary>
	/// Gets the side defined as Outside.
	/// </summary>
	/// <value></value>
	property Plane::Side Outside 
	{
		Plane::Side get();
	}
};
}