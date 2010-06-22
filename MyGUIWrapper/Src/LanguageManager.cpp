#include "Stdafx.h"

extern "C" _AnomalousExport MyGUI::LanguageManager* LanguageManager_getInstancePtr()
{
	return MyGUI::LanguageManager::getInstancePtr();
}

extern "C" _AnomalousExport void LanguageManager_setCurrentLanguage(MyGUI::LanguageManager* languageManager, String name)
{
	languageManager->setCurrentLanguage(name);
}

extern "C" _AnomalousExport String LanguageManager_getCurrentLanguage(MyGUI::LanguageManager* languageManager)
{
	return languageManager->getCurrentLanguage().c_str();
}

extern "C" _AnomalousExport void LanguageManager_addUserTag(MyGUI::LanguageManager* languageManager, UStringIn tag, UStringIn replace)
{
	languageManager->addUserTag(tag, replace);
}

extern "C" _AnomalousExport void LanguageManager_clearUserTags(MyGUI::LanguageManager* languageManager)
{
	languageManager->clearUserTags();
}

extern "C" _AnomalousExport bool LanguageManager_loadUserTags(MyGUI::LanguageManager* languageManager, String file)
{
	return languageManager->loadUserTags(file);
}