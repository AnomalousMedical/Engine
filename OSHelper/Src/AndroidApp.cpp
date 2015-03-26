#include "Stdafx.h"
#include "AndroidApp.h"
#include "AndroidWindow.h"

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
	
}

static AndroidApp* currentAndroidApp;

//PInvoke
extern "C" _AnomalousExport AndroidApp* App_create()
{
	currentAndroidApp = new AndroidApp();
	return currentAndroidApp;
}

//android_app_handle_input variables
int deviceId; //We can use this to identify if we are a mouse or the touchscreen or something else
int eventPointerIndex; //The index into the event of the pointer
int pointerId; //The unique id of the pointer
float rawX; //The x value for the pointer's location
float rawY; //The y value for the pointer's location
int eventAction; //Event full info, contains action and pointer index
int action; //The action that was performed
int pointerCount; //The pointer count for this event

/**
* Process the next input event.
*/
static int32_t android_app_handle_input(struct android_app* app, AInputEvent* event) 
{
	struct AndroidAppState* appState = (struct AndroidAppState*)app->userData;
	if (AInputEvent_getType(event) == AINPUT_EVENT_TYPE_MOTION) 
	{
		//int deviceId = AInputEvent_getDeviceId(event); //We can use this to identify if we are a mouse or the touchscreen or something else

		eventAction = AMotionEvent_getAction(event);
		action = (int)(AMOTION_EVENT_ACTION_MASK & eventAction);

		switch (action)
		{
		case AMOTION_EVENT_ACTION_DOWN:
			pointerId = AMotionEvent_getPointerId(event, 0);
			rawX = AMotionEvent_getRawX(event, 0);
			rawY = AMotionEvent_getRawY(event, 0);
			LOGI("Motion event down id: %i x: %f y: %f", pointerId, rawX, rawY);
			break;

		case AMOTION_EVENT_ACTION_UP:
			pointerId = AMotionEvent_getPointerId(event, 0);
			rawX = AMotionEvent_getRawX(event, 0);
			rawY = AMotionEvent_getRawY(event, 0);
			LOGI("Motion event up id: %i x: %f y: %f", pointerId, rawX, rawY);
			break;

		case AMOTION_EVENT_ACTION_MOVE:
			pointerCount = AMotionEvent_getPointerCount(event);
			for (int i = 0; i < pointerCount; ++i)
			{
				pointerId = AMotionEvent_getPointerId(event, i);
				rawX = AMotionEvent_getRawX(event, i);
				rawY = AMotionEvent_getRawY(event, i);
				LOGI("Motion event move id: %i x: %f y: %f", pointerId, rawX, rawY);
			}
			break;

		case AMOTION_EVENT_ACTION_CANCEL:
			LOGI("Motion event cancel");
			break;

		case AMOTION_EVENT_ACTION_POINTER_DOWN:
			eventPointerIndex = (int)(AMOTION_EVENT_ACTION_POINTER_INDEX_MASK & eventAction) >> AMOTION_EVENT_ACTION_POINTER_INDEX_SHIFT;
			pointerId = AMotionEvent_getPointerId(event, eventPointerIndex);
			LOGI("Motion event pointer down id: %i x: %f y: %f", pointerId, rawX, rawY);
			break;

		case AMOTION_EVENT_ACTION_POINTER_UP:
			eventPointerIndex = (int)(AMOTION_EVENT_ACTION_POINTER_INDEX_MASK & eventAction) >> AMOTION_EVENT_ACTION_POINTER_INDEX_SHIFT;
			pointerId = AMotionEvent_getPointerId(event, eventPointerIndex);
			LOGI("Motion event pointer up id: %i x: %f y: %f", pointerId, rawX, rawY);
			break;

		case AMOTION_EVENT_ACTION_HOVER_MOVE:
			pointerId = AMotionEvent_getPointerId(event, 0);
			rawX = AMotionEvent_getRawX(event, 0);
			rawY = AMotionEvent_getRawY(event, 0);
			LOGI("Motion event hover move id: %i x: %f y: %f", pointerId, rawX, rawY);
			break;

		case AMOTION_EVENT_ACTION_SCROLL:
			pointerId = AMotionEvent_getPointerId(event, 0);
			LOGI("Motion event scroll id: %i x: %f y: %f", pointerId, rawX, rawY);
			break;
		}

		return 1;
	}
	return 0;
}

/**
* Process the next main command.
*/
static void android_app_handle_cmd(struct android_app* app, int32_t cmd) {
	struct AndroidAppState* appState = (struct AndroidAppState*)app->userData;
	switch (cmd) 
	{
		case APP_CMD_SAVE_STATE:
			//Save state
			break;
		case APP_CMD_INIT_WINDOW:
			//Init
			if (appState->initOnce)
			{
				appState->androidWindow->fireCreateInternalResources();
			}
			else
			{
				currentAndroidApp->fireInit();
				appState->initOnce = true;
			}
			break;
		case APP_CMD_TERM_WINDOW:
			//Window terminated
			appState->androidWindow->fireDestroyInternalResources();
			break;
		case APP_CMD_GAINED_FOCUS:
			//App gained focus
			currentAndroidApp->fireMovedToForeground();
			appState->animating = 1;
			break;
		case APP_CMD_LOST_FOCUS:
			//App lost focus
			currentAndroidApp->fireMovedToBackground();
			//Also stop animating.
			appState->animating = 0;
			break;
	}
}

/**
* This is the main entry point of a native application that is using
* android_native_app_glue.  It runs in its own thread, with its own
* event loop for receiving input events and doing other things.
*/
void android_main(struct android_app* state) 
{
	AndroidWindow_setApp(state);
	struct AndroidAppState appState;

	// Make sure glue isn't stripped.
	app_dummy();

	memset(&appState, 0, sizeof(AndroidAppState));
	state->userData = &appState;
	state->onAppCmd = android_app_handle_cmd;
	state->onInputEvent = android_app_handle_input;
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
		while ((ident = ALooper_pollAll(appState.animating ? 0 : -1, NULL, &events, (void**)&source)) >= 0) 
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

		if (appState.animating) 
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