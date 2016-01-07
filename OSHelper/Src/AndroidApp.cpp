#include "Stdafx.h"
#include "AndroidApp.h"
#include "AndroidWindow.h"

static AndroidApp* currentAndroidApp;
static android_app* currentApp;

AndroidApp::AndroidApp()
{

}

AndroidApp::~AndroidApp()
{

}

void AndroidApp::run()
{
	//Doesn't do anything, handled by the activity starting
}

void AndroidApp::exit()
{
	ANativeActivity_finish(currentApp->activity);
}

//PInvoke
extern "C" _AnomalousExport AndroidApp* App_create()
{
	currentAndroidApp = new AndroidApp();
	return currentAndroidApp;
}

extern "C" _AnomalousExport void AndroidApp_outputCurrentABI()
{
#if ANDROID
#	if ARCH_ARM_V7A
		logger.sendMessage("Using arm v7a ABI", LogLevel::Info);
#	elif ARCH_X86
		logger.sendMessage("Using x86 ABI", LogLevel::Info);
#	else
		logger.sendMessage("Using Unknown (unable to detect at compile time) ABI", LogLevel::Info);
#	endif
#endif
}

/**
* Process the next input event.
*/
static int32_t android_app_handle_input(struct android_app* app, AInputEvent* event)
{
	struct AndroidAppState* appState = (struct AndroidAppState*)app->userData;
	return appState->androidWindow->handleInputEvent(app, event);
}

void checkHardwareResources(struct AndroidAppState* appState)
{
	//Check graphics
	bool graphicsResourcesCreated = (int)(appState->animating & GRAPHICS_RESOURCES_CREATED) == (int)GRAPHICS_RESOURCES_CREATED;
	bool surfaceCreated = (int)(appState->animating & ACTIVATION_MODE_SURFACE_CREATED) == (int)ACTIVATION_MODE_SURFACE_CREATED;
	if (graphicsResourcesCreated && !surfaceCreated)
	{
		appState->androidWindow->fireDestroyInternalResources(RT_LargeReloadableResources);
		appState->androidWindow->fireDestroyInternalResources(RT_Graphics);
		appState->animating &= ~(int)GRAPHICS_RESOURCES_CREATED;
	}
	else if(!graphicsResourcesCreated && surfaceCreated)
	{
		if (appState->initOnce)
		{
			appState->androidWindow->fireCreateInternalResources(RT_Graphics);
			appState->androidWindow->fireCreateInternalResources(RT_LargeReloadableResources);
		}
		appState->animating |= (int)GRAPHICS_RESOURCES_CREATED;
	}

	//Check audio
	bool audioResourcesCreated = (int)(appState->animating & SOUND_RESOURCES_CREATED) == (int)SOUND_RESOURCES_CREATED;
	bool safeToCreateAudio = (int)(appState->animating & ACTIVATION_MODE_RESUMED) == (int)ACTIVATION_MODE_RESUMED;

	if (audioResourcesCreated && !safeToCreateAudio)
	{
		appState->androidWindow->fireDestroyInternalResources(RT_Sound);
		appState->animating &= ~(int)SOUND_RESOURCES_CREATED;
	}
	else if (!audioResourcesCreated && safeToCreateAudio)
	{
		if (appState->initOnce)
		{
			appState->androidWindow->fireCreateInternalResources(RT_Sound);
		}
		appState->animating |= (int)SOUND_RESOURCES_CREATED;
	}
}

/**
* Process the next main command. Note that this runs on another thread from the main loop below.
*/
static void android_app_handle_cmd(struct android_app* app, int32_t cmd) {
	struct AndroidAppState* appState = (struct AndroidAppState*)app->userData;
	switch (cmd)
	{
	case APP_CMD_SAVE_STATE:
		//Save state
		break;

	//Window Surface
	case APP_CMD_INIT_WINDOW:
		//Init
		appState->animating |= ACTIVATION_MODE_SURFACE_CREATED;
		if (!appState->initOnce) //For some reson we have to do the intial activation this way, after this just use checkHardwareResourcess
		{
			currentAndroidApp->fireInit();
			appState->initOnce = true;
			appState->animating |= (int)GRAPHICS_RESOURCES_CREATED | (int)SOUND_RESOURCES_CREATED;
		}
		else
		{
			checkHardwareResources(appState);
		}
		break;

	case APP_CMD_TERM_WINDOW:
		appState->animating &= ~(int)ACTIVATION_MODE_SURFACE_CREATED;
		checkHardwareResources(appState);
		break;

	//Pause / Resume
	case APP_CMD_PAUSE:
		//Window terminated
		appState->animating &= ~(int)ACTIVATION_MODE_RESUMED;
		checkHardwareResources(appState);
		break;

	case APP_CMD_RESUME:
		appState->animating |= (int)ACTIVATION_MODE_RESUMED;
		checkHardwareResources(appState);

	//Focus
	case APP_CMD_GAINED_FOCUS:
		//App gained focus
		currentAndroidApp->fireMovedToForeground();
		appState->animating |= (int)ACTIVATION_MODE_WINDOW_FOCUSED;
		break;

	case APP_CMD_LOST_FOCUS:
		//App lost focus
		currentAndroidApp->fireMovedToBackground();
		//Also stop animating.
		appState->animating &= ~(int)ACTIVATION_MODE_WINDOW_FOCUSED;
		break;
	}
}



static void custom_process_input(struct android_app* app, struct android_poll_source* source)
{
	struct AndroidAppState* appState = (struct AndroidAppState*)app->userData;
	AInputEvent* event = NULL;
	while (AInputQueue_getEvent(app->inputQueue, &event) >= 0)
	{
		if (appState->androidWindow->getOnscreenKeyboardMode() != OnscreenKeyboardMode::Hidden)
		{
			int type = AInputEvent_getType(event);
			if (type == AINPUT_EVENT_TYPE_KEY && AKeyEvent_getAction(event) == AKEY_EVENT_ACTION_DOWN)
			{
				switch (AKeyEvent_getKeyCode(event))
				{
				case AKEYCODE_BACK:
					//Hide the onscreen keyboard, this is done automatically, but this way we can actually
					//track the status.
					appState->androidWindow->setOnscreenKeyboardMode(OnscreenKeyboardMode::Hidden);
					break;
				}
			}
		}
		
		if (AInputQueue_preDispatchEvent(app->inputQueue, event)) 
		{
			continue;
		}
		
		int32_t handled = 0;
		if (app->onInputEvent != NULL) handled = app->onInputEvent(app, event);
		AInputQueue_finishEvent(app->inputQueue, event, handled);
	}
}

/**
* This is the main entry point of a native application that is using
* android_native_app_glue.  It runs in its own thread, with its own
* event loop for receiving input events and doing other things.
*/
void android_main(struct android_app* state) 
{
	currentApp = state;

	AndroidWindow_setApp(state);
	struct AndroidAppState appState;

	// Make sure glue isn't stripped.
	app_dummy();

	memset(&appState, 0, sizeof(AndroidAppState));
	state->userData = &appState;
	state->onAppCmd = android_app_handle_cmd;
	state->onInputEvent = android_app_handle_input;
	state->inputPollSource.process = custom_process_input; //Use our own custom process input function to try to track the keyboard.
	appState.app = state;

	//It seems that we need to poll for app size changes, that sucks, these hold the current width, height
	int currentWidth = 0;
	int currentHeight = 0;
	int lastWidth = 0;
	int lastHeight = 0;

	// loop waiting for stuff to do.

	while (1) 
	{
		// Read all pending events.
		int ident;
		int events;
		struct android_poll_source* source;

		// If not animating, we will block forever waiting for events.
		// If animating, we loop until all events are read, then continue
		// to draw the next frame of animation.
		while ((ident = ALooper_pollAll(appState.animating == TOTALLY_ACTIVATED ? 0 : -1, NULL, &events, (void**)&source)) >= 0)
		{

			// Process this event.
			if (source != NULL) 
			{
				source->process(state, source);
			}			

			// Check if we are exiting.
			if (state->destroyRequested != 0) 
			{
				//Exiting
				currentAndroidApp->fireExit();
				return;
			}
		}

		//Make sure we are animating and have a window to draw in
		if (appState.animating && state->window != NULL)
		{
			currentWidth = ANativeWindow_getWidth(state->window);
			currentHeight = ANativeWindow_getHeight(state->window);
			if (currentWidth != lastWidth || currentHeight != lastHeight)
			{
				appState.androidWindow->fireSized();
				lastWidth = currentWidth;
				lastHeight = currentHeight;
			}

			currentAndroidApp->fireIdle();
		}
	}
}