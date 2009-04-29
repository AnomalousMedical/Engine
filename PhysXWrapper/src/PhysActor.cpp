#include "StdAfx.h"
#include "..\include\PhysActor.h"

#include "NxActor.h"
#include "MarshalUtils.h"
#include "MathUtil.h"
#include "ActiveTransformCallback.h"
#include "ContactListener.h"
#include "PhysActorDesc.h"
#include "PhysBodyDesc.h"
#include "PhysShape.h"
#include "PhysShapeDesc.h"

namespace PhysXWrapper
{

using namespace System;

PhysActor::PhysActor(NxActor* actor)
:actor(actor), 
actorRoot(new PhysActorGCRoot()), 
callback( nullptr ),
contactListeners( gcnew ContactListenerList() )
{
	*actorRoot.Get() = this;
	actor->userData = actorRoot.Get();
}

PhysActor::~PhysActor()
{
	actor = 0;
}

void PhysActor::setActiveTransformCallback(ActiveTransformCallback^ callback)
{
	this->callback = callback;
}

void PhysActor::fireLocationChanged(NxMat34& newLoc)
{
	if(callback != nullptr)
	{
		NxVec3 loc = newLoc.t;
		NxQuat rot;
		newLoc.M.toQuat(rot);
		callback->firePositionUpdate(Engine::Vector3(loc.x, loc.y, loc.z), 
			Engine::Quaternion(rot.x, rot.y, rot.z, rot.w));
	}
}

void PhysActor::saveToDesc(PhysActorDesc^ actorDesc)
{
	actor->saveToDesc(*actorDesc->actorDesc.Get());
}

bool PhysActor::saveBodyToDesc(PhysBodyDesc^ bodyDesc)
{
	return actor->saveBodyToDesc(*bodyDesc->bodyDesc.Get());
}

void PhysActor::setGlobalPosition( Engine::Vector3 translation )
{
	actor->setGlobalPosition( MathUtil::copyVector3(translation) );
}

void PhysActor::setGlobalOrientationQuat( Engine::Quaternion rotation )
{
	actor->setGlobalOrientationQuat( MathUtil::copyQuaternion( rotation ) );
}

Engine::Vector3 PhysActor::getGlobalPosition()
{
	return MathUtil::copyVector3(actor->getGlobalPose().t);
}

Engine::Quaternion PhysActor::getGlobalOrientationQuat()
{
	return MathUtil::copyQuaternion(actor->getGlobalOrientationQuat());
}

void PhysActor::setActorGroup( int actorGroup )
{
	actor->setGroup( actorGroup );
}

int PhysActor::getActorGroup()
{
	return actor->getGroup();
}

void PhysActor::setCollisionGroup( int collisionGroup )
{
	int numShapes = actor->getNbShapes();
	NxShape* const* shapes = actor->getShapes();
	while( numShapes-- )
	{
		shapes[numShapes]->setGroup(collisionGroup);
	}
}

int PhysActor::getCollisionGroup(){
	if( actor->getNbShapes() > 0 ) {
		return actor->getShapes()[0]->getGroup();
	}
	return -1;
}

void PhysActor::raiseBodyFlag( BodyFlag flag )
{
	actor->raiseBodyFlag( (NxBodyFlag)flag );
}

void PhysActor::clearBodyFlag( BodyFlag flag )
{
	actor->clearBodyFlag( (NxBodyFlag)flag );
}

bool PhysActor::readBodyFlag( BodyFlag flag )
{
	return actor->readBodyFlag( (NxBodyFlag)flag );
}

void PhysActor::raiseActorFlag (ActorFlag actorFlag)
{
	actor->raiseActorFlag((NxActorFlag)actorFlag);
}

void PhysActor::clearActorFlag (ActorFlag actorFlag)
{
	actor->clearActorFlag((NxActorFlag)actorFlag);
}

bool PhysActor::readActorFlag (ActorFlag actorFlag)
{
	return actor->readActorFlag((NxActorFlag)actorFlag);
}

bool PhysActor::isDynamic()
{
	return actor->isDynamic();
}

//-----------
//Forces
//-----------

void PhysActor::addForceAtPos( Engine::Vector3% force, Engine::Vector3% pos, ForceMode mode, bool wakeup )
{
	actor->addForceAtPos( MathUtil::copyVector3(force), MathUtil::copyVector3(pos), static_cast<NxForceMode>(mode), wakeup );
}

void PhysActor::addForceAtLocalPos (const Engine::Vector3% force, const Engine::Vector3% pos, ForceMode mode, bool wakeup)
{
	actor->addForceAtLocalPos( MathUtil::copyVector3(force), MathUtil::copyVector3(pos), (NxForceMode)mode, wakeup );
}

void PhysActor::addLocalForceAtPos (const Engine::Vector3% force, const Engine::Vector3% pos, ForceMode mode, bool wakeup){
	actor->addLocalForceAtPos( MathUtil::copyVector3(force), MathUtil::copyVector3(pos), (NxForceMode)mode, wakeup );
}

void PhysActor::addLocalForceAtLocalPos (const Engine::Vector3% force, const Engine::Vector3% pos, ForceMode mode, bool wakeup)
{
	actor->addLocalForceAtLocalPos( MathUtil::copyVector3(force), MathUtil::copyVector3(pos), (NxForceMode)mode, wakeup );
}

void PhysActor::addForce (const Engine::Vector3% force, ForceMode mode, bool wakeup)
{
	actor->addForce( MathUtil::copyVector3(force), (NxForceMode)mode, wakeup );
}

void PhysActor::addLocalForce (const Engine::Vector3% force, ForceMode mode, bool wakeup)
{
	actor->addLocalForce( MathUtil::copyVector3(force), (NxForceMode)mode, wakeup );
}

void PhysActor::addTorque (const Engine::Vector3% torque, ForceMode mode, bool wakeup)
{
	actor->addTorque( MathUtil::copyVector3(torque), (NxForceMode)mode, wakeup );
}

void PhysActor::addLocalTorque (const Engine::Vector3% torque, ForceMode mode, bool wakeup)
{
	actor->addLocalTorque( MathUtil::copyVector3(torque), (NxForceMode)mode, wakeup );
}	

//-----------------
//Kinematic
//-----------------
	
/*void PhysActor::moveGlobalPose (const Matrix% mat){
	if( actor ){
		actor->moveGlobalPose( Conversion::MatrixToNxMat34( mat ) );
	}
}*/

void PhysActor::moveGlobalPosition (const Engine::Vector3% vec)
{
	actor->moveGlobalPosition( MathUtil::copyVector3(vec) );
}

/*void PhysActor::moveGlobalOrientation (const Matrix% mat){	
	if( actor ){
		actor->moveGlobalOrientation( Conversion::MatrixToNxMat33( mat ) );
	}
}*/

void PhysActor::moveGlobalOrientationQuat (Engine::Quaternion% quat)
{
	actor->moveGlobalOrientationQuat( MathUtil::copyQuaternion( quat ) );
}

//---------------------
//Center of mass
//---------------------

/*void PhysActor::setCMassOffsetLocalPose(Matrix mat)
{
	actor->setCMassOffsetLocalPose( Conversion::MatrixToNxMat34( mat ) );
}*/

void PhysActor::setCMassOffsetLocalPosition(Engine::Vector3 vec)
{
	actor->setCMassOffsetLocalPosition( MathUtil::copyVector3(vec) );
}

/*void PhysActor::setCMassOffsetLocalOrientation(Matrix mat)
{
	actor->setCMassOffsetLocalOrientation( Conversion::MatrixToNxMat33( mat ) );
}*/

/*void PhysActor::setCMassOffsetGlobalPose(Matrix mat)
{
	actor->setCMassOffsetGlobalPose( Conversion::MatrixToNxMat34( mat ) );
}*/

void PhysActor::setCMassOffsetGlobalPosition(Engine::Vector3 vec)
{
	actor->setCMassOffsetGlobalPosition( MathUtil::copyVector3(vec) );
}

/*void PhysActor::setCMassOffsetGlobalOrientation(Matrix mat)
{
	actor->setCMassOffsetGlobalOrientation( Conversion::MatrixToNxMat33( mat ) );
}*/

/*void PhysActor::setCMassGlobalPose(Matrix mat)
{
	actor->setCMassGlobalPose( Conversion::MatrixToNxMat34( mat ) );
}*/

void PhysActor::setCMassGlobalPosition(Engine::Vector3 vec)
{
	actor->setCMassGlobalPosition( MathUtil::copyVector3(vec) );
}

/*void PhysActor::setCMassGlobalOrientation(Matrix mat)
{
	actor->setCMassGlobalOrientation( Conversion::MatrixToNxMat33( mat ) );
}*/

/*void PhysActor::getCMassLocalPose( Matrix% cMass )
{
	Conversion::NxMat34ToMatrix( actor->getCMassLocalPose(), cMass );
}*/

void PhysActor::getCMassLocalPosition( Engine::Vector3% cMass )
{
	MathUtil::copyVector3( actor->getCMassLocalPosition(), cMass );
}

/*void PhysActor::getCMassLocalOrientation( Matrix% cMass )
{
	Conversion::NxMat33ToMatrix( actor->getCMassLocalOrientation(), cMass );
}*/

/*void PhysActor::getCMassGlobalPose( Matrix% cMass )
{
	Conversion::NxMat34ToMatrix( actor->getCMassGlobalPose(), cMass );
}*/

void PhysActor::getCMassGlobalPosition( Engine::Vector3% cMass )
{
	MathUtil::copyVector3( actor->getCMassGlobalPosition(), cMass );
}

/*void PhysActor::getCMassGlobalOrientation( Matrix% cMass )
{
	Conversion::NxMat33ToMatrix( actor->getCMassGlobalOrientation(), cMass );
}*/

void PhysActor::setMass(float mass)
{
	actor->setMass( mass );
}

float PhysActor::getMass()
{
	return actor->getMass();
}

void PhysActor::setMassSpaceInertiaTensor(Engine::Vector3% m)
{
	actor->setMassSpaceInertiaTensor( MathUtil::copyVector3(m) );
}

void PhysActor::getMassSpaceInertiaTensor( Engine::Vector3% inertiaTensor )
{
	return MathUtil::copyVector3( actor->getMassSpaceInertiaTensor(), inertiaTensor );
}

/*void PhysActor::getGlobalInertiaTensor( Matrix% inertiaTensor )
{
	return Conversion::NxMat33ToMatrix( actor->getGlobalInertiaTensor(), inertiaTensor );
}*/

/*void PhysActor::getGlobalInertiaTensorInverse( Matrix% inertiaTensorInv )
{
	return Conversion::NxMat33ToMatrix( actor->getGlobalInertiaTensorInverse(), inertiaTensorInv );
}*/

void PhysActor::updateMassFromShapes(float density, float totalMass)
{
	actor->updateMassFromShapes( density, totalMass );
}


void PhysActor::setLinearDamping(float linDamp)
{
	actor->setLinearDamping( linDamp );
}
	

float PhysActor::getLinearDamping()
{
		return actor->getLinearDamping();
}
  

void PhysActor::setAngularDamping(NxReal angDamp)
{
	actor->setAngularDamping( angDamp );
}
  

float PhysActor::getAngularDamping()
{
	return actor->getAngularDamping();
}


void PhysActor::setLinearVelocity(Engine::Vector3 linVel)
{
	actor->setLinearVelocity( MathUtil::copyVector3(linVel) );
}

void PhysActor::setAngularVelocity(Engine::Vector3 angVel)
{
	actor->setAngularVelocity( MathUtil::copyVector3(angVel) );
}  

void PhysActor::getLinearVelocity( Engine::Vector3% linVel )
{
	MathUtil::copyVector3( actor->getLinearVelocity(), linVel );
} 

void PhysActor::getAngularVelocity( Engine::Vector3% angVel )
{
	MathUtil::copyVector3( actor->getAngularVelocity(), angVel );
}

void PhysActor::setMaxAngularVelocity(float maxAngVel)
{
	actor->setMaxAngularVelocity( maxAngVel );
}
   
float PhysActor::getMaxAngularVelocity()
{
	return actor->getMaxAngularVelocity();
}

//-------------------
//Momentum
//-------------------

void PhysActor::setLinearMomentum(Engine::Vector3 linMoment)
{
	actor->setLinearMomentum( MathUtil::copyVector3(linMoment) );
}

void PhysActor::setAngularMomentum(Engine::Vector3 angMoment)
{
	actor->setAngularMomentum( MathUtil::copyVector3(angMoment) );
}

void PhysActor::getLinearMomentum( Engine::Vector3% momentum )
{
	MathUtil::copyVector3( actor->getLinearMomentum(), momentum );
}
  
void PhysActor::getAngularMomentum( Engine::Vector3% momentum )
{
	MathUtil::copyVector3( actor->getAngularMomentum(), momentum );
}


void PhysActor::getPointVelocity(Engine::Vector3 point, Engine::Vector3% result)
{
	NxVec3 pointVel = actor->getPointVelocity( NxVec3(point.x, point.y, point.z) );
	MathUtil::copyVector3( pointVel, result );
} 

void PhysActor::getLocalPointVelocity(Engine::Vector3 point, Engine::Vector3% result){
	NxVec3 pointVel = actor->getLocalPointVelocity( NxVec3(point.x, point.y, point.z) );
	MathUtil::copyVector3( pointVel, result );
}

void PhysActor::addContactListener( ContactListener^ listener )
{
	contactListeners->AddLast( listener );
}

void PhysActor::removeContactListener( ContactListener^ listener )
{
	contactListeners->Remove( listener );
}

void PhysActor::alertContact( PhysActor^ contactWith, PhysActor^ myself, ContactIterator^ contacts, ContactPairFlag contactType )
{
	for each( ContactListener^ listener in contactListeners )
	{
		listener->contact( contactWith, myself, contacts, contactType );
	}
}

PhysShape^ PhysActor::createShape(PhysShapeDesc^ desc)
{
	return shapes.getObject(actor->createShape(*desc->shapeDesc));
}

void PhysActor::releaseShape(PhysShape^ shape)
{
	shapes.destroyObject(shape->getNxShape());
	actor->releaseShape(*shape->getNxShape());
}

unsigned int PhysActor::getNbShapes()
{
	return actor->getNbShapes();
}

ShapeEnumerator^ PhysActor::getShapes()
{
	NxShape* const* shapes = actor->getShapes();
	unsigned int numShapes = actor->getNbShapes();
	ShapeEnumerator^ shapeList = gcnew ShapeEnumerator(numShapes);
	for(unsigned int i = 0; i < numShapes; ++i)
	{
		shapeList->Add(this->shapes.getObject(shapes[i]));
	}
	return shapeList;
}

}