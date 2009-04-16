#pragma once

namespace OgreWrapper{

public value class Color
{
public:
	Color(float r, float g, float b);
		
	Color(float r, float g, float b, float a);

	float r, g, b, a;
};

}