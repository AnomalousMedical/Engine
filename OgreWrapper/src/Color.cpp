#include "StdAfx.h"
#include "..\include\Color.h"

namespace Engine{

namespace Rendering{

Color::Color(float r, float g, float b)
:r(r), g(g), b(b), a(1.0f)
{

}

Color::Color(float r, float g, float b, float a)
:r(r), g(g), b(b), a(a)
{

}

}

}