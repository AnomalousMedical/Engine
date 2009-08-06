#pragma once

using namespace System;
using namespace Engine;
using namespace System::Collections::Generic;
using namespace Engine::ObjectManagement;
using namespace Engine::Saving;
using namespace Engine::Editing;
using namespace Engine::Reflection;

namespace BulletPlugin
{

[Engine::Attributes::NativeSubsystemType]
public ref class BulletSceneDefinition : public SimElementManagerDefinition
{
private:
	String^ name;	
	Vector3 worldAabbMin;
	Vector3 worldAabbMax;
	int maxProxies;
	Vector3 gravity;

	EditInterface^ editInterface;

	static MemberScanner^ memberScanner = gcnew MemberScanner();        

internal:
	/// <summary>
    /// Create function for commands.
    /// </summary>
    /// <param name="name">The name of the definition to create.</param>
    /// <returns>A new definition.</returns>
    static SimElementManagerDefinition^ Create(String^ name, EditUICallback^ callback)
    {
        return gcnew BulletSceneDefinition(name);
    }

public:
	static BulletSceneDefinition()
    {
        memberScanner->ProcessFields = false;
        memberScanner->Filter = gcnew EditableAttributeFilter();
    }

	BulletSceneDefinition(String^ name);

	virtual ~BulletSceneDefinition();

	virtual EditInterface^ getEditInterface();

	virtual SimElementManager^ createSimElementManager();

	virtual Type^ getSimElementManagerType();

	property String^ Name
	{
		virtual String^ get()
		{
			return name;
		}
	}

	[Editable]
	property Vector3 WorldAabbMin
	{
		Vector3 get()
		{
			return worldAabbMin;
		}
		void set(Vector3 value)
		{
			worldAabbMin = value;
		}
	}

	[Editable]
	property Vector3 WorldAabbMax
	{
		Vector3 get()
		{
			return worldAabbMax;
		}
		void set(Vector3 value)
		{
			worldAabbMax = value;
		}
	}

	[Editable]
	property int MaxProxies
	{
		int get()
		{
			return maxProxies;
		}
		void set(int value)
		{
			maxProxies = value;
		}
	}

	[Editable]
	property Vector3 Gravity
	{
		Vector3 get()
		{
			return gravity;
		}
		void set(Vector3 value)
		{
			gravity = value;
		}
	}

//Saving
protected:
	BulletSceneDefinition(LoadInfo^ info);

public:
	virtual void getInfo(SaveInfo^ info);
//End Saving
};

}