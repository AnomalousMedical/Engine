#include "Stdafx.h"

extern "C" _AnomalousExport CEGUI::Scheme* SchemeManager_create(String scheme)
{
	return &CEGUI::SchemeManager::getSingleton().create(scheme);
}