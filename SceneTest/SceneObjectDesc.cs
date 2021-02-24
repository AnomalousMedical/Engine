using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class SceneObjectDesc
    {
        public Vector3 Translation { get; set; } = Vector3.Zero;

        public Quaternion Orientation { get; set; } = Quaternion.Identity;

        public Vector3 Scale { get; set; } = Vector3.ScaleIdentity;
    }
}
