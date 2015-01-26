#ifndef STDAFX_H
#define STDAFX_H

#include <Rocket/Core.h>
#include <Rocket/Controls.h>
#include <Rocket/Debugger.h>

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

#include "../Engine/Interop/NativeDelegates.h"

class Vector2i
{
public:
	int x;
    int y;

	Rocket::Core::Vector2i toVector2i()
	{
		return Rocket::Core::Vector2i(x, y);
	}
};

class Vector2f
{
public:
	float x;
    float y;

	Rocket::Core::Vector2f toVector2f()
	{
		return Rocket::Core::Vector2f(x, y);
	}
};

class ThreeIntHack
{
public:
	int x;
    int y;
    int z;

	ThreeIntHack(Rocket::Core::Vector2i vector2i)
    {
		x = vector2i.x;
        y = vector2i.y;
        z = 0;
    }
};

class Color
{
public:
	float r, g, b, a;

	Color(const Rocket::Core::Colourf myColor)
		:r(myColor.red),
		g(myColor.green),
		b(myColor.blue),
		a(myColor.alpha)
	{

	}

	Color(const Rocket::Core::Colourb myColor)
		:r(myColor.red / 255.0f),
		g(myColor.green / 255.0f),
		b(myColor.blue / 255.0f),
		a(myColor.alpha / 255.0f)
	{

	}

	Rocket::Core::Colourf toLibRocket() const
	{
		return Rocket::Core::Colourf(r, g, b, a);
	}
};

#endif