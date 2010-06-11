#include "StdAfx.h"
#include "..\Include\WindowTypeManager.h"

WindowTypeEntry::WindowTypeEntry(const CEGUI::String& name, int windowType, WindowTypeEntry* parent)
:name(name),
windowType(windowType),
parent(parent)
{

}

WindowTypeEntry::~WindowTypeEntry()
{
	for(ChildWindowIterator iter = children.begin(); iter != children.end(); iter++)
	{
		delete (*iter);
	}
}

WindowTypeEntry* WindowTypeEntry::addType(const CEGUI::String& name, int windowType)
{
	WindowTypeEntry* entry = new WindowTypeEntry(name, windowType, this);
	children.push_back(entry);
	return entry;
}

int WindowTypeEntry::searchType(CEGUI::Window* window)
{
	if(window->testClassName(name))
	{
		for(ChildWindowIterator iter = children.begin(); iter != children.end(); iter++)
		{
			int childType = (*iter)->searchType(window);
			if(childType != -1)
			{
				return childType;
			}
		}
		return windowType;
	}
	return -1;
}

WindowTypeEntry* WindowTypeEntry::getParent()
{
	return parent;
}

WindowTypeManager::WindowTypeManager(String baseName, int baseWindowType)
:baseType(baseName, baseWindowType, 0),
currentType(&baseType)
{

}

WindowTypeManager::~WindowTypeManager(void)
{

}

void WindowTypeManager::pushType(String name, int windowType)
{
	currentType = currentType->addType(name, windowType);
}

void WindowTypeManager::popType()
{
	currentType = currentType->getParent();
}

int WindowTypeManager::searchType(CEGUI::Window* window)
{
	return baseType.searchType(window);
}

extern "C" _AnomalousExport WindowTypeManager* WindowTypeManager_Create(String baseName, int baseWindowType)
{
	return new WindowTypeManager(baseName, baseWindowType);
}

extern "C" _AnomalousExport void WindowTypeManager_Delete(WindowTypeManager* manager)
{
	delete manager;
}

extern "C" _AnomalousExport void WindowTypeManager_pushType(WindowTypeManager* manager, String name, int windowType)
{
	manager->pushType(name, windowType);
}

extern "C" _AnomalousExport void WindowTypeManager_popType(WindowTypeManager* manager)
{
	manager->popType();
}

extern "C" _AnomalousExport int WindowTypeManager_searchType(WindowTypeManager* manager, CEGUI::Window* window)
{
	return manager->searchType(window);
}