#pragma once

#include "CEGUIEventTranslator.h"

typedef bool (*KeyEventDelegate)(CEGUI::Window* window, uint codepoint, CEGUI::Key::Scan scancode, uint sysKeys);

class KeyEventTranslator : public CEGUIEventTranslator
{
private:
	KeyEventDelegate eventDelegate;

public:
	KeyEventTranslator(String eventName, KeyEventDelegate eventDelegate);

	virtual ~KeyEventTranslator(void);

	virtual bool handleEvent(const CEGUI::EventArgs& event);
};
