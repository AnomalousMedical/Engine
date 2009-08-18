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
dispatchStartA(false),
dispatchStartB(false),
manifoldArray(gcnew cli::array<btPersistentManifold*>(10)),
numManifolds(0),
closestPoint(Single::MaxValue)
{

}

void ContactInfo::reset()
{
	rbA = 0;
	rbB = 0;
	next = nullptr;
	previous = nullptr;
	pluginBodyA = nullptr;
	pluginBodyB = nullptr;
	dispatchStartA = false;
	dispatchStartB = false;
	numManifolds = 0;
	closestPoint = Single::MaxValue;
}

void ContactInfo::process()
{
	//finished
	if(numManifolds == 0)
	{
		if(dispatchStartA)
		{
			pluginBodyA->fireContactEnded(this, pluginBodyB, true);
		}
		if(dispatchStartB)
		{
			pluginBodyB->fireContactEnded(this, pluginBodyA, false);
		}
		cache->queueRemoval(this);
	}
	else
	{
		//Body A
		if(closestPoint <= pluginBodyA->MaxContactDistance)
		{
			if(dispatchStartA)
			{
				pluginBodyA->fireContactContinues(this, pluginBodyB, true);
			}
			else
			{
				pluginBodyA->fireContactStarted(this, pluginBodyB, true);
				dispatchStartA = true;
			}
		}
		else if(dispatchStartA)
		{
			pluginBodyA->fireContactEnded(this, pluginBodyB, true);
			dispatchStartA = false;
		}

		//Body B
		if(closestPoint <= pluginBodyB->MaxContactDistance)
		{
			if(dispatchStartB)
			{
				pluginBodyB->fireContactContinues(this, pluginBodyA, false);
			}
			else
			{
				pluginBodyB->fireContactStarted(this, pluginBodyA, false);
				dispatchStartB = true;
			}
		}
		else if(dispatchStartB)
		{
			pluginBodyB->fireContactEnded(this, pluginBodyA, false);
			dispatchStartB = false;
		}
	}
	numManifolds = 0;
	closestPoint = Single::MaxValue;
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

void ContactInfo::addManifold(btPersistentManifold* contactManifold)
{
	if(numManifolds < manifoldArray->Length)
	{	
		int numPoints = contactManifold->getNumContacts();
		for(int i = 0; i < numPoints; ++i)
		{
			btManifoldPoint& pt = contactManifold->getContactPoint(i);
			float dis = pt.getDistance();
			if(dis < closestPoint)
			{
				closestPoint = dis;
			}
		}
		manifoldArray[numManifolds++] = contactManifold;
	}
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