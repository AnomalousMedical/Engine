#include "StdAfx.h"

extern "C" _AnomalousExport int ReferenceCountable_GetReferenceCount(Rocket::Core::ReferenceCountable* refCount)
{
	return refCount->GetReferenceCount();
}

extern "C" _AnomalousExport void ReferenceCountable_AddReference(Rocket::Core::ReferenceCountable* refCount)
{
	return refCount->AddReference();
}

extern "C" _AnomalousExport void ReferenceCountable_RemoveReference(Rocket::Core::ReferenceCountable* refCount)
{
	return refCount->RemoveReference();
}

extern "C" _AnomalousExport void ReferenceCountable_DumpLeakReport()
{
	Rocket::Core::ReferenceCountable::DumpLeakReport();
}