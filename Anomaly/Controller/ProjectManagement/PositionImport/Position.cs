using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomaly
{
    public class Position
    {
        public Position(Vector3 translation, Quaternion rotation)
        {
            this.Translation = translation;
            this.Rotation = rotation;
        }

        public Position()
        {

        }

        public Vector3 Translation { get; set; }

        public Quaternion Rotation { get; set; }
    }
}
