#include "Stdafx.h"

extern "C" _AnomalousExport void MenuCtrl_setVisibleSmooth(MyGUI::MenuControl* widget, bool value)
{
	widget->setVisibleSmooth(value);
}

extern "C" _AnomalousExport size_t MenuCtrl_getItemCount(MyGUI::MenuControl* menuCtrl)
{
	return menuCtrl->getItemCount();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItemAt(MyGUI::MenuControl* menuCtrl, size_t index, UStringIn name)
{
	return menuCtrl->insertItemAt(index, name);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItemAt2(MyGUI::MenuControl* menuCtrl, size_t index, UStringIn name, MyGUI::MenuItemType::Enum type)
{
	return menuCtrl->insertItemAt(index, name, type);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItemAt3(MyGUI::MenuControl* menuCtrl, size_t index, UStringIn name, MyGUI::MenuItemType::Enum type, String id)
{
	return menuCtrl->insertItemAt(index, name, type, id);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItem(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* to, UStringIn name)
{
	return menuCtrl->insertItem(to, name);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItem2(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* to, UStringIn name, MyGUI::MenuItemType::Enum type)
{
	return menuCtrl->insertItem(to, name, type);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_insertItem3(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* to, UStringIn name, MyGUI::MenuItemType::Enum type, String id)
{
	return menuCtrl->insertItem(to, name, type, id);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_addItem(MyGUI::MenuControl* menuCtrl, UStringIn name)
{
	return menuCtrl->addItem(name);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_addItem2(MyGUI::MenuControl* menuCtrl, UStringIn name, MyGUI::MenuItemType::Enum type)
{
	return menuCtrl->addItem(name, type);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_addItem3(MyGUI::MenuControl* menuCtrl, UStringIn name, MyGUI::MenuItemType::Enum type, String id)
{
	return menuCtrl->addItem(name, type, id);
}

extern "C" _AnomalousExport void MenuCtrl_removeItemAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	menuCtrl->removeItemAt(index);
}

extern "C" _AnomalousExport void MenuCtrl_removeItem(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	menuCtrl->removeItem(item);
}

extern "C" _AnomalousExport void MenuCtrl_removeAllItems(MyGUI::MenuControl* menuCtrl)
{
	menuCtrl->removeAllItems();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_getItemAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->getItemAt(index);
}

extern "C" _AnomalousExport size_t MenuCtrl_getItemIndex(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemIndex(item);
}

extern "C" _AnomalousExport size_t MenuCtrl_findItemIndex(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->findItemIndex(item);
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_findItemWith(MyGUI::MenuControl* menuCtrl, UStringIn name)
{
	return menuCtrl->findItemWith(name);
}

extern "C" _AnomalousExport void MenuCtrl_setItemIdAt(MyGUI::MenuControl* menuCtrl, size_t index, String id)
{
	menuCtrl->setItemIdAt(index, id);
}

extern "C" _AnomalousExport void MenuCtrl_setItemId(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item, String id)
{
	menuCtrl->setItemId(item, id);
}

extern "C" _AnomalousExport String MenuCtrl_getItemIdAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->getItemIdAt(index).c_str();
}

extern "C" _AnomalousExport String MenuCtrl_getItemId(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemId(item).c_str();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_getItemById(MyGUI::MenuControl* menuCtrl, String id)
{
	return menuCtrl->getItemById(id);
}

extern "C" _AnomalousExport size_t MenuCtrl_getItemIndexById(MyGUI::MenuControl* menuCtrl, String id)
{
	return menuCtrl->getItemIndexById(id);
}

extern "C" _AnomalousExport void MenuCtrl_setItemNameAt(MyGUI::MenuControl* menuCtrl, size_t index, UStringIn name)
{
	menuCtrl->setItemNameAt(index, name);
}

extern "C" _AnomalousExport void MenuCtrl_setItemName(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item, UStringIn name)
{
	menuCtrl->setItemName(item, name);
}

extern "C" _AnomalousExport const MyGUI::UString::code_point* MenuCtrl_getItemNameAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->getItemNameAt(index).c_str();
}

extern "C" _AnomalousExport const MyGUI::UString::code_point* MenuCtrl_getItemName(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemName(item).c_str();
}

extern "C" _AnomalousExport size_t MenuCtrl_findItemIndexWith(MyGUI::MenuControl* menuCtrl, UStringIn name)
{
	return menuCtrl->findItemIndexWith(name);
}

extern "C" _AnomalousExport void MenuCtrl_setItemChildVisibleAt(MyGUI::MenuControl* menuCtrl, size_t index, bool visible)
{
	menuCtrl->setItemChildVisibleAt(index, visible);
}

extern "C" _AnomalousExport void MenuCtrl_setItemChildVisible(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item, bool visible)
{
	menuCtrl->setItemChildVisible(item, visible);
}

extern "C" _AnomalousExport MyGUI::MenuControl* MenuCtrl_getItemChildAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->getItemChildAt(index);
}

extern "C" _AnomalousExport MyGUI::MenuControl* MenuCtrl_getItemChild(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->getItemChild(item);
}

extern "C" _AnomalousExport MyGUI::MenuControl* MenuCtrl_createItemChildAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->createItemChildAt(index);
}

extern "C" _AnomalousExport MyGUI::MenuControl* MenuCtrl_createItemChild(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->createItemChild(item);
}

extern "C" _AnomalousExport MyGUI::PopupMenu* MenuCtrl_createItemPopupMenuChildAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return menuCtrl->createItemChildTAt<MyGUI::PopupMenu>(index);
}

extern "C" _AnomalousExport MyGUI::PopupMenu* MenuCtrl_createItemPopupMenuChild(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return menuCtrl->createItemChildT<MyGUI::PopupMenu>(item);
}

extern "C" _AnomalousExport void MenuCtrl_removeItemChildAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	menuCtrl->removeItemChildAt(index);
}

extern "C" _AnomalousExport void MenuCtrl_removeItemChild(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	menuCtrl->removeItemChild(item);
}

extern "C" _AnomalousExport MyGUI::MenuItemType::Enum MenuCtrl_getItemTypeAt(MyGUI::MenuControl* menuCtrl, size_t index)
{
	return getMenuItemTypeEnumVal(menuCtrl->getItemTypeAt(index));
}

extern "C" _AnomalousExport MyGUI::MenuItemType::Enum MenuCtrl_getItemType(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item)
{
	return getMenuItemTypeEnumVal(menuCtrl->getItemType(item));
}

extern "C" _AnomalousExport void MenuCtrl_setItemTypeAt(MyGUI::MenuControl* menuCtrl, size_t index, MyGUI::MenuItemType::Enum type)
{
	menuCtrl->setItemTypeAt(index, type);
}

extern "C" _AnomalousExport void MenuCtrl_setItemType(MyGUI::MenuControl* menuCtrl, MyGUI::MenuItem* item, MyGUI::MenuItemType::Enum type)
{
	menuCtrl->setItemType(item, type);
}

extern "C" _AnomalousExport void MenuCtrl_setPopupAccept(MyGUI::MenuControl* menuCtrl, bool value)
{
	menuCtrl->setPopupAccept(value);
}

extern "C" _AnomalousExport bool MenuCtrl_getPopupAccept(MyGUI::MenuControl* menuCtrl)
{
	return menuCtrl->getPopupAccept();
}

extern "C" _AnomalousExport MyGUI::MenuItem* MenuCtrl_getMenuItemParent(MyGUI::MenuControl* menuCtrl)
{
	return menuCtrl->getMenuItemParent();
}