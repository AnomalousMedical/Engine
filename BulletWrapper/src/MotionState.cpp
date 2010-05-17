#include "StdAfx.h"
#include "..\Include\MotionState.h"

MotionState::MotionState(void (*setXformCallback)(Vector3 trans, Quaternion rot), const Vector3* initialTrans, const Quaternion* initialRot)
:m_setXformCallback(setXformCallback)
{
	transform.setIdentity();
	transform.setOrigin(initialTrans->toBullet());
	transform.getBasis().setRotation(initialRot->toBullet());
}

MotionState::~MotionState(void)
{
}

void MotionState::getWorldTransform (btTransform &worldTrans) const
{
	worldTrans = transform;
}

void MotionState::setWorldTransform (const btTransform &worldTrans)
{
	btQuaternion rot;
	worldTrans.getBasis().getRotation(rot);
	m_setXformCallback(worldTrans.getOrigin(), rot);
}

extern "C" _declspec(dllexport) MotionState* MotionState_Create(void (*setXformCallback)(Vector3 trans, Quaternion rot), Vector3* initialTrans, Quaternion* initialRot)
{
	return new MotionState(setXformCallback, initialTrans, initialRot);
}

extern "C" _declspec(dllexport) void MotionState_Delete(MotionState* instance)
{
	delete instance;
}