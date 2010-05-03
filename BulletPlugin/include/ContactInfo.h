#pragma once

using namespace Engine;
using namespace System;
using namespace System::Collections::Generic;

namespace BulletPlugin
{

ref class ContactCache;
ref class RigidBody;
ref class ManifoldPoint;

public ref class ContactInfo : public PooledObject
{
private:
	btRigidBody* rbA;
	btRigidBody* rbB;
	unsigned long key;
	ContactInfo^ previous;
	ContactInfo^ next;
	ContactCache^ cache;
	RigidBody^ pluginBodyA;
	RigidBody^ pluginBodyB;
	bool dispatchStartA;	//True if RigidBodyA has gotten a ContactStarted Event
	bool dispatchStartB;	//True if RigidBodyB has gotten a ContactStarted Event
	float closestPoint;		//The closest contact point found this frame
	cli::array<btPersistentManifold*>^ manifoldArray;
	int numManifolds;

internal:
	property btRigidBody* RbA
	{
		btRigidBody* get()
		{
			return rbA;
		}
		void set(btRigidBody* value);
	}

	property btRigidBody* RbB
	{
		btRigidBody* get()
		{
			return rbB;
		}
		void set(btRigidBody* value);
	}

	property unsigned long Key
	{
		unsigned long get()
		{
			return key;
		}
		void set(unsigned long value)
		{
			key = value;
		}
	}

	property ContactCache^ Cache
	{
		ContactCache^get()
		{
			return cache;
		}
		void set(ContactCache^ value)
		{
			cache = value;
		}
	}

protected:
	virtual void reset() override;

internal:
	void process();

	void destroy();

	ContactInfo^ findMatch(btRigidBody* rbA, btRigidBody* rbB);

	void add(ContactInfo^ info);

	void addManifold(btPersistentManifold* contactManifold);

public:
	ContactInfo(void);

	property RigidBody^ RigidBodyA
	{
		RigidBody^ get()
		{
			return pluginBodyA;
		}
	}

	property RigidBody^ RigidBodyB
	{
		RigidBody^ get()
		{
			return pluginBodyB;
		}
	}

	int getNumContacts();

	void getContactPoint(int index, ManifoldPoint^ point);
};

}