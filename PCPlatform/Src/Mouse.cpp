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

typedef void(*ButtonCallback) (OIS::MouseButtonID id);
typedef void(*MovedCallback) (int x, int y, int z);

class BufferedMouseListener : public OIS::MouseListener
{
private:
	ButtonCallback buttonPressedCb;
	ButtonCallback buttonReleasedCb;
	MovedCallback movedCallback;

public:
	BufferedMouseListener(ButtonCallback buttonPressedCb, ButtonCallback buttonReleasedCb, MovedCallback movedCallback)
		:buttonPressedCb(buttonPressedCb),
		buttonReleasedCb(buttonReleasedCb),
		movedCallback(movedCallback)
	{

	}

	virtual ~BufferedMouseListener()
	{

	}

	virtual bool mouseMoved(const OIS::MouseEvent &arg)
	{
		movedCallback(arg.state.X.abs, arg.state.Y.abs, arg.state.Z.rel);
		return true;
	}

	virtual bool mousePressed(const OIS::MouseEvent &arg, OIS::MouseButtonID id)
	{
		buttonPressedCb(id);
		return true;
	}

	virtual bool mouseReleased(const OIS::MouseEvent &arg, OIS::MouseButtonID id)
	{
		buttonReleasedCb(id);
		return true;
	}
};

extern "C" _AnomalousExport BufferedMouseListener* oisMouse_createListener(OIS::Mouse* mouse, ButtonCallback buttonPressedCb, ButtonCallback buttonReleasedCb, MovedCallback movedCallback)
{
	BufferedMouseListener* listener = new BufferedMouseListener(buttonPressedCb, buttonReleasedCb, movedCallback);
	mouse->setEventCallback(listener);
	return listener;
}

extern "C" _AnomalousExport void oisMouse_destroyListener(OIS::Mouse* mouse, BufferedMouseListener* listener)
{
	mouse->setEventCallback(0);
	delete listener;
}