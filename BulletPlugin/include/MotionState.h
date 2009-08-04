#pragma once

#include "vcclr.h"

namespace BulletPlugin
{

ref class RigidBody;

class MotionState : public btMotionState
{
private:
	gcroot<RigidBody^> rigidBody;
	btTransform transform;

	void updateRigidBody(const float* trans, const float* rot);

public:
	MotionState(gcroot<RigidBody^> rigidBody);

	virtual ~MotionState(void);

	virtual void getWorldTransform (btTransform &worldTrans) const;

	virtual void setWorldTransform (const btTransform &worldTrans);

	void setStartingTransform(float* trans, float* rot);
};

}