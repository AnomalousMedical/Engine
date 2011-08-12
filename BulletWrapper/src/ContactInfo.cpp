#include "StdAfx.h"
#include "../Include/ContactInfo.h"
#include "float.h"
#include "ContactCache.h"
#include "MotionState.h"

ContactInfo::ContactInfo(void)
:rbA(0),
rbB(0),
previous(0),
next(0),
dispatchStartA(false),
dispatchStartB(false),
numManifolds(0),
closestPoint(FLT_MAX),
manifoldArray(),
bodyAMotion(0),
bodyBMotion(0)
{
	manifoldArray.resize(10);
}

ContactInfo::~ContactInfo(void)
{
}

void ContactInfo::reset()
{
	rbA = 0;
	rbB = 0;
	next = 0;
	previous = 0;
	dispatchStartA = false;
	dispatchStartB = false;
	numManifolds = 0;
	closestPoint = FLT_MAX;
	bodyAMotion = 0;
	bodyBMotion = 0;
}

void ContactInfo::process()
{
	//finished
	if(numManifolds == 0)
	{
		if(dispatchStartA)
		{
			bodyAMotion->fireContactStopped(this, rbA, rbB, true);
			//pluginBodyA->fireContactEnded(this, pluginBodyB, true);
		}
		if(dispatchStartB)
		{
			bodyBMotion->fireContactStopped(this, rbB, rbA, false);
			//pluginBodyB->fireContactEnded(this, pluginBodyA, false);
		}
		cache->queueRemoval(this);
	}
	else
	{
		//Body A
		if(closestPoint <= bodyAMotion->maxContactDistance)
		{
			if(dispatchStartA)
			{
				bodyAMotion->fireContactContinues(this, rbA, rbB, true);
				//pluginBodyA->fireContactContinues(this, pluginBodyB, true);
			}
			else
			{
				bodyAMotion->fireContactStarted(this, rbA, rbB, true);
				//pluginBodyA->fireContactStarted(this, pluginBodyB, true);
				dispatchStartA = true;
			}
		}
		else if(dispatchStartA)
		{
			bodyAMotion->fireContactStopped(this, rbA, rbB, true);
			//pluginBodyA->fireContactEnded(this, pluginBodyB, true);
			dispatchStartA = false;
		}

		//Body B
		if(closestPoint <= bodyBMotion->maxContactDistance)
		{
			if(dispatchStartB)
			{
				bodyBMotion->fireContactContinues(this, rbB, rbA, false);
				//pluginBodyB->fireContactContinues(this, pluginBodyA, false);
			}
			else
			{
				bodyBMotion->fireContactStarted(this, rbB, rbA, false);
				//pluginBodyB->fireContactStarted(this, pluginBodyA, false);
				dispatchStartB = true;
			}
		}
		else if(dispatchStartB)
		{
			bodyBMotion->fireContactStopped(this, rbB, rbA, false);
			//pluginBodyB->fireContactEnded(this, pluginBodyA, false);
			dispatchStartB = false;
		}
	}
	numManifolds = 0;
	closestPoint = FLT_MAX;
	//Process the next value
	if(next != 0)
	{
		next->process();
	}
}

void ContactInfo::fireContactEndedOnBodyDelete(btRigidBody* deletingBody)
{
	//Only delete if we are matching the body.
	if(rbA == deletingBody || rbB == deletingBody)
	{
		if(dispatchStartA)
		{
			bodyAMotion->fireContactStopped(this, rbA, rbB, true);
			//pluginBodyA->fireContactEnded(this, pluginBodyB, true);
		}
		if(dispatchStartB)
		{
			bodyBMotion->fireContactStopped(this, rbB, rbA, false);
			//pluginBodyB->fireContactEnded(this, pluginBodyA, false);
		}
		cache->queueRemoval(this);
	}
	//Check next for more stuff to delete
	if(next != 0)
	{
		next->fireContactEndedOnBodyDelete(deletingBody);
	}
}

void ContactInfo::destroy()
{
	//In middle
	if(next != 0)
	{
		next->previous = this->previous;
		//current head
		if(previous == 0)
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
		if(previous == 0)
		{
			cache->destroyHead(this);
		}
		else
		{
			previous->next = 0;
		}
	}
	cache->returnToPool(this);
}

ContactInfo* ContactInfo::findMatch(btRigidBody* rbA, btRigidBody* rbB)
{
	if(this->rbA == rbA && this->rbB == rbB)
	{
		return this;
	}
	else if(next != 0)
	{
		return next->findMatch(rbA, rbB);
	}
	else
	{
		return 0;
	}
}

void ContactInfo::add(ContactInfo* info)
{
	if(next == 0)
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
		if(dis < bodyAMotion->maxContactDistance || dis < bodyBMotion->maxContactDistance)
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
	if(addManifold && numManifolds < manifoldArray.capacity())
	{
		manifoldArray[numManifolds++] = contactManifold;
	}
}

void ContactInfo::setValues(btRigidBody* btRbA, btRigidBody* btRbB, unsigned long sumPtr, ContactCache* cache)
{
	this->rbA = btRbA;
	this->rbB = btRbB;
	this->key = sumPtr;
	this->cache = cache;
	this->bodyAMotion = dynamic_cast<MotionState*>(btRbA->getMotionState());
	this->bodyBMotion = dynamic_cast<MotionState*>(btRbB->getMotionState());
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

void ContactInfo::startPointIterator()
{
	currentManifold = 0;
	currentPoint = 0;
}

bool ContactInfo::hasNextPoint()
{
	return currentManifold < numManifolds;
}

btManifoldPoint* ContactInfo::nextPoint()
{
	if(currentPoint < manifoldArray[currentManifold]->getNumContacts())
	{
		return &manifoldArray[currentManifold]->getContactPoint(currentPoint);
	}
	else
	{
		currentPoint = 0;
		if(++currentManifold < numManifolds)
		{
			return nextPoint();
		}
	}
}

extern "C" _AnomalousExport int ContactInfo_getNumContacts(ContactInfo* contactInfo)
{
	return contactInfo->getNumContacts();
}

extern "C" _AnomalousExport void ContactInfo_startPointIterator(ContactInfo* contactInfo)
{
	contactInfo->startPointIterator();
}

extern "C" _AnomalousExport bool ContactInfo_hasNextPoint(ContactInfo* contactInfo)
{
	return contactInfo->hasNextPoint();
}

extern "C" _AnomalousExport btManifoldPoint* ContactInfo_nextPoint(ContactInfo* contactInfo)
{
	return contactInfo->nextPoint();
}