#include "StdAfx.h"
#include "..\include\WindowsMessageHandler.h"
#include "windows.h"

namespace Engine
{

namespace Platform
{

WindowsMessageHandler::WindowsMessageHandler(void)
{
}

#pragma unmanaged

void processLoop()
{
	MSG  msg;
	while( PeekMessage( &msg, NULL, 0U, 0U, PM_REMOVE ) )
	{
		TranslateMessage( &msg );
		DispatchMessage( &msg );
	}
}

#pragma managed

void WindowsMessageHandler::sendUpdate( Clock^ clock )
{
	processLoop();
}

void WindowsMessageHandler::loopStarting()
{

}

void WindowsMessageHandler::exceededMaxDelta()
{

}

}

}