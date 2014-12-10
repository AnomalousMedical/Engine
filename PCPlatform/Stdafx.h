// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once


#ifdef WINDOWS
#define _AnomalousExport __declspec(dllexport)
#define Int64 LONGLONG
#endif

#ifdef MAC_OSX
#define _AnomalousExport __attribute__ ((visibility("default")))
#define Int64 __int64_t
#endif

typedef unsigned int uint;
typedef unsigned char byte;
typedef unsigned short ushort;