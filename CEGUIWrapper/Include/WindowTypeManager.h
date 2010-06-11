#pragma once

#include <vector>

class WindowTypeEntry;

typedef std::vector<WindowTypeEntry*> ChildWindowCollection;
typedef ChildWindowCollection::iterator ChildWindowIterator;

class WindowTypeEntry
{
private:
	CEGUI::String name;
	int windowType;
	WindowTypeEntry* parent;
	ChildWindowCollection children;

public:
	WindowTypeEntry(const CEGUI::String& name, int windowType, WindowTypeEntry* parent);

	~WindowTypeEntry();

	WindowTypeEntry* addType(const CEGUI::String& name, int windowType);

	int searchType(CEGUI::Window* window);

	WindowTypeEntry* getParent();
};

class WindowTypeManager
{
private:
	WindowTypeEntry baseType;
	WindowTypeEntry* currentType;

public:
	WindowTypeManager(String baseName, int baseWindowType);

	~WindowTypeManager(void);

	void pushType(String name, int windowType);

	void popType();

	int searchType(CEGUI::Window* window);
};
