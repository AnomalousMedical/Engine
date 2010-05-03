#pragma once

using namespace Engine;
using namespace System;
using namespace Engine::Renderer;
using namespace Engine::ObjectManagement;
using namespace System::Collections::Generic;

namespace BulletPlugin
{

ref class BulletDebugInterface : public DebugInterface
{
private:
	List<DebugEntry^>^ debugEntries;
	bool enabled;
	bool firstFrameDisabled;
	DebugDrawingSurface^ drawingSurface;
	bool depthTesting;

public:
	BulletDebugInterface(void);

	virtual ~BulletDebugInterface(void);

	virtual IEnumerable<DebugEntry^>^ getEntries();

	virtual void renderDebug(SimSubScene^ subScene);

	virtual void createDebugInterface(RendererPlugin^ rendererPlugin, SimSubScene^ subScene);

    virtual void destroyDebugInterface(RendererPlugin^ rendererPlugin, SimSubScene^ subScene);

	virtual void setDepthTesting(bool depthCheckEnabled);

	virtual bool isDepthTestingEnabled();

	virtual void setEnabled(bool enabled);

	property String^ Name
	{
		virtual String^ get()
		{
			return "Bullet Debug";
		}
	}
};

}