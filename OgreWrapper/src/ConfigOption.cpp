#include "StdAfx.h"
#include "..\include\ConfigOption.h"
#include "MarshalUtils.h"

namespace OgreWrapper
{

ConfigOption::ConfigOption(Ogre::ConfigOption* configOption)
:possibleValues(gcnew System::Collections::Generic::List<System::String^>())
{
	this->name = MarshalUtils::convertString(configOption->name);
	this->currentValue = MarshalUtils::convertString(configOption->currentValue);
	Ogre::StringVector::iterator iter;
	for(iter = configOption->possibleValues.begin(); iter != configOption->possibleValues.end(); ++iter)
	{
		possibleValues->Add(MarshalUtils::convertString(*iter));
	}
	immutable = configOption->immutable;
}

System::String^ ConfigOption::Name::get() 
{
	return name;
}

System::String^ ConfigOption::CurrentValue::get() 
{
	return currentValue;
}

System::Collections::Generic::List<System::String^>^ ConfigOption::PossibleValues::get() 
{
	return possibleValues;
}

bool ConfigOption::Immutable::get() 
{
	return immutable;
}

}