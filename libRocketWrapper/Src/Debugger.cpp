#include "StdAfx.h"

extern "C" _AnomalousExport void Debugger_Initialise(Rocket::Core::Context* con)
{
	Rocket::Debugger::Initialise(con);
}

extern "C" _AnomalousExport bool Debugger_SetContext(Rocket::Core::Context* context)
{
	return Rocket::Debugger::SetContext(context);
}

extern "C" _AnomalousExport void Debugger_SetVisible(bool visibility)
{
	Rocket::Debugger::SetVisible(visibility);
}

extern "C" _AnomalousExport bool Debugger_IsVisible()
{
	return Rocket::Debugger::IsVisible();
}