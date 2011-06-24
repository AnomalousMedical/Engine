using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Ellipse
    {
        private Radian theta;
        private float sinTheta;
        private float cosTheta;

        public Ellipse()
        {
            X = 0;
            Y = 0;
            MajorAxis = 1.0f;
            MinorAxis = 1.0f;
            Theta = 0;
        }

        public Vector2 getPoint(Radian t)
        {
            float sinT = (float)Math.Sin(t);
            float cosT = (float)Math.Cos(t);
            return new Vector2
                (
                    X + (MajorAxis * cosT * cosTheta - MinorAxis * sinT * sinTheta),
                    Y + (MajorAxis * cosT * sinTheta + MinorAxis * sinT * cosTheta)
                );
        }
        
        public float X { get; set; }

        public float Y { get; set; }

        public float MajorAxis { get; set; }

        public float MinorAxis { get; set; }

        public Radian Theta
        {
            get
            {
                return theta;
            }
            set
            {
                theta = value;
                sinTheta = (float)Math.Sin(-theta);
                cosTheta = (float)Math.Cos(-theta);
            }
        }
    }
}
