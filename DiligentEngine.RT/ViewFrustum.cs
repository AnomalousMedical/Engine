using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    class Plane3D
    {
        public Vector3 Normal;
        public float Distance = 0; //Distance from the coordinate system origin to the plane along the normal direction
    }

    class ViewFrustum
    {
        public enum PLANE_IDX
        {
            LEFT_PLANE_IDX = 0,
            RIGHT_PLANE_IDX = 1,
            BOTTOM_PLANE_IDX = 2,
            TOP_PLANE_IDX = 3,
            NEAR_PLANE_IDX = 4,
            FAR_PLANE_IDX = 5,
            NUM_PLANES = 6
        };

        public Plane3D LeftPlane = new Plane3D();
        public Plane3D RightPlane = new Plane3D();
        public Plane3D BottomPlane = new Plane3D();
        public Plane3D TopPlane = new Plane3D();
        public Plane3D NearPlane = new Plane3D();
        public Plane3D FarPlane = new Plane3D();

        public Plane3D GetPlane(PLANE_IDX Idx)
        {
            switch (Idx)
            {
                case PLANE_IDX.LEFT_PLANE_IDX:
                    return LeftPlane;
                case PLANE_IDX.RIGHT_PLANE_IDX:
                    return RightPlane;
                case PLANE_IDX.BOTTOM_PLANE_IDX:
                    return BottomPlane;
                case PLANE_IDX.TOP_PLANE_IDX:
                    return TopPlane;
                case PLANE_IDX.NEAR_PLANE_IDX:
                    return NearPlane;
                case PLANE_IDX.FAR_PLANE_IDX:
                    return FarPlane;
            }

            throw new InvalidOperationException($"Cannot find plane type '{Idx}'");
        }
    }
}
