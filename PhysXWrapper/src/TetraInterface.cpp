#include "StdAfx.h"
#include "..\include\TetraInterface.h"
#include "NxTetra.h"
#include "TetraMesh.h"
#include "stdlib.h"
#include "windows.h"

namespace Physics
{

TetraInterface::TetraInterface(NxTetraInterface* tetraInterface)
:tetraInterface(tetraInterface)
{
}

TetraInterface^ TetraInterface::getTetraInterface()
{
	if(instance == nullptr)
	{
		NxTetraInterface *ret = 0;
		const char *dmodule = "NxTetra.dll";
		wchar_t dname[512];
		mbstowcs(dname,dmodule,512);
		HMODULE module = LoadLibrary( dname );
		if ( module )
		{
			if ( module )
			{
				void *proc = GetProcAddress(module,"getTetraInterface");
				if ( proc )
				{
					typedef NxTetraInterface * (__cdecl * NX_GetToolkit)();
					ret = ((NX_GetToolkit)proc)();
				}
			}
		}
		if(ret != 0)
		{
			Logging::Log::Default->sendMessage("Loaded NxTetra.dll.  Tetra mesh generation functions are avaliable.", Logging::LogLevel::Info, "Physics");
			instance = gcnew TetraInterface(ret);
		}
		else
		{
			Logging::Log::Default->sendMessage("Could not load NxTetra.dll.  Tetra Mesh generation functions not avaliable.", Logging::LogLevel::Error, "Physics");
		}
	}
	return instance;
}

bool TetraInterface::createTetraMesh(TetraMesh^ mesh, unsigned int vcount, const float *vertices, unsigned int tcount, const unsigned int *indices, bool isTetra)
{
	return tetraInterface->createTetraMesh(*mesh->getNxTetraMesh(), vcount, vertices, tcount, indices, isTetra);
}

void TetraInterface::setSubdivisionLevel(unsigned int subdivisionLevel)
{
	return tetraInterface->setSubdivisionLevel(subdivisionLevel);
}

unsigned int TetraInterface::createIsoSurface(TetraMesh^ input, TetraMesh^ output, bool isoSingle)
{
	return tetraInterface->createIsoSurface(*input->getNxTetraMesh(), *output->getNxTetraMesh(), isoSingle);
}

unsigned int TetraInterface::simplifySurface(float factor, TetraMesh^ input, TetraMesh^ output)
{
	return tetraInterface->simplifySurface(factor, *input->getNxTetraMesh(), *output->getNxTetraMesh());
}

unsigned int TetraInterface::createTetraMesh(TetraMesh^ inputMesh, TetraMesh^ outputMesh)
{
	return tetraInterface->createTetraMesh(*inputMesh->getNxTetraMesh(), *outputMesh->getNxTetraMesh());
}

bool TetraInterface::releaseTetraMesh(TetraMesh^ mesh)
{
	return tetraInterface->releaseTetraMesh(*mesh->getNxTetraMesh());
}

}