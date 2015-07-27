#include "Stdafx.h"

extern "C" _AnomalousExport void GpuProgram_setSourceFile(Ogre::GpuProgram* gpuProgram, String filename)
{
	return gpuProgram->setSourceFile(filename);
}

extern "C" _AnomalousExport void GpuProgram_setSource(Ogre::GpuProgram* gpuProgram, String source)
{
	return gpuProgram->setSource(source);
}

extern "C" _AnomalousExport void GpuProgram_getSyntaxCode(Ogre::GpuProgram* gpuProgram, StringRetrieverCallback srCallback, void* handle)
{
	srCallback(gpuProgram->getSyntaxCode().c_str(), handle);
}

extern "C" _AnomalousExport void GpuProgram_setSyntaxCode(Ogre::GpuProgram* gpuProgram, String syntax)
{
	return gpuProgram->setSyntaxCode(syntax);
}

extern "C" _AnomalousExport void GpuProgram_getSourceFile(Ogre::GpuProgram* gpuProgram, StringRetrieverCallback srCallback, void* handle)
{
	srCallback(gpuProgram->getSourceFile().c_str(), handle);
}

extern "C" _AnomalousExport void GpuProgram_getSource(Ogre::GpuProgram* gpuProgram, StringRetrieverCallback srCallback, void* handle)
{
	srCallback(gpuProgram->getSource().c_str(), handle);
}

extern "C" _AnomalousExport void GpuProgram_setType(Ogre::GpuProgram* gpuProgram, Ogre::GpuProgramType t)
{
	return gpuProgram->setType(t);
}

extern "C" _AnomalousExport Ogre::GpuProgramType GpuProgram_getType(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->getType();
}

extern "C" _AnomalousExport bool GpuProgram_isSupported(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->isSupported();
}

extern "C" _AnomalousExport void GpuProgram_setSkeletalAnimationIncluded(Ogre::GpuProgram* gpuProgram, bool included)
{
	return gpuProgram->setSkeletalAnimationIncluded(included);
}

extern "C" _AnomalousExport bool GpuProgram_isSkeletalAnimationIncluded(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->isSkeletalAnimationIncluded();
}

extern "C" _AnomalousExport void GpuProgram_setMorphAnimationIncluded(Ogre::GpuProgram* gpuProgram, bool included)
{
	return gpuProgram->setMorphAnimationIncluded(included);
}

extern "C" _AnomalousExport void GpuProgram_setPoseAnimationIncluded(Ogre::GpuProgram* gpuProgram, ushort poseCount)
{
	return gpuProgram->setPoseAnimationIncluded(poseCount);
}

extern "C" _AnomalousExport bool GpuProgram_isMorphAnimationIncluded(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->isMorphAnimationIncluded();
}

extern "C" _AnomalousExport bool GpuProgram_isPoseAnimationIncluded(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->isPoseAnimationIncluded();
}

extern "C" _AnomalousExport ushort GpuProgram_getNumberOfPosesIncluded(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->getNumberOfPosesIncluded();
}

extern "C" _AnomalousExport void GpuProgram_setVertexTextureFetchRequired(Ogre::GpuProgram* gpuProgram, bool r)
{
	return gpuProgram->setVertexTextureFetchRequired(r);
}

extern "C" _AnomalousExport bool GpuProgram_isVertexTextureFetchRequired(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->isVertexTextureFetchRequired();
}

extern "C" _AnomalousExport void GpuProgram_setAdjacencyInfoRequired(Ogre::GpuProgram* gpuProgram, bool r)
{
	return gpuProgram->setAdjacencyInfoRequired(r);
}

extern "C" _AnomalousExport bool GpuProgram_isAdjacencyInfoRequired(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->isAdjacencyInfoRequired();
}

extern "C" _AnomalousExport void GpuProgram_setComputeGroupDimensions(Ogre::GpuProgram* gpuProgram, Vector3 dimensions)
{
	return gpuProgram->setComputeGroupDimensions(dimensions.toOgre());
}

extern "C" _AnomalousExport Vector3 GpuProgram_getComputeGroupDimensions(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->getComputeGroupDimensions();
}

extern "C" _AnomalousExport Ogre::GpuProgramParameters* GpuProgram_getDefaultParameters(Ogre::GpuProgram* gpuProgram, ProcessWrapperObjectDelegate processWrapper)
{
	const Ogre::GpuProgramParametersSharedPtr& ptr = gpuProgram->getDefaultParameters();
	processWrapper(ptr.getPointer(), &ptr);
	return ptr.getPointer();
}

extern "C" _AnomalousExport bool GpuProgram_hasDefaultParameters(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->hasDefaultParameters();
}

extern "C" _AnomalousExport bool GpuProgram_getPassSurfaceAndLightStates(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->getPassSurfaceAndLightStates();
}

extern "C" _AnomalousExport bool GpuProgram_getPassFogStates(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->getPassFogStates();
}

extern "C" _AnomalousExport bool GpuProgram_getPassTransformStates(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->getPassTransformStates();
}

extern "C" _AnomalousExport void GpuProgram_getLanguage(Ogre::GpuProgram* gpuProgram, StringRetrieverCallback srCallback, void* handle)
{
	srCallback(gpuProgram->getLanguage().c_str(), handle);
}

extern "C" _AnomalousExport bool GpuProgram_hasCompileError(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->hasCompileError();
}

extern "C" _AnomalousExport void GpuProgram_resetCompileError(Ogre::GpuProgram* gpuProgram)
{
	return gpuProgram->resetCompileError();
}

extern "C" _AnomalousExport void GpuProgram_setParam(Ogre::GpuProgram* gpuProgram, String name, String value)
{
	gpuProgram->setParameter(name, value);
}

extern "C" _AnomalousExport void GpuProgram_getParam(Ogre::GpuProgram* gpuProgram, String name, StringRetrieverCallback srCallback, void* handle)
{
	srCallback(gpuProgram->getParameter(name).c_str(), handle);
}