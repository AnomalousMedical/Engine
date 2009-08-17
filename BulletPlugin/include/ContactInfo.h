#pragma once

using namespace Engine;
using namespace System;
using namespace System::Collections::Generic;

namespace BulletPlugin
{

ref class ContactCache;

ref class ContactInfo : public PooledObject
{
private:
	btRigidBody* rbA;
	btRigidBody* rbB;
	List<IntPtr> contactManifolds;
	unsigned long key;
	ContactInfo^ previous;
	ContactInfo^ next;
	ContactCache^ cache;

internal:
	property btRigidBody* RbA
	{
		btRigidBody* get()
		{
			return rbA;
		}
		void set(btRigidBody* value)
		{
			rbA = value;
		}
	}

	property btRigidBody* RbB
	{
		btRigidBody* get()
		{
			return rbB;
		}
		void set(btRigidBody* value)
		{
			rbB = value;
		}
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

internal:
	void process();

	void destroy();

	ContactInfo^ findMatch(btRigidBody* rbA, btRigidBody* rbB);

	void add(ContactInfo^ info);

	void addManifold(btPersistentManifold* manifold);

public:
	ContactInfo(void);

	virtual void reset() override;
};

}