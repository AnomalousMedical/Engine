// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#include "al.h" 
#include "alc.h" 

#define ENDIAN 0

#ifdef WINDOWS
#define _AnomalousExport __declspec(dllexport)
#endif

#ifdef MAC_OSX
#define _AnomalousExport __attribute__ ((visibility("default")))
#endif

#define NULL 0

#include "NativeLog.h"

namespace SoundWrapper
{

class Vector3
{
public:
	float x, y, z;

	Vector3()
		:x(0.0f), y(0.0f), z(0.0f)
	{

	}

	Vector3(float x, float y, float z)
		:x(x),
		y(y),
		z(z)
	{

	}
};

extern NativeLog logger;

void checkOpenAL(const char* hint = NULL);

}