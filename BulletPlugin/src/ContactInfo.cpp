#include "StdAfx.h"
#include "..\include\ContactInfo.h"
#include "ContactCache.h"
#include "MotionState.h"
#include "RigidBody.h"

namespace BulletPlugin
{

ContactInfo::ContactInfo(void)
:rbA(0),
rbB(0),
previous(nullptr),
next(nullptr),
pluginBodyA(nullptr),
pluginBodyB(nullptr),
firstFrame(true)
{

}

void ContactInfo::reset()
{
	rbA = 0;
	rbB = 0;
	contactManifolds.Clear();
	next = nullptr;
	previous = nullptr;
	pluginBodyA = nullptr;
	pluginBodyB = nullptr;
	firstFrame = true;
}

void ContactInfo::process()
{
	//finished
	if(contactManifolds.Count == 0)
	{
		pluginBodyA->fireContactEnded(this, pluginBodyB, true);
		pluginBodyA->fireContactEnded(this, pluginBodyA, false);
		cache->queueRemoval(this);
	}
	else
	{
		if(firstFrame)
		{
			pluginBodyA->fireContactStarted(this, pluginBodyB, true);
			pluginBodyA->fireContactStarted(this, pluginBodyA, false);
			firstFrame = false;
		}
		else
		{
			pluginBodyA->fireContactContinues(this, pluginBodyB, true);
			pluginBodyA->fireContactContinues(this, pluginBodyA, false);
		}
		contactManifolds.Clear();
	}
}

void ContactInfo::destroy()
{
	//In middle
	if(next != nullptr)
	{
		next->previous = this->previous;
		//current head
		if(previous == nullptr)
		{
			cache->replaceHead(next);
		}
		else
		{
			previous->next = next;
		}
	}
	//At end
	else
	{
		//current head with no next
		if(previous == nullptr)
		{
			cache->destroyHead(this);
		}
		else
		{
			previous->next = nullptr;
		}
	}
	returnToPool();
}

ContactInfo^ ContactInfo::findMatch(btRigidBody* rbA, btRigidBody* rbB)
{
	if(this->rbA == rbA && this->rbB == rbB)
	{
		return this;
	}
	else if(next != nullptr)
	{
		return next->findMatch(rbA, rbB);
	}
	else
	{
		return nullptr;
	}
}

void ContactInfo::add(ContactInfo^ info)
{
	if(next == nullptr)
	{
		next = info;
		info->previous = this;
	}
	else
	{
		next->add(info);
	}
}

void ContactInfo::addManifold(btPersistentManifold* manifold)
{
	contactManifolds.Add(IntPtr(manifold));
}


void ContactInfo::RbA::set(btRigidBody* value)
{
	rbA = value;
	MotionState* ms = static_cast<MotionState*>(value->getMotionState());
	pluginBodyA = ms->getRigidBody();
}

void ContactInfo::RbB::set(btRigidBody* value)
{
	rbB = value;
	MotionState* ms = static_cast<MotionState*>(value->getMotionState());
	pluginBodyB = ms->getRigidBody();
}

}