#pragma once

#include "CEGUIEventTranslator.h"

typedef bool (*MouseEventDelegate)(CEGUI::Window* window, Vector2 position, Vector2 moveDelta, CEGUI::MouseButton button, uint sysKeys, float wheelChange, uint clickCount);

class MouseEventTranslator : public CEGUIEventTranslator
{
private:
	MouseEventDelegate eventDelegate;

public:
	MouseEventTranslator(String eventName, MouseEventDelegate eventDelegate);

	virtual ~MouseEventTranslator(void);

	virtual bool handleEvent(const CEGUI::EventArgs& event);
};
