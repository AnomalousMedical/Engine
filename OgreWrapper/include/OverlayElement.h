#pragma once

namespace Ogre
{
	class OverlayElement;
}

namespace Engine{

namespace Rendering{

ref class RenderMaterialPtr;
value class Color;
ref class OverlayContainer;

/// <summary>
/// Enum describing how the position / size of an element is to be recorded. 
/// </summary>
public enum class GuiMetricsMode : unsigned int
{
	/// <summary>
	/// 'left', 'top', 'height' and 'width' are parametrics from 0.0 to 1.0
	/// </summary>
	GMM_RELATIVE,
	/// <summary>
	/// Positions &amp; sizes are in absolute pixels
	/// </summary>
	GMM_PIXELS,
	/// <summary>
	/// Positions &amp; sizes are in virtual pixels
	/// </summary>
    GMM_RELATIVE_ASPECT_ADJUSTED
};

/// <summary>
/// Enum describing where '0' is in relation to the parent in the horizontal
/// dimension. Affects how 'left' is interpreted.
/// </summary>
public enum class GuiHorizontalAlignment : unsigned int
{
    GHA_LEFT,
    GHA_CENTER,
    GHA_RIGHT
};

/// <summary>
/// Enum describing where '0' is in relation to the parent in the vertical
/// dimension. Affects how 'top' is interpreted.
/// </summary>
public enum class GuiVerticalAlignment : unsigned int
{
    GVA_TOP,
    GVA_CENTER,
    GVA_BOTTOM
};

/// <summary>
/// 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class OverlayElement abstract
{
private:
	Ogre::OverlayElement* overlayElement;

internal:
	/// <summary>
	/// Returns the native OverlayElement
	/// </summary>
	Ogre::OverlayElement* getOverlayElement();

	/// <summary>
	/// Constructor
	/// </summary>
	OverlayElement(Ogre::OverlayElement* overlayElement);

public:

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~OverlayElement();

	/// <summary>
	/// 
	/// </summary>
	void initialize();

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	System::String^ getName();

	/// <summary>
	/// 
	/// </summary>
	void show();

	/// <summary>
	/// 
	/// </summary>
	void hide();

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	bool isVisible();

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	bool isEnabled();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="b"></param>
	void setEnabled(bool b);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="width"></param>
	/// <param name="height"></param>
	void setDimensions(float width, float height);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="left"></param>
	/// <param name="top"></param>
	void setPosition(float left, float top);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="width"></param>
	void setWidth(float width);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	float getWidth();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="height"></param>
	void setHeight(float height);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	float getHeight();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="left"></param>
	void setLeft(float left);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	float getLeft();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="top"></param>
	void setTop(float top);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	float getTop();

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	System::String^ getMaterialName();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="matName"></param>
	void setMaterialName(System::String^ matName);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	RenderMaterialPtr^ getMaterial();

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	System::String^ getTypeName();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="displayString"></param>
	void setCaption(System::String^ displayString);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	System::String^ getCaption();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="color"></param>
	void setColor(Color color);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="color"></param>
	void setColor(Color% color);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	Color getColor();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="mode"></param>
	void setMetricsMode(GuiMetricsMode mode);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	GuiMetricsMode getMetricsMode();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="gha"></param>
	void setHorizontalAlignment(GuiHorizontalAlignment gha);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	GuiHorizontalAlignment getHorizontalAlignment();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="gva"></param>
	void setVerticalAlignment(GuiVerticalAlignment gva);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	GuiVerticalAlignment getVerticalAlignment();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	bool contains(float x, float y);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	OverlayElement^ findElementAt(float x, float y);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	bool isContainer();

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	bool isKeyEnabled();

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	bool isCloneable();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="c"></param>
	void setCloneable(bool c);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	OverlayContainer^ getParent();

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	unsigned short getZOrder();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="templateOverlay"></param>
	void copyFromTemplate(OverlayElement^ templateOverlay);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="instanceName"></param>
	/// <returns></returns>
	OverlayElement^ clone(System::String^ instanceName);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	OverlayElement^ getSourceTemplate();
};

}

}
