#include "StdAfx.h"
#include "..\include\Win32UpdateTimer.h"

namespace Engine
{

namespace Platform
{

Win32UpdateTimer::Win32UpdateTimer(SystemTimer^ systemTimer)
:UpdateTimer(systemTimer)
{
}

Win32UpdateTimer::~Win32UpdateTimer(void)
{
}

}

}