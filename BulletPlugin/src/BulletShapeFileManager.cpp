#include "StdAfx.h"
#include "..\include\BulletShapeFileManager.h"
#include "BulletShapeBuilder.h"
#include "BulletShapeRepository.h"

namespace BulletPlugin
{

BulletShapeFileManager::BulletShapeFileManager(void)
:shapeRepository(gcnew BulletShapeRepository()),
shapeBuilder(gcnew BulletShapeBuilder()),
ShapeFileManager(shapeRepository, shapeBuilder)
{
	shapeBuilder->setRepository(shapeRepository);
}

void BulletShapeFileManager::loadStarted()
{

}

void BulletShapeFileManager::loadEnded()
{

}

float BulletShapeFileManager::ShapeMargin::get()
{
	return shapeBuilder->ShapeMargin;
}
void BulletShapeFileManager::ShapeMargin::set(float value)
{
	shapeBuilder->ShapeMargin = value;
}

}