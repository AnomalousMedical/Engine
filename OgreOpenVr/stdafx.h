// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#ifndef STDAFX_H
#define STDAFX_H

#if defined(WINDOWS)
#pragma warning(push)
#pragma warning(disable : 4635)
#endif
#include "Ogre.h"
#include "openvr.h"
//#include "ogre2d-main.h"
#if defined(WINDOWS)
#pragma warning(pop)
#endif

#if defined(WINDOWS) || defined(WINRT)
#define _AnomalousExport __declspec(dllexport)
#endif

#define VSCREEN_W 1280                  
#define VSCREEN_H 720

#endif