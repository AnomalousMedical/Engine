// stdafx.cpp : source file that includes just the standard includes
// ZipAccess.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

std::string convertString( System::String^ str )
{
	msclr::interop::marshal_context context;
	return context.marshal_as<const char*>(str);
}

System::String^ convertString( std::string str )
{
	return gcnew System::String( str.c_str() );
}