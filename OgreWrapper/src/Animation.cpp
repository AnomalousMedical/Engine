#include "stdafx.h"
#include "Animation.h"
#include "MarshalUtils.h"
#include "Ogre.h"
#include "Entity.h"
#include "Skeleton.h"
#include "NodeAnimationTrack.h"
#include "NumericAnimationTrack.h"
#include "VertexAnimationTrack.h"

namespace OgreWrapper
{

Animation::Animation(Ogre::Animation* ogreAnimation)
:ogreAnimation(ogreAnimation)
{

}

Animation::~Animation()
{
	ogreAnimation = 0;
}

Ogre::Animation* Animation::getOgreAnimation()
{
	return ogreAnimation;
}

System::String^ Animation::getName()
{
	return MarshalUtils::convertString(ogreAnimation->getName());
}

float Animation::getLength()
{
	return ogreAnimation->getLength();
}

NodeAnimationTrack^ Animation::createNodeTrack(unsigned short handle)
{
	Ogre::NodeAnimationTrack* ogreTrack = ogreAnimation->createNodeTrack(handle);
	return nodeAnimations.getObject(ogreTrack, this);
}

NumericAnimationTrack^ Animation::createNumericTrack(unsigned short handle)
{
	Ogre::NumericAnimationTrack* ogreTrack = ogreAnimation->createNumericTrack(handle);
	return numericAnimations.getObject(ogreTrack);
}

VertexAnimationTrack^ Animation::createVertexTrack(unsigned short handle, VertexAnimationType animType)
{
	Ogre::VertexAnimationTrack* ogreTrack = ogreAnimation->createVertexTrack(handle, (Ogre::VertexAnimationType)animType);
	return vertexAnimations.getObject(ogreTrack, this);
}

unsigned short Animation::getNumNodeTracks()
{
	return ogreAnimation->getNumNodeTracks();
}

NodeAnimationTrack^ Animation::getNodeTrack(unsigned short handle)
{
	if(ogreAnimation->hasNodeTrack(handle))
	{
		Ogre::NodeAnimationTrack* ogreTrack = ogreAnimation->getNodeTrack(handle);
		return nodeAnimations.getObject(ogreTrack, this);
	}
	Logging::Log::Default->sendMessage("Attempted to get a NodeTrack with handle {0} from animation {1} that does not exist.", Logging::LogLevel::Warning, "Rendering", handle.ToString(), getName());
	return nullptr;
}

bool Animation::hasNodeTrack(unsigned short handle)
{
	return ogreAnimation->hasNodeTrack(handle);
}

unsigned short Animation::getNumNumericTracks()
{
	return ogreAnimation->getNumNumericTracks();
}

NumericAnimationTrack^ Animation::getNumericTrack(unsigned short handle)
{
	if(ogreAnimation->hasNumericTrack(handle))
	{
		Ogre::NumericAnimationTrack* ogreTrack = ogreAnimation->getNumericTrack(handle);
		return numericAnimations.getObject(ogreTrack);
	}
	Logging::Log::Default->sendMessage("Attempted to get a NumericTrack with handle {0} from animation {1} that does not exist.", Logging::LogLevel::Warning, "Rendering", handle.ToString(), getName());
	return nullptr;
}

bool Animation::hasNumericTrack(unsigned short handle)
{
	return ogreAnimation->hasNumericTrack(handle);
}

unsigned short Animation::getNumVertexTracks()
{
	return ogreAnimation->getNumVertexTracks();
}

VertexAnimationTrack^ Animation::getVertexTrack(unsigned short handle)
{
	if(ogreAnimation->hasVertexTrack(handle))
	{
		Ogre::VertexAnimationTrack* ogreTrack = ogreAnimation->getVertexTrack(handle);
		return vertexAnimations.getObject(ogreTrack, this);
	}
	Logging::Log::Default->sendMessage("Attempted to get a VertexTrack with handle {0} from animation {1} that does not exist.", Logging::LogLevel::Warning, "Rendering", handle.ToString(), getName());
	return nullptr;
}

bool Animation::hasVertexTrack(unsigned short handle)
{
	return ogreAnimation->hasVertexTrack(handle);
}

void Animation::destroyNodeTrack(unsigned short handle)
{
	if(ogreAnimation->hasNodeTrack(handle))
	{
		nodeAnimations.destroyObject(ogreAnimation->getNodeTrack(handle));
		ogreAnimation->destroyNodeTrack(handle);
	}
	Logging::Log::Default->sendMessage("Attempted to delete a NodeTrack with handle {0} from animation {1} that does not exist.  No changes made.", Logging::LogLevel::Warning, "Rendering", handle.ToString(), getName());
}

void Animation::destroyNumericTrack(unsigned short handle)
{
	if(ogreAnimation->hasNumericTrack(handle))
	{
		numericAnimations.destroyObject(ogreAnimation->getNumericTrack(handle));
		ogreAnimation->destroyNumericTrack(handle);
	}
	Logging::Log::Default->sendMessage("Attempted to delete a NumericTrack with handle {0} from animation {1} that does not exist.  No changes made.", Logging::LogLevel::Warning, "Rendering", handle.ToString(), getName());
}

void Animation::destroyVertexTrack(unsigned short handle)
{
	if(ogreAnimation->hasVertexTrack(handle))
	{
		vertexAnimations.destroyObject(ogreAnimation->getVertexTrack(handle));
		ogreAnimation->destroyVertexTrack(handle);
	}
	Logging::Log::Default->sendMessage("Attempted to delete a VertexTrack with handle {0} from animation {1} that does not exist.  No changes made.", Logging::LogLevel::Warning, "Rendering", handle.ToString(), getName());
}

void Animation::destroyAllTracks()
{
	vertexAnimations.clearObjects();
	numericAnimations.clearObjects();
	nodeAnimations.clearObjects();
	ogreAnimation->destroyAllTracks();
}

void Animation::destroyAllNodeTracks()
{
	nodeAnimations.clearObjects();
	ogreAnimation->destroyAllNodeTracks();
}

void Animation::destroyAllNumericTracks()
{
	numericAnimations.clearObjects();
	ogreAnimation->destroyAllNumericTracks();
}

void Animation::destroyAllVertexTracks()
{
	vertexAnimations.clearObjects();
	ogreAnimation->destroyAllVertexTracks();
}

void Animation::apply(float timePos)
{
	return ogreAnimation->apply(timePos);
}

void Animation::apply(float timePos, float weight)
{
	return ogreAnimation->apply(timePos, weight);
}

void Animation::apply(float timePos, float weight, float scale)
{
	return ogreAnimation->apply(timePos, weight, scale);
}

void Animation::apply(Skeleton^ skeleton, float timePos)
{
	return ogreAnimation->apply(skeleton->getSkeleton(), timePos);
}

void Animation::apply(Skeleton^ skeleton, float timePos, float weight)
{
	return ogreAnimation->apply(skeleton->getSkeleton(), timePos, weight);
}

void Animation::apply(Skeleton^ skeleton, float timePos, float weight, float scale)
{
	return ogreAnimation->apply(skeleton->getSkeleton(), timePos, weight, scale);
}

void Animation::apply(Skeleton^ skeleton, float timePos, float weight, BoneBlendMask^ blendMask, float scale)
{
	Ogre::AnimationState::BoneBlendMask ogreMask;
	for each(float mask in blendMask)
	{
		ogreMask.push_back(mask);
	}
	return ogreAnimation->apply(skeleton->getSkeleton(), timePos, weight, &ogreMask, scale);
}

void Animation::apply(Entity^ entity, float timePos, float weight, bool software, bool hardware)
{
	return ogreAnimation->apply(entity->getEntity(), timePos, weight, software, hardware);
}

void Animation::setInterpolationMode(InterpolationMode im)
{
	return ogreAnimation->setInterpolationMode((Ogre::Animation::InterpolationMode)im);
}

InterpolationMode Animation::getInterpolationMode()
{
	return (InterpolationMode)ogreAnimation->getInterpolationMode();
}

void Animation::setRotationInterpolationMode(RotationInterpolationMode im)
{
	return ogreAnimation->setRotationInterpolationMode((Ogre::Animation::RotationInterpolationMode)im);
}

RotationInterpolationMode Animation::getRotationInterpolationMode()
{
	return (RotationInterpolationMode)ogreAnimation->getRotationInterpolationMode();
}

void Animation::optimize()
{
	return ogreAnimation->optimise();
}

void Animation::optimize(bool discardIdentityNodeTracks)
{
	return ogreAnimation->optimise(discardIdentityNodeTracks);
}

}