#include "Stdafx.h"
#include "OIS.h"

class IntVector3
{
public:
	int x, y, z;

	IntVector3(int x, int y, int z)
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

extern "C" _AnomalousExport void oisMouse_capture(OIS::Mouse* mouse, IntVector3* absPos, IntVector3* relPos)
{
	mouse->capture();

	const OIS::MouseState& state = mouse->getMouseState();
	absPos->x = state.X.abs;
	absPos->y = state.Y.abs;
	absPos->z = state.Z.abs;

	relPos->x = state.X.rel;
	relPos->y = state.Y.rel;
	relPos->z = state.Z.rel;
}