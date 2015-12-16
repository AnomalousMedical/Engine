#include "StdAfx.h"
#include "../Include/ReshapeableRigidBodySection.h"
#include "../Extras/Serialize/BulletFileLoader/btBulletFile.h"

ReshapeableRigidBodySection::ReshapeableRigidBodySection(void)
	:scale(1.0f, 1.0f, 1.0f),
	shape(0)
{
	transform.setIdentity();
}

ReshapeableRigidBodySection::~ReshapeableRigidBodySection(void)
{
	
}

void ReshapeableRigidBodySection::addShapes(btCompoundShape* compoundShape)
{
	if (shape != 0)
	{
		compoundShape->addChildShape(this->transform, shape);
	}
}

void ReshapeableRigidBodySection::removeShapes(btCompoundShape* compoundShape)
{
	if (shape != 0)
	{
		compoundShape->removeChildShape(shape);
	}
}

void ReshapeableRigidBodySection::cloneAndSetShape(btCollisionShape* toClone, btCompoundShape* compoundShape)
{
	this->shape = toClone;
}

void ReshapeableRigidBodySection::moveOrigin(const Vector3& translation, const Quaternion& orientation)
{
	transform.setOrigin(translation.toBullet());
	transform.setRotation(orientation.toBullet());
}

void ReshapeableRigidBodySection::setLocalScaling(const Vector3& scale)
{
	this->scale = scale.toBullet();

	if (shape != 0)
	{
		shape->setLocalScaling(this->scale);
	}
}