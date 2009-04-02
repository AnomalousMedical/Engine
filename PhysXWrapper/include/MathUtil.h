#pragma once

class NxVec3;
class NxQuat;

namespace Engine
{

namespace Physics
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
	static void copyVector3( const NxVec3& source, EngineMath::Vector3% dest );

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyVector3( const EngineMath::Vector3% source, NxVec3& dest );

	/// <summary>
	/// Copies source into a new vector3
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static NxVec3 copyVector3(const EngineMath::Vector3% source);

	/// <summary>
	/// Copies source into a new vector3
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static EngineMath::Vector3 copyVector3(const NxVec3& source);

	/// <summary>
	/// Creates a new NxQuat based on the given quat.  This is provided for
	/// simplicity since you cannot use the NxQuat constructor that takes
	/// x, y, z, w directly.
	/// </summary>
	/// <param name="quat"></param>
	static NxQuat convertNxQuaternion( EngineMath::Quaternion% quat );

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyQuaternion( const NxQuat& source, EngineMath::Quaternion% dest );

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static NxQuat copyQuaternion(EngineMath::Quaternion% source);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static EngineMath::Quaternion copyQuaternion(const NxQuat& source);

	/// <summary>
	/// Converts source into a matrix and puts it in dest.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyQuaternion(EngineMath::Quaternion% source, NxMat33& dest);

	/// <summary>
	/// Converts source into a matrix and returns is.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static NxMat33 quaternionToMat(EngineMath::Quaternion% source);

	/// <summary>
	/// Converts source into a quaternion and puts it in dest.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyMatrix(const NxMat33& source, EngineMath::Quaternion% dest);

	/// <summary>
	/// Converts source into a quaternion and returns it.
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static EngineMath::Quaternion matToQuaternion(const NxMat33& source);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyRay(EngineMath::Ray3% source, NxRay& dest);

	/// <summary>
	/// Copies source into dest
	/// </summary>
	/// <param name="source">Copy from this.</param>
	/// <param name="dest">Copy to this.</param>
	static void copyRay(const NxRay& source, EngineMath::Ray3% dest);

	/// <summary>
	/// Copies source and returns
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static NxRay copyRay(EngineMath::Ray3% source);

	/// <summary>
	/// Copies source and returns
	/// </summary>
	/// <param name="source">Copy from this.</param>
	static EngineMath::Ray3 copyRay(const NxRay& source);
};

}

}