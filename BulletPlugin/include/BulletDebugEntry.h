#pragma once

using namespace System;
using namespace Engine;

namespace BulletPlugin
{

ref class BulletDebugEntry : public DebugEntry
{
private:
	String^ text;
	btIDebugDraw::DebugDrawModes mode;

public:
	BulletDebugEntry(String^ text, btIDebugDraw::DebugDrawModes mode);

	virtual void setEnabled(bool enabled);

	virtual String^ ToString() override
	{
		return text;
	}

	property String^ Text
	{
		virtual String^ get()
		{
			return text;
		}
	}
};

}