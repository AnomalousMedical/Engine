#include "StdAfx.h"
#include "..\Include\MotionState.h"

MotionState::MotionState(SetXformCallback xformCallback, ContactCallback contactStartedCallback, ContactCallback contactEndedCallback, ContactCallback contactContinuesCallback, float maxContactDistance, const Vector3* initialTrans, const Quaternion* initialRot)
:xformCallback(xformCallback),
contactStartedCallback(contactStartedCallback),
contactEndedCallback(contactEndedCallback),
contactContinuesCallback(contactContinuesCallback),
maxContactDistance(maxContactDistance),
hasContactStartedCallback(false),
hasContactEndedCallback(false),
hasContactContinuesCallback(false)
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
	xformCallback(worldTrans.getOrigin(), rot);
}

extern "C" _declspec(dllexport) MotionState* MotionState_Create(SetXformCallback xformCallback, ContactCallback contactStartedCallback,	ContactCallback contactEndedCallback, ContactCallback contactContinuesCallback, float maxContactDistance, const Vector3* initialTrans, const Quaternion* initialRot)
{
	return new MotionState(xformCallback, contactStartedCallback, contactEndedCallback, contactContinuesCallback, maxContactDistance, initialTrans, initialRot);
}

extern "C" _declspec(dllexport) void MotionState_Delete(MotionState* instance)
{
	delete instance;
}

extern "C" _declspec(dllexport) void MotionState_setHasContactStartedCallback(MotionState* instance, bool hasCallback)
{
	instance->hasContactStartedCallback = hasCallback;
}

extern "C" _declspec(dllexport) void MotionState_setHasContactEndedCallback(MotionState* instance, bool hasCallback)
{
	instance->hasContactEndedCallback = hasCallback;
}

extern "C" _declspec(dllexport) void MotionState_setHasContactContinuesCallback(MotionState* instance, bool hasCallback)
{
	instance->hasContactContinuesCallback = hasCallback;
}
extern "C" _declspec(dllexport) void MotionState_setMaxContactDistance(MotionState* instance, float maxContactDistance)
{
	instance->maxContactDistance = maxContactDistance;
}

extern "C" _declspec(dllexport) float MotionState_getMaxContactDistance(MotionState* instance)
{
	return instance->maxContactDistance;
}
