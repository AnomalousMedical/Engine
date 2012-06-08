#include "Stdafx.h"

extern "C" _AnomalousExport void EditBox_setTextIntervalColor(MyGUI::EditBox* edit, size_t start, size_t count, Color colour)
{
	edit->setTextIntervalColour(start, count, colour.toMyGUI());
}

extern "C" _AnomalousExport void EditBox_setTextSelection(MyGUI::EditBox* edit, size_t start, size_t end)
{
	edit->setTextSelection(start, end);
}

extern "C" _AnomalousExport void EditBox_deleteTextSelection(MyGUI::EditBox* edit)
{
	edit->deleteTextSelection();
}

extern "C" _AnomalousExport void EditBox_setTextSelectionColor(MyGUI::EditBox* edit, Color value)
{
	edit->setTextSelectionColour(value.toMyGUI());
}

extern "C" _AnomalousExport void EditBox_insertText1(MyGUI::EditBox* edit, UStringIn text)
{
	edit->insertText(text);
}

extern "C" _AnomalousExport void EditBox_insertText2(MyGUI::EditBox* edit, UStringIn text, size_t index)
{
	edit->insertText(text, index);
}

extern "C" _AnomalousExport void EditBox_addText(MyGUI::EditBox* edit, UStringIn text)
{
	edit->addText(text);
}

extern "C" _AnomalousExport void EditBox_eraseText1(MyGUI::EditBox* edit, size_t start)
{
	edit->eraseText(start);
}

extern "C" _AnomalousExport void EditBox_eraseText2(MyGUI::EditBox* edit, size_t start, size_t count)
{
	edit->eraseText(start, count);
}

extern "C" _AnomalousExport size_t EditBox_getTextSelectionStart(MyGUI::EditBox* edit)
{
	return edit->getTextSelectionStart();
}

extern "C" _AnomalousExport size_t EditBox_getTextSelectionEnd(MyGUI::EditBox* edit)
{
	return edit->getTextSelectionEnd();
}

extern "C" _AnomalousExport size_t EditBox_getTextSelectionLength(MyGUI::EditBox* edit)
{
	return edit->getTextSelectionLength();
}

extern "C" _AnomalousExport bool EditBox_isTextSelection(MyGUI::EditBox* edit)
{
	return edit->isTextSelection();
}

extern "C" _AnomalousExport void EditBox_setTextCursor(MyGUI::EditBox* edit, size_t index)
{
	edit->setTextCursor(index);
}

extern "C" _AnomalousExport size_t EditBox_getTextCursor(MyGUI::EditBox* edit)
{
	return edit->getTextCursor();
}

extern "C" _AnomalousExport size_t EditBox_getTextLength(MyGUI::EditBox* edit)
{
	return edit->getTextLength();
}

extern "C" _AnomalousExport void EditBox_setOverflowToTheLeft(MyGUI::EditBox* edit, bool value)
{
	edit->setOverflowToTheLeft(value);
}

extern "C" _AnomalousExport bool EditBox_getOverflowToTheLeft(MyGUI::EditBox* edit)
{
	return edit->getOverflowToTheLeft();
}

extern "C" _AnomalousExport void EditBox_setMaxTextLength(MyGUI::EditBox* edit, size_t value)
{
	edit->setMaxTextLength(value);
}

extern "C" _AnomalousExport size_t EditBox_getMaxTextLength(MyGUI::EditBox* edit)
{
	return edit->getMaxTextLength();
}

extern "C" _AnomalousExport void EditBox_setEditReadOnly(MyGUI::EditBox* edit, bool value)
{
	edit->setEditReadOnly(value);
}

extern "C" _AnomalousExport bool EditBox_getEditReadOnly(MyGUI::EditBox* edit)
{
	return edit->getEditReadOnly();
}

extern "C" _AnomalousExport void EditBox_setEditPassword(MyGUI::EditBox* edit, bool value)
{
	edit->setEditPassword(value);
}

extern "C" _AnomalousExport bool EditBox_getEditPassword(MyGUI::EditBox* edit)
{
	return edit->getEditPassword();
}

extern "C" _AnomalousExport void EditBox_setEditMultiLine(MyGUI::EditBox* edit, bool value)
{
	edit->setEditMultiLine(value);
}

extern "C" _AnomalousExport bool EditBox_getEditMultiLine(MyGUI::EditBox* edit)
{
	return edit->getEditMultiLine();
}

extern "C" _AnomalousExport void EditBox_setEditStatic(MyGUI::EditBox* edit, bool value)
{
	edit->setEditStatic(value);
}

extern "C" _AnomalousExport bool EditBox_getEditStatic(MyGUI::EditBox* edit)
{
	return edit->getEditStatic();
}

extern "C" _AnomalousExport void EditBox_setPasswordChar(MyGUI::EditBox* edit, char value)
{
	edit->setPasswordChar(value);
}

extern "C" _AnomalousExport char EditBox_getPasswordChar(MyGUI::EditBox* edit)
{
	return edit->getPasswordChar();
}

extern "C" _AnomalousExport void EditBox_setEditWordWrap(MyGUI::EditBox* edit, bool value)
{
	edit->setEditWordWrap(value);
}

extern "C" _AnomalousExport bool EditBox_getEditWordWrap(MyGUI::EditBox* edit)
{
	return edit->getEditWordWrap();
}

extern "C" _AnomalousExport void EditBox_setTabPrinting(MyGUI::EditBox* edit, bool value)
{
	edit->setTabPrinting(value);
}

extern "C" _AnomalousExport bool EditBox_getTabPrinting(MyGUI::EditBox* edit)
{
	return edit->getTabPrinting();
}

extern "C" _AnomalousExport bool EditBox_getInvertSelected(MyGUI::EditBox* edit)
{
	return edit->getInvertSelected();
}

extern "C" _AnomalousExport void EditBox_setInvertSelected(MyGUI::EditBox* edit, bool value)
{
	edit->setInvertSelected(value);
}

extern "C" _AnomalousExport void EditBox_setVisibleVScroll(MyGUI::EditBox* edit, bool value)
{
	edit->setVisibleVScroll(value);
}

extern "C" _AnomalousExport bool EditBox_isVisibleVScroll(MyGUI::EditBox* edit)
{
	return edit->isVisibleVScroll();
}

extern "C" _AnomalousExport size_t EditBox_getVScrollRange(MyGUI::EditBox* edit)
{
	return edit->getVScrollRange();
}

extern "C" _AnomalousExport size_t EditBox_getVScrollPosition(MyGUI::EditBox* edit)
{
	return edit->getVScrollPosition();
}

extern "C" _AnomalousExport void EditBox_setVScrollPosition(MyGUI::EditBox* edit, size_t index)
{
	edit->setVScrollPosition(index);
}

extern "C" _AnomalousExport void EditBox_setVisibleHScroll(MyGUI::EditBox* edit, bool value)
{
	edit->setVisibleHScroll(value);
}

extern "C" _AnomalousExport bool EditBox_isVisibleHScroll(MyGUI::EditBox* edit)
{
	return edit->isVisibleHScroll();
}

extern "C" _AnomalousExport size_t EditBox_getHScrollRange(MyGUI::EditBox* edit)
{
	return edit->getHScrollRange();
}

extern "C" _AnomalousExport size_t EditBox_getHScrollPosition(MyGUI::EditBox* edit)
{
	return edit->getHScrollPosition();
}

extern "C" _AnomalousExport void EditBox_setHScrollPosition(MyGUI::EditBox* edit, size_t index)
{
	edit->setHScrollPosition(index);
}

extern "C" _AnomalousExport void EditBox_setAllowMouseScroll(MyGUI::EditBox* edit, bool value)
{
	edit->setAllowMouseScroll(value);
}

extern "C" _AnomalousExport bool EditBox_getAllowMouseScroll(MyGUI::EditBox* edit)
{
	return edit->getAllowMouseScroll();
}

extern "C" _AnomalousExport void EditBox_setOnlyText(MyGUI::EditBox* edit, UStringIn value)
{
	edit->setOnlyText(value);
}

extern "C" _AnomalousExport void EditBox_getOnlyText(MyGUI::EditBox* edit, TempStringCallback onlyTextDelegate)
{
	onlyTextDelegate(edit->getOnlyText().c_str());
}

extern "C" _AnomalousExport void EditBox_cut(MyGUI::EditBox* edit)
{
	return edit->cut();
}

extern "C" _AnomalousExport void EditBox_copy(MyGUI::EditBox* edit)
{
	return edit->copy();
}

extern "C" _AnomalousExport void EditBox_paste(MyGUI::EditBox* edit)
{
	return edit->paste();
}