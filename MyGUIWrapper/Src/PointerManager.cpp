#include "stdafx.h"

extern "C" _AnomalousExport MyGUI::PointerManager* PointerManager_getInstancePtr()
{
	return MyGUI::PointerManager::getInstancePtr();
}

extern "C" _AnomalousExport void PointerManager_setPointer(MyGUI::PointerManager* pointerManager, String name)
{
	return pointerManager->setPointer(name);
}

extern "C" _AnomalousExport void PointerManager_resetToDefaultPointer(MyGUI::PointerManager* pointerManager)
{
	return pointerManager->resetToDefaultPointer();
}

extern "C" _AnomalousExport void PointerManager_setVisible(MyGUI::PointerManager* pointerManager, bool visible)
{
	return pointerManager->setVisible(visible);
}

extern "C" _AnomalousExport bool PointerManager_isVisible(MyGUI::PointerManager* pointerManager)
{
	return pointerManager->isVisible();
}

extern "C" _AnomalousExport String PointerManager_getDefaultPointer(MyGUI::PointerManager* pointerManager)
{
	return pointerManager->getDefaultPointer().c_str();
}

extern "C" _AnomalousExport void PointerManager_setDefaultPointer(MyGUI::PointerManager* pointerManager, String value)
{
	return pointerManager->setDefaultPointer(value);
}

extern "C" _AnomalousExport String PointerManager_getLayerName(MyGUI::PointerManager* pointerManager)
{
	return pointerManager->getLayerName().c_str();
}

extern "C" _AnomalousExport void PointerManager_setLayerName(MyGUI::PointerManager* pointerManager, String value)
{
	return pointerManager->setLayerName(value);
}