#pragma once

#include "OverlayElement.h"

namespace Ogre
{
	class OverlayContainer;
}

namespace OgreWrapper{

/// <summary>
/// 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class OverlayContainer abstract : public OverlayElement
{
private:
	Ogre::OverlayContainer* overlayContainer;

internal:
	/// <summary>
	/// Returns the native OverlayContainer
	/// </summary>
	Ogre::OverlayContainer* getOverlayContainer();

	/// <summary>
	/// Constructor
	/// </summary>
	OverlayContainer(Ogre::OverlayContainer* overlayContainer);

public:

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~OverlayContainer();

	void addChild(OverlayElement^ elem);

	void removeChild(System::String^ name);

	OverlayElement^ getChild(System::String^ name);
};

}