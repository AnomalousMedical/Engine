#include "StdAfx.h"
#include "../Include/NativeFrameListener.h"

NativeFrameListener::NativeFrameListener(NativeAction_Float_Float_NoHandle frameStartedCallback, NativeAction_Float_Float_NoHandle frameRenderingQueuedCallback, NativeAction_Float_Float_NoHandle frameEndedCallback)
:frameStartedCallback(frameStartedCallback),
frameRenderingQueuedCallback(frameRenderingQueuedCallback),
frameEndedCallback(frameEndedCallback)
{
}

NativeFrameListener::~NativeFrameListener(void)
{
}

extern "C" _AnomalousExport NativeFrameListener* NativeFrameListener_Create(NativeAction_Float_Float_NoHandle frameStartedCallback, NativeAction_Float_Float_NoHandle frameRenderingQueuedCallback, NativeAction_Float_Float_NoHandle frameEndedCallback)
{
	return new NativeFrameListener(frameStartedCallback, frameRenderingQueuedCallback, frameEndedCallback);
}

extern "C" _AnomalousExport void NativeFrameListener_Delete(NativeFrameListener* nativeFrameListener)
{
	delete nativeFrameListener;
}