#pragma once

namespace Engine
{

namespace Platform
{

public ref class Win32UpdateTimer : UpdateTimer
{
private:
	SystemTimer^ systemTimer;

public:
	Win32UpdateTimer(SystemTimer^ systemTimer);
	virtual ~Win32UpdateTimer(void);
};

}

}