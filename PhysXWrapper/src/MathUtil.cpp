#include "StdAfx.h"
#include "..\include\MathUtil.h"

#include "NxVec3.h"
#include "NxQuat.h"

namespace PhysXWrapper
{

void MathUtil::copyVector3( const NxVec3& source, EngineMath::Vector3% dest )
{
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
}

void MathUtil::copyVector3( const EngineMath::Vector3% source, NxVec3& dest )
{
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
}

NxVec3 MathUtil::copyVector3(const EngineMath::Vector3% source)
{
	return NxVec3(source.x, source.y, source.z);
}

EngineMath::Vector3 MathUtil::copyVector3(const NxVec3& source)
{
	EngineMath::Vector3 dest;
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
	return dest;
}

NxQuat MathUtil::convertNxQuaternion( EngineMath::Quaternion% quat )
{
	NxQuat nxQuat;
	nxQuat.x = quat.x;
	nxQuat.y = quat.y;
	nxQuat.z = quat.z;
	nxQuat.w = quat.w;
	return nxQuat;
}

void MathUtil::copyQuaternion( const NxQuat& source, EngineMath::Quaternion% dest )
{
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
	dest.w = source.w;
}

NxQuat MathUtil::copyQuaternion(EngineMath::Quaternion% source)
{
	NxQuat dest;
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
	dest.w = source.w;
	return dest;
}

EngineMath::Quaternion MathUtil::copyQuaternion(const NxQuat& source)
{
	EngineMath::Quaternion dest;
	dest.x = source.x;
	dest.y = source.y;
	dest.z = source.z;
	dest.w = source.w;
	return dest;
}

void MathUtil::copyQuaternion(EngineMath::Quaternion% source, NxMat33& dest)
{
	NxQuat nxQuat;
	nxQuat.x = source.x;
	nxQuat.y = source.y;
	nxQuat.z = source.z;
	nxQuat.w = source.w;
	dest.fromQuat(nxQuat);
}

NxMat33 MathUtil::quaternionToMat(EngineMath::Quaternion% source)
{
	NxMat33 ret;
	copyQuaternion(source, ret);
	return ret;
}

void MathUtil::copyMatrix(const NxMat33& source, EngineMath::Quaternion% dest)
{
	NxQuat nxQuat;
	source.toQuat(nxQuat);
	dest.setValue(nxQuat.x, nxQuat.y, nxQuat.z, nxQuat.w);
}

EngineMath::Quaternion MathUtil::matToQuaternion(const NxMat33& source)
{
	EngineMath::Quaternion ret;
	copyMatrix(source, ret);
	return ret;
}

void MathUtil::copyRay(EngineMath::Ray3% source, NxRay& dest)
{
	copyVector3(source.Origin, dest.orig);
	copyVector3(source.Direction, dest.dir);
}

void MathUtil::copyRay(const NxRay& source, EngineMath::Ray3% dest)
{
	EngineMath::Vector3 vec = EngineMath::Vector3();
	copyVector3(source.orig, vec);
	dest.Origin = vec;
	copyVector3(source.dir, vec);
	dest.Direction = vec;
}

NxRay MathUtil::copyRay(EngineMath::Ray3% source)
{
	NxRay dest;
	copyVector3(source.Origin, dest.orig);
	copyVector3(source.Direction, dest.dir);
	return dest;
}

EngineMath::Ray3 MathUtil::copyRay(const NxRay& source)
{
	EngineMath::Ray3 dest;
	EngineMath::Vector3 vec = EngineMath::Vector3();
	copyVector3(source.orig, vec);
	dest.Origin = vec;
	copyVector3(source.dir, vec);
	dest.Direction = vec;
	return dest;
}

}