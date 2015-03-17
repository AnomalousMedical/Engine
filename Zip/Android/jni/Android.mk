LOCAL_PATH := $(call my-dir)

DEP_ROOT		:= $(LOCAL_PATH)/../../../../Dependencies
DEP_ROOT_LIB	:= $(LOCAL_PATH)/../../../../../Dependencies

PRJ_INCLUDES    := $(LOCAL_PATH)/../../Include \
				   $(LOCAL_PATH)/../.. \
	               $(DEP_ROOT)/OgreDeps/AndroidInstall/include
PRJ_SRC		    := /../../Src

# zlib
	include $(CLEAR_VARS)
	LOCAL_MODULE    := zlib
	LOCAL_SRC_FILES := $(DEP_ROOT_LIB)/OgreDeps/AndroidInstall/lib/libzlib.a
	include $(PREBUILT_STATIC_LIBRARY)

# zzip
	include $(CLEAR_VARS)
	LOCAL_MODULE    := zzip
	LOCAL_SRC_FILES := $(DEP_ROOT_LIB)/OgreDeps/AndroidInstall/lib/libzziplib.a
	include $(PREBUILT_STATIC_LIBRARY)



#Main Project

include $(CLEAR_VARS)

LOCAL_CPPFLAGS := -DANDROID=1
LOCAL_ARM_MODE := arm
LOCAL_MODULE   := libZip
LOCAL_CFLAGS   := -ffast-math -fsigned-char -O2 -fPIC -DPIC \
                  -DBYTE_ORDER=LITTLE_ENDIAN -D_ARM_ASSEM_

LOCAL_C_INCLUDES := $(PRJ_INCLUDES)

LOCAL_SRC_FILES :=  \
                    $(PRJ_SRC)/../Stdafx.cpp \
					$(PRJ_SRC)/ZipFile.cpp \
					$(PRJ_SRC)/ZipStream.cpp \

LOCAL_STATIC_LIBRARIES	:= zzip zlib

include $(BUILD_SHARED_LIBRARY)
