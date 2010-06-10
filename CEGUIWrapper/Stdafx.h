// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#include "CEGUI.h"

#ifdef WINDOWS
#define _AnomalousExport __declspec(dllexport)
#endif

#ifdef MAC_OSX
#define _AnomalousExport __attribute__ ((visibility("default")))
#endif

typedef const char* String;

typedef unsigned int uint;
typedef unsigned int UInt32;