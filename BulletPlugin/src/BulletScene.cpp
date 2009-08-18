#include "StdAfx.h"
#include "..\include\BulletScene.h"
#include "BulletSceneDefinition.h"
#include "BulletFactory.h"
#include "vcclr.h"
#include "BulletDebugDraw.h"
#include "MotionState.h"
#include "RigidBody.h"

namespace BulletPlugin
{

#pragma unmanaged
//Functions to get around lack of support for aligned types in .net
btAxisSweep3* createAxisSweep(float* worldMin, float* worldMax, int maxProxies)
{
	return new btAxisSweep3(btVector3(worldMin[0], worldMin[1], worldMin[2]), btVector3(worldMax[0], worldMax[1], worldMax[2]), maxProxies);
}

void setGravity(btDiscreteDynamicsWorld* dynamicsWorld, float* gravity)
{
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
debugDraw(new BulletDebugDraw())
{
	sceneRoot = new gcroot<BulletScene^>(this);

	factory = gcnew BulletFactory(this);
	collisionConfiguration = new btDefaultCollisionConfiguration();
	dispatcher = new btCollisionDispatcher(collisionConfiguration);

	overlappingPairCache = createAxisSweep(&definition->WorldAabbMin.x, &definition->WorldAabbMax.x, definition->MaxProxies);
	solver = new btSequentialImpulseConstraintSolver;
	dynamicsWorld = new btDiscreteDynamicsWorld(dispatcher,overlappingPairCache,solver,collisionConfiguration);
	dynamicsWorld->setInternalTickCallback(BulletPlugin::tickCallback, static_cast<void*>(sceneRoot));
	setGravity(dynamicsWorld, &definition->Gravity.x);

	timer->addFixedUpdateListener(this);
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
	dynamicsWorld->stepSimulation((float)clock->Seconds);
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