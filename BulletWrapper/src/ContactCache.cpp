#include "StdAfx.h"
#include "../Include/ContactCache.h"

ContactCache::ContactCache(void)
{
}

ContactCache::~ContactCache(void)
{
}

void ContactCache::queueRemoval(ContactInfo* info)
{
	finishedContacts.push_back(info);
}

void ContactCache::replaceHead(ContactInfo* newHead)
{
	liveContacts[newHead->key] = newHead;
}

void ContactCache::destroyHead(ContactInfo* head)
{
	liveContacts.erase(head->key);
}

void ContactCache::addManifold(btPersistentManifold* contactManifold)
{
	btRigidBody* btRbA = static_cast<btRigidBody*>(const_cast<btCollisionObject*>(contactManifold->getBody0()));
	btRigidBody* btRbB = static_cast<btRigidBody*>(const_cast<btCollisionObject*>(contactManifold->getBody1()));
	unsigned long sumPtr = (unsigned long)btRbA + (unsigned long)btRbB;
	ContactInfo* info = 0;
	ContactMapIter head = liveContacts.find(sumPtr);
	//Something with sumPtr is already in the map so search for these bodies
	if(head != liveContacts.end())
	{
		info = head->second->findMatch(btRbA, btRbB);
	}
	//Need to make a new ContactInfo
	if(info == 0)
	{
		info = getPooledObject();
		info->setValues(btRbA, btRbB, sumPtr, this);
		if(head == liveContacts.end())
		{
			liveContacts[sumPtr] = info;
		}
		else
		{
			head->second->add(info);
		}
	}
	info->addManifold(contactManifold);
}

void ContactCache::dispatchContacts()
{
	for(ContactMapIter mapIter = liveContacts.begin(); mapIter != liveContacts.end(); ++mapIter)
	{
		mapIter->second->process();
	}
	for(ContactInfoVectorIter vecIter = finishedContacts.begin(); vecIter != finishedContacts.end(); ++vecIter)
	{
		(*vecIter)->destroy();
	}
	finishedContacts.clear();
}

void ContactCache::removeRigidBodyContacts(btRigidBody* rigidBody)
{
	//Search for body
	for(ContactMapIter mapIter = liveContacts.begin(); mapIter != liveContacts.end(); ++mapIter)
	{
			mapIter->second->fireContactEndedOnBodyDelete(rigidBody);
	}

	//destroy contacts.
	for(ContactInfoVectorIter vecIter = finishedContacts.begin(); vecIter != finishedContacts.end(); ++vecIter)
	{
		(*vecIter)->destroy();
	}
	finishedContacts.clear();
}

void ContactCache::returnToPool(ContactInfo* info)
{
	info->reset();
	contactPool.push(info);
}

ContactInfo* ContactCache::getPooledObject()
{
	if(contactPool.empty())
	{
		return new ContactInfo();
	}
	ContactInfo* info = contactPool.top();
	contactPool.pop();
	return info;
}