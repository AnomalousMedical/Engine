#include "stdafx.h"

extern "C" _AnomalousExport void Message_setMessageText(MyGUI::Message* message, UStringIn value)
{
	message->setMessageText(value);
}

extern "C" _AnomalousExport void Message_addButtonName(MyGUI::Message* message, UStringIn name)
{
	message->addButtonName(name);
}

extern "C" _AnomalousExport void Message_setSmoothShow(MyGUI::Message* message, bool value)
{
	message->setSmoothShow(value);
}

extern "C" _AnomalousExport String Message_getDefaultLayer(MyGUI::Message* message)
{
	return message->getDefaultLayer().c_str();
}

extern "C" _AnomalousExport void Message_setMessageIcon(MyGUI::Message* message, MyGUI::MessageBoxStyle::Enum value)
{
	message->setMessageIcon(value);
}

extern "C" _AnomalousExport void Message_setWindowFade(MyGUI::Message* message, bool value)
{
	message->setWindowFade(value);
}

extern "C" _AnomalousExport void Message_endMessage(MyGUI::Message* message, MyGUI::MessageBoxStyle::Enum result)
{
	message->endMessage(result);
}

extern "C" _AnomalousExport void Message_endMessage2(MyGUI::Message* message)
{
	message->endMessage();
}

extern "C" _AnomalousExport void Message_setMessageButton(MyGUI::Message* message, MyGUI::MessageBoxStyle::Enum value)
{
	message->setMessageButton(value);
}

extern "C" _AnomalousExport void Message_setMessageStyle(MyGUI::Message* message, MyGUI::MessageBoxStyle::Enum value)
{
	message->setMessageStyle(value);
}

extern "C" _AnomalousExport void Message_setMessageModal(MyGUI::Message* message, bool value)
{
	message->setMessageModal(value);
}

extern "C" _AnomalousExport MyGUI::Message* Message_createMessageBox(
 String skin,
 UStringIn caption,
 UStringIn message,
 MyGUI::MessageBoxStyle::Enum style,
 String layer,
 bool modal,
 String button1,
 String button2,
 String button3,
 String button4)
{
	return MyGUI::Message::createMessageBox(skin, caption, message, style, layer, modal, button1, button2, button3, button4);
}