#include "Stdafx.h"

extern "C" _AnomalousExport size_t VectorWidgetPtr_getNumRootWidgets(MyGUI::VectorWidgetPtr* vectorWidgetPtr)
{
	return vectorWidgetPtr->size();
}

extern "C" _AnomalousExport MyGUI::Widget* VectorWidgetPtr_getRootWidget(MyGUI::VectorWidgetPtr* vectorWidgetPtr, size_t index)
{
	return (*vectorWidgetPtr)[index];
}