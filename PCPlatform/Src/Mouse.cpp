#include "Stdafx.h"
#include "OIS.h"

class Vector3
{
public:
	float x, y, z;

	Vector3(float x, float y, float z)
		:x(x),
		y(y),
		z(z)
	{

	}
};

extern "C" _AnomalousExport void oisMouse_setWindowSize(OIS::Mouse* mouse, int width, int height)
{
	const OIS::MouseState& state = mouse->getMouseState();
	state.width = width;
	state.height = height;
}

extern "C" _AnomalousExport bool oisMouse_buttonDown(OIS::Mouse* mouse, OIS::MouseButtonID button)
{
	return mouse->getMouseState().buttonDown(button);
}

extern "C" _AnomalousExport void oisMouse_capture(OIS::Mouse* mouse, Vector3* absPos, Vector3* relPos)
{
	mouse->capture();

	const OIS::MouseState& state = mouse->getMouseState();
	absPos->x = (float)state.X.abs;
	absPos->y = (float)state.Y.abs;
	absPos->z = (float)state.Z.abs;

	relPos->x = (float)state.X.rel;
	relPos->y = (float)state.Y.rel;
	relPos->z = (float)state.Z.rel;
}