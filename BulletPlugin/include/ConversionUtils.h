#pragma once

#pragma unmanaged

void GetRotation(const btTransform& tf, float* values)
{
	btQuaternion& rot = tf.getRotation();
	values[0] = rot.x();
	values[1] = rot.y();
	values[2] = rot.z();
	values[3] = rot.w();
}

#pragma managed