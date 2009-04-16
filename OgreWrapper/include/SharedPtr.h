#pragma once

namespace Rendering{

generic<typename T>
ref class SharedPtrCollection;

generic<typename T>
ref class SharedPtr
{
private:
	T value;
	SharedPtrCollection<T>^ owner;

internal:
	SharedPtr(T value, System::IntPtr nativePointer, SharedPtrCollection<T>^ owner)
		:value(value), nativePointer(nativePointer), st(true), owner(owner) {}

	System::Diagnostics::StackTrace st;

	System::IntPtr nativePointer;
public:
	virtual ~SharedPtr();

	property T Value
	{
		T get()
		{
			return value;
		}
	}
};

generic<typename T>
ref struct SharedPtrEntry
{
	int numReferences;
	T handle;
	SharedPtrEntry(T handle)
		: handle(handle) {}
	~SharedPtrEntry();
	System::Collections::Generic::LinkedList<SharedPtr<T>^> ptrList;
};

generic<typename T>
ref class SharedPtrCollection abstract
{
private:
	System::Collections::Generic::Dictionary<PtrType, SharedPtrEntry<T>^> ptrDictionary;

protected:
	SharedPtr<T>^ getObjectVoid(void* nativeObject, const void* sharedPtr, ...array<System::Object^>^ args);

	virtual T createWrapper(const void* sharedPtr, array<System::Object^>^ args){throw gcnew System::NotImplementedException();} //This class will not accept that it is abstract with this function so just throw the NotImplementedException if it is somehow called.

public:
	SharedPtrCollection();

	virtual ~SharedPtrCollection();

	void clearObjects();

	void returnPtr(SharedPtr<T>^ ptr);
};

}

/**
Usage template replace CLSNAME with the name of the shared pointer.

//header
#pragma once

#include "SharedPtr.h"

namespace Ogre
{
	class CLSNAMEPtr;
}

namespace Engine{

namespace Rendering{

ref class CLSNAME;
ref class CLSNAMEPtr;

ref class CLSNAMEPtrCollection : public SharedPtrCollection<CLSNAME^>
{
protected:
	virtual CLSNAME^ createWrapper(const void* sharedPtr, array<System::Object^>^ args) override;

public:
	CLSNAMEPtr^ getObject(const Ogre::CLSNAMEPtr& meshPtr);
};

/// <summary>
/// This class is a shared pointer. This means that it keeps the wrapper
/// instance alive by reference counting the number of times the reference is
/// retrieved. When using this class care must be taken to dispose the pointer
/// whenever it is returned. If this is not done the reference will leak and the
/// underlying Ogre object will leak until the program is shut down. Any leaks
/// that are detected will be printed in the log when the program is closed
/// along with a stack trace of where they were created. It is very important to
/// call dispose in the appropriate place if a leak occures.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class CLSNAMEPtr
{
private:
	SharedPtr<CLSNAME^>^ sharedPtr;
	CLSNAME^ object;

internal:
	CLSNAMEPtr(SharedPtr<CLSNAME^>^ sharedPtr);

public:
	virtual ~CLSNAMEPtr();

	property CLSNAME^ Value
	{
		CLSNAME^ get()
		{
			return object;
		}
	}
};

}

}

//source
#include "stdafx.h"
#include "CLSNAMEPtr.h"
#include "CLSNAME.h"

namespace Engine{

namespace Rendering{

CLSNAME^ CLSNAMEPtrCollection::createWrapper(const void* sharedPtr, array<System::Object^>^ args)
{
	return gcnew CLSNAME(*(const Ogre::CLSNAMEPtr*)sharedPtr);
}

CLSNAMEPtr^ CLSNAMEPtrCollection::getObject(const Ogre::CLSNAMEPtr& meshPtr)
{
	return gcnew CLSNAMEPtr(getObjectVoid(meshPtr.get(), &meshPtr));
}

CLSNAMEPtr::CLSNAMEPtr(SharedPtr<CLSNAME^>^ sharedPtr)
:sharedPtr(sharedPtr), object(sharedPtr->Value)
{

}

CLSNAMEPtr::~CLSNAMEPtr()
{
	delete sharedPtr;
}

}

}
*/