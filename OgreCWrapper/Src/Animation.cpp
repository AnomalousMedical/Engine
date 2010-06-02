#include "Stdafx.h"

extern "C" __declspec(dllexport) String Animation_getName(Ogre::Animation* animation)
{
	return animation->getName().c_str();
}

extern "C" __declspec(dllexport) float Animation_getLength(Ogre::Animation* animation)
{
	return animation->getLength();
}

extern "C" __declspec(dllexport) Ogre::NodeAnimationTrack* Animation_createNodeTrack(Ogre::Animation* animation, ushort handle)
{
	return animation->createNodeTrack(handle);
}

extern "C" __declspec(dllexport) Ogre::NumericAnimationTrack* Animation_createNumericTrack(Ogre::Animation* animation, ushort handle)
{
	return animation->createNumericTrack(handle);
}

extern "C" __declspec(dllexport) Ogre::VertexAnimationTrack* Animation_createVertexTrack(Ogre::Animation* animation, ushort handle, Ogre::VertexAnimationType animType)
{
	return animation->createVertexTrack(handle, animType);
}

extern "C" __declspec(dllexport) ushort Animation_getNumNodeTracks(Ogre::Animation* animation)
{
	return animation->getNumNodeTracks();
}

extern "C" __declspec(dllexport) Ogre::NodeAnimationTrack* Animation_getNodeTrack(Ogre::Animation* animation, ushort handle)
{
	return animation->getNodeTrack(handle);
}

extern "C" __declspec(dllexport) bool Animation_hasNodeTrack(Ogre::Animation* animation, ushort handle)
{
	return animation->hasNodeTrack(handle);
}

extern "C" __declspec(dllexport) ushort Animation_getNumNumericTracks(Ogre::Animation* animation)
{
	return animation->getNumNumericTracks();
}

extern "C" __declspec(dllexport) Ogre::NumericAnimationTrack* Animation_getNumericTrack(Ogre::Animation* animation, ushort handle)
{
	return animation->getNumericTrack(handle);
}

extern "C" __declspec(dllexport) bool Animation_hasNumericTrack(Ogre::Animation* animation, ushort handle)
{
	return animation->hasNumericTrack(handle);
}

extern "C" __declspec(dllexport) ushort Animation_getNumVertexTracks(Ogre::Animation* animation)
{
	return animation->getNumVertexTracks();
}

extern "C" __declspec(dllexport) Ogre::VertexAnimationTrack* Animation_getVertexTrack(Ogre::Animation* animation, ushort handle)
{
	return animation->getVertexTrack(handle);
}

extern "C" __declspec(dllexport) bool Animation_hasVertexTrack(Ogre::Animation* animation, ushort handle)
{
	return animation->hasVertexTrack(handle);
}

extern "C" __declspec(dllexport) void Animation_destroyNodeTrack(Ogre::Animation* animation, ushort handle)
{
	animation->destroyNodeTrack(handle);
}

extern "C" __declspec(dllexport) void Animation_destroyNumericTrack(Ogre::Animation* animation, ushort handle)
{
	animation->destroyNumericTrack(handle);
}

extern "C" __declspec(dllexport) void Animation_destroyVertexTrack(Ogre::Animation* animation, ushort handle)
{
	animation->destroyVertexTrack(handle);
}

extern "C" __declspec(dllexport) void Animation_destroyAllTracks(Ogre::Animation* animation)
{
	animation->destroyAllTracks();
}

extern "C" __declspec(dllexport) void Animation_destroyAllNodeTracks(Ogre::Animation* animation)
{
	animation->destroyAllNodeTracks();
}

extern "C" __declspec(dllexport) void Animation_destroyAllNumericTracks(Ogre::Animation* animation)
{
	animation->destroyAllNumericTracks();
}

extern "C" __declspec(dllexport) void Animation_destroyAllVertexTracks(Ogre::Animation* animation)
{
	animation->destroyAllVertexTracks();
}

extern "C" __declspec(dllexport) void Animation_apply1(Ogre::Animation* animation, float timePos)
{
	animation->apply(timePos);
}

extern "C" __declspec(dllexport) void Animation_apply2(Ogre::Animation* animation, float timePos, float wieght)
{
	animation->apply(timePos, wieght);
}

extern "C" __declspec(dllexport) void Animation_apply3(Ogre::Animation* animation, float timePos, float wieght, float scale)
{
	animation->apply(timePos, wieght, scale);
}

extern "C" __declspec(dllexport) void Animation_apply4(Ogre::Animation* animation, Ogre::Skeleton* skeleton, float timePos)
{
	animation->apply(skeleton, timePos);
}

extern "C" __declspec(dllexport) void Animation_apply5(Ogre::Animation* animation, Ogre::Skeleton* skeleton, float timePos, float weight)
{
	animation->apply(skeleton, timePos, weight);
}

extern "C" __declspec(dllexport) void Animation_apply6(Ogre::Animation* animation, Ogre::Skeleton* skeleton, float timePos, float weight, float scale)
{
	animation->apply(skeleton, timePos, weight, scale);
}

extern "C" __declspec(dllexport) void Animation_apply7(Ogre::Animation* animation, Ogre::Skeleton* skeleton, float timePos, float weight, float* blendMask, int blendMaskSize, float scale)
{
	Ogre::AnimationState::BoneBlendMask boneMask;
	for(int i = 0; i < blendMaskSize; ++i)
	{
		boneMask.push_back(blendMask[i]);
	}
	animation->apply(skeleton, timePos, weight, &boneMask, scale);
}

extern "C" __declspec(dllexport) void Animation_apply8(Ogre::Animation* animation, Ogre::Entity* entity, float timePos, float weight, bool software, bool hardware)
{
	animation->apply(entity, timePos, weight, software, hardware);
}

extern "C" __declspec(dllexport) void Animation_setInterpolationMode(Ogre::Animation* animation, Ogre::Animation::InterpolationMode im)
{
	animation->setInterpolationMode(im);
}

extern "C" __declspec(dllexport) Ogre::Animation::InterpolationMode Animation_getInterpolationMode(Ogre::Animation* animation)
{
	return animation->getInterpolationMode();
}

extern "C" __declspec(dllexport) void Animation_setRotationInterpolationMode(Ogre::Animation* animation, Ogre::Animation::RotationInterpolationMode im)
{
	animation->setRotationInterpolationMode(im);
}

extern "C" __declspec(dllexport) Ogre::Animation::RotationInterpolationMode Animation_getRotationInterpolationMode(Ogre::Animation* animation)
{
	return animation->getRotationInterpolationMode();
}

extern "C" __declspec(dllexport) void Animation_optimize1(Ogre::Animation* animation)
{
	animation->optimise();
}

extern "C" __declspec(dllexport) void Animation_optimize2(Ogre::Animation* animation, bool discardIdentityNodeTracks)
{
	animation->optimise(discardIdentityNodeTracks);
}