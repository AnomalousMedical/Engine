#include "StdAfx.h"
#include "..\include\ContactInfo.h"
#include "ContactCache.h"
#include "MotionState.h"
#include "RigidBody.h"
#include "ManifoldPoint.h"

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
	//Process the next value
	if(next != nullptr)
	{
		next->process();
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

void ContactInfo::addManifold(btPersistentManifold* contactManifold)
{	
	bool addManifold = false;
	int numPoints = contactManifold->getNumContacts();
	for(int i = 0; i < numPoints; ++i)
	{
		btManifoldPoint& pt = contactManifold->getContactPoint(i);
		float dis = pt.getDistance();
		if(dis < pluginBodyA->MaxContactDistance || dis < pluginBodyB->MaxContactDistance)
		{
			addManifold = true;
		}
		if(dis < closestPoint)
		{
			closestPoint = dis;
		}
	}
	//Add the manifold if its point is below one of the contact distances and
	//slots are avaliable.
	if(addManifold && numManifolds < manifoldArray->Length)
	{
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

int ContactInfo::getNumContacts()
{
	int totalContacts = 0;
	for(int i = 0; i < numManifolds; ++i)
	{
		totalContacts += manifoldArray[i]->getNumContacts();
	}
	return totalContacts;
}

void ContactInfo::getContactPoint(int index, ManifoldPoint^ point)
{
	for(int i = 0; i < numManifolds; ++i)
	{
		int numContacts = manifoldArray[i]->getNumContacts();
		if(index < numContacts)
		{
			point->setInfo(manifoldArray[i]->getContactPoint(index));
			return;
		}
		else
		{
			index -= numContacts;
		}
	}
}

}