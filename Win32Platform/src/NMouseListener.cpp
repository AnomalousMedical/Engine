#include "StdAfx.h"
#include "..\include\NMouseListener.h"
#include "OISMouse.h"
#include "Mouse.h"

namespace Engine{

namespace Platform{

NMouseListener::NMouseListener(gcroot<OISMouse^> mouse)
:mouse(mouse)
{
}

NMouseListener::~NMouseListener()
{

}

bool NMouseListener::mouseMoved( const OIS::MouseEvent &arg )
{
	mouse->moved();
	return true;
}

bool NMouseListener::mousePressed( const OIS::MouseEvent &arg, OIS::MouseButtonID id )
{
	mouse->buttonPressed(id);
	return true;
}

bool NMouseListener::mouseReleased( const OIS::MouseEvent &arg, OIS::MouseButtonID id )
{
	mouse->buttonReleased(id);
	return true;
}

}

}