#include "StdAfx.h"
#include "..\include\ContactInfo.h"
#include "ContactCache.h"

namespace BulletPlugin
{

ContactInfo::ContactInfo(void)
:rbA(0),
rbB(0),
previous(nullptr),
next(nullptr)
{

}

void ContactInfo::reset()
{
	rbA = 0;
	rbB = 0;
	contactManifolds.Clear();
	next = nullptr;
	previous = nullptr;
}

void ContactInfo::process()
{
	//finished
	if(contactManifolds.Count == 0)
	{
		cache->queueRemoval(this);
	}
	else
	{
		/*MotionState* msA = static_cast<MotionState*>(btRbA->getMotionState());
	MotionState* msB = static_cast<MotionState*>(btRbB->getMotionState());
	RigidBody^ rbA = msA->getRigidBody();
	RigidBody^ rbB = msB->getRigidBody();*/
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

}