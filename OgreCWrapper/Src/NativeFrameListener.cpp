#include "StdAfx.h"
#include "../Include/NativeFrameListener.h"

NativeFrameListener::NativeFrameListener(FrameEventCallback frameStartedCallback, FrameEventCallback frameRenderingQueuedCallback, FrameEventCallback frameEndedCallback)
:frameStartedCallback(frameStartedCallback),
frameRenderingQueuedCallback(frameRenderingQueuedCallback),
frameEndedCallback(frameEndedCallback)
{
}

NativeFrameListener::~NativeFrameListener(void)
{
}

extern "C" _AnomalousExport NativeFrameListener* NativeFrameListener_Create(FrameEventCallback frameStartedCallback, FrameEventCallback frameRenderingQueuedCallback, FrameEventCallback frameEndedCallback)
{
	return new NativeFrameListener(frameStartedCallback, frameRenderingQueuedCallback, frameEndedCallback);
}

extern "C" _AnomalousExport void NativeFrameListener_Delete(NativeFrameListener* nativeFrameListener)
{
	delete nativeFrameListener;
}