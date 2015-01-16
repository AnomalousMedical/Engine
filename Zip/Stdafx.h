// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#include <zzip/zzip.h>
#include <zzip/plugin.h>

#ifdef WINDOWS
#define _AnomalousExport __declspec(dllexport)
#endif

#if defined(MAC_OSX) || defined(APPLE_IOS)
#define _AnomalousExport __attribute__ ((visibility("default")))
#endif