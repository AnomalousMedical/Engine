#pragma once

namespace Engine
{

namespace Platform
{

class PerformanceCounter;
ref class WindowsMessageHandler;

ref class PerformanceTimer : public Timer
{
private:
	PerformanceCounter* performanceCounter;
	WindowsMessageHandler^ messageHandler;

	double fixedFrequency;
	double maxDelta;
	int maxFrameSkip;

	double totalTime; //The total time for all frames that hasnt been processed

	bool started;

public:
	PerformanceTimer(void);

	~PerformanceTimer();

	property double FixedFrequency 
	{
		virtual double get() override;
		virtual void set(double value) override;
	}

	property double MaxDelta 
	{
		virtual double get() override;
		virtual void set(double value) override;
	}

	property int MaxFrameSkip 
	{
		virtual int get() override;
		virtual void set(int value) override;
	}

	virtual void processMessageLoop(bool process) override;

	virtual bool startLoop() override;

	virtual void stopLoop() override;
};

}

}