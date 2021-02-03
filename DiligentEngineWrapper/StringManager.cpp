#include "StdAfx.h"
#include <map>
#include <string>
#include <cstdio>
#include <cstring>
#include "Graphics/GraphicsEngine/interface/SwapChain.h"

using namespace Diligent;
using namespace std;

class StringManager 
{
private:
	map<string, Char*> stringMap;

public: 
	StringManager() 
	{

	}

	~StringManager() 
	{
		auto it = stringMap.begin();
		while (it != stringMap.end()) 
		{
			delete[] it->second;
			it++;
		}
	}

	Char* SetString(string name, Char* input, size_t length)
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

		size_t finalSize = length + 1;
		Char* newCopy = new Char[finalSize]; //Include null terminator
		strncpy_s(newCopy, finalSize, input, finalSize);
		stringMap[name] = newCopy;
		return newCopy;
	}
};