#pragma once

#define WIN32_LEAN_AND_MEAN
#define NOMINMAX
#include "windows.h"

namespace Engine
{

namespace Platform
{

/// <summary>
/// This class provides access to the QueryPerformanceCounter functions as a
/// timer that returns delta times between calls to getDelta. First call
/// initialize to get the frequency and see if the counter is valid. Then call
/// prime to set the start time followed by calls to getDelta to get the delta
/// between the call to prime, or between calls to getDelta. Prime would be
/// called outside of the loop.
/// </summary>
class PerformanceCounter
{
private:	
	DWORD startTick;
	LONGLONG lastTime;
	LARGE_INTEGER startTime;
	LARGE_INTEGER frequency;

	DWORD timerMask;

public:
	/// <summary>
	/// Constructor.
	/// </summary>
	PerformanceCounter();

	/// <summary>
	/// Destructor.
	/// </summary>
	~PerformanceCounter();

	/// <summary>
	/// Initialize the counter. Will return true if the counter can be used.
	/// </summary>
	/// <returns>True if the counter can be used.</returns>
	bool initialize();

	/// <summary>
	/// Get the current time in microseconds.
	/// </summary>
	/// <returns>The current time in microseconds.</returns>
	LONGLONG getCurrentTime();
};

}

}