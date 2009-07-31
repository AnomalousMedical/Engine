#include "StdAfx.h"
#include "..\include\MotionState.h"
#include "RigidBody.h"

using namespace Engine;

namespace BulletPlugin
{

#pragma unmanaged

MotionState::MotionState(gcroot<RigidBody^> rigidBody)
:rigidBody(rigidBody)
{

}

void MotionState::getWorldTransform (btTransform &worldTrans) const
{
	
}

void MotionState::setWorldTransform (const btTransform &worldTrans)
{
	btQuaternion rot;
	worldTrans.getBasis().getRotation(rot);
	updateRigidBody(&worldTrans.getOrigin().x(), &rot.x());
}

#pragma managed

MotionState::~MotionState(void)
{

}

void MotionState::updateRigidBody(const float* trans, const float* rot)
{
	Vector3 translation(trans[0], trans[1], trans[2]);
	Quaternion rotation(rot[0], rot[1], rot[2], rot[3]);
	rigidBody->updatePosition(translation, rotation);
}

}