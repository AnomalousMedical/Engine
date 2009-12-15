#include "stdafx.h"
#include "SoftBody.h"

#include "SoftBodyDefinition.h"
#include "BulletScene.h"

namespace BulletPlugin
{

#pragma unmanaged

btSoftBody* createElipsoid(btSoftBodyWorldInfo* worldInfo, float* center, float* radius, int res)
{
	return btSoftBodyHelpers::CreateEllipsoid(*worldInfo, btVector3(center[0], center[1], center[2]), btVector3(radius[0], radius[1], radius[2]), res);
}

#pragma managed

SoftBody::SoftBody(SoftBodyDefinition^ description, BulletScene^ scene)
:SimElement(description->Name, description->Subscription),
scene(scene)
{
	float center[3] = {0, 0, 0};
	float radius[3] = {1, 1, 1};
	softBody = createElipsoid(scene->SoftBodyWorldInfo, center, radius, 512);

	btSoftBody::Config* config = description->sbConfig;
	softBody->m_cfg.aeromodel = config->aeromodel;
	softBody->m_cfg.kVCF = config->kVCF;		
	softBody->m_cfg.kDG = config->kDG;
	softBody->m_cfg.kLF = config->kLF;
	softBody->m_cfg.kDP = config->kDP;
	softBody->m_cfg.kPR = config->kPR;
	softBody->m_cfg.kVC = config->kVC;
	softBody->m_cfg.kDF = config->kDF;
	softBody->m_cfg.kMT = config->kMT;
	softBody->m_cfg.kCHR = config->kCHR;
	softBody->m_cfg.kKHR = config->kKHR;
	softBody->m_cfg.kSHR = config->kSHR;
	softBody->m_cfg.kAHR = config->kAHR;
	softBody->m_cfg.kSRHR_CL = config->kSRHR_CL;
	softBody->m_cfg.kSKHR_CL = config->kSKHR_CL;
	softBody->m_cfg.kSSHR_CL = config->kSSHR_CL;
	softBody->m_cfg.kSR_SPLT_CL = config->kSR_SPLT_CL;
	softBody->m_cfg.kSK_SPLT_CL = config->kSK_SPLT_CL;
	softBody->m_cfg.kSS_SPLT_CL = config->kSS_SPLT_CL;
	softBody->m_cfg.maxvolume = config->maxvolume;
	softBody->m_cfg.timescale = config->timescale;
	softBody->m_cfg.viterations = config->viterations;
	softBody->m_cfg.piterations = config->piterations;
	softBody->m_cfg.diterations = config->diterations;
	softBody->m_cfg.citerations = config->citerations;
	softBody->m_cfg.collisions = config->collisions;

	softBody->m_materials[0]->m_kLST = description->DefaultMaterial->m_kLST;
	softBody->m_materials[0]->m_kAST = description->DefaultMaterial->m_kAST;
	softBody->m_materials[0]->m_kVST = description->DefaultMaterial->m_kVST;
}

SoftBody::~SoftBody(void)
{
	if(softBody != 0)
	{
		if(Owner->Enabled)
		{
			scene->DynamicsWorld->removeSoftBody(softBody);
		}
		delete softBody;
		softBody = 0;
	}
}

SimElementDefinition^ SoftBody::saveToDefinition()
{
	SoftBodyDefinition^ def = gcnew SoftBodyDefinition(this->Name);

	//Config
	btSoftBody::Config* config = def->sbConfig;
	config->aeromodel = softBody->m_cfg.aeromodel;
	config->kVCF = softBody->m_cfg.kVCF;		
	config->kDG = softBody->m_cfg.kDG;
	config->kLF = softBody->m_cfg.kLF;
	config->kDP = softBody->m_cfg.kDP;
	config->kPR = softBody->m_cfg.kPR;
	config->kVC = softBody->m_cfg.kVC;
	config->kDF = softBody->m_cfg.kDF;
	config->kMT = softBody->m_cfg.kMT;
	config->kCHR = softBody->m_cfg.kCHR;
	config->kKHR = softBody->m_cfg.kKHR;
	config->kSHR = softBody->m_cfg.kSHR;
	config->kAHR = softBody->m_cfg.kAHR;
	config->kSRHR_CL = softBody->m_cfg.kSRHR_CL;
	config->kSKHR_CL = softBody->m_cfg.kSKHR_CL;
	config->kSSHR_CL = softBody->m_cfg.kSSHR_CL;
	config->kSR_SPLT_CL = softBody->m_cfg.kSR_SPLT_CL;
	config->kSK_SPLT_CL = softBody->m_cfg.kSK_SPLT_CL;
	config->kSS_SPLT_CL = softBody->m_cfg.kSS_SPLT_CL;
	config->maxvolume = softBody->m_cfg.maxvolume;
	config->timescale = softBody->m_cfg.timescale;
	config->viterations = softBody->m_cfg.viterations;
	config->piterations = softBody->m_cfg.piterations;
	config->diterations = softBody->m_cfg.diterations;
	config->citerations = softBody->m_cfg.citerations;
	config->collisions = softBody->m_cfg.collisions;

	def->DefaultMaterial->m_kLST = softBody->m_materials[0]->m_kLST;
	def->DefaultMaterial->m_kAST = softBody->m_materials[0]->m_kAST;
	def->DefaultMaterial->m_kVST = softBody->m_materials[0]->m_kVST;

	return def;
}

void SoftBody::updatePositionImpl(Vector3% translation, Quaternion% rotation)
{

}

void SoftBody::updateTranslationImpl(Vector3% translation)
{

}

void SoftBody::updateRotationImpl(Quaternion% rotation)
{

}

void SoftBody::updateScaleImpl(Vector3% scale)
{

}

void SoftBody::setEnabled(bool enabled)
{
	if(enabled)
	{
		scene->DynamicsWorld->addSoftBody(softBody);
	}
	else
	{
		scene->DynamicsWorld->removeSoftBody(softBody);
	}
}

}