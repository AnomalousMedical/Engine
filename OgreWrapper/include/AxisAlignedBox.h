#pragma once
#include "AutoPtr.h"
#include "Enums.h"

namespace Ogre
{
	class AxisAlignedBox;
}

namespace Rendering{

using namespace EngineMath;

ref class Plane;

/// <summary>
/// Wrapper for AnimationStateSets.  This is a set of pose animations for an entity.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class AxisAlignedBox
{
public:
[Engine::Attributes::SingleEnum]
enum class Extent : unsigned int
{
	EXTENT_NULL,
	EXTENT_FINITE,
	EXTENT_INFINITE
};

/// <summary>
/// 1-----2
/// /|    /|
/// / |   / |
/// 5-----4  |
/// |  0--|--3
/// | /   | /
/// |/    |/
/// 6-----7
/// </summary>
[Engine::Attributes::SingleEnum]
enum class CornerEnum : unsigned int
{
	FAR_LEFT_BOTTOM = 0,
	FAR_LEFT_TOP = 1,
	FAR_RIGHT_TOP = 2,
	FAR_RIGHT_BOTTOM = 3,
	NEAR_RIGHT_BOTTOM = 7,
	NEAR_LEFT_BOTTOM = 6,
	NEAR_LEFT_TOP = 5,
	NEAR_RIGHT_TOP = 4
};

private:
	AutoPtr<Ogre::AxisAlignedBox> ogreBox;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreBox">The Ogre AxisAlignedBox to wrap.</param>
	AxisAlignedBox(const Ogre::AxisAlignedBox* ogreBox);

	Ogre::AxisAlignedBox* getOgreBox();

public:
	AxisAlignedBox();

	AxisAlignedBox(Extent e);

	AxisAlignedBox(EngineMath::Vector3 minVal, EngineMath::Vector3 maxVal);

	AxisAlignedBox(float mx, float my, float mz, float Mx, float My, float Mz);

	virtual ~AxisAlignedBox(void);

	Vector3 getMinimum();

	Vector3 getMaximum();

	void setMinimum(Vector3 minimum);

	void setMinimum(Vector3% minimum);

	void setMaximum(Vector3 maximum);

	void setMaximum(Vector3% maximum);

	void setMinimum(float x, float y, float z);

	void setMaximum(float x, float y, float z);

	void setExtents(Vector3 minimum, Vector3 maximum);

	void setExtents(Vector3% minimum, Vector3% maximum); 

	Vector3 getCorner(CornerEnum cornerToGet);

	void merge(AxisAlignedBox^ rhs);

	void merge(Vector3 point);

	void merge(Vector3% point);

	void setNull();

	bool isNull();

	bool isFinite();

	void setInfinite();

	bool isInfinite();

	bool intersects(AxisAlignedBox^ box);

	AxisAlignedBox^ intersection(AxisAlignedBox^ box);

	float volume();

	void scale(Vector3 scale);

	void scale(Vector3% scale);

	//intersect for sphere
	
	bool intersects(Plane^ plane);

	bool intersects(Vector3 point);

	bool intersects(Vector3% point);

	Vector3 getCenter();

	Vector3 getSize();

	Vector3 getHalfSize();

	bool contains(Vector3 point);

	bool contains(Vector3% point);

	//operator ==

	//operator !=
};

}