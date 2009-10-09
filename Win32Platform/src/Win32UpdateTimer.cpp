#include "StdAfx.h"
#include "..\include\Win32UpdateTimer.h"

namespace Engine
{

namespace Platform
{

Win32UpdateTimer::Win32UpdateTimer(SystemTimer^ systemTimer)
:UpdateTimer(systemTimer),
hAccel(NULL),
hWnd(NULL),
systemMessageListener(nullptr)
{
}

Win32UpdateTimer::~Win32UpdateTimer(void)
{
}

bool Win32UpdateTimer::startLoop()
{
	if (!systemTimer->initialize())
    {
        return false;
    }

    started = true;
    fireLoopStarted();

	System::Int64 deltaTime;
    System::Int64 totalTime = 0;
    System::Int64 frameStartTime;
    System::Int64 lastTime = systemTimer->getCurrentTime();
    System::Int64 totalFrameTime;

	bool gotMessage;
	MSG msg;
	msg.message = WM_NULL;
	PeekMessage( &msg, NULL, 0U, 0U, PM_NOREMOVE );

    while (started)
    {
		gotMessage = ( PeekMessage( &msg, NULL, 0U, 0U, PM_REMOVE ) != 0 );
		if(gotMessage)
		{
			if( hAccel == NULL || hWnd == NULL || 
                0 == TranslateAccelerator( hWnd, hAccel, &msg ) )
            {
                TranslateMessage( &msg );
                DispatchMessage( &msg );
            }
		}
		else
		{
			frameStartTime = systemTimer->getCurrentTime();
			deltaTime = frameStartTime - lastTime;

			if (deltaTime > maxDelta)
			{
				deltaTime = maxDelta;
				fireExceededMaxDelta();
			}
			totalTime += deltaTime;
			if (totalTime > fixedFrequency * maxFrameSkip)
			{
				totalTime = fixedFrequency * maxFrameSkip;
			}

			//Frame skipping
			while (totalTime >= fixedFrequency)
			{
				fireFixedUpdate(fixedFrequency);
				totalTime -= fixedFrequency;
			}

			fireFullSpeedUpdate(deltaTime);

			lastTime = frameStartTime;

			//cap the framerate if required
			totalFrameTime = systemTimer->getCurrentTime() - frameStartTime;
			while (totalFrameTime < framerateCap)
			{
				System::Threading::Thread::Sleep((int)((framerateCap - totalFrameTime) / 1000));
				totalFrameTime = systemTimer->getCurrentTime() - frameStartTime;
			}
		}
    }
	if(hAccel != NULL)
	{
		DestroyAcceleratorTable(hAccel);
		hAccel = NULL;
	}
    return true;
}

void Win32UpdateTimer::setWindowHandle(OSWindow^ window)
{
	hWnd = (HWND)window->WindowHandle.ToInt32();
	hInstance = (HINSTANCE)GetWindowLongPtr(hWnd, GWLP_HINSTANCE);
}

void Win32UpdateTimer::setSystemMessageListener(UpdateListener^ systemMessageListener)
{
	this->systemMessageListener = systemMessageListener;
}

}

}