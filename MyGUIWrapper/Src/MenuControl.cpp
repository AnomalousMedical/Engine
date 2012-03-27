#include "Stdafx.h"

extern "C" _AnomalousExport void MenuControl_setVisibleSmooth(MyGUI::MenuControl* widget, bool value)
{
	widget->setVisibleSmooth(value);
}

extern "C" _AnomalousExport size_t MenuControl_getItemCount(MyGUI::MenuControl* menuCtrl)
{
	return menuCtrl->getItemCount();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_insertItemAt(MyGUI::MenuControl* menuCtrl, size_t index, UStringIn name)
{
	return menuCtrl->insertItemAt(index, name);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_insertItemAt2(MyGUI::MenuControl* menuCtrl, size_t index, UStringIn name, MyGUI::MenuItemType::Enum type)
{
	return menuCtrl->insertItemAt(index, name, type);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_insertItemAt3(MyGUI::MenuControl* menuCtrl, size_t index, UStringIn name, MyGUI::MenuItemType::Enum type, String id)
{
	return menuCtrl->insertItemAt(index, name, type, id);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_insertItem(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* to, UStringIn name)
{
	return menuCtrl->insertItem(to, name);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_insertItem2(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* to, UStringIn name, MyGUI::MenuItemType::Enum type)
{
	return menuCtrl->insertItem(to, name, type);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_insertItem3(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* to, UStringIn name, MyGUI::MenuItemType::Enum type, String id)
{
	return menuCtrl->insertItem(to, name, type, id);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_addItem(MyGUI::MenuControl* menuCtrl, UStringIn name)
{
	return menuCtrl->addItem(name);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_addItem2(MyGUI::MenuControl* menuCtrl, UStringIn name, MyGUI::MenuItemType::Enum type)
{
	return menuCtrl->addItem(name, type);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_addItem3(MyGUI::MenuControl* menuCtrl, UStringIn name, MyGUI::MenuItemType::Enum type, String id)
{
	return menuCtrl->addItem(name, type, id);
}

extern "C" _AnomalousExport void MenuControl_removeItemAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	menuCtrl->removeItemAt(index);
}

extern "C" _AnomalousExport void MenuControl_removeItem(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	menuCtrl->removeItem(item);
}

extern "C" _AnomalousExport void MenuControl_removeAllItems(MyGUI::MenuControl* menuCtrl)
{
	menuCtrl->removeAllItems();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_getItemAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->getItemAt(index);
}

extern "C" _AnomalousExport size_t MenuControl_getItemIndex(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemIndex(item);
}

extern "C" _AnomalousExport size_t MenuControl_findItemIndex(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->findItemIndex(item);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_findItemWith(MyGUI::MenuControl* menuCtrl, UStringIn name)
{
	return menuCtrl->findItemWith(name);
}

extern "C" _AnomalousExport void MenuControl_setItemIdAt(MyGUI::MenuControl* menuCtrl, size_t index, String id)
{
	menuCtrl->setItemIdAt(index, id);
}

extern "C" _AnomalousExport void MenuControl_setItemId(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item, String id)
{
	menuCtrl->setItemId(item, id);
}

extern "C" _AnomalousExport String MenuControl_getItemIdAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->getItemIdAt(index).c_str();
}

extern "C" _AnomalousExport String MenuControl_getItemId(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemId(item).c_str();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_getItemById(MyGUI::MenuControl* menuCtrl, String id)
{
	return menuCtrl->getItemById(id);
}

extern "C" _AnomalousExport size_t MenuControl_getItemIndexById(MyGUI::MenuControl* menuCtrl, String id)
{
	return menuCtrl->getItemIndexById(id);
}

extern "C" _AnomalousExport void MenuControl_setItemNameAt(MyGUI::MenuControl* menuCtrl, size_t index, UStringIn name)
{
	menuCtrl->setItemNameAt(index, name);
}

extern "C" _AnomalousExport void MenuControl_setItemName(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item, UStringIn name)
{
	menuCtrl->setItemName(item, name);
}

extern "C" _AnomalousExport const MyGUI::UString::code_point* MenuControl_getItemNameAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->getItemNameAt(index).c_str();
}

extern "C" _AnomalousExport const MyGUI::UString::code_point* MenuControl_getItemName(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemName(item).c_str();
}

extern "C" _AnomalousExport size_t MenuControl_findItemIndexWith(MyGUI::MenuControl* menuCtrl, UStringIn name)
{
	return menuCtrl->findItemIndexWith(name);
}

extern "C" _AnomalousExport void MenuControl_setItemChildVisibleAt(MyGUI::MenuControl* menuCtrl, size_t index, bool visible)
{
	menuCtrl->setItemChildVisibleAt(index, visible);
}

extern "C" _AnomalousExport void MenuControl_setItemChildVisible(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item, bool visible)
{
	menuCtrl->setItemChildVisible(item, visible);
}

extern "C" _AnomalousExport MyGUI::MenuControl* MenuControl_getItemChildAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->getItemChildAt(index);
}

extern "C" _AnomalousExport MyGUI::MenuControl* MenuControl_getItemChild(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemChild(item);
}

extern "C" _AnomalousExport MyGUI::MenuControl* MenuControl_createItemChildAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->createItemChildAt(index);
}

extern "C" _AnomalousExport MyGUI::MenuControl* MenuControl_createItemChild(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->createItemChild(item);
}

extern "C" _AnomalousExport MyGUI::PopupMenu* MenuControl_createItemPopupMenuChildAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->createItemChildTAt<MyGUI::PopupMenu>(index);
}

extern "C" _AnomalousExport MyGUI::PopupMenu* MenuControl_createItemPopupMenuChild(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->createItemChildT<MyGUI::PopupMenu>(item);
}

extern "C" _AnomalousExport void MenuControl_removeItemChildAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	menuCtrl->removeItemChildAt(index);
}

extern "C" _AnomalousExport void MenuControl_removeItemChild(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	menuCtrl->removeItemChild(item);
}

extern "C" _AnomalousExport MyGUI::MenuItemType::Enum MenuControl_getItemTypeAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return getMenuItemTypeEnumVal(menuCtrl->getItemTypeAt(index));
}

extern "C" _AnomalousExport MyGUI::MenuItemType::Enum MenuControl_getItemType(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return getMenuItemTypeEnumVal(menuCtrl->getItemType(item));
}

extern "C" _AnomalousExport void MenuControl_setItemTypeAt(MyGUI::MenuControl* menuCtrl, size_t index, MyGUI::MenuItemType::Enum type)
{
	menuCtrl->setItemTypeAt(index, type);
}

extern "C" _AnomalousExport void MenuControl_setItemType(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item, MyGUI::MenuItemType::Enum type)
{
	menuCtrl->setItemType(item, type);
}

extern "C" _AnomalousExport void MenuControl_setPopupAccept(MyGUI::MenuControl* menuCtrl, bool value)
{
	menuCtrl->setPopupAccept(value);
}

extern "C" _AnomalousExport bool MenuControl_getPopupAccept(MyGUI::MenuControl* menuCtrl)
{
	return menuCtrl->getPopupAccept();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuControl_getMenuItemParent(MyGUI::MenuControl* menuCtrl)
{
	return menuCtrl->getMenuItemParent();
}