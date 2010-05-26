#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" __declspec(dllexport) Ogre::RenderQueue* SceneManager_getRenderQueue(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getRenderQueue();
}

extern "C" __declspec(dllexport) const char* SceneManager_getName(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getName().c_str();
}

extern "C" __declspec(dllexport) Ogre::Camera* SceneManager_createCamera(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->createCamera(name);
}

extern "C" __declspec(dllexport) Ogre::Camera* SceneManager_getCamera(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->getCamera(name);
}

extern "C" __declspec(dllexport) bool SceneManager_hasCamera(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->hasCamera(name);
}

extern "C" __declspec(dllexport) void SceneManager_destroyCamera(Ogre::SceneManager* sceneManager, Ogre::Camera* camera)
{
	sceneManager->destroyCamera(camera);
}

extern "C" __declspec(dllexport) Ogre::Light* SceneManager_createLight(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->createLight(name);
}

extern "C" __declspec(dllexport) Ogre::Light* SceneManager_getLight(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->getLight(name);
}

extern "C" __declspec(dllexport) bool SceneManager_hasLight(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->hasLight(name);
}

extern "C" __declspec(dllexport) void SceneManager_destroyLight(Ogre::SceneManager* sceneManager, Ogre::Light* light)
{
	sceneManager->destroyLight(light);
}

extern "C" __declspec(dllexport) Ogre::SceneNode* SceneManager_createSceneNode(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->createSceneNode(name);
}

extern "C" __declspec(dllexport) Ogre::SceneNode* SceneManager_getRootSceneNode(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getRootSceneNode();
}

extern "C" __declspec(dllexport) Ogre::SceneNode* SceneManager_getSceneNode(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->getSceneNode(name);
}

extern "C" __declspec(dllexport) bool SceneManager_hasSceneNode(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->hasSceneNode(name);
}

extern "C" __declspec(dllexport) void SceneManager_destroySceneNode(Ogre::SceneManager* sceneManager, Ogre::SceneNode* node)
{
	sceneManager->destroySceneNode(node);
}

extern "C" __declspec(dllexport) Ogre::Entity* SceneManager_createEntity(Ogre::SceneManager* sceneManager, String entityName, String meshName)
{
	return sceneManager->createEntity(entityName, meshName);
}

extern "C" __declspec(dllexport) Ogre::Entity* SceneManager_getEntity(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->getEntity(name);
}

extern "C" __declspec(dllexport) bool SceneManager_hasEntity(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->hasEntity(name);
}

extern "C" __declspec(dllexport) void SceneManager_destroyEntity(Ogre::SceneManager* sceneManager, Ogre::Entity* entity)
{
	sceneManager->destroyEntity(entity);
}

extern "C" __declspec(dllexport) Ogre::ManualObject* SceneManager_createManualObject(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->createManualObject(name);
}

extern "C" __declspec(dllexport) Ogre::ManualObject* SceneManager_getManualObject(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->getManualObject(name);
}

extern "C" __declspec(dllexport) bool SceneManager_hasManualObject(Ogre::SceneManager* sceneManager, String name)
{
	return sceneManager->hasManualObject(name);
}

extern "C" __declspec(dllexport) void SceneManager_destroyManualObject(Ogre::SceneManager* sceneManager, Ogre::ManualObject* obj)
{
	sceneManager->destroyManualObject(obj);
}

extern "C" __declspec(dllexport) void SceneManager_setVisibilityMask(Ogre::SceneManager* sceneManager, uint mask)
{
	sceneManager->setVisibilityMask(mask);
}

extern "C" __declspec(dllexport) uint SceneManager_getVisibilityMask(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getVisibilityMask();
}

extern "C" __declspec(dllexport) void SceneManager_addSceneListener(Ogre::SceneManager* sceneManager, Ogre::SceneManager::Listener* listener)
{
	sceneManager->addListener(listener);
}

extern "C" __declspec(dllexport) void SceneManager_removeSceneListener(Ogre::SceneManager* sceneManager, Ogre::SceneManager::Listener* listener)
{
	sceneManager->removeListener(listener);
}

extern "C" __declspec(dllexport) Ogre::RaySceneQuery* SceneManager_createRaySceneQuery(Ogre::SceneManager* sceneManager, Ray3 ray, unsigned long mask)
{
	return sceneManager->createRayQuery(ray.toOgre(), mask);
}

extern "C" __declspec(dllexport) void SceneManager_destroyRayQuery(Ogre::SceneManager* sceneManager, Ogre::RaySceneQuery* query)
{
	sceneManager->destroyQuery(query);
}

extern "C" __declspec(dllexport) void SceneManager_setDisplaySceneNodes(Ogre::SceneManager* sceneManager, bool display)
{
	sceneManager->setDisplaySceneNodes(display);
}

extern "C" __declspec(dllexport) bool SceneManager_getDisplaySceneNodes(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getDisplaySceneNodes();
}

extern "C" __declspec(dllexport) void SceneManager_showBoundingBoxes(Ogre::SceneManager* sceneManager, bool bShow)
{
	sceneManager->showBoundingBoxes(bShow);
}

extern "C" __declspec(dllexport) bool SceneManager_getShowBoundingBoxes(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShowBoundingBoxes();
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTechnique(Ogre::SceneManager* sceneManager, Ogre::ShadowTechnique technique)
{
	sceneManager->setShadowTechnique(technique);
}

extern "C" __declspec(dllexport) Ogre::ShadowTechnique SceneManager_getShadowTechnique(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowTechnique();
}

extern "C" __declspec(dllexport) void SceneManager_setShowDebugShadows(Ogre::SceneManager* sceneManager, bool debug)
{
	sceneManager->setShowDebugShadows(debug);
}

extern "C" __declspec(dllexport) bool SceneManager_getShowDebugShadows(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShowDebugShadows();
}

extern "C" __declspec(dllexport) void SceneManager_setShadowColor(Ogre::SceneManager* sceneManager, Color color)
{
	sceneManager->setShadowColour(color.toOgre());
}

extern "C" __declspec(dllexport) Color SceneManager_getShadowColor(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowColour();
}

extern "C" __declspec(dllexport) void SceneManager_setShadowDirectionalLightExtrusionDistance(Ogre::SceneManager* sceneManager, float dist)
{
	sceneManager->setShadowDirectionalLightExtrusionDistance(dist);
}

extern "C" __declspec(dllexport) float SceneManager_getShadowDirectionalLightExtrusionDistance(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowDirectionalLightExtrusionDistance();
}

extern "C" __declspec(dllexport) void SceneManager_setShadowFarDistance(Ogre::SceneManager* sceneManager, float distance)
{
	sceneManager->setShadowFarDistance(distance);
}

extern "C" __declspec(dllexport) float SceneManager_getShadowFarDistance(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowFarDistance();
}

extern "C" __declspec(dllexport) float SceneManager_getShadowFarDistanceSquared(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowFarDistanceSquared();
}

extern "C" __declspec(dllexport) void SceneManager_setShadowIndexBufferSize(Ogre::SceneManager* sceneManager, int size)
{
	sceneManager->setShadowIndexBufferSize(size);
}

extern "C" __declspec(dllexport) int SceneManager_getShadowIndexBufferSize(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowIndexBufferSize();
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTextureSize(Ogre::SceneManager* sceneManager, ushort size)
{
	sceneManager->setShadowTextureSize(size);
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTextureConfig(Ogre::SceneManager* sceneManager, int shadowIndex, ushort width, ushort height, Ogre::PixelFormat format)
{
	sceneManager->setShadowTextureConfig(shadowIndex, width, height, format);
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTexturePixelFormat(Ogre::SceneManager* sceneManager, Ogre::PixelFormat fmt)
{
	sceneManager->setShadowTexturePixelFormat(fmt);
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTextureCount(Ogre::SceneManager* sceneManager, int count)
{
	sceneManager->setShadowTextureCount(count);
}

extern "C" __declspec(dllexport) int SceneManager_getShadowTextureCount(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowTextureCount();
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTextureCountPerLightType(Ogre::SceneManager* sceneManager, Ogre::Light::LightTypes type, int count)
{
	sceneManager->setShadowTextureCountPerLightType(type, count);
}

extern "C" __declspec(dllexport) int SceneManager_getShadowTextureCountPerLightType(Ogre::SceneManager* sceneManager, Ogre::Light::LightTypes type)
{
	return sceneManager->getShadowTextureCountPerLightType(type);
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTextureSettings(Ogre::SceneManager* sceneManager, ushort size, ushort count, Ogre::PixelFormat fmt)
{
	sceneManager->setShadowTextureSettings(size, count, fmt);
}

extern "C" __declspec(dllexport) void SceneManager_setShadowDirLightTextureOffset(Ogre::SceneManager* sceneManager, float offset)
{
	sceneManager->setShadowDirLightTextureOffset(offset);
}

extern "C" __declspec(dllexport) float SceneManager_getShadowDirLightTextureOffset(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowDirLightTextureOffset();
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTextureFadeStart(Ogre::SceneManager* sceneManager, float fadeStart)
{
	sceneManager->setShadowTextureFadeStart(fadeStart);
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTextureFadeEnd(Ogre::SceneManager* sceneManager, float fadeEnd)
{
	sceneManager->setShadowTextureFadeEnd(fadeEnd);
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTextureSelfShadow(Ogre::SceneManager* sceneManager, bool selfShadow)
{
	sceneManager->setShadowTextureSelfShadow(selfShadow);
}

extern "C" __declspec(dllexport) bool SceneManager_getShadowTextureSelfShadow(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowTextureSelfShadow();
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTextureCasterMaterial(Ogre::SceneManager* sceneManager, String name)
{
	sceneManager->setShadowTextureCasterMaterial(name);
}

extern "C" __declspec(dllexport) void SceneManager_setShadowTextureReceiverMaterial(Ogre::SceneManager* sceneManager, String name)
{
	sceneManager->setShadowTextureReceiverMaterial(name);
}

extern "C" __declspec(dllexport) void SceneManager_setShadowCasterRenderBackFaces(Ogre::SceneManager* sceneManager, bool bf)
{
	sceneManager->setShadowCasterRenderBackFaces(bf);
}

extern "C" __declspec(dllexport) bool SceneManager_getShadowCasterRenderBackFaces(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowCasterRenderBackFaces();
}

extern "C" __declspec(dllexport) void SceneManager_setShadowUseInfiniteFarPlane(Ogre::SceneManager* sceneManager, bool enable)
{
	sceneManager->setShadowUseInfiniteFarPlane(enable);
}

extern "C" __declspec(dllexport) bool SceneManager_isShadowTechniqueStencilBased(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueStencilBased();
}

extern "C" __declspec(dllexport) bool SceneManager_isShadowTechniqueTextureBased(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueTextureBased();
}

extern "C" __declspec(dllexport) bool SceneManager_isShadowTechniqueModulative(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueModulative();
}

extern "C" __declspec(dllexport) bool SceneManager_isShadowTechniqueAdditive(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueAdditive();
}

extern "C" __declspec(dllexport) bool SceneManager_isShadowTechniqueIntegrated(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueIntegrated();
}

extern "C" __declspec(dllexport) bool SceneManager_isShadowTechniqueInUse(Ogre::SceneManager* sceneManager)
{
	return sceneManager->isShadowTechniqueInUse();
}

extern "C" __declspec(dllexport) void SceneManager_setShadowUseLightClipPlanes(Ogre::SceneManager* sceneManager, bool enabled)
{
	sceneManager->setShadowUseLightClipPlanes(enabled);
}

extern "C" __declspec(dllexport) bool SceneManager_getShadowUseLightClipPlanes(Ogre::SceneManager* sceneManager)
{
	return sceneManager->getShadowUseLightClipPlanes();
}

extern "C" __declspec(dllexport) void SceneManager_setSkyPlane(Ogre::SceneManager* sceneManager, bool enabled, float d, Vector3 normal, String matName, float scale, float tiling, bool drawFirst, float bow)
{
	Ogre::Plane p;
	p.d = d;
	p.normal = normal.toOgre();
	sceneManager->setSkyPlane(enabled, p, matName, scale, tiling, drawFirst, bow);
}

extern "C" __declspec(dllexport) void SceneManager_setSkyBox(Ogre::SceneManager* sceneManager, bool enabled, String matName, float distance, bool drawFirst)
{
	sceneManager->setSkyBox(enabled, matName, distance, drawFirst);
}

extern "C" __declspec(dllexport) void SceneManager_setSkyDome(Ogre::SceneManager* sceneManager, bool enabled, String matName)
{
	sceneManager->setSkyDome(enabled, matName);
}

#pragma warning(pop)