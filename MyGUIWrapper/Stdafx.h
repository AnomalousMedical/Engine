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

typedef const MyGUI::UString::code_point* UStringIn;
typedef const MyGUI::UString::code_point* UStringOut;

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

class Vector2
{
public:
	float x;
    float y;

	MyGUI::IntPoint toIntPoint()
	{
		return MyGUI::IntPoint(x, y);
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

MyGUI::MenuItemType::Enum getMenuItemTypeEnumVal(const MyGUI::MenuItemType& type);

MyGUI::Align::Enum getAlignEnumVal(const MyGUI::Align& align);