#pragma once

#include "ContactCache.h"

/// <summary>
/// Info for the bullet scene.
/// </summary>
class BulletSceneInfo
{
public:
	Vector3 worldAabbMin;
	Vector3 worldAabbMax;
	int maxProxies;
	Vector3 gravity;
};

/// <summary>
/// The Bullet Scene, wraps up all the elements needed for a scene and provides
/// an update mechanism.
/// </summary>
class BulletScene
{
private:
	int maxProxies;
//	BulletDebugDraw* debugDraw;

	btDefaultCollisionConfiguration* collisionConfiguration;
	btCollisionDispatcher* dispatcher;
	btAxisSweep3* overlappingPairCache;
	btSequentialImpulseConstraintSolver* solver;

#ifdef USE_SOFTBODY_WORLD
	btSoftRigidDynamicsWorld* dynamicsWorld;
	btSoftBodyWorldInfo* softBodyWorldInfo;
#else
	btDiscreteDynamicsWorld* dynamicsWorld;
#endif

	ContactCache contactCache;
	float internalTimestep;

#ifdef USE_PARALLEL_DISPATCHER
	Win32ThreadSupport* m_threadSupportCollision;
#endif

public:
	BulletScene(BulletSceneInfo* sceneInfo);

	~BulletScene(void);

	void fillOutInfo(BulletSceneInfo* sceneInfo);

	void update(float seconds);

	void addRigidBody(btRigidBody* rigidBody, short group, short mask);

	void removeRigidBody(btRigidBody* rigidBody);

	void addConstraint(btTypedConstraint* constraint, bool disableCollisionsBetweenLinkedBodies);

	void removeConstraint(btTypedConstraint* constraint);

	void tickCallback(btScalar timeStep);
};
