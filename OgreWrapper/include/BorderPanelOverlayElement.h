#pragma once

#include "PanelOverlayElement.h"

namespace Ogre
{
	class BorderPanelOverlayElement;
}

namespace OgreWrapper{

/// <summary>
/// 
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class BorderPanelOverlayElement : public PanelOverlayElement
{
private:
	Ogre::BorderPanelOverlayElement* borderPanel;

internal:
	/// <summary>
	/// Returns the native BorderPanelOverlayElement
	/// </summary>
	Ogre::BorderPanelOverlayElement* getBorderPanelOverlayElement();

	/// <summary>
	/// Constructor
	/// </summary>
	BorderPanelOverlayElement(Ogre::BorderPanelOverlayElement* borderPanel);

public:

	static property System::String^ TypeName
	{
		System::String^ get()
		{
			return "BorderPanel";
		}
	}

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~BorderPanelOverlayElement();

	void setBorderSize(float size);

	void setBorderSize(float sides, float topAndBottom);

	void setBorderSize(float left, float right, float top, float bottom);

	float getLeftBorderSize();

	float getRightBorderSize();

	float getTopBorderSize();

	float getBottomBorderSize();

	void setLeftBorderUV(float u1, float v1, float u2, float v2);

	void setRightBorderUV(float u1, float v1, float u2, float v2);

	void setTopBorderUV(float u1, float v1, float u2, float v2);

	void setBottomBorderUV(float u1, float v1, float u2, float v2);

	void setTopLeftBorderUV(float u1, float v1, float u2, float v2);

	void setTopRightBorderUV(float u1, float v1, float u2, float v2);

	void setBottomLeftBorderUV(float u1, float v1, float u2, float v2);

	void setBottomRightBorderUV(float u1, float v1, float u2, float v2);

	void setBorderMaterialName(System::String^ name);

	System::String^ getBorderMaterialName();
};

}