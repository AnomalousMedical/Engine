// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#ifndef STDAFX_H
#define STDAFX_H


#include "MyGUI.h"

#ifdef WINDOWS
#define _AnomalousExport __declspec(dllexport)
#endif

#if defined(MAC_OSX) || defined(APPLE_IOS)
#define _AnomalousExport __attribute__ ((visibility("default")))
#endif

typedef unsigned int uint;
typedef unsigned char byte;
typedef unsigned short ushort;

typedef const char* String;

typedef const MyGUI::UString::code_point* UStringIn;
typedef const MyGUI::UString::code_point* UStringOut;

#include "../Engine/Interop/NativeDelegates.h"

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

class IntSize2
{
public:
	int width;
    int height;

	MyGUI::IntSize toIntSize()
	{
		return MyGUI::IntSize(width, height);
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

	ThreeIntHack(const MyGUI::IntPoint& point)
	{
		x = point.left;
        y = point.top;
        z = 0;
	}

	ThreeIntHack(int x, int y)
		:x(x),
		y(y),
		z(0)
	{
		
	}

	ThreeIntHack(int x, int y, int z)
		:x(x),
		y(y),
		z(z)
	{

	}
};

MyGUI::MenuItemType::Enum getMenuItemTypeEnumVal(const MyGUI::MenuItemType& type);

MyGUI::Align::Enum getAlignEnumVal(const MyGUI::Align& align);

MyGUI::FlowDirection::Enum getFlowDirectionEnumValue(const MyGUI::FlowDirection& flowDirection);

MyGUI::ResizingPolicy::Enum getResizingPolicyEnumValue(const MyGUI::ResizingPolicy& resizingPolicy);

#endif