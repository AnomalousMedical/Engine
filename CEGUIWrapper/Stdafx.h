// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#include "CEGUI.h"

#ifdef WINDOWS
#define _AnomalousExport __declspec(dllexport)
#endif

#ifdef MAC_OSX
#define _AnomalousExport __attribute__ ((visibility("default")))
#endif

typedef const char* String;

typedef unsigned int uint;
typedef unsigned int UInt32;

class Rect
{
public:
	float top;
	float bottom;
	float left;
	float right;

	Rect(const CEGUI::Rect& rect)
	{
		top = rect.d_top;
		bottom = rect.d_bottom;
		left = rect.d_left;
		right = rect.d_right;
	}

	CEGUI::Rect toCEGUI()
	{
		return CEGUI::Rect(left, top, right, bottom);
	}
};

class Vector2
{
public:
	float x;
	float y;

	Vector2(const CEGUI::Vector2& vec)
	{
		x = vec.d_x;
		y = vec.d_y;
	}

	CEGUI::Vector2 toCEGUI()
	{
		return CEGUI::Vector2(x, y);
	}
};

class Size
{
public:
	float width;
	float height;

	Size(const CEGUI::Size& sz)
	{
		width = sz.d_width;
		height = sz.d_height;
	}

	CEGUI::Size toCEGUI()
	{
		return CEGUI::Size(width, height);
	}
};

class Vector3
{
public:
	float x, y, z;

	Vector3(const CEGUI::Vector3& vec)
		:x(vec.d_x),
		y(vec.d_y),
		z(vec.d_z)
	{

	}

	CEGUI::Vector3 toCEGUI() const
	{
		return CEGUI::Vector3(x, y, z);
	}
};

class FloatStructHack
{
public:
	float x;
	float y;
	float z;

	FloatStructHack(const CEGUI::Size& sz)
	{
		x = sz.d_width;
		y = sz.d_height;
	}

	FloatStructHack(const CEGUI::Vector2& vec)
	{
		x = vec.d_x;
		y = vec.d_y;
	}

	FloatStructHack(const CEGUI::UDim& udim)
	{
		x = udim.d_scale;
		y = udim.d_offset;
	}
};