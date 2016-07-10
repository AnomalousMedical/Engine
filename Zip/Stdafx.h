// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#if !defined(MAC_OSX) && !defined(APPLE_IOS)
#pragma once
#endif

#include <zzip/zzip.h>
#include <zzip/plugin.h>

#ifdef WINDOWS
#define _AnomalousExport __declspec(dllexport)
#endif

#if defined(MAC_OSX) || defined(APPLE_IOS) || defined(ANDROID)
#define _AnomalousExport __attribute__ ((visibility("default")))
#endif