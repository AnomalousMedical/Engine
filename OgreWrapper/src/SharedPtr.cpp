#include "StdAfx.h"
#include "..\include\SharedPtr.h"

namespace OgreWrapper{

generic<typename T>
SharedPtr<T>::~SharedPtr()
{
	owner->returnPtr(this);
}

generic<typename T>
SharedPtrEntry<T>::~SharedPtrEntry()
{
	delete handle;
}

generic<typename T>
SharedPtrCollection<T>::SharedPtrCollection()
{

}

generic<typename T>
SharedPtrCollection<T>::~SharedPtrCollection()
{
	clearObjects();
}

generic<typename T>
SharedPtr<T>^ SharedPtrCollection<T>::getObjectVoid(void* ogreObject, const void* sharedPtr, ...array<System::Object^>^ args)
{
	PtrType key(ogreObject);
	if(!ptrDictionary.ContainsKey(key))
	{
		SharedPtrEntry<T>^ entry = gcnew SharedPtrEntry<T>(createWrapper(sharedPtr, args));
		ptrDictionary.Add(key, entry);
	}
	SharedPtrEntry<T>^ entry = ptrDictionary[key];
	entry->numReferences++;
	SharedPtr<T>^ sp = gcnew SharedPtr<T>(entry->handle, key, this);
	entry->ptrList.AddLast(sp);
	return sp;
}

generic<typename T>
void SharedPtrCollection<T>::returnPtr(SharedPtr<T>^ ptr)
{
	PtrType key = ptr->nativePointer;
	SharedPtrEntry<T>^ entry = ptrDictionary[key];
	entry->numReferences--;
	entry->ptrList.Remove(ptr);
	if(entry->numReferences == 0)
	{
		ptrDictionary.Remove(key);
		delete entry;
	}
}

generic<typename T>
void SharedPtrCollection<T>::clearObjects()
{
	System::String^ filename = System::Diagnostics::StackTrace(true).GetFrame(0)->GetFileName();
	for each(SharedPtrEntry<T>^ entry in ptrDictionary.Values)
	{
		Logging::Log::Default->sendMessage("Memory leak detected in {0}.  There were {1} instances of the pointer outstanding.  Double check to make sure all SharedPtrs of this type are being disposed.", Logging::LogLevel::Warning, "Rendering", filename, entry->numReferences);
		for each(SharedPtr<T>^ ptr in entry->ptrList)
		{
			Logging::Log::Default->sendMessage("Leaked pointer stack trace:", Logging::LogLevel::Warning, "Rendering");
			for each(System::Diagnostics::StackFrame^ f in ptr->st.GetFrames())
			{
				if(f->GetFileName() != nullptr)
				{
					Logging::Log::Default->sendMessage("-\t{0} in file {1}:{2}:{3}", Logging::LogLevel::Warning, "Rendering", f->GetMethod(), f->GetFileName(), f->GetFileLineNumber(), f->GetFileColumnNumber());
				}
			}
		}
		delete entry;
	}
	ptrDictionary.Clear();
}

}