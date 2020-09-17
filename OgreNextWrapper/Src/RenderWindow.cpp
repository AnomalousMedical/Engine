#include "Stdafx.h"
#include "OgreWindow.h"

#include <sstream>

#ifdef ANDROID
#include <jni.h>
#include <errno.h>
#include <android_native_app_glue.h>
#include <EGL/egl.h>
#include "Android/OgreAndroidEGLWindow.h"
#endif

extern "C" _AnomalousExport void RenderWindow_destroy(Ogre::Window* renderWindow)
{
	renderWindow->destroy();
}

extern "C" _AnomalousExport void RenderWindow_windowMovedOrResized(Ogre::Window* renderWindow)
{
	renderWindow->windowMovedOrResized();
}

extern "C" _AnomalousExport void RenderWindow_getWindowHandleStr(Ogre::Window* renderWindow, StringRetrieverCallback stringCb, void* handle)
{
	size_t winHandle = 0;
	renderWindow->getCustomAttribute("WINDOW", &winHandle);
	std::stringstream winHandleStr;
	winHandleStr << winHandle;
	stringCb(winHandleStr.str().c_str(), handle);
}

extern "C" _AnomalousExport void RenderWindow_requestFullscreenSwitch(Ogre::Window* renderWindow, 
	bool goFullscreen, bool borderless, uint32 monitorIdx,
	uint32 widthPt, uint32 heightPt,
	uint32 frequencyNumerator, uint32 frequencyDenominator)
{
	renderWindow->requestFullscreenSwitch(goFullscreen, borderless, monitorIdx,
		widthPt, heightPt,
		frequencyNumerator, frequencyDenominator);
}

extern "C" _AnomalousExport bool RenderWindow_isVisible(Ogre::Window* renderWindow)
{
	return renderWindow->isVisible();
}

extern "C" _AnomalousExport void RenderWindow_createInternalResources(Ogre::Window* renderWindow, void* osWindowHandle)
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

extern "C" _AnomalousExport void RenderWindow_destroyInternalResources(Ogre::Window* renderWindow, void* osWindowHandle)
{
#ifdef ANDROID
	static_cast<Ogre::AndroidEGLWindow*>(renderWindow)->_destroyInternalResources();
#endif
}

extern "C" _AnomalousExport Ogre::TextureGpu * RenderWindow_getTexture(Ogre::Window * renderWindow)
{
	return renderWindow->getTexture();
}