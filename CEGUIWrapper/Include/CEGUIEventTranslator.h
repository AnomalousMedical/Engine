#pragma once

class CEGUIEventTranslator
{
private:
	CEGUI::String eventName;
	CEGUI::Event::Connection connection;

public:
	CEGUIEventTranslator(String eventName);

	virtual ~CEGUIEventTranslator(void);

	void bindEvent(CEGUI::Window* window);

	void unbindEvent();

	bool eventCallback(const CEGUI::EventArgs& event);

	virtual bool handleEvent(const CEGUI::EventArgs& event) = 0;
};