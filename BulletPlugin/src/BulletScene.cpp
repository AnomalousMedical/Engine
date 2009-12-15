#include "StdAfx.h"
#include "..\include\BulletScene.h"
#include "BulletSceneDefinition.h"
#include "BulletFactory.h"
#include "vcclr.h"
#include "BulletDebugDraw.h"
#include "MotionState.h"
#include "RigidBody.h"

#ifdef USE_PARALLEL_DISPATCHER
#include "BulletMultiThreaded/SpuGatheringCollisionDispatcher.h"
#include "BulletMultiThreaded/PlatformDefinitions.h"
#include "BulletMultiThreaded/Win32ThreadSupport.h"
#include "BulletMultiThreaded/SpuNarrowPhaseCollisionTask/SpuGatheringCollisionTask.h"
#endif

#ifdef USE_SOFTBODY_WORLD

#endif

namespace BulletPlugin
{

#pragma unmanaged
//Functions to get around lack of support for aligned types in .net
btAxisSweep3* createAxisSweep(float* worldMin, float* worldMax, int maxProxies)
{
	return new btAxisSweep3(btVector3(worldMin[0], worldMin[1], worldMin[2]), btVector3(worldMax[0], worldMax[1], worldMax[2]), maxProxies);
}

#ifdef USE_SOFTBODY_WORLD
void setWaterNormal(btSoftBodyWorldInfo* softBodyWorldInfo, float* norm)
{
	softBodyWorldInfo->water_normal	= btVector3(norm[0], norm[1], norm[2]);
}

void setGravity(btDiscreteDynamicsWorld* dynamicsWorld, btSoftBodyWorldInfo* softBodyWorldInfo, float* gravity)
{
	softBodyWorldInfo->m_gravity.setValue(gravity[0], gravity[1], gravity[2]);
#else
void setGravity(btDiscreteDynamicsWorld* dynamicsWorld, float* gravity)
{
#endif
	dynamicsWorld->setGravity(btVector3(gravity[0], gravity[1], gravity[2]));
}

void getGravity(btDiscreteDynamicsWorld* dynamicsWorld, float* gravity)
{
	btVector3 grav = dynamicsWorld->getGravity();
	gravity[0] = grav.x();
	gravity[1] = grav.y();
	gravity[2] = grav.z();
}

void getBroadphaseAabb(btAxisSweep3* axisSweep, float* aabbMin, float* aabbMax)
{
	btVector3 btMin, btMax;
	axisSweep->getBroadphaseAabb(btMin, btMax);
	aabbMin[0] = btMin.x();
	aabbMin[1] = btMin.y();
	aabbMin[2] = btMin.z();
	aabbMax[0] = btMax.x();
	aabbMax[1] = btMax.y();
	aabbMax[2] = btMax.z();
}

#pragma managed

static void tickCallback(btDynamicsWorld *world, btScalar timeStep)
{
	gcroot<BulletScene^>* scene = static_cast<gcroot<BulletScene^>*>(world->getWorldUserInfo());
	(*scene)->tickCallback(timeStep);
}

BulletScene::BulletScene(BulletSceneDefinition^ definition, UpdateTimer^ timer)
:name(definition->Name), 
timer(timer), 
maxProxies(definition->MaxProxies),
debugDraw(new BulletDebugDraw()),
internalTimestep(1.0f / 60.0f)
#if USE_SOFTBODY_WORLD
,
softBodyWorldInfo(new btSoftBodyWorldInfo())
#endif
{
	sceneRoot = new gcroot<BulletScene^>(this);

	factory = gcnew BulletFactory(this);

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

	overlappingPairCache = createAxisSweep(&definition->WorldAabbMin.x, &definition->WorldAabbMax.x, definition->MaxProxies);
	solver = new btSequentialImpulseConstraintSolver();

#ifdef USE_SOFTBODY_WORLD
	softBodyWorldInfo->m_dispatcher = dispatcher;
	softBodyWorldInfo->m_broadphase = overlappingPairCache;
	dynamicsWorld = new btSoftRigidDynamicsWorld(dispatcher, overlappingPairCache, solver, collisionConfiguration);
	setGravity(dynamicsWorld, softBodyWorldInfo, &definition->Gravity.x);
#else
	dynamicsWorld = new btDiscreteDynamicsWorld(dispatcher,overlappingPairCache,solver,collisionConfiguration);
	setGravity(dynamicsWorld, &definition->Gravity.x);
#endif

	dynamicsWorld->setInternalTickCallback(BulletPlugin::tickCallback, static_cast<void*>(sceneRoot));
	/*dynamicsWorld->getDispatchInfo().m_useConvexConservativeDistanceUtil = true;
	dynamicsWorld->getDispatchInfo().m_convexConservativeDistanceThreshold = 0.01;*/

	timer->addFixedUpdateListener(this);

#ifdef USE_SOFTBODY_WORLD
	softBodyWorldInfo->m_sparsesdf.Initialize();
	softBodyWorldInfo->m_sparsesdf.Reset();
	softBodyWorldInfo->air_density = (btScalar)1.2;
	softBodyWorldInfo->water_density = 0;
	softBodyWorldInfo->water_offset	= 0;
	float waterNormal[3] = {0, 0, 0};
	setWaterNormal(softBodyWorldInfo, waterNormal);
#endif
}

BulletScene::~BulletScene(void)
{
	timer->removeFixedUpdateListener(this);

	delete debugDraw;

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

	delete sceneRoot;
}

void BulletScene::tickCallback(btScalar timeStep)
{
	int numManifolds = dispatcher->getNumManifolds();
	for(int i = 0; i < numManifolds; ++i)
	{
		btPersistentManifold* contactManifold = dispatcher->getManifoldByIndexInternal(i);
		contactCache.addManifold(contactManifold);
	}
	contactCache.dispatchContacts();
}

SimElementFactory^ BulletScene::getFactory()
{
	return factory;
}

Type^ BulletScene::getSimElementManagerType()
{
	return BulletScene::typeid;
}

String^ BulletScene::getName()
{
	return name;
}

SimElementManagerDefinition^ BulletScene::createDefinition()
{
	BulletSceneDefinition^ definition = gcnew BulletSceneDefinition(name);
	Vector3 vector, vector2;
	getGravity(dynamicsWorld, &vector.x);
	definition->Gravity = vector;
	getBroadphaseAabb(overlappingPairCache, &vector.x, &vector2.x);
	definition->WorldAabbMin = vector;
	definition->WorldAabbMax = vector2;
	definition->MaxProxies = maxProxies;
	return definition;
}

void BulletScene::sendUpdate(Clock^ clock)
{
	float seconds = (float)clock->Seconds;
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

void BulletScene::loopStarting()
{

}

void BulletScene::exceededMaxDelta()
{

}

BulletFactory^ BulletScene::getBulletFactory()
{
	return factory;
}

void BulletScene::drawDebug(DebugDrawingSurface^ drawingSurface)
{
	drawingSurface->begin(name + "BulletDebug", Engine::Renderer::DrawingType::LineList);
	debugDraw->setDrawingSurface(drawingSurface);
	dynamicsWorld->setDebugDrawer(debugDraw);
	dynamicsWorld->debugDrawWorld();
	dynamicsWorld->setDebugDrawer(0);
	drawingSurface->end();
}

}