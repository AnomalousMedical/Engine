#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"
#include "MessageBox/MessageBox.h"

class EventMessageBoxResultTranslator : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Message* sender, MyGUI::MessageBoxStyle::Enum result HANDLE_ARG);

private:
	MyGUI::Message* widget;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

public:
	EventMessageBoxResultTranslator(MyGUI::Message* widget, EventMessageBoxResultTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
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
		nativeEvent(_sender, button PASS_HANDLE_ARG);
	}

};

extern "C" _AnomalousExport EventMessageBoxResultTranslator* EventMessageBoxResultTranslator_Create(MyGUI::Message* widget, EventMessageBoxResultTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventMessageBoxResultTranslator(widget, nativeEventCallback PASS_HANDLE_ARG);
}