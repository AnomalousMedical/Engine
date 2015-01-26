#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventChangeMousePointerTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(String pointerName HANDLE_ARG);

private:
	MyGUI::PointerManager* pointerManager;
	NativeEventDelegate nativeEvent;
	HANDLE_INSTANCE

public:
	EventChangeMousePointerTranslator(MyGUI::PointerManager* pointerManager, EventChangeMousePointerTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:pointerManager(pointerManager),
		nativeEvent(nativeEventCallback)
		ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EventChangeMousePointerTranslator()
	{

	}

	void nativeCallbackFunc(const std::string& pointerName)
	{
		nativeEvent(pointerName.c_str() PASS_HANDLE_ARG);
	}

	virtual void bindEvent()
	{
		pointerManager->eventChangeMousePointer += MyGUI::newDelegate(this, &EventChangeMousePointerTranslator::nativeCallbackFunc);
	}

	virtual void unbindEvent()
	{
		pointerManager->eventChangeMousePointer -= MyGUI::newDelegate(this, &EventChangeMousePointerTranslator::nativeCallbackFunc);
	}
};

extern "C" _AnomalousExport EventChangeMousePointerTranslator* EventChangeMousePointerTranslator_Create(MyGUI::PointerManager* pointerManager, EventChangeMousePointerTranslator::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EventChangeMousePointerTranslator(pointerManager, nativeEventCallback PASS_HANDLE_ARG);
}