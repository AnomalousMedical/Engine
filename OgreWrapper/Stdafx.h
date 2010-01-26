// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

/// <summary>
/// Handle for pointers
/// </summary>
typedef System::IntPtr PtrType;

// Shortcut out define.
#define _OUT [System::Runtime::InteropServices::Out]

#pragma unmanaged
#pragma warning(push)
#pragma warning(disable : 4635)
#include "Ogre.h"
#include "OgreCgPlugin.h"
#include "OgreD3D9Plugin.h"
#pragma warning(pop)
#pragma managed

//GCROOT anys
#define GCROOT_ANY_KEY "GCROOT"