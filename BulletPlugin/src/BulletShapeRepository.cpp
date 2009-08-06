#include "StdAfx.h"
#include "..\include\BulletShapeRepository.h"
#include "BulletShapeCollection.h"

namespace BulletPlugin
{

BulletShapeRepository::BulletShapeRepository(void)
{

}

BulletShapeRepository::~BulletShapeRepository()
{

}

bool BulletShapeRepository::addCollection(BulletShapeCollection^ collection)
{
    if (shapes.ContainsKey(collection->Name))
    {
		Logging::Log::Default->sendMessage("Attempted to add a shape with a duplicate name " + collection->Name + " ignoring the new shape.", Logging::LogLevel::Error, "Physics");
        return false;
    }
    else
    {
        shapes.Add(collection->Name, collection);
        collection->SourceLocation = CurrentLoadingLocation;
        CurrentLoadingLocation->addShape(collection->Name);
        return true;
    }
}

void BulletShapeRepository::removeCollection(String^ name)
{
    if (shapes.ContainsKey(name))
    {
        BulletShapeCollection^ collection = shapes[name];
        shapes.Remove(name);
        delete collection;
    }
    else
    {
		Logging::Log::Default->sendMessage("Attempted to remove a shape " + name + " that does not exist.  No changes made.", Logging::LogLevel::Error, "Physics");
    }
}

BulletShapeCollection^ BulletShapeRepository::getCollection(String^ name)
{
    if (name != nullptr && shapes.ContainsKey(name))
    {
        return shapes[name];
    }
    else
    {
		Logging::Log::Default->sendMessage("Could not find a shape named " + name + ".", Logging::LogLevel::Error, "Physics");
    }
    return nullptr;
}

bool BulletShapeRepository::containsValidCollection(String^ name)
{
    return name != nullptr && shapes.ContainsKey(name) && shapes[name]->Count > 0;
}

void BulletShapeRepository::addConvexMesh(String^ name, ConvexMesh^ mesh)
{

}

void BulletShapeRepository::destroyConvexMesh(String^ name)
{

}

void BulletShapeRepository::addTriangleMesh(String^ name, TriangleMesh^ mesh)
{

}

void BulletShapeRepository::destroyTriangleMesh(String^ name)
{

}

void BulletShapeRepository::addMaterial(String^ name, ShapeMaterial^ materialDesc)
{

}

bool BulletShapeRepository::hasMaterial(String^ name)
{
	throw gcnew NotImplementedException();
}

ShapeMaterial^ BulletShapeRepository::getMaterial(String^ name)
{
	throw gcnew NotImplementedException();
}

void BulletShapeRepository::destroyMaterial(String^ name)
{

}

void BulletShapeRepository::addSoftBodyMesh(SoftBodyMesh^ softBodyMesh)
{

}

SoftBodyMesh^ BulletShapeRepository::getSoftBodyMesh(String^ name)
{
	throw gcnew NotImplementedException();
}

void BulletShapeRepository::destroySoftBodyMesh(String^ name)
{

}

}