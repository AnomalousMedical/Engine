#include "Stdafx.h"

extern "C" _AnomalousExport void btTypedConstraint_Delete(btTypedConstraint* instance)
{
	delete instance;
}

extern "C" _AnomalousExport void btTypedConstraint_setOverrideNumSolverIterations(btTypedConstraint* instance, int overrideNumIterations)
{
	instance->setOverrideNumSolverIterations(overrideNumIterations);
}

extern "C" _AnomalousExport int btTypedConstraint_getOverrideNumSolverIterations(btTypedConstraint* instance)
{
	return instance->getOverrideNumSolverIterations();
}