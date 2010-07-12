#include "Stdafx.h"

extern "C" _AnomalousExport MyGUI::InputManager* InputManager_getInstancePtr()
{
	return MyGUI::InputManager::getInstancePtr();
}

extern "C" _AnomalousExport bool InputManager_injectMouseMove(MyGUI::InputManager* inputManager, int absx, int absy, int absz)
{
	return inputManager->injectMouseMove(absx, absy, absz);
}

extern "C" _AnomalousExport bool InputManager_injectMousePress(MyGUI::InputManager* inputManager, int absx, int absy, MyGUI::MouseButton id)
{
	return inputManager->injectMousePress(absx, absy, id);
}

extern "C" _AnomalousExport bool InputManager_injectMouseRelease(MyGUI::InputManager* inputManager, int absx, int absy, MyGUI::MouseButton id)
{
	return inputManager->injectMouseRelease(absx, absy, id);
}

extern "C" _AnomalousExport bool InputManager_injectKeyPress(MyGUI::InputManager* inputManager, MyGUI::KeyCode key, uint text)
{
	return inputManager->injectKeyPress(key, text);
}

extern "C" _AnomalousExport bool InputManager_injectKeyRelease(MyGUI::InputManager* inputManager, MyGUI::KeyCode key)
{
	return inputManager->injectKeyRelease(key);
}

extern "C" _AnomalousExport bool InputManager_isFocusMouse(MyGUI::InputManager* inputManager)
{
	return inputManager->isFocusMouse();
}

extern "C" _AnomalousExport bool InputManager_isFocusKey(MyGUI::InputManager* inputManager)
{
	return inputManager->isFocusKey();
}

extern "C" _AnomalousExport bool InputManager_isCaptureMouse(MyGUI::InputManager* inputManager)
{
	return inputManager->isCaptureMouse();
}

extern "C" _AnomalousExport void InputManager_setKeyFocusWidget(MyGUI::InputManager* inputManager, MyGUI::Widget* widget)
{
	inputManager->setKeyFocusWidget(widget);
}

extern "C" _AnomalousExport void InputManager_resetKeyFocusWidget(MyGUI::InputManager* inputManager, MyGUI::Widget* widget)
{
	inputManager->resetKeyFocusWidget(widget);
}

extern "C" _AnomalousExport void InputManager_resetKeyFocusWidget2(MyGUI::InputManager* inputManager)
{
	inputManager->resetKeyFocusWidget();
}

extern "C" _AnomalousExport MyGUI::Widget* InputManager_getMouseFocusWidget(MyGUI::InputManager* inputManager)
{
	return inputManager->getMouseFocusWidget();
}

extern "C" _AnomalousExport MyGUI::Widget* InputManager_getKeyFocusWidget(MyGUI::InputManager* inputManager)
{
	return inputManager->getKeyFocusWidget();
}

extern "C" _AnomalousExport ThreeIntHack InputManager_getLastLeftPressed(MyGUI::InputManager* inputManager)
{
	return inputManager->getLastLeftPressed();
}

extern "C" _AnomalousExport ThreeIntHack InputManager_getMousePosition(MyGUI::InputManager* inputManager)
{
	return inputManager->getMousePosition();
}

extern "C" _AnomalousExport ThreeIntHack InputManager_getMousePositionByLayer(MyGUI::InputManager* inputManager)
{
	return inputManager->getMousePositionByLayer();
}

extern "C" _AnomalousExport void InputManager_resetMouseFocusWidget(MyGUI::InputManager* inputManager)
{
	inputManager->resetMouseFocusWidget();
}

extern "C" _AnomalousExport void InputManager_addWidgetModal(MyGUI::InputManager* inputManager, MyGUI::Widget* widget)
{
	inputManager->addWidgetModal(widget);
}

extern "C" _AnomalousExport void InputManager_removeWidgetModal(MyGUI::InputManager* inputManager, MyGUI::Widget* widget)
{
	inputManager->removeWidgetModal(widget);
}

extern "C" _AnomalousExport bool InputManager_isModalAny(MyGUI::InputManager* inputManager)
{
	return inputManager->isModalAny();
}

extern "C" _AnomalousExport bool InputManager_isControlPressed(MyGUI::InputManager* inputManager)
{
	return inputManager->isControlPressed();
}

extern "C" _AnomalousExport bool InputManager_isShiftPressed(MyGUI::InputManager* inputManager)
{
	return inputManager->isShiftPressed();
}

extern "C" _AnomalousExport void InputManager_resetMouseCaptureWidget(MyGUI::InputManager* inputManager)
{
	inputManager->resetMouseCaptureWidget();
}

extern "C" _AnomalousExport void InputManager_unlinkWidget(MyGUI::InputManager* inputManager, MyGUI::Widget* widget)
{
	inputManager->unlinkWidget(widget);
}