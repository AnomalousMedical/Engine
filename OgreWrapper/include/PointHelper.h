#pragma once

#include "AutoPtr.h"

using namespace EngineMath;

namespace Rendering{

class NativePointHelper;

ref class SceneNode;
ref class ManualObject;
value class Color;

[Engine::Attributes::DoNotSaveAttribute]
public ref class PointHelper
{
internal:
	AutoPtr<NativePointHelper> nativeSurface;

public:
	PointHelper();

	void drawPoint(Vector3% point);

	void drawPoint(Vector3 point);

	void setColor(float r, float g, float b, float a);

	void setColor(Color color);

	void clear();

	void begin();

	void end();

	void attachToNode(SceneNode^ node);

	void detachFromNode(SceneNode^ node);

	void setBuffer(ManualObject^ manualObject);

	void clearBuffer();

	NativePointHelper* getPointHelper();

	void setVisible(bool visible);

	void setMaterial(System::String^ material);

	/// <summary>
	/// Sets the render queue group this entity will be rendered through. 
	/// </summary>
	/// <param name="queueID">The queue id to add this object to.</param>
	void setRenderQueueGroup(unsigned char queueID);

	/// <summary>
	/// Gets the queue group for this entity.
	/// </summary>
	/// <returns>The render queue group of this object.</returns>
	unsigned char getRenderQueueGroup();
};

}