using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// This class can track how far a point has moved.
    /// </summary>
    public class TravelTracker
    {
        private int moveGracePixels = 3;

        private IntVector2 travelDistance;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TravelTracker()
            :this(3)
        {

        }

        /// <summary>
        /// Constructor, takes the number of grace pixels to allow.
        /// </summary>
        /// <param name="moveGracePixels">The number of pixels that can be moved in one direction without traveling over the limit.</param>
        public TravelTracker(int moveGracePixels)
        {
            this.moveGracePixels = moveGracePixels;
        }

        /// <summary>
        /// Reset to 0.
        /// </summary>
        public void reset()
        {
            travelDistance = new IntVector2(0, 0);
        }

        /// <summary>
        /// Alert a travel amount.
        /// </summary>
        /// <param name="travelAmount">The IntVector3 to use, only uses x and y, makes it easy to pass in mouse positions.</param>
        public void traveled(IntVector3 travelAmount)
        {
            travelDistance.x += Math.Abs(travelAmount.x);
            travelDistance.y += Math.Abs(travelAmount.y);
        }

        /// <summary>
        /// Alert a travel amount.
        /// </summary>
        /// <param name="x">The x delta.</param>
        /// <param name="y">The y delta.</param>
        public void traveled(int x, int y)
        {
            travelDistance.x += Math.Abs(x);
            travelDistance.y += Math.Abs(y);
        }

        /// <summary>
        /// True if the limit has been traveled.
        /// </summary>
        public bool TraveledOverLimit
        {
            get
            {
                return travelDistance.x > moveGracePixels || travelDistance.y > moveGracePixels;
            }
        }
    }
}
