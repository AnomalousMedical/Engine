#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"
#include "MessageBox/MessageBox.h"

class EventMessageBoxResultTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Message* sender, MyGUI::MessageBoxStyle::Enum result);

private:
	MyGUI::Message* widget;
	NativeEventDelegate nativeEvent;

public:
	EventMessageBoxResultTranslator(MyGUI::Message* widget, EventMessageBoxResultTranslator::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventMessageBoxResultTranslator()
	{

	}

	virtual void bindEvent()
	{
		widget->eventMessageBoxResult = MyGUI::newDelegate(this, &EventMessageBoxResultTranslator::callback);
	}

	virtual void unbindEvent()
	{
		widget->eventMessageBoxResult = NULL;
	}

private:
	void callback(MyGUI::Message* _sender, MyGUI::MessageBoxStyle _result)
	{
		MyGUI::MessageBoxStyle::Enum button = MyGUI::MessageBoxStyle::None;
		if(_result == MyGUI::MessageBoxStyle::Ok)
		{
			button = MyGUI::MessageBoxStyle::Ok;
		}
		else if(_result == MyGUI::MessageBoxStyle::Yes)
		{
			button = MyGUI::MessageBoxStyle::Yes;
		}
		else if(_result == MyGUI::MessageBoxStyle::No)
		{
			button = MyGUI::MessageBoxStyle::No;
		}
		else if(_result == MyGUI::MessageBoxStyle::Abort)
		{
			button = MyGUI::MessageBoxStyle::Abort;
		}
		else if(_result == MyGUI::MessageBoxStyle::Retry)
		{
			button = MyGUI::MessageBoxStyle::Retry;
		}
		else if(_result == MyGUI::MessageBoxStyle::Ignore)
		{
			button = MyGUI::MessageBoxStyle::Ignore;
		}
		else if(_result == MyGUI::MessageBoxStyle::Cancel)
		{
			button = MyGUI::MessageBoxStyle::Cancel;
		}
		else if(_result == MyGUI::MessageBoxStyle::Try)
		{
			button = MyGUI::MessageBoxStyle::Try;
		}
		else if(_result == MyGUI::MessageBoxStyle::Continue)
		{
			button = MyGUI::MessageBoxStyle::Continue;
		}
		nativeEvent(_sender, button);
	}

};

extern "C" _AnomalousExport EventMessageBoxResultTranslator* EventMessageBoxResultTranslator_Create(MyGUI::Message* widget, EventMessageBoxResultTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventMessageBoxResultTranslator(widget, nativeEventCallback);
}