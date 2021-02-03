// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

#ifdef WINDOWS
#define _AnomalousExport __declspec(dllexport)

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
#include <windows.h>
#endif

#if defined(MAC_OSX) || defined(APPLE_IOS) || defined(ANDROID)
#define _AnomalousExport __attribute__ ((visibility("default")))
#endif

#include "StringManager.h"

#endif //PCH_H
