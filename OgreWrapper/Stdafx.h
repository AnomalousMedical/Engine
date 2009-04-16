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

#pragma warning(push)
#pragma warning(disable : 4635)
#include "Ogre.h"
#pragma warning(pop)