#include "Stdafx.h"

extern "C" _AnomalousExport CEGUI::SchemeManager* SchemeManager_getSingletonPtr()
{
	return CEGUI::SchemeManager::getSingletonPtr();
}

extern "C" _AnomalousExport CEGUI::Scheme* SchemeManager_create(CEGUI::SchemeManager* schemeManager, String scheme)
{
	return &schemeManager->create(scheme);
}