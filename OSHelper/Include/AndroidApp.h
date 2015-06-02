#pragma once

#include "App.h"

class AndroidWindow;

enum ActivationMode
{
	ACTIVATION_MODE_RESUMED = 1,
	ACTIVATION_MODE_SURFACE_CREATED = 2,
	ACTIVATION_MODE_WINDOW_FOCUSED = 4,
	GRAPHICS_RESOURCES_CREATED  = 8,
	SOUND_RESOURCES_CREATED = 16,

	TOTALLY_ACTIVATED = ACTIVATION_MODE_RESUMED | ACTIVATION_MODE_SURFACE_CREATED | ACTIVATION_MODE_WINDOW_FOCUSED | GRAPHICS_RESOURCES_CREATED | SOUND_RESOURCES_CREATED
};

/**
* Shared state for our app.
*/
struct AndroidAppState
{
	struct android_app* app;
	int animating;
	bool initOnce;
	AndroidWindow* androidWindow;
};

class AndroidApp : public App
{
public:
	AndroidApp();
    
	virtual ~AndroidApp();
    
    virtual void run();
    
    virtual void exit();
};