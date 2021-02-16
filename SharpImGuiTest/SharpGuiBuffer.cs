using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    public class SharpGuiBuffer
    {
        private readonly OSWindow osWindow;
        public const int MaxNumberOfQuads = 1000;
        SharpImGuiVertex[] verts;
        private int currentQuad = 0;

        public SharpGuiBuffer(GraphicsEngine graphicsEngine, OSWindow osWindow)
        {
            verts = new SharpImGuiVertex[MaxNumberOfQuads * 4];

            this.osWindow = osWindow;
        }

        public void Begin()
        {
            currentQuad = 0;
            NumIndices = 0;
        }

        public void DrawQuad(int x, int y, int width, int height, Color color)
        {
            float left = x / (float)osWindow.WindowWidth * 2.0f - 1.0f;
            float right = (x + width) / (float)osWindow.WindowWidth * 2.0f - 1.0f;
            float top = y / (float)osWindow.WindowHeight * -2.0f + 1.0f;
            float bottom = (y + height) / (float)osWindow.WindowHeight * -2.0f + 1.0f;

            verts[currentQuad].pos = new Vector3(left, top, 0);
            verts[currentQuad + 1].pos = new Vector3(right, top, 0);
            verts[currentQuad + 2].pos = new Vector3(right, bottom, 0);
            verts[currentQuad + 3].pos = new Vector3(left, bottom, 0);

            verts[currentQuad].color = color;
            verts[currentQuad + 1].color = color;
            verts[currentQuad + 2].color = color;
            verts[currentQuad + 3].color = color;

            currentQuad += 4;
            NumIndices += 6;
        }

        public uint NumIndices { get; private set; }

        internal SharpImGuiVertex[] Verts => verts;
    }
}
