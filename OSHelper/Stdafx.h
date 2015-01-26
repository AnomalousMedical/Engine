// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#ifndef STDAFX_H
#define STDAFX_H

#ifdef WINDOWS
#define WINVER 0x0500
#define WIN32_LEAN_AND_MEAN
#define NOMINMAX
#include <windows.h>
#endif

#ifdef WINRT
#include <collection.h>
#include <ppltasks.h>
#endif

#if defined(WINDOWS) || defined(WINRT)
#define _AnomalousExport __declspec(dllexport)
typedef const wchar_t* String; //UTF16 Needed, On windows this is ok
#endif

#if defined(MAC_OSX) || defined(APPLE_IOS)
#define _AnomalousExport __attribute__ ((visibility("default")))
typedef const unsigned short* String; //UTF16 Needed
#endif

#include "../Engine/Interop/NativeDelegates.h"

typedef unsigned int uint;
typedef unsigned char byte;
typedef unsigned short ushort;

#include "NativeLog.h"
extern NativeLog logger;

#endif