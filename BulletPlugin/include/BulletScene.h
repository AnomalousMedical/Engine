#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Platform;

namespace BulletPlugin
{

class BulletDebugDraw;

ref class BulletSceneDefinition;
ref class BulletFactory;

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
	btDiscreteDynamicsWorld* dynamicsWorld;

internal:
	BulletScene(BulletSceneDefinition^ definition, UpdateTimer^ timer);

	BulletFactory^ getBulletFactory();

	property btDiscreteDynamicsWorld* DynamicsWorld
	{
		btDiscreteDynamicsWorld* get()
		{
			return dynamicsWorld;
		}
	}

public:
	virtual ~BulletScene(void);

	virtual SimElementFactory^ getFactory();

	virtual Type^ getSimElementManagerType();

	virtual String^ getName();

	virtual SimElementManagerDefinition^ createDefinition();

	virtual void sendUpdate(Clock^ clock);

	virtual void loopStarting();

	virtual void exceededMaxDelta();
};

}