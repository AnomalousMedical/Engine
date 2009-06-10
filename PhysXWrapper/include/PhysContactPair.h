#pragma once

namespace PhysXWrapper
{

ref class ContactIterator;
ref class PhysActor;

public ref class PhysContactPair
{
private:
	ContactIterator^ contactIterator;
	NxContactPair* contactPair;
	NxContactStreamIterator* csi;

internal:
	void setPair(NxContactPair* contactPair);

	void clearPair();

	PhysContactPair(void);
public:
	bool isActorDeleted(int actor);

	PhysActor^ getActor(int actor);

	ContactIterator^ getContactIterator();

	Engine::Vector3 getSumNormalForce();

	Engine::Vector3 getSumFrictionForce();
};

}