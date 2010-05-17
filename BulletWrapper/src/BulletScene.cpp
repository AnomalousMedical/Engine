#include "StdAfx.h"
#include "..\Include\BulletScene.h"

BulletScene::BulletScene(BulletSceneInfo* sceneInfo)
:maxProxies(sceneInfo->maxProxies),
//debugDraw(new BulletDebugDraw()),
internalTimestep(1.0f / 60.0f)
#if USE_SOFTBODY_WORLD
,
softBodyWorldInfo(new btSoftBodyWorldInfo())
#endif
{
#ifdef USE_SOFTBODY_WORLD
	collisionConfiguration = new btSoftBodyRigidBodyCollisionConfiguration();
#else
	collisionConfiguration = new btDefaultCollisionConfiguration();
#endif

#ifdef USE_PARALLEL_DISPATCHER
	int maxNumOutstandingTasks = 4;

	m_threadSupportCollision = new Win32ThreadSupport(Win32ThreadSupport::Win32ThreadConstructionInfo(
								"collision",
								processCollisionTask,
								createCollisionLocalStoreMemory,
								maxNumOutstandingTasks));

	dispatcher = new	SpuGatheringCollisionDispatcher(m_threadSupportCollision,maxNumOutstandingTasks,collisionConfiguration);
	
#else
	dispatcher = new btCollisionDispatcher(collisionConfiguration);
#endif

	overlappingPairCache = new btAxisSweep3(sceneInfo->worldAabbMin.toBullet(), sceneInfo->worldAabbMax.toBullet(), maxProxies);
	solver = new btSequentialImpulseConstraintSolver();

#ifdef USE_SOFTBODY_WORLD
	softBodyWorldInfo->m_dispatcher = dispatcher;
	softBodyWorldInfo->m_broadphase = overlappingPairCache;
	dynamicsWorld = new btSoftRigidDynamicsWorld(dispatcher, overlappingPairCache, solver, collisionConfiguration);
	softBodyWorldInfo->m_gravity = sceneInfo->gravity.toBullet();
#else
	dynamicsWorld = new btDiscreteDynamicsWorld(dispatcher,overlappingPairCache,solver,collisionConfiguration);
#endif
	dynamicsWorld->setGravity(sceneInfo->gravity.toBullet());

	//dynamicsWorld->setInternalTickCallback(BulletPlugin::tickCallback, static_cast<void*>(sceneRoot));

#ifdef USE_SOFTBODY_WORLD
	softBodyWorldInfo->m_sparsesdf.Initialize();
	softBodyWorldInfo->m_sparsesdf.Reset();
	softBodyWorldInfo->air_density = (btScalar)1.2;
	softBodyWorldInfo->water_density = 0;
	softBodyWorldInfo->water_offset	= 0;
	softBodyWorldInfo->water_normal	= btVector3(0, 0, 0);
	dynamicsWorld->setDrawFlags(fDrawFlags::Std);
#endif
}

BulletScene::~BulletScene(void)
{
	//delete debugDraw;

	//delete dynamics world
	delete dynamicsWorld;

	//delete solver
	delete solver;

	//delete broadphase
	delete overlappingPairCache;

	//delete dispatcher
	delete dispatcher;

#ifdef USE_PARALLEL_DISPATCHER
	delete m_threadSupportCollision;
#endif

#ifdef USE_SOFTBODY_WORLD
	delete softBodyWorldInfo;
#endif

	delete collisionConfiguration;
}

void BulletScene::fillOutInfo(BulletSceneInfo* sceneInfo)
{
	sceneInfo->gravity = dynamicsWorld->getGravity();
	btVector3 aabbMin, aabbMax;
	overlappingPairCache->getBroadphaseAabb(aabbMin, aabbMax);
	sceneInfo->worldAabbMin = aabbMin;
	sceneInfo->worldAabbMax = aabbMax;
	sceneInfo->maxProxies = maxProxies;
}

void BulletScene::update(float seconds)
{
	int subSteps = 2;
	if(seconds > internalTimestep)
	{
		subSteps = seconds / internalTimestep + 1;
		if(subSteps > 7)
		{
			subSteps = 7;
		}
	}
	dynamicsWorld->stepSimulation(seconds, subSteps, internalTimestep);
}

void BulletScene::addRigidBody(btRigidBody* rigidBody, short group, short mask)
{
	dynamicsWorld->addRigidBody(rigidBody, group, mask);
}

void BulletScene::removeRigidBody(btRigidBody* rigidBody)
{
	dynamicsWorld->removeRigidBody(rigidBody);
}

void BulletScene::addConstraint(btTypedConstraint* constraint, bool disableCollisionsBetweenLinkedBodies)
{
	dynamicsWorld->addConstraint(constraint, disableCollisionsBetweenLinkedBodies);
}

void BulletScene::removeConstraint(btTypedConstraint* constraint)
{
	dynamicsWorld->removeConstraint(constraint);
}

//--------------------------------------------------
//Wrapper functions
//--------------------------------------------------
extern "C" _declspec(dllexport) BulletScene* BulletScene_CreateBulletScene(BulletSceneInfo* sceneInfo)
{
	return new BulletScene(sceneInfo);
}

extern "C" _declspec(dllexport) void BulletScene_DestroyBulletScene(BulletScene* instance)
{
	delete instance;
}

extern "C" _declspec(dllexport) void BulletScene_fillOutInfo(BulletScene* instance, BulletSceneInfo* sceneInfo)
{
	instance->fillOutInfo(sceneInfo);
}

extern "C" _declspec(dllexport) void BulletScene_update(BulletScene* instance, float seconds)
{
	instance->update(seconds);
}

extern "C" _declspec(dllexport) void BulletScene_addRigidBody(BulletScene* instance, btRigidBody* rigidBody, short group, short mask)
{
	instance->addRigidBody(rigidBody, group, mask);
}

extern "C" _declspec(dllexport) void BulletScene_removeRigidBody(BulletScene* instance, btRigidBody* rigidBody)
{
	instance->removeRigidBody(rigidBody);
}

extern "C" _declspec(dllexport) void BulletScene_addConstraint(BulletScene* instance, btTypedConstraint* constraint, bool disableCollisionsBetweenLinkedBodies)
{
	instance->addConstraint(constraint, disableCollisionsBetweenLinkedBodies);
}

extern "C" _declspec(dllexport) void BulletScene_removeConstraint(BulletScene* instance, btTypedConstraint* constraint)
{
	instance->removeConstraint(constraint);
}