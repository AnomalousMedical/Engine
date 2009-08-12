// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#pragma unmanaged
#include <zzip/zzip.h>
#pragma managed

#include <string>
#include <msclr/marshal.h>

std::string convertString( System::String^ str );
System::String^ convertString( std::string str );