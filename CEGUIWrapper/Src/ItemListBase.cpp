#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport uint ItemListBase_getItemCount(CEGUI::ItemListBase* itemListBase)
{
	return itemListBase->getItemCount();
}

extern "C" _AnomalousExport CEGUI::ItemEntry* ItemListBase_getItemFromIndex(CEGUI::ItemListBase* itemListBase, uint index)
{
	return itemListBase->getItemFromIndex(index);
}

extern "C" _AnomalousExport uint ItemListBase_getItemIndex(CEGUI::ItemListBase* itemListBase, CEGUI::ItemEntry* item)
{
	return itemListBase->getItemIndex(item);
}

extern "C" _AnomalousExport CEGUI::ItemEntry* ItemListBase_findItemWithText(CEGUI::ItemListBase* itemListBase, String text, CEGUI::ItemEntry* start_item)
{
	return itemListBase->findItemWithText(text, start_item);
}

extern "C" _AnomalousExport bool ItemListBase_isItemInList(CEGUI::ItemListBase* itemListBase, CEGUI::ItemEntry* item)
{
	return itemListBase->isItemInList(item);
}

extern "C" _AnomalousExport bool ItemListBase_isAutoResizeEnabled(CEGUI::ItemListBase* itemListBase)
{
	return itemListBase->isAutoResizeEnabled();
}

extern "C" _AnomalousExport bool ItemListBase_isSortEnabled(CEGUI::ItemListBase* itemListBase)
{
	return itemListBase->isSortEnabled();
}

extern "C" _AnomalousExport CEGUI::ItemListBase::SortMode ItemListBase_getSortMode(CEGUI::ItemListBase* itemListBase)
{
	return itemListBase->getSortMode();
}

extern "C" _AnomalousExport void ItemListBase_initialiseComponents(CEGUI::ItemListBase* itemListBase)
{
	itemListBase->initialiseComponents();
}

extern "C" _AnomalousExport void ItemListBase_resetList(CEGUI::ItemListBase* itemListBase)
{
	itemListBase->resetList();
}

extern "C" _AnomalousExport void ItemListBase_addItem(CEGUI::ItemListBase* itemListBase, CEGUI::ItemEntry* item)
{
	itemListBase->addItem(item);
}

extern "C" _AnomalousExport void ItemListBase_insertItem(CEGUI::ItemListBase* itemListBase, CEGUI::ItemEntry* item, CEGUI::ItemEntry* position)
{
	itemListBase->insertItem(item, position);
}

extern "C" _AnomalousExport void ItemListBase_removeItem(CEGUI::ItemListBase* itemListBase, CEGUI::ItemEntry* item)
{
	itemListBase->removeItem(item);
}

extern "C" _AnomalousExport void ItemListBase_handleUpdatedItemData(CEGUI::ItemListBase* itemListBase)
{
	itemListBase->handleUpdatedItemData();
}

extern "C" _AnomalousExport void ItemListBase_handleUpdatedItemData2(CEGUI::ItemListBase* itemListBase, bool resort)
{
	itemListBase->handleUpdatedItemData(resort);
}

extern "C" _AnomalousExport void ItemListBase_setAutoResizeEnabled(CEGUI::ItemListBase* itemListBase, bool setting)
{
	itemListBase->setAutoResizeEnabled(setting);
}

extern "C" _AnomalousExport void ItemListBase_sizeToContent(CEGUI::ItemListBase* itemListBase)
{
	itemListBase->sizeToContent();
}

extern "C" _AnomalousExport void ItemListBase_endInitialisation(CEGUI::ItemListBase* itemListBase)
{
	itemListBase->endInitialisation();
}

extern "C" _AnomalousExport void ItemListBase_performChildWindowLayout(CEGUI::ItemListBase* itemListBase)
{
	itemListBase->performChildWindowLayout();
}

extern "C" _AnomalousExport Rect ItemListBase_getItemRenderArea(CEGUI::ItemListBase* itemListBase)
{
	return itemListBase->getItemRenderArea();
}

extern "C" _AnomalousExport CEGUI::Window* ItemListBase_getContentPane(CEGUI::ItemListBase* itemListBase)
{
	return itemListBase->getContentPane();
}

extern "C" _AnomalousExport void ItemListBase_notifyItemClicked(CEGUI::ItemListBase* itemListBase, CEGUI::ItemEntry* item)
{
	itemListBase->notifyItemClicked(item);
}

extern "C" _AnomalousExport void ItemListBase_notifyItemSelectState(CEGUI::ItemListBase* itemListBase, CEGUI::ItemEntry* item, bool select)
{
	itemListBase->notifyItemSelectState(item, select);
}

extern "C" _AnomalousExport void ItemListBase_setSortEnabled(CEGUI::ItemListBase* itemListBase, bool setting)
{
	itemListBase->setSortEnabled(setting);
}

extern "C" _AnomalousExport void ItemListBase_setSortMode(CEGUI::ItemListBase* itemListBase, CEGUI::ItemListBase::SortMode mode)
{
	itemListBase->setSortMode(mode);
}

extern "C" _AnomalousExport void ItemListBase_sortList(CEGUI::ItemListBase* itemListBase)
{
	itemListBase->sortList();
}

extern "C" _AnomalousExport void ItemListBase_sortList2(CEGUI::ItemListBase* itemListBase, bool relayout)
{
	itemListBase->sortList(relayout);
}

#pragma warning(pop)