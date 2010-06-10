#pragma once

typedef bool (*BasicEventDelegate)();

class WindowEventTranslator
{
private:
	BasicEventDelegate basicEvent;
	CEGUI::String eventName;
	CEGUI::Event::Connection connection;

public:
	WindowEventTranslator(String eventName, BasicEventDelegate basicEvent);

	virtual ~WindowEventTranslator(void);

	void bindEvent(CEGUI::Window* window);

	bool eventCallback(const CEGUI::EventArgs& event);
};
