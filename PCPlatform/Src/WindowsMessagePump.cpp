#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include "windows.h"

struct WinMsg
{
public:
	HWND hwnd;
	int message;
	WPARAM wParam;
	LPARAM lParam;
	int time;
	int pt_x;
	int pt_y;
};

extern "C" __declspec(dllexport) MSG* WindowsMessagePump_primeMessages()
{
	MSG* msg = new MSG();
	msg->message = WM_NULL;
	PeekMessage( msg, NULL, 0U, 0U, PM_NOREMOVE );
	return msg;
}

extern "C" __declspec(dllexport) bool WindowsMessagePump_peekMessage(MSG* msgHandle, WinMsg* msg)
{
	if(PeekMessage(msgHandle, NULL, 0, 0, PM_REMOVE) != 0)
	{
		msg->hwnd = msgHandle->hwnd;
		msg->message = msgHandle->message;
		msg->wParam = msgHandle->wParam;
		msg->lParam = msgHandle->lParam;
		msg->time = msgHandle->time;
		msg->pt_x = msgHandle->pt.x;
		msg->pt_y = msgHandle->pt.y;
		return true;
	}
	return false;
}

extern "C" __declspec(dllexport) void WindowsMessagePump_translateAndDispatchMessage(MSG* msgHandle)
{
	/*if( hAccel == NULL || hWnd == NULL || 
		0 == TranslateAccelerator( hWnd, hAccel, &msg ) )
	{*/
		TranslateMessage( msgHandle );
		DispatchMessage( msgHandle );
	//}
}

extern "C" __declspec(dllexport) void WindowsMessagePump_finishMessages(MSG* msgHandle)
{
	/*if(hAccel != NULL)
	{
		DestroyAcceleratorTable(hAccel);
		hAccel = NULL;
	}*/
	delete msgHandle;
}