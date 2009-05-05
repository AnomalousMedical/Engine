using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Editor
{
    public interface SelectableObject
    {
        /// <summary>
        /// Edit the translation of the SimObject.
        /// </summary>
        /// <param name="localTrans">The new translation to set.</param>
        void editTranslation(ref Vector3 localTrans);

        void editPosition(ref Vector3 localTrans, ref Quaternion localRot);

        void editRotation(ref Quaternion newRot);

        Quaternion getRotation();

        Vector3 getTranslation();
    }
}
