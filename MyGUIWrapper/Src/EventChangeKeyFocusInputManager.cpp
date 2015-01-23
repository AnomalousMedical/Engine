#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventChangeKeyFocusInputManager : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* widget);

private:
	MyGUI::InputManager* inputManager;
	NativeEventDelegate nativeEvent;

public:
	EventChangeKeyFocusInputManager(MyGUI::InputManager* inputManager, EventChangeKeyFocusInputManager::NativeEventDelegate nativeEventCallback)
		:inputManager(inputManager),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventChangeKeyFocusInputManager()
	{

	}

	void nativeCallbackFunc(MyGUI::Widget* widget)
	{
		nativeEvent(widget);
	}

	virtual void bindEvent()
	{
		inputManager->eventChangeKeyFocus += MyGUI::newDelegate(this, &EventChangeKeyFocusInputManager::nativeCallbackFunc);
	}

	virtual void unbindEvent()
	{
		inputManager->eventChangeKeyFocus -= MyGUI::newDelegate(this, &EventChangeKeyFocusInputManager::nativeCallbackFunc);
	}
};

extern "C" _AnomalousExport EventChangeKeyFocusInputManager* EventChangeKeyFocusInputManager_Create(MyGUI::InputManager* inputManager, EventChangeKeyFocusInputManager::NativeEventDelegate nativeEventCallback)
{
	return new EventChangeKeyFocusInputManager(inputManager, nativeEventCallback);
}