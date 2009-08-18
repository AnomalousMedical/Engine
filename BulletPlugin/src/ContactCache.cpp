#include "StdAfx.h"
#include "..\include\ContactCache.h"

namespace BulletPlugin
{

ContactCache::ContactCache(void)
{
}

void ContactCache::addManifold(btPersistentManifold* contactManifold)
{
	bool add = false;
	int numPoints = contactManifold->getNumContacts();
	for(int i = 0; i < numPoints; ++i)
	{
		btManifoldPoint& pt = contactManifold->getContactPoint(i);
		if(pt.getDistance() < 0.0f)
		{
			add = true;
			break;
		}
	}
	if(add)
	{
		btRigidBody* btRbA = static_cast<btRigidBody*>(contactManifold->getBody0());
		btRigidBody* btRbB = static_cast<btRigidBody*>(contactManifold->getBody1());
		unsigned long sumPtr = (unsigned long)btRbA + (unsigned long)btRbB;
		ContactInfo^ info = nullptr;
		ContactInfo^ head = nullptr;
		liveContacts.TryGetValue(sumPtr, head);
		if(head != nullptr)
		{
			info = head->findMatch(btRbA, btRbB);
		}
		if(info == nullptr)
		{
			info = contactPool.getPooledObject();
			info->RbA = btRbA;
			info->RbB = btRbB;
			info->Key = sumPtr;
			info->Cache = this;
			if(head == nullptr)
			{
				liveContacts.Add(sumPtr, info);
			}
			else
			{
				head->add(info);
			}
		}
		info->addManifold(contactManifold);
	}
}

void ContactCache::dispatchContacts()
{
	for each(ContactInfo^ info in liveContacts.Values)
	{
		info->process();
	}
	for each(ContactInfo^ info in finishedContacts)
	{
		info->destroy();
	}
	finishedContacts.Clear();
}

void ContactCache::queueRemoval(ContactInfo^ info)
{
	finishedContacts.Add(info);
}

void ContactCache::replaceHead(ContactInfo^ newHead)
{
	liveContacts[newHead->Key] = newHead;
}

void ContactCache::destroyHead(ContactInfo^ head)
{
	liveContacts.Remove(head->Key);
}

}