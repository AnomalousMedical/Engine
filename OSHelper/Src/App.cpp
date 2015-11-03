#include "StdAfx.h"
#include "App.h"

App::App()
{

}

App::~App()
{

}

void App::registerDelegates(OnInitDelegate onInitCB, OnExitDelegate onExitCB, NativeAction onIdleCB, NativeAction onMovedToBackgroundCB, NativeAction onMovedToForegroundCB HANDLE_ARG)
{
	this->onInitCB = onInitCB;
	this->onExitCB = onExitCB;
	this->onIdleCB = onIdleCB;
	this->onMovedToBackgroundCB = onMovedToBackgroundCB;
	this->onMovedToForegroundCB = onMovedToForegroundCB;
	ASSIGN_HANDLE
}

//PInvoke

//The OSX app instance has some custom deletion code in CocoaApp
#ifndef MAC_OSX
extern "C" _AnomalousExport void App_delete(App* app)
{
	delete app;
}
#endif

extern "C" _AnomalousExport void App_registerDelegates(App* app, OnInitDelegate onInitCB, OnExitDelegate onExitCB, NativeAction onIdleCB, NativeAction onMovedToBackgroundCB, NativeAction onMovedToForegroundCB HANDLE_ARG)
{
	app->registerDelegates(onInitCB, onExitCB, onIdleCB, onMovedToBackgroundCB, onMovedToForegroundCB PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void App_run(App* app)
{
	app->run();
}

extern "C" _AnomalousExport void App_exit(App* app)
{
	app->exit();
}