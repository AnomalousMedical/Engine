#include "StdAfx.h"
#include "..\include\Win32SystemTimer.h"
#include "PerformanceCounter.h"

namespace Engine
{

namespace Platform
{

Win32SystemTimer::Win32SystemTimer(void)
:performanceTimer(new PerformanceCounter())
{

}

Win32SystemTimer::~Win32SystemTimer(void)
{
	delete performanceTimer;
}

bool Win32SystemTimer::initialize()
{
	return performanceTimer->initialize();
}

void Win32SystemTimer::prime()
{
	performanceTimer->prime();
}

double Win32SystemTimer::getDelta()
{
	return performanceTimer->getDelta();
}

}

}