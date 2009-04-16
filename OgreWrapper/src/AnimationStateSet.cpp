#include "StdAfx.h"
#include "..\include\AnimationStateSet.h"
#include "Ogre.h"
#include "MarshalUtils.h"
#include "AnimationState.h"

namespace Engine{

namespace Rendering{

AnimationStateSet::AnimationStateSet(Ogre::AnimationStateSet* animationStateSet)
:animationStateSet(animationStateSet)
{

}

AnimationStateSet::~AnimationStateSet(void)
{

}

AnimationState^ AnimationStateSet::createAnimationState(System::String^ animName, float timePos, float length, float weight, bool enabled)
{
	if(!hasAnimationState(animName))
	{
		Ogre::AnimationState* ogreAnimationState = animationStateSet->createAnimationState(MarshalUtils::convertString(animName), timePos, length, weight, enabled);
		return states.getObject(ogreAnimationState, this);
	}
	Logging::Log::Default->sendMessage("Attempted to create a duplicate animation named {0} in set {1}.  New animation ignored.", Logging::LogLevel::Warning, "Rendering", animName);
	return nullptr;
}

AnimationState^ AnimationStateSet::getAnimationState(System::String^ name)
{
	Ogre::AnimationState* ogreAnimationState = animationStateSet->getAnimationState(MarshalUtils::convertString(name));
	return states.getObject(ogreAnimationState, this);
}

bool AnimationStateSet::hasAnimationState(System::String^ name)
{
	return animationStateSet->hasAnimationState(MarshalUtils::convertString(name));
}

void AnimationStateSet::removeAnimationState(System::String^ name)
{
	states.destroyObject(animationStateSet->getAnimationState(MarshalUtils::convertString(name)));
	animationStateSet->removeAnimationState(MarshalUtils::convertString(name));
}

void AnimationStateSet::removeAllAnimationStates()
{
	states.clearObjects();
	animationStateSet->removeAllAnimationStates();
}

void AnimationStateSet::copyMatchingState(AnimationStateSet^ target)
{
	animationStateSet->copyMatchingState(target->animationStateSet);
}

unsigned long AnimationStateSet::getDirtyFrameNumber()
{
	return animationStateSet->getDirtyFrameNumber();
}

bool AnimationStateSet::hasEnabledAnimationState()
{
	return animationStateSet->hasEnabledAnimationState();
}

void AnimationStateSet::notifyDirty()
{
	animationStateSet->_notifyDirty();
}

}

}