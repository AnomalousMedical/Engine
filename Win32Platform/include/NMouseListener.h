#pragma once

#include "ois.h"
#include "vcclr.h"

namespace Engine{

namespace Platform{

ref class OISMouse;

/// <summary>
/// This is the native mouse listener that forwards mouse events from OIS to .NET.
/// </summary>
class NMouseListener : public OIS::MouseListener
{
private:
	gcroot<OISMouse^> mouse;

public:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="mouse">The mouse to forward events to.</param>
	NMouseListener(gcroot<OISMouse^> mouse);

	/// <summary>
	/// Destructor.
	/// </summary>
	virtual ~NMouseListener();

	/// <summary>
	/// Fired when the mouse is moved.
	/// </summary>
	/// <param name="arg">The MouseEvent.</param>
	virtual bool mouseMoved( const OIS::MouseEvent &arg );

	/// <summary>
	/// Fired when a button is pressed.
	/// </summary>
	/// <param name="arg">THe MouseEvent.</param>
	/// <param name="id">The id of the button that was pushed.</param>
	virtual bool mousePressed( const OIS::MouseEvent &arg, OIS::MouseButtonID id );

	/// <summary>
	/// 
	/// </summary>
	/// <param name="arg">THe MouseEvent.</param>
	/// <param name="id">The id of the button that was pushed.</param>
	virtual bool mouseReleased( const OIS::MouseEvent &arg, OIS::MouseButtonID id );
};

}

}