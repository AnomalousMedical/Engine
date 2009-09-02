#include "stdafx.h"
#include "PerformanceCounter.h"
#include "winbase.h"
#include "mmsystem.h"
#include <algorithm>

namespace Engine
{

namespace Platform
{

#pragma unmanaged

PerformanceCounter::PerformanceCounter()
:timerMask(0)
{

}

PerformanceCounter::~PerformanceCounter()
{

}

bool PerformanceCounter::initialize()
{
	DWORD procMask;
	DWORD sysMask;

	//Find the lowest used core
	GetProcessAffinityMask(GetCurrentProcess(), &procMask, &sysMask);
	if(procMask ==0)
	{
		procMask = 1;
	}
	if(timerMask == 0)
	{
		timerMask = 1;
		while( ( timerMask & procMask ) == 0 )
		{
			timerMask <<= 1;
		}
	}

	//Change affinity and read counter values
	HANDLE thread = GetCurrentThread();
	DWORD oldMask = SetThreadAffinityMask(thread, timerMask);
	bool valid = QueryPerformanceFrequency(&frequency);
	if(valid)
	{
		QueryPerformanceCounter(&startTime);
		startTick = GetTickCount();
	}
	SetThreadAffinityMask(thread, oldMask);

	//Finish and return
	lastTime = 0;
	return valid;
}

LONGLONG PerformanceCounter::getCurrentTime()
{
	//Set affinity and read current time
	LARGE_INTEGER currentTime;
	HANDLE thread = GetCurrentThread();
	DWORD oldMask = SetThreadAffinityMask(thread, timerMask);
	QueryPerformanceCounter(&currentTime);
	SetThreadAffinityMask(thread, oldMask);

	//Compute the number of ticks in milliseconds since initialize was called.
	LONGLONG time = currentTime.QuadPart - startTime.QuadPart;
	LONGLONG ticks = 1000 * time / frequency.QuadPart;

	//Check for performance counter leaps
	DWORD check = GetTickCount() - startTick;
	signed long off = (signed long)(ticks - check);
	if(off < -100 || off > 100)
	{
		//Adjust timer
		LONGLONG adjust = (std::min)(off * frequency.QuadPart / 1000, time - lastTime);
        startTime.QuadPart += adjust;
        time -= adjust;

        // Re-calculate milliseconds
        ticks = (unsigned long) (1000 * time / frequency.QuadPart);
	}

	lastTime = time;

	return ticks;
}

#pragma managed

}

}