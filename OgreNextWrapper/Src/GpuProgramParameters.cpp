#include "Stdafx.h"

extern "C" _AnomalousExport void GpuProgramParameters_addSharedParameters(Ogre::GpuProgramParameters* param, Ogre::GpuSharedParametersPtr* sharedParams)
{
	try 
	{
		param->addSharedParameters(*sharedParams);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_addSharedParametersName(Ogre::GpuProgramParameters* param, String name)
{
	try
	{
		param->addSharedParameters(name);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant1(Ogre::GpuProgramParameters* param, String name, Ogre::Real val)
{
	try
	{
		param->setNamedConstant(name, val);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant2(Ogre::GpuProgramParameters* param, String name, int val)
{
	try 
	{
		param->setNamedConstant(name, val);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant3(Ogre::GpuProgramParameters* param, String name, Quaternion vec)
{
	try
	{
		param->setNamedConstant(name, vec.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant4(Ogre::GpuProgramParameters* param, String name, Vector3 vec)
{
	try
	{
		param->setNamedConstant(name, vec.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant5(Ogre::GpuProgramParameters* param, String name, Vector2 vec)
{
	try
	{
		param->setNamedConstant(name, vec.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant6(Ogre::GpuProgramParameters* param, String name, Color colour)
{
	try
	{
		param->setNamedConstant(name, colour.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant7(Ogre::GpuProgramParameters* param, String name, int *val, size_t count)
{
	try
	{
		param->setNamedConstant(name, val, count);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant8(Ogre::GpuProgramParameters* param, String name, float *val, size_t count)
{
	try
	{
		param->setNamedConstant(name, val, count);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant9(Ogre::GpuProgramParameters* param, String name, double *val, size_t count)
{
	try
	{
		param->setNamedConstant(name, val, count);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

//Indexed
extern "C" _AnomalousExport void GpuProgramParameters_setConstant1(Ogre::GpuProgramParameters* param, size_t index, Ogre::Real val)
{
	try
	{
		param->setConstant(index, val);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant2(Ogre::GpuProgramParameters* param, size_t index, int val)
{
	try
	{
		param->setConstant(index, val);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant3(Ogre::GpuProgramParameters* param, size_t index, Quaternion vec)
{
	try
	{
		param->setConstant(index, vec.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant4(Ogre::GpuProgramParameters* param, size_t index, Vector3 vec)
{
	try
	{
		param->setConstant(index, vec.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant5(Ogre::GpuProgramParameters* param, size_t index, Vector2 vec)
{
	try
	{
		param->setConstant(index, vec.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant6(Ogre::GpuProgramParameters* param, size_t index, Color colour)
{
	try
	{
		param->setConstant(index, colour.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant7(Ogre::GpuProgramParameters* param, size_t index, int *val, size_t count)
{
	try
	{
		param->setConstant(index, val, count);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant8(Ogre::GpuProgramParameters* param, size_t index, float *val, size_t count)
{
	try
	{
		param->setConstant(index, val, count);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant9(Ogre::GpuProgramParameters* param, size_t index, double *val, size_t count)
{
	try
	{
		param->setConstant(index, val, count);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport bool GpuProgramParameters_hasNamedConstant(Ogre::GpuProgramParameters* param, String name)
{
	try
	{
		return param->_findNamedConstantDefinition(name, false) != NULL;
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
    return false;
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedAutoConstant1(Ogre::GpuProgramParameters* param, String name, Ogre::GpuProgramParameters::AutoConstantType acType)
{
	try
	{
		param->setNamedAutoConstant(name, acType);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedAutoConstant2(Ogre::GpuProgramParameters* param, String name, Ogre::GpuProgramParameters::AutoConstantType acType, size_t extraInfo)
{
	try
	{
		param->setNamedAutoConstant(name, acType, extraInfo);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedAutoConstant3(Ogre::GpuProgramParameters* param, String name, Ogre::GpuProgramParameters::AutoConstantType acType, size_t extraInfo1, size_t extraInfo2)
{
	try
	{
		param->setNamedAutoConstant(name, acType, extraInfo1, extraInfo2);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedAutoConstantReal(Ogre::GpuProgramParameters* param, String name, Ogre::GpuProgramParameters::AutoConstantType acType, float rData)
{
	try
	{
		param->setNamedAutoConstantReal(name, acType, rData);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

//Shared Program Parameters
extern "C" _AnomalousExport void GpuSharedParameters_addConstantDefinition(Ogre::GpuSharedParameters* param, String name, Ogre::GpuConstantType constType, size_t arraySize)
{
	try
	{
		param->addConstantDefinition(name, constType, arraySize);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant1(Ogre::GpuSharedParameters* param, String name, Ogre::Real val)
{
	try
	{
		param->setNamedConstant(name, val);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant2(Ogre::GpuSharedParameters* param, String name, int val)
{
	try
	{
		param->setNamedConstant(name, val);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant3(Ogre::GpuSharedParameters* param, String name, Quaternion vec)
{
	try
	{
		param->setNamedConstant(name, vec.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant4(Ogre::GpuSharedParameters* param, String name, Vector3 vec)
{
	try
	{
		param->setNamedConstant(name, vec.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant5(Ogre::GpuSharedParameters* param, String name, Vector2 vec)
{
	try
	{
		param->setNamedConstant(name, vec.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant6(Ogre::GpuSharedParameters* param, String name, Color colour)
{
	try
	{
		param->setNamedConstant(name, colour.toOgre());
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant7(Ogre::GpuSharedParameters* param, String name, int *val, size_t count)
{
	try
	{
		param->setNamedConstant(name, val, count);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant8(Ogre::GpuSharedParameters* param, String name, float *val, size_t count)
{
	try
	{
		param->setNamedConstant(name, val, count);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant9(Ogre::GpuSharedParameters* param, String name, double *val, size_t count)
{
	try
	{
		param->setNamedConstant(name, val, count);
	}
	catch (Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}