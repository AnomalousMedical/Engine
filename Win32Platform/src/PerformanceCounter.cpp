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
}

double PerformanceCounter::getDelta()
{
	QueryPerformanceCounter(&thisTime);
	double deltaTime = static_cast<double>(thisTime.QuadPart - lastTime.QuadPart) / static_cast<double>(performanceFrequency.QuadPart);
	lastTime = thisTime;
	//Prevent negative deltas
	if(deltaTime < 0.0)
	{
		deltaTime = 0.0;
	}
	return deltaTime;
}

#pragma managed

}

}