#pragma once

#include "AutoPtr.h"
#include "OgrePlane.h"

namespace OgreWrapper
{

ref class AxisAlignedBox;

/// <summary>
/// Defines a plane in 3D space. A plane is defined in 3D space by the equation
/// Ax + By + Cz + D = 0 
///
/// This equates to a vector (the normal of the plane, whose x, y and z
/// components equate to the coefficients A, B and C respectively), and a
/// constant (D) which is the distance along the normal you have to go to move
/// the plane back to the origin. 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class Plane
{
private:
	AutoPtr<Ogre::Plane> ogrePlane;
internal:
	/// <summary>
	/// Get the Ogre::Plane that is wrapped by this class.
	/// </summary>
	/// <returns>The wrapped Ogre::Plane.</returns>
	Ogre::Plane* getPlane();

public:
	/// <summary>
	/// The "positive side" of the plane is the half space to which the plane
    /// normal points.
	/// 
	/// The "negative side" is the other half space. The flag "no side"
    /// indicates the plane itself. 
	/// </summary>
	enum class Side
	{
		NO_SIDE,
		POSITIVE_SIDE,
		NEGATIVE_SIDE,
		BOTH_SIDE
	};

	/// <summary>
	/// Default constructor - sets everything to 0. 
	/// </summary>
	Plane();

	/// <summary>
	/// Construct a plane through a normal, and a distance to move the plane
    /// along the normal. 
	/// </summary>
	/// <param name="normal">The normal of the plane.</param>
	/// <param name="constant">The distance to move the plane along the normal.</param>
	Plane(EngineMath::Vector3 normal, float constant);

	/// <summary>
	/// Construct a plane using the 4 constants directly. 
	/// </summary>
	/// <param name="a">Ax +</param>
	/// <param name="b">By +</param>
	/// <param name="c">Cz +</param>
	/// <param name="d">D</param>
	Plane(float a, float b, float c, float d);

	/// <summary>
	/// Build the plane from a normal and a point.
	/// </summary>
	/// <param name="normal">The normal of the plane.</param>
	/// <param name="point">The point to place the plane on.</param>
	Plane(EngineMath::Vector3 normal, EngineMath::Vector3 point);

	/// <summary>
	/// Build a plane based on 3 points.
	/// </summary>
	/// <param name="point0">The first point.</param>
	/// <param name="point1">The second point.</param>
	/// <param name="point2">The third point.</param>
	Plane(EngineMath::Vector3 point0, EngineMath::Vector3 point1, EngineMath::Vector3 point2);

	/// <summary>
	/// Get the side of the plane the given point is on.
	/// </summary>
	/// <param name="point">The point to test.</param>
	/// <returns>The Point::Side the point is on.</returns>
	Side getSide(EngineMath::Vector3 point);

	/// <summary>
	/// Get the side the AxisAlignedBox is on.
	/// </summary>
	/// <param name="box">The box to test.</param>
	/// <returns>The flag BOTH_SIDE indicates an intersecting box. one corner ON the plane is sufficient to consider the box and the plane intersecting.</returns>
	Side getSide(AxisAlignedBox^ box);

	/// <summary>
	/// Returns which side of the plane that the given box lies on.
	/// 
	/// The box is defined as centre/half-size pairs for effectively. 
	/// </summary>
	/// <param name="center">The centre of the box.</param>
	/// <param name="halfSize">The half-size of the box. </param>
	/// <returns>POSITIVE_SIDE if the box complete lies on the "positive side" of the plane, NEGATIVE_SIDE if the box complete lies on the "negative side" of the plane, and BOTH_SIDE if the box intersects the plane.</returns>
	Side getSide(EngineMath::Vector3 center, EngineMath::Vector3 halfSize);

	/// <summary>
	/// This is a pseudodistance.
	/// 
	/// The sign of the return value is positive if the point is on the positive
    /// side of the plane, negative if the point is on the negative side, and
    /// zero if the point is on the plane.
	/// 
    /// The absolute value of the return value is the true distance only when
    /// the plane normal is a unit length vector. 
	/// </summary>
	/// <param name="point">The point to test.</param>
	/// <returns>The pseudodistance, see explination as to the nature of this distance.</returns>
	float getDistance(EngineMath::Vector3 point);

	/// <summary>
	/// Redefine this plane based on 3 points. 
	/// </summary>
	/// <param name="point0">The first point.</param>
	/// <param name="point1">The second point.</param>
	/// <param name="point2">The third point.</param>
	void redefine(EngineMath::Vector3 point0, EngineMath::Vector3 point1, EngineMath::Vector3 point2);

	/// <summary>
	/// Redefine the plane from a normal and a point.
	/// </summary>
	/// <param name="normal">The normal of the plane.</param>
	/// <param name="point">The point to place the plane on.</param>
	void redefine(EngineMath::Vector3 normal, EngineMath::Vector3 point);

	/// <summary>
	/// Project a vector onto the plane.
	/// 
	/// This gives you the element of the input vector that is perpendicular to
    /// the normal of the plane. You can get the element which is parallel to
    /// the normal of the plane by subtracting the result of this method from
    /// the original vector, since parallel + perpendicular = original.
	/// </summary>
	/// <param name="v">The vector to project.</param>
	/// <returns>The vector projected onto the plane.</returns>
	EngineMath::Vector3 projectVector(EngineMath::Vector3 v);

	/// <summary>
	/// Normalises the plane. 
	/// 
	/// This method normalises the plane's normal and the length scale of d is
    /// as well. 
	/// 
	/// This function will not crash for zero-sized vectors, but there will be
    /// no changes made to their components. 
	/// </summary>
	/// <returns>The previous length of the plane's normal.</returns>
	float normalize();

	/// <summary>
	/// Comparison operator.
	/// </summary>
	/// <param name="p1">LHS</param>
	/// <param name="p2">RHS</param>
	/// <returns>True if they are equal.</returns>
	static bool operator == (Plane p1,  Plane p2) 
	{
		return (*p1.getPlane()) == (*p2.getPlane());
	}

	/// <summary>
	/// Comparison operator.
	/// </summary>
	/// <param name="p1">LHS</param>
	/// <param name="p2">RHS</param>
	/// <returns>True if they are not equal.</returns>
	static bool operator != (Plane p1, Plane p2)
	{
		return (*p1.getPlane()) != (*p2.getPlane());
	}
};

}