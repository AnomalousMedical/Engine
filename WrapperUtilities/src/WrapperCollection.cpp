#include "StdAfx.h"
#include "..\include\WrapperCollection.h"

generic<typename T>
WrapperCollection<T>::WrapperCollection()
{

}

generic<typename T>
WrapperCollection<T>::~WrapperCollection()
{
	clearObjects();
}

generic<typename T>
T WrapperCollection<T>::getObjectVoid(void* nativeObject, ...array<System::Object^>^ args)
{
	PtrType key(nativeObject);
	if(!ptrDictionary.ContainsKey(key))
	{
		ptrDictionary.Add(key, createWrapper(nativeObject, args));
	}
	return ptrDictionary[key];
}

generic<typename T>
bool WrapperCollection<T>::getObjectVoidNoCreate(void* nativeObject, T% object)
{
	PtrType key(nativeObject);
	if(ptrDictionary.ContainsKey(key))
	{
		object = ptrDictionary[key];
		return true;
	}
	return false;
}

generic<typename T>
void WrapperCollection<T>::destroyObjectVoid(void* ogreObject)
{
	PtrType key(ogreObject);
	if(ptrDictionary.ContainsKey(key))
	{
		delete ptrDictionary[key];
		ptrDictionary.Remove(key);
	}
}

generic<typename T>
void WrapperCollection<T>::clearObjects()
{
	for each(T obj in ptrDictionary.Values)
	{
		delete obj;
	}
	ptrDictionary.Clear();
}