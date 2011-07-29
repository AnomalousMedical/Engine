using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Engine
{
    public class Position
    {
        public Position(String name, Vector3 translation, Quaternion rotation)
        {
            this.Translation = translation;
            this.Rotation = rotation;
            this.Name = name;
        }

        public Position(String name)
        {
            this.Name = name;
        }

        public Vector3 Translation { get; set; }

        public Quaternion Rotation { get; set; }

        public String Name { get; set; }
    }
}
