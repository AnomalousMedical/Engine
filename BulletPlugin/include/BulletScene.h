#pragma once

#include "vcclr.h"
#include "ContactCache.h"

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Platform;
using namespace Engine::Renderer;

//#define USE_PARALLEL_DISPATCHER 1

#ifdef USE_PARALLEL_DISPATCHER
class Win32ThreadSupport;
#endif

#ifdef USE_SOFTBODY_WORLD
struct btSoftBodyWorldInfo;
#endif

namespace BulletPlugin
{

class BulletDebugDraw;

ref class BulletSceneDefinition;
ref class BulletFactory;
ref class SoftBodyProvider;

[Engine::Attributes::NativeSubsystemType]
public ref class BulletScene : public SimElementManager, UpdateListener
{
private:
	String^ name;
	UpdateTimer^ timer;
	BulletFactory^ factory;
	int maxProxies;
	BulletDebugDraw* debugDraw;

	btDefaultCollisionConfiguration* collisionConfiguration;
	btCollisionDispatcher* dispatcher;
	btAxisSweep3* overlappingPairCache;
	btSequentialImpulseConstraintSolver* solver;

	//List of soft body providers to be called back for update
	List<SoftBodyProvider^> softBodyProviders;

#ifdef USE_SOFTBODY_WORLD
	btSoftRigidDynamicsWorld* dynamicsWorld;
	btSoftBodyWorldInfo* softBodyWorldInfo;
#else
	btDiscreteDynamicsWorld* dynamicsWorld;
#endif

	gcroot<BulletScene^>* sceneRoot;
	ContactCache contactCache;
	float internalTimestep;

	#ifdef USE_PARALLEL_DISPATCHER
	Win32ThreadSupport* m_threadSupportCollision;
	#endif

internal:
	BulletScene(BulletSceneDefinition^ definition, UpdateTimer^ timer);

	BulletFactory^ getBulletFactory();

	void tickCallback(btScalar timeStep);

	void addSoftBodyProvider(SoftBodyProvider^ sbProvider);

	void removeSoftBodyProvider(SoftBodyProvider^ sbProvider);

#ifdef USE_SOFTBODY_WORLD
	property btSoftRigidDynamicsWorld* DynamicsWorld
	{
		btSoftRigidDynamicsWorld* get()
		{
			return dynamicsWorld;
		}
	}

	property btSoftBodyWorldInfo* SoftBodyWorldInfo
	{
		btSoftBodyWorldInfo* get()
		{
			return softBodyWorldInfo;
		}
	}
#else
	property btDiscreteDynamicsWorld* DynamicsWorld
	{
		btDiscreteDynamicsWorld* get()
		{
			return dynamicsWorld;
		}
	}
#endif

public:
	virtual ~BulletScene(void);

	virtual SimElementFactory^ getFactory();

	virtual Type^ getSimElementManagerType();

	virtual String^ getName();

	virtual SimElementManagerDefinition^ createDefinition();

	virtual void sendUpdate(Clock^ clock);

	virtual void loopStarting();

	virtual void exceededMaxDelta();

	void drawDebug(DebugDrawingSurface^ drawingSurface);

	#ifdef USE_SOFTBODY_WORLD
	property void* SoftBodyWorldInfoExternal
	{
		void* get()
		{
			return softBodyWorldInfo;
		}
	}
	#endif
};

}