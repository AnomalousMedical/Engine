/// <file>MarshalUtils.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

#include <msclr/marshal.h>
#include <string>

/// <summary>
/// Helper class to assist in marshaling from managed to native.
/// </summary>
ref class MarshalUtils
{
public:
	/// <summary>
	/// Converts between System::String and std::string.  Will allocate a new std::string.
	/// </summary>
	/// <param name="str">The System::String to convert.</param>
	/// <returns>A new std::string with the value in str.</returns>
	static std::string convertString( System::String^ str )
	{
		msclr::interop::marshal_context context;
		return context.marshal_as<const char*>(str);
	}

	static System::String^ convertString( std::string str )
	{
		return gcnew System::String( str.c_str() );
	}
};