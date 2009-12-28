#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System;
using namespace Engine::Editing;
using namespace Engine::Reflection;
using namespace Engine::Saving;
using namespace Engine::Attributes;

class btSoftBody;

namespace BulletPlugin
{

ref class BulletScene;
ref class SoftBodyProviderDefinition;

public ref class SoftBodyProvider abstract : public SimElement
{
private:
	bool updatingPosition;

protected:
	virtual void* createSoftBodyImpl(BulletScene^ scene) = 0;

	virtual void destroySoftBodyImpl(BulletScene^ scene) = 0;


	/// <summary>
	/// This property should be set to true when a soft body provider subclass
    /// is updating the position of a SimObject. This will make the SoftBody
    /// elements ignore all position updates, which will keep the simulation
    /// from breaking. Set it back to false when finished.
	/// </summary>
	/// <value></value>
	property bool UpdatingPosition
	{
		void set(bool value)
		{
			updatingPosition = value;
		}
		bool get()
		{
			return updatingPosition;
		}
	}

internal:
	btSoftBody* createSoftBody(BulletScene^ scene);

	void destroySoftBody(BulletScene^ scene);

	property bool IsUpdatingPosition
	{
		bool get()
		{
			return updatingPosition;
		}
	}

public:
	SoftBodyProvider(SoftBodyProviderDefinition^ description);

	virtual ~SoftBodyProvider(void);

	virtual void updateOtherSubsystems() = 0;
};

}