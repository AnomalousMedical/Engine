#include "Stdafx.h"

#include "MyGUI_OgrePlatform.h"

extern "C" _AnomalousExport MyGUI::OgrePlatform* OgrePlatform_Create()
{
	return new MyGUI::OgrePlatform();
}

extern "C" _AnomalousExport MyGUI::OgreRenderManager* OgrePlatform_getRenderManagerPtr(MyGUI::OgrePlatform* ogrePlatform)
{
	return ogrePlatform->getRenderManagerPtr();
}

extern "C" _AnomalousExport void OgrePlatform_Delete(MyGUI::OgrePlatform* ogrePlatform)
{
	delete ogrePlatform;
}

extern "C" _AnomalousExport void OgrePlatform_initialize(MyGUI::OgrePlatform* ogrePlatform, int windowWidth, int windowHeight, String resourceGroup, String logName)
{
	ogrePlatform->initialise(windowWidth, windowHeight, resourceGroup, logName);
}

extern "C" _AnomalousExport void OgrePlatform_shutdown(MyGUI::OgrePlatform* ogrePlatform)
{
	ogrePlatform->shutdown();
}