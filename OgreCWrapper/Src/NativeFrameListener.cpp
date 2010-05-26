#include "StdAfx.h"
#include "..\Include\NativeFrameListener.h"

NativeFrameListener::NativeFrameListener(FrameEventCallback frameStartedCallback, FrameEventCallback frameRenderingQueuedCallback, FrameEventCallback frameEndedCallback)
:frameStartedCallback(frameStartedCallback),
frameRenderingQueuedCallback(frameRenderingQueuedCallback),
frameEndedCallback(frameEndedCallback)
{
}

NativeFrameListener::~NativeFrameListener(void)
{
}

extern "C" __declspec(dllexport) NativeFrameListener* NativeFrameListener_Create(FrameEventCallback frameStartedCallback, FrameEventCallback frameRenderingQueuedCallback, FrameEventCallback frameEndedCallback)
{
	return new NativeFrameListener(frameStartedCallback, frameRenderingQueuedCallback, frameEndedCallback);
}

extern "C" __declspec(dllexport) void NativeFrameListener_Delete(NativeFrameListener* nativeFrameListener)
{
	delete nativeFrameListener;
}