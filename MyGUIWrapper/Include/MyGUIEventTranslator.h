#pragma once

class MyGUIEventTranslator
{
public:
	MyGUIEventTranslator(void);

	virtual ~MyGUIEventTranslator(void);

	virtual void bindEvent() = 0;

	virtual void unbindEvent() = 0;
};
