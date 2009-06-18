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

	virtual void prime();

	virtual double getDelta();
};

}

}