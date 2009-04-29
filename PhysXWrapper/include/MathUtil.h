#pragma once

class NxVec3;
class NxQuat;

namespace PhysXWrapper
{

/// <summary>
/// Helper class for converting between the two math systems.
/// </summary>
/// <remarks>
/// There are no conversions between NxVec3 and Vector3 because it is easy 
/// to simply call their constructors.  The same goes for NxQuat to Quaternion.
/// </remarks>
ref class MathUtil
{
public:
	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyVector3( const NxVec3& source, Engine::Vector3% dest );

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyVector3( const Engine::Vector3% source, NxVec3& dest );

	/// <summary>
	/// Copies source into a new vector3
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static NxVec3 copyVector3(const Engine::Vector3% source);

	/// <summary>
	/// Copies source into a new vector3
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Engine::Vector3 copyVector3(const NxVec3& source);

	/// <summary>
	/// Creates a new NxQuat based on the given quat.  This is provided for
	/// simplicity since you cannot use the NxQuat constructor that takes
	/// x, y, z, w directly.
	/// </summary>
	/// <param name="quat"></param>
	static NxQuat convertNxQuaternion( Engine::Quaternion% quat );

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyQuaternion( const NxQuat& source, Engine::Quaternion% dest );

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static NxQuat copyQuaternion(Engine::Quaternion% source);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Engine::Quaternion copyQuaternion(const NxQuat& source);

	/// <summary>
	/// Converts source into a matrix and puts it in dest.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyQuaternion(Engine::Quaternion% source, NxMat33& dest);

	/// <summary>
	/// Converts source into a matrix and returns is.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static NxMat33 quaternionToMat(Engine::Quaternion% source);

	/// <summary>
	/// Converts source into a quaternion and puts it in dest.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyMatrix(const NxMat33& source, Engine::Quaternion% dest);

	/// <summary>
	/// Converts source into a quaternion and returns it.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Engine::Quaternion matToQuaternion(const NxMat33& source);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyRay(Engine::Ray3% source, NxRay& dest);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyRay(const NxRay& source, Engine::Ray3% dest);

	/// <summary>
	/// Copies source and returns
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static NxRay copyRay(Engine::Ray3% source);

	/// <summary>
	/// Copies source and returns
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static Engine::Ray3 copyRay(const NxRay& source);
};

}