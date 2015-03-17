// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#ifndef STDAFX_H
#define STDAFX_H

#include "btBulletDynamicsCommon.h"
#include "BulletSoftBody/btSoftRigidDynamicsWorld.h"
#include "BulletSoftBody/btSoftBodyRigidBodyCollisionConfiguration.h"
#include "BulletSoftBody/btSoftBodyHelpers.h"
#include "../Engine/Interop/NativeDelegates.h"

#ifdef WINDOWS
#define _AnomalousExport __declspec(dllexport)
#endif

#if defined(MAC_OSX) || defined(APPLE_IOS) || defined(ANDROID)
#define _AnomalousExport __attribute__ ((visibility("default")))
#endif

class Vector3
{
public:
	float x, y, z;

	Vector3(const btVector3& bulletVector)
		:x(bulletVector.x()),
		y(bulletVector.y()),
		z(bulletVector.z())
	{
		
	}

	btVector3 toBullet() const
	{
		return btVector3(x, y, z);
	}
};

class Quaternion
{
public:
	float x, y, z, w;

	Quaternion(const btQuaternion& bulletQuaternion)
		:x(bulletQuaternion.x()),
		y(bulletQuaternion.y()),
		z(bulletQuaternion.z()),
		w(bulletQuaternion.w())
	{

	}

	btQuaternion toBullet() const
	{
		return btQuaternion(x, y, z, w);
	}
};

#endif