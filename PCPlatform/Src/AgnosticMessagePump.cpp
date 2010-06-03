#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include "windows.h"

extern "C" __declspec(dllexport) void AgnosticMessagePump_primeMessages()
{
	MSG msg;
	PeekMessage( &msg, NULL, 0U, 0U, PM_NOREMOVE );
}

extern "C" __declspec(dllexport) bool AgnosticMessagePump_processMessage()
{
	MSG  msg;
	while( PeekMessage( &msg, NULL, 0U, 0U, PM_REMOVE ) )
	{
		TranslateMessage( &msg );
		DispatchMessage( &msg );
	}
	return false;
}

extern "C" __declspec(dllexport) void AgnosticMessagePump_finishMessages()
{

}