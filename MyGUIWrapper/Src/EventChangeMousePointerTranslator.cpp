#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EventChangeMousePointerTranslator : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(String pointerName);

private:
	MyGUI::PointerManager* pointerManager;
	NativeEventDelegate nativeEvent;

public:
	EventChangeMousePointerTranslator(MyGUI::PointerManager* pointerManager, EventChangeMousePointerTranslator::NativeEventDelegate nativeEventCallback)
		:pointerManager(pointerManager),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EventChangeMousePointerTranslator()
	{

	}

	void nativeCallbackFunc(const std::string& pointerName)
	{
		nativeEvent(pointerName.c_str());
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

extern "C" _AnomalousExport EventChangeMousePointerTranslator* EventChangeMousePointerTranslator_Create(MyGUI::PointerManager* pointerManager, EventChangeMousePointerTranslator::NativeEventDelegate nativeEventCallback)
{
	return new EventChangeMousePointerTranslator(pointerManager, nativeEventCallback);
}