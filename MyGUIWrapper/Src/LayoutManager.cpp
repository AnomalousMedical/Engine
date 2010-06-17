#include "Stdafx.h"

extern "C" _AnomalousExport MyGUI::LayoutManager* LayoutManager_getInstancePtr()
{
	return MyGUI::LayoutManager::getInstancePtr();
}

extern "C" _AnomalousExport MyGUI::VectorWidgetPtr* LayoutManager_loadLayout(MyGUI::LayoutManager* layoutManager, String file)
{
	return new MyGUI::VectorWidgetPtr(layoutManager->loadLayout(file));
}

extern "C" _AnomalousExport MyGUI::VectorWidgetPtr* LayoutManager_loadLayout2(MyGUI::LayoutManager* layoutManager, String file, String prefix)
{
	return new MyGUI::VectorWidgetPtr(layoutManager->loadLayout(file));
}

extern "C" _AnomalousExport MyGUI::VectorWidgetPtr* LayoutManager_loadLayout3(MyGUI::LayoutManager* layoutManager, String file, String prefix, MyGUI::Widget* parent)
{
	return new MyGUI::VectorWidgetPtr(layoutManager->loadLayout(file));
}

extern "C" _AnomalousExport void LayoutManager_unloadLayout(MyGUI::LayoutManager* layoutManager, MyGUI::VectorWidgetPtr* vectorWidgetPtr)
{
	layoutManager->unloadLayout(*vectorWidgetPtr);
}

extern "C" _AnomalousExport void VectorWidgetPtr_Delete(MyGUI::VectorWidgetPtr* vectorWidgetPtr)
{
	delete vectorWidgetPtr;
}