#pragma once

namespace PhysXWrapper
{

#pragma unmanaged

/// <summary>
/// This class gets around the fact that the NxScene cannot be returned from a function in 
/// this class due to error C3767.  Normal c++ classes do not care about this restriction
/// so this is a public class that wraps the NxScene.
/// </summary>
public class __declspec(dllexport) NxSceneExportWrapper
{
public:
	NxScene* scene;

	NxSceneExportWrapper(NxScene* scene);
};

#pragma managed

}
