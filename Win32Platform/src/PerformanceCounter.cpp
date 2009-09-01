#include "stdafx.h"
#include "PerformanceCounter.h"
#include "winbase.h"
#include "mmsystem.h"

namespace Engine
{

namespace Platform
{

#pragma unmanaged

PerformanceCounter::PerformanceCounter()
:threadAffinity( 1 << 0 )
{

}

PerformanceCounter::~PerformanceCounter()
{

}

bool PerformanceCounter::initialize()
{
	return !QueryPerformanceFrequency(&performanceFrequency) || !SetThreadAffinityMask( GetCurrentThread(), threadAffinity );
}

void PerformanceCounter::prime()
{
	QueryPerformanceCounter(&lastTime);
	lastTickCount = GetTickCount();
}

double PerformanceCounter::getDelta()
{
	QueryPerformanceCounter(&thisTime);
	currentTickCount = GetTickCount();

	LONGLONG largeElapsed = thisTime.QuadPart - lastTime.QuadPart;

	tickElapsed = currentTickCount - lastTickCount;
	deltaTime = static_cast<double>(largeElapsed) / static_cast<double>(performanceFrequency.QuadPart);

	signed long check = tickElapsed - (signed long)(deltaTime * 1000);
	if(check < -100 | check > 100)
	{
		LONGLONG adjust = check * performanceFrequency.QuadPart / 1000;
		thisTime.QuadPart -= adjust;

		deltaTime = static_cast<double>(thisTime.QuadPart - lastTime.QuadPart) / static_cast<double>(performanceFrequency.QuadPart);
	}
	
	//Prevent negative deltas
	if(deltaTime < 0.0)
	{
		deltaTime = 0.0;
	}

	lastTime = thisTime;
	lastTickCount = currentTickCount;

	return deltaTime;
}

#pragma managed

}

}