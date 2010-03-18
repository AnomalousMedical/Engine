#include "StdAfx.h"
#include "..\include\SoftBodyAnchor.h"
#include "SoftBodyAnchorDefinition.h"

namespace BulletPlugin
{

SoftBodyAnchor::SoftBodyAnchor(SoftBodyAnchorDefinition^ definition)
:SimElement(definition->Name, definition->Subscription),
rigidBodySimObject(definition->RigidBodySimObject),
rigidBodyElement(definition->RigidBodyElement),
softBodySimObject(definition->SoftBodySimObject),
softBodyElement(definition->SoftBodyElement),
disableCollisionBetweenNodes(definition->DisableCollisionBetweenNodes),
findClosestNode(definition->FindClosestNode),
specificNode(definition->SpecificNode)
{

}

SoftBodyAnchor::~SoftBodyAnchor(void)
{

}

SimElementDefinition^ SoftBodyAnchor::saveToDefinition()
{
	SoftBodyAnchorDefinition^ definition = gcnew SoftBodyAnchorDefinition(this->Name);
	definition->RigidBodySimObject = rigidBodySimObject;
	definition->RigidBodyElement = rigidBodyElement;
	definition->SoftBodySimObject = softBodySimObject;
	definition->SoftBodyElement = softBodyElement;
	definition->DisableCollisionBetweenNodes = disableCollisionBetweenNodes;
	definition->FindClosestNode = findClosestNode;
	definition->SpecificNode = specificNode;
	return definition;
}

void SoftBodyAnchor::updatePositionImpl(Vector3% translation, Quaternion% rotation)
{

}

void SoftBodyAnchor::updateTranslationImpl(Vector3% translation)
{

}

void SoftBodyAnchor::updateRotationImpl(Quaternion% rotation)
{

}

void SoftBodyAnchor::updateScaleImpl(Vector3% scale)
{

}

void SoftBodyAnchor::setEnabled(bool enabled)
{

}

}