#include "Stdafx.h"

extern "C" _AnomalousExport MyGUI::Gui* Gui_Create()
{
	return new MyGUI::Gui();
}

extern "C" _AnomalousExport void Gui_Delete(MyGUI::Gui* gui)
{
	delete gui;
}

extern "C" _AnomalousExport void Gui_initialize(MyGUI::Gui* gui, String coreConfig, String logFile)
{
	gui->initialise(coreConfig, logFile);
}

extern "C" _AnomalousExport void Gui_shutdown(MyGUI::Gui* gui)
{
	gui->shutdown();
}