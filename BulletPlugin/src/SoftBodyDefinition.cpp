#include "StdAfx.h"
#include "..\include\SoftBodyDefinition.h"
#include "BulletScene.h"
#include "BulletFactory.h"
#include "BulletInterface.h"
#include "SoftBody.h"

namespace BulletPlugin
{


SoftBodyDefinition::SoftBodyDefinition(String^ name)
:BulletElementDefinition(name),
editInterface(nullptr),
config(new btSoftBody::Config()),
material(new btSoftBody::Material()),
softBodyProviderName(""),
collisionMargin(0.25f),
generateBendingConstraints(false),
bendingConstraintDistance(2),
setPose(false),
setPoseVolume(true),
setPoseFrame(false)
{
	config->aeromodel		=	btSoftBody::eAeroModel::V_Point;
	config->kVCF			=	1;
	config->kDG			=	0;
	config->kLF			=	0;
	config->kDP			=	0;
	config->kPR			=	0;
	config->kVC			=	0;
	config->kDF			=	(btScalar)0.2;
	config->kMT			=	0;
	config->kCHR			=	(btScalar)1.0;
	config->kKHR			=	(btScalar)0.1;
	config->kSHR			=	(btScalar)1.0;
	config->kAHR			=	(btScalar)0.7;
	config->kSRHR_CL		=	(btScalar)0.1;
	config->kSKHR_CL		=	(btScalar)1;
	config->kSSHR_CL		=	(btScalar)0.5;
	config->kSR_SPLT_CL	=	(btScalar)0.5;
	config->kSK_SPLT_CL	=	(btScalar)0.5;
	config->kSS_SPLT_CL	=	(btScalar)0.5;
	config->maxvolume		=	(btScalar)1;
	config->timescale		=	1;
	config->viterations	=	0;
	config->piterations	=	1;	
	config->diterations	=	0;
	config->citerations	=	4;
	config->collisions	=	btSoftBody::fCollision::Default;

	material->m_kLST = 1.0f;
	material->m_kAST = 1.0f;
	material->m_kVST = 1.0f;
	material->m_flags = btSoftBody::fMaterial::Default;

	mass = 10.0f;
	massFromFaces = true;
	randomizeConstraints = true;
}

SoftBodyDefinition::~SoftBodyDefinition(void)
{
}

void SoftBodyDefinition::registerScene(SimSubScene^ subscene, SimObjectBase^ instance)
{
	if (subscene->hasSimElementManagerType(BulletScene::typeid))
    {
        BulletScene^ sceneManager = subscene->getSimElementManager<BulletScene^>();
        sceneManager->getBulletFactory()->addSoftBody(this, instance);
    }
    else
    {
		Logging::Log::Default->sendMessage("Cannot add SoftBodyDefinition {0} to SimSubScene {1} because it does not contain a BulletSceneManager.", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, subscene->Name);
    }
}

EditInterface^ SoftBodyDefinition::getEditInterface()
{
	if(editInterface == nullptr)
	{
		editInterface = ReflectedEditInterface::createEditInterface(this, memberScanner, this->Name + " - Soft Body", nullptr);
	}
	return editInterface;
}

void SoftBodyDefinition::createProduct(SimObjectBase^ instance, BulletScene^ scene)
{
	SoftBodyProvider^ sbProvider = (SoftBodyProvider^)instance->getElement(softBodyProviderName);
	if(sbProvider != nullptr)
	{
		SoftBody^ softBody = gcnew SoftBody(this, scene, sbProvider);
		softBody->setInitialPosition(instance->Translation, instance->Rotation);
		instance->addElement(softBody);
	}
	else
	{
		Logging::Log::Default->sendMessage("Cannot create Soft Body {0} because the soft body provider {1} cannot be found", Logging::LogLevel::Warning, BulletInterface::PluginName, Name, softBodyProviderName);
	}
}

void SoftBodyDefinition::createStaticProduct(SimObjectBase^ instance, BulletScene^ scene)
{

}

//Saving
SoftBodyDefinition::SoftBodyDefinition(LoadInfo^ info)
:BulletElementDefinition(info),
config(new btSoftBody::Config()),
material(new btSoftBody::Material())
{
	//Config
	AeroModel = info->GetValue<SoftBodyAeroModel>("aeromodel");
	config->kVCF = info->GetFloat("kVCF");
	config->kDG = info->GetFloat("kDG");
	config->kLF = info->GetFloat("kLF");
	config->kDP = info->GetFloat("kDP");
	config->kPR = info->GetFloat("kPR");
	config->kVC = info->GetFloat("kVC");
	config->kDF = info->GetFloat("kDF");
	config->kMT = info->GetFloat("kMT");
	config->kCHR = info->GetFloat("kCHR");
	config->kKHR = info->GetFloat("kKHR");
	config->kSHR = info->GetFloat("kSHR");
	config->kAHR = info->GetFloat("kAHR");
	config->kSRHR_CL = info->GetFloat("kSRHR_CL");
	config->kSKHR_CL = info->GetFloat("kSKHR_CL");
	config->kSSHR_CL = info->GetFloat("kSSHR_CL");
	config->kSR_SPLT_CL = info->GetFloat("kSR_SPLT_CL");
	config->kSK_SPLT_CL = info->GetFloat("kSK_SPLT_CL");
	config->kSS_SPLT_CL = info->GetFloat("kSS_SPLT_CL");
	config->maxvolume = info->GetFloat("maxvolume");
	config->timescale = info->GetFloat("timescale");
	config->viterations = info->GetInt32("viterations");
	config->piterations = info->GetInt32("piterations");
	config->diterations = info->GetInt32("diterations");
	config->citerations = info->GetInt32("citerations");
	Collisions = info->GetValue<SoftBodyCollision>("collisions");

	//Default material
	material->m_kLST = info->GetFloat("kLST");
	material->m_kAST = info->GetFloat("kAST");
	material->m_kVST = info->GetFloat("kVST");
	material->m_flags = btSoftBody::fMaterial::Default;

	//Mass properties
	mass = info->GetFloat("mass");
	massFromFaces = info->GetBoolean("massFromFaces");
	randomizeConstraints = info->GetBoolean("randomizeConstraints");

	collisionMargin = info->GetFloat("collisionMargin");
	softBodyProviderName = info->GetString("softBodyProviderName");

	generateBendingConstraints = info->GetBoolean("generateBendingConstraints");
	bendingConstraintDistance = info->GetInt32("bendingConstraintDistance");

	//Pose
	setPose = info->GetBoolean("setPose");
	setPoseVolume = info->GetBoolean("setPoseVolume");
	setPoseFrame = info->GetBoolean("setPoseFrame");
}

void SoftBodyDefinition::getInfo(SaveInfo^ info)
{
	BulletElementDefinition::getInfo(info);

	//Config
	info->AddValue("aeromodel", config->aeromodel);
	info->AddValue("kVCF", config->kVCF);
	info->AddValue("kDG", config->kDG);
	info->AddValue("kLF", config->kLF);
	info->AddValue("kDP", config->kDP);
	info->AddValue("kPR", config->kPR);
	info->AddValue("kVC", config->kVC);
	info->AddValue("kDF", config->kDF);
	info->AddValue("kMT", config->kMT);
	info->AddValue("kCHR", config->kCHR);
	info->AddValue("kKHR", config->kKHR);
	info->AddValue("kSHR", config->kSHR);
	info->AddValue("kAHR", config->kAHR);
	info->AddValue("kSRHR_CL", config->kSRHR_CL);
	info->AddValue("kSKHR_CL", config->kSKHR_CL);
	info->AddValue("kSSHR_CL", config->kSSHR_CL);
	info->AddValue("kSR_SPLT_CL", config->kSR_SPLT_CL);
	info->AddValue("kSK_SPLT_CL", config->kSK_SPLT_CL);
	info->AddValue("kSS_SPLT_CL", config->kSS_SPLT_CL);
	info->AddValue("maxvolume", config->maxvolume);
	info->AddValue("timescale", config->timescale);
	info->AddValue("viterations", config->viterations);
	info->AddValue("piterations", config->piterations);
	info->AddValue("diterations", config->diterations);
	info->AddValue("citerations", config->citerations);
	info->AddValue("collisions", config->collisions);

	//Default material
	info->AddValue("kLST", material->m_kLST);
	info->AddValue("kAST", material->m_kAST);
	info->AddValue("kVST", material->m_kVST);

	//Mass properties
	info->AddValue("mass", mass);
	info->AddValue("massFromFaces", massFromFaces);
	info->AddValue("randomizeConstraints", randomizeConstraints);

	info->AddValue("collisionMargin", collisionMargin);
	info->AddValue("softBodyProviderName", softBodyProviderName);

	//Bending constraints
	info->AddValue("generateBendingConstraints", generateBendingConstraints);
	info->AddValue("bendingConstraintDistance", bendingConstraintDistance);

	//Pose
	info->AddValue("setPose", setPose);
	info->AddValue("setPoseVolume", setPoseVolume);
	info->AddValue("setPoseFrame", setPoseFrame);
}
//End saving

}