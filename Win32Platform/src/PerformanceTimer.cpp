#include "StdAfx.h"
#include "..\include\PerformanceTimer.h"
#include "PerformanceCounter.h"
#include "WindowsMessageHandler.h"

namespace Engine
{

namespace Platform
{

PerformanceTimer::PerformanceTimer(void)
:started( false ),  
totalTime( 0.0 ),
performanceCounter(new PerformanceCounter()),
messageHandler(nullptr)
{
}

PerformanceTimer::~PerformanceTimer()
{
	delete performanceCounter;
}

void PerformanceTimer::processMessageLoop(bool process)
{
	if(process && messageHandler == nullptr)
	{
		messageHandler = gcnew WindowsMessageHandler();
		this->addFullSpeedUpdateListener(messageHandler);
	}
	else if(messageHandler != nullptr)
	{
		this->removeFullSpeedUpdateListener(messageHandler);
		messageHandler = nullptr;
	}
}

bool PerformanceTimer::startLoop()
{
	if(performanceCounter->initialize())
	{
		return false;
	}

	started = true;
	fireLoopStarted();
	performanceCounter->prime();
	double deltaTime;
	totalTime = 0;
	int loops = 0;

	while( started )
	{	
		deltaTime = performanceCounter->getDelta();
		if( deltaTime < 0.0 )
		{
			deltaTime = 0.0;
		}
		else if( deltaTime > maxDelta )
		{
			deltaTime = maxDelta;
			fireExceededMaxDelta();
		}
		totalTime += deltaTime;

		loops = 0;
		while( totalTime > fixedFrequency && loops < maxFrameSkip )
		{
			fireFixedUpdate();
			totalTime -= fixedFrequency;
			loops++;
		}
		
		fireFullSpeedUpdate(deltaTime);
	}
	return true;
}

void PerformanceTimer::stopLoop()
{
	started = false;
}

double PerformanceTimer::FixedFrequency::get() 
{
	return fixedFrequency;
}

void PerformanceTimer::FixedFrequency::set(double value) 
{
	fixedFrequency = value;
}

double PerformanceTimer::MaxDelta::get() 
{
	return maxDelta;
}

void PerformanceTimer::MaxDelta::set(double value) 
{
	maxDelta = value;	
}

int PerformanceTimer::MaxFrameSkip::get() 
{
	return maxFrameSkip;
}

void PerformanceTimer::MaxFrameSkip::set(int value) 
{
	maxFrameSkip = value;
}

}

}