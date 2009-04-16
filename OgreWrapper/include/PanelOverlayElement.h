#pragma once

#include "OverlayContainer.h"

namespace Ogre
{
	class PanelOverlayElement;
}

namespace Rendering{

/// <summary>
/// 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PanelOverlayElement : public OverlayContainer
{
private:
	Ogre::PanelOverlayElement* panelOverlay;

internal:
	/// <summary>
	/// Returns the native PanelOverlayElement
	/// </summary>
	Ogre::PanelOverlayElement* getPanelOverlayElement();

	/// <summary>
	/// Constructor
	/// </summary>
	PanelOverlayElement(Ogre::PanelOverlayElement* panelOverlay);

public:

	static property System::String^ TypeName
	{
		System::String^ get()
		{
			return "Panel";
		}
	}

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PanelOverlayElement();

	void setTiling(float x, float y);

	void setTiling(float x, float y, unsigned short layer);

	float getTileX();

	float getTileX(unsigned short layer);

	float getTileY();

	float getTileY(unsigned short layer);

	void setUV(float u1, float v1, float u2, float v2);

	void getUV(float% u1, float% v1, float% u2, float% v2);

	void setTransparent(bool isTransparent);

	bool isTransparent();
};

}