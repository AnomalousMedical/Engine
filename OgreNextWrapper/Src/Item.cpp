#include "Stdafx.h"
#include "OgreItem.h"

//extern "C" _AnomalousExport Ogre::MeshPtr * getMesh(Ogre::Item* item)
//{
//    return item->getMesh();
//}

extern "C" _AnomalousExport Ogre::SubItem* Item_getSubItem(Ogre::Item * item, size_t index)
{
    return item->getSubItem(index);
}

//extern "C" _AnomalousExport SubItem* getSubItem(size_t index) const;

extern "C" _AnomalousExport size_t Item_getNumSubItems(Ogre::Item * item)
{
    return item->getNumSubItems();
}

extern "C" _AnomalousExport void Item_setDatablock(Ogre::Item * item, Ogre::HlmsDatablock* datablock)
{
    return item->setDatablock(datablock);
}

extern "C" _AnomalousExport void Item_setDatablockName(Ogre::Item * item, String datablockName)
{
    return item->setDatablock(datablockName);
}

//extern "C" _AnomalousExport Ogre::Item* Item_clone(Ogre::Item * item, String newName)
//{
//    return item->clone(newName);
//}

extern "C" _AnomalousExport void Item_setDatablockOrMaterialName(Ogre::Item * item, String name,
    String groupName)//= Ogre::ResourceGroupManager::AUTODETECT_RESOURCE_GROUP_NAME);
{
    return item->setDatablockOrMaterialName(name, groupName);
}
    
extern "C" _AnomalousExport void Item_setMaterialName(Ogre::Item * item, String name, String groupName)// = ResourceGroupManager::AUTODETECT_RESOURCE_GROUP_NAME);
{
    return item->setMaterialName(name, groupName);
}

//extern "C" _AnomalousExport void setMaterial(Ogre::Item * item, const Ogre::MaterialPtr& material)
//{
//    return item->fffff();
//}

//extern "C" _AnomalousExport String getMovableType(Ogre::Item * item)
//{
//    return item->getMovableType();
//}

extern "C" _AnomalousExport bool Item_hasSkeleton(Ogre::Item * item)
{
    return item->hasSkeleton();
}

extern "C" _AnomalousExport void Item_useSkeletonInstanceFrom(Ogre::Item * item, Ogre::Item* master)
{
    return item->useSkeletonInstanceFrom(master);
}

extern "C" _AnomalousExport void Item_stopUsingSkeletonInstanceFromMaster(Ogre::Item * item)
{
    return item->stopUsingSkeletonInstanceFromMaster();
}

extern "C" _AnomalousExport bool Item_sharesSkeletonInstance(Ogre::Item * item)
{
    return item->sharesSkeletonInstance();
}

//extern "C" _AnomalousExport bool Item_hasVertexAnimation(Ogre::Item * item)
//{
//    return item->hasVertexAnimation();
//}

extern "C" _AnomalousExport bool Item_isInitialised(Ogre::Item* item)
{
    return item->isInitialised();
}