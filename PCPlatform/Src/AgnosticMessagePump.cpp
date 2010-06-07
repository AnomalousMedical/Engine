#include "Stdafx.h"
#ifdef WINDOWS
#define WIN32_LEAN_AND_MEAN
#include "windows.h"
#endif

#  include <Carbon/Carbon.h>

extern "C" _AnomalousExport void AgnosticMessagePump_primeMessages()
{
#ifdef WINDOWS
	MSG msg;
	PeekMessage( &msg, NULL, 0U, 0U, PM_NOREMOVE );
#endif
}

extern "C" _AnomalousExport bool AgnosticMessagePump_processMessage()
{
#ifdef WINDOWS
	MSG  msg;
	while( PeekMessage( &msg, NULL, 0U, 0U, PM_REMOVE ) )
	{
		TranslateMessage( &msg );
		DispatchMessage( &msg );
	}
#endif

	// OSX Message Pump
	EventRef event = NULL;
	EventTargetRef targetWindow;
	targetWindow = GetEventDispatcherTarget();
	
	// If we are unable to get the target then we no longer care about events.
	if( !targetWindow ) return false;
	
	// Grab the next event, process it if it is a window event
	if( ReceiveNextEvent( 0, NULL, kEventDurationNoWait, true, &event ) == noErr )
	{
		// Dispatch the event
		SendEventToEventTarget( event, targetWindow );
		ReleaseEvent( event );
	}

	return false;
}

extern "C" _AnomalousExport void AgnosticMessagePump_finishMessages()
{

}