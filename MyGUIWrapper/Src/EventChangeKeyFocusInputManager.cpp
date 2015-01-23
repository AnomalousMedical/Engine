#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventChangeKeyFocusInputManager : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* widget HANDLE_ARG);

private:
	MyGUI::InputManager* inputManager;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

public:
	EventChangeKeyFocusInputManager(MyGUI::InputManager* inputManager, EventChangeKeyFocusInputManager::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:inputManager(inputManager),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventChangeKeyFocusInputManager()
	{

	}

	void nativeCallbackFunc(MyGUI::Widget* widget)
	{
		nativeEvent(widget PASS_HANDLE_ARG);
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

extern "C" _AnomalousExport EventChangeKeyFocusInputManager* EventChangeKeyFocusInputManager_Create(MyGUI::InputManager* inputManager, EventChangeKeyFocusInputManager::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventChangeKeyFocusInputManager(inputManager, nativeEventCallback PASS_HANDLE_ARG);
}