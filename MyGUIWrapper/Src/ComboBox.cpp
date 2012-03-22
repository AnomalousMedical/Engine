#include "Stdafx.h"

extern "C" _AnomalousExport size_t ComboBox_getItemCount(MyGUI::ComboBox* comboBox)
{
	return comboBox->getItemCount();
}

extern "C" _AnomalousExport void ComboBox_insertItemAt(MyGUI::ComboBox* comboBox, size_t index, UStringIn name)
{
	comboBox->insertItemAt(index, name);
}

extern "C" _AnomalousExport void ComboBox_addItem(MyGUI::ComboBox* comboBox, UStringIn name)
{
	comboBox->addItem(name);
}

extern "C" _AnomalousExport void ComboBox_removeItemAt(MyGUI::ComboBox* comboBox, size_t index)
{
	comboBox->removeItemAt(index);
}

extern "C" _AnomalousExport void ComboBox_removeAllItems(MyGUI::ComboBox* comboBox)
{
	comboBox->removeAllItems();
}

extern "C" _AnomalousExport size_t ComboBox_findItemIndexWith(MyGUI::ComboBox* comboBox, UStringIn name)
{
	return comboBox->findItemIndexWith(name);
}

extern "C" _AnomalousExport size_t ComboBox_getIndexSelected(MyGUI::ComboBox* comboBox)
{
	return comboBox->getIndexSelected();
}

extern "C" _AnomalousExport void ComboBox_setIndexSelected(MyGUI::ComboBox* comboBox, size_t index)
{
	comboBox->setIndexSelected(index);
}

extern "C" _AnomalousExport void ComboBox_clearIndexSelected(MyGUI::ComboBox* comboBox)
{
	comboBox->clearIndexSelected();
}

extern "C" _AnomalousExport void ComboBox_setItemNameAt(MyGUI::ComboBox* comboBox, size_t index, UStringIn name)
{
	comboBox->setItemNameAt(index, name);
}

extern "C" _AnomalousExport UStringOut ComboBox_getItemNameAt(MyGUI::ComboBox* comboBox, size_t index)
{
	return comboBox->getItemNameAt(index).c_str();
}

extern "C" _AnomalousExport void ComboBox_beginToItemAt(MyGUI::ComboBox* comboBox, size_t index)
{
	comboBox->beginToItemAt(index);
}

extern "C" _AnomalousExport void ComboBox_beginToItemFirst(MyGUI::ComboBox* comboBox)
{
	comboBox->beginToItemFirst();
}

extern "C" _AnomalousExport void ComboBox_beginToItemLast(MyGUI::ComboBox* comboBox)
{
	comboBox->beginToItemLast();
}

extern "C" _AnomalousExport void ComboBox_beginToItemSelected(MyGUI::ComboBox* comboBox)
{
	comboBox->beginToItemSelected();
}

extern "C" _AnomalousExport void ComboBox_setComboModeDrop(MyGUI::ComboBox* comboBox, bool value)
{
	comboBox->setComboModeDrop(value);
}

extern "C" _AnomalousExport bool ComboBox_getComboModeDrop(MyGUI::ComboBox* comboBox)
{
	return comboBox->getComboModeDrop();
}

extern "C" _AnomalousExport void ComboBox_setSmoothShow(MyGUI::ComboBox* comboBox, bool value)
{
	comboBox->setSmoothShow(value);
}

extern "C" _AnomalousExport bool ComboBox_getSmoothShow(MyGUI::ComboBox* comboBox)
{
	return comboBox->getSmoothShow();
}

extern "C" _AnomalousExport void ComboBox_setMaxListLength(MyGUI::ComboBox* comboBox, int value)
{
	comboBox->setMaxListLength(value);
}

extern "C" _AnomalousExport int ComboBox_getMaxListLength(MyGUI::ComboBox* comboBox)
{
	return comboBox->getMaxListLength();
}