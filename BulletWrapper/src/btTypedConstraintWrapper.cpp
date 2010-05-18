#include "Stdafx.h"

extern "C" _declspec(dllexport) void btTypedConstraint_Delete(btTypedConstraint* instance)
{
	delete instance;
}