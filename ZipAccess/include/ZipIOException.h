#pragma once

using namespace System;

namespace ZipAccess
{

public ref class ZipIOException : Exception
{
public:
	ZipIOException(String^ message, ...cli::array<System::Object^>^ args);
};

}