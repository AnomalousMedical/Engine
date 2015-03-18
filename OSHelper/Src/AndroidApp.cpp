/*
* Copyright (C) 2010 The Android Open Source Project
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

//BEGIN_INCLUDE(all)
#include <jni.h>
#include <errno.h>

#include <EGL/egl.h>
#include <GLES/gl.h>

#include <android/log.h>
#include <android_native_app_glue.h>

#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO, "native-activity", __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN, "native-activity", __VA_ARGS__))

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
static int32_t engine_handle_input(struct android_app* app, AInputEvent* event) 
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
static void engine_handle_cmd(struct android_app* app, int32_t cmd) {
	struct engine* engine = (struct engine*)app->userData;
	switch (cmd) 
	{
		case APP_CMD_SAVE_STATE:
			//Save state
			LOGI("Save State");
			break;
		case APP_CMD_INIT_WINDOW:
			//Init
			LOGI("Init window");
			break;
		case APP_CMD_TERM_WINDOW:
			//Window terminated
			LOGI("Terminate window");
			break;
		case APP_CMD_GAINED_FOCUS:
			//App gained focus
			LOGI("Gained focus, setting animating to 1");
			engine->animating = 1;
			break;
		case APP_CMD_LOST_FOCUS:
			//App lost focus
			LOGI("Lost focus, setting animating to 0");
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
	LOGI("Starting android_main");
	struct engine engine;

	// Make sure glue isn't stripped.
	app_dummy();

	memset(&engine, 0, sizeof(engine));
	state->userData = &engine;
	state->onAppCmd = engine_handle_cmd;
	state->onInputEvent = engine_handle_input;
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
				LOGI("Exiting");
				return;
			}
		}

		if (engine.animating) 
		{
			//animating
			LOGI("Animating");
		}
	}
}
//END_INCLUDE(all)
