#pragma once

typedef bool (*BasicEventDelegate)();

class CEGUIEventTranslator
{
private:
	BasicEventDelegate basicEvent;
	CEGUI::String eventName;
	CEGUI::Event::Connection connection;

public:
	CEGUIEventTranslator(String eventName, BasicEventDelegate basicEvent);

	virtual ~CEGUIEventTranslator(void);

	void bindEvent(CEGUI::Window* window);

	void unbindEvent();

	bool eventCallback(const CEGUI::EventArgs& event);
};