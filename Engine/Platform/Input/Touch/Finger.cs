using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Engine.Platform
{
    public class Finger : PooledObject
    {
        private int id;
        private float xNrmlPos;
        private float yNrmlPos;
        private float lastNrmlXPos;
        private float lastNrmlYPos;
        private float deltaNrmlX;
        private float deltaNrmlY;

        public Finger()
        {

        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public float NrmlX
        {
            get
            {
                return xNrmlPos;
            }
        }

        public float NrmlY
        {
            get
            {
                return yNrmlPos;
            }
        }

        public float NrmlLastX
        {
            get
            {
                return lastNrmlXPos;
            }
        }

        public float NrmlLastY
        {
            get
            {
                return lastNrmlYPos;
            }
        }

        public float NrmlDeltaX
        {
            get
            {
                return deltaNrmlX;
            }
        }

        public float NrmlDeltaY
        {
            get
            {
                return deltaNrmlY;
            }
        }

        public int PixelX { get; private set; }

        public int PixelY { get; private set; }

        public int PixelLastX { get; private set; }

        public int PixelLastY { get; private set; }
        
        public int PixelDeltaX { get; private set; }
        
        public int PixelDeltaY { get; private set; }

        internal void setCurrentPosition(float xNrmlPos, float yNrmlPos, int xPixelPos, int yPixelPos)
        {
            this.xNrmlPos = xNrmlPos;
            this.yNrmlPos = yNrmlPos;
            deltaNrmlX = xNrmlPos - lastNrmlXPos;
            deltaNrmlY = yNrmlPos - lastNrmlYPos;

            PixelX = xPixelPos;
            PixelY = yPixelPos;
            PixelDeltaX = xPixelPos - PixelLastX;
            PixelDeltaY = yPixelPos - PixelLastY;
        }

        internal void captureCurrentPositionAsLast()
        {
            lastNrmlXPos = xNrmlPos;
            lastNrmlYPos = yNrmlPos;
            deltaNrmlX = 0;
            deltaNrmlY = 0;

            PixelLastX = PixelX;
            PixelLastY = PixelY;
            PixelDeltaX = 0;
            PixelDeltaY = 0;
        }

        internal void finished()
        {
            this.returnToPool();
        }

        internal void setInfoOutOfPool(int id, float xPos, float yPos, int xPixelPos, int yPixelPos)
        {
            this.id = id;
            this.xNrmlPos = xPos;
            this.lastNrmlXPos = xPos;
            this.yNrmlPos = yPos;
            this.lastNrmlYPos = yPos;

            this.PixelX = xPixelPos;
            this.PixelLastX = xPixelPos;
            this.PixelY = yPixelPos;
            this.PixelLastY = yPixelPos;
        }

        protected override void reset()
        {
            id = -1;
        }
    }
}
