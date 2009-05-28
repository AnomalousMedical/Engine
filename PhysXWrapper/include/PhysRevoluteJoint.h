#pragma once

#include "PhysJoint.h"
#include "Enums.h"

class NxRevoluteJoint;
class NxJointDesc;
class NxRevoluteJointDesc;

namespace PhysXWrapper
{

ref class PhysRevoluteJointDesc;
ref class PhysJointLimitPairDesc;
ref class PhysMotorDesc;
ref class PhysSpringDesc;

/// <summary>
/// Wrapper for NxRevoluteJoint.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
public ref class PhysRevoluteJoint : public PhysJoint
{
internal:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysRevoluteJoint(NxRevoluteJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene);

	//A pointer to the actual joint subclass.
	NxRevoluteJoint* typedJoint;

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysRevoluteJoint(){}
public:
	/// <summary>
	/// Writes all of the object's attributes to the desc struct
	/// </summary>
	/// <param name="desc">The descriptor used to retrieve the state of the object.</param>
	void saveToDesc(PhysRevoluteJointDesc^ desc);

	/// <summary>
	/// Use this for changing a significant number of joint parameters at once.
	/// </summary>
	/// <remarks>
	/// Use the set() methods for changing only a single property at once.
	/// 
	/// Please note that you can not change the actor pointers using this function, if you 
	/// do so the joint will be marked as broken and will stop working.
	/// 
	/// Calling the loadFromDesc() method on a broken joint will result in an error message.
	/// 
	/// Sleeping: This call wakes the actor if it is sleeping.
	/// </remarks>
	/// <param name="desc">The descriptor used to set the state of the object.</param>
	void loadFromDesc(PhysRevoluteJointDesc^ desc);

	/// <summary>
	/// Sets angular joint limits.
	/// If either of these limits are set, any planar limits in NxJoint are ignored. The limits are angles defined the same way as the values getAngle() returns.
	/// 
	/// The following has to hold:
	/// 
	/// Pi &lt; lowAngle &lt; highAngle &lt; Pi Both limits are disabled by default.
	/// Also sets coefficients of restitutions for the low and high angular limits. These settings are only used if valid limits are set using setLimits(). These restitution coefficients work the same way as for contacts.
	/// 
	/// The coefficient of restitution determines whether a collision with the joint limit is completely elastic (like pool balls, restitution = 1, no energy is lost in the collision), completely inelastic (like putty, restitution = 0, no rebound after collision) or somewhere in between. The default is 0 for both.
	/// 
	/// This automatically enables the limit.
	/// </summary>
	/// <param name="desc">The new joint limit settings. Range: See PhysJointLimitPairDesc.</param>
	void setLimits(PhysJointLimitPairDesc^ desc);

	/// <summary>
	/// Retrieves the joint limits.
	/// </summary>
	/// <param name="desc">Used to retrieve the joint limit settings.</param>
	/// <returns>True if the limit is enabled.</returns>
	bool getLimits(PhysJointLimitPairDesc^ desc);

	/// <summary>
	/// Sets motor parameters for the joint.
	/// </summary>
	/// <remarks>
	/// The motor rotates the bodies relative to each other along the hinge axis. The motor 
	/// has these parameters:
	/// 
	/// velTarget - the relative velocity the motor is trying to achieve. The motor will 
	/// only be able to reach this velocity if the maxForce is sufficiently large. If the 
	/// joint is spinning faster than this velocity, the motor will actually try to brake. 
	/// If you set this to infinity then the motor will keep speeding up, unless there is 
	/// some sort of resistance on the attached bodies. The sign of this variable determines 
	/// the rotation direction, with positive values going the same way as positive joint 
	/// angles. Default is infinity. 
	/// maxForce - the maximum force (torque in this case) the motor can exert. Zero disables
	/// the motor. Default is 0, should be >= 0. Setting this to a very large value if 
	/// velTarget is also very large may not be a good idea. 
	/// freeSpin - if this flag is set, and if the joint is spinning faster than velTarget, 
	/// then neither braking nor additional acceleration will result. default: false. 
	/// 	
	/// This automatically enables the motor.
	/// </remarks>
	/// <param name="motor">The motor to use.</param>
	void setMotor(PhysMotorDesc^ motor);

	/// <summary>
	/// Reads back the motor parameters.
	/// </summary>
	/// <param name="motor">Used to store the motor parameters of the joint.</param>
	/// <returns>True if the motor is enabled.</returns>
	bool getMotor(PhysMotorDesc^ motor);

	/// <summary>
	/// Sets spring parameters.
	/// </summary>
	/// <remarks>
	/// The spring is implicitly integrated so no instability should result for arbitrary 
	/// spring and damping constants. Using these settings together with a motor is not 
	/// possible -- the motor will have priority and the spring settings are ignored. If you 
	/// would like to simulate your motor's internal friction, do this by altering the motor 
	/// parameters directly.
	/// 
	/// spring - The rotational spring acts along the hinge axis and tries to force the joint
	/// angle to zero. A setting of zero disables the spring. Default is 0, should be >= 0. 
	/// damper - Damping coefficient; acts against the hinge's angular velocity. A setting 
	/// of zero disables the damping. The default is 0, should be >= 0. targetValue - The 
	/// angle at which the spring is relaxed. In [-Pi,Pi]. Default is 0.
	/// 
	/// This automatically enables the spring
	/// </remarks>
	/// <param name="spring">The new spring parameters for the joint.</param>
	void setSpring(PhysSpringDesc^ spring);

	/// <summary>
	/// Retrieves spring settings.
	/// </summary>
	/// <param name="spring">Used to retrieve the spring parameters for the joint.</param>
	/// <returns>True if the spring is enabled.</returns>
	bool getSpring(PhysSpringDesc^ spring);

	/// <summary>
	/// Retrieves the current revolute joint angle.
	/// </summary>
	/// <remarks>
	/// The relative orientation of the bodies is stored when the joint is created, or when 
	/// setAxis() or setAnchor() is called. This initial orientation returns an angle of zero,
	/// and joint angles are measured relative to this pose. The angle is in the range 
	/// [-Pi, Pi], with positive angles CCW around the axis, measured from body2 to body1.
	/// 
	/// Unit: Radians Range: [-PI,PI]
	/// </remarks>
	/// <returns></returns>
	float getAngle();

	/// <summary>
	/// Retrieves the revolute joint angle's rate of change (angular velocity). 
	/// 
	/// It is the angular velocity of body1 minus body2 projected along the axis
	/// </summary>
	/// <returns>The hinge velocity.</returns>
	float getVelocity();

	/// <summary>
	/// Sets the flags to enable/disable the spring/motor/limit.
	/// </summary>
	/// <param name="flags">A combination of RevoluteJointFlag flags to set for this joint.</param>
	void setFlags(RevoluteJointFlag flags);

	/// <summary>
	/// Retrieve the revolute joints flags.
	/// </summary>
	/// <returns>The current flag settings.</returns>
	RevoluteJointFlag getFlags();

	/// <summary>
	/// Sets the joint projection mode.
	/// </summary>
	/// <param name="projectionMode">The new projection mode.</param>
	void setProjectionMode(JointProjectionMode projectionMode);

	/// <summary>
	/// Retrieve the revolute joints flags.
	/// </summary>
	/// <returns>The current flag settings.</returns>
	JointProjectionMode getProjectionMode();
};

}