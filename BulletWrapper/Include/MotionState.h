#pragma once

class ContactInfo;

typedef void (*SetXformCallback)(const Vector3& trans, const Quaternion& rot HANDLE_ARG);
typedef void (*ContactCallback)(ContactInfo* contact, btRigidBody* sourceBody, btRigidBody* otherBody, bool isBodyA HANDLE_ARG);

class MotionState : public btMotionState
{
private:
	SetXformCallback xformCallback;
	ContactCallback contactStartedCallback;
	ContactCallback contactEndedCallback;
	ContactCallback contactContinuesCallback;
	
	btTransform transform;
	HANDLE_INSTANCE

public:
	MotionState(SetXformCallback xformCallback, ContactCallback contactStartedCallback,	ContactCallback contactEndedCallback, ContactCallback contactContinuesCallback, float maxContactDistance, const Vector3* initialTrans, const Quaternion* initialRot HANDLE_ARG);

	virtual ~MotionState(void);

	virtual void getWorldTransform (btTransform &worldTrans) const;

	virtual void setWorldTransform (const btTransform &worldTrans);

	void fireContactStarted(ContactInfo* contact, btRigidBody* sourceBody, btRigidBody* otherBody, bool isBodyA)
	{
		if(hasContactStartedCallback)
		{
			contactStartedCallback(contact, sourceBody, otherBody, isBodyA PASS_HANDLE_ARG);
		}
	}

	void fireContactStopped(ContactInfo* contact, btRigidBody* sourceBody, btRigidBody* otherBody, bool isBodyA)
	{
		if(hasContactEndedCallback)
		{
			contactEndedCallback(contact, sourceBody, otherBody, isBodyA PASS_HANDLE_ARG);
		}
	}

	void fireContactContinues(ContactInfo* contact, btRigidBody* sourceBody, btRigidBody* otherBody, bool isBodyA)
	{
		if(hasContactContinuesCallback)
		{
			contactContinuesCallback(contact, sourceBody, otherBody, isBodyA PASS_HANDLE_ARG);
		}
	}

	//Use booleans to prevent crossing managed/unmanaged barrier if there is nothing on the other side
	bool hasContactStartedCallback;
	bool hasContactEndedCallback;
	bool hasContactContinuesCallback;

	//The max contact distance
	float maxContactDistance;
};
