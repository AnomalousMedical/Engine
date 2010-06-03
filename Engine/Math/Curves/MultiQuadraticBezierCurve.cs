using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class MultiQuadraticBezierCurve
    {
        private List<Vector3> controlPoints = new List<Vector3>();

        public Vector3 computePoint(float percent)
        {
            Vector3 start = controlPoints[0];
            Vector3 end = controlPoints[1];
            Vector3 middle = (controlPoints[2] - controlPoints[1]) * -1 + controlPoints[1];

            float minusPercent = 1.0f - percent;
            return new Vector3(start.x * minusPercent * minusPercent + middle.x * 2 * percent * minusPercent + end.x * percent * percent,
                               start.y * minusPercent * minusPercent + middle.y * 2 * percent * minusPercent + end.y * percent * percent,
                               start.z * minusPercent * minusPercent + middle.z * 2 * percent * minusPercent + end.z * percent * percent);
        }

        public void addPoint(Vector3 vector)
        {
            controlPoints.Add(vector);
        }
    }
}
