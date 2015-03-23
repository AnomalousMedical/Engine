#pragma once

#include "App.h"

class AndroidWindow;

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