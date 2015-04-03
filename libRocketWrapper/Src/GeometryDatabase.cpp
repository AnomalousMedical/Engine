#include "StdAfx.h"
#include "../Source/Core/GeometryDatabase.h"

extern "C" _AnomalousExport void GeometryDatabase_ReleaseGeometries()
{
	Rocket::Core::GeometryDatabase::ReleaseGeometries();
}