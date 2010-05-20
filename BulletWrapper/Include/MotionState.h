#pragma once

class ContactInfo;

typedef void (*SetXformCallback)(const Vector3& trans, const Quaternion& rot);
typedef void (*ContactCallback)(ContactInfo* contact, btRigidBody* sourceBody, btRigidBody* otherBody, bool isBodyA);

class MotionState : public btMotionState
{
private:
	SetXformCallback xformCallback;
	ContactCallback contactStartedCallback;
	ContactCallback contactEndedCallback;
	ContactCallback contactContinuesCallback;
	
	btTransform transform;

public:
	MotionState(SetXformCallback xformCallback, ContactCallback contactStartedCallback,	ContactCallback contactEndedCallback, ContactCallback contactContinuesCallback, float maxContactDistance, const Vector3* initialTrans, const Quaternion* initialRot);

	virtual ~MotionState(void);

	virtual void getWorldTransform (btTransform &worldTrans) const;

	virtual void setWorldTransform (const btTransform &worldTrans);

	void fireContactStarted(ContactInfo* contact, btRigidBody* sourceBody, btRigidBody* otherBody, bool isBodyA)
	{
		if(hasContactStartedCallback)
		{
			contactStartedCallback(contact, sourceBody, otherBody, isBodyA);
		}
	}

	void fireContactStopped(ContactInfo* contact, btRigidBody* sourceBody, btRigidBody* otherBody, bool isBodyA)
	{
		if(hasContactEndedCallback)
		{
			contactEndedCallback(contact, sourceBody, otherBody, isBodyA);
		}
	}

	void fireContactContinues(ContactInfo* contact, btRigidBody* sourceBody, btRigidBody* otherBody, bool isBodyA)
	{
		if(hasContactContinuesCallback)
		{
			contactContinuesCallback(contact, sourceBody, otherBody, isBodyA);
		}
	}

	//Use booleans to prevent crossing managed/unmanaged barrier if there is nothing on the other side
	bool hasContactStartedCallback;
	bool hasContactEndedCallback;
	bool hasContactContinuesCallback;

	//The max contact distance
	float maxContactDistance;
};
