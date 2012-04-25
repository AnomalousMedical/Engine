#include "StdAfx.h"

extern "C" _AnomalousExport void Dictionary_Set(Rocket::Core::Dictionary* dictionary, String key, Rocket::Core::Variant* value)
{
	dictionary->Set(key, value);
}
	
extern "C" _AnomalousExport Rocket::Core::Variant* Dictionary_Get(Rocket::Core::Dictionary* dictionary, String key)
{
	return dictionary->Get(key);
}

extern "C" _AnomalousExport bool Dictionary_Remove(Rocket::Core::Dictionary* dictionary, String key)
{
	return dictionary->Remove(key);
}

extern "C" _AnomalousExport bool Dictionary_Iterate(Rocket::Core::Dictionary* dictionary, int &pos, StringRetrieverCallback keyCb, Rocket::Core::Variant* &value)
{
	Rocket::Core::String rktKey;
	bool retVal = dictionary->Iterate(pos, rktKey, value);
	keyCb(rktKey.CString());
	return retVal;
}

extern "C" _AnomalousExport bool Dictionary_Reserve(Rocket::Core::Dictionary* dictionary, int &size)
{
	return dictionary->Reserve(size);
}

extern "C" _AnomalousExport void Dictionary_Clear(Rocket::Core::Dictionary* dictionary)
{
	dictionary->Clear();
}

extern "C" _AnomalousExport bool Dictionary_IsEmpty(Rocket::Core::Dictionary* dictionary)
{
	return dictionary->IsEmpty();
}

extern "C" _AnomalousExport int Dictionary_Size(Rocket::Core::Dictionary* dictionary)
{
	return dictionary->Size();
}

extern "C" _AnomalousExport void Dictionary_Merge(Rocket::Core::Dictionary* dictionary, Rocket::Core::Dictionary* dict)
{
	dictionary->Merge(*dict);
}

//template <typename T>
//inline T Get(const String& key, const T& default_val) const;
	
//template <typename T>
//inline bool GetInto(const String& key, T& value) const;

///// Templated set eases setting of values
//template <typename T>
//inline void Set(const String& key, const T& value);

//template <typename T>
//bool Iterate(int &pos, String& key, T& value) const;