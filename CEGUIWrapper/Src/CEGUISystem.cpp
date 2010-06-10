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

extern "C" _AnomalousExport bool CEGUISystem_injectMouseMove(float delta_x, float delta_y)
{
	return CEGUI::System::getSingleton().injectMouseMove(delta_x, delta_y);
}

extern "C" _AnomalousExport bool CEGUISystem_injectMousePosition(float x_pos, float y_pos)
{
	return CEGUI::System::getSingleton().injectMousePosition(x_pos, y_pos);
}

extern "C" _AnomalousExport bool CEGUISystem_injectMouseLeaves()
{
	return CEGUI::System::getSingleton().injectMouseLeaves();
}

extern "C" _AnomalousExport bool CEGUISystem_injectMouseButtonDown(CEGUI::MouseButton button)
{
	return CEGUI::System::getSingleton().injectMouseButtonDown(button);
}

extern "C" _AnomalousExport bool CEGUISystem_injectMouseButtonUp(CEGUI::MouseButton button)
{
	return CEGUI::System::getSingleton().injectMouseButtonUp(button);
}

extern "C" _AnomalousExport bool CEGUISystem_injectKeyDown(uint key_code)
{
	return CEGUI::System::getSingleton().injectKeyDown(key_code);
}

extern "C" _AnomalousExport bool CEGUISystem_injectKeyUp(uint key_cod)
{
	return CEGUI::System::getSingleton().injectKeyUp(key_cod);
}

extern "C" _AnomalousExport bool CEGUISystem_injectChar(UInt32 code_point)
{
	return CEGUI::System::getSingleton().injectChar(code_point);
}

extern "C" _AnomalousExport bool CEGUISystem_injectMouseWheelChange(float delta)
{
	return CEGUI::System::getSingleton().injectMouseWheelChange(delta);
}

extern "C" _AnomalousExport bool CEGUISystem_injectTimePulse(float timeElapsed)
{
	return CEGUI::System::getSingleton().injectTimePulse(timeElapsed);
}

extern "C" _AnomalousExport void CEGUISystem_notifyDisplaySizeChanged(float width, float height)
{
	return CEGUI::System::getSingleton().notifyDisplaySizeChanged(CEGUI::Size(width, height));
}