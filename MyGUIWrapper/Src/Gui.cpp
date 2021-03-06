#include "Stdafx.h"

extern "C" _AnomalousExport MyGUI::Gui* Gui_Create()
{
	return new MyGUI::Gui();
}

extern "C" _AnomalousExport void Gui_Delete(MyGUI::Gui* gui)
{
	delete gui;
}

extern "C" _AnomalousExport void Gui_initialize(MyGUI::Gui* gui, String coreConfig)
{
	gui->initialise(coreConfig);
}

extern "C" _AnomalousExport void Gui_shutdown(MyGUI::Gui* gui)
{
	gui->shutdown();
}

extern "C" _AnomalousExport MyGUI::Widget* Gui_createWidgetT(MyGUI::Gui* gui, String type, String skin, int left, int top, int width, int height, MyGUI::Align align, String layer, String name)
{
	return gui->createWidgetT(type, skin, left, top, width, height, align, layer, name);
}

extern "C" _AnomalousExport MyGUI::Widget* Gui_createWidgetRealT(MyGUI::Gui* gui, String type, String skin, int left, int top, int width, int height, MyGUI::Align align, String layer, String name)
{
	return gui->createWidgetRealT(type, skin, left, top, width, height, align, layer, name);
}

extern "C" _AnomalousExport void Gui_destroyWidget(MyGUI::Gui* gui, MyGUI::Widget* widget)
{
	gui->destroyWidget(widget);
}

extern "C" _AnomalousExport MyGUI::Widget* Gui_findWidgetT(MyGUI::Gui* gui, String name)
{
	return gui->findWidgetT(name, false);
}

extern "C" _AnomalousExport MyGUI::Widget* Gui_findWidgetT2(MyGUI::Gui* gui, String name, String prefix)
{
	return gui->findWidgetT(name, prefix, false);
}

extern "C" _AnomalousExport void Gui_setScaleFactor(MyGUI::Gui* gui, float scaleFactor)
{
	return gui->setScaleFactor(scaleFactor);
}

extern "C" _AnomalousExport float Gui_getScaleFactor(MyGUI::Gui* gui)
{
	return gui->getScaleFactor();
}

extern "C" _AnomalousExport void Gui_frameEvent(MyGUI::Gui* gui, float time)
{
	gui->frameEvent(time);
}