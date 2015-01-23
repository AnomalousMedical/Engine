#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventChangeMouseFocusInputManager : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* widget);

private:
	MyGUI::InputManager* inputManager;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

public:
	EventChangeMouseFocusInputManager(MyGUI::InputManager* inputManager, EventChangeMouseFocusInputManager::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:inputManager(inputManager),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventChangeMouseFocusInputManager()
	{

	}

	void nativeCallbackFunc(MyGUI::Widget* widget)
	{
		nativeEvent(widget PASS_HANDLE);
	}

	virtual void bindEvent()
	{
		inputManager->eventChangeMouseFocus += MyGUI::newDelegate(this, &EventChangeMouseFocusInputManager::nativeCallbackFunc);
	}

	virtual void unbindEvent()
	{
		inputManager->eventChangeMouseFocus -= MyGUI::newDelegate(this, &EventChangeMouseFocusInputManager::nativeCallbackFunc);
	}
};

extern "C" _AnomalousExport EventChangeMouseFocusInputManager* EventChangeMouseFocusInputManager_Create(MyGUI::InputManager* inputManager, EventChangeMouseFocusInputManager::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventChangeMouseFocusInputManager(inputManager, nativeEventCallback PASS_HANDLE);
}