#pragma once

class ContactCache;
class MotionState;

class ContactInfo
{
private:
	ContactInfo* previous;
	ContactInfo* next;
	bool dispatchStartA;	//True if RigidBodyA has gotten a ContactStarted Event
	bool dispatchStartB;	//True if RigidBodyB has gotten a ContactStarted Event
	float closestPoint;		//The closest contact point found this frame
	btAlignedObjectArray<btPersistentManifold*> manifoldArray;
	int numManifolds;

	//Point Iterator
	int currentManifold;
	int currentPoint;

public:
	btRigidBody* rbA;
	btRigidBody* rbB;
	unsigned long key;
	ContactCache* cache;
	MotionState* bodyAMotion;
	MotionState* bodyBMotion;

	ContactInfo(void);

	~ContactInfo(void);

	void reset();

	void process();

	void fireContactEndedOnBodyDelete(btRigidBody* deletingBody);

	void destroy();

	ContactInfo* findMatch(btRigidBody* rbA, btRigidBody* rbB);

	void add(ContactInfo* info);

	void addManifold(btPersistentManifold* contactManifold);

	void setValues(btRigidBody* btRbA, btRigidBody* btRbB, unsigned long sumPtr, ContactCache* cache);

	int getNumContacts();

	void startPointIterator();

	bool hasNextPoint();

	btManifoldPoint* nextPoint();
};
