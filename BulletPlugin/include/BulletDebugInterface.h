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

public:
	BulletDebugInterface(void);

	virtual ~BulletDebugInterface(void);

	virtual IEnumerable<DebugEntry^>^ getEntries();

	virtual void renderDebug(DebugDrawingSurface^ drawingSurface, SimSubScene^ subScene);

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