#pragma once

#include "Primitives/interface/BasicTypes.h"
#include <map>
#include <string>

class StringManager
{
private:
	std::map<std::string, Diligent::Char*> stringMap;

public:
	StringManager()
	{

	}

	~StringManager();

	const Diligent::Char* SetString(std::string name, const Diligent::Char* input, size_t length);
};