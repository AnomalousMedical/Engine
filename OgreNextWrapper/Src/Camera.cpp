#include "Stdafx.h"

#pragma warning(push)
#pragma warning(disable : 4190) //Disable c linkage warning

extern "C" _AnomalousExport void Camera_setPosition(Ogre::Camera* camera, Vector3 position)
{
	camera->setPosition(position.toOgre());
}

extern "C" _AnomalousExport Vector3 Camera_getPosition(Ogre::Camera* camera)
{
	return camera->getPosition();
}

extern "C" _AnomalousExport Quaternion Camera_getDerivedOrientation(Ogre::Camera* camera)
{
	return camera->getDerivedOrientation();
}

extern "C" _AnomalousExport Vector3 Camera_getDerivedPosition(Ogre::Camera* camera)
{
	return camera->getDerivedPosition();
}

extern "C" _AnomalousExport Vector3 Camera_getDerivedDirection(Ogre::Camera* camera)
{
	return camera->getDerivedDirection();
}

extern "C" _AnomalousExport Vector3 Camera_getDerivedUp(Ogre::Camera* camera)
{
	return camera->getDerivedUp();
}

extern "C" _AnomalousExport Vector3 Camera_getDerivedRight(Ogre::Camera* camera)
{
	return camera->getDerivedRight();
}

extern "C" _AnomalousExport Quaternion Camera_getRealOrientation(Ogre::Camera* camera)
{
	return camera->getRealOrientation();
}

extern "C" _AnomalousExport Vector3 Camera_getRealPosition(Ogre::Camera* camera)
{
	return camera->getRealPosition();
}

extern "C" _AnomalousExport Vector3 Camera_getRealDirection(Ogre::Camera* camera)
{
	return camera->getRealDirection();
}

extern "C" _AnomalousExport Vector3 Camera_getRealUp(Ogre::Camera* camera)
{
	return camera->getRealUp();
}

extern "C" _AnomalousExport Vector3 Camera_getRealRight(Ogre::Camera* camera)
{
	return camera->getRealRight();
}

extern "C" _AnomalousExport void Camera_lookAt(Ogre::Camera* camera, Vector3 lookAt)
{
	camera->lookAt(lookAt.toOgre());
}

extern "C" _AnomalousExport void Camera_setDirectionRaw(Ogre::Camera* camera, float x, float y, float z)
{
	camera->setDirection(x, y, z);
}

extern "C" _AnomalousExport void Camera_setDirection(Ogre::Camera* camera, Vector3 direction)
{
	camera->setDirection(direction.toOgre());
}

extern "C" _AnomalousExport Vector3 Camera_getDirection(Ogre::Camera* camera)
{
	return camera->getDirection();
}

extern "C" _AnomalousExport Vector3 Camera_getUp(Ogre::Camera* camera)
{
	return camera->getUp();
}

extern "C" _AnomalousExport Vector3 Camera_getRight(Ogre::Camera* camera)
{
	return camera->getRight();
}

extern "C" _AnomalousExport void Camera_setLodBias(Ogre::Camera* camera, float factor)
{
	camera->setLodBias(factor);
}

extern "C" _AnomalousExport float Camera_getLodBias(Ogre::Camera* camera)
{
	return camera->getLodBias();
}

extern "C" _AnomalousExport Ray3 Camera_getCameraToViewportRay(Ogre::Camera* camera, float screenx, float screeny)
{
	return camera->getCameraToViewportRay(screenx, screeny);
}

extern "C" _AnomalousExport void Camera_setWindow(Ogre::Camera* camera, float left, float top, float right, float bottom)
{
	camera->setWindow(left, top, right, bottom);
}

extern "C" _AnomalousExport void Camera_resetWindow(Ogre::Camera* camera)
{
	camera->resetWindow();
}

extern "C" _AnomalousExport bool Camera_isWindowSet(Ogre::Camera* camera)
{
	return camera->isWindowSet();
}

extern "C" _AnomalousExport void Camera_setAutoAspectRatio(Ogre::Camera* camera, bool autoRatio)
{
	camera->setAutoAspectRatio(autoRatio);
}

extern "C" _AnomalousExport bool Camera_getAutoAspectRatio(Ogre::Camera* camera)
{
	return camera->getAutoAspectRatio();
}

extern "C" _AnomalousExport float Camera_getNearClipDistance(Ogre::Camera* camera)
{
	return camera->getNearClipDistance();
}

extern "C" _AnomalousExport float Camera_getFarClipDistance(Ogre::Camera* camera)
{
	return camera->getFarClipDistance();
}

extern "C" _AnomalousExport void Camera_setUseRenderingDistance(Ogre::Camera* camera, bool use)
{
	camera->setUseRenderingDistance(use);
}

extern "C" _AnomalousExport bool Camera_getUseRenderingDistance(Ogre::Camera* camera)
{
	return camera->getUseRenderingDistance();
}

extern "C" _AnomalousExport void Camera_setFOVy(Ogre::Camera* camera, float fovy)
{
	camera->setFOVy(Ogre::Radian(fovy));
}

extern "C" _AnomalousExport float Camera_getFOVy(Ogre::Camera* camera)
{
	return camera->getFOVy().valueRadians();
}

extern "C" _AnomalousExport void Camera_setNearClipDistance(Ogre::Camera* camera, float nearDistance)
{
	camera->setNearClipDistance(nearDistance);
}

extern "C" _AnomalousExport void Camera_setFarClipDistance(Ogre::Camera* camera, float farDistance)
{
	camera->setFarClipDistance(farDistance);
}

extern "C" _AnomalousExport void Camera_setAspectRatio(Ogre::Camera* camera, float ratio)
{
	camera->setAspectRatio(ratio);
}

extern "C" _AnomalousExport float Camera_getAspectRatio(Ogre::Camera* camera)
{
	return camera->getAspectRatio();
}

extern "C" _AnomalousExport void Camera_setRenderingDistance(Ogre::Camera* camera, float dist)
{
	camera->setRenderingDistance(dist);
}

extern "C" _AnomalousExport float Camera_getRenderingDistance(Ogre::Camera* camera)
{
	return camera->getRenderingDistance();
}

extern "C" _AnomalousExport Ogre::ProjectionType Camera_getProjectionType(Ogre::Camera* camera)
{
	return camera->getProjectionType();
}

extern "C" _AnomalousExport void Camera_setProjectionType(Ogre::Camera* camera, Ogre::ProjectionType type)
{
	camera->setProjectionType(type);
}

extern "C" _AnomalousExport void Camera_setOrthoWindow(Ogre::Camera* camera, float w, float h)
{
	camera->setOrthoWindow(w, h);
}

extern "C" _AnomalousExport void Camera_setOrthoWindowWidth(Ogre::Camera* camera, float w)
{
	camera->setOrthoWindowWidth(w);
}

extern "C" _AnomalousExport void Camera_setOrthoWindowHeight(Ogre::Camera* camera, float h)
{
	camera->setOrthoWindowHeight(h);
}

extern "C" _AnomalousExport float Camera_getOrthoWindowWidth(Ogre::Camera* camera)
{
	return camera->getOrthoWindowWidth();
}

extern "C" _AnomalousExport float Camera_getOrthoWindowHeight(Ogre::Camera* camera)
{
	return camera->getOrthoWindowHeight();
}

extern "C" _AnomalousExport Matrix4x4 Camera_getViewMatrix(Ogre::Camera* camera)
{
	return camera->getViewMatrix();
}

extern "C" _AnomalousExport Matrix4x4 Camera_getProjectionMatrix(Ogre::Camera* camera)
{
	return camera->getProjectionMatrix();
}

extern "C" _AnomalousExport void Camera_getFrustumExtents(Ogre::Camera* camera, float* outLeft, float* outRight, float* outTop, float* outBottom)
{
	camera->getFrustumExtents(*outLeft, *outRight, *outTop, *outBottom);
}

extern "C" _AnomalousExport void Camera_setFrustumExtents(Ogre::Camera* camera, float left, float right, float top, float bottom)
{
	camera->setFrustumExtents(left, right, top, bottom);
}

extern "C" _AnomalousExport Vector3 Camera_getPositionForViewUpdate(Ogre::Camera* camera)
{
	return camera->getPositionForViewUpdate();
}

extern "C" _AnomalousExport Quaternion Camera_getOrientationForViewUpdate(Ogre::Camera* camera)
{
	return camera->getOrientationForViewUpdate();
}

#pragma warning(pop)