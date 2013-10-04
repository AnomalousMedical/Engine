using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace BulletPlugin
{
    class MotionState : IDisposable
    {
        private SetXformCallback xformCallback;
        private ContactCallback contactStartedCallback;
        private ContactCallback contactEndedCallback;
        private ContactCallback contactContinuesCallback;
        internal IntPtr motionState;
        private ContactInfo contactInfo = new ContactInfo();
        private RigidBody rigidBody;

        public MotionState(RigidBody rigidBody, float maxContactDistance, ref Vector3 initialTrans, ref Quaternion initialRot)
        {
            this.rigidBody = rigidBody;
            xformCallback = new SetXformCallback(motionStateCallback);
            contactStartedCallback = new ContactCallback(contactStartedCallbackFunc);
            contactEndedCallback = new ContactCallback(contactEndedCallbackFunc);
            contactContinuesCallback = new ContactCallback(contactContinuesCallbackFunc);
            motionState = MotionState_Create(xformCallback, contactStartedCallback, contactEndedCallback, contactContinuesCallback, maxContactDistance, ref initialTrans, ref initialRot);
        }

        public void Dispose()
        {
            if(motionState != IntPtr.Zero)
            {
                MotionState_Delete(motionState);
                motionState = IntPtr.Zero;
                xformCallback = null;
                contactStartedCallback = null;
                contactEndedCallback = null;
                contactContinuesCallback = null;
            }
        }

        public float MaxContactDistance
        {
            get
            {
                return MotionState_getMaxContactDistance(motionState);
            }
            set
            {
                MotionState_setMaxContactDistance(motionState, value);
            }
        }

        private void motionStateCallback(ref Vector3 trans, ref Quaternion rot)
        {
            rigidBody.updateObjectPosition(ref trans, ref rot);
        }

        private void contactStartedCallbackFunc(IntPtr contact, IntPtr sourceBody, IntPtr otherBody, bool isBodyA)
        {
            if (m_contactStarted != null)
            {
                contactInfo.setInfo(contact);
                m_contactStarted.Invoke(contactInfo, RigidBodyManager.get(sourceBody), RigidBodyManager.get(otherBody), isBodyA);
            }
        }

        private void contactEndedCallbackFunc(IntPtr contact, IntPtr sourceBody, IntPtr otherBody, bool isBodyA)
        {
            if (m_contactEnded != null)
            {
                contactInfo.setInfo(contact);
                m_contactEnded.Invoke(contactInfo, RigidBodyManager.get(sourceBody), RigidBodyManager.get(otherBody), isBodyA);
            }
        }

        private void contactContinuesCallbackFunc(IntPtr contact, IntPtr sourceBody, IntPtr otherBody, bool isBodyA)
        {
            if (m_contactContinues != null)
            {
                contactInfo.setInfo(contact);
                m_contactContinues.Invoke(contactInfo, RigidBodyManager.get(sourceBody), RigidBodyManager.get(otherBody), isBodyA);
            }
        }

        private event CollisionCallback m_contactStarted;
        public event CollisionCallback ContactStarted
        {
            add
            {
                if (m_contactStarted == null && motionState != IntPtr.Zero)
                {
                    MotionState_setHasContactStartedCallback(motionState, true);
                }
                m_contactStarted += value;
            }
            remove
            {
                m_contactStarted -= value;
                if (m_contactStarted == null && motionState != IntPtr.Zero)
                {
                    MotionState_setHasContactStartedCallback(motionState, false);
                }
            }
        }

        private event CollisionCallback m_contactContinues;
        public event CollisionCallback ContactContinues
        {
            add
            {
                if (m_contactContinues == null && motionState != IntPtr.Zero)
                {
                    MotionState_setHasContactContinuesCallback(motionState, true);
                }
                m_contactContinues += value;
            }
            remove
            {
                m_contactContinues -= value;
                if (m_contactContinues == null && motionState != IntPtr.Zero)
                {
                    MotionState_setHasContactContinuesCallback(motionState, false);
                }
            }
        }

        private event CollisionCallback m_contactEnded;
        public event CollisionCallback ContactEnded
        {
            add
            {
                if (m_contactEnded == null && motionState != IntPtr.Zero)
                {
                    MotionState_setHasContactEndedCallback(motionState, true);
                }
                m_contactEnded += value;
            }
            remove
            {
                m_contactEnded -= value;
                if (m_contactEnded == null && motionState != IntPtr.Zero)
                {
                    MotionState_setHasContactEndedCallback(motionState, false);
                }
            }
        }

        //MotionState
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void SetXformCallback(ref Vector3 trans, ref Quaternion rot);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ContactCallback(IntPtr contact, IntPtr sourceBody, IntPtr otherBody, bool isBodyA);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MotionState_Create(SetXformCallback xformCallback, ContactCallback contactStartedCallback, ContactCallback contactEndedCallback, ContactCallback contactContinuesCallback, float maxContactDistance, ref Vector3 initialTrans, ref Quaternion initialRot);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void MotionState_Delete(IntPtr instance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void MotionState_setHasContactStartedCallback(IntPtr instance, bool hasCallback);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void MotionState_setHasContactEndedCallback(IntPtr instance, bool hasCallback);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void MotionState_setHasContactContinuesCallback(IntPtr instance, bool hasCallback);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void MotionState_setMaxContactDistance(IntPtr instance, float maxContactDistance);

        [DllImport("BulletWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float MotionState_getMaxContactDistance(IntPtr instance);
    }
}
