using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Engine
{
    public class Distance
    {
        const float gs_fTolerance = 1e-05f;

        private Distance()
        {

        }

        public static float SqrDistance(ref Ray3 rkRay, ref Segment3 rkSeg)
        {
            float rayP = 0.0f, segP = 0.0f;
            return SqrDistance(ref rkRay, ref rkSeg, out rayP, out segP);
        }

        public static float SqrDistance(ref Ray3 rkRay, ref Segment3 rkSeg, out float pfRayP, out float pfSegP)
        {
            Vector3 kDiff = rkRay.Origin - rkSeg.Origin;
	        float fA00 = rkRay.Direction.length2();
            float fA01 = -rkRay.Direction.dot(ref rkSeg.Direction);
            float fA11 = rkSeg.Direction.length2();
            float fB0 = kDiff.dot(ref rkRay.Direction);
            float fC = kDiff.length2();
	        float fDet = System.Math.Abs(fA00*fA11-fA01*fA01);
            float fB1, fS, fT, fSqrDist, fTmp;

            if ( fDet >= gs_fTolerance )
            {
                // ray and segment are not parallel
                fB1 = -kDiff.dot(ref rkSeg.Direction);
                fS = fA01*fB1-fA11*fB0;
                fT = fA01*fB0-fA00*fB1;

                if ( fS >= 0.0 )
                {
                    if ( fT >= 0.0 )
                    {
                        if ( fT <= fDet )  // region 0
                        {
                            // minimum at interior points of ray and segment
                            float fInvDet = 1.0f/fDet;
                            fS *= fInvDet;
                            fT *= fInvDet;
                            fSqrDist = fS*(fA00*fS+fA01*fT+2.0f*fB0) +
                                fT*(fA01*fS+fA11*fT+2.0f*fB1)+fC;
                        }
                        else  // region 1
                        {
                            fT = 1.0f;
                            if ( fB0 >= -fA01 )
                            {
                                fS = 0.0f;
                                fSqrDist = fA11+2.0f*fB1+fC;
                            }
                            else
                            {
                                fTmp = fA01 + fB0;
                                fS = -fTmp/fA00;
                                fSqrDist = fTmp*fS+fA11+2.0f*fB1+fC;
                            }
                        }
                    }
                    else  // region 5
                    {
                        fT = 0.0f;
                        if ( fB0 >= 0.0 )
                        {
                            fS = 0.0f;
                            fSqrDist = fC;
                        }
                        else
                        {
                            fS = -fB0/fA00;
                            fSqrDist = fB0*fS+fC;
                        }
                    }
                }
                else
                {
                    if ( fT <= 0.0 )  // region 4
                    {
                        if ( fB0 < 0.0 )
                        {
                            fS = -fB0/fA00;
                            fT = 0.0f;
                            fSqrDist = fB0*fS+fC;
                        }
                        else
                        {
                            fS = 0.0f;
                            if ( fB1 >= 0.0 )
                            {
                                fT = 0.0f;
                                fSqrDist = fC;
                            }
                            else if ( -fB1 >= fA11 )
                            {
                                fT = 1.0f;
                                fSqrDist = fA11+2.0f*fB1+fC;
                            }
                            else
                            {
                                fT = -fB1/fA11;
                                fSqrDist = fB1*fT+fC;
                            }
                        }
                    }
                    else if ( fT <= fDet )  // region 3
                    {
                        fS = 0.0f;
                        if ( fB1 >= 0.0 )
                        {
                            fT = 0.0f;
                            fSqrDist = fC;
                        }
                        else if ( -fB1 >= fA11 )
                        {
                            fT = 1.0f;
                            fSqrDist = fA11+2.0f*fB1+fC;
                        }
                        else
                        {
                            fT = -fB1/fA11;
                            fSqrDist = fB1*fT+fC;
                        }
                    }
                    else  // region 2
                    {
                        fTmp = fA01+fB0;
                        if ( fTmp < 0.0 )
                        {
                            fS = -fTmp/fA00;
                            fT = 1.0f;
                            fSqrDist = fTmp*fS+fA11+2.0f*fB1+fC;
                        }
                        else
                        {
                            fS = 0.0f;
                            if ( fB1 >= 0.0 )
                            {
                                fT = 0.0f;
                                fSqrDist = fC;
                            }
                            else if ( -fB1 >= fA11 )
                            {
                                fT = 1.0f;
                                fSqrDist = fA11+2*fB1+fC;
                            }
                            else
                            {
                                fT = -fB1/fA11;
                                fSqrDist = fB1*fT+fC;
                            }
                        }
                    }
                }
            }
            else
            {
                // ray and segment are parallel
                if ( fA01 > 0.0 )
                {
                    // opposite direction vectors
                    fT = 0.0f;
                    if ( fB0 >= 0.0 )
                    {
                        fS = 0.0f;
                        fSqrDist = fC;
                    }
                    else
                    {
                        fS = -fB0/fA00;
                        fSqrDist = fB0*fS+fC;
                    }
                }
                else
                {
                    // same direction vectors
                    fB1 = -kDiff.dot(ref rkSeg.Direction);
                    fT = 1.0f;
                    fTmp = fA01+fB0;
                    if ( fTmp >= 0.0 )
                    {
                        fS = 0.0f;
                        fSqrDist = fA11+2.0f*fB1+fC;
                    }
                    else
                    {
                        fS = -fTmp/fA00;
                        fSqrDist = fTmp*fS+fA11+2.0f*fB1+fC;
                    }
                }
            }

            pfRayP = fS;
	        pfSegP = fT;

	        return System.Math.Abs(fSqrDist);
        }

        public static float SqrDistance(ref Vector3 rkPoint, ref Ray3 rkRay)
        {
            Vector3 kDiff = rkPoint - rkRay.Origin;
            float fT = kDiff.dot(ref rkRay.Direction);

            if (fT <= 0.0)
            {
                fT = 0.0f;
            }
            else
            {
                fT /= rkRay.Direction.length2();
                kDiff -= fT * rkRay.Direction;
            }

            return kDiff.length2();
        }

    }
}
