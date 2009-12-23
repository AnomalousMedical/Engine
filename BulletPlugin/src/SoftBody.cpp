#include "stdafx.h"
#include "SoftBody.h"

#include "SoftBodyDefinition.h"
#include "BulletScene.h"
#include "SoftBodyProvider.h"

namespace BulletPlugin
{

#pragma unmanaged

void translate(btSoftBody* softBody, float* pos)
{
	softBody->translate(btVector3(pos[0], pos[1], pos[2]));
}

void rotate(btSoftBody* softBody, float* rot)
{
	softBody->rotate(btQuaternion(rot[0], rot[1], rot[2], rot[3]));
}

void transform(btSoftBody* softBody, float* pos, float* rot)
{
	btTransform trans;
	trans.setIdentity();
	trans.setOrigin(btVector3(pos[0], pos[1], pos[2]));
	trans.setRotation(btQuaternion(rot[0], rot[1], rot[2], rot[3]));
	softBody->transform(trans);
}

void scale(btSoftBody* softBody, float* scale)
{
	softBody->scale(btVector3(scale[0], scale[1], scale[2]));
}

#pragma managed

SoftBody::SoftBody(SoftBodyDefinition^ description, BulletScene^ scene, SoftBodyProvider^ sbProvider)
:SimElement(description->Name, description->Subscription),
scene(scene),
sbProvider(sbProvider),
generateBendingConstraints(description->GenerateBendingConstraints),
bendingConstraintDistance(description->BendingConstraintDistance)
{
	softBody = sbProvider->createSoftBody(scene);

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

	if(generateBendingConstraints)
	{
		softBody->generateBendingConstraints(bendingConstraintDistance);
	}

	softBody->getCollisionShape()->setMargin(description->CollisionMargin);

	mass = description->Mass;
	massFromFaces = description->MassFromFaces;
	softBody->setTotalMass(mass, massFromFaces);
	randomizeConstraints = description->RandomizeConstraints;
	if(randomizeConstraints)
	{
		softBody->randomizeConstraints();
	}
}

SoftBody::~SoftBody(void)
{
	if(softBody != 0)
	{
		if(Owner->Enabled)
		{
			scene->DynamicsWorld->removeSoftBody(softBody);
		}
		sbProvider->destroySoftBody(scene);
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

	def->CollisionMargin = softBody->getCollisionShape()->getMargin();

	def->Mass = mass;
	def->MassFromFaces = massFromFaces;
	def->RandomizeConstraints = randomizeConstraints;
	
	def->SoftBodyProviderName = sbProvider->Name;

	def->GenerateBendingConstraints = generateBendingConstraints;
	def->BendingConstraintDistance = bendingConstraintDistance;

	return def;
}

void SoftBody::updatePositionImpl(Vector3% translation, Quaternion% rotation)
{
	Vector3 localTrans = translation;
	Quaternion localRot = rotation;
	transform(softBody, &localTrans.x, &localRot.x);
}

void SoftBody::updateTranslationImpl(Vector3% translation)
{
	Vector3 localTrans = translation;
	translate(softBody, &localTrans.x);
}

void SoftBody::updateRotationImpl(Quaternion% rotation)
{
	Quaternion localRot = rotation;
	rotate(softBody, &localRot.x);
}

void SoftBody::updateScaleImpl(Vector3% scale)
{
	Vector3 localScale = scale;
	BulletPlugin::scale(softBody, &localScale.x);
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