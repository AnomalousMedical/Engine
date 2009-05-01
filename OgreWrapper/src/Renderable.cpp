#include "StdAfx.h"
#include "..\include\Renderable.h"

#include "OgreRenderable.h"

namespace OgreWrapper
{

Renderable::Renderable(Ogre::Renderable* renderable)
:renderable( renderable )
{

}

Renderable::~Renderable()
{
	renderable = 0;
}

Ogre::Renderable* Renderable::getRenderable()
{
	return renderable;
}

}