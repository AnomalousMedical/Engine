#pragma once

class MotionState : btMotionState
{
private:
	void (*m_setXformCallback)(Vector3 trans, Quaternion rot);
	btTransform transform;

public:
	MotionState(void (*setXformCallback)(Vector3 trans, Quaternion rot), const Vector3* initialTrans, const Quaternion* initialRot);

	virtual ~MotionState(void);

	virtual void getWorldTransform (btTransform &worldTrans) const;

	virtual void setWorldTransform (const btTransform &worldTrans);
};
