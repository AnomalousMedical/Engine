#pragma once

#include "ContactInfo.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace Engine;

namespace BulletPlugin
{

ref class ContactCache
{
private:
	GenericObjectPool<ContactInfo^> contactPool;
	Dictionary<unsigned long, ContactInfo^> liveContacts;
	List<ContactInfo^> finishedContacts;

internal:
	void queueRemoval(ContactInfo^ info);

	void replaceHead(ContactInfo^ newHead);

	void destroyHead(ContactInfo^ head);

public:
	ContactCache(void);

	void addManifold(btPersistentManifold* contactManifold);

	void dispatchContacts();
};

}