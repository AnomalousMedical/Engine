#include "Stdafx.h"

extern "C" _AnomalousExport void CEGUISystem_create(CEGUI::Renderer* renderer, CEGUI::ResourceProvider* resourceProvider, CEGUI::XMLParser* xmlParser, CEGUI::ImageCodec* imageCodec, CEGUI::ScriptModule* scriptModule, String configFile, String logFile)
{
	CEGUI::System::create(*renderer, resourceProvider, xmlParser, imageCodec, scriptModule, configFile, logFile);
}

extern "C" _AnomalousExport void CEGUISystem_destroy()
{
	CEGUI::System::destroy();
}

extern "C" _AnomalousExport CEGUI::Window* CEGUISystem_setGUISheet(CEGUI::Window* window)
{
	return CEGUI::System::getSingleton().setGUISheet(window);
}