#pragma once

#include "ContactInfo.h"
#include <map>
#include <vector>
#include <stack>

typedef std::map<unsigned long, ContactInfo*> ContactMap;
typedef ContactMap::iterator ContactMapIter;

typedef std::vector<ContactInfo*> ContactInfoVector;
typedef ContactInfoVector::iterator ContactInfoVectorIter;

typedef std::stack<ContactInfo*> ContactInfoStack;

class ContactCache
{
private:
	/*GenericObjectPool<ContactInfo^> contactPool;
	Dictionary<unsigned long, ContactInfo^> liveContacts;
	List<ContactInfo^> finishedContacts;*/
	ContactInfoStack contactPool;
	ContactMap liveContacts;
	ContactInfoVector finishedContacts;

public:
	ContactCache(void);

	~ContactCache(void);

	void queueRemoval(ContactInfo* info);

	void replaceHead(ContactInfo* newHead);

	void destroyHead(ContactInfo* head);

	void addManifold(btPersistentManifold* contactManifold);

	void dispatchContacts();

	void returnToPool(ContactInfo* info);

	ContactInfo* getPooledObject();
};
