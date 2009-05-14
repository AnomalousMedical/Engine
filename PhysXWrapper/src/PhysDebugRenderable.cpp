#include "StdAfx.h"
#include "..\include\PhysDebugRenderable.h"
#include "PhysDebugLine.h"
#include "PhysDebugPoint.h"
#include "PhysDebugTriangle.h"

namespace PhysXWrapper
{

PhysDebugRenderable::PhysDebugRenderable(void)
:debugRenderable(0)
{
}

PhysDebugRenderable::~PhysDebugRenderable(void)
{
	debugRenderable = 0;
}

NxU32 PhysDebugRenderable::getNbPoints()
{
	return debugRenderable->getNbPoints();
}

const PhysDebugPoint* PhysDebugRenderable::getPoints()
{
	return static_cast<const PhysDebugPoint*>(static_cast<const void*>(debugRenderable->getPoints()));
}

NxU32 PhysDebugRenderable::getNbLines()
{
	return debugRenderable->getNbLines();
}

const PhysDebugLine* PhysDebugRenderable::getLines()
{
	return static_cast<const PhysDebugLine*>(static_cast<const void*>(debugRenderable->getLines()));
}

NxU32 PhysDebugRenderable::getNbTriangles()
{
	return debugRenderable->getNbTriangles();
}

const PhysDebugTriangle* PhysDebugRenderable::getTriangles()
{
	return static_cast<const PhysDebugTriangle*>(static_cast<const void*>(debugRenderable->getTriangles()));
}

void PhysDebugRenderable::setDebugRenderable(const NxDebugRenderable* debugRenderable)
{
	this->debugRenderable = debugRenderable;
}

}