/// <file>ManualObject.cpp</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "StdAfx.h"
#include "..\include\ManualObject.h"
#include "MarshalUtils.h"
#include "MathUtils.h"
#include "ManualObjectSection.h"

#include "Ogre.h"

namespace OgreWrapper{

ManualObject::ManualObject(Ogre::ManualObject* obj)
:MovableObject(obj), 
obj( obj ), 
root(new ManualObjectRoot())
{
	*(root.Get()) = this;
	userDefinedObj.Reset(new VoidUserDefinedObject(CAMERA_GCROOT, root.Get()));
	obj->setUserObject(userDefinedObj.Get());

}

ManualObject::~ManualObject()
{

}

Ogre::ManualObject* ManualObject::getManualObject()
{
	return obj;
}

System::String^ ManualObject::getName()
{
	return MarshalUtils::convertString(obj->getName());
}

void ManualObject::clear()
{
	obj->clear();
	sections.clearObjects();
}

void ManualObject::estimateVertexCount(unsigned int count)
{
	obj->estimateVertexCount(count);
}

void ManualObject::estimateIndexCount(unsigned int count)
{
	obj->estimateIndexCount(count);
}

void ManualObject::begin(System::String^ materialName, OperationType opType)
{
	obj->begin(MarshalUtils::convertString(materialName), (Ogre::RenderOperation::OperationType)opType);
}

void ManualObject::setDynamic(bool dyn)
{
	obj->setDynamic(dyn);
}

bool ManualObject::getDynamic()
{
	return obj->getDynamic();
}

void ManualObject::beginUpdate(unsigned int sectionIndex)
{
	obj->beginUpdate(sectionIndex);
}

void ManualObject::position(Engine::Vector3% pos)
{
	Ogre::Vector3 ogreVec;
	MathUtils::copyVector3(pos, ogreVec);
	obj->position(ogreVec);
}

void ManualObject::position(float x, float y, float z)
{
	obj->position(x, y, z);
}

void ManualObject::normal(Engine::Vector3% normal)
{
	Ogre::Vector3 ogreVec;
	MathUtils::copyVector3(normal, ogreVec);
	obj->normal(ogreVec);
}

void ManualObject::normal(float x, float y, float z)
{
	obj->normal(x, y, z);
}

void ManualObject::textureCoord(float u)
{
	obj->textureCoord(u);
}

void ManualObject::textureCoord(float u, float v)
{
	obj->textureCoord(u, v);
}

void ManualObject::textureCoord(float u, float v, float w)
{
	obj->textureCoord(u, v, w);
}

void ManualObject::textureCoord(float x, float y, float z, float w)
{
	obj->textureCoord(x, y, z, w);
}

void ManualObject::textureCoord(Engine::Vector3% uvw)
{
	Ogre::Vector3 ogreVec;
	MathUtils::copyVector3(uvw, ogreVec);
	obj->textureCoord(ogreVec);
}

void ManualObject::color(float r, float g, float b, float a)
{
	obj->colour(r, g, b, a);
}

void ManualObject::index(unsigned int idx)
{
	obj->index(idx);
}

void ManualObject::triangle(unsigned int i1, unsigned int i2, unsigned int i3)
{
	obj->triangle(i1, i2, i3);
}

void ManualObject::quad(unsigned int i1, unsigned int i2, unsigned int i3, unsigned int i4)
{
	obj->quad(i1, i2, i3, i4);
}

ManualObjectSection^ ManualObject::end()
{
	return sections.getObject(obj->end());
}

void ManualObject::setMaterialName(unsigned int subindex, System::String^ name)
{
	obj->setMaterialName(subindex, MarshalUtils::convertString(name));
}

//convert to mesh

void ManualObject::setUseIdentityProjection(bool useIdentityProjection)
{
	obj->setUseIdentityProjection(useIdentityProjection);
}

bool ManualObject::getUseIdentityProjection()
{
	return obj->getUseIdentityProjection();
}

void ManualObject::setUseIdentityView(bool useIdentityView)
{
	obj->setUseIdentityView(useIdentityView);
}

bool ManualObject::getUseIdentityView()
{
	return obj->getUseIdentityView();
}

//set bounding box

ManualObjectSection^ ManualObject::getSection(unsigned int index)
{
	return sections.getObject(obj->getSection(index));
}

unsigned int ManualObject::getNumSections()
{
	return obj->getNumSections();
}

void ManualObject::setKeepDeclarationOrder(bool keepOrder)
{
	obj->setKeepDeclarationOrder(keepOrder);
}

bool ManualObject::getKeepDeclarationOrder()
{
	return obj->getKeepDeclarationOrder();
}

float ManualObject::getBoundingRadius()
{
	return obj->getBoundingRadius();
}

}