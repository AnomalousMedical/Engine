#include "Stdafx.h"

extern "C" _AnomalousExport void GpuProgramParameters_addSharedParameters(Ogre::GpuProgramParameters* param, Ogre::GpuSharedParametersPtr* sharedParams)
{
	param->addSharedParameters(*sharedParams);
}

extern "C" _AnomalousExport void GpuProgramParameters_addSharedParametersName(Ogre::GpuProgramParameters* param, String name)
{
	param->addSharedParameters(name);
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant1(Ogre::GpuProgramParameters* param, String name, Ogre::Real val)
{
	param->setNamedConstant(name, val);
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant2(Ogre::GpuProgramParameters* param, String name, int val)
{
	param->setNamedConstant(name, val);
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant3(Ogre::GpuProgramParameters* param, String name, Quaternion vec)
{
	param->setNamedConstant(name, vec.toOgre());
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant4(Ogre::GpuProgramParameters* param, String name, Vector3 vec)
{
	param->setNamedConstant(name, vec.toOgre());
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant5(Ogre::GpuProgramParameters* param, String name, Vector2 vec)
{
	param->setNamedConstant(name, vec.toOgre());
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant6(Ogre::GpuProgramParameters* param, String name, Color colour)
{
	param->setNamedConstant(name, colour.toOgre());
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant7(Ogre::GpuProgramParameters* param, String name, int *val, size_t count)
{
	param->setNamedConstant(name, val, count);
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant8(Ogre::GpuProgramParameters* param, String name, float *val, size_t count)
{
	param->setNamedConstant(name, val, count);
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedConstant9(Ogre::GpuProgramParameters* param, String name, double *val, size_t count)
{
	param->setNamedConstant(name, val, count);
}

//Indexed
extern "C" _AnomalousExport void GpuProgramParameters_setConstant1(Ogre::GpuProgramParameters* param, size_t index, Ogre::Real val)
{
	param->setConstant(index, val);
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant2(Ogre::GpuProgramParameters* param, size_t index, int val)
{
	param->setConstant(index, val);
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant3(Ogre::GpuProgramParameters* param, size_t index, Quaternion vec)
{
	param->setConstant(index, vec.toOgre());
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant4(Ogre::GpuProgramParameters* param, size_t index, Vector3 vec)
{
	param->setConstant(index, vec.toOgre());
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant5(Ogre::GpuProgramParameters* param, size_t index, Vector2 vec)
{
	param->setConstant(index, vec.toOgre());
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant6(Ogre::GpuProgramParameters* param, size_t index, Color colour)
{
	param->setConstant(index, colour.toOgre());
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant7(Ogre::GpuProgramParameters* param, size_t index, int *val, size_t count)
{
	param->setConstant(index, val, count);
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant8(Ogre::GpuProgramParameters* param, size_t index, float *val, size_t count)
{
	param->setConstant(index, val, count);
}

extern "C" _AnomalousExport void GpuProgramParameters_setConstant9(Ogre::GpuProgramParameters* param, size_t index, double *val, size_t count)
{
	param->setConstant(index, val, count);
}

extern "C" _AnomalousExport bool GpuProgramParameters_hasNamedConstant(Ogre::GpuProgramParameters* param, String name)
{
	return param->_findNamedConstantDefinition(name, false) != NULL;
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedAutoConstant1(Ogre::GpuProgramParameters* param, String name, Ogre::GpuProgramParameters::AutoConstantType acType)
{
	param->setNamedAutoConstant(name, acType);
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedAutoConstant2(Ogre::GpuProgramParameters* param, String name, Ogre::GpuProgramParameters::AutoConstantType acType, size_t extraInfo)
{
	param->setNamedAutoConstant(name, acType, extraInfo);
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedAutoConstant3(Ogre::GpuProgramParameters* param, String name, Ogre::GpuProgramParameters::AutoConstantType acType, size_t extraInfo1, size_t extraInfo2)
{
	param->setNamedAutoConstant(name, acType, extraInfo1, extraInfo2);
}

extern "C" _AnomalousExport void GpuProgramParameters_setNamedAutoConstantReal(Ogre::GpuProgramParameters* param, String name, Ogre::GpuProgramParameters::AutoConstantType acType, float rData)
{
	param->setNamedAutoConstantReal(name, acType, rData);
}

//Shared Program Parameters
extern "C" _AnomalousExport void GpuSharedParameters_addConstantDefinition(Ogre::GpuSharedParameters* param, String name, Ogre::GpuConstantType constType, size_t arraySize)
{
	param->addConstantDefinition(name, constType, arraySize);
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant1(Ogre::GpuSharedParameters* param, String name, Ogre::Real val)
{
	param->setNamedConstant(name, val);
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant2(Ogre::GpuSharedParameters* param, String name, int val)
{
	param->setNamedConstant(name, val);
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant3(Ogre::GpuSharedParameters* param, String name, Quaternion vec)
{
	param->setNamedConstant(name, vec.toOgre());
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant4(Ogre::GpuSharedParameters* param, String name, Vector3 vec)
{
	param->setNamedConstant(name, vec.toOgre());
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant5(Ogre::GpuSharedParameters* param, String name, Vector2 vec)
{
	param->setNamedConstant(name, vec.toOgre());
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant6(Ogre::GpuSharedParameters* param, String name, Color colour)
{
	param->setNamedConstant(name, colour.toOgre());
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant7(Ogre::GpuSharedParameters* param, String name, int *val, size_t count)
{
	param->setNamedConstant(name, val, count);
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant8(Ogre::GpuSharedParameters* param, String name, float *val, size_t count)
{
	param->setNamedConstant(name, val, count);
}

extern "C" _AnomalousExport void GpuSharedParameters_setNamedConstant9(Ogre::GpuSharedParameters* param, String name, double *val, size_t count)
{
	param->setNamedConstant(name, val, count);
}