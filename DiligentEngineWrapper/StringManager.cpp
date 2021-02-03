#include "StdAfx.h"
#include <map>
#include <string>
#include "StringManager.h"

using namespace Diligent;
using namespace std;

StringManager::~StringManager()
{
	auto it = stringMap.begin();
	while (it != stringMap.end())
	{
		delete[] it->second;
		it++;
	}
}

const Char* StringManager::SetString(string name, const Char* input, size_t length)
{
	auto currentValue = stringMap.find(name);
	if (currentValue != stringMap.end())
	{
		delete[] currentValue->second;
	}

	if (input == nullptr)
	{
		stringMap.erase(name);
		return nullptr;
	}

	size_t finalSize = length + 1; //Include null terminator
	Char* newCopy = new Char[finalSize];
	strncpy_s(newCopy, finalSize, input, finalSize);
	stringMap[name] = newCopy;
	return newCopy;
}

extern "C" _AnomalousExport StringManager * StringManager_Create()
{
	return new StringManager();
}

extern "C" _AnomalousExport void StringManager_Delete(StringManager * obj)
{
	delete obj;
}