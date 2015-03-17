LOCAL_PATH := $(call my-dir)

DEP_ROOT		:= $(LOCAL_PATH)/../../../../Dependencies
DEP_ROOT_LIB	:= $(LOCAL_PATH)/../../../../../Dependencies

PRJ_INCLUDES    := $(LOCAL_PATH)/../../Include \
				   $(LOCAL_PATH)/../.. \
	               $(DEP_ROOT)/Bullet/src/src \
				   $(DEP_ROOT)/Bullet/src/Extras/ConvexDecomposition \

PRJ_SRC		    := /../../Src

LIB_ROOT		:= $(DEP_ROOT_LIB)/Bullet/AndroidBuild/libs/armeabi

# bulletsoftbody
	include $(CLEAR_VARS)
	LOCAL_MODULE    := bulletsoftbody
	LOCAL_SRC_FILES := $(LIB_ROOT)/libBulletSoftBody.a
	include $(PREBUILT_STATIC_LIBRARY)

# bulletdynamics
	include $(CLEAR_VARS)
	LOCAL_MODULE    := bulletdynamics
	LOCAL_SRC_FILES := $(LIB_ROOT)/libBulletDynamics.a
	include $(PREBUILT_STATIC_LIBRARY)

# bulletcollision
	include $(CLEAR_VARS)
	LOCAL_MODULE    := bulletcollision
	LOCAL_SRC_FILES := $(LIB_ROOT)/libBulletCollision.a
	include $(PREBUILT_STATIC_LIBRARY)

# linearmath
	include $(CLEAR_VARS)
	LOCAL_MODULE    := linearmath
	LOCAL_SRC_FILES := $(LIB_ROOT)/libLinearMath.a
	include $(PREBUILT_STATIC_LIBRARY)

# bulletmultithreaded
	include $(CLEAR_VARS)
	LOCAL_MODULE    := bulletmultithreaded
	LOCAL_SRC_FILES := $(LIB_ROOT)/libBulletMultiThreaded.a
	include $(PREBUILT_STATIC_LIBRARY)

# convexdecomposition
	include $(CLEAR_VARS)
	LOCAL_MODULE    := convexdecomposition
	LOCAL_SRC_FILES := $(LIB_ROOT)/libConvexDecomposition.a
	include $(PREBUILT_STATIC_LIBRARY)



#Main Project

include $(CLEAR_VARS)

LOCAL_CPPFLAGS := -DANDROID=1
LOCAL_ARM_MODE := arm
LOCAL_MODULE   := libBulletWrapper
LOCAL_CFLAGS   := -ffast-math -fsigned-char -O2 -fPIC -DPIC \
                  -DBYTE_ORDER=LITTLE_ENDIAN -D_ARM_ASSEM_ \
				  -frtti

LOCAL_C_INCLUDES := $(PRJ_INCLUDES)

LOCAL_SRC_FILES :=  \
                    $(PRJ_SRC)/../Stdafx.cpp \
					$(PRJ_SRC)/btFixedConstraintWrapper.cpp \
					$(PRJ_SRC)/btGeneric6DofConstraintWrapper.cpp \
					$(PRJ_SRC)/btManifoldPoint.cpp \
					$(PRJ_SRC)/btRigidBodyWrapper.cpp \
					$(PRJ_SRC)/btTypedConstraintWrapper.cpp \
					$(PRJ_SRC)/BulletDebugDraw.cpp \
					$(PRJ_SRC)/BulletScene.cpp \
					$(PRJ_SRC)/CollisionShapeInterface.cpp \
					$(PRJ_SRC)/ContactCache.cpp \
					$(PRJ_SRC)/ContactInfo.cpp \
					$(PRJ_SRC)/MotionState.cpp \
					$(PRJ_SRC)/RayTests.cpp \
					$(PRJ_SRC)/ReshapeableRigidBody.cpp \
					$(PRJ_SRC)/ReshapeableRigidBodySection.cpp \

LOCAL_STATIC_LIBRARIES	:= bulletdynamics bulletsoftbody convexdecomposition bulletcollision linearmath bulletmultithreaded

include $(BUILD_SHARED_LIBRARY)
