#pragma once

namespace OgreWrapper
{

public ref class ConfigOption
{
private:
	System::String^ name;
	System::String^ currentValue;
	System::Collections::Generic::List<System::String^>^ possibleValues;
	bool immutable;

internal:
	ConfigOption(Ogre::ConfigOption* configOption);

public:
	property System::String^ Name 
	{
		System::String^ get();
	}

	property System::String^ CurrentValue 
	{
		System::String^ get();
	}

	property System::Collections::Generic::List<System::String^>^ PossibleValues 
	{
		System::Collections::Generic::List<System::String^>^ get();
	}

	property bool Immutable 
	{
		bool get();
	}
};

}