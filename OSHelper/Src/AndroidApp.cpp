#include "Stdafx.h"
#include "AndroidApp.h"

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

#include <jni.h>
#include <errno.h>
#include <android_native_app_glue.h>

/**
* Shared state for our app.
*/
struct engine 
{
	struct android_app* app;
	int animating;
};

/**
* Process the next input event.
*/
static int32_t android_app_handle_input(struct android_app* app, AInputEvent* event) 
{
	struct engine* engine = (struct engine*)app->userData;
	if (AInputEvent_getType(event) == AINPUT_EVENT_TYPE_MOTION) 
	{
		//engine->animating = 1;
		return 1;
	}
	return 0;
}

/**
* Process the next main command.
*/
static void android_app_handle_cmd(struct android_app* app, int32_t cmd) {
	struct engine* engine = (struct engine*)app->userData;
	switch (cmd) 
	{
		case APP_CMD_SAVE_STATE:
			//Save state
			break;
		case APP_CMD_INIT_WINDOW:
			//Init
			break;
		case APP_CMD_TERM_WINDOW:
			//Window terminated
			break;
		case APP_CMD_GAINED_FOCUS:
			//App gained focus
			currentAndroidApp->fireMovedToForeground();
			engine->animating = 1;
			break;
		case APP_CMD_LOST_FOCUS:
			//App lost focus
			currentAndroidApp->fireMovedToBackground();
			//Also stop animating.
			engine->animating = 0;
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
	currentAndroidApp->fireInit();
	struct engine engine;

	// Make sure glue isn't stripped.
	app_dummy();

	memset(&engine, 0, sizeof(engine));
	state->userData = &engine;
	state->onAppCmd = android_app_handle_cmd;
	state->onInputEvent = android_app_handle_input;
	engine.app = state;

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
		while ((ident = ALooper_pollAll(engine.animating ? 0 : -1, NULL, &events, (void**)&source)) >= 0) 
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

		if (engine.animating) 
		{
			currentAndroidApp->fireIdle();
		}
	}
}