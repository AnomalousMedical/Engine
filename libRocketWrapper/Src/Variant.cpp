#include "StdAfx.h"

extern "C" _AnomalousExport Rocket::Core::Variant* Variant_Create()
{
	return new Rocket::Core::Variant();
}

extern "C" _AnomalousExport void Variant_Delete(Rocket::Core::Variant* variant)
{
	delete variant;
}

extern "C" _AnomalousExport void Variant_Clear(Rocket::Core::Variant* variant)
{
	return variant->Clear();
}

extern "C" _AnomalousExport Rocket::Core::Variant::Type Variant_GetType(Rocket::Core::Variant* variant)
{
	return variant->GetType();
}

extern "C" _AnomalousExport void Variant_Set_Byte(Rocket::Core::Variant* variant, byte value)
{
	return variant->Set(value);
}

extern "C" _AnomalousExport void Variant_Set_Char(Rocket::Core::Variant* variant, char value)
{
	return variant->Set(value);
}

extern "C" _AnomalousExport void Variant_Set_Float(Rocket::Core::Variant* variant, float value)
{
	return variant->Set(value);
}

extern "C" _AnomalousExport void Variant_Set_Int(Rocket::Core::Variant* variant, int value)
{
	return variant->Set(value);
}

extern "C" _AnomalousExport void Variant_Set_Word(Rocket::Core::Variant* variant, Rocket::Core::word value)
{
	return variant->Set(value);
}

extern "C" _AnomalousExport void Variant_Set_String(Rocket::Core::Variant* variant, String value)
{
	return variant->Set(value);
}

extern "C" _AnomalousExport byte Variant_Get_Byte(Rocket::Core::Variant* variant)
{
	return variant->Get<byte>();
}

extern "C" _AnomalousExport char Variant_Get_Char(Rocket::Core::Variant* variant)
{
	return variant->Get<char>();
}

extern "C" _AnomalousExport float Variant_Get_Float(Rocket::Core::Variant* variant)
{
	return variant->Get<float>();
}

extern "C" _AnomalousExport int Variant_Get_Int(Rocket::Core::Variant* variant)
{
	return variant->Get<int>();
}

extern "C" _AnomalousExport Rocket::Core::word Variant_Get_Word(Rocket::Core::Variant* variant)
{
	return variant->Get<Rocket::Core::word>();
}

extern "C" _AnomalousExport void Variant_Get_String(Rocket::Core::Variant* variant, StringRetrieverCallback stringCb)
{
	stringCb(variant->Get<Rocket::Core::String>().CString());
}