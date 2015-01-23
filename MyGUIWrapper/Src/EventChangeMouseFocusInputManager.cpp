#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventChangeMouseFocusInputManager : public MyGUIEventTranslator
{
public:
	typedef void(*NativeEventDelegate)(MyGUI::Widget* widget);

private:
	MyGUI::InputManager* inputManager;
	NativeEventDelegate nativeEvent;

public:
	EventChangeMouseFocusInputManager(MyGUI::InputManager* inputManager, EventChangeMouseFocusInputManager::NativeEventDelegate nativeEventCallback)
		:inputManager(inputManager),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventChangeMouseFocusInputManager()
	{

	}

	void nativeCallbackFunc(MyGUI::Widget* widget)
	{
		nativeEvent(widget);
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

extern "C" _AnomalousExport EventChangeMouseFocusInputManager* EventChangeMouseFocusInputManager_Create(MyGUI::InputManager* inputManager, EventChangeMouseFocusInputManager::NativeEventDelegate nativeEventCallback)
{
	return new EventChangeMouseFocusInputManager(inputManager, nativeEventCallback);
}