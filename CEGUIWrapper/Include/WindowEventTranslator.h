#pragma once

#include "CEGUIEventTranslator.h"

typedef bool (*WindowEventDelegate)(CEGUI::Window* window);

class WindowEventTranslator : public CEGUIEventTranslator
{
private:
	WindowEventDelegate eventDelegate;

public:
	WindowEventTranslator(String eventName, WindowEventDelegate eventDelegate);

	virtual ~WindowEventTranslator(void);

	virtual bool handleEvent(const CEGUI::EventArgs& event);
};
