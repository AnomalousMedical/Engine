#include "Stdafx.h"

#include <sstream>

#ifdef ANDROID
#include <jni.h>
#include <errno.h>
#include <android_native_app_glue.h>
#include <EGL/egl.h>
#include "Android/OgreAndroidEGLWindow.h"
#endif

extern "C" _AnomalousExport void RenderWindow_destroy(Ogre::RenderWindow* renderWindow)
{
	renderWindow->destroy();
}

extern "C" _AnomalousExport void RenderWindow_windowMovedOrResized(Ogre::RenderWindow* renderWindow)
{
	renderWindow->windowMovedOrResized();
}

extern "C" _AnomalousExport void RenderWindow_getWindowHandleStr(Ogre::RenderWindow* renderWindow, StringRetrieverCallback stringCb, void* handle)
{
	size_t winHandle = 0;
	renderWindow->getCustomAttribute("WINDOW", &winHandle);
	std::stringstream winHandleStr;
	winHandleStr << winHandle;
	stringCb(winHandleStr.str().c_str(), handle);
}

extern "C" _AnomalousExport void RenderWindow_setFullscreen(Ogre::RenderWindow* renderWindow, bool fullscreen, uint width, uint height)
{
	renderWindow->setFullscreen(fullscreen, width, height);
}

extern "C" _AnomalousExport bool RenderWindow_isDeactivatedOnFocusChange(Ogre::RenderWindow* renderWindow)
{
	return renderWindow->isDeactivatedOnFocusChange();
}

extern "C" _AnomalousExport void RenderWindow_setDeactivatedOnFocusChange(Ogre::RenderWindow* renderWindow, bool deactivate)
{
	renderWindow->setDeactivateOnFocusChange(deactivate);
}

extern "C" _AnomalousExport bool RenderWindow_isVisible(Ogre::RenderWindow* renderWindow)
{
	return renderWindow->isVisible();
}

extern "C" _AnomalousExport void RenderWindow_setVisible(Ogre::RenderWindow* renderWindow, bool visible)
{
	renderWindow->setVisible(visible);
}

extern "C" _AnomalousExport void RenderWindow_createInternalResources(Ogre::RenderWindow* renderWindow, void* osWindowHandle)
{
#ifdef ANDROID
	struct android_app* app = (struct android_app*)osWindowHandle;
	AConfiguration* config = AConfiguration_new();
	AConfiguration_fromAssetManager(config, app->activity->assetManager);
	static_cast<Ogre::AndroidEGLWindow*>(renderWindow)->_createInternalResources(app->window, config);
	if (config != 0)
	{
		AConfiguration_delete(config);
	}
#endif
}

extern "C" _AnomalousExport void RenderWindow_destroyInternalResources(Ogre::RenderWindow* renderWindow, void* osWindowHandle)
{
#ifdef ANDROID
	static_cast<Ogre::AndroidEGLWindow*>(renderWindow)->_destroyInternalResources();
#endif
}