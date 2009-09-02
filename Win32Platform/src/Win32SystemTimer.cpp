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

System::Int64 Win32SystemTimer::getCurrentTime()
{
	return performanceTimer->getCurrentTime();
}

}

}