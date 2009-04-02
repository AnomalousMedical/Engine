#pragma once

#include "AutoPtr.h"

class HullLibrary;

namespace PhysXWrapper
{

namespace StanHull
{

public enum class HullError : unsigned int
{
	QE_OK,            // success!
	QE_FAIL           // failed.
};

ref class HullDesc;
ref class HullResult;

/// <summary>
/// The interface class for the library.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class HullLibrary
{
private:
	AutoPtr<::HullLibrary> library;

public:
	HullLibrary(void);

	/// <summary>
	/// Compute a convex hull from the description.  Do not forget to release the result
	/// when finished.
	/// </summary>
	/// <param name="desc">Describes the input request.</param>
	/// <param name="result">Contains the results.</param>
	/// <returns></returns>
	HullError createConvexHull(HullDesc^ desc, HullResult^ result);

	/// <summary>
	/// Destroys the given result.
	/// </summary>
	/// <param name="result">The result to destroy.</param>
	/// <returns></returns>
	HullError releaseResult(HullResult^ result);
};

}

}