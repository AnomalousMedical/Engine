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

extern "C" _AnomalousExport byte Event_GetParameter_Byte(Rocket::Core::Event* evt, String key, byte default_value)
{
	return evt->GetParameter<byte>(key, default_value);
}

extern "C" _AnomalousExport char Event_GetParameter_Char(Rocket::Core::Event* evt, String key, char default_value)
{
	return evt->GetParameter<char>(key, default_value);
}

extern "C" _AnomalousExport float Event_GetParameter_Float(Rocket::Core::Event* evt, String key, float default_value)
{
	return evt->GetParameter<float>(key, default_value);
}

extern "C" _AnomalousExport int Event_GetParameter_Int(Rocket::Core::Event* evt, String key, int default_value)
{
	return evt->GetParameter<int>(key, default_value);
}

extern "C" _AnomalousExport void Event_GetParameter_String(Rocket::Core::Event* evt, String key, StringRetrieverCallback setString)
{
	//This allows us to prevent marshaling of the default value.
	static Rocket::Core::String reservedDefaultString = "_NULL!";

	Rocket::Core::String& retrieved = evt->GetParameter<Rocket::Core::String>(key, reservedDefaultString);
	if(retrieved != reservedDefaultString)
	{
		setString(retrieved.CString());
	}
}

extern "C" _AnomalousExport Rocket::Core::word Event_GetParameter_Word(Rocket::Core::Event* evt, String key, Rocket::Core::word default_value)
{
	return evt->GetParameter<Rocket::Core::word>(key, default_value);
}

//BYTE = 'b',
//CHAR = 'c',
//FLOAT = 'f',
//INT = 'i', 
//STRING = 's',
//WORD = 'w',
//VECTOR2 = '2',