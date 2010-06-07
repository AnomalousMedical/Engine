#include "Stdafx.h"

extern "C" _AnomalousExport void btTypedConstraint_Delete(btTypedConstraint* instance)
{
	delete instance;
}