#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport String Window_getType(CEGUI::Window* window)
{
	return window->getType().c_str();
}

extern "C" _AnomalousExport String Window_getName(CEGUI::Window* window)
{
	return window->getName().c_str();
}

extern "C" _AnomalousExport bool Window_isDestroyedByParent(CEGUI::Window* window)
{
	return window->isDestroyedByParent();
}

extern "C" _AnomalousExport bool Window_isAlwaysOnTop(CEGUI::Window* window)
{
	return window->isAlwaysOnTop();
}

extern "C" _AnomalousExport bool Window_isDisabled(CEGUI::Window* window)
{
	return window->isDisabled();
}

extern "C" _AnomalousExport bool Window_isDisabled2(CEGUI::Window* window, bool localOnly)
{
	return window->isDisabled(localOnly);
}

extern "C" _AnomalousExport bool Window_isVisible(CEGUI::Window* window)
{
	return window->isVisible();
}

extern "C" _AnomalousExport bool Window_isVisible2(CEGUI::Window* window, bool localOnly)
{
	return window->isVisible(localOnly);
}

extern "C" _AnomalousExport bool Window_isActive(CEGUI::Window* window)
{
	return window->isActive();
}

extern "C" _AnomalousExport bool Window_isClippedByParent(CEGUI::Window* window)
{
	return window->isClippedByParent();
}

extern "C" _AnomalousExport uint Window_getID(CEGUI::Window* window)
{
	return window->getID();
}

extern "C" _AnomalousExport size_t Window_getChildCount(CEGUI::Window* window)
{
	return window->getChildCount();
}

extern "C" _AnomalousExport bool Window_isChildName(CEGUI::Window* window, String name)
{
	return window->isChild(name);
}

extern "C" _AnomalousExport bool Window_isChildID(CEGUI::Window* window, uint ID)
{
	return window->isChild(ID);
}

extern "C" _AnomalousExport bool Window_isChildRecursive(CEGUI::Window* window, uint ID)
{
	return window->isChildRecursive(ID);
}

extern "C" _AnomalousExport bool Window_isChildWin(CEGUI::Window* window, CEGUI::Window* child)
{
	return window->isChild(child);
}

extern "C" _AnomalousExport CEGUI::Window* Window_getChild(CEGUI::Window* window, String name)
{
	return window->getChild(name);
}

extern "C" _AnomalousExport CEGUI::Window* Window_getChildId(CEGUI::Window* window, uint id)
{
	return window->getChild(id);
}

extern "C" _AnomalousExport CEGUI::Window* Window_getChildRecursive(CEGUI::Window* window, String name)
{
	return window->getChildRecursive(name);
}

extern "C" _AnomalousExport CEGUI::Window* Window_getChildRecursiveId(CEGUI::Window* window, uint id)
{
	return window->getChildRecursive(id);
}

extern "C" _AnomalousExport CEGUI::Window* Window_getChildAtIdx(CEGUI::Window* window, uint index)
{
	return window->getChildAtIdx(index);
}

extern "C" _AnomalousExport CEGUI::Window* Window_getActiveChild(CEGUI::Window* window)
{
	return window->getActiveChild();
}

extern "C" _AnomalousExport bool Window_isAncestorName(CEGUI::Window* window, String name)
{
	return window->isAncestor(name);
}

extern "C" _AnomalousExport bool Window_isAncestorId(CEGUI::Window* window, uint id)
{
	return window->isAncestor(id);
}

extern "C" _AnomalousExport bool Window_isAncestorWin(CEGUI::Window* window, CEGUI::Window* ancestor)
{
	return window->isAncestor(ancestor);
}

extern "C" _AnomalousExport String Window_getText(CEGUI::Window* window)
{
	return window->getText().c_str();
}

extern "C" _AnomalousExport String Window_getTextVisual(CEGUI::Window* window)
{
	return window->getTextVisual().c_str();
}

extern "C" _AnomalousExport bool Window_inheritsAlpha(CEGUI::Window* window)
{
	return window->inheritsAlpha();
}

extern "C" _AnomalousExport float Window_getAlpha(CEGUI::Window* window)
{
	return window->getAlpha();
}

extern "C" _AnomalousExport float Window_getEffectiveAlpha(CEGUI::Window* window)
{
	return window->getEffectiveAlpha();
}

extern "C" _AnomalousExport Rect Window_getUnclippedOuterRect(CEGUI::Window* window)
{
	return window->getUnclippedOuterRect();
}

extern "C" _AnomalousExport Rect Window_getUnclippedInnerRect(CEGUI::Window* window)
{
	return window->getUnclippedInnerRect();
}

extern "C" _AnomalousExport Rect Window_getUnclippedRect(CEGUI::Window* window, bool inner)
{
	return window->getUnclippedRect(inner);
}

extern "C" _AnomalousExport Rect Window_getOuterRectClipper(CEGUI::Window* window)
{
	return window->getOuterRectClipper();
}

extern "C" _AnomalousExport Rect Window_getInnerRectClipper(CEGUI::Window* window)
{
	return window->getInnerRectClipper();
}

extern "C" _AnomalousExport Rect Window_getClipRect(CEGUI::Window* window)
{
	return window->getClipRect();
}

extern "C" _AnomalousExport Rect Window_getHitTestRect(CEGUI::Window* window)
{
	return window->getHitTestRect();
}

extern "C" _AnomalousExport bool Window_isCapturedByThis(CEGUI::Window* window)
{
	return window->isCapturedByThis();
}

extern "C" _AnomalousExport bool Window_isCapturedByAncestor(CEGUI::Window* window)
{
	return window->isCapturedByAncestor();
}

extern "C" _AnomalousExport bool Window_isCapturedByChild(CEGUI::Window* window)
{
	return window->isCapturedByChild();
}

extern "C" _AnomalousExport bool Window_isHit(CEGUI::Window* window, Vector2 position)
{
	return window->isHit(position.toCEGUI());
}

extern "C" _AnomalousExport bool Window_isHit2(CEGUI::Window* window, Vector2 position, bool allowDisabled)
{
	return window->isHit(position.toCEGUI(), allowDisabled);
}

extern "C" _AnomalousExport CEGUI::Window* Window_getChildAtPosition(CEGUI::Window* window, Vector2 position)
{
	return window->getChildAtPosition(position.toCEGUI());
}

extern "C" _AnomalousExport CEGUI::Window* Window_getTargetChildAtPosition(CEGUI::Window* window, Vector2 position)
{
	return window->getTargetChildAtPosition(position.toCEGUI());
}

extern "C" _AnomalousExport CEGUI::Window* Window_getTargetChildAtPosition2(CEGUI::Window* window, Vector2 position, bool allowDisabled)
{
	return window->getTargetChildAtPosition(position.toCEGUI(), allowDisabled);
}

extern "C" _AnomalousExport CEGUI::Window* Window_getParent(CEGUI::Window* window)
{
	return window->getParent();
}

extern "C" _AnomalousExport Size Window_getPixelSize(CEGUI::Window* window)
{
	return window->getPixelSize();
}

extern "C" _AnomalousExport bool Window_restoresOldCapture(CEGUI::Window* window)
{
	return window->restoresOldCapture();
}

extern "C" _AnomalousExport bool Window_isZOrderingEnabled(CEGUI::Window* window)
{
	return window->isZOrderingEnabled();
}

extern "C" _AnomalousExport bool Window_wantsMultiClickEvents(CEGUI::Window* window)
{
	return window->wantsMultiClickEvents();
}

extern "C" _AnomalousExport bool Window_isMouseAutoRepeatEnabled(CEGUI::Window* window)
{
	return window->isMouseAutoRepeatEnabled();
}

extern "C" _AnomalousExport float Window_getAutoRepeatDelay(CEGUI::Window* window)
{
	return window->getAutoRepeatDelay();
}

extern "C" _AnomalousExport float Window_getAutoRepeatRate(CEGUI::Window* window)
{
	return window->getAutoRepeatRate();
}

extern "C" _AnomalousExport bool Window_distributesCapturedInputs(CEGUI::Window* window)
{
	return window->distributesCapturedInputs();
}

extern "C" _AnomalousExport bool Window_isUsingDefaultTooltip(CEGUI::Window* window)
{
	return window->isUsingDefaultTooltip();
}

extern "C" _AnomalousExport CEGUI::Tooltip* Window_getTooltip(CEGUI::Window* window)
{
	return window->getTooltip();
}

extern "C" _AnomalousExport String Window_getTooltipType(CEGUI::Window* window)
{
	return window->getTooltipType().c_str();
}

extern "C" _AnomalousExport String Window_getTooltipText(CEGUI::Window* window)
{
	return window->getTooltipText().c_str();
}

extern "C" _AnomalousExport bool Window_inheritsTooltipText(CEGUI::Window* window)
{
	return window->inheritsTooltipText();
}

extern "C" _AnomalousExport bool Window_isRiseOnClickEnabled(CEGUI::Window* window)
{
	return window->isRiseOnClickEnabled();
}

extern "C" _AnomalousExport CEGUI::VerticalAlignment Window_getVerticalAlignment(CEGUI::Window* window)
{
	return window->getVerticalAlignment();
}

extern "C" _AnomalousExport CEGUI::HorizontalAlignment Window_getHorizontalAlignment(CEGUI::Window* window)
{
	return window->getHorizontalAlignment();
}

extern "C" _AnomalousExport String Window_getLookNFeel(CEGUI::Window* window)
{
	return window->getLookNFeel().c_str();
}

extern "C" _AnomalousExport bool Window_getModalState(CEGUI::Window* window)
{
	return window->getModalState();
}

extern "C" _AnomalousExport CEGUI::Window* Window_getActiveSibling(CEGUI::Window* window)
{
	return window->getActiveSibling();
}

extern "C" _AnomalousExport Size Window_getParentPixelSize(CEGUI::Window* window)
{
	return window->getParentPixelSize();
}

extern "C" _AnomalousExport float Window_getParentPixelWidth(CEGUI::Window* window)
{
	return window->getParentPixelWidth();
}

extern "C" _AnomalousExport float Window_getParentPixelHeight(CEGUI::Window* window)
{
	return window->getParentPixelHeight();
}

extern "C" _AnomalousExport bool Window_isMousePassThroughEnabled(CEGUI::Window* window)
{
	return window->isMousePassThroughEnabled();
}

extern "C" _AnomalousExport bool Window_isAutoWindow(CEGUI::Window* window)
{
	return window->isAutoWindow();
}

extern "C" _AnomalousExport bool Window_isDragDropTarget(CEGUI::Window* window)
{
	return window->isDragDropTarget();
}

extern "C" _AnomalousExport CEGUI::Window* Window_getRootWindow(CEGUI::Window* window)
{
	return window->getRootWindow();
}

extern "C" _AnomalousExport Vector3 Window_getRotation(CEGUI::Window* window)
{
	return window->getRotation();
}

extern "C" _AnomalousExport bool Window_isNonClientWindow(CEGUI::Window* window)
{
	return window->isNonClientWindow();
}

extern "C" _AnomalousExport void Window_rename(CEGUI::Window* window, String newName)
{
	window->rename(newName);
}

extern "C" _AnomalousExport void Window_initializeComponents(CEGUI::Window* window)
{
	window->initialiseComponents();
}

extern "C" _AnomalousExport void Window_setDestroyedByParent(CEGUI::Window* window, bool setting)
{
	window->setDestroyedByParent(setting);
}

extern "C" _AnomalousExport void Window_setAlwaysOnTop(CEGUI::Window* window, bool setting)
{
	window->setAlwaysOnTop(setting);
}

extern "C" _AnomalousExport void Window_setEnabled(CEGUI::Window* window, bool setting)
{
	window->setEnabled(setting);
}

extern "C" _AnomalousExport void Window_enable(CEGUI::Window* window)
{
	window->enable();
}

extern "C" _AnomalousExport void Window_disable(CEGUI::Window* window)
{
	window->disable();
}

extern "C" _AnomalousExport void Window_setVisible(CEGUI::Window* window, bool setting)
{
	window->setVisible(setting);
}

extern "C" _AnomalousExport void Window_show(CEGUI::Window* window)
{
	window->show();
}

extern "C" _AnomalousExport void Window_hide(CEGUI::Window* window)
{
	window->hide();
}

extern "C" _AnomalousExport void Window_activate(CEGUI::Window* window)
{
	window->activate();
}

extern "C" _AnomalousExport void Window_deactivate(CEGUI::Window* window)
{
	window->deactivate();
}

extern "C" _AnomalousExport void Window_setClippedByParent(CEGUI::Window* window, bool setting)
{
	window->setClippedByParent(setting);
}

extern "C" _AnomalousExport void Window_setID(CEGUI::Window* window, uint id)
{
	window->setID(id);
}

extern "C" _AnomalousExport void Window_setText(CEGUI::Window* window, String text)
{
	window->setText(text);
}

extern "C" _AnomalousExport void Window_insertText(CEGUI::Window* window, String text, uint position)
{
	window->insertText(text, position);
}

extern "C" _AnomalousExport void Window_appendText(CEGUI::Window* window, String text)
{
	window->appendText(text);
}

extern "C" _AnomalousExport void Window_setFont(CEGUI::Window* window, String name)
{
	window->setFont(name);
}

extern "C" _AnomalousExport void Window_addChildWindow(CEGUI::Window* window, String name)
{
	window->addChildWindow(name);
}

extern "C" _AnomalousExport void Window_addChildWindow2(CEGUI::Window* window, CEGUI::Window* child)
{
	window->addChildWindow(child);
}

extern "C" _AnomalousExport void Window_removeChildWindowName(CEGUI::Window* window, String name)
{
	window->removeChildWindow(name);
}

extern "C" _AnomalousExport void Window_removeChildWindowWin(CEGUI::Window* window, CEGUI::Window* child)
{
	window->removeChildWindow(child);
}

extern "C" _AnomalousExport void Window_removeChildWindowId(CEGUI::Window* window, uint id)
{
	window->removeChildWindow(id);
}

extern "C" _AnomalousExport void Window_moveToFront(CEGUI::Window* window)
{
	window->moveToFront();
}

extern "C" _AnomalousExport void Window_moveToBack(CEGUI::Window* window)
{
	window->moveToBack();
}

extern "C" _AnomalousExport bool Window_captureInput(CEGUI::Window* window)
{
	return window->captureInput();
}

extern "C" _AnomalousExport void Window_releaseInput(CEGUI::Window* window)
{
	window->releaseInput();
}

extern "C" _AnomalousExport void Window_setRestoreCapture(CEGUI::Window* window, bool setting)
{
	window->setRestoreCapture(setting);
}

extern "C" _AnomalousExport void Window_setAlpha(CEGUI::Window* window, float alpha)
{
	window->setAlpha(alpha);
}

extern "C" _AnomalousExport void Window_setInheritsAlpha(CEGUI::Window* window, bool setting)
{
	window->setInheritsAlpha(setting);
}

extern "C" _AnomalousExport void Window_invalidate(CEGUI::Window* window)
{
	window->invalidate();
}

extern "C" _AnomalousExport void Window_invalidate2(CEGUI::Window* window, bool recursive)
{
	window->invalidate(recursive);
}

extern "C" _AnomalousExport void Window_setMouseCursor(CEGUI::Window* window, String imageset, String imageName)
{
	window->setMouseCursor(imageset, imageName);
}

extern "C" _AnomalousExport void Window_setZOrderingEnabled(CEGUI::Window* window, bool setting)
{
	window->setZOrderingEnabled(setting);
}

extern "C" _AnomalousExport void Window_setWantsMultiClickEvents(CEGUI::Window* window, bool setting)
{
	window->setWantsMultiClickEvents(setting);
}

extern "C" _AnomalousExport void Window_setMouseAutoRepeatEnabled(CEGUI::Window* window, bool setting)
{
	window->setMouseAutoRepeatEnabled(setting);
}

extern "C" _AnomalousExport void Window_setAutoRepeatDelay(CEGUI::Window* window, float delay)
{
	window->setAutoRepeatDelay(delay);
}

extern "C" _AnomalousExport void Window_setAutoRepeatRate(CEGUI::Window* window, float rate)
{
	window->setAutoRepeatRate(rate);
}

extern "C" _AnomalousExport void Window_setDistributesCapturedInputs(CEGUI::Window* window, bool setting)
{
	window->setDistributesCapturedInputs(setting);
}

extern "C" _AnomalousExport void Window_setTooltip(CEGUI::Window* window, CEGUI::Tooltip* tooltip)
{
	window->setTooltip(tooltip);
}

extern "C" _AnomalousExport void Window_setTooltipType(CEGUI::Window* window, String tooltipType)
{
	window->setTooltipType(tooltipType);
}

extern "C" _AnomalousExport void Window_setTooltipText(CEGUI::Window* window, String text)
{
	window->setTooltipText(text);
}

extern "C" _AnomalousExport void Window_setInheritsTooltipText(CEGUI::Window* window, bool setting)
{
	window->setInheritsTooltipText(setting);
}

extern "C" _AnomalousExport void Window_setRiseOnClickEnabled(CEGUI::Window* window, bool setting)
{
	window->setRiseOnClickEnabled(setting);
}

extern "C" _AnomalousExport void Window_setVerticalAlignment(CEGUI::Window* window, CEGUI::VerticalAlignment alignment)
{
	window->setVerticalAlignment(alignment);
}

extern "C" _AnomalousExport void Window_setHorizontalAlignment(CEGUI::Window* window, CEGUI::HorizontalAlignment alignment)
{
	window->setHorizontalAlignment(alignment);
}

extern "C" _AnomalousExport void Window_setLookNFeel(CEGUI::Window* window, String look)
{
	window->setLookNFeel(look);
}

extern "C" _AnomalousExport void Window_setModalState(CEGUI::Window* window, bool state)
{
	window->setModalState(state);
}

extern "C" _AnomalousExport void Window_performChildWindowLayout(CEGUI::Window* window)
{
	window->performChildWindowLayout();
}

extern "C" _AnomalousExport void Window_setArea(CEGUI::Window* window, CEGUI::UDim xPos, CEGUI::UDim yPos, CEGUI::UDim width, CEGUI::UDim height)
{
	window->setArea(xPos, yPos, width, height);
}

extern "C" _AnomalousExport void Window_setArea2(CEGUI::Window* window, CEGUI::UVector2 pos, CEGUI::UVector2 size)
{
	window->setArea(pos, size);
}

extern "C" _AnomalousExport void Window_setArea3(CEGUI::Window* window, CEGUI::URect area)
{
	window->setArea(area);
}

extern "C" _AnomalousExport void Window_setPosition(CEGUI::Window* window, CEGUI::UVector2 pos)
{
	window->setPosition(pos);
}

extern "C" _AnomalousExport void Window_setXPosition(CEGUI::Window* window, CEGUI::UDim x)
{
	window->setXPosition(x);
}

extern "C" _AnomalousExport void Window_setYPosition(CEGUI::Window* window, CEGUI::UDim y)
{
	window->setYPosition(y);
}

extern "C" _AnomalousExport void Window_setSize(CEGUI::Window* window, CEGUI::UVector2 size)
{
	window->setSize(size);
}

extern "C" _AnomalousExport void Window_setWidth(CEGUI::Window* window, CEGUI::UDim width)
{
	window->setWidth(width);
}

extern "C" _AnomalousExport void Window_setHeight(CEGUI::Window* window, CEGUI::UDim height)
{
	window->setHeight(height);
}

extern "C" _AnomalousExport void Window_setMaxSize(CEGUI::Window* window, CEGUI::UVector2 size)
{
	window->setMaxSize(size);
}

extern "C" _AnomalousExport void Window_setMinSize(CEGUI::Window* window, CEGUI::UVector2 size)
{
	window->setMinSize(size);
}

extern "C" _AnomalousExport CEGUI::URect Window_getArea(CEGUI::Window* window)
{
	return window->getArea();
}

extern "C" _AnomalousExport CEGUI::UVector2 Window_getPosition(CEGUI::Window* window)
{
	return window->getPosition();
}

extern "C" _AnomalousExport CEGUI::UDim Window_getXPosition(CEGUI::Window* window)
{
	return window->getXPosition();
}

extern "C" _AnomalousExport CEGUI::UDim Window_getYPosition(CEGUI::Window* window)
{
	return window->getYPosition();
}

extern "C" _AnomalousExport CEGUI::UVector2 Window_getSize(CEGUI::Window* window)
{
	return window->getSize();
}

extern "C" _AnomalousExport CEGUI::UDim Window_getWidth(CEGUI::Window* window)
{
	return window->getWidth();
}

extern "C" _AnomalousExport CEGUI::UDim Window_getHeight(CEGUI::Window* window)
{
	return window->getHeight();
}

extern "C" _AnomalousExport CEGUI::UVector2 Window_getMaxSize(CEGUI::Window* window)
{
	return window->getMaxSize();
}

extern "C" _AnomalousExport CEGUI::UVector2 Window_getMinSize(CEGUI::Window* window)
{
	return window->getMinSize();
}

extern "C" _AnomalousExport void Window_setMousePassThroughEnabled(CEGUI::Window* window, bool setting)
{
	window->setMousePassThroughEnabled(setting);
}

extern "C" _AnomalousExport void Window_setFalagardType(CEGUI::Window* window, String type, String rendererType)
{
	window->setFalagardType(type, rendererType);
}

extern "C" _AnomalousExport void Window_setDragDropTarget(CEGUI::Window* window, bool setting)
{
	window->setDragDropTarget(setting);
}

extern "C" _AnomalousExport void Window_setRotation(CEGUI::Window* window, Vector3 rotation)
{
	window->setRotation(rotation.toCEGUI());
}

extern "C" _AnomalousExport void Window_setNonClientWindow(CEGUI::Window* window, bool setting)
{
	window->setNonClientWindow(setting);
}

extern "C" _AnomalousExport bool Window_isTextParsingEnabled(CEGUI::Window* window)
{
	return window->isTextParsingEnabled();
}

extern "C" _AnomalousExport void Window_setTextParsingEnabled(CEGUI::Window* window, bool setting)
{
	window->setTextParsingEnabled(setting);
}

extern "C" _AnomalousExport Vector2 Window_getUnprojectedPosition(CEGUI::Window* window, Vector2 pos)
{
	return window->getUnprojectedPosition(pos.toCEGUI());
}

#pragma warning(pop)