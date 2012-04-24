#include "StdAfx.h"

extern "C" _AnomalousExport Rocket::Core::Event::EventPhase Event_GetPhase(Rocket::Core::Event* evt)
{
	return evt->GetPhase();
}

extern "C" _AnomalousExport void Event_SetPhase(Rocket::Core::Event* evt, Rocket::Core::Event::EventPhase phase)
{
	return evt->SetPhase(phase);
}

extern "C" _AnomalousExport void Event_SetCurrentElement(Rocket::Core::Event* evt, Rocket::Core::Element* element)
{
	return evt->SetCurrentElement(element);
}

extern "C" _AnomalousExport Rocket::Core::Element* Event_GetCurrentElement(Rocket::Core::Event* evt)
{
	return evt->GetCurrentElement();
}

extern "C" _AnomalousExport Rocket::Core::Element* Event_GetTargetElement(Rocket::Core::Event* evt)
{
	return evt->GetTargetElement();
}

extern "C" _AnomalousExport void Event_GetType(Rocket::Core::Event* evt, StringRetrieverCallback stringCb)
{
	stringCb(evt->GetType().CString());
}

extern "C" _AnomalousExport bool Event_IsPropagating(Rocket::Core::Event* evt)
{
	return evt->IsPropagating();
}

extern "C" _AnomalousExport void Event_StopPropagation(Rocket::Core::Event* evt)
{
	return evt->StopPropagation();
}

//template < typename T >
//T GetParameter(const String& key, const T& default_value)
//{
//	return parameters.Get(key, default_value);
//}

//const Dictionary* GetParameters() const;