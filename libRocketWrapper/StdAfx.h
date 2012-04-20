#pragma once

#include <Rocket/Core.h>
#include <Rocket/Controls.h>
#include <Rocket/Debugger.h>

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