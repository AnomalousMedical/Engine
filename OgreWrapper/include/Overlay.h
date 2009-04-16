#pragma once

#include "AutoPtr.h"

namespace Ogre
{
	class Overlay;
	class SceneNode;
}

namespace OgreWrapper{

ref class SceneNode;
ref class OverlayContainer;

[Engine::Attributes::DoNotSaveAttribute]
public ref class Overlay
{
private:
	Ogre::Overlay* overlay;

internal:
	Overlay(Ogre::Overlay* overlay);

	Ogre::Overlay* getOverlay();

public:
	virtual ~Overlay(void);

	OverlayContainer^ getChild(System::String^ name);

	System::String^ getName();

	void setZOrder(unsigned short zOrder);

	unsigned short getZOrder();

	bool isVisible();

	bool isInitialized();

	void show();

	void hide();

	void add2d(OverlayContainer^ cont);

	void remove2d(OverlayContainer^ cont);

	void add3d(SceneNode^ node);

	void remove3d(SceneNode^ node);

	void clear();

	void setScroll(float x, float y);

	float getScrollX();

	float getScrollY();

	void scroll(float xOff, float yOff);

	void setRotate(float radAngle);

	float getRotate();

	void rotate(float radAngle);

	void setScale(float x, float y);

	float getScaleX();

	float getScaleY();
};

}