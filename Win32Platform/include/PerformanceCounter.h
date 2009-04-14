#pragma once

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
	DWORD threadAffinity;

	LARGE_INTEGER lastTime;
	LARGE_INTEGER thisTime;
	LARGE_INTEGER performanceFrequency;

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
	/// Prime the counter's lastTime value. Should be called once before getDelta.
	/// </summary>
	void prime();

	/// <summary>
	/// Get the time since the last call to prime or the last call to
    /// getDelta().
	/// </summary>
	/// <returns>The time since the last call.</returns>
	double getDelta();
};

}

}