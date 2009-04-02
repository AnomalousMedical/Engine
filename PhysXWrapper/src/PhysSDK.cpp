#include "StdAfx.h"
#include "..\include\PhysSDK.h"
#include "NxPhysics.h"
#include "PhysScene.h"
#include "PhysSceneDesc.h"
#include "MarshalUtils.h"
#include "PhysConvexMesh.h"
#include "PhysMemoryReadBuffer.h"
#include "PhysTriangleMesh.h"
#include "PhysSoftBodyMesh.h"

namespace Engine
{

namespace Physics
{

PhysSDK::PhysSDK(void)
:logger(new PhysXLogger()),
physicsSDK(0),
scenes(gcnew SceneDictionary()), 
remoteDebugger(0)
{
	physicsSDK = NxCreatePhysicsSDK( NX_PHYSICS_SDK_VERSION, NULL, logger.Get() );
	if( !physicsSDK)
	{
		Logging::Log::Default->sendMessage( "PhysX SDK not created.  Wrong SDK DLL version?", Logging::LogLevel::Error, "Physics" );
		physicsSDK->setParameter( NX_SKIN_WIDTH, 0.01f );
	}
	else
	{
		Logging::Log::Default->sendMessage( "PhysX SDK created.", Logging::LogLevel::Info, "Physics" );
		if(getHWVersion() != HWVersion::NX_HW_VERSION_NONE)
		{
			Logging::Log::Default->sendMessage("Hardware found: {0}", Logging::LogLevel::Info, "Physics", getHWVersion());
		}
	}
}

PhysSDK::~PhysSDK()
{
	if(remoteDebugger)
	{
		remoteDebugger->disconnect();
		remoteDebugger = 0;
	}
	if(physicsSDK)
	{
		physicsSDK->release();
		physicsSDK = 0;
		Logging::Log::Default->sendMessage( "PhysX SDK shut down.", Logging::LogLevel::Info, "Physics" );
	}
}

void PhysSDK::connectRemoteDebugger( String^ host ){
	//Connect the remote debugger
	Logging::Log::Default->sendMessage( "Connecting to PhysX remote debugger at " + host, Logging::LogLevel::ImportantInfo, "Physics" );
	remoteDebugger = physicsSDK->getFoundationSDK().getRemoteDebugger();
	std::string h = MarshalUtils::convertString( host );
	remoteDebugger->connect( h.c_str() );
	if( remoteDebugger->isConnected() ){
		Logging::Log::Default->sendMessage( "PhysX Remote debugger connection successful", Logging::LogLevel::ImportantInfo, "Physics" );
	}
	else{
		Logging::Log::Default->sendMessage( "PhysX Remote debugger connection failed", Logging::LogLevel::Error, "Physics" );
	}
}

void PhysSDK::disconnectRemoteDebugger()
{
	if(remoteDebugger->isConnected())
	{
		remoteDebugger->disconnect();
	}
}

PhysScene^ PhysSDK::createScene(PhysSceneDesc^ desc)
{
	PhysScene^ scene;
	if( scenes->ContainsKey(desc->Name) )
	{
		Logging::Log::Default->sendMessage("A scene named " + desc->Name + " already exists, returning existing scene.", Logging::LogLevel::Warning, "Physics");
		scene = scenes[desc->Name];
	}
	else
	{
		NxSceneDesc sceneDesc = *(desc->sceneDesc.Get());
		NxScene* nxScene = physicsSDK->createScene(sceneDesc);
		scene = gcnew PhysScene(nxScene, desc->Name);
		scenes[scene->Name] = scene;
		Logging::Log::Default->sendMessage("Created physics scene named " + scene->Name + ".", Logging::LogLevel::Info, "Physics");
	}
	return scene;
}

void PhysSDK::releaseScene(PhysScene^ scene)
{
	if(scenes->ContainsValue(scene))
	{
		physicsSDK->releaseScene(*(scene->scene));
		scenes->Remove(scene->Name);
		delete scene;
		Logging::Log::Default->sendMessage("Released physics scene named " + scene->Name + ".", Logging::LogLevel::Info, "Physics");
	}
	else
	{
		Logging::Log::Default->sendMessage("Attempted to erase an unused scene " + scene->Name + " nothing changed.", Logging::LogLevel::Error, "Physics");
	}
}

int PhysSDK::getNbScenes()
{
	return physicsSDK->getNbScenes();
}

PhysScene^ PhysSDK::getScene(String^ name)
{
	if(scenes->ContainsKey(name))
	{
		return scenes[name];
	}
	else
	{
		Logging::Log::Default->sendMessage("The scene " + name + " does not exist.", Logging::LogLevel::Error, "Physics");
		return nullptr;
	}
}

PhysConvexMesh^ PhysSDK::createConvexMesh(PhysMemoryReadBuffer^ mesh)
{
	return gcnew PhysConvexMesh(physicsSDK->createConvexMesh( *(mesh->readBuffer.Get())) );
}

void PhysSDK::releaseConvexMesh(PhysConvexMesh^ mesh)
{
	physicsSDK->releaseConvexMesh(*(mesh->convexMesh));
	delete mesh;
}

PhysTriangleMesh^ PhysSDK::createTriangleMesh(PhysMemoryReadBuffer^ mesh)
{
	return gcnew PhysTriangleMesh(physicsSDK->createTriangleMesh(*mesh->readBuffer.Get()));
}

void PhysSDK::releaseTriangleMesh(PhysTriangleMesh^ mesh)
{
	physicsSDK->releaseTriangleMesh(*mesh->triangleMesh);
	delete mesh;
}

PhysSoftBodyMesh^ PhysSDK::createSoftBodyMesh(System::String^ name, PhysMemoryReadBuffer^ mesh)
{
	return gcnew PhysSoftBodyMesh(name, physicsSDK->createSoftBodyMesh(*mesh->readBuffer.Get()));
}

void PhysSDK::releaseSoftBodyMesh(PhysSoftBodyMesh^ mesh)
{
	physicsSDK->releaseSoftBodyMesh(*mesh->softMesh);
	delete mesh;
}

HWVersion PhysSDK::getHWVersion()
{
	return (HWVersion)physicsSDK->getHWVersion();
}

bool PhysSDK::setParameter(PhysParameter paramEnum, float paramValue)
{
	return physicsSDK->setParameter((NxParameter)paramEnum, paramValue);
}

float PhysSDK::getParameter(PhysParameter paramEnum)
{
	return physicsSDK->getParameter((NxParameter)paramEnum);
}

}

}