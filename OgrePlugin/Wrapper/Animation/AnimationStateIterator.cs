using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class AnimationStateIterator : IEnumerator<AnimationState>, IEnumerable<AnimationState>
    {
        private IntPtr animationStateIterator;
        private AnimationStateSet parentSet;
        private bool foundFirst;
        
        internal AnimationStateIterator(AnimationStateSet parentSet, IntPtr animationStateIterator)
        {
            this.animationStateIterator = animationStateIterator;
            this.parentSet = parentSet;
        }

        public void Dispose()
        {
            parentSet.destroyIterator(animationStateIterator);
        }

        public AnimationState Current
        {
            get
            {
                return parentSet.getStateFromPointer(AnimationStateIterator_peekNextValue(animationStateIterator));
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return parentSet.getStateFromPointer(AnimationStateIterator_peekNextValue(animationStateIterator));
            }
        }

        public bool MoveNext()
        {
            if (foundFirst)
            {
                AnimationStateIterator_moveNext(animationStateIterator);
            }
            else
            {
                foundFirst = true;
            }
            return AnimationStateIterator_hasMoreElements(animationStateIterator);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<AnimationState> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        #region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr AnimationStateIterator_peekNextValue(IntPtr iter);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AnimationStateIterator_moveNext(IntPtr iter);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool AnimationStateIterator_hasMoreElements(IntPtr iter);

        #endregion
    }
}
