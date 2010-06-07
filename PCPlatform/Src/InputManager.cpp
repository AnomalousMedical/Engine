#include "Stdafx.h"
#include "OIS.h"

extern "C" _AnomalousExport OIS::InputManager* InputManager_Create(char* windowHandle, bool foreground, bool exclusive, bool noWinKey)
{
	OIS::ParamList pl;

	pl.insert(std::make_pair( std::string("WINDOW"), windowHandle ));

	std::string foregroundMode = "DISCL_BACKGROUND";
	if( foreground )
	{
		foregroundMode = "DISCL_FOREGROUND";
	}
	std::string exclusiveMode = "DISCL_NONEXCLUSIVE";
	if( exclusive )
	{
		exclusiveMode = "DISCL_EXCLUSIVE";
	}
	pl.insert(std::make_pair(std::string("w32_mouse"), foregroundMode));
	pl.insert(std::make_pair(std::string("w32_mouse"), exclusiveMode));
	if( noWinKey )
	{
		pl.insert(std::make_pair(std::string("w32_mouse"), std::string("DISCL_NOWINKEY")));
	}

	return OIS::InputManager::createInputSystem(pl);
}

extern "C" _AnomalousExport void InputManager_Delete(OIS::InputManager* inputManager)
{
	OIS::InputManager::destroyInputSystem(inputManager);
}

extern "C" _AnomalousExport int InputManager_getNumberOfDevices(OIS::InputManager* inputManager, OIS::Type inputType)
{
	return inputManager->getNumberOfDevices(inputType);
}

extern "C" _AnomalousExport unsigned int InputManager_getVersionNumber(OIS::InputManager* inputManager)
{
	return inputManager->getVersionNumber();
}

extern "C" _AnomalousExport const char* InputManager_getVersionName(OIS::InputManager* inputManager)
{
	return inputManager->getVersionName().c_str();
}

extern "C" _AnomalousExport const char* InputManager_inputSystemName(OIS::InputManager* inputManager)
{
	return inputManager->inputSystemName().c_str();
}

extern "C" _AnomalousExport OIS::Object* InputManager_createInputObject(OIS::InputManager* inputManager, OIS::Type inputType, bool buffered)
{
	return inputManager->createInputObject(inputType, buffered);
}

extern "C" _AnomalousExport void InputManager_destroyInputObject(OIS::InputManager* inputManager, OIS::Object* inputObject)
{
	inputManager->destroyInputObject(inputObject);
}