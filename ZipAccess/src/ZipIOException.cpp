#include "StdAfx.h"
#include "..\include\ZipIOException.h"

namespace ZipAccess
{

ZipIOException::ZipIOException(String^ message, ...cli::array<System::Object^>^ args)
:Exception(String::Format(message, args))
{
}

}