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
	MotionState(gcroot<RigidBody^> rigidBody, float* initialTrans, float* initialRot);

	virtual ~MotionState(void);

	virtual void getWorldTransform (btTransform &worldTrans) const;

	virtual void setWorldTransform (const btTransform &worldTrans);

	gcroot<RigidBody^> getRigidBody()
	{
		return rigidBody;
	}
};

}