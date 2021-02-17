// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#ifndef STDAFX_H
#define STDAFX_H



#ifdef WINDOWS
#define _AnomalousExport __declspec(dllexport)
#endif

#if defined(MAC_OSX) || defined(APPLE_IOS) || defined(ANDROID)
#define _AnomalousExport __attribute__ ((visibility("default")))
#endif

#endif