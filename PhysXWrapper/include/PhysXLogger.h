#pragma once

#include "NxUserOutputStream.h"

namespace Physics
{

/// <summary>
/// This class interacts with the PhysX SDK and forwards log messages to the 
/// system log.
/// </summary>
class PhysXLogger : public NxUserOutputStream
{
public:
	PhysXLogger(void);

	virtual ~PhysXLogger(void);

	virtual void reportError( NxErrorCode code, const char* message, const char* file, int line );

	virtual NxAssertResponse reportAssertViolation( const char* message, const char* file, int line );

	virtual void print( const char* message );
};

}