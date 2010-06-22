// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once


#include "MyGUI.h"

#ifdef WINDOWS
#define _AnomalousExport __declspec(dllexport)
#endif

#ifdef MAC_OSX
#define _AnomalousExport __attribute__ ((visibility("default")))
#endif

typedef unsigned int uint;
typedef unsigned char byte;
typedef unsigned short ushort;

typedef const char* String;

typedef const wchar_t* UStringIn;

class Color
{
public:
	float r, g, b, a;

	Color(const MyGUI::Colour myColor)
		:r(myColor.red),
		g(myColor.green),
		b(myColor.blue),
		a(myColor.alpha)
	{

	}

	MyGUI::Colour toMyGUI() const
	{
		return MyGUI::Colour(r, g, b, a);
	}
};

class ThreeIntHack
{
public:
	int x;
    int y;
    int z;

	ThreeIntHack(const MyGUI::IntSize& size)
    {
        x = size.width;
        y = size.height;
        z = 0;
    }
};