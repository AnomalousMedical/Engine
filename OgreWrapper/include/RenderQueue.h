#pragma once

namespace Ogre
{
	class RenderQueue;
}

namespace OgreWrapper{

ref class Renderable;

/// <summary>
/// 
/// </summary>
public ref class RenderQueue
{
private:
	Ogre::RenderQueue* renderQueue;

internal:
	/// <summary>
	/// Returns the native RenderQueue
	/// </summary>
	Ogre::RenderQueue* getRenderQueue();

	/// <summary>
	/// Constructor
	/// </summary>
	RenderQueue(Ogre::RenderQueue* renderQueue);

public:

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~RenderQueue();

	void addRenderable(Renderable^ renderable, unsigned char groupID, unsigned short priority);

	void addRenderable(Renderable^ renderable, unsigned char groupID);

	void addRenderable(Renderable^ renderable);

	unsigned char getDefaultQueueGroup();

	void setDefaultRenderablePriority(unsigned short priority);

	unsigned short getDefaultRenderablePriority();

	void setDefaultQueueGroup(unsigned char grp);

	void setSplitPassesByLightingType(bool split);

	void setSplitNoShadowPasses(bool split);

	void setShadowCastersCannotBeReceivers(bool ind);	
};

}
