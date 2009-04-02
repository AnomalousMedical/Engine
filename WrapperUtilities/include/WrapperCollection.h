#pragma once

typedef System::IntPtr PtrType;

generic<typename T>
public ref class WrapperCollection abstract
{
private:
	System::Collections::Generic::Dictionary<PtrType, T> ptrDictionary;

protected:
	T getObjectVoid(void* nativeObject, ...array<System::Object^>^ args);

	bool getObjectVoidNoCreate(void* nativeObject, T% object);

	void destroyObjectVoid(void* nativeObject);

	virtual T createWrapper(void* nativeObject, array<System::Object^>^ args) = 0;

public:
	WrapperCollection();

	virtual ~WrapperCollection();

	void clearObjects();
};

/**
Template for subclasses:
//Header
#pragma once

namespace 
{

ref class WRAPPED_CLASS;
ref class WRAPPED_CLASSCollection : public WrapperCollection<WRAPPED_CLASS^>
{
protected:
	virtual WRAPPED_CLASS^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~WRAPPED_CLASSCollection() {}

	WRAPPED_CLASS^ getObject(WRAPPED_CLASS* nativeObject);

	void destroyObject(WRAPPED_CLASS* nativeObject);
};

}

//Source
#include "stdafx.h"
#include "WRAPPED_CLASSCollection.h"
#include "WRAPPED_CLASS.h"

namespace 
{

WRAPPED_CLASS^ WRAPPED_CLASSCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew WRAPPED_CLASS(static_cast<WRAPPED_CLASS*>(nativeObject));
}

WRAPPED_CLASS^ WRAPPED_CLASSCollection::getObject(WRAPPED_CLASS* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void WRAPPED_CLASSCollection::destroyObject(WRAPPED_CLASS* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}
*/