#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport Ogre::RenderQueue* SceneManager_getRenderQueue(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getRenderQueue();
}

extern "C" _AnomalousExport const char* SceneManager_getName(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getName().c_str();
}

extern "C" _AnomalousExport Ogre::Camera* SceneManager_createCamera(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->createCamera(name);
}

extern "C" _AnomalousExport Ogre::Camera* SceneManager_getCamera(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->getCamera(name);
}

extern "C" _AnomalousExport bool SceneManager_hasCamera(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->hasCamera(name);
}

extern "C" _AnomalousExport void SceneManager_destroyCamera(Ogre::SceneManager* sceneManager, Ogre::Camera* camera)
{
	sceneManager->destroyCamera(camera);
}

extern "C" _AnomalousExport Ogre::Light* SceneManager_createLight(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->createLight(name);
}

extern "C" _AnomalousExport Ogre::Light* SceneManager_getLight(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->getLight(name);
}

extern "C" _AnomalousExport bool SceneManager_hasLight(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->hasLight(name);
}

extern "C" _AnomalousExport void SceneManager_destroyLight(Ogre::SceneManager* sceneManager, Ogre::Light* light)
{
	sceneManager->destroyLight(light);
}

extern "C" _AnomalousExport Ogre::SceneNode* SceneManager_createSceneNode(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->createSceneNode(name);
}

extern "C" _AnomalousExport Ogre::SceneNode* SceneManager_getRootSceneNode(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getRootSceneNode();
}

extern "C" _AnomalousExport Ogre::SceneNode* SceneManager_getSceneNode(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->getSceneNode(name);
}

extern "C" _AnomalousExport bool SceneManager_hasSceneNode(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->hasSceneNode(name);
}

extern "C" _AnomalousExport void SceneManager_destroySceneNode(Ogre::SceneManager* sceneManager, Ogre::SceneNode* node)
{
	sceneManager->destroySceneNode(node);
}

extern "C" _AnomalousExport Ogre::Entity* SceneManager_createEntity(Ogre::SceneManager* sceneManager, String entityName, String meshName)
{
	return sceneManager->createEntity(entityName, meshName);
}

extern "C" _AnomalousExport Ogre::Entity* SceneManager_getEntity(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->getEntity(name);
}

extern "C" _AnomalousExport bool SceneManager_hasEntity(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->hasEntity(name);
}

extern "C" _AnomalousExport void SceneManager_destroyEntity(Ogre::SceneManager* sceneManager, Ogre::Entity* entity)
{
	sceneManager->destroyEntity(entity);
}

extern "C" _AnomalousExport Ogre::ManualObject* SceneManager_createManualObject(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->createManualObject(name);
}

extern "C" _AnomalousExport Ogre::ManualObject* SceneManager_getManualObject(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->getManualObject(name);
}

extern "C" _AnomalousExport bool SceneManager_hasManualObject(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->hasManualObject(name);
}

extern "C" _AnomalousExport void SceneManager_destroyManualObject(Ogre::SceneManager* sceneManager, Ogre::ManualObject* obj)
{
	sceneManager->destroyManualObject(obj);
}

extern "C" _AnomalousExport Ogre::StaticGeometry* SceneManager_createStaticGeometry(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->createStaticGeometry(name);
}

extern "C" _AnomalousExport Ogre::StaticGeometry* SceneManager_getStaticGeometry(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->getStaticGeometry(name);
}

extern "C" _AnomalousExport bool SceneManager_hasStaticGeometry(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->hasStaticGeometry(name);
}

extern "C" _AnomalousExport void SceneManager_destroyStaticGeometry(Ogre::SceneManager* sceneManager, Ogre::StaticGeometry* obj)
{
	sceneManager->destroyStaticGeometry(obj);
}

extern "C" _AnomalousExport void SceneManager_setVisibilityMask(Ogre::SceneManager* sceneManager, uint mask)
{
	sceneManager->setVisibilityMask(mask);
}

extern "C" _AnomalousExport uint SceneManager_getVisibilityMask(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getVisibilityMask();
}

extern "C" _AnomalousExport void SceneManager_addSceneListener(Ogre::SceneManager* sceneManager, Ogre::SceneManager::Listener* listener)
{
	sceneManager->addListener(listener);
}

extern "C" _AnomalousExport void SceneManager_removeSceneListener(Ogre::SceneManager* sceneManager, Ogre::SceneManager::Listener* listener)
{
	sceneManager->removeListener(listener);
}

extern "C" _AnomalousExport Ogre::RaySceneQuery* SceneManager_createRaySceneQuery(Ogre::SceneManager* sceneManager, Ray3 ray, unsigned long mask)
{
	return sceneManager->createRayQuery(ray.toOgre(), mask);
}

extern "C" _AnomalousExport void SceneManager_destroyRayQuery(Ogre::SceneManager* sceneManager, Ogre::RaySceneQuery* query)
{
	sceneManager->destroyQuery(query);
}

extern "C" _AnomalousExport void SceneManager_setDisplaySceneNodes(Ogre::SceneManager* sceneManager, bool display)
{
	sceneManager->setDisplaySceneNodes(display);
}

extern "C" _AnomalousExport bool SceneManager_getDisplaySceneNodes(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getDisplaySceneNodes();
}

extern "C" _AnomalousExport void SceneManager_showBoundingBoxes(Ogre::SceneManager* sceneManager, bool bShow)
{
	sceneManager->showBoundingBoxes(bShow);
}

extern "C" _AnomalousExport bool SceneManager_getShowBoundingBoxes(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShowBoundingBoxes();
}

extern "C" _AnomalousExport void SceneManager_setShadowTechnique(Ogre::SceneManager* sceneManager, Ogre::ShadowTechnique technique)
{
	sceneManager->setShadowTechnique(technique);
}

extern "C" _AnomalousExport Ogre::ShadowTechnique SceneManager_getShadowTechnique(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowTechnique();
}

extern "C" _AnomalousExport void SceneManager_setShowDebugShadows(Ogre::SceneManager* sceneManager, bool debug)
{
	sceneManager->setShowDebugShadows(debug);
}

extern "C" _AnomalousExport bool SceneManager_getShowDebugShadows(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShowDebugShadows();
}

extern "C" _AnomalousExport void SceneManager_setShadowColor(Ogre::SceneManager* sceneManager, Color color)
{
	sceneManager->setShadowColour(color.toOgre());
}

extern "C" _AnomalousExport Color SceneManager_getShadowColor(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowColour();
}

extern "C" _AnomalousExport void SceneManager_setShadowDirectionalLightExtrusionDistance(Ogre::SceneManager* sceneManager, float dist)
{
	sceneManager->setShadowDirectionalLightExtrusionDistance(dist);
}

extern "C" _AnomalousExport float SceneManager_getShadowDirectionalLightExtrusionDistance(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowDirectionalLightExtrusionDistance();
}

extern "C" _AnomalousExport void SceneManager_setShadowFarDistance(Ogre::SceneManager* sceneManager, float distance)
{
	sceneManager->setShadowFarDistance(distance);
}

extern "C" _AnomalousExport float SceneManager_getShadowFarDistance(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowFarDistance();
}

extern "C" _AnomalousExport float SceneManager_getShadowFarDistanceSquared(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowFarDistanceSquared();
}

extern "C" _AnomalousExport void SceneManager_setShadowIndexBufferSize(Ogre::SceneManager* sceneManager, int size)
{
	sceneManager->setShadowIndexBufferSize(size);
}

extern "C" _AnomalousExport int SceneManager_getShadowIndexBufferSize(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowIndexBufferSize();
}

extern "C" _AnomalousExport void SceneManager_setShadowTextureSize(Ogre::SceneManager* sceneManager, ushort size)
{
	sceneManager->setShadowTextureSize(size);
}

extern "C" _AnomalousExport void SceneManager_setShadowTextureConfig(Ogre::SceneManager* sceneManager, int shadowIndex, ushort width, ushort height, Ogre::PixelFormat format)
{
	sceneManager->setShadowTextureConfig(shadowIndex, width, height, format);
}

extern "C" _AnomalousExport void SceneManager_setShadowTexturePixelFormat(Ogre::SceneManager* sceneManager, Ogre::PixelFormat fmt)
{
	sceneManager->setShadowTexturePixelFormat(fmt);
}

extern "C" _AnomalousExport void SceneManager_setShadowTextureCount(Ogre::SceneManager* sceneManager, int count)
{
	sceneManager->setShadowTextureCount(count);
}

extern "C" _AnomalousExport int SceneManager_getShadowTextureCount(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowTextureCount();
}

extern "C" _AnomalousExport void SceneManager_setShadowTextureCountPerLightType(Ogre::SceneManager* sceneManager, Ogre::Light::LightTypes type, int count)
{
	sceneManager->setShadowTextureCountPerLightType(type, count);
}

extern "C" _AnomalousExport int SceneManager_getShadowTextureCountPerLightType(Ogre::SceneManager* sceneManager, Ogre::Light::LightTypes type)
{
	return sceneManager->getShadowTextureCountPerLightType(type);
}

extern "C" _AnomalousExport void SceneManager_setShadowTextureSettings(Ogre::SceneManager* sceneManager, ushort size, ushort count, Ogre::PixelFormat fmt)
{
	sceneManager->setShadowTextureSettings(size, count, fmt);
}

extern "C" _AnomalousExport void SceneManager_setShadowDirLightTextureOffset(Ogre::SceneManager* sceneManager, float offset)
{
	sceneManager->setShadowDirLightTextureOffset(offset);
}

extern "C" _AnomalousExport float SceneManager_getShadowDirLightTextureOffset(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowDirLightTextureOffset();
}

extern "C" _AnomalousExport void SceneManager_setShadowTextureFadeStart(Ogre::SceneManager* sceneManager, float fadeStart)
{
	sceneManager->setShadowTextureFadeStart(fadeStart);
}

extern "C" _AnomalousExport void SceneManager_setShadowTextureFadeEnd(Ogre::SceneManager* sceneManager, float fadeEnd)
{
	sceneManager->setShadowTextureFadeEnd(fadeEnd);
}

extern "C" _AnomalousExport void SceneManager_setShadowTextureSelfShadow(Ogre::SceneManager* sceneManager, bool selfShadow)
{
	sceneManager->setShadowTextureSelfShadow(selfShadow);
}

extern "C" _AnomalousExport bool SceneManager_getShadowTextureSelfShadow(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowTextureSelfShadow();
}

extern "C" _AnomalousExport void SceneManager_setShadowTextureCasterMaterial(Ogre::SceneManager* sceneManager, String name)
{
	sceneManager->setShadowTextureCasterMaterial(name);
}

extern "C" _AnomalousExport void SceneManager_setShadowTextureReceiverMaterial(Ogre::SceneManager* sceneManager, String name)
{
	sceneManager->setShadowTextureReceiverMaterial(name);
}

extern "C" _AnomalousExport void SceneManager_setShadowCasterRenderBackFaces(Ogre::SceneManager* sceneManager, bool bf)
{
	sceneManager->setShadowCasterRenderBackFaces(bf);
}

extern "C" _AnomalousExport bool SceneManager_getShadowCasterRenderBackFaces(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowCasterRenderBackFaces();
}

extern "C" _AnomalousExport void SceneManager_setShadowUseInfiniteFarPlane(Ogre::SceneManager* sceneManager, bool enable)
{
	sceneManager->setShadowUseInfiniteFarPlane(enable);
}

extern "C" _AnomalousExport bool SceneManager_isShadowTechniqueStencilBased(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueStencilBased();
}

extern "C" _AnomalousExport bool SceneManager_isShadowTechniqueTextureBased(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueTextureBased();
}

extern "C" _AnomalousExport bool SceneManager_isShadowTechniqueModulative(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueModulative();
}

extern "C" _AnomalousExport bool SceneManager_isShadowTechniqueAdditive(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueAdditive();
}

extern "C" _AnomalousExport bool SceneManager_isShadowTechniqueIntegrated(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueIntegrated();
}

extern "C" _AnomalousExport bool SceneManager_isShadowTechniqueInUse(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueInUse();
}

extern "C" _AnomalousExport void SceneManager_setShadowUseLightClipPlanes(Ogre::SceneManager* sceneManager, bool enabled)
{
	sceneManager->setShadowUseLightClipPlanes(enabled);
}

extern "C" _AnomalousExport bool SceneManager_getShadowUseLightClipPlanes(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowUseLightClipPlanes();
}

extern "C" _AnomalousExport void SceneManager_setSkyPlane(Ogre::SceneManager* sceneManager, bool enabled, float d, Vector3 normal, String matName, float scale, float tiling, bool drawFirst, float bow)
{
	Ogre::Plane p;
	p.d = d;
	p.normal = normal.toOgre();
	sceneManager->setSkyPlane(enabled, p, matName, scale, tiling, drawFirst, bow);
}

extern "C" _AnomalousExport void SceneManager_setSkyBox(Ogre::SceneManager* sceneManager, bool enabled, String matName, float distance, bool drawFirst)
{
	sceneManager->setSkyBox(enabled, matName, distance, drawFirst);
}

extern "C" _AnomalousExport void SceneManager_setSkyDome(Ogre::SceneManager* sceneManager, bool enabled, String matName)
{
	sceneManager->setSkyDome(enabled, matName);
}

extern "C" _AnomalousExport Ogre::Viewport* SceneManager_getCurrentViewport(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getCurrentViewport();
}

#pragma warning(pop)