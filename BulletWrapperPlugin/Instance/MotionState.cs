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
        internal IntPtr motionState;
        private ContactInfo contactInfo = new ContactInfo();
        protected RigidBody rigidBody;

        private bool positionUpdated = false;
        private CallbackHandler callbackHandler;
        protected Vector3 localTranslation;
        protected Quaternion localRotation;

        public MotionState(RigidBody rigidBody, float maxContactDistance, ref Vector3 initialTrans, ref Quaternion initialRot)
        {
            this.rigidBody = rigidBody;
            callbackHandler = new CallbackHandler();
            motionState = callbackHandler.create(this, maxContactDistance, ref initialTrans, ref initialRot);
            localTranslation = initialTrans;
            localRotation = initialRot;
        }

        public void Dispose()
        {
            if(motionState != IntPtr.Zero)
            {
                MotionState_Delete(motionState);
                motionState = IntPtr.Zero;
                callbackHandler.Dispose();
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

        /// <summary>
        /// True if the position changed.
        /// </summary>
        public bool PositionUpdated
        {
            get
            {
                return positionUpdated;
            }
        }

        /// <summary>
        /// The translation of the object in the physics scene's coords.
        /// </summary>
        public Vector3 LocalTranslation
        {
            get
            {
                return localTranslation;
            }
        }

        /// <summary>
        /// The rotation of the object in the physics scene's coords.
        /// </summary>
        public Quaternion LocalRotation
        {
            get
            {
                return localRotation;
            }
        }

        /// <summary>
        /// The translation on this motion state in world coords. This will include 
        /// any parent object offset if this is a transformed motion state.
        /// </summary>
        public virtual Vector3 WorldTranslation
        {
            get
            {
                return localTranslation;
            }
        }

        /// <summary>
        /// The rotation on this motion state in world coords. This will include 
        /// any parent object offset if this is a transformed motion state.
        /// </summary>
        public virtual Quaternion WorldRotation
        {
            get
            {
                return localRotation;
            }
        }

        /// <summary>
        /// Call this after synching the updated translation and rotation.
        /// </summary>
        internal void positionSynched()
        {
            positionUpdated = false;
        }

        private void motionStateCallback(ref Vector3 trans, ref Quaternion rot)
        {
            localTranslation = trans;
            localRotation = rot;
            positionUpdated = true;
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
        delegate void SetXformCallback(ref Vector3 trans, ref Quaternion rot
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ContactCallback(IntPtr contact, IntPtr sourceBody, IntPtr otherBody, bool isBodyA
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MotionState_Create(SetXformCallback xformCallback, ContactCallback contactStartedCallback, ContactCallback contactEndedCallback, ContactCallback contactContinuesCallback, float maxContactDistance, ref Vector3 initialTrans, ref Quaternion initialRot
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MotionState_Delete(IntPtr instance);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MotionState_setHasContactStartedCallback(IntPtr instance, bool hasCallback);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MotionState_setHasContactEndedCallback(IntPtr instance, bool hasCallback);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MotionState_setHasContactContinuesCallback(IntPtr instance, bool hasCallback);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MotionState_setMaxContactDistance(IntPtr instance, float maxContactDistance);

        [DllImport(BulletInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern float MotionState_getMaxContactDistance(IntPtr instance);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static SetXformCallback xformCallback;
            private static ContactCallback contactStartedCallback;
            private static ContactCallback contactEndedCallback;
            private static ContactCallback contactContinuesCallback;

            static CallbackHandler()
            {
                xformCallback = new SetXformCallback(motionStateCallback);
                contactStartedCallback = new ContactCallback(contactStartedCallbackFunc);
                contactEndedCallback = new ContactCallback(contactEndedCallbackFunc);
                contactContinuesCallback = new ContactCallback(contactContinuesCallbackFunc);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(SetXformCallback))]
            private static void motionStateCallback(ref Vector3 trans, ref Quaternion rot, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as MotionState).motionStateCallback(ref trans, ref rot);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(ContactCallback))]
            private static void contactStartedCallbackFunc(IntPtr contact, IntPtr sourceBody, IntPtr otherBody, bool isBodyA, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as MotionState).contactStartedCallbackFunc(contact, sourceBody, otherBody, isBodyA);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(ContactCallback))]
            private static void contactEndedCallbackFunc(IntPtr contact, IntPtr sourceBody, IntPtr otherBody, bool isBodyA, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as MotionState).contactEndedCallbackFunc(contact, sourceBody, otherBody, isBodyA);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(ContactCallback))]
            private static void contactContinuesCallbackFunc(IntPtr contact, IntPtr sourceBody, IntPtr otherBody, bool isBodyA, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as MotionState).contactContinuesCallbackFunc(contact, sourceBody, otherBody, isBodyA);
            }

            private GCHandle handle;

            public IntPtr create(MotionState obj, float maxContactDistance, ref Vector3 initialTrans, ref Quaternion initialRot)
            {
                handle = GCHandle.Alloc(obj);
                return MotionState_Create(xformCallback, contactStartedCallback, contactEndedCallback, contactContinuesCallback, maxContactDistance, ref initialTrans, ref initialRot, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private SetXformCallback xformCallback;
            private ContactCallback contactStartedCallback;
            private ContactCallback contactEndedCallback;
            private ContactCallback contactContinuesCallback;

            public IntPtr create(MotionState obj, float maxContactDistance, ref Vector3 initialTrans, ref Quaternion initialRot)
            {
                xformCallback = new SetXformCallback(obj.motionStateCallback);
                contactStartedCallback = new ContactCallback(obj.contactStartedCallbackFunc);
                contactEndedCallback = new ContactCallback(obj.contactEndedCallbackFunc);
                contactContinuesCallback = new ContactCallback(obj.contactContinuesCallbackFunc);
                return MotionState_Create(xformCallback, contactStartedCallback, contactEndedCallback, contactContinuesCallback, maxContactDistance, ref initialTrans, ref initialRot);
            }

            public void Dispose()
            {
                xformCallback = null;
                contactStartedCallback = null;
                contactEndedCallback = null;
                contactContinuesCallback = null;
            }
        }
#endif
    }
}
