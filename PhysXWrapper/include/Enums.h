#pragma once

namespace PhysXWrapper
{

[Engine::Attributes::SingleEnum]
public enum class ForceMode : unsigned int
{
	NX_FORCE,                   //!< parameter has unit of mass * distance/ time^2, i.e. a force
	NX_IMPULSE,                 //!< parameter has unit of mass * distance /time
	NX_VELOCITY_CHANGE,			//!< parameter has unit of distance / time, i.e. the effect is mass independent: a velocity change.
	NX_SMOOTH_IMPULSE,          //!< same as NX_IMPULSE but the effect is applied over all substeps. Use this for motion controllers that repeatedly apply an impulse.
	NX_SMOOTH_VELOCITY_CHANGE,	//!< same as NX_VELOCITY_CHANGE but the effect is applied over all substeps. Use this for motion controllers that repeatedly apply an impulse.
	NX_ACCELERATION				//!< parameter has unit of distance/ time^2, i.e. an acceleration. It gets treated just like a force except the mass is not divided out before integration.
};

[Engine::Attributes::MultiEnum]
public enum class ContactPairFlag : unsigned int
{
	NX_IGNORE_PAIR								= (1<<0),	//!< Disable contact generation for this pair

	NX_NOTIFY_ON_START_TOUCH					= (1<<1),	//!< Pair callback will be called when the pair starts to be in contact
	NX_NOTIFY_ON_END_TOUCH						= (1<<2),	//!< Pair callback will be called when the pair stops to be in contact
	NX_NOTIFY_ON_TOUCH							= (1<<3),	//!< Pair callback will keep getting called while the pair is in contact
	NX_NOTIFY_ON_IMPACT							= (1<<4),	//!< [Not yet implemented] pair callback will be called when it may be appropriate for the pair to play an impact sound
	NX_NOTIFY_ON_ROLL							= (1<<5),	//!< [Not yet implemented] pair callback will be called when the pair is in contact and rolling.
	NX_NOTIFY_ON_SLIDE							= (1<<6),	//!< [Not yet implemented] pair callback will be called when the pair is in contact and sliding (and not rolling).
	NX_NOTIFY_FORCES							= (1<<7),	//!< The (summed total) friction force and normal force will be given in the NxContactPair variable in the contact report.
	NX_NOTIFY_ON_START_TOUCH_FORCE_THRESHOLD	= (1<<8),	//!< Pair callback will be called when the contact force between two actors exceeds one of the actor-defined force thresholds
	NX_NOTIFY_ON_END_TOUCH_FORCE_THRESHOLD		= (1<<9),	//!< Pair callback will be called when the contact force between two actors falls below the actor-defined force thresholds
	NX_NOTIFY_ON_TOUCH_FORCE_THRESHOLD			= (1<<10),	//!< Pair callback will keep getting called while the contact force between two actors exceeds one of the actor-defined force thresholds

	NX_NOTIFY_CONTACT_MODIFICATION				= (1<<16),	//!< Generate a callback for all associated contact constraints, making it possible to edit the constraint. This flag is not included in NX_NOTIFY_ALL for performance reasons. \see NxUserContactModify

	[Engine::Attributes::InclusiveEnumField]
	NX_NOTIFY_ALL								= (NX_NOTIFY_ON_START_TOUCH|NX_NOTIFY_ON_END_TOUCH|NX_NOTIFY_ON_TOUCH|NX_NOTIFY_ON_IMPACT|NX_NOTIFY_ON_ROLL|NX_NOTIFY_ON_SLIDE|NX_NOTIFY_FORCES|
												   NX_NOTIFY_ON_START_TOUCH_FORCE_THRESHOLD|NX_NOTIFY_ON_END_TOUCH_FORCE_THRESHOLD|NX_NOTIFY_ON_TOUCH_FORCE_THRESHOLD)
};

[Engine::Attributes::MultiEnum]
public enum class WheelShapeFlags : unsigned int
{
	/**<summary>
	 Determines whether the suspension axis or the ground contact normal is used for the suspension constraint.

	</summary>*/
	NX_WF_WHEEL_AXIS_CONTACT_NORMAL = 1 << 0,
	
	/**<summary>
	 If set, the laterial slip velocity is used as the input to the tire function, rather than the slip angle.

	</summary>*/
	NX_WF_INPUT_LAT_SLIPVELOCITY = 1 << 1,
	
	/**<summary>
	 If set, the longutudal slip velocity is used as the input to the tire function, rather than the slip ratio.  
	</summary>*/
	NX_WF_INPUT_LNG_SLIPVELOCITY = 1 << 2,

	/**<summary>
	 If set, does not factor out the suspension travel and wheel radius from the spring force computation.  This is the legacy behavior from the raycast capsule approach.
	</summary>*/
	NX_WF_UNSCALED_SPRING_BEHAVIOR = 1 << 3, 

	/**<summary>
	 If set, the axle speed is not computed by the simulation but is rather expected to be provided by the user every simulation step via NxWheelShape::setAxleSpeed().
	</summary>*/
	NX_WF_AXLE_SPEED_OVERRIDE = 1 << 4,

	/**<summary>
	 If set, the NxWheelShape will emulate the legacy raycast capsule based wheel.
	See #NxTireFunctionDesc
	</summary>*/
	NX_WF_EMULATE_LEGACY_WHEEL = 1 << 5,

	/**<summary>
	 If set, the NxWheelShape will clamp the force in the friction constraints.
	See #NxTireFunctionDesc
	</summary>*/
	NX_WF_CLAMPED_FRICTION = 1 << 6,
};

[Engine::Attributes::MultiEnum]
public enum class ActorFlag : unsigned int
{
	/**<summary>
	 Enable/disable collision detection

	Turn off collision detection, i.e. the actor will not collide with other objects. Please note that you might need to wake 
	the actor up if it is sleeping, this depends on the result you wish to get when using this flag. (If a body is asleep it 
	will not start to fall through objects unless you activate it).

	Note: Also excludes the actor from overlap tests!
	</summary>*/
	NX_AF_DISABLE_COLLISION			= (1<<0),

	/**<summary>
	 Enable/disable collision response (reports contacts but don't use them)

	@see NxUserContactReport
	</summary>*/
	NX_AF_DISABLE_RESPONSE			= (1<<1), 

	/**<summary>
	 Disables COM update when computing inertial properties at creation time.

	When sdk computes inertial properties, by default the center of mass will be calculated too.  However, if lockCOM is set to a non-zero (true) value then the center of mass will not be altered.
	</summary>*/
	NX_AF_LOCK_COM					= (1<<2),

	/**<summary>
	 Enable/disable collision with fluid.
	</summary>*/
	NX_AF_FLUID_DISABLE_COLLISION	= (1<<3),

	/**<summary>
	 Turn on contact modification callback for the actor.

	@see NxScene.setUserContactModify(), NX_NOTIFY_CONTACT_MODIFICATION
	</summary>*/
	NX_AF_CONTACT_MODIFICATION		= (1<<4),


	/**<summary>
	 Force cone friction to be used for this actor.	

	This ensures that all contacts involving the actor will use cone friction, rather than the default
	simplified scheme. This will however have a negative impact on performance in software scenes. Use this
	flag if sliding objects show an affinity for moving along the world axes.

	\note Only applies to software scenes. Hardware scenes always force cone friction.

	Cone friction may also be applied wholesale to a scene using the NX_SF_FORCE_CONE_FRICTION 
	flag, see #NxSceneFlags.
	</summary>*/
	NX_AF_FORCE_CONE_FRICTION		= (1<<5),

	/**<summary>
	 Enable/disable custom contact filtering. 

	When enabled the user will be queried for contact filtering for all contacts involving this actor.
	</summary>*/
	NX_AF_USER_ACTOR_PAIR_FILTERING	= (1<<6)
};

[Engine::Attributes::MultiEnum]
public enum class BodyFlag : unsigned int
{
	/**<summary>
	 Set if gravity should not be applied on this body

	@see NxBodyDesc.flags NxScene.setGravity()
	</summary>*/
	NX_BF_DISABLE_GRAVITY	= (1<<0),
	
	/**<summary>	
	 Enable/disable freezing for this body/actor. 

	\note This is an EXPERIMENTAL feature which doesn't always work on in all situations, e.g. 
	for actors which have joints connected to them.
	
	To freeze an actor is a way to simulate that it is static. The actor is however still simulated
	as if it was dynamic, it's position is just restored after the simulation has finished. A much
	more stable way to make an actor temporarily static is to raise the NX_BF_KINEMATIC flag.
	</summary>*/
	NX_BF_FROZEN_POS_X		= (1<<1),
	NX_BF_FROZEN_POS_Y		= (1<<2),
	NX_BF_FROZEN_POS_Z		= (1<<3),
	NX_BF_FROZEN_ROT_X		= (1<<4),
	NX_BF_FROZEN_ROT_Y		= (1<<5),
	NX_BF_FROZEN_ROT_Z		= (1<<6),
	[Engine::Attributes::InclusiveEnumField]
	NX_BF_FROZEN_POS		= NX_BF_FROZEN_POS_X|NX_BF_FROZEN_POS_Y|NX_BF_FROZEN_POS_Z,
	[Engine::Attributes::InclusiveEnumField]
	NX_BF_FROZEN_ROT		= NX_BF_FROZEN_ROT_X|NX_BF_FROZEN_ROT_Y|NX_BF_FROZEN_ROT_Z,
	[Engine::Attributes::InclusiveEnumField]
	NX_BF_FROZEN			= NX_BF_FROZEN_POS|NX_BF_FROZEN_ROT,


	/**<summary>
	 Enables kinematic mode for the actor.
	
	Kinematic actors are special dynamic actors that are not 
	influenced by forces (such as gravity), and have no momentum. They are considered to have infinite
	mass and can be moved around the world using the moveGlobal*() methods. They will push 
	regular dynamic actors out of the way. Kinematics will not collide with static or other kinematic objects.
	
	Kinematic actors are great for moving platforms or characters, where direct motion control is desired.

	You can not connect Reduced joints to kinematic actors. Lagrange joints work ok if the platform
	is moving with a relatively low, uniform velocity.

	@see NxActor NxActor.raiseActorFlag()
	</summary>*/
	NX_BF_KINEMATIC			= (1<<7),		//!< Enable kinematic mode for the body.

	/**<summary>
	 Enable debug renderer for this body

	@see NxScene.getDebugRenderable() NxDebugRenderable NxParameter
	</summary>*/
	NX_BF_VISUALIZATION		= (1<<8),

	NX_BF_DUMMY_0			= (1<<9), // deprecated flag placeholder

	/**<summary>
	 Filter velocities used keep body awake. The filter reduces rapid oscillations and transient spikes.
	@see NxActor.isSleeping()
	</summary>*/
	NX_BF_FILTER_SLEEP_VEL	= (1<<10),

	/**<summary>
	 Enables energy-based sleeping algorithm.
	@see NxActor.isSleeping() NxBodyDesc.sleepEnergyThreshold 
	</summary>*/
	NX_BF_ENERGY_SLEEP_TEST	= (1<<11),
};

[Engine::Attributes::MultiEnum]
public enum class ShapesType : unsigned int
{
	NX_STATIC_SHAPES		= (1<<0),								//!< Hits static shapes
	NX_DYNAMIC_SHAPES		= (1<<1),								//!< Hits dynamic shapes
	[Engine::Attributes::InclusiveEnumField]
	NX_ALL_SHAPES			= NX_STATIC_SHAPES|NX_DYNAMIC_SHAPES	//!< Hits both static & dynamic shapes
};

[Engine::Attributes::MultiEnum]
public enum class RaycastBit : unsigned int
{
	NX_RAYCAST_SHAPE		= (1<<0),								//!< "shape" member of #NxRaycastHit is valid
	NX_RAYCAST_IMPACT		= (1<<1),								//!< "worldImpact" member of #NxRaycastHit is valid
	NX_RAYCAST_NORMAL		= (1<<2),								//!< "worldNormal" member of #NxRaycastHit is valid
	NX_RAYCAST_FACE_INDEX	= (1<<3),								//!< "faceID" member of #NxRaycastHit is valid
	NX_RAYCAST_DISTANCE		= (1<<4),								//!< "distance" member of #NxRaycastHit is valid
	NX_RAYCAST_UV			= (1<<5),								//!< "u" and "v" members of #NxRaycastHit are valid
	NX_RAYCAST_FACE_NORMAL	= (1<<6),								//!< Same as NX_RAYCAST_NORMAL but computes a non-smoothed normal
	NX_RAYCAST_MATERIAL		= (1<<7),								//!< "material" member of #NxRaycastHit is valid

	[Engine::Attributes::InclusiveEnumField]
	NX_RAYCAST_ALL = NX_RAYCAST_SHAPE | NX_RAYCAST_IMPACT | NX_RAYCAST_NORMAL | NX_RAYCAST_FACE_INDEX | NX_RAYCAST_DISTANCE | NX_RAYCAST_UV | NX_RAYCAST_FACE_NORMAL | NX_RAYCAST_MATERIAL,
};

[Engine::Attributes::MultiEnum]
public enum class SceneFlags : unsigned int
{
	/**<summary>
	 Used to disable use of SSE in the solver.

	SSE is detected at runtime(on appropriate platforms) and used if present by default.

	However use of SSE can be disabled, even if present, using this flag.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : N/A
	\li PS3  : N/A
	\li XB360: N/A
	</summary>*/
	NX_SF_DISABLE_SSE	=0x1,

	/**<summary>
	 Disable all collisions in a scene. Use the flags NX_AF_DISABLE_COLLISION and NX_SF_DISABLE_COLLISION for disabling collisions between specific actors and shapes.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NX_AF_DISABLE_COLLISION, NX_SF_DISABLE_COLLISION
	</summary>*/
	NX_SF_DISABLE_COLLISIONS	=0x2,

	/**<summary>
	 Perform the simulation in a separate thread.

	By default the SDK runs the physics on a separate thread to the user thread(i.e. the thread which
	calls the API).

	However if this flag is disabled, then the simulation is run in the thread which calls #NxScene::simulate()

	<b>Default:</b> True

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes

	@see NxScene.simulate()
	</summary>*/
	NX_SF_SIMULATE_SEPARATE_THREAD	=0x4,

	/**<summary>
	 Enable internal multi threading.

	This flag enables the multi threading code within the SDK which allows the simulation to
	be divided into tasks for execution on an arbitrary number of threads. 
	
	This is an orthogonal feature to running the simulation in a separate thread, see #NX_SF_SIMULATE_SEPARATE_THREAD.

	\note There may be a small performance penalty for enabling the multi threading code, hence this flag should
	only be enabled if the application intends to use the feature.

	<b>Default:</b> False

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : No
	\li XB360: Yes

	@see NxSceneDesc NX_SF_SIMULATE_SEPARATE_THREAD
	</summary>*/
	NX_SF_ENABLE_MULTITHREAD		=0x8,

	/**<summary>
	 Enable Active Transform Notification.

	This flag enables the the Active Transform Notification feature for a scene.  This
	feature defaults to disabled.  When disabled, the function
	NxScene::getActiveTransforms() will always return a NULL list.

	\note There may be a performance penalty for enabling the Active Transform Notification, hence this flag should
	only be enabled if the application intends to use the feature.

	<b>Default:</b> False

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_SF_ENABLE_ACTIVETRANSFORMS	=0x10,

	/**<summary>
	 Enable Restricted Scene.

	\note Only applies to hardware scenes.

	This flag creates a restricted scene, running the broadphase collision detection on hardware, 
	while limiting the number of actors (see "AGEIA PhysX Hardware Scenes" in the Guide).

	<b>Default:</b> False

	<b>Platform:</b>
	\li PC SW: No
	\li PPU  : Yes
	\li PS3  : No
	\li XB360: No
	</summary>*/
	NX_SF_RESTRICTED_SCENE				=0x20,

	/**<summary>
	 Disable the mutex which serializes scene execution

	Under normal circumstances scene execution is serialized by a mutex. This flag
	can be used to disable this serialization.

	\warning This flag is _experimental_ and in future versions is likely be removed. In favour of 
	completely removing	the mutex.

	If this flag is used the recommended scenario is for use with a software scene and hardware fluid/cloth scene.

	<b>Default:</b> True (change from earlier beta versions)

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_SF_DISABLE_SCENE_MUTEX			=0x40,


	/**<summary>
	 Force the friction model to cone friction

	This ensures that all contacts in the scene will use cone friction, rather than the default
	simplified scheme. This will however have a negative impact on performance in software scenes. Use this
	flag if sliding objects show an affinity for moving along the world axes.

	\note Only applies to software scenes; hardware scenes always force cone friction.
	
	Cone friction may also be activated on an actor-by-actor basis using the NX_AF_FORCE_CONE_FRICTION flag, see #NxActorFlag.

	<b>Default:</b> False

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes (always active)
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_SF_FORCE_CONE_FRICTION			=0x80,


	/**<summary>
	 When set to 1, the compartments are all executed before the primary scene is executed.  This may lower performance
	but it improves interaction quality between compartments and the primary scene.

	<b>Default:</b> False

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: No
	</summary>*/
	NX_SF_SEQUENTIAL_PRIMARY			=0x80*2,

	/**<summary>
	 Enables faster but less accurate fluid collision with static geometry.

	If the flag is set static geometry is considered one simulation step late, which 
	can cause particles to leak through static geometry. In order to prevent this, 
	NxFluidDesc.collisionDistanceMultiplier can be increased.
	
	<b>Default:</b> False

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_SF_FLUID_PERFORMANCE_HINT		=0x80*4,
	
};

[Engine::Attributes::MultiEnum]
public enum class ShapeFlag : unsigned int
{
	/**<summary>
	 Trigger callback will be called when a shape enters the trigger volume.

	@see NxUserTriggerReport NxScene.setUserTriggerReport()
	</summary>*/
	NX_TRIGGER_ON_ENTER				= (1<<0),
	
	/**<summary>
	 Trigger callback will be called after a shape leaves the trigger volume.

	@see NxUserTriggerReport NxScene.setUserTriggerReport()
	</summary>*/
	NX_TRIGGER_ON_LEAVE				= (1<<1),
	
	/**<summary>
	 Trigger callback will be called while a shape is intersecting the trigger volume.

	@see NxUserTriggerReport NxScene.setUserTriggerReport()
	</summary>*/
	NX_TRIGGER_ON_STAY				= (1<<2),

	/**<summary>
	 Combination of all the other trigger enable flags.

	@see NxUserTriggerReport NxScene.setUserTriggerReport()
	</summary>*/
	NX_TRIGGER_ENABLE				= NX_TRIGGER_ON_ENTER|NX_TRIGGER_ON_LEAVE|NX_TRIGGER_ON_STAY,

	/**<summary>
	 Enable debug renderer for this shape

	@see NxScene.getDebugRenderable() NxDebugRenderable NxParameter
	</summary>*/
	NX_SF_VISUALIZATION				= (1<<3),

	/**<summary>
	 Disable collision detection for this shape (counterpart of NX_AF_DISABLE_COLLISION)

	\warning IMPORTANT: this is only used for compound objects! Use NX_AF_DISABLE_COLLISION otherwise.
	</summary>*/
	NX_SF_DISABLE_COLLISION			= (1<<4),

	/**<summary>
	 Enable feature indices in contact stream.

	@see NxUserContactReport NxContactStreamIterator NxContactStreamIterator.getFeatureIndex0()
	</summary>*/
	NX_SF_FEATURE_INDICES			= (1<<5),

	/**<summary>
	 Disable raycasting for this shape
	</summary>*/
	NX_SF_DISABLE_RAYCASTING		= (1<<6),

	/**<summary>
	 Enable contact force reporting per contact point in contact stream (otherwise we only report force per actor pair)
	</summary>*/
	NX_SF_POINT_CONTACT_FORCE		= (1<<7),

	NX_SF_FLUID_DRAIN				= (1<<8),	//!< Sets the shape to be a fluid drain.
	NX_SF_FLUID_DISABLE_COLLISION	= (1<<10),	//!< Disable collision with fluids.
	NX_SF_FLUID_TWOWAY				= (1<<11),	//!< Enables the reaction of the shapes actor on fluid collision.

	/**<summary>
	 Disable collision response for this shape (counterpart of NX_AF_DISABLE_RESPONSE)

	\warning not supported by cloth / soft bodies
	</summary>*/
	NX_SF_DISABLE_RESPONSE			= (1<<12),

	/**<summary>
	 Enable dynamic-dynamic CCD for this shape. Used only when CCD is globally enabled and shape have a CCD skeleton.
	</summary>*/
	NX_SF_DYNAMIC_DYNAMIC_CCD		= (1<<13),

	/**<summary>
	 Disable participation in ray casts, overlap tests and sweeps.

	NOTE: Setting this flag for static non-trigger shapes may cause incorrect behavior for 
	colliding fluid and cloth.
	</summary>*/
	NX_SF_DISABLE_SCENE_QUERIES			= (1<<14),

	NX_SF_CLOTH_DRAIN					= (1<<15),	//!< Sets the shape to be a cloth drain.
	NX_SF_CLOTH_DISABLE_COLLISION		= (1<<16),	//!< Disable collision with cloths.

	/**<summary>
	  Enables the reaction of the shapes actor on cloth collision.
	\warning Compound objects cannot use a different value for each constituent shape.
	</summary>*/
	NX_SF_CLOTH_TWOWAY					= (1<<17),	

	NX_SF_SOFTBODY_DRAIN				= (1<<18),	//!< Sets the shape to be a soft body drain.
	NX_SF_SOFTBODY_DISABLE_COLLISION	= (1<<19),	//!< Disable collision with soft bodies.

	/**<summary>
	  Enables the reaction of the shapes actor on soft body collision.
	\warning Compound objects cannot use a different value for each constituent shape.
	</summary>*/
	NX_SF_SOFTBODY_TWOWAY				= (1<<20),
};

/**<summary>
 Flags which describe the format and behavior of a convex mesh.
</summary>*/
[Engine::Attributes::MultiEnum]
public enum class ConvexFlags : unsigned int
{
	/**<summary>
	 Used to flip the normals if the winding order is reversed.

	The Nx libraries assume that the face normal of a triangle with vertices [a,b,c] can be computed as:
	edge1 = b-a
	edge2 = c-a
	face_normal = edge1 x edge2.

	Note: this is the same as counterclockwise winding in a right handed graphics coordinate system.

	If this does not match the winding order for your triangles, raise the below flag.
	</summary>*/
	NX_CF_FLIPNORMALS		=	(1<<0),

	/**<summary>
	Denotes the use of 16-bit vertex indices in NxConvexMeshDesc::triangles.
	(otherwise, 32-bit indices are assumed)
	@see #NxConvexMeshDesc.triangles
	</summary>*/
	NX_CF_16_BIT_INDICES	=	(1<<1),

	/**<summary>
	Automatically recomputes the hull from the vertices. If this flag is not set, you must provide the entire geometry manually.
	</summary>*/
	NX_CF_COMPUTE_CONVEX	=	(1<<2),	

	/**<summary>
	 Inflates the convex object according to skin width

	\note This flag is only used in combination with NX_CF_COMPUTE_CONVEX.

	@see NxCookingParams
	</summary>*/
	NX_CF_INFLATE_CONVEX	=	(1<<3),

	/**<summary>
	 Instructs cooking to save normals uncompressed.  The cooked hull data will be larger, but will load faster.

	@see NxCookingParams
	</summary>*/
	NX_CF_USE_UNCOMPRESSED_NORMALS	=	(1<<5),
};

[Engine::Attributes::MultiEnum]
public enum class MeshShapeFlag : unsigned int
{
	/**<summary>
	 Select between "normal" or "smooth" sphere-mesh/raycastcapsule-mesh contact generation routines.

	The 'normal' algorithm assumes that the mesh is composed from flat triangles. 
	When a ball rolls or a raycast capsule slides along the mesh surface, it will experience small,
	sudden changes in velocity as it rolls from one triangle to the next. The smooth algorithm, on
	the other hand, assumes that the triangles are just an approximation of a surface that is smooth.  
	It uses the Gouraud algorithm to smooth the triangles' vertex normals (which in this 
	case are particularly important). This way the rolling sphere's/capsule's velocity will change
	smoothly over time, instead of suddenly. We recommend this algorithm for simulating car wheels
	on a terrain.

	@see NxSphereShape NxTriangleMeshShape NxHeightFieldShape
	</summary>*/
	NX_MESH_SMOOTH_SPHERE_COLLISIONS	= (1<<0),
	NX_MESH_DOUBLE_SIDED				= (1<<1)	//!< The mesh is double-sided. This is currently only used for raycasting.
};

[Engine::Attributes::MultiEnum]
public enum class JointFlag : unsigned int
{
	/**<summary>
	 Raised if collision detection should be enabled between the bodies attached to the joint.

	By default collision constraints are not generated between pairs of bodies which are connected by joints.
	</summary>*/
    NX_JF_COLLISION_ENABLED	= (1<<0),

	/**<summary>
	 Enable debug renderer for this joint

	@see NxScene.getDebugRenderable() NxDebugRenderable NxParameter
	</summary>*/
    NX_JF_VISUALIZATION		= (1<<1),
};

[Engine::Attributes::MultiEnum]
public enum class D6JointDriveType : unsigned int
{
	/**<summary>
	 Used to set a position goal when driving.

	Note: the appropriate target positions/orientations should be set.

	@see NxD6JointDesc.xDrive NxD6Joint.swingDrive NxD6JointDesc.drivePosition
	</summary>*/
	NX_D6JOINT_DRIVE_POSITION	= 1<<0,

	/**<summary>
	 Used to set a velocity goal when driving.

	Note: the appropriate target velocities should beset.

	@see NxD6JointDesc.xDrive NxD6Joint.swingDrive NxD6JointDesc.driveLinearVelocity
	</summary>*/
	NX_D6JOINT_DRIVE_VELOCITY	= 1<<1
};

/**<summary>
 Used to specify the range of motions allowed for a DOF in a D6 joint.

@see NxD6Joint NxD6JointDesc
@see NxD6Joint.xMotion NxD6Joint.swing1Motion
</summary>*/
[Engine::Attributes::SingleEnum]
public enum class D6JointMotion : unsigned int
{
	NX_D6JOINT_MOTION_LOCKED,	//!< The DOF is locked, it does not allow relative motion.
	NX_D6JOINT_MOTION_LIMITED,  //!< The DOF is limited, it only allows motion within a specific range.
	NX_D6JOINT_MOTION_FREE		//!< The DOF is free and has its full range of motions.
};

/**<summary>
 Joint projection modes.

Joint projection is a method for correcting large joint errors.

It is also necessary to set the distance above which projection occurs.

@see NxRevoluteJointDesc.projectionMode NxRevoluteJointDesc.projectionDistance NxRevoluteJointDesc.projectionAngle
@see NxSphericalJointDesc.projectionMode
@see NxD6JointDesc.projectionMode
</summary>*/
[Engine::Attributes::SingleEnum]
public enum class JointProjectionMode : unsigned int
{
	NX_JPM_NONE  = 0,				//!< don't project this joint
	NX_JPM_POINT_MINDIST = 1,		//!< linear and angular minimum distance projection
	NX_JPM_LINEAR_MINDIST = 2,		//!< linear only minimum distance projection
	//there may be more modes later
};

/**<summary>
 Flags which control the general behavior of D6 joints.

@see NxD6Joint NxD6JointDesc NxD6JointDesc.flags
</summary>*/
[Engine::Attributes::MultiEnum]
public enum class D6JointFlag : unsigned int
{
	/**<summary>
	 Drive along the shortest spherical arc.

	@see NxD6JointDesc.slerpDrive
	</summary>*/
	NX_D6JOINT_SLERP_DRIVE = 1<<0,
	/**<summary>
   	 Apply gearing to the angular motion, e.g. body 2s angular motion is twice body 1s etc.
   
   	@see NxD6JointDesc.gearRatio
   	</summary>*/
	NX_D6JOINT_GEAR_ENABLED = 1<<1
};

/**<summary>
 Flags which control the behavior of distance joints.

@see NxDistanceJoint NxDistanceJointDesc NxDistanceJointDesc.flags
</summary>*/
[Engine::Attributes::MultiEnum]
public enum class DistanceJointFlag : unsigned int
{
	/**<summary>
	 true if the joint enforces the maximum separate distance.
	</summary>*/
	NX_DJF_MAX_DISTANCE_ENABLED = 1 << 0,

	/**<summary>
	 true if the joint enforces the minimum separate distance.
	</summary>*/
	NX_DJF_MIN_DISTANCE_ENABLED = 1 << 1,

	/**<summary>
	 true if the spring is enabled
	
	@see NxDistanceJointDesc.spring
	</summary>*/
	NX_DJF_SPRING_ENABLED		= 1 << 2,
};

/**<summary>
 Flags to control the behavior of pulley joints.

@see NxPulleyJoint NxPulleyJointDesc NxPulleyJoint.setFlags()
</summary>*/
[Engine::Attributes::MultiEnum]
public enum class PulleyJointFlag : unsigned int
{
	/**<summary>
	 true if the joint also maintains a minimum distance, not just a maximum.
	</summary>*/
	NX_PJF_IS_RIGID = 1 << 0,

	/**<summary>
	 true if the motor is enabled

	@see NxPulleyJointDesc.motor
	</summary>*/
	NX_PJF_MOTOR_ENABLED = 1 << 1
};

/**<summary>
 Flags to control the behavior of revolute joints.

@see NxRevoluteJoint NxRevoluteJointDesc
</summary>*/
[Engine::Attributes::MultiEnum]
public enum class RevoluteJointFlag : unsigned int
{
	/**<summary>
	 true if limits is enabled

	@see NxRevoluteJointDesc.limit
	</summary>*/
	NX_RJF_LIMIT_ENABLED = 1 << 0,
	
	/**<summary>
	 true if the motor is enabled

	@see NxRevoluteJoint.motor
	</summary>*/
	NX_RJF_MOTOR_ENABLED = 1 << 1,
	
	/**<summary>
	 true if the spring is enabled. The spring will only take effect if the motor is disabled.

	@see NxRevoluteJoint.spring
	</summary>*/
	NX_RJF_SPRING_ENABLED = 1 << 2,
};

/**<summary>
 Flags which control the behavior of spherical joints.

@see NxSphericalJoint NxSphericalJointDesc NxSphericalJointDesc.flags
</summary>*/
[Engine::Attributes::MultiEnum]
public enum class SphericalJointFlag : unsigned int
{
	/**<summary>
	 true if the twist limit is enabled

	@see NxSphericalJointDesc.twistLimit
	</summary>*/
	NX_SJF_TWIST_LIMIT_ENABLED = 1 << 0,
	
	/**<summary>
	 true if the swing limit is enabled

	@see NxSphericalJointDesc.swingLimit
	</summary>*/
	NX_SJF_SWING_LIMIT_ENABLED = 1 << 1,
	
	/**<summary>
	 true if the twist spring is enabled

	@see NxSphericalJointDesc.twistSpring
	</summary>*/
	NX_SJF_TWIST_SPRING_ENABLED= 1 << 2,
	
	/**<summary>
	 true if the swing spring is enabled

	@see NxSphericalJointDesc.swingSpring
	</summary>*/
	NX_SJF_SWING_SPRING_ENABLED= 1 << 3,
	
	/**<summary>
	 true if the joint spring is enabled

	@see NxSphericalJointDesc.jointSpring

	</summary>*/
	NX_SJF_JOINT_SPRING_ENABLED= 1 << 4,

	/**<summary>
	 Add additional constraints to linear movement.

	Constrain movements along directions perpendicular to the distance vector defined by the two anchor points.

	\note Setting this flag can increase the stability of the joint but the computation will be more expensive.
	</summary>*/
    NX_SJF_PERPENDICULAR_DIR_CONSTRAINTS	= 1 << 5,
};

/**<summary>
 Describes the state of the joint.

@see NxJoint
</summary>*/
[Engine::Attributes::SingleEnum]
public enum class JointState : unsigned int
{
	/**<summary>
	 Not used.
	</summary>*/
	NX_JS_UNBOUND,

	/**<summary>
	 The joint is being simulated under normal conditions. I.e. it is not broken or deleted.
	</summary>*/
	NX_JS_SIMULATING,

	/**<summary>
	 Set when the joint has been broken or one of the actors connected to the joint has been remove.
	@see NxUserNotify.onJointBreak() NxJoint.setBreakable()
	</summary>*/
	NX_JS_BROKEN
};

/**<summary>
 Identifies each type of joint.

@see NxJoint NxJointDesc NxScene.createJoint()
</summary>*/
[Engine::Attributes::SingleEnum]
public enum class JointType : unsigned int
{
	/**<summary>
	 Permits a single translational degree of freedom.

	@see NxPrismaticJoint
	</summary>*/
	NX_JOINT_PRISMATIC,

	/**<summary>
	 Also known as a hinge joint, permits one rotational degree of freedom.

	@see NxRevoluteJoint
	</summary>*/
	NX_JOINT_REVOLUTE,

	/**<summary>
	 Formerly known as a sliding joint, permits one translational and one rotational degree of freedom.

	@see NxCylindricalJoint
	</summary>*/
	NX_JOINT_CYLINDRICAL,

	/**<summary>
	 Also known as a ball or ball and socket joint.

	@see NxSphericalJoint
	</summary>*/
	NX_JOINT_SPHERICAL,

	/**<summary>
	 A point on one actor is constrained to stay on a line on another.

	@see NxPointOnLineJoint
	</summary>*/
	NX_JOINT_POINT_ON_LINE,

	/**<summary>
	 A point on one actor is constrained to stay on a plane on another.

	@see NxPointInPlaneJoint
	</summary>*/
	NX_JOINT_POINT_IN_PLANE,

	/**<summary>
	 A point on one actor maintains a certain distance range to another point on another actor.

	@see NxDistanceJoint
	</summary>*/
	NX_JOINT_DISTANCE,

	/**<summary>
	 A pulley joint.

	@see NxPulleyJoint
	</summary>*/
	NX_JOINT_PULLEY,

	/**<summary>
	 A "fixed" connection.

	@see NxFixedJoint
	</summary>*/
	NX_JOINT_FIXED,

	/**<summary>
	 A 6 degree of freedom joint

	@see NxD6Joint
	</summary>*/
	NX_JOINT_D6,

	NX_JOINT_COUNT,				//!< Just to track the number of available enum values. Not a joint type.
	NX_JOINT_FORCE_DWORD = 0x7fffffff
};

/**<summary>
 Enum with flag values to be used in NxSimpleTriangleMesh::flags.
</summary>*/
[Engine::Attributes::MultiEnum]
public enum class MeshFlags : unsigned int
{
	/**<summary>
	 Specifies if the SDK should flip normals.

	The Nx libraries assume that the face normal of a triangle with vertices [a,b,c] can be computed as:
	edge1 = b-a
	edge2 = c-a
	face_normal = edge1 x edge2.

	Note: This is the same as a counterclockwise winding in a right handed coordinate system or
	alternatively a clockwise winding order in a left handed coordinate system.

	If this does not match the winding order for your triangles, raise the below flag.
	</summary>*/
	NX_MF_FLIPNORMALS		=	(1<<0),
	NX_MF_16_BIT_INDICES	=	(1<<1),	//<! Denotes the use of 16-bit vertex indices
	NX_MF_HARDWARE_MESH		=	(1<<2),	//<! The mesh will be used in hardware scenes
};

[Engine::Attributes::SingleEnum]
public enum class SimulationType : unsigned int
{
	NX_SIMULATION_SW	= 0,		//!< Create a software master scene.
	NX_SIMULATION_HW	= 1,		//!< Create a hardware master scene.

	NX_STY_FORCE_DWORD = 0x7fffffff			//!< Just to make sure sizeof(enum) == 4, not a valid value.
};

[Engine::Attributes::SingleEnum]
public enum class HWVersion : unsigned int
{
	NX_HW_VERSION_NONE = 0,
	NX_HW_VERSION_ATHENA_1_0 = 1
};

[Engine::Attributes::SingleEnum]
public enum class PhysParameter : unsigned int
{
/* RigidBody-related parameters  </summary>*/
	
	/**<summary>
	 DEPRECATED! Do not use!

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_PENALTY_FORCE			= 0,
	
	/**<summary>
	 Default value for ::NxShapeDesc::skinWidth, see for more info.
	
	<b>Range:</b> [0, inf)
	<b>Default:</b> 0.025
	<b>Unit:</b> distance.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxShapeDesc.skinWidth
	</summary>*/
	NX_SKIN_WIDTH = 1,

	
	/**<summary>
	 The default linear velocity, squared, below which objects start going to sleep. 
	Note: Only makes sense when the NX_BF_ENERGY_SLEEP_TEST is not set.
	
	<b>Range:</b> [0, inf)
	<b>Default:</b> (0.15*0.15)

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes (Sleep behavior on hardware is different)
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_DEFAULT_SLEEP_LIN_VEL_SQUARED = 2,
	
	/**<summary>
	 The default angular velocity, squared, below which objects start going to sleep. 
	Note: Only makes sense when the NX_BF_ENERGY_SLEEP_TEST is not set.
	
	<b>Range:</b> [0, inf)
	<b>Default:</b> (0.14*0.14)
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes (Sleep behavior on hardware is different)
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_DEFAULT_SLEEP_ANG_VEL_SQUARED = 3,

	
	/**<summary>
	 A contact with a relative velocity below this will not bounce.	
	
	<b>Range:</b> (-inf, 0]
	<b>Default:</b> -2

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxMaterial
	</summary>*/
	NX_BOUNCE_THRESHOLD = 4,

	/**<summary>
	 This lets the user scale the magnitude of the dynamic friction applied to all objects.	
	
	<b>Range:</b> [0, inf)
	<b>Default:</b> 1

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxMaterial
	</summary>*/
	NX_DYN_FRICT_SCALING = 5,
	
	/**<summary>
	 This lets the user scale the magnitude of the static friction applied to all objects.
	
	<b>Range:</b> [0, inf)
	<b>Default:</b> 1
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxMaterial
	</summary>*/
	NX_STA_FRICT_SCALING = 6,

	
	/**<summary>
	 See the comment for NxActor::setMaxAngularVelocity() for details.
	
	<b>Range:</b> [0, inf)
	<b>Default:</b> 7

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxBodyDesc.setMaxAngularVelocity()
	</summary>*/
	NX_MAX_ANGULAR_VELOCITY = 7,

/* Collision-related parameters:  </summary>*/

	
	/**<summary>
	 Enable/disable continuous collision detection (0.0f to disable)

	<b>Range:</b> [0, inf)
	<b>Default:</b> 0.0

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxPhysicsSDK.createCCDSkeleton()
	</summary>*/
	NX_CONTINUOUS_CD = 8,
	
	/**<summary>
	 This overall visualization scale gets multiplied with the individual scales. Setting to zero turns ignores all visualizations. Default is 0.

	The below settings permit the debug visualization of various simulation properties. 
	The setting is either zero, in which case the property is not drawn. Otherwise it is a scaling factor
	that determines the size of the visualization widgets.

	Only bodies and joints for which visualization is turned on using setFlag(VISUALIZE) are visualized.
	Contacts are visualized if they involve a body which is being visualized.
	Default is 0.

	Notes:
	- to see any visualization, you have to set NX_VISUALIZATION_SCALE to nonzero first.
	- the scale factor has been introduced because it's difficult (if not impossible) to come up with a
	good scale for 3D vectors. Normals are normalized and their length is always 1. But it doesn't mean
	we should render a line of length 1. Depending on your objects/scene, this might be completely invisible
	or extremely huge. That's why the scale factor is here, to let you tune the length until it's ok in
	your scene.
	- however, things like collision shapes aren't ambiguous. They are clearly defined for example by the
	triangles and polygons themselves, and there's no point in scaling that. So the visualization widgets
	are only scaled when it makes sense.

	<b>Range:</b> [0, inf)
	<b>Default:</b> 0

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes (only a subset of visualizations are supported)
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZATION_SCALE = 9,

	
	/**<summary>
	 Visualize the world axes.
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_WORLD_AXES = 10,
	
/* Body visualizations </summary>*/

	/**<summary>
	 Visualize a bodies axes.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxActor.globalPose NxActor
	</summary>*/
	NX_VISUALIZE_BODY_AXES = 11,
	
	/**<summary>
	 Visualize a body's mass axes.

	This visualization is also useful for visualizing the sleep state of bodies. Sleeping bodies are drawn in
	black, while awake bodies are drawn in white. If the body is sleeping and part of a sleeping group, it is
	drawn in red.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxBodyDesc.massLocalPose NxActor
	</summary>*/
	NX_VISUALIZE_BODY_MASS_AXES = 12,
	
	/**<summary>
	 Visualize the bodies linear velocity.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxBodyDesc.linearVelocity NxActor
	</summary>*/
	NX_VISUALIZE_BODY_LIN_VELOCITY = 13,
	
	/**<summary>
	 Visualize the bodies angular velocity.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxBodyDesc.angularVelocity NxActor
	</summary>*/
	NX_VISUALIZE_BODY_ANG_VELOCITY = 14,
	
/*
	Obsolete parameters
	NX_VISUALIZE_BODY_LIN_MOMENTUM = 15,
	NX_VISUALIZE_BODY_ANG_MOMENTUM = 16,
	NX_VISUALIZE_BODY_LIN_ACCEL = 17,
	NX_VISUALIZE_BODY_ANG_ACCEL = 18,
	NX_VISUALIZE_BODY_LIN_FORCE = 19,
	NX_VISUALIZE_BODY_ANG_FORCE = 20,
	NX_VISUALIZE_BODY_REDUCED = 21,
</summary>*/

	/**<summary>
	 Visualize joint groups 
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_BODY_JOINT_GROUPS = 22,

/* Obsolete parameters
	NX_VISUALIZE_BODY_CONTACT_LIST = 23,
	NX_VISUALIZE_BODY_JOINT_LIST = 24,
	NX_VISUALIZE_BODY_DAMPING = 25,
	NX_VISUALIZE_BODY_SLEEP = 26,
</summary>*/

/* Joint visualisations </summary>*/
	/**<summary>
	 Visualize local joint axes (including drive targets, if any)

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes

	@see NxJointDesc.localAxis NxJoint
	</summary>*/
	NX_VISUALIZE_JOINT_LOCAL_AXES = 27,
	
	/**<summary>
	 Visualize joint world axes.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes

	@see NxJoint
	</summary>*/
	NX_VISUALIZE_JOINT_WORLD_AXES = 28,
	
	/**<summary>
	 Visualize joint limits.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes

	@see NxJoint
	</summary>*/
	NX_VISUALIZE_JOINT_LIMITS = 29,
	
/* Obsolete parameters
	NX_VISUALIZE_JOINT_ERROR = 30,
	NX_VISUALIZE_JOINT_FORCE = 31,
	NX_VISUALIZE_JOINT_REDUCED = 32,
</summary>*/

/* Contact visualisations </summary>*/

	/**<summary>
	  Visualize contact points. Will enable contact information.
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_CONTACT_POINT = 33,
	
	/**<summary>
	 Visualize contact normals. Will enable contact information.
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_CONTACT_NORMAL = 34,
	
	/**<summary>
	  Visualize contact errors. Will enable contact information.
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_CONTACT_ERROR = 35,
	
	/**<summary>
	 Visualize Contact forces. Will enable contact information.
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_CONTACT_FORCE = 36,

	
	/**<summary>
	 Visualize actor axes.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxActor
	</summary>*/
	NX_VISUALIZE_ACTOR_AXES = 37,

	
	/**<summary>
	 Visualize bounds (AABBs in world space)
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_COLLISION_AABBS = 38,
	
	/**<summary>
	 Shape visualization

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes

	@see NxShape
	</summary>*/
	NX_VISUALIZE_COLLISION_SHAPES = 39,
	
	/**<summary>
	 Shape axis visualization

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes

	@see NxShape
	</summary>*/
	NX_VISUALIZE_COLLISION_AXES = 40,
	
	/**<summary>
	 Compound visualization (compound AABBs in world space)
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_COLLISION_COMPOUNDS = 41,
	
	/**<summary>
	 Mesh and convex vertex normals

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes

	@see NxTriangleMesh NxConvexMesh
	</summary>*/
	NX_VISUALIZE_COLLISION_VNORMALS = 42,
	
	/**<summary>
	 Mesh and convex face normals

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes

	@see NxTriangleMesh NxConvexMesh
	</summary>*/
	NX_VISUALIZE_COLLISION_FNORMALS = 43,
	
	/**<summary>
	 Active edges for meshes

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes

	@see NxTriangleMesh
	</summary>*/
	NX_VISUALIZE_COLLISION_EDGES = 44,
	
	/**<summary>
	 Bounding spheres
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_COLLISION_SPHERES = 45,
	
/* Obsolete parameter
	NX_VISUALIZE_COLLISION_SAP = 46,
</summary>*/
	
	/**<summary>
	 Static pruning structures
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_COLLISION_STATIC = 47,
	
	/**<summary>
	 Dynamic pruning structures
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_COLLISION_DYNAMIC = 48,
	
	/**<summary>
	 "Free" pruning structures
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_COLLISION_FREE = 49,
	
	/**<summary>
	 Visualize the CCD tests

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxPhysicsSDK.createCCDSkeleton()
	</summary>*/
	NX_VISUALIZE_COLLISION_CCD = 50,	
	
	/**<summary>
	 Visualize CCD Skeletons

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxPhysicsSDK.createCCDSkeleton()
	</summary>*/
	NX_VISUALIZE_COLLISION_SKELETONS = 51,

/* Fluid visualizations </summary>*/
	/**<summary>
	 Emitter visualization.
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_FLUID_EMITTERS = 52,
	
	/**<summary>
	 Particle position visualization.
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_FLUID_POSITION = 53,
	
	/**<summary>
	 Particle velocity visualization.
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_FLUID_VELOCITY = 54,
	
	/**<summary>
	 Particle kernel radius visualization.
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_FLUID_KERNEL_RADIUS = 55,
	
	/**<summary>
	 Fluid AABB visualization.
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_VISUALIZE_FLUID_BOUNDS = 56,

	/**<summary>
	 Fluid Packet visualization.
	</summary>*/
	NX_VISUALIZE_FLUID_PACKETS = 57,
	
	/**<summary>
	 Fluid motion limit visualization.
	</summary>*/
	NX_VISUALIZE_FLUID_MOTION_LIMIT = 58,

	/**<summary>
	 Fluid dynamic collision visualization.
	</summary>*/
	NX_VISUALIZE_FLUID_DYN_COLLISION = 59,

	/**<summary>
	 Fluid static collision visualization.
	</summary>*/
	NX_VISUALIZE_FLUID_STC_COLLISION = 60,

	/**<summary>
	 Fluid available mesh packets visualization.
	</summary>*/
	NX_VISUALIZE_FLUID_MESH_PACKETS = 61,

	/**<summary>
	 Fluid drain shape visualization.
	</summary>*/
	NX_VISUALIZE_FLUID_DRAINS = 62,

	/**<summary>
	 Fluid Packet Data visualization.
	</summary>*/
	NX_VISUALIZE_FLUID_PACKET_DATA = 90,

/* Cloth visualizations </summary>*/
	/**<summary>
	 Cloth mesh visualization.
	</summary>*/
	NX_VISUALIZE_CLOTH_MESH = 63,

	/**<summary>
	 Cloth rigid body collision visualization.
	</summary>*/
	NX_VISUALIZE_CLOTH_COLLISIONS = 64,

	/**<summary>
	 Cloth cloth collision visualization.
	</summary>*/
	NX_VISUALIZE_CLOTH_SELFCOLLISIONS = 65,
	
	/**<summary>
	 Cloth clustering for the PPU simulation visualization.
	</summary>*/
	NX_VISUALIZE_CLOTH_WORKPACKETS = 66,
	
	/**<summary>
	 Cloth overall sleeping visualization.
	</summary>*/
	NX_VISUALIZE_CLOTH_SLEEP = 67,

	/**<summary>
	 Cloth per vertex sleeping visualization.
	</summary>*/
	NX_VISUALIZE_CLOTH_SLEEP_VERTEX = 94,

	/**<summary>
	 Cloth tearable vertices visualization.
	</summary>*/
	NX_VISUALIZE_CLOTH_TEARABLE_VERTICES = 80,

	/**<summary>
	 Cloth tearing visualization.
	</summary>*/
	NX_VISUALIZE_CLOTH_TEARING = 81,

	/**<summary>
	 Cloth attachments visualization.
	</summary>*/
	NX_VISUALIZE_CLOTH_ATTACHMENT = 82,

	/**<summary>
	 Cloth valid bounds visualization.
	</summary>*/
	NX_VISUALIZE_CLOTH_VALIDBOUNDS = 92,

/* SoftBody visualizations </summary>*/
	/**<summary>
	 Soft body mesh visualization.
	</summary>*/
	NX_VISUALIZE_SOFTBODY_MESH = 83,

	/**<summary>
	 Soft body rigid body collision visualization.
	</summary>*/
	NX_VISUALIZE_SOFTBODY_COLLISIONS = 84,

	/**<summary>
	 Soft body clustering for the PPU simulation visualization.
	</summary>*/
	NX_VISUALIZE_SOFTBODY_WORKPACKETS = 85,

	/**<summary>
	 Soft body overall sleeping visualization.
	</summary>*/
	NX_VISUALIZE_SOFTBODY_SLEEP = 86,

	/**<summary>
	 Soft body per vertex sleeping visualization.
	</summary>*/
	NX_VISUALIZE_SOFTBODY_SLEEP_VERTEX = 95,

	/**<summary>
	 Soft body tearable vertices visualization.
	</summary>*/
	NX_VISUALIZE_SOFTBODY_TEARABLE_VERTICES = 87,

	/**<summary>
	 Soft body tearing visualization.
	</summary>*/
	NX_VISUALIZE_SOFTBODY_TEARING = 88,

	/**<summary>
	 Soft body attachments visualization.
	</summary>*/
	NX_VISUALIZE_SOFTBODY_ATTACHMENT = 89,

	/**<summary>
	 Soft body valid bounds visualization.
	</summary>*/
	NX_VISUALIZE_SOFTBODY_VALIDBOUNDS = 93,

/* General parameters and new parameters </summary>*/

	/**<summary>
	 Used to enable adaptive forces to accelerate convergence of the solver.

	<b>Range:</b> [0, inf)
	<b>Default:</b> 1.0
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_ADAPTIVE_FORCE = 68,
	
	/**<summary>
	 Controls default filtering for jointed bodies. True means collision is disabled.

	<b>Range:</b> {true, false}
	<b>Default:</b> true

	@see NX_JF_COLLISION_ENABLED
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_COLL_VETO_JOINTED = 69,
	
	/**<summary>
	 Controls whether two touching triggers generate a callback or not.

	<b>Range:</b> {true, false}
	<b>Default:</b> true

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No
	\li PS3  : Yes
	\li XB360: Yes

	@see NxUserTriggerReport
	</summary>*/
	NX_TRIGGER_TRIGGER_CALLBACK = 70,
	
	/**<summary>
	 Internal, used for debugging and testing. Not to be used.
	</summary>*/
	NX_SELECT_HW_ALGO = 71,
	
	/**<summary>
	 Internal, used for debugging and testing. Not to be used.
	</summary>*/
	NX_VISUALIZE_ACTIVE_VERTICES = 72,

	/**<summary>
	 Distance epsilon for the CCD algorithm.  

	<b>Range:</b> [0, inf)
	<b>Default:</b> 0.01
	
	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_CCD_EPSILON = 73,

	/**<summary>
	 Used to accelerate solver.

	<b>Range:</b> [0, inf)
	<b>Default:</b> 0
	
	<b>Platform:</b>
	\li PC SW: No
	\li PPU  : Yes
	\li PS3  : No
	\li XB360: No
	</summary>*/
	NX_SOLVER_CONVERGENCE_THRESHOLD = 74,

	/**<summary>
	 Used to accelerate HW Broad Phase.
	
	<b>Range:</b> [0, inf)
	<b>Default:</b> 0.001

	<b>Platform:</b>
	\li PC SW: No
	\li PPU  : Yes
	\li PS3  : No
	\li XB360: No
	</summary>*/
	NX_BBOX_NOISE_LEVEL = 75,

	/**<summary>
	 Used to set the sweep cache size.

	<b>Range:</b> [0, inf)
	<b>Default:</b> 5.0

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_IMPLICIT_SWEEP_CACHE_SIZE = 76,

	/**<summary>
	 The default sleep energy threshold. Objects with an energy below this threshold are allowed
	to go to sleep. 
	Note: Only used when the NX_BF_ENERGY_SLEEP_TEST flag is set.

	<b>Range:</b> [0, inf)
	<b>Default:</b> 0.005

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_DEFAULT_SLEEP_ENERGY = 77,

	/**<summary>
	 Constant for the maximum number of packets per fluid. Used to compute the fluid packet buffer size in NxFluidPacketData.

	<b>Range:</b> [925, 925]
	<b>Default:</b> 925

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxFluidPacketData
	</summary>*/
	NX_CONSTANT_FLUID_MAX_PACKETS = 78,

	/**<summary>
	 Constant for the maximum number of new fluid particles per frame.
	<b>Range:</b> [4096, 4096]
	<b>Default:</b> 4096

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_CONSTANT_FLUID_MAX_PARTICLES_PER_STEP = 79,

	/**<summary>
	 Force field visualization.
	</summary>*/
	NX_VISUALIZE_FORCE_FIELDS = 91,

	/**<summary>
	 [Experimental] Disables scene locks when creating/releasing meshes.

	Prevents the SDK from locking all scenes when creating and releasing triangle meshes, convex meshes, height field 
	meshes, softbody meshes and cloth meshes, which is the default behavior. Can be used to improve parallelism but beware
	of possible side effects.

	\warning Experimental feature.
	</summary>*/
	NX_ASYNCHRONOUS_MESH_CREATION = 96,

	/**<summary>
	 Epsilon for custom force field kernels.
	
	This epsilon is used in custom force field kernels (NxSwTarget). Methods like recip()
	or recipSqrt() evaluate to zero if their input is smaller than this epsilon.
	
	
	</summary>*/
	NX_FORCE_FIELD_CUSTOM_KERNEL_EPSILON = 97,

	/**<summary>
	 Enable/disable improved spring solver for joints and wheelshapes
	
	This parameter allows to enable/disable an improved version of the spring solver for joints and wheelshapes.

	\warning 
	The parameter is introduced for legacy purposes only and will be removed in future versions (such that
	the improved spring solver will always be used).

	\note 
	Changing the parameter will only affect newly created scenes but not existing ones.

	<b>Range:</b> {0: disabled, 1: enabled}
	<b>Default:</b> 1

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : No (disabled by default)
	\li PS3  : Yes
	\li XB360: Yes
	</summary>*/
	NX_IMPROVED_SPRING_SOLVER = 98,

	/**<summary>
	 This is not a parameter, it just records the current number of parameters (as max(NxParameter)+1) for use in loops.
	When a new parameter is added this number should be assigned to it and then incremented.
	</summary>*/
	NX_PARAMS_NUM_VALUES = 99,

	NX_PARAMS_FORCE_DWORD = 0x7fffffff
};

[Engine::Attributes::SingleEnum]
public enum class PhysPlatform : unsigned int
{
	PLATFORM_PC,
	PLATFORM_XENON,
	PLATFORM_PLAYSTATION3
};

[Engine::Attributes::MultiEnum]
public enum class PhysMaterialFlag : unsigned int
{
	NX_MF_ANISOTROPIC = 1 << 0,
	NX_MF_DISABLE_FRICTION = 1 << 4,
	NX_MF_DISABLE_STRONG_FRICTION = 1 << 5,
};

[Engine::Attributes::SingleEnum]
public enum class PhysCombineMode : unsigned int
{
	NX_CM_AVERAGE = 0,		//!< Average: (a + b)/2
	NX_CM_MIN = 1,			//!< Minimum: min(a,b)
	NX_CM_MULTIPLY = 2,		//!< Multiply: a*b
	NX_CM_MAX = 3,			//!< Maximum: max(a,b)
	NX_CM_N_VALUES = 4,	//!< This is not a valid combine mode, it is a sentinel to denote the number of possible values. We assert that the variable's value is smaller than this.
	NX_CM_PAD_32 = 0xffffffff //!< This is not a valid combine mode, it is to assure that the size of the enum type is big enough.
};

/// <summary>
/// Very similar to NxMeshFlags used for the NxSimpleTriangleMesh type.
/// </summary>
[Engine::Attributes::MultiEnum]
public enum class PhysMeshDataFlags : unsigned int
{
	/// <summary>
	/// Denotes the use of 16-bit vertex indices.
	/// </summary>
	NX_MDF_16_BIT_INDICES				=	1 << 0, 
};

/// <summary>
/// Enum with flag values to be used in NxMeshData::dirtyBufferFlagsPtr.
/// </summary>
[Engine::Attributes::MultiEnum]
public enum class PhysMeshDataDirtyBufferFlags : unsigned int
{
	/// <summary>
	/// Denotes a change in the vertex position buffer.
	/// </summary>
	NX_MDF_VERTICES_POS_DIRTY			=	1 << 0, 
	/// <summary>
	/// Denotes a change in the vertex normal buffer.
	/// </summary>
	NX_MDF_VERTICES_NORMAL_DIRTY		=	1 << 1, 
	/// <summary>
	/// Denotes a change in the index buffer.
	/// </summary>
	NX_MDF_INDICES_DIRTY				=	1 << 2, 
	/// <summary>
	/// Denotes a change in the parent index buffer.
	/// </summary>
	NX_MDF_PARENT_INDICES_DIRTY			=	1 << 3, 
};

/// <summary>
/// Soft body flags.
/// </summary>
[Engine::Attributes::MultiEnum]
public enum class PhysSoftBodyFlag : unsigned int
{
	/**
	\brief Makes the soft body static. 

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	*/
	NX_SBF_STATIC			  = (1<<1),

	/**
	\brief Disable collision handling with the rigid body scene. 

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxSoftBodyDesc.collisionResponseCoefficient
	*/
	NX_SBF_DISABLE_COLLISION  = (1<<2),

	/**
	\brief Enable/disable self-collision handling within a single soft body.
	
	Note: self-collisions are only handled inbetween the soft body's particles, i.e.,
	particles do not collide against the tetrahedra of the soft body.
	The user should therefore specify a large enough particleRadius to avoid
	most interpenetrations. See NxSoftBodyDesc.particleRadius.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	*/
	NX_SBF_SELFCOLLISION	  = (1<<3),

	/**
	\brief Enable/disable debug visualization. 

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	*/
	NX_SBF_VISUALIZATION	  = (1<<4),

	/**
	\brief Enable/disable gravity. If off, the soft body is not subject to the gravitational force
	of the rigid body scene.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	*/
	NX_SBF_GRAVITY            = (1<<5),

	/**
	\brief Enable/disable volume conservation. Select volume conservation through 
	NxSoftBodyDesc.volumeStiffness.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxSoftBodyDesc.volumeStiffness
	*/
	NX_SBF_VOLUME_CONSERVATION            = (1<<6),

	/**
	\brief Enable/disable damping of internal velocities. Use NxSoftBodyDesc.dampingCoefficient
	to control damping.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxSoftBodyDesc.dampingCoefficient
	*/
	NX_SBF_DAMPING            = (1<<7),

	/**
	\brief Enable/disable two way collision of the soft body with the rigid body scene.
	
	In either case, the soft body is influenced by colliding rigid bodies.
	If NX_SBF_COLLISION_TWOWAY is not set, rigid bodies are not influenced by 
	colliding with the soft body. Use NxSoftBodyDesc.collisionResponseCoefficient to
	control the strength of the feedback force on rigid bodies.

	When using two way interaction care should be taken when setting the density of the attached objects.
	For example if an object with a very low or high density is attached to a soft body then the simulation 
	may behave poorly. This is because impulses are only transfered between the soft body and rigid body solver
	outside the solvers.

	Two way interaction works best when NX_SF_SEQUENTIAL_PRIMARY is set in the primary scene. If not set,
	collision and attachment artifacts may happen.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxSoftBodyDesc.collisionResponseCoefficient
	*/
	NX_SBF_COLLISION_TWOWAY   = (1<<8),

	/**
	\brief Defines whether the soft body is tearable. 
	
	Make sure meshData.maxVertices and the corresponding buffers
	in meshData are substantially larger (e.g. 2x) than the number 
	of original vertices since tearing will generate new vertices.
	When the buffer cannot hold the new vertices anymore, tearing stops.
	If this buffer is chosen big enough, the entire mesh can be 
	torn into all constituent tetrahedral elements. 
	(The theoretical maximum needed is 12 times the original number of vertices. 
	For reasonable mesh topologies, this should never be reached though.)
	
	If the soft body is simulated on the hardware, additional buffer 
	limitations that cannot be controlled by the user exist. Therefore, soft 
	bodies might cease to tear apart further, even though not all space in 
	the user buffer is used up.

	Note: For tearing in hardware, make sure you cook the mesh with
	the flag NX_SOFTBODY_MESH_TEARABLE set in the NxSoftBodyMeshDesc.flags.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxSoftBodyDesc.tearFactor NxSoftBodyDesc.meshData NxSoftBodyMeshDesc.flags
	*/
	NX_SBF_TEARABLE           = (1<<9),

	/**
	\brief Defines whether this soft body is simulated on the PPU.
	
	To simulate a soft body on the PPU
	set this flag and create the soft body in a regular software scene. 
	Note: only use this flag during creation, do not change it using NxSoftBody.setFlags().
	*/
	NX_SBF_HARDWARE           = (1<<10),

	/**
	\brief Enable/disable center of mass damping of internal velocities. 

	This flag only has an effect if the flag NX_SBF_DAMPING is set. If set, 
	the global rigid body modes (translation and rotation) are extracted from damping. 
	This way, the soft body can freely move and rotate even under high damping. 
	Use NxSoftBodyDesc.dampingCoefficient to control damping. 

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxSoftBodyDesc.dampingCoefficient
	*/
	NX_SBF_COMDAMPING		  = (1<<11),

	/**
	\brief If the flag NX_SBF_VALIDBOUNDS is set, soft body particles outside the volume
	defined by NxSoftBodyDesc.validBounds are automatically removed from the simulation. 

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxSoftBodyDesc.validBounds
	*/
	NX_SBF_VALIDBOUNDS		  = (1<<12),

	/**
	\brief Enable/disable collision handling between this soft body and fluids. 

	Note: With the current implementation, do not switch on fluid collision for
	many soft bodies. Create scenes with a few bodies because the memory usage
	increases linearly with the number of soft bodies.
	The performance of the collision detection is dependent on both the particle 
	radius of the soft body and the particle radius of the fluid, so tuning 
	these parameters might improve the performance significantly.

	Note: The current implementation does not obey the NxScene::setGroupCollisionFlag
	settings. If NX_SBF_FLUID_COLLISION is set, collisions will take place even if
	collisions between the groups that the corresponding soft body and fluid belong to are
	disabled.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxSoftBodyDesc.fluidCollisionResponseCoefficient
	*/
	NX_SBF_FLUID_COLLISION    = (1<<13),

	/**
	\brief Disable continuous collision detection with dynamic actors. 
	Dynamic actors are handled as static ones.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes
	*/
	NX_SBF_DISABLE_DYNAMIC_CCD  = (1<<14),

	/**
	\brief Moves soft body partially in the frame of the attached actor. 

	This feature is useful when the soft body is attached to a fast moving shape.
	In that case the soft body adheres to the shape it is attached to while only 
	velocities below the parameter minAdhereVelocity are used for secondary effects.

	<b>Platform:</b>
	\li PC SW: Yes
	\li PPU  : Yes
	\li PS3  : Yes
	\li XB360: Yes

	@see NxSoftBodyDesc.minAdhereVelocity
	*/
	NX_SBF_ADHERE  = (1<<15),

};

/// <summary>
/// Soft body mesh flags.
/// </summary>
[Engine::Attributes::MultiEnum]
public enum class PhysSoftBodyMeshFlags : unsigned int
{
	/// <summary>
	/// No flags.
	/// </summary>
	NONE = 0,

	/// <summary>
	/// Denotes the use of 16-bit vertex indices
	/// </summary>
	NX_SOFTBODY_MESH_16_BIT_INDICES	=	(1<<1),

	/// <summary>
	/// Not supported in current release. Specifies whether extra space is
    /// allocated for tearing on the PPU. If this flag is not set, less memory
    /// is needed on the PPU but tearing is not possible.
	/// </summary>
	NX_SOFTBODY_MESH_TEARABLE	=	(1<<2),
};


/// <summary>
/// Soft body attachment flags.
/// </summary>
[Engine::Attributes::MultiEnum]
public enum class PhysSoftBodyAttachmentFlag : unsigned int
{
	/// <summary>
	/// No flags.
	/// </summary>
	NONE = 0,

	/// <summary>
	/// The default is only object->soft body interaction (one way).
	/// <para>
	/// With this flag set, soft body->object interaction is turned on as well.
	/// </para>
	/// <para>
	/// Care should be taken if objects with small masses (either through low 
	/// density or small volume) are attached, as the simulation may easily become unstable. 
	/// The NxSoftBodyDesc.attachmentResponseCoefficient field should be used to lower
	/// the magnitude of the impulse transfer from the soft body to the attached rigid body.
	/// </para>
	/// </summary>
	NX_SOFTBODY_ATTACHMENT_TWOWAY			  = (1<<0),

	/// <summary>
	/// When this flag is set, the attachment is tearable.
	/// </summary>
	NX_SOFTBODY_ATTACHMENT_TEARABLE		  = (1<<1),
};

/// <summary>
/// Shape types.
/// </summary>
[Engine::Attributes::SingleEnum]
public enum class PhysShapeType
	{
	/**
	\brief A physical plane
    @see NxPlaneShape
	*/
	NX_SHAPE_PLANE,

	/**
	\brief A physical sphere
	@see NxSphereShape
	*/
	NX_SHAPE_SPHERE,

	/**
	\brief A physical box (OBB)
	@see NxBoxShape
	*/
	NX_SHAPE_BOX,

	/**
	\brief A physical capsule (LSS)
	@see NxCapsuleShape
	*/
	NX_SHAPE_CAPSULE,

	/**
	\brief A wheel for raycast cars
	@see NxWheelShape
	*/
	NX_SHAPE_WHEEL,

	/**
	\brief A physical convex mesh
	@see NxConvexShape NxConvexMesh
	*/
	NX_SHAPE_CONVEX,

	/**
	\brief A physical mesh
	@see NxTriangleMeshShape NxTriangleMesh
	*/
	NX_SHAPE_MESH,

	/**
	\brief A physical height-field
	@see NxHeightFieldShape NxHeightField
	*/
	NX_SHAPE_HEIGHTFIELD,

	/**
	\brief internal use only!

	*/
	NX_SHAPE_RAW_MESH,

	NX_SHAPE_COMPOUND,

	NX_SHAPE_COUNT,

	NX_SHAPE_FORCE_DWORD = 0x7fffffff
};

}