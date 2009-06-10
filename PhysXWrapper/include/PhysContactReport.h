#pragma once

#include "Enums.h"

namespace PhysXWrapper
{

ref class PhysContactPair;

public interface class PhysContactReport
{
public:
	void onContactNotify( PhysContactPair^ pair, ContactPairFlag events );
};

}