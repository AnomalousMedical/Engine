#pragma once

namespace Engine
{

namespace Platform
{

class PerformanceCounter;

ref class Win32SystemTimer : SystemTimer
{
private:
	PerformanceCounter* performanceTimer;

public:
	Win32SystemTimer();

	~Win32SystemTimer();

	virtual bool initialize();

	virtual System::Int64 getCurrentTime();
};

}

}